// ------------------------------------------
// <copyright file="ClusterSet.cs" company="Pedro Sequeira">
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
//    Last updated: 04/27/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Aglomera
{
    /// <summary>
    ///     Represents a set of <see cref="Cluster{TInstance}" /> elements that were found during the execution of the
    ///     clustering algorithm separated at some minimum distance.
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class ClusterSet<TInstance> : IEnumerable<Cluster<TInstance>> where TInstance : IComparable<TInstance>
    {
        #region Fields

        private readonly Cluster<TInstance>[] _clusters;

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

        #region Public Methods

        /// <summary>
        ///     Returns a string representation for the cluster-set in the form 'Dissimilarity   {cluster1, cluster2, ...,
        ///     clusterN}'.
        /// </summary>
        /// <returns>A string representation for the cluster-set.</returns>
        public override string ToString() => this.ToString(true);

        #endregion

        #region Public Methods

        /// <summary>
        ///     Returns a string representation for the cluster-set in the form 'Dissimilarity   {cluster1, cluster2, ...,
        ///     clusterN}'. The presentation of the dissimilarity value is optional.
        /// </summary>
        /// <param name="includeDissimilarity">
        ///     Whether to include the value of <see cref="Dissimilarity" /> in the string representation.
        /// </param>
        /// <returns>A string representation for the cluster-set.</returns>
        public string ToString(bool includeDissimilarity)
        {
            var sb = includeDissimilarity
                ? new StringBuilder($"{this.Dissimilarity:0.000}\t{{")
                : new StringBuilder("{{");

            foreach (var cluster in this)
                sb.Append($"{cluster}, ");
            if (this.Count > 0) sb.Remove(sb.Length - 2, 2);
            sb.Append("}");
            return sb.ToString();
        }

        /// <inheritdoc />
        public IEnumerator<Cluster<TInstance>> GetEnumerator() =>
            ((IEnumerable<Cluster<TInstance>>) this._clusters).GetEnumerator();

        #endregion

        #region Private & Protected Methods

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion
    }
}