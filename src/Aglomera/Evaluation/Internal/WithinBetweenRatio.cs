// ------------------------------------------
// <copyright file="WithinBetweenRatio.cs" company="Pedro Sequeira">
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
    ///     Implements the within-between ratio (WB) internal evaluation method in [1] measuring the ratio of the
    ///     sum-of-squares within cluster (SSW) and sum-of-squares between clusters(SSB).
    ///     The result is multiplied by the negative of the number of clusters so that maximizing the ratio in some
    ///     <see cref="ClusteringResult{TInstance}" /> provides the optimal partition, i.e., the optimal
    ///     <see cref="ClusterSet{TInstance}" />.
    /// </summary>
    /// <remarks>
    ///     Notes:
    ///     - In the original formulation in [1] the value was minimized, hence this implementation returns the negative WB
    ///     ratio.
    ///     References:
    ///     [1] -
    ///     <see href="https://doi.org/10.1007/978-3-642-04921-7_32">
    ///         Zhao, Q., Xu, M., &amp; Fränti, P. (2009, April). Sum-of-Squares Based Cluster Validity Index and Significance
    ///         Analysis. In ICANNGA (Vol. 5495, pp. 313-322).
    ///     </see>
    /// </remarks>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class WithinBetweenRatio<TInstance> : IInternalEvaluationCriterion<TInstance>
        where TInstance : IComparable<TInstance>

    {
        #region Fields

        private readonly CentroidFunction<TInstance> _centroidFunc;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="WithinBetweenRatio{TInstance}" /> with given dissimilarity metric.
        /// </summary>
        /// <param name="dissimilarityMetric">The metric used to calculate dissimilarity between cluster elements.</param>
        /// <param name="centroidFunc">
        ///     A function to get an element representing the centroid of a <see cref="Cluster{TInstance}" />.
        /// </param>
        public WithinBetweenRatio(IDissimilarityMetric<TInstance> dissimilarityMetric,
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
            Cluster<TInstance> totalCluster = null;
            foreach (var cluster in clusterSet)
            {
                totalCluster = totalCluster == null
                    ? new Cluster<TInstance>(cluster)
                    : new Cluster<TInstance>(totalCluster, cluster, 0);
                centroids.Add(this._centroidFunc(cluster));
            }

            var centroid = this._centroidFunc(totalCluster);

            var sumSquaresBetween = 0d;
            var sumSquaresWithin = 0d;
            for (var i = 0; i < clusterSet.Count; i++)
            {
                // updates sum of squared distances to centroid
                foreach (var instance in clusterSet[i])
                {
                    var distWithin = this.DissimilarityMetric.Calculate(instance, centroids[i]);
                    sumSquaresWithin += distWithin * distWithin;
                }

                // updates sum of squared distances of cluster centroid to global centroid
                var distBetween = this.DissimilarityMetric.Calculate(centroids[i], centroid);
                sumSquaresBetween += distBetween * distBetween * clusterSet[i].Count;
            }

            // - m * (SSW / SSB)
            return -clusterSet.Count * (sumSquaresWithin / sumSquaresBetween);
        }

        #endregion
    }
}