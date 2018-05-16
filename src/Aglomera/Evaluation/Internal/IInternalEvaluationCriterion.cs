// ------------------------------------------
// <copyright file="IInternalEvaluationCriterion.cs" company="Pedro Sequeira">
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
//    Last updated: 05/14/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;

namespace Aglomera.Evaluation.Internal
{
    /// <summary>
    ///     Represents an interface for criteria which uses the internal information resulting from a
    ///     <see cref="AgglomerativeClusteringAlgorithm{TInstance}" /> process to evaluate the goodness of a clustering structure without
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
    public interface IInternalEvaluationCriterion<TInstance> where TInstance : IComparable<TInstance>
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