// ------------------------------------------
// <copyright file="ClusterExtensions.cs" company="Pedro Sequeira">
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

namespace Agnes
{
    public static class ClusterExtensions
    {
        #region Public Methods

        public static Cluster<TInstance> GetIntersection<TInstance>(this IEnumerable<Cluster<TInstance>> clusters)
            where TInstance : IEquatable<TInstance>
        {
            var clusterList = clusters as IList<Cluster<TInstance>> ?? clusters?.ToList();
            if (clusterList == null || clusterList.Count == 0) return null;

            // just gets the intersection off all clusters
            var cluster = clusterList[0];
            for (var i = 1; i < clusterList.Count; i++)
                cluster = cluster.IntersectWith(clusterList[i]);
            return cluster;
        }

        public static Cluster<TInstance> GetUnion<TInstance>(this IEnumerable<Cluster<TInstance>> clusters)
            where TInstance : IEquatable<TInstance>
        {
            var clusterList = clusters as IList<Cluster<TInstance>> ?? clusters?.ToList();
            if (clusterList == null || clusterList.Count == 0) return null;

            // just gets the union off all clusters
            var cluster = clusterList[0];
            for (var i = 1; i < clusterList.Count; i++)
                cluster = cluster.UnionWith(clusterList[i]);
            return cluster;
        }

        #endregion
    }
}