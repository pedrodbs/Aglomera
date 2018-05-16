// ------------------------------------------
// <copyright file="IClusteringAlgorithm.cs" company="Pedro Sequeira">
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
using Aglomera.Linkage;

namespace Aglomera
{
    /// <summary>
    ///     Represents an interface for hierarchical agglomerative clustering algorithms.
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public interface IClusteringAlgorithm<TInstance> where TInstance : IComparable<TInstance>
    {
        #region Properties & Indexers

        /// <summary>
        ///     Gets the <see cref="ILinkageCriterion{TInstance}" /> used by this algorithm to create the clusters.
        /// </summary>
        ILinkageCriterion<TInstance> LinkageCriterion { get; }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Clusters the set of <typeparamref name="TInstance" /> given to the algorithm.
        /// </summary>
        /// <param name="instances">The instances to be clustered by the algorithm.</param>
        /// <returns>
        ///     A <see cref="ClusteringResult{TInstance}" /> containing all the <see cref="ClusterSet{TInstance}" /> found in each
        ///     step of the algorithm and the corresponding the dissimilarity/distance at which they were found.
        /// </returns>
        ClusteringResult<TInstance> GetClustering(ISet<TInstance> instances);

        /// <summary>
        ///     Runs the clustering algorithm over the set of given <see cref="Cluster{TInstance}" />.
        /// </summary>
        /// <param name="clusters">The initial clusters provided to the algorithm.</param>
        /// <param name="dissimilarity">The initial dissimilarity associated with the given clusters.</param>
        /// <returns>
        ///     A <see cref="ClusteringResult{TInstance}" /> containing all the <see cref="ClusterSet{TInstance}" /> found in each
        ///     step of the algorithm and the corresponding the dissimilarity/distance at which they were found.
        /// </returns>
        ClusteringResult<TInstance> GetClustering(IEnumerable<Cluster<TInstance>> clusters, double dissimilarity = 0d);

        /// <summary>
        ///     Runs the clustering algorithm over the given <see cref="ClusterSet{TInstance}" />.
        /// </summary>
        /// <param name="clusterSet">The initial clusters and dissimilarity provided to the algorithm.</param>
        /// <returns>
        ///     A <see cref="ClusteringResult{TInstance}" /> containing all the <see cref="ClusterSet{TInstance}" /> found in each
        ///     step of the algorithm and the corresponding the dissimilarity/distance at which they were found.
        /// </returns>
        ClusteringResult<TInstance> GetClustering(ClusterSet<TInstance> clusterSet);

        #endregion
    }
}