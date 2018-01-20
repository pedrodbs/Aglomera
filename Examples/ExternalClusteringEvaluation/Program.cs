// ------------------------------------------
// <copyright file="Program.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: ExternalClusteringEvaluation
//    Last updated: 2018/01/05
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Agnes;
using Agnes.Evaluation.External;
using Agnes.Linkage;
using ExamplesUtil;

namespace ExternalClusteringEvaluation
{
    internal class Program
    {
        #region Static Fields & Constants

        private const string DATASET_FILE = "../../../../datasets/iris.csv";
        private const uint NUM_CLUSTERS = 3; // for seeds and iris data-sets

        #endregion

        #region Private & Protected Methods

        private static void EvaluateClustering(
            ISet<DataPoint> dataPoints, ILinkageCriterion<DataPoint> linkage, string linkageName, uint numClusters)
        {
            var clusteringAlg = new ClusteringAlgorithm<DataPoint>(linkage);
            var clustering = clusteringAlg.GetClustering(dataPoints);

            // gets cluster set according to predefined number of clusters
            var clusterSet = clustering.First(cs => cs.Count == numClusters);

            // gets classes for each data-point (first character of the ID in the seeds dataset)
            var pointClasses = dataPoints.ToDictionary(dataPoint => dataPoint, dataPoint => dataPoint.ID[0]);

            Console.WriteLine("=============================================");
            Console.WriteLine($"Evaluating {linkageName} clustering using Euclidean distance...");

            // evaluates the clustering according to different criteria
            var evaluations =
                new Dictionary<string, double>
                {
                    {"Purity", new Purity<DataPoint, char>().Evaluate(clusterSet, pointClasses)},
                    {"NMI", new NormalizedMutualInformation<DataPoint, char>().Evaluate(clusterSet, pointClasses)},
                    {"Accuracy", new RandIndex<DataPoint, char>().Evaluate(clusterSet, pointClasses)},
                    {"Precision", new Precision<DataPoint, char>().Evaluate(clusterSet, pointClasses)},
                    {"Recall", new Recall<DataPoint, char>().Evaluate(clusterSet, pointClasses)},
                    {"F1Measure", new FMeasure<DataPoint, char>(1).Evaluate(clusterSet, pointClasses)},
                    {"F2Measure", new FMeasure<DataPoint, char>(2).Evaluate(clusterSet, pointClasses)},
                    {"F05Measure", new FMeasure<DataPoint, char>(0.5).Evaluate(clusterSet, pointClasses)}
                };
            foreach (var evaluation in evaluations)
                Console.WriteLine($" - {evaluation.Key}: {evaluation.Value:0.000}");
        }

        private static void Main(string[] args)
        {
            // loads points from csv file
            var dataSetFile = args.Length > 0 ? args[0] : DATASET_FILE;
            var filePath = Path.GetFullPath(dataSetFile);

            Console.WriteLine($"Loading data-points from {filePath}...");

            var parser = new CsvParser();
            var dataPoints = parser.Load(filePath);

            Console.WriteLine($"Clustering {dataPoints.Count} data-points...");

            var metric = new DataPoint(null, null);
            var linkages = new Dictionary<ILinkageCriterion<DataPoint>, string>
                           {
                               {new AverageLinkage<DataPoint>(metric), "average"},
                               {new CompleteLinkage<DataPoint>(metric), "complete"},
                               {new SingleLinkage<DataPoint>(metric), "single"},
                               {new MinimumEnergyLinkage<DataPoint>(metric), "min-energy"},
                               {new CentroidLinkage<DataPoint>(metric, DataPoint.GetCentroid), "centroid"},
                               {new WardsMinimumVarianceLinkage<DataPoint>(metric, DataPoint.GetCentroid), "ward"}
                           };

            // executes agglomerative clustering with several linkage criteria
            var numClusters = NUM_CLUSTERS;
            if (args.Length > 1) uint.TryParse(args[1], out numClusters);
            foreach (var linkage in linkages)
                EvaluateClustering(dataPoints, linkage.Key, linkage.Value, numClusters);

            Console.WriteLine("\nDone!");
            Console.ReadKey();
        }

        #endregion
    }
}