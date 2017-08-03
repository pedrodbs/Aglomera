// ------------------------------------------
// <copyright file="ClusteringAlgorithm.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes
//    Last updated: 2017/07/29
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Agnes.Linkage;

namespace Agnes
{
    /// <summary>
    ///     Implements the agglomerative nesting clustering algorithm (program AGNES) in [1].
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    /// <remarks>
    ///     [1] Kaufman, L., & Rousseeuw, P. J. (1990). Agglomerative nesting (program AGNES). Finding Groups in Data: An
    ///     Introduction to Cluster Analysis, 199-252.
    /// </remarks>
    public class ClusteringAlgorithm<TInstance> where TInstance : IComparable<TInstance>
    {
        #region Fields

        private readonly ILinkageCriterion<TInstance> _linkageCriterion;
        private Cluster<TInstance>[] _clusters;
        private int _curClusterCount;
        private double[][] _dissimilarities;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new instance of <see cref="ClusteringAlgorithm{TInstance}" /> with the given set of instances and linkage
        ///     criterion.
        /// </summary>
        /// <param name="linkageCriterion">The criterion used to measure dissimilarities within and between clusters.</param>
        public ClusteringAlgorithm(ILinkageCriterion<TInstance> linkageCriterion)
        {
            this._linkageCriterion = linkageCriterion;
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Clusters the set of <see cref="TInstance" /> given to the algorithm.
        /// </summary>
        /// <param name="instances">The instances to be clustered by the algorithm.</param>
        /// <returns>
        ///     A <see cref="ClusteringResult{TInstance}" /> containing all the <see cref="ClusterSet{TInstance}" /> found in each
        ///     step of the algorithm and the corresponding the dissimilarity/distance at which they were found.
        /// </returns>
        public ClusteringResult<TInstance> GetClustering(ISet<TInstance> instances)
        {
            // initial setting: every instance in its own cluster
            var currentClusters = instances.Select(instance => new Cluster<TInstance>(instance));

            // executes clustering algorithm
            var clustering = this.GetClustering(currentClusters);

            return clustering;
        }

        /// <summary>
        ///     Runs the clustering algorithm over the set of given <see cref="Cluster{TInstance}" />.
        /// </summary>
        /// <param name="clusters">The initial clusters provided to the algorithm.</param>
        /// <returns>
        ///     A <see cref="ClusteringResult{TInstance}" /> containing all the <see cref="ClusterSet{TInstance}" /> found in each
        ///     step of the algorithm and the corresponding the dissimilarity/distance at which they were found.
        /// </returns>
        public ClusteringResult<TInstance> GetClustering(IEnumerable<Cluster<TInstance>> clusters)
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
                                 [0] = new ClusterSet<TInstance>(currentClusters)
                             };
            var numSteps = currentClusters.Length;
            for (var i = 1; i < numSteps; i++)
            {
                // gets minimal dissimilarity between a pair of existing clusters
                var minDissimilarity = this.GetMinDissimilarity(out int clusterIdx1, out int clusterIdx2);

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
                    if (minDissimilarity < dissimilarity) continue;
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
                        this._linkageCriterion.Calculate(this._clusters[j], this._clusters[this._curClusterCount]);
        }

        #endregion
    }
}