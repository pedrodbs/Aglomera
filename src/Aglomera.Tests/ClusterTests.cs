// ------------------------------------------
// <copyright file="ClusterTests.cs" company="Pedro Sequeira">
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aglomera.Tests
{
    [TestClass]
    public class ClusterTests
    {
        #region Public Methods

        [TestMethod]
        public void CloneTest()
        {
            var cluster = new Cluster<int>(new[] {1, 2, 3});
            Console.WriteLine(cluster);

            var clusterClone = new Cluster<int>(cluster);
            Console.WriteLine(clusterClone);
            Assert.AreEqual(cluster, clusterClone, $"{cluster} and {clusterClone} should be considered equal.");

            clusterClone = clusterClone.Clone();
            Console.WriteLine(clusterClone);
            Assert.AreEqual(cluster, clusterClone, $"{cluster} and {clusterClone} should be considered equal.");
        }

        [TestMethod]
        public void CompareToTest()
        {
            var clusters = new[]
                           {
                               new Cluster<int>(new[] {1, 2}),
                               new Cluster<int>(new Cluster<int>(1),
                                   new Cluster<int>(new Cluster<int>(3), new Cluster<int>(2), 0), 0),
                               new Cluster<int>(new[] {1, 2, 4})
                           };
            for (var i = 0; i < clusters.Length; i++)
            {
                for (var j = i + 1; j < clusters.Length; j++)
                {
                    Assert.IsTrue(clusters[i].CompareTo(clusters[i]) == 0,
                        $"Comparison between cluster {clusters[i]} and itself should result in 0.");

                    var compareToij = clusters[i].CompareTo(clusters[j]);
                    Console.WriteLine($"{clusters[i]} compare to {clusters[j]}: {compareToij}");
                    Assert.IsTrue(compareToij < 0,
                        $"Cluster {clusters[i]} should be ranked lower than cluster {clusters[j]}.");

                    var compareToji = clusters[j].CompareTo(clusters[i]);
                    Console.WriteLine($"{clusters[j]} compare to {clusters[i]}: {compareToji}");
                    Assert.IsTrue(compareToji > 0,
                        $"Cluster {clusters[i]} should be ranked higher than cluster {clusters[j]}.");
                }
            }
        }

        [TestMethod]
        public void ContainsTest()
        {
            var cluster = new Cluster<int>(new[] {1, 2, 3});
            Console.WriteLine(cluster);

            foreach (var item in cluster)
            {
                Console.WriteLine(item);
                Assert.IsTrue(cluster.Contains(item), $"Cluster {cluster} should contain item {item}.");
            }
        }

        [TestMethod]
        public void EmptySetTest()
        {
            Console.WriteLine(Cluster<int>.Empty);
            Assert.AreEqual(Cluster<int>.Empty, Cluster<int>.Empty,
                "There should only be one instance of the empty cluster.");
            Assert.AreEqual(Cluster<int>.Empty.GetHashCode(), 0,
                "Hash-code of the empty cluster should be 0.");
            Assert.IsTrue(Cluster<int>.Empty.Count == 0,
                $"Empty cluster {Cluster<int>.Empty} should have a count of 0.");
        }

        [TestMethod]
        public void EqualParentsTest()
        {
            var cluster1 = new Cluster<int>(new Cluster<int>(1),
                new Cluster<int>(new Cluster<int>(3), new Cluster<int>(2), 0), 0);
            var cluster2 = new Cluster<int>(new Cluster<int>(new Cluster<int>(3), new Cluster<int>(2), 0),
                new Cluster<int>(1), 0);
            Console.WriteLine($"{cluster1} parents: {cluster1.Parent1}, {cluster1.Parent2}");
            Console.WriteLine($"{cluster2} parents: {cluster2.Parent1}, {cluster2.Parent2}");
            Assert.AreEqual(cluster1.Parent1, cluster2.Parent1,
                $"Cluster parents {cluster1.Parent1} and {cluster2.Parent1} should be considered equal.");
            Assert.AreEqual(cluster1.Parent2, cluster2.Parent2,
                $"Cluster parents {cluster1.Parent2} and {cluster2.Parent2} should be considered equal.");
        }

        [TestMethod]
        public void EqualsTest()
        {
            var clusters = new[]
                           {
                               new Cluster<int>(new[] {1, 2, 3}),
                               new Cluster<int>(new[] {2, 3, 1}),
                               new Cluster<int>(new Cluster<int>(1),
                                   new Cluster<int>(new Cluster<int>(3), new Cluster<int>(2), 0), 0)
                           };
            for (var i = 0; i < clusters.Length; i++)
            {
                Console.WriteLine(clusters[i]);
                for (var j = i + 1; j < clusters.Length; j++)
                {
                    Assert.AreEqual(clusters[i], clusters[i], $"{clusters[i]} should be considered equal with itself.");
                    Assert.AreEqual(clusters[i], clusters[j],
                        $"{clusters[i]} and {clusters[j]} should be considered equal.");
                }
            }
        }

        [TestMethod]
        public void HashcodeTest()
        {
            var clusters = new[]
                           {
                               new Cluster<int>(new[] {1, 2, 3}),
                               new Cluster<int>(new[] {3, 4}),
                               new Cluster<int>(new[] {1, 2}),
                               new Cluster<int>(new[] {5}),
                               new Cluster<int>(new[] {5}, 3d)
                           };
            foreach (var cluster in clusters)
            {
                Console.WriteLine($"{cluster}: {cluster.GetHashCode()}");
                Assert.AreNotEqual(0, cluster.GetHashCode(), $"Hash-code of {cluster} should not be 0.");
            }
        }

        [TestMethod]
        public void NotEqualsTest()
        {
            var clusters = new[]
                           {
                               new Cluster<int>(new[] {1, 2}),
                               new Cluster<int>(new[] {1, 2, 3}),
                               new Cluster<int>(new[] {1, 2, 3}, 1d),
                               new Cluster<int>(new Cluster<int>(1),
                                   new Cluster<int>(new Cluster<int>(4), new Cluster<int>(2), 0), 0)
                           };
            for (var i = 0; i < clusters.Length; i++)
            {
                Console.WriteLine(clusters[i]);
                for (var j = i + 1; j < clusters.Length; j++)
                    Assert.AreNotEqual(clusters[i], clusters[j],
                        $"{clusters[i]} and {clusters[j]} should not be considered equal.");
            }
        }

        #endregion
    }
}