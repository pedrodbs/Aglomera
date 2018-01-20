// ------------------------------------------
// <copyright file="CalinskiHarabaszIndex.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes
//    Last updated: 2018/01/18
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;

namespace Agnes.Evaluation.Internal
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
    ///         Caliński, T., & Harabasz, J. (1974). A dendrite method for cluster analysis. Communications in
    ///         Statistics-theory and Methods, 3(1), 1-27.
    ///     </see>
    /// </remarks>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class CalinskiHarabaszIndex<TInstance> : IInternalEvaluationCriterion<TInstance>
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

        /// <summary>
        ///     Gets the metric used by this criterion to measure the dissimilarity / distance between cluster elements.
        /// </summary>
        public IDissimilarityMetric<TInstance> DissimilarityMetric { get; }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Evaluates the given <see cref="ClusterSet{TInstance}" /> partition according to this evaluation criterion.
        /// </summary>
        /// <param name="clusterSet">The clustering partition.</param>
        /// <returns>The evaluation of the given partition according to this criterion.</returns>
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