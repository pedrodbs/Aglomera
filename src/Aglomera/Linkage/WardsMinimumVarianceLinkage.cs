// ------------------------------------------
// <copyright file="WardsMinimumVarianceLinkage.cs" company="Pedro Sequeira">
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

namespace Aglomera.Linkage
{
    /// <summary>
    ///     Implements Ward's minimum variance method, i.e., returns the total within-cluster variance, corresponding to a
    ///     weighted squared distance between cluster centers.
    /// </summary>
    /// <remarks>
    ///     "With hierarchical clustering, the sum of squares starts out at zero (because every point is in its own cluster)
    ///     and then grows as we merge clusters. Ward's method keeps this growth as small as possible. This is nice if you
    ///     believe that the sum of squares should be small. Notice that the number of points shows up in [the formula], as
    ///     well as their geometric separation. Given two pairs of clusters whose centers are equally far apart, Ward's method
    ///     will prefer to merge the smaller ones." [1]
    ///     References:
    ///     [1] - <see href="http://www.stat.cmu.edu/~cshalizi/350/lectures/08/lecture-08.pdf" />.
    /// </remarks>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class WardsMinimumVarianceLinkage<TInstance> : ILinkageCriterion<TInstance>
        where TInstance : IComparable<TInstance>
    {
        #region Fields

        private readonly CentroidFunction<TInstance> _centroidFunc;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="WardsMinimumVarianceLinkage{TInstance}" /> with given dissimilarity metric and centroid
        ///     function.
        /// </summary>
        /// <param name="metric">The metric used to calculate dissimilarity between cluster elements.</param>
        /// <param name="centroidFunc">
        ///     A function to get an element representing the centroid of a <see cref="Cluster{TInstance}" />.
        /// </param>
        public WardsMinimumVarianceLinkage(
            IDissimilarityMetric<TInstance> metric, CentroidFunction<TInstance> centroidFunc)
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
            var centroidDist =
                this.DissimilarityMetric.Calculate(this._centroidFunc(cluster1), this._centroidFunc(cluster2));
            return centroidDist * centroidDist *
                   (cluster1.Count * cluster2.Count / ((double) cluster1.Count + cluster2.Count));
        }

        #endregion
    }
}