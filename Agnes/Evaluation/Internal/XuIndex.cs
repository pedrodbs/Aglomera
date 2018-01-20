// ------------------------------------------
// <copyright file="XuIndex.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes
//    Last updated: 2018/01/19
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
    ///     Implements the Xu-index internal evaluation method proposed in [1] measuring the compactness of clusters given some
    ///     partition scheme (<see cref="ClusterSet{TInstance}" />). The higher the negative value of the Xu-index, the better
    ///     the partition in some <see cref="ClusteringResult{TInstance}" /> is.
    /// </summary>
    /// <remarks>
    ///     Notes:
    ///     - In the original formulation in [1] the value was minimized, hence this implementation returns the negative
    ///     Xu-index ratio.
    ///     References:
    ///     [1] -
    ///     <see href="https://doi.org/10.1016/S0167-8655(97)00121-9">
    ///         Xu, L. (1997). Bayesian Ying–Yang machine, clustering and number of clusters. Pattern Recognition Letters,
    ///         18(11), 1167-1178.
    ///     </see>
    /// </remarks>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class XuIndex<TInstance> : IInternalEvaluationCriterion<TInstance>

    {
        #region Fields

        private readonly CentroidFunction<TInstance> _centroidFunc;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="XuIndex{TInstance}" /> with given dissimilarity metric.
        /// </summary>
        /// <param name="dissimilarityMetric">The metric used to calculate dissimilarity between cluster elements.</param>
        /// <param name="centroidFunc">
        ///     A function to get an element representing the centroid of a <see cref="Cluster{TInstance}" />.
        /// </param>
        public XuIndex(IDissimilarityMetric<TInstance> dissimilarityMetric,
            CentroidFunction<TInstance> centroidFunc)
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

            // gets clusters' centroids and total cluster
            var centroids = new List<TInstance>();
            var n = 0d;
            foreach (var cluster in clusterSet)
            {
                n += cluster.Count;
                centroids.Add(this._centroidFunc(cluster));
            }

            // updates sum of squared distances to centroids
            var sumDistWithin = 0d;
            for (var i = 0; i < clusterSet.Count; i++)
                foreach (var instance in clusterSet[i])
                {
                    var distWithin = this.DissimilarityMetric.Calculate(instance, centroids[i]);
                    sumDistWithin += distWithin * distWithin;
                }

            return -(Math.Log(Math.Sqrt(sumDistWithin / (n * n)), 2) + Math.Log(clusterSet.Count));
        }

        #endregion
    }
}