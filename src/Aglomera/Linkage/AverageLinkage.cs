// ------------------------------------------
// <copyright file="AverageLinkage.cs" company="Pedro Sequeira">
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
using System.Linq;

namespace Aglomera.Linkage
{
    /// <summary>
    ///     Implements the unweighted pair-group average method or UPGMA, i.e., returns the mean distance between the elements
    ///     in each cluster.
    /// </summary>
    /// <remarks>
    ///     Average linkage tries to strike a balance between <see cref="SingleLinkage{TInstance}" /> and
    ///     <see cref="CompleteLinkage{TInstance}" />. It uses average pairwise dissimilarity, so clusters tend to be
    ///     relatively compact and relatively far apart. However, it is not clear what properties the resulting clusters have
    ///     when we cut an average linkage tree at given distance. Single and complete linkage trees each had simple
    ///     interpretations (<see href="http://www.stat.cmu.edu/~ryantibs/datamining/lectures/05-clus2-marked.pdf" />).
    /// </remarks>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class AverageLinkage<TInstance> : ILinkageCriterion<TInstance> where TInstance : IComparable<TInstance>

    {
        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="AverageLinkage{TInstance}" /> with given dissimilarity metric.
        /// </summary>
        /// <param name="metric">The metric used to calculate dissimilarity between cluster elements.</param>
        public AverageLinkage(IDissimilarityMetric<TInstance> metric)
        {
            this.DissimilarityMetric = metric;
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
            var sum = cluster1.Sum(
                instance1 => cluster2.Sum(instance2 => this.DissimilarityMetric.Calculate(instance1, instance2)));
            return sum / (cluster1.Count * cluster2.Count);
        }

        #endregion
    }
}