// ------------------------------------------
// <copyright file="D3Extensions.cs" company="Pedro Sequeira">
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
//    Project: Aglomera.D3
//    Last updated: 05/15/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Aglomera.D3
{
    /// <summary>
    ///     Contains a set of extensions for <see cref="ClusteringResult{TInstance}" /> objects to enable export to D3.js
    ///     dendrogram files.
    /// </summary>
    public static class D3Extensions
    {
        #region Public Methods

        /// <summary>
        ///     Saves the given <see cref="ClusteringResult{TInstance}" /> to a d3.js dendrogram file.
        /// </summary>
        /// <typeparam name="TInstance">The type of instance considered.</typeparam>
        /// <param name="clustering">The clustering result to be saved to a dendrogram file.</param>
        /// <param name="filePath">The path to the file in which to save the clustering dendrogram.</param>
        /// <param name="printNames">Whether to include clusters' string representation in their nodes.</param>
        /// <param name="formatting">The Json file formatting.</param>
        public static void SaveD3DendrogramFile<TInstance>(
            this ClusteringResult<TInstance> clustering, string filePath,
            bool printNames = true, Formatting formatting = Formatting.None)
            where TInstance : IComparable<TInstance>
        {
            using (var fs = File.Create(filePath))
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