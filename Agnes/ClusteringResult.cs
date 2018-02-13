// ------------------------------------------
// <copyright file="ClusteringResult.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes
//    Last updated: 2018/01/21
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Agnes
{
    /// <summary>
    ///     Represents the result of a clustering algorithm, consisting in the sequence of <see cref="ClusterSet{TInstance}" />
    ///     elements that were found during the agglomeration of all clusters.
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class ClusteringResult<TInstance> : IEnumerable<ClusterSet<TInstance>>
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
        ///     Gets the <see cref="Cluster{TInstance}" /> corresponding to the agglomeration of all the <see cref="TInstance" />
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
            using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
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
                sw.Close();
            }
        }

        public IEnumerator<ClusterSet<TInstance>> GetEnumerator()
        {
            return ((IEnumerable<ClusterSet<TInstance>>) this._clusterSets).GetEnumerator();
        }

        #endregion

        #region Private & Protected Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}