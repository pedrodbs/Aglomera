// ------------------------------------------
// <copyright file="ModifiedGammaStatistic.cs" company="Pedro Sequeira">
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
using System.Linq;

namespace Aglomera.Evaluation.Internal
{
    /// <summary>
    ///     Implements an internal evaluation method based on a modified/improved version of Hubert's Gamma (Γ) statistic in
    ///     [1] with the transformation introduced in [2] in order to be maximized.
    /// </summary>
    /// <remarks>
    ///     Notes:
    ///     - "The improved Hubert Γ statistic describes the degree of a partition fitting the data set. [...] The partition
    ///     number fitting data set may be discovered from the plot of Γ versus n, however, it is difficult to find the
    ///     inflexion from the plot and it is possible that the partition number obtained is just close to the best solution,
    ///     but not that we want. So, it is not feasible in practice to determine the optimal partition by the plot of Γ versus
    ///     n directly. [...] In the plot of [transformed Γ] versus [number of clusters c], Γ goes to zero with c close to n,
    ///     and a max peak value that corresponds to a significant increase of c can be found. The number of clusters at which
    ///     the peak value occurs is equal to the number of clusters fitting the data." [2]
    ///     References:
    ///     [1] -
    ///     <see href="https://doi.org/10.1007/BF01908075">
    ///         Hubert, L., &amp; Arabie, P. (1985). Comparing partitions. Journal of classification, 2(1), 193-218.
    ///     </see>
    ///     [2] -
    ///     <see href="https://doi.org/10.1109/ICICIC.2006.250">
    ///         Zhao, H., Liang, J., &amp; Hu, H. (2006, August). Clustering Validity Based on the Improved Hubert\Gamma
    ///         Statistic and the Separation of Clusters. In First International Conference on Innovative Computing,
    ///         Information and Control, 2006. ICICIC'06.  (Vol. 2, pp. 539-543). IEEE.
    ///     </see>
    /// </remarks>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class ModifiedGammaStatistic<TInstance> : IInternalEvaluationCriterion<TInstance>
        where TInstance : IComparable<TInstance>

    {
        #region Fields

        private readonly CentroidFunction<TInstance> _centroidFunc;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="ModifiedGammaStatistic{TInstance}" /> with given dissimilarity metric.
        /// </summary>
        /// <param name="dissimilarityMetric">The metric used to calculate dissimilarity between cluster elements.</param>
        /// <param name="centroidFunc">
        ///     A function to get an element representing the centroid of a <see cref="Cluster{TInstance}" />.
        /// </param>
        public ModifiedGammaStatistic(
            IDissimilarityMetric<TInstance> dissimilarityMetric, CentroidFunction<TInstance> centroidFunc)
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

            // gets clusters' centroids 
            var centroids = clusterSet.Select(t => this._centroidFunc(t)).ToList();

            var n = 0;
            var sum = 0d;
            var minCentDist = double.MaxValue;
            for (var i = 0; i < clusterSet.Count; i++)
            {
                n += clusterSet[i].Count;
                for (var j = i + 1; j < clusterSet.Count; j++)
                {
                    // updates weighted pairwise distances
                    var betweenClusterDist = this.DissimilarityMetric.Calculate(centroids[i], centroids[j]);
                    minCentDist = Math.Min(minCentDist, betweenClusterDist);
                    sum += clusterSet[i].Sum(
                        inst1 => clusterSet[j]
                            .Sum(inst2 => this.DissimilarityMetric.Calculate(inst1, inst2) * betweenClusterDist));
                }
            }

            return 2 * sum * minCentDist / (n * (n - 1));
        }

        #endregion
    }
}