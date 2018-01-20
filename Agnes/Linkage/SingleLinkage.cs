// ------------------------------------------
// <copyright file="SingleLinkage.cs" company="Pedro Sequeira">
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

using System.Linq;

namespace Agnes.Linkage
{
    /// <summary>
    ///     Implements the minimum or single-linkage clustering method, i.e., returns the minimum value of all pairwise
    ///     distances between the elements in each cluster. The method is also known as nearest neighbor clustering.
    /// </summary>
    /// <remarks>
    ///     A drawback of this method is that it tends to produce long thin clusters in which nearby elements of the same
    ///     cluster have small distances, but elements at opposite ends of a cluster may be much farther from each other than
    ///     two elements of other clusters (<see href="https://en.wikipedia.org/wiki/Single-linkage_clustering" />).
    ///     Since the merge criterion is strictly local, a chain of points can be extended for long distances without regard to
    ///     the overall shape of the emerging cluster. This effect is called chaining (
    ///     <see href="https://nlp.stanford.edu/IR-book/html/htmledition/single-link-and-complete-link-clustering-1.html" />).
    /// </remarks>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class SingleLinkage<TInstance> : ILinkageCriterion<TInstance>

    {
        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="SingleLinkage{TInstance}" /> with given dissimilarity metric.
        /// </summary>
        /// <param name="metric">The metric used to calculate dissimilarity between cluster elements.</param>
        public SingleLinkage(IDissimilarityMetric<TInstance> metric)
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
            return cluster1.Min(
                instance1 => cluster2.Min(instance2 => this.DissimilarityMetric.Calculate(instance1, instance2)));
        }

        #endregion
    }
}