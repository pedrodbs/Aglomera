// ------------------------------------------
// <copyright file="ClusteringAlgorithm.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes
//    Last updated: 2017/03/10
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
    /// <typeparam name="TInstance">The type of instace considered.</typeparam>
    /// <remarks>
    ///     [1] Kaufman, L., & Rousseeuw, P. J. (1990). Agglomerative nesting (program AGNES). Finding Groups in Data: An
    ///     Introduction to Cluster Analysis, 199-252.
    /// </remarks>
    public class ClusteringAlgorithm<TInstance> where TInstance : IEquatable<TInstance>
    {
        #region Fields

        private readonly ISet<TInstance> _instances;

        private readonly ILinkageCriterion<TInstance> _linkageCriterion;

        #endregion

        #region Constructors

        public ClusteringAlgorithm(ISet<TInstance> instances, ILinkageCriterion<TInstance> linkageCriterion)
        {
            this._instances = instances;
            this._linkageCriterion = linkageCriterion;
        }

        #endregion

        #region Public Methods

        public IDictionary<double, IEnumerable<Cluster<TInstance>>> GetClusters()
        {
            var currentClusters =
                new HashSet<Cluster<TInstance>>(this._instances.Select(instance => new Cluster<TInstance>(instance)));

            var clusters = new Dictionary<double, IEnumerable<Cluster<TInstance>>> {{0, currentClusters}};
            for (var i = 1; i < this._instances.Count; i++)
            {
                // gets minimal dissimilarity between a pair of existing clusters
                double minDissimilarity;
                var newClusterPair = this.GetMinDissimilarity(currentClusters.ToList(), out minDissimilarity);

                // gets a copy of previous clusters, removes new cluster elements
                var newClusters = new HashSet<Cluster<TInstance>>(currentClusters);
                newClusters.Remove(newClusterPair.Key);
                newClusters.Remove(newClusterPair.Value);

                // creates a new cluster from the union of closest clusters
                var newCluster = newClusterPair.Key.UnionWith(newClusterPair.Value);
                newClusters.Add(newCluster);

                // updates global list of clusters
                clusters.Add(minDissimilarity, newClusters);
                currentClusters = newClusters;
            }

            return clusters;
        }

        #endregion

        #region Private & Protected Methods

        private KeyValuePair<Cluster<TInstance>, Cluster<TInstance>> GetMinDissimilarity(
            IList<Cluster<TInstance>> clusters, out double minDissimilarity)
        {
            minDissimilarity = double.MaxValue;
            var newCluster = new KeyValuePair<Cluster<TInstance>, Cluster<TInstance>>(null, null);
            for (var i = 0; i < clusters.Count; i++)
            {
                var cluster1 = clusters[i];
                for (var j = i + 1; j < clusters.Count; j++)
                {
                    var cluster2 = clusters[j];
                    var dissimilarity = this._linkageCriterion.Calculate(cluster1, cluster2);
                    if (dissimilarity >= minDissimilarity) continue;
                    minDissimilarity = dissimilarity;
                    newCluster = new KeyValuePair<Cluster<TInstance>, Cluster<TInstance>>(cluster1, cluster2);
                }
            }
            return newCluster;
        }

        #endregion
    }
}