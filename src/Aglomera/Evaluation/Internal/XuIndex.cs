// ------------------------------------------
// <copyright file="XuIndex.cs" company="Pedro Sequeira">
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
using System.Collections.Generic;

namespace Aglomera.Evaluation.Internal
{
    /// <summary>
    ///     Implements the Xu-index internal evaluation method proposed in [1] measuring the compactness of clusters given some
    ///     partition scheme (<see cref="ClusterSet{TInstance}" />). The higher the negative value of the Xu-index, the better
    ///     the partition in some <see cref="ClusteringResult{TInstance}" /> is.
    /// </summary>
    /// <remarks>
    ///     Notes:
    ///     - In the original formulation in [1] the value was minimized, hence this implementation returns the negative
    ///     Xu-index ratio.
    ///     References:
    ///     [1] -
    ///     <see href="https://doi.org/10.1016/S0167-8655(97)00121-9">
    ///         Xu, L. (1997). Bayesian Ying–Yang machine, clustering and number of clusters. Pattern Recognition Letters,
    ///         18(11), 1167-1178.
    ///     </see>
    /// </remarks>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class XuIndex<TInstance> : IInternalEvaluationCriterion<TInstance> where TInstance : IComparable<TInstance>

    {
        #region Fields

        private readonly CentroidFunction<TInstance> _centroidFunc;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="XuIndex{TInstance}" /> with given dissimilarity metric.
        /// </summary>
        /// <param name="dissimilarityMetric">The metric used to calculate dissimilarity between cluster elements.</param>
        /// <param name="centroidFunc">
        ///     A function to get an element representing the centroid of a <see cref="Cluster{TInstance}" />.
        /// </param>
        public XuIndex(IDissimilarityMetric<TInstance> dissimilarityMetric,
            CentroidFunction<TInstance> centroidFunc)
        {
            this._centroidFunc = centroidFunc;
            this.DissimilarityMetric = dissimilarityMetric;
        }

        #endregion

        #region Properties & Indexers

        /// <inheritdoc />
        public IDissimilarityMetric<TInstance> DissimilarityMetric { get; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public double Evaluate(ClusterSet<TInstance> clusterSet)
        {
            // undefined if only one cluster
            if (clusterSet.Count < 2) return double.NaN;

            // gets clusters' centroids and total cluster
            var centroids = new List<TInstance>();
            var n = 0d;
            foreach (var cluster in clusterSet)
            {
                n += cluster.Count;
                centroids.Add(this._centroidFunc(cluster));
            }

            // updates sum of squared distances to centroids
            var sumDistWithin = 0d;
            for (var i = 0; i < clusterSet.Count; i++)
                foreach (var instance in clusterSet[i])
                {
                    var distWithin = this.DissimilarityMetric.Calculate(instance, centroids[i]);
                    sumDistWithin += distWithin * distWithin;
                }

            return -(Math.Log(Math.Sqrt(sumDistWithin / (n * n)), 2) + Math.Log(clusterSet.Count));
        }

        #endregion
    }
}