// ------------------------------------------
// <copyright file="ILinkageCriterion.cs" company="Pedro Sequeira">
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

namespace Grupo.Linkage
{
    /// <summary>
    ///     An interface for agglomeration (linkage) methods for computing distance between clusters to be used during
    ///     agglomerative clustering.
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public interface ILinkageCriterion<TInstance>
    {
        #region Properties & Indexers

        /// <summary>
        ///     Gets the metric used to measure the dissimilarity / distance between cluster elements according to this linkage
        ///     criterion.
        /// </summary>
        IDissimilarityMetric<TInstance> DissimilarityMetric { get; }

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
        double Calculate(Cluster<TInstance> cluster1, Cluster<TInstance> cluster2);

        #endregion
    }
}