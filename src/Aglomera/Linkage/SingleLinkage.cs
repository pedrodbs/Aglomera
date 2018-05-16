// ------------------------------------------
// <copyright file="SingleLinkage.cs" company="Pedro Sequeira">
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
using System.Linq;

namespace Aglomera.Linkage
{
    /// <summary>
    ///     Implements the minimum or single-linkage clustering method, i.e., returns the minimum value of all pairwise
    ///     distances between the elements in each cluster. The method is also known as nearest neighbor clustering.
    /// </summary>
    /// <remarks>
    ///     A drawback of this method is that it tends to produce long thin clusters in which nearby elements of the same
    ///     cluster have small distances, but elements at opposite ends of a cluster may be much farther from each other than
    ///     two elements of other clusters [1].
    ///     Since the merge criterion is strictly local, a chain of points can be extended for long distances without regard to
    ///     the overall shape of the emerging cluster. This effect is called chaining [2].
    ///     References:
    ///     [1] - <see href="https://en.wikipedia.org/wiki/Single-linkage_clustering" />.
    ///     [2] -
    ///     <see href="https://nlp.stanford.edu/IR-book/html/htmledition/single-link-and-complete-link-clustering-1.html" />
    /// </remarks>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class SingleLinkage<TInstance> : ILinkageCriterion<TInstance> where TInstance : IComparable<TInstance>

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

        /// <inheritdoc />
        public IDissimilarityMetric<TInstance> DissimilarityMetric { get; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public double Calculate(Cluster<TInstance> cluster1, Cluster<TInstance> cluster2)
        {
            return cluster1.Min(
                instance1 => cluster2.Min(instance2 => this.DissimilarityMetric.Calculate(instance1, instance2)));
        }

        #endregion
    }
}