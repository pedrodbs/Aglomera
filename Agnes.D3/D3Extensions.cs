// ------------------------------------------
// <copyright file="D3Extensions.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes.D3
//    Last updated: 2017/03/10
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Agnes.D3.Util;

namespace Agnes.D3
{
    public static class D3Extensions
    {
        #region Public Methods

        public static void SaveD3File<TInstance>(
            this IList<KeyValuePair<double, IEnumerable<Cluster<TInstance>>>> clusters, string filePath, bool printNames = true)
            where TInstance : IEquatable<TInstance>
        {
            // creates a new d3 cluster for each initial cluster in the list
            var d3Clusters = GetD3Clusters(clusters[0].Key, clusters[0].Value, printNames);

            // iterates over the clusters and creates d3 cluster hierarchy
            for (var i = 1; i < clusters.Count; i++)
                d3Clusters = GetD3Clusters(clusters[i].Key, clusters[i].Value, d3Clusters, printNames);

            // gets/creates the root node
            var rootD3Cluster = (d3Clusters.Count == 1)
                ? d3Clusters.Values.First()
                : new D3Cluster<TInstance>(d3Clusters.Keys.GetUnion(), 1);

            // prints root cluster to json file
            rootD3Cluster.SerializeJsonFile(filePath);
        }

        #endregion

        #region Private & Protected Methods

        private static IDictionary<Cluster<TInstance>, D3Cluster<TInstance>> GetD3Clusters<TInstance>(
            double dissimilarity, IEnumerable<Cluster<TInstance>> clusters, bool printNames)
            where TInstance : IEquatable<TInstance>
        {
            return clusters.ToDictionary(
                cluster => cluster,
                cluster =>
                    new D3Cluster<TInstance>(cluster, dissimilarity) {Name = printNames ? cluster.ToString() : string.Empty});
        }

        private static IDictionary<Cluster<TInstance>, D3Cluster<TInstance>> GetD3Clusters<TInstance>(
            double dissimilarity, IEnumerable<Cluster<TInstance>> clusters,
            IDictionary<Cluster<TInstance>, D3Cluster<TInstance>> prevD3Clusters, bool printNames)
            where TInstance : IEquatable<TInstance>
        {
            // for each cluster
            var newD3Clusters = new Dictionary<Cluster<TInstance>, D3Cluster<TInstance>>();
            var children = new List<D3Cluster<TInstance>>();
            Cluster<TInstance> newCluster = null;
            foreach (var cluster in clusters)
            {
                // checks whether the same cluster exists in the previous step, just add to new cluster list
                if (prevD3Clusters.ContainsKey(cluster))
                {
                    newD3Clusters.Add(cluster, prevD3Clusters[cluster]);
                    continue;
                }

                // new cluster, gets the clusters from which it was formed (and the children for the D3 cluster)
                children.AddRange(
                    from prevD3Cluster in prevD3Clusters
                    where cluster.IsSupersetOf(prevD3Cluster.Key)
                    select prevD3Cluster.Value);
                newCluster = cluster;
            }
            if (newCluster == null) return null;

            // adds new d3 cluster with reference to children and add to new list
            var d3Cluster = new D3Cluster<TInstance>(newCluster, dissimilarity);
            if (printNames) d3Cluster.Name = newCluster.ToString();
            d3Cluster.Children.AddRange(children);
            newD3Clusters.Add(newCluster, d3Cluster);
            return newD3Clusters;
        }

        #endregion
    }
}