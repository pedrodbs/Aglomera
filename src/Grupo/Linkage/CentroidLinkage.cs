// ------------------------------------------
// <copyright file="CentroidLinkage.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Grupo
//    Last updated: 2018/01/18
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;

namespace Grupo.Linkage
{
    /// <summary>
    ///     Implements the centroid linkage clustering method, i.e., returns the distance between the centroid for each cluster
    ///     (a mean vector).
    /// </summary>
    /// <remarks>
    ///     Centroid linkage is equivalent to <see cref="AverageLinkage{TInstance}" /> of all pairs of documents from
    ///     different clusters. Thus, the difference between average and centroid clustering is that the former considers all
    ///     pairs of documents in computing average pairwise similarity, whereas centroid clustering excludes pairs from the
    ///     same cluster (<see href="https://nlp.stanford.edu/IR-book/html/htmledition/centroid-clustering-1.html" />).
    /// </remarks>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class CentroidLinkage<TInstance> : ILinkageCriterion<TInstance>

    {
        #region Fields

        private readonly Func<Cluster<TInstance>, TInstance> _centroidFunc;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="CentroidLinkage{TInstance}" /> with given dissimilarity metric and centroid
        ///     function.
        /// </summary>
        /// <param name="metric">The metric used to calculate dissimilarity between cluster elements.</param>
        /// <param name="centroidFunc">
        ///     A function to get an element representing the centroid of a <see cref="Cluster{TInstance}" />.
        /// </param>
        public CentroidLinkage(
            IDissimilarityMetric<TInstance> metric, Func<Cluster<TInstance>, TInstance> centroidFunc)
        {
            this.DissimilarityMetric = metric;
            this._centroidFunc = centroidFunc;
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
        ///     Calculates the distance / dissimilarity between the two given clusters according to this linkage criterion.
        /// </summary>
        /// <param name="cluster1">The first cluster.</param>
        /// <param name="cluster2">The second cluster.</param>
        /// <returns>
        ///     A value corresponding to the distance / dissimilarity between <paramref name="cluster1" /> and
        ///     <paramref name="cluster2" />, according to this linkage criterion.
        /// </returns>
        public double Calculate(Cluster<TInstance> cluster1, Cluster<TInstance> cluster2)
        {
            return this.DissimilarityMetric.Calculate(this._centroidFunc(cluster1), this._centroidFunc(cluster2));
        }

        #endregion
    }
}