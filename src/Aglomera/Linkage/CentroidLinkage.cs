// ------------------------------------------
// <copyright file="CentroidLinkage.cs" company="Pedro Sequeira">
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

namespace Aglomera.Linkage
{
    /// <summary>
    ///     Implements the centroid linkage clustering method, i.e., returns the distance between the centroid for each cluster
    ///     (a mean vector).
    /// </summary>
    /// <remarks>
    ///     Centroid linkage is equivalent to <see cref="AverageLinkage{TInstance}" /> of all pairs of documents from
    ///     different clusters. Thus, the difference between average and centroid clustering is that the former considers all
    ///     pairs of documents in computing average pairwise similarity, whereas centroid clustering excludes pairs from the
    ///     same cluster [1].
    ///     References:
    ///     [1] - <see href="https://nlp.stanford.edu/IR-book/html/htmledition/centroid-clustering-1.html" />.
    /// </remarks>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class CentroidLinkage<TInstance> : ILinkageCriterion<TInstance> where TInstance : IComparable<TInstance>

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

        /// <inheritdoc />
        public IDissimilarityMetric<TInstance> DissimilarityMetric { get; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public double Calculate(Cluster<TInstance> cluster1, Cluster<TInstance> cluster2)
        {
            return this.DissimilarityMetric.Calculate(this._centroidFunc(cluster1), this._centroidFunc(cluster2));
        }

        #endregion
    }
}