// ------------------------------------------
// <copyright file="Program.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: SmallExample
//    Last updated: 2017/03/10
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Agnes;
using Agnes.D3;
using Agnes.Linkage;

namespace NumericClustering
{
    internal class Program
    {
        private const string DATASET_FILE = "test.csv";

        #region Private & Protected Methods

        private static string EnumerableToString<TInstance>(IEnumerable<TInstance> collection)
            where TInstance : IEquatable<TInstance>
        {
            var sb = new StringBuilder("{");
            var list = collection as IList<TInstance> ?? collection.ToList();
            foreach (var item in list)
                sb.Append($"{item}, ");
            if (list.Count > 0) sb.Remove(sb.Length - 2, 2);
            sb.Append("}");
            return sb.ToString();
        }

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
            var clusteringAlg = new ClusteringAlgorithm<DataPoint>(instances, linkage);
            var clusters = clusteringAlg.GetClusters();

            Console.WriteLine("_____________________________________________");
            Console.WriteLine($"Clusters found for linkage criterion '{name}':");
            foreach (var cluster in clusters)
                Console.WriteLine($" - {cluster.Value:0.000}\t{EnumerableToString(cluster.Key)}");

            clusters.SaveD3File(Path.GetFullPath($"{name}.json"));
        }

        #endregion
    }
}