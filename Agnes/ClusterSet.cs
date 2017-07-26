// ------------------------------------------
// <copyright file="ClusterSet.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes
//    Last updated: 2017/07/26
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Agnes
{
    /// <summary>
    ///     Represents a set of <see cref="Cluster{TInstance}" /> elements that were found during the execution of the
    ///     clustering algorithm separated at some minimum distance.
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class ClusterSet<TInstance> : IEnumerable<Cluster<TInstance>>
        where TInstance : IComparable<TInstance>
    {
        #region Fields

        private readonly Cluster<TInstance>[] _clusters;

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets the number of clusters.
        /// </summary>
        public int Count => this._clusters.Length;

        /// <summary>
        ///     The minimum dissimilarity/distance at which the clusters were found.
        /// </summary>
        public double Dissimilarity { get; }

        /// <summary>
        ///     Gets the cluster at the give index.
        /// </summary>
        public Cluster<TInstance> this[int index] => this._clusters[index];

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="ClusterSet{TInstance}" /> with the given clusters and distance.
        /// </summary>
        /// <param name="clusters">The set of clusters.</param>
        /// <param name="dissimilarity">The dissimilarity/distance at which the clusters were found.</param>
        public ClusterSet(Cluster<TInstance>[] clusters, double dissimilarity = 0)
        {
            this._clusters = clusters;
            this.Dissimilarity = dissimilarity;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            var sb = new StringBuilder($"{this.Dissimilarity:0.000}\t{{");
            foreach (var cluster in this)
                sb.Append($"{cluster}, ");
            if (this.Count > 0) sb.Remove(sb.Length - 2, 2);
            sb.Append("}");
            return sb.ToString();
        }

        #endregion

        #region Public Methods

        public IEnumerator<Cluster<TInstance>> GetEnumerator()
        {
            return ((IEnumerable<Cluster<TInstance>>) this._clusters).GetEnumerator();
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