// ------------------------------------------
// <copyright file="Program.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: ClusteringEvaluation
//    Last updated: 2017/07/26
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
using Agnes.Evaluation;
using Agnes.Linkage;

namespace ClusteringEvaluation
{
    internal class Program
    {
        #region Static Fields & Constants

        private const string DATASET_FILE = "../../../../datasets/seeds.csv";
        private const uint NUM_CLUSTERS = 3;

        #endregion

        #region Private & Protected Methods

        private static void Main(string[] args)
        {
            // loads points from csv file
            var dataSetFile = args.Length > 0 ? args[0] : DATASET_FILE;
            var filePath = Path.GetFullPath(dataSetFile);

            Console.WriteLine($"Loading data-points from {filePath}...");

            var parser = new CsvParser();
            var dataPoints = parser.Load(filePath);

            Console.WriteLine($"Clustering {dataPoints.Count} data-points...");

            // executes agglomerative clustering with given linkage criterion
            var metric = new DataPoint(null, null);
            var linkage = new AverageLinkageClustering<DataPoint>(metric);
            var clusteringAlg = new ClusteringAlgorithm<DataPoint>(linkage);
            var clustering = clusteringAlg.GetClustering(dataPoints);

            // gets cluster set according to predefined number of clusters
            var clusterSet = clustering.First(cs => cs.Count == NUM_CLUSTERS);

            // gets classes for each data-point (first character of the ID in the seeds dataset)
            var pointClasses = dataPoints.ToDictionary(dataPoint => dataPoint, dataPoint => dataPoint.ID[0]);

            Console.WriteLine("Evaluating clustering...");

            // evaluates the clustering according to different criteria
            var evaluations =
                new Dictionary<string, double>
                {
                    {"purity", new Purity<DataPoint, char>().Evaluate(clusterSet, pointClasses)},
                    {"nmi", new NormalizedMutualInformation<DataPoint, char>().Evaluate(clusterSet, pointClasses)},
                    {"accuracy", new RandIndex<DataPoint, char>().Evaluate(clusterSet, pointClasses)},
                    {"precision", new Precision<DataPoint, char>().Evaluate(clusterSet, pointClasses)},
                    {"recall", new Recall<DataPoint, char>().Evaluate(clusterSet, pointClasses)},
                    {"f1Measure", new FMeasure<DataPoint, char>(1).Evaluate(clusterSet, pointClasses)},
                    {"f2Measure", new FMeasure<DataPoint, char>(2).Evaluate(clusterSet, pointClasses)},
                    {"f05Measure", new FMeasure<DataPoint, char>(0.5).Evaluate(clusterSet, pointClasses)}
                };
            foreach (var evaluation in evaluations)
                Console.WriteLine($" - {evaluation.Key}: {evaluation.Value:0.000}");

            Console.WriteLine("\nDone!");
            Console.ReadKey();
        }

        #endregion
    }
}