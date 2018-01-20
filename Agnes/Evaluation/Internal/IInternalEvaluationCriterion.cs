// ------------------------------------------
// <copyright file="IInternalEvaluationCriterion.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes
//    Last updated: 2018/01/11
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;

namespace Agnes.Evaluation.Internal
{
    /// <summary>
    ///     Represents an interface for criteria which uses the internal information resulting from a
    ///     <see cref="ClusteringAlgorithm{TInstance}" /> process to evaluate the goodness of a clustering structure without
    ///     reference to external information.
    ///     Implementations should be created so that when the criterion is <b>maximized</b> for a given
    ///     <see cref="ClusteringResult{TInstance}" />'s partition scheme, it provides the best
    ///     <see cref="ClusterSet{TInstance}" /> according to that criterion.
    /// </summary>
    /// <remarks>
    ///     These methods are useful for estimating the number of clusters to group data after executing the clustering
    ///     algorithm without any external data.
    /// </remarks>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public interface IInternalEvaluationCriterion<TInstance> 
    {
        #region Properties & Indexers

        /// <summary>
        ///     Gets the metric used by this criterion to measure the dissimilarity / distance between cluster elements.
        /// </summary>
        IDissimilarityMetric<TInstance> DissimilarityMetric { get; }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Evaluates the given <see cref="ClusterSet{TInstance}" /> partition according to this evaluation criterion.
        /// </summary>
        /// <param name="clusterSet">The clustering partition.</param>
        /// <returns>The evaluation of the given partition according to this criterion.</returns>
        double Evaluate(ClusterSet<TInstance> clusterSet);

        #endregion
    }
}