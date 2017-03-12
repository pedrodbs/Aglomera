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

namespace SmallExample
{
    internal class Program
    {
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
            var instances = new HashSet<DataPoint>
                            {
                                new DataPoint("1", new[] {2.0d, 2.0d}),
                                new DataPoint("2", new[] {5.5d, 4.0d}),
                                new DataPoint("3", new[] {5.0d, 5.0d}),
                                new DataPoint("4", new[] {1.5d, 2.5d}),
                                new DataPoint("5", new[] {1.0d, 1.0d}),
                                new DataPoint("6", new[] {7.0d, 5.0d}),
                                new DataPoint("7", new[] {5.75d, 6.5d})
                            };

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
                Console.WriteLine($" - {cluster.Key:0.000}\t{EnumerableToString(cluster.Value)}");

            clusters.ToList().SaveD3File(Path.GetFullPath($"{name}.json"));
        }

        #endregion
    }
}