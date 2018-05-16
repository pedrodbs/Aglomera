// ------------------------------------------
// <copyright file="SilhouetteCoefficient.cs" company="Pedro Sequeira">
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
using System.Linq;

namespace Aglomera.Evaluation.Internal
{
    /// <summary>
    ///     Implements an internal evaluation method that measures how similar an element is to its own cluster (cohesion)
    ///     compared to other clusters (separation). The silhouette ranges from −1 to +1, where a high value indicates that the
    ///     element is well matched to its own cluster and poorly matched to neighboring clusters. If most elements (average)
    ///     have a high value, then the clustering configuration is appropriate. If the average is a low or negative value,
    ///     then the clustering configuration may have too many or too few clusters.
    /// </summary>
    /// <remarks>
    ///     Assumptions in [2]:
    ///     - "Note that the construction [...] depends on the availability of other clusters apart from A, so we have to
    ///     assume [...] that the number of clusters k is more than one."
    ///     - "When cluster A contains only a single object it is unclear how u(i) should be defined, and then we simply set
    ///     s(i) equal to zero. This choice is of course arbitrary, but a value of zero appears to be most neutral."
    ///     Therefore, Silhouette coefficient punishes outliers and noise, so in the presence of such data we should avoid it.
    ///     References:
    ///     [1] - <see href="https://en.wikipedia.org/wiki/Silhouette_(clustering)" />
    ///     [2] -
    ///     <see href="https://doi.org/10.1016/0377-0427(87)90125-7">
    ///         Rousseeuw, P. J. (1987). Silhouettes: a graphical aid to
    ///         the interpretation and validation of cluster analysis. Journal of computational and applied mathematics, 20,
    ///         53-65.
    ///     </see>
    /// </remarks>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class SilhouetteCoefficient<TInstance> : IInternalEvaluationCriterion<TInstance>
        where TInstance : IComparable<TInstance>

    {
        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="SilhouetteCoefficient{TInstance}" /> with given dissimilarity metric.
        /// </summary>
        /// <param name="dissimilarityMetric">The metric used to calculate dissimilarity between cluster elements.</param>
        public SilhouetteCoefficient(IDissimilarityMetric<TInstance> dissimilarityMetric)
        {
            this.DissimilarityMetric = dissimilarityMetric;
        }

        #endregion

        #region Properties & Indexers

        /// <inheritdoc />
        public IDissimilarityMetric<TInstance> DissimilarityMetric { get; }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Calculates the silhouette coefficient for each element in the given <see cref="ClusterSet{TInstance}" />.
        /// </summary>
        /// <param name="clusterSet">The clustering partition.</param>
        /// <returns>A dictionary containing the silhouette coefficient for each element in the given partition.</returns>
        public IDictionary<TInstance, double> EvaluateEach(ClusterSet<TInstance> clusterSet)
        {
            // gets silhouette coefficient for all instances in all clusters
            var coefs = new Dictionary<TInstance, double>();
            for (var i = 0; i < clusterSet.Count; i++)
            {
                var cluster = clusterSet[i].ToList();
                for (var j = 0; j < cluster.Count; j++)
                {
                    var instance = cluster[j];
                    if (clusterSet.Count == 1 || cluster.Count == 1)
                    {
                        // silhouette is undefined for singletons or when there is only one cluster
                        coefs.Add(instance, 0); // double.NaN);
                        continue;
                    }

                    // gets the average distance with all other data within the same cluster
                    var avgWithinDist = 0d;
                    for (var k = 0; k < cluster.Count; k++)
                        if (k != j)
                            avgWithinDist += this.DissimilarityMetric.Calculate(instance, cluster[k]);
                    avgWithinDist /= cluster.Count - 1;

                    // gets minimal dissimilarity to other clusters
                    var minAvgBetweenDist = clusterSet.Count == 1 ? 0 : double.MaxValue;
                    for (var l = 0; l < clusterSet.Count; l++)
                    {
                        if (l == i) continue;
                        var avgBetweenDist = clusterSet[l]
                            .Average(other => this.DissimilarityMetric.Calculate(instance, other));
                        minAvgBetweenDist = Math.Min(minAvgBetweenDist, avgBetweenDist);
                    }

                    // calculates silhouette coefficient for the instance 
                    // (if both distances are 0 clusters should be together, so set -1)
                    var maxDist = Math.Max(minAvgBetweenDist, avgWithinDist);
                    var coef = Math.Abs(maxDist) < double.Epsilon ? -1 : (minAvgBetweenDist - avgWithinDist) / maxDist;
                    coefs.Add(instance, coef);
                }
            }

            return coefs;
        }

        /// <inheritdoc />
        public double Evaluate(ClusterSet<TInstance> clusterSet)
        {
            // gets average silhouette coefficient of all instances in all clusters
            return clusterSet.Count < 2 ? double.NaN : this.EvaluateEach(clusterSet).Values.Average();
        }

        #endregion
    }
}