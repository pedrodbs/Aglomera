// ------------------------------------------
// <copyright file="Program.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: NumericClustering
//    Last updated: 2017/04/06
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Agnes;
using Agnes.D3;
using Agnes.Linkage;
using Newtonsoft.Json;

namespace NumericClustering
{
    internal class Program
    {
        #region Static Fields & Constants

        //private const string DATASET_FILE = "../../../../datasets/test.csv";
        private const string DATASET_FILE = "../../../../datasets/seeds.csv";

        #endregion

        #region Private & Protected Methods

        private static void Main(string[] args)
        {
            var dataSetFile = args.Length > 0 ? args[0] : DATASET_FILE;
            var parser = new CsvParser();
            var instances = parser.Load(Path.GetFullPath(dataSetFile));

            var metric = new DataPoint(null, null);
            var linkages = new Dictionary<ILinkageCriterion<DataPoint>, string>
                           {
                               {new AverageLinkageClustering<DataPoint>(metric), "average"},
                               {new CompleteLinkageClustering<DataPoint>(metric), "complete"},
                               {new SingleLinkageClustering<DataPoint>(metric), "single"},
                               {new MinimumEnergyClustering<DataPoint>(metric), "min-energy"}
                           };
            foreach (var linkage in linkages)
                PrintClusters(instances, linkage.Key, linkage.Value);

            Console.WriteLine("\nDone!");
            Console.ReadKey();
        }

        private static void PrintClusters(ISet<DataPoint> instances, ILinkageCriterion<DataPoint> linkage, string name)
        {
            var clusteringAlg = new ClusteringAlgorithm<DataPoint>(linkage);
            var clustering = clusteringAlg.GetClustering(instances);

            Console.WriteLine("_____________________________________________");
            Console.WriteLine($"Clusters found for linkage criterion '{name}':");
            foreach (var clusterSet in clustering)
                Console.WriteLine($" - {clusterSet}");

            clustering.SaveD3DendrogramFile(Path.GetFullPath($"{name}.json"), formatting: Formatting.Indented);
        }

        #endregion
    }
}