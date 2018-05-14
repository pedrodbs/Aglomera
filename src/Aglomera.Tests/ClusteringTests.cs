// ------------------------------------------
// <copyright file="ClusteringTests.cs" company="Pedro Sequeira">
// 
//     Copyright (c) 2018 Pedro Sequeira
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// </copyright>
// <summary>
//    Project: Aglomera.Tests
//    Last updated: 04/27/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aglomera.Linkage;
using ExamplesUtil;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aglomera.Tests
{
    [TestClass]
    public class ClusteringTests
    {
        #region Static Fields & Constants

        private static readonly ISet<DataPoint> DataPoints = new HashSet<DataPoint>(
            new[]
            {
                new DataPoint("A1", new[] {2.0d, 2.0d}),
                new DataPoint("B2", new[] {5.5d, 4.0d}),
                new DataPoint("B3", new[] {5.0d, 5.0d}),
                new DataPoint("A4", new[] {1.5d, 2.5d}),
                new DataPoint("A5", new[] {1.0d, 1.0d}),
                new DataPoint("B6", new[] {7.0d, 5.0d}),
                new DataPoint("B7", new[] {5.75d, 6.5d})
            });

        private static readonly IDissimilarityMetric<DataPoint> DistanceMetric = new DataPoint();

        private static readonly List<ILinkageCriterion<DataPoint>> Linkages =
            new List<ILinkageCriterion<DataPoint>>
            {
                new CompleteLinkage<DataPoint>(DistanceMetric),
                new AverageLinkage<DataPoint>(DistanceMetric),
                new CentroidLinkage<DataPoint>(DistanceMetric, DataPoint.GetCentroid),
                new MinimumEnergyLinkage<DataPoint>(DistanceMetric),
                new SingleLinkage<DataPoint>(DistanceMetric),
                new WardsMinimumVarianceLinkage<DataPoint>(new DataPoint(), DataPoint.GetCentroid)
            };

        #endregion

        #region Public Methods

        [TestMethod]
        public void ClusteringSizeTest()
        {
            foreach (var linkage in Linkages)
            {
                Console.WriteLine("________________________");
                var clusteringResult = new ClusteringAlgorithm<DataPoint>(linkage).GetClustering(DataPoints);
                Console.WriteLine(clusteringResult.Count);
                Assert.AreEqual(clusteringResult.Count, DataPoints.Count,
                    "Clustering result should have as many cluster-sets as there are clusters.");
            }
        }

        [TestMethod]
        public void ClusterSetsSizeTest()
        {
            foreach (var linkage in Linkages)
            {
                Console.WriteLine("________________________");
                var clusteringResult = new ClusteringAlgorithm<DataPoint>(linkage).GetClustering(DataPoints);
                for (var i = 0; i < clusteringResult.Count; i++)
                {
                    var clusterSet = clusteringResult[i];
                    Console.WriteLine(clusterSet);
                    Assert.AreEqual(clusterSet.Count, DataPoints.Count - i,
                        $"Cluster-set size at iteration {i} should be inverse to the number of items.");

                    if (i > 0)
                        Assert.IsTrue(clusterSet.Dissimilarity > 0d,
                            "Dissimilarity of the single cluster-set should be > 0.");

                    foreach (var dataPoint in DataPoints)
                        Assert.IsTrue(clusterSet.Any(cluster => cluster.Contains(dataPoint)),
                            $"There should be a cluster in {clusterSet} that contains data-point {dataPoint}");
                }
            }
        }

        [TestMethod]
        public void InitialClusterTest()
        {
            foreach (var linkage in Linkages)
            {
                Console.WriteLine("________________________");
                var clusteringResult = new ClusteringAlgorithm<DataPoint>(linkage).GetClustering(DataPoints);
                var firstClusterSet = clusteringResult[0];
                Console.WriteLine(firstClusterSet);
                Assert.AreEqual(firstClusterSet.Count, DataPoints.Count,
                    "First cluster-set size should be the same as the number of items.");
                Assert.AreEqual(firstClusterSet.Dissimilarity, 0d, double.Epsilon,
                    "First cluster-set dissimilarity should be 0.");
            }
        }

        [TestMethod]
        public void InitializedClusteringTest()
        {
            foreach (var linkage in Linkages)
            {
                Console.WriteLine("________________________");
                var completeClusteringResult = new ClusteringAlgorithm<DataPoint>(linkage).GetClustering(DataPoints);
                for (var i = 0; i < completeClusteringResult.Count; i++)
                {
                    var clusterSet = completeClusteringResult[i];
                    var clusteringResult = new ClusteringAlgorithm<DataPoint>(linkage).GetClustering(clusterSet);
                    Assert.AreEqual(clusteringResult.Count, DataPoints.Count - i);

                    for (var j = 0; j < clusteringResult.Count; j++)
                    {
                        var clusterSet1 = completeClusteringResult[i + j];
                        var clusterSet2 = clusteringResult[j];
                        Console.WriteLine(clusterSet1);
                        Console.WriteLine(clusterSet2);
                        Assert.AreEqual(clusterSet1.Dissimilarity, clusterSet2.Dissimilarity, double.Epsilon);
                        Assert.IsTrue(clusterSet1.SequenceEqual(clusterSet2),
                            $"{clusterSet1} should be equal to {clusterSet2}");
                    }
                }
            }
        }

        [TestMethod]
        public void SaveFileTest()
        {
            for (var i = 0; i < Linkages.Count; i++)
            {
                var linkage = Linkages[i];
                Console.WriteLine("________________________");
                var clusteringResult = new ClusteringAlgorithm<DataPoint>(linkage).GetClustering(DataPoints);
                var filePath = Path.GetFullPath($"linkage-{i}.csv");
                Console.WriteLine(filePath);
                clusteringResult.SaveToCsv(filePath);
                Assert.IsTrue(File.Exists(filePath), $"CSV file should exist in {filePath}");
                Assert.AreNotEqual(0, new FileInfo(filePath).Length, "CSV file size should be > 0 bytes.");

#if !DEBUG
                File.Delete(filePath);
#endif
            }
        }

        [TestMethod]
        public void SingleClusterTest()
        {
            foreach (var linkage in Linkages)
            {
                Console.WriteLine("________________________");
                var clusteringResult = new ClusteringAlgorithm<DataPoint>(linkage).GetClustering(DataPoints);
                var singleClusterSet = clusteringResult[DataPoints.Count - 1];
                Console.WriteLine(singleClusterSet);
                Assert.AreEqual(singleClusterSet.Count, 1, "Single cluster-set should only have a cluster.");

                var cluster = singleClusterSet.First();
                Assert.AreEqual(cluster.Count, DataPoints.Count,
                    "Single cluster-set size should be the same as the number of items.");
                Assert.AreEqual(cluster, clusteringResult.SingleCluster,
                    $"Cluster {cluster} should be the single cluster.");
            }
        }

        #endregion
    }
}