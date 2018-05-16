// ------------------------------------------
// <copyright file="AgglomerativeClusteringAlgorithm.cs" company="Pedro Sequeira">
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
using System.Linq;
using Aglomera.Linkage;

namespace Aglomera
{
    /// <summary>
    ///     Implements the agglomerative nesting clustering algorithm (program AGNES) in [1].
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    /// <remarks>
    ///     [1] Kaufman, L., &amp; Rousseeuw, P. J. (1990). Agglomerative nesting (program AGNES). Finding Groups in Data: An
    ///     Introduction to Cluster Analysis, 199-252.
    /// </remarks>
    public class AgglomerativeClusteringAlgorithm<TInstance> : IClusteringAlgorithm<TInstance>
        where TInstance : IComparable<TInstance>
    {
        #region Fields

        private Cluster<TInstance>[] _clusters;
        private int _curClusterCount;
        private double[][] _dissimilarities;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new instance of <see cref="AgglomerativeClusteringAlgorithm{TInstance}" /> with the given set of
        ///     instances and linkage
        ///     criterion.
        /// </summary>
        /// <param name="linkageCriterion">The criterion used to measure dissimilarities within and between clusters.</param>
        public AgglomerativeClusteringAlgorithm(ILinkageCriterion<TInstance> linkageCriterion)
        {
            this.LinkageCriterion = linkageCriterion;
        }

        #endregion

        #region Properties & Indexers

        /// <inheritdoc />
        public ILinkageCriterion<TInstance> LinkageCriterion { get; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public ClusteringResult<TInstance> GetClustering(ISet<TInstance> instances)
        {
            // initial setting: every instance in its own cluster
            var currentClusters = instances.Select(instance => new Cluster<TInstance>(instance));

            // executes clustering algorithm
            var clustering = this.GetClustering(currentClusters);

            return clustering;
        }

        /// <inheritdoc />
        public ClusteringResult<TInstance> GetClustering(ClusterSet<TInstance> clusterSet) =>
            this.GetClustering(clusterSet, clusterSet.Dissimilarity);

        /// <inheritdoc />
        public ClusteringResult<TInstance> GetClustering(IEnumerable<Cluster<TInstance>> clusters,
            double dissimilarity = 0d)
        {
            // initializes elements
            var currentClusters = clusters.ToArray();
            this._clusters = new Cluster<TInstance>[currentClusters.Length * 2 - 1];
            this._dissimilarities = new double[currentClusters.Length * 2 - 1][];
            this._curClusterCount = 0;

            // calculates initial dissimilarities
            foreach (var cluster in currentClusters)
            {
                this._clusters[this._curClusterCount] = cluster;
                this.UpdateDissimilarities();
                this._curClusterCount++;
            }

            var clustering = new ClusteringResult<TInstance>(currentClusters.Length)
                             {
                                 [0] = new ClusterSet<TInstance>(currentClusters, dissimilarity)
                             };
            var numSteps = currentClusters.Length;
            for (var i = 1; i < numSteps; i++)
            {
                // gets minimal dissimilarity between a pair of existing clusters
                var minDissimilarity = this.GetMinDissimilarity(out var clusterIdx1, out var clusterIdx2);

                // gets a copy of previous clusters, removes new cluster elements
                var cluster1 = this._clusters[clusterIdx1];
                var cluster2 = this._clusters[clusterIdx2];
                var newClusters = new Cluster<TInstance>[currentClusters.Length - 1];
                var idx = 0;
                foreach (var cluster in currentClusters)
                    if (!cluster.Equals(cluster1) && !cluster.Equals(cluster2))
                        newClusters[idx++] = cluster;
                this._clusters[clusterIdx1] = null;
                this._clusters[clusterIdx2] = null;

                // creates a new cluster from the union of closest clusters (save reference to parents)
                var newCluster = new Cluster<TInstance>(cluster1, cluster2, minDissimilarity);

                // adds cluster to list and calculates distance to all others
                newClusters[idx] = this._clusters[this._curClusterCount] = newCluster;
                this.UpdateDissimilarities();
                this._curClusterCount++;

                // updates global list of clusters
                currentClusters = newClusters;
                clustering[i] = new ClusterSet<TInstance>(currentClusters, minDissimilarity);
            }

            this._clusters = null;
            this._dissimilarities = null;
            return clustering;
        }

        #endregion

        #region Private & Protected Methods

        private double GetMinDissimilarity(out int clusterIdx1, out int clusterIdx2)
        {
            clusterIdx1 = clusterIdx2 = 0;

            // for each pair of clusters that still exist
            var minDissimilarity = double.MaxValue;
            for (var i = this._curClusterCount - 1; i > 0; i--)
            {
                if (this._clusters[i] == null) continue;
                for (var j = 0; j < i; j++)
                {
                    if (this._clusters[j] == null) continue;

                    // check dissimilarity and register indexes if minimal
                    var dissimilarity = this._dissimilarities[i][j];
                    if (minDissimilarity <= dissimilarity) continue;
                    minDissimilarity = dissimilarity;
                    clusterIdx1 = i;
                    clusterIdx2 = j;
                }
            }

            return minDissimilarity;
        }

        private void UpdateDissimilarities()
        {
            // update the dissimilarities / distances to all still existing clusters
            this._dissimilarities[this._curClusterCount] = new double[this._curClusterCount];
            for (var j = 0; j < this._curClusterCount; j++)
                if (this._clusters[j] != null)
                    this._dissimilarities[this._curClusterCount][j] =
                        this.LinkageCriterion.Calculate(this._clusters[j], this._clusters[this._curClusterCount]);
        }

        #endregion
    }
}