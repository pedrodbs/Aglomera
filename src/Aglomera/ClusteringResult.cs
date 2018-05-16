// ------------------------------------------
// <copyright file="ClusteringResult.cs" company="Pedro Sequeira">
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Aglomera
{
    /// <summary>
    ///     Represents the result of a clustering algorithm, consisting in the sequence of <see cref="ClusterSet{TInstance}" />
    ///     elements that were found during the agglomeration of all clusters.
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class ClusteringResult<TInstance> : IEnumerable<ClusterSet<TInstance>>
        where TInstance : IComparable<TInstance>
    {
        #region Fields

        private readonly ClusterSet<TInstance>[] _clusterSets;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="ClusteringResult{TInstance}" /> of the given size.
        /// </summary>
        /// <param name="size">The maximum number of <see cref="ClusterSet{TInstance}" /> to be added by the algorithm.</param>
        public ClusteringResult(int size)
        {
            this._clusterSets = new ClusterSet<TInstance>[size];
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets the number of <see cref="ClusterSet{TInstance}" /> found by the algorithm.
        /// </summary>
        public int Count => this._clusterSets.Length;

        /// <summary>
        ///     Gets or sets the <see cref="ClusterSet{TInstance}" /> at the given index of the sequence.
        /// </summary>
        /// <param name="index">The index of the cluster set we want to get or set.</param>
        /// <returns>The <see cref="ClusterSet{TInstance}" /> at the given index of the sequence.</returns>
        public ClusterSet<TInstance> this[int index]
        {
            get => this._clusterSets[index];
            set => this._clusterSets[index] = value;
        }

        /// <summary>
        ///     Gets the <see cref="Cluster{TInstance}" /> corresponding to the agglomeration of all the
        ///     <typeparamref name="TInstance" />
        ///     elements considered by the algorithm.
        /// </summary>
        public Cluster<TInstance> SingleCluster => this._clusterSets[this._clusterSets.Length - 1][0];

        #endregion

        #region Public Methods

        /// <summary>
        ///     Saves the <see cref="ClusterSet{TInstance}" /> objects stored in this <see cref="ClusteringResult{TInstance}" /> in
        ///     a comma-separated values (CSV) file.
        /// </summary>
        /// <param name="filePath">The path to the file in which to save the clustering results.</param>
        /// <param name="sepChar">The character used to separate the fields in the CSV file.</param>
        public void SaveToCsv(string filePath, char sepChar = ',')
        {
            using (var fs = new FileStream(filePath, FileMode.Create))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.WriteLine($"Num. clusters{sepChar}Dissimilarity{sepChar}Cluster{sepChar}Instance");
                foreach (var clusterSet in this.Reverse())
                    for (var i = 0; i < clusterSet.Count; i++)
                    {
                        var cluster = clusterSet[i];
                        foreach (var instance in cluster)
                            sw.WriteLine(
                                $"{clusterSet.Count}{sepChar}{clusterSet.Dissimilarity}{sepChar}{i}{sepChar}{instance}");
                    }
            }
        }

        /// <inheritdoc />
        public IEnumerator<ClusterSet<TInstance>> GetEnumerator() =>
            ((IEnumerable<ClusterSet<TInstance>>) this._clusterSets).GetEnumerator();

        #endregion

        #region Private & Protected Methods

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion
    }
}