// ------------------------------------------
// <copyright file="AverageLinkage.cs" company="Pedro Sequeira">
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
    ///     Implements the unweighted pair-group average method or UPGMA, i.e., returns the mean distance between the elements
    ///     in each cluster.
    /// </summary>
    /// <remarks>
    ///     Average linkage tries to strike a balance between <see cref="SingleLinkage{TInstance}" /> and
    ///     <see cref="CompleteLinkage{TInstance}" />. It uses average pairwise dissimilarity, so clusters tend to be
    ///     relatively compact and relatively far apart. However, it is not clear what properties the resulting clusters have
    ///     when we cut an average linkage tree at given distance. Single and complete linkage trees each had simple
    ///     interpretations [1].
    ///     References:
    ///     [1] - <see href="http://www.stat.cmu.edu/~ryantibs/datamining/lectures/05-clus2-marked.pdf" />.
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

        /// <inheritdoc />
        public IDissimilarityMetric<TInstance> DissimilarityMetric { get; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public double Calculate(Cluster<TInstance> cluster1, Cluster<TInstance> cluster2)
        {
            var sum = cluster1.Sum(
                instance1 => cluster2.Sum(instance2 => this.DissimilarityMetric.Calculate(instance1, instance2)));
            return sum / (cluster1.Count * cluster2.Count);
        }

        #endregion
    }
}