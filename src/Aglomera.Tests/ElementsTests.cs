// ------------------------------------------
// <copyright file="ElementsTests.cs" company="Pedro Sequeira">
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
using System.Linq;
using ExamplesUtil;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aglomera.Tests
{
    [TestClass]
    public class ElementsTests
    {
        #region Public Methods

        [TestMethod]
        public void CentroidTest()
        {
            var datapoint1 = new DataPoint("A", new[] {0d, 0d});
            var datapoint2 = new DataPoint("B", new[] {6d, 0d});
            var datapoint3 = new DataPoint("C", new[] {3d, 0d});
            var cluster = new Cluster<DataPoint>(new[] {datapoint1, datapoint2});
            Console.WriteLine(cluster);
            var centroid = DataPoint.GetCentroid(cluster);
            Console.WriteLine($"{centroid.Value[0]},{centroid.Value[1]}");
            Assert.IsTrue(centroid.Value.SequenceEqual(datapoint3.Value),
                $"Centroid {centroid} should be equal to {datapoint3}");
        }

        [TestMethod]
        public void ClusterSetLengthTest()
        {
            var cluster1 = new Cluster<int>(new[] {1, 2, 3});
            var cluster2 = new Cluster<int>(new[] {1, 2, 3});
            var clusterSet = new ClusterSet<int>(new[] {cluster1, cluster2});
            Console.WriteLine(clusterSet);
            Assert.IsTrue(clusterSet.Count == 2, $"Cluster-set {clusterSet} should have length of 2.");
        }

        [TestMethod]
        public void MedoidTest()
        {
            var datapoint1 = new DataPoint("A", new[] {0d, 0d});
            var datapoint2 = new DataPoint("B", new[] {6d, 0d});
            var datapoint3 = new DataPoint("C", new[] {3d, 0d});
            var cluster = new Cluster<DataPoint>(new[] {datapoint1, datapoint2, datapoint3});
            Console.WriteLine(cluster);
            var medoid = cluster.GetMedoid(new DataPoint());
            Console.WriteLine(medoid);
            Assert.AreEqual(medoid, datapoint3, $"Medoid {medoid} should be equal to {datapoint3}");
        }

        #endregion
    }
}