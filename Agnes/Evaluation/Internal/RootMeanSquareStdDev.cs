// ------------------------------------------
// <copyright file="RootMeanSquareStdDev.cs" company="Pedro Sequeira">
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
using System.Linq;

namespace Agnes.Evaluation.Internal
{
    /// <summary>
    ///     Implements an internal evaluation method measuring the root-mean-square standard deviation (RMSSD), i.e., the
    ///     square root of the variance between all elements. This criterion considers only the compactness of the clustering
    ///     partition.
    /// </summary>
    /// <remarks>
    ///     In order to select the optimal partition / <see cref="ClusterSet{TInstance}" /> using this criterion given
    ///     some <see cref="ClusteringResult{TInstance}" /> one has to find the 'knee' in the plot of the criterion value vs.
    ///     the number of clusters.
    /// </remarks>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class RootMeanSquareStdDev<TInstance> : IInternalEvaluationCriterion<TInstance>

    {
        #region Fields

        private readonly CentroidFunction<TInstance> _centroidFunc;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="RootMeanSquareStdDev{TInstance}" /> with given dissimilarity metric.
        /// </summary>
        /// <param name="dissimilarityMetric">The metric used to calculate dissimilarity between cluster elements.</param>
        /// <param name="centroidFunc">
        ///     A function to get an element representing the centroid of a <see cref="Cluster{TInstance}" />.
        /// </param>
        public RootMeanSquareStdDev(
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

            // gets clusters' centroids 
            var centroids = clusterSet.Select(t => this._centroidFunc(t)).ToList();

            var n = 0;
            var sum = 0d;
            for (var i = 0; i < clusterSet.Count; i++)
            {
                n += clusterSet[i].Count;

                // updates sum of squared distances to centroids
                foreach (var instance in clusterSet[i])
                {
                    var dist = this.DissimilarityMetric.Calculate(instance, centroids[i]);
                    sum += dist * dist;
                }
            }

            return Math.Sqrt(sum / n);
        }

        #endregion
    }
}