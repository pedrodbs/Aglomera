// ------------------------------------------
// <copyright file="Program.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: InternalClusteringEvaluation
//    Last updated: 2018/01/20
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Grupo;
using Grupo.D3;
using Grupo.Evaluation.Internal;
using Grupo.Linkage;
using ExamplesUtil;

namespace InternalClusteringEvaluation
{
    internal class Program
    {
        #region Static Fields & Constants

        private const string DATASET_FILE = "../../../../datasets/zoo.csv";
        private const string RESULTS_PATH = "./results";

        #endregion

        #region Private & Protected Methods

        private static void GetBestPartition(
            ClusteringResult<DataPoint> clustering,
            IInternalEvaluationCriterion<DataPoint> criterion, string criterionName)
        {
            // gets coeffs for all cluster-sets
            var evals = clustering.EvaluateClustering(criterion);

            // saves cluster-sets indexes to CSV file
            SaveToCsv(evals, Path.GetFullPath(Path.Combine(RESULTS_PATH, $"{criterionName}.csv")), criterionName);

            // gets max coeff
            var maxEval = new ClusterSetEvaluation<DataPoint>(null, double.MinValue);
            foreach (var eval in evals)
                if (eval.EvaluationValue > maxEval.EvaluationValue)
                    maxEval = eval;

            // prints cluster set info
            Console.WriteLine("======================================");
            Console.WriteLine($"Max {criterionName}: {maxEval.EvaluationValue:0.00}");
            if (maxEval.ClusterSet == null) return;
            Console.WriteLine(
                $"Clusters at distance: {maxEval.ClusterSet.Dissimilarity:0.00} ({maxEval.ClusterSet.Count})");
            foreach (var cluster in maxEval.ClusterSet)
                Console.WriteLine($" - {cluster}");
        }

        private static void Main(string[] args)
        {
            var globalPerf = new PerformanceMeasure();
            globalPerf.Start();

            // loads points from csv file
            var dataSetFile = args.Length > 0 ? args[0] : DATASET_FILE;
            var filePath = Path.GetFullPath(dataSetFile);

            Console.WriteLine($"Loading data-points from {filePath}...");
            var parser = new CsvParser();
            var dataPoints = parser.Load(filePath);

            Console.WriteLine($"Clustering {dataPoints.Count} data-points...");

            // performs hierarchical clustering
            var clusterPerf = new PerformanceMeasure();
            clusterPerf.Start();
            var metric = new DataPoint(); // Euclidean distance
            var linkage = new AverageLinkage<DataPoint>(metric);
            var clusteringAlg = new ClusteringAlgorithm<DataPoint>(linkage);
            var clustering = clusteringAlg.GetClustering(dataPoints);
            clusterPerf.Stop();

            Directory.CreateDirectory(Path.GetFullPath(RESULTS_PATH));
            clustering.SaveD3DendrogramFile(Path.GetFullPath(Path.Combine(RESULTS_PATH, "dendrogram.json")));
            clustering.SaveToCsv(Path.GetFullPath(Path.Combine(RESULTS_PATH, "clustering.csv")));
            Console.WriteLine($"Finished clustering: {clusterPerf}");

            // evaluates the clustering according to several criteria
            //CentroidFunction<DataPoint> centroidFunc = DataPoint.GetMedoid;
            CentroidFunction<DataPoint> centroidFunc = DataPoint.GetCentroid;
            var criteria =
                new Dictionary<string, IInternalEvaluationCriterion<DataPoint>>
                {
                    {"Silhouette coefficient", new SilhouetteCoefficient<DataPoint>(metric)},
                    {"Dunn index", new DunnIndex<DataPoint>(metric)},
                    {"Davies-Bouldin index", new DaviesBouldinIndex<DataPoint>(metric, centroidFunc)},
                    {"Calinski-Harabasz index", new CalinskiHarabaszIndex<DataPoint>(metric, centroidFunc)},
                    {"Modified Gamma statistic", new ModifiedGammaStatistic<DataPoint>(metric, centroidFunc)},
                    {"Xie-Beni index", new XieBeniIndex<DataPoint>(metric, centroidFunc)},
                    {"Within-Between ratio", new WithinBetweenRatio<DataPoint>(metric, centroidFunc)},
                    {"I-index", new IIndex<DataPoint>(metric, centroidFunc)},
                    {"Xu index", new XuIndex<DataPoint>(metric, centroidFunc)}

                    //{"RMSSD", new RootMeanSquareStdDev<DataPoint>(metric, centroidFunc)},
                    //{"R-squared", new RSquared<DataPoint>(metric, centroidFunc)},
                };

            foreach (var criterion in criteria)
                GetBestPartition(clustering, criterion.Value, criterion.Key);

            globalPerf.Stop();
            Console.WriteLine($"\nFinished: {globalPerf}");
            Console.ReadKey();
        }

        private static void SaveToCsv(
            IEnumerable<ClusterSetEvaluation<DataPoint>> evals, string filePath, string criterionName,
            char sepChar = ',')
        {
            using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // writes header
                sw.WriteLine($"Num. clusters{sepChar}{criterionName}{sepChar}Cluster-set");

                // writes all key-value-pairs, one per line
                foreach (var eval in evals)
                    sw.WriteLine($"{eval.ClusterSet.Count}{sepChar}{eval.EvaluationValue}{sepChar}" +
                                 $"{eval.ClusterSet.ToString(false).Replace(sepChar, ';')}");
                sw.Close();
            }
        }

        #endregion
    }
}