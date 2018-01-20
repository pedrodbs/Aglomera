// ------------------------------------------
// <copyright file="IIndex.cs" company="Pedro Sequeira">
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
    ///     Implements the I-index internal evaluation method [1] that uses the ratio of the separation and compactness of a
    ///     given clustering partition scheme. To measure separation, it adopts the maximum distance between cluster centers
    ///     and for compactness, the distance from an to its cluster center.
    /// </summary>
    /// <remarks>
    ///     References:
    ///     [1] -
    ///     <see href="https://doi.org/10.1109/TPAMI.2002.1114856">
    ///         Maulik, U., & Bandyopadhyay, S. (2002). Performance evaluation of some clustering algorithms and validity
    ///         indices. IEEE Transactions on Pattern Analysis and Machine Intelligence, 24(12), 1650-1654.
    ///     </see>
    /// </remarks>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class IIndex<TInstance> : IInternalEvaluationCriterion<TInstance>

    {
        #region Fields

        private readonly CentroidFunction<TInstance> _centroidFunc;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="IIndex{TInstance}" /> with given dissimilarity metric.
        /// </summary>
        /// <param name="dissimilarityMetric">The metric used to calculate dissimilarity between cluster elements.</param>
        /// <param name="centroidFunc">
        ///     A function to get an element representing the centroid of a <see cref="Cluster{TInstance}" />.
        /// </param>
        public IIndex(
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

            // gets clusters' centroids and total cluster
            var centroids = new List<TInstance>();
            Cluster<TInstance> totalCluster = null;
            foreach (var cluster in clusterSet)
            {
                totalCluster = totalCluster == null
                    ? new Cluster<TInstance>(cluster)
                    : new Cluster<TInstance>(totalCluster, cluster, 0);
                centroids.Add(this._centroidFunc(cluster));
            }
            var centroid = this._centroidFunc(totalCluster);

            var sumTotal = 0d;
            var sumWithin = 0d;
            var maxBetweenDist = double.MinValue;
            for (var i = 0; i < clusterSet.Count; i++)
            {
                // updates within- and total-cluster distance sum
                foreach (var instance in clusterSet[i])
                {
                    sumWithin += this.DissimilarityMetric.Calculate(instance, centroids[i]);
                    sumTotal += this.DissimilarityMetric.Calculate(instance, centroid);
                }

                // updates max between-cluster distance
                for (var j = i + 1; j < clusterSet.Count; j++)
                    maxBetweenDist = Math.Max(maxBetweenDist,
                        this.DissimilarityMetric.Calculate(centroids[i], centroids[j]));
            }

            return maxBetweenDist * sumTotal / (clusterSet.Count * sumWithin);
        }

        #endregion
    }
}