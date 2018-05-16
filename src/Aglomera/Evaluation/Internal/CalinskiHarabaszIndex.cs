// ------------------------------------------
// <copyright file="CalinskiHarabaszIndex.cs" company="Pedro Sequeira">
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
//    Project: Aglomera
//    Last updated: 05/15/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;

namespace Aglomera.Evaluation.Internal
{
    /// <summary>
    ///     Implements the internal evaluation method in [1] that measures compactness and separation of clusters
    ///     simultaneously. The numerator reflects the degree of separation in the way of how much the cluster centers are
    ///     spread, and the denominator corresponds to compactness, to reflect how close the within-cluster objects are
    ///     gathered around the cluster center.
    /// </summary>
    /// <remarks>
    ///     References:
    ///     [1] -
    ///     <see href="https://doi.org/10.1080/03610927408827101">
    ///         Caliński, T., &amp; Harabasz, J. (1974). A dendrite method for cluster analysis. Communications in
    ///         Statistics-theory and Methods, 3(1), 1-27.
    ///     </see>
    /// </remarks>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class CalinskiHarabaszIndex<TInstance> : IInternalEvaluationCriterion<TInstance>
        where TInstance : IComparable<TInstance>
    {
        #region Fields

        private readonly CentroidFunction<TInstance> _centroidFunc;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="DaviesBouldinIndex{TInstance}" /> with given dissimilarity metric.
        /// </summary>
        /// <param name="dissimilarityMetric">The metric used to calculate dissimilarity between cluster elements.</param>
        /// <param name="centroidFunc">
        ///     A function to get an element representing the centroid of a <see cref="Cluster{TInstance}" />.
        /// </param>
        public CalinskiHarabaszIndex(
            IDissimilarityMetric<TInstance> dissimilarityMetric, CentroidFunction<TInstance> centroidFunc)
        {
            this._centroidFunc = centroidFunc;
            this.DissimilarityMetric = dissimilarityMetric;
        }

        #endregion

        #region Properties & Indexers

        /// <inheritdoc />
        public IDissimilarityMetric<TInstance> DissimilarityMetric { get; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public double Evaluate(ClusterSet<TInstance> clusterSet)
        {
            // undefined if only one cluster
            if (clusterSet.Count < 2) return double.NaN;

            // gets clusters' centroids and overall centroid
            var centroids = new List<TInstance>();
            Cluster<TInstance> allPoints = null;
            for (var i = 0; i < clusterSet.Count; i++)
            {
                allPoints = allPoints == null
                    ? new Cluster<TInstance>(clusterSet[i])
                    : new Cluster<TInstance>(allPoints, clusterSet[i], 0);
                centroids.Add(this._centroidFunc(clusterSet[i]));
            }

            var overallCentroid = this._centroidFunc(allPoints);

            var betweenVar = 0d;
            var withinVar = 0d;
            for (var i = 0; i < clusterSet.Count; i++)
            {
                // updates overall between-cluster variance
                var betweenDist = this.DissimilarityMetric.Calculate(centroids[i], overallCentroid);
                betweenVar += betweenDist * betweenDist * clusterSet[i].Count;

                // updates overall within-cluster variance
                foreach (var instance in clusterSet[i])
                {
                    var withinDist = this.DissimilarityMetric.Calculate(instance, centroids[i]);
                    withinVar += withinDist * withinDist;
                }
            }

            return Math.Abs(withinVar) < double.Epsilon
                ? double.NaN
                : betweenVar * (allPoints.Count - clusterSet.Count) / (withinVar * (clusterSet.Count - 1));
        }

        #endregion
    }
}