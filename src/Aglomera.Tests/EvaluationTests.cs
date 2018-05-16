// ------------------------------------------
// <copyright file="EvaluationTests.cs" company="Pedro Sequeira">
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
//    Last updated: 05/14/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Aglomera.Evaluation.External;
using Aglomera.Evaluation.Internal;
using Aglomera.Linkage;
using ExamplesUtil;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aglomera.Tests
{
    [TestClass]
    public class EvaluationTests
    {
        #region Static Fields & Constants

        private static readonly CentroidFunction<DataPoint> CentroidFunc = DataPoint.GetCentroid;

        private static readonly DataPoint Metric = new DataPoint();

        private static readonly Dictionary<string, IInternalEvaluationCriterion<DataPoint>> InternalCriteria =
            new Dictionary<string, IInternalEvaluationCriterion<DataPoint>>
            {
                {"Silhouette coefficient", new SilhouetteCoefficient<DataPoint>(Metric)},
                {"Dunn index", new DunnIndex<DataPoint>(Metric)},
                {"Davies-Bouldin index", new DaviesBouldinIndex<DataPoint>(Metric, CentroidFunc)},
                {"Calinski-Harabasz index", new CalinskiHarabaszIndex<DataPoint>(Metric, CentroidFunc)},
                {"Modified Gamma statistic", new ModifiedGammaStatistic<DataPoint>(Metric, CentroidFunc)},
                {"Xie-Beni index", new XieBeniIndex<DataPoint>(Metric, CentroidFunc)},
                {"Within-Between ratio", new WithinBetweenRatio<DataPoint>(Metric, CentroidFunc)},
                {"I-index", new IIndex<DataPoint>(Metric, CentroidFunc)},
                {"Xu index", new XuIndex<DataPoint>(Metric, CentroidFunc)},
                {"RMSSD", new RootMeanSquareStdDev<DataPoint>(Metric, CentroidFunc)},
                {"R-squared", new RSquared<DataPoint>(Metric, CentroidFunc)}
            };

        private static readonly Dictionary<string, IExternalEvaluationCriterion<DataPoint, char>> ExternalCriteria =
            new Dictionary<string, IExternalEvaluationCriterion<DataPoint, char>>
            {
                {"Purity", new Purity<DataPoint, char>()},
                {"NMI", new NormalizedMutualInformation<DataPoint, char>()},
                {"Accuracy", new RandIndex<DataPoint, char>()},
                {"Precision", new Precision<DataPoint, char>()},
                {"Recall", new Recall<DataPoint, char>()},
                {"F1Measure", new FMeasure<DataPoint, char>(1)},
                {"F2Measure", new FMeasure<DataPoint, char>(2)},
                {"F05Measure", new FMeasure<DataPoint, char>(0.5)}
            };

        private static readonly Dictionary<DataPoint, char> DataPointClasses =
            ClusteringTests.DataPoints.ToDictionary(dataPoint => dataPoint, dataPoint => dataPoint.ID[0]);

        #endregion

        #region Public Methods

        [TestMethod]
        public void ExternalCombinedEvalTest()
        {
            var clustering = GetClustering();
            var criteria = ExternalCriteria.Values.ToDictionary(crit => crit, crit => 1d / InternalCriteria.Count);
            var criterion = new CombinedExternalCriterion<DataPoint, char>(criteria);
            var evals = clustering.EvaluateClustering(criterion, DataPointClasses, (uint) clustering.Count);

            Assert.AreEqual(evals.Count, clustering.Count - 1,
                $"Clustering evaluation count should be {clustering.Count - 1}.");
        }

        [TestMethod]
        public void ExternalEvaluationCountTest()
        {
            var clustering = GetClustering();
            foreach (var criterion in ExternalCriteria)
            {
                Console.WriteLine("__________________________");
                Console.WriteLine(criterion.Key);
                var evals = clustering.EvaluateClustering(criterion.Value, DataPointClasses, (uint) clustering.Count)
                    .ToList();
                foreach (var eval in evals)
                {
                    Console.WriteLine(eval);
                    Assert.AreNotEqual(eval.ClusterSet.Count, 1,
                        $"Clustering evaluation for {criterion.Key} should not include the singleton cluster.");
                }

                Assert.AreEqual(evals[0].ClusterSet.Count, 2,
                    $"First cluster-set {evals[0].ClusterSet} should have 2 clusters.");
                Assert.AreEqual(evals[0].EvaluationValue, 1d, 1E-5,
                    $"First cluster-set {evals[0].ClusterSet} should have an external evaluation of 1.");
                Assert.AreEqual(evals.Count, clustering.Count - 1,
                    $"Clustering evaluation count for {criterion.Key} should be {clustering.Count - 1}.");
            }
        }

        [TestMethod]
        public void ExternalEvaluationValueTest()
        {
            var clustering = GetClustering();
            foreach (var criterion in ExternalCriteria)
            {
                Console.WriteLine("__________________________");
                Console.WriteLine(criterion.Key);
                var evals = clustering.EvaluateClustering(criterion.Value, DataPointClasses, (uint) clustering.Count)
                    .ToList();
                for (var i = 0; i < evals.Count; i++)
                {
                    var eval = evals[i];
                    Assert.IsTrue(
                        double.IsNaN(eval.EvaluationValue) || eval.EvaluationValue >= 0 && eval.EvaluationValue <= 1,
                        $"{criterion.Key} evaluation value for {eval} should be in [0,1].");

                    if (i == 0) continue;
                    var prevEval = evals[i - 1];
                    Assert.IsTrue(
                        double.IsNaN(eval.EvaluationValue) || prevEval.EvaluationValue >= eval.EvaluationValue,
                        $"{criterion.Key} evaluation value for {prevEval} should be greater than or equal to {eval}.");
                }
            }
        }

        [TestMethod]
        public void InternalCombinedEvalTest()
        {
            var clustering = GetClustering();
            var criteria = InternalCriteria.Values.ToDictionary(crit => crit, crit => 1d / InternalCriteria.Count);
            var criterion = new CombinedInternalCriterion<DataPoint>(criteria);
            var evals = clustering.EvaluateClustering(criterion, (uint) clustering.Count);

            Assert.AreEqual(evals.Count, clustering.Count - 1,
                $"Clustering evaluation count should be {clustering.Count - 1}.");
        }

        [TestMethod]
        public void InternalEvaluationCountTest()
        {
            var clustering = GetClustering();
            foreach (var criterion in InternalCriteria)
            {
                var evals = clustering.EvaluateClustering(criterion.Value, (uint) clustering.Count).ToList();
                foreach (var eval in evals)
                {
                    Console.WriteLine(eval);
                    Assert.AreNotEqual(eval.ClusterSet.Count, 1,
                        $"Clustering evaluation for {criterion.Key} should not include the singleton cluster.");
                }

                Assert.AreEqual(evals[0].ClusterSet.Count, 2,
                    $"First cluster-set {evals[0].ClusterSet} should have 2 clusters.");
                Assert.AreEqual(evals.Count, clustering.Count - 1,
                    $"Clustering evaluation count for {criterion.Key} should be {clustering.Count - 1}.");
            }
        }

        #endregion

        #region Private & Protected Methods

        private static ClusteringResult<DataPoint> GetClustering()
        {
            // performs hierarchical clustering
            var linkage = new AverageLinkage<DataPoint>(Metric);
            var clusteringAlg = new AgglomerativeClusteringAlgorithm<DataPoint>(linkage);
            return clusteringAlg.GetClustering(ClusteringTests.DataPoints);
        }

        #endregion
    }
}