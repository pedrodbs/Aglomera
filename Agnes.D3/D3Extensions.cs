// ------------------------------------------
// <copyright file="D3Extensions.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes.D3
//    Last updated: 2017/06/05
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Agnes.D3
{
    public static class D3Extensions
    {
        #region Public Methods

        public static void SaveD3DendrogramFile<TInstance>(
            this ClusteringResult<TInstance> clustering, string filePath,
            bool printNames = true, Formatting formatting = Formatting.None)
            where TInstance : IComparable<TInstance>
        {
            using (var fs = File.OpenWrite(filePath))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                var writer = new JsonTextWriter(sw) {Formatting = formatting};
                WriteJson(clustering.SingleCluster, writer, printNames);
            }
        }

        #endregion

        #region Private & Protected Methods

        private static void WriteJson<TInstance>(
            Cluster<TInstance> cluster, JsonTextWriter writer, bool printNames)
            where TInstance : IComparable<TInstance>
        {
            writer.WriteStartObject();

            // writes name or node id
            writer.WritePropertyName("n");
            writer.WriteValue(printNames ? cluster.ToString() : string.Empty);

            // writes distance/dissimilarity
            writer.WritePropertyName("d");
            writer.WriteValue(Math.Round(cluster.Dissimilarity, 2));

            // writes dendrogram node's children, which in this case are the cluster's parents
            writer.WritePropertyName("c");
            writer.WriteStartArray();
            if (cluster.Parent1 != null)
            {
                // write parent 2 first if we want a left-balanced dendrogram
                WriteJson(cluster.Parent2, writer, printNames);
                WriteJson(cluster.Parent1, writer, printNames);
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        #endregion
    }
}