// ------------------------------------------
// <copyright file="ClusteringAlgorithm.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes
//    Last updated: 2017/03/14
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using Agnes.Linkage;

namespace Agnes
{
    /// <summary>
    ///     Implements the agglomerative nesting clustering algorithm (program AGNES) in [1].
    /// </summary>
    /// <typeparam name="TInstance">The type of instace considered.</typeparam>
    /// <remarks>
    ///     [1] Kaufman, L., & Rousseeuw, P. J. (1990). Agglomerative nesting (program AGNES). Finding Groups in Data: An
    ///     Introduction to Cluster Analysis, 199-252.
    /// </remarks>
    public class ClusteringAlgorithm<TInstance> where TInstance : IComparable<TInstance>
    {
        #region Fields

        private readonly ISet<TInstance> _instances;

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
        /// <param name="instances">The instances to be clustered by the algorithm.</param>
        /// <param name="linkageCriterion">The criterion used to measure dissimilarities within and between clusters.</param>
        public ClusteringAlgorithm(ISet<TInstance> instances, ILinkageCriterion<TInstance> linkageCriterion)
        {
            this._instances = instances;
            this._linkageCriterion = linkageCriterion;
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Clusters the set of <see cref="TInstance" /> given to the algorithm.
        /// </summary>
        /// <returns>
        ///     A list of <see cref="KeyValuePair{TKey,TValue}" /> elements where the keys are sets of
        ///     <see cref="Cluster{TInstance}" /> found in each step of the algorithm and each value is the dissimilarity/distance
        ///     at which the corresponding set of clusters was created.
        /// </returns>
        public IList<KeyValuePair<IEnumerable<Cluster<TInstance>>, double>> GetClusters()
        {
            // initializes elements
            this._clusters = new Cluster<TInstance>[this._instances.Count * 2 - 1];
            this._dissimilarities = new double[this._instances.Count * 2 - 1][];
            this._curClusterCount = 0;

            // initial setting: every instance in its own cluster
            var currentClusters = new HashSet<Cluster<TInstance>>();
            foreach (var instance in this._instances)
            {
                currentClusters.Add(
                    this._clusters[this._curClusterCount] = new Cluster<TInstance>(instance));
                this.UpdateDissimilarities();
                this._curClusterCount++;
            }

            var clusters = new List<KeyValuePair<IEnumerable<Cluster<TInstance>>, double>>
                               {new KeyValuePair<IEnumerable<Cluster<TInstance>>, double>(currentClusters, 0)};
            for (var i = 1; i < this._instances.Count; i++)
            {
                // gets minimal dissimilarity between a pair of existing clusters
                int clusterIdx1, clusterIdx2;
                var minDissimilarity = this.GetMinDissimilarity(out clusterIdx1, out clusterIdx2);

                // gets a copy of previous clusters, removes new cluster elements
                var cluster1 = this._clusters[clusterIdx1];
                var cluster2 = this._clusters[clusterIdx2];
                currentClusters = new HashSet<Cluster<TInstance>>(currentClusters);
                currentClusters.Remove(cluster1);
                currentClusters.Remove(cluster2);
                this._clusters[clusterIdx1] = null;
                this._clusters[clusterIdx2] = null;

                // creates a new cluster from the union of closest clusters
                var newCluster = cluster1.UnionWith(cluster2);

                // adds cluster to list and calculates distance to all others
                currentClusters.Add(this._clusters[this._curClusterCount] = newCluster);
                this.UpdateDissimilarities();
                this._curClusterCount++;

                // updates global list of clusters
                clusters.Add(new KeyValuePair<IEnumerable<Cluster<TInstance>>, double>(currentClusters, minDissimilarity));
            }

            this._clusters = null;
            this._dissimilarities = null;
            return clusters;
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
                    if (dissimilarity >= minDissimilarity) continue;
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