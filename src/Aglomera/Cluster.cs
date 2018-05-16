// ------------------------------------------
// <copyright file="Cluster.cs" company="Pedro Sequeira">
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
using System.Linq;
using System.Text;

namespace Aglomera
{
    /// <summary>
    ///     Represents a set of <typeparamref name="TInstance" /> elements arranged in a hierarchical form.
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class Cluster<TInstance> :
        IEnumerable<TInstance>, IEquatable<Cluster<TInstance>>, IComparable<Cluster<TInstance>>
        where TInstance : IComparable<TInstance>
    {
        #region Static Fields & Constants

        private const double MIN_DISSIMILARITY = double.Epsilon;

        /// <summary>
        ///     Gets an empty cluster.
        /// </summary>
        public static readonly Cluster<TInstance> Empty = new Cluster<TInstance>(new List<TInstance>());

        #endregion

        #region Fields

        private readonly TInstance[] _cluster;

        private readonly int _hashCode;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="Cluster{TInstance}" /> by joining the two given clusters.
        /// </summary>
        /// <param name="parent1">The first parent of the new cluster.</param>
        /// <param name="parent2">The second parent of the new cluster.</param>
        /// <param name="dissimilarity">The dissimilarity/distance at which the new cluster was found.</param>
        public Cluster(Cluster<TInstance> parent1, Cluster<TInstance> parent2, double dissimilarity)
        {
            // parent order is not important
            this.Parent1 = parent1.CompareTo(parent2) > 0 ? parent2 : parent1;
            this.Parent2 = parent1.CompareTo(parent2) > 0 ? parent1 : parent2;
            this.Dissimilarity = dissimilarity;

            // copies and sorts elements 
            this._cluster = new TInstance[parent1._cluster.Length + parent2._cluster.Length];
            int idx = 0, i = 0, j = 0;
            while (i < parent1.Count && j < parent2.Count)
            {
                // checks elements from each parent, chooses lowest
                this._cluster[idx++] = parent1._cluster[i].CompareTo(parent2._cluster[j]) <= 0
                    ? parent1._cluster[i++]
                    : parent2._cluster[j++];

                // if all the elements of one parent were copied, just copy those of the other parent
                if (i >= parent1.Count)
                {
                    Array.Copy(parent2._cluster, j, this._cluster, idx, parent2._cluster.Length - j);
                    break;
                }

                if (j >= parent2.Count)
                {
                    Array.Copy(parent1._cluster, i, this._cluster, idx, parent1._cluster.Length - i);
                    break;
                }
            }

            this._hashCode = this.ProduceHashCode();
        }

        /// <summary>
        ///     Creates a new <see cref="Cluster{TInstance}" /> with a single <typeparamref name="TInstance" /> element.
        /// </summary>
        /// <param name="instance">The single element in the new cluster.</param>
        /// <param name="dissimilarity">The dissimilarity/distance at which the new cluster was found.</param>
        public Cluster(TInstance instance, double dissimilarity = 0) : this(new[] {instance}, dissimilarity)
        {
        }

        /// <summary>
        ///     Creates a new <see cref="Cluster{TInstance}" /> with the given <typeparamref name="TInstance" /> elements.
        /// </summary>
        /// <param name="instances">The elements in the new cluster.</param>
        /// <param name="dissimilarity">The dissimilarity/distance at which the new cluster was found.</param>
        public Cluster(IEnumerable<TInstance> instances, double dissimilarity = 0)
        {
            this.Dissimilarity = dissimilarity;
            var list = instances as List<TInstance> ?? instances.ToList();
            list.Sort();
            this._cluster = list.ToArray();
            this._hashCode = this.ProduceHashCode();
        }

        /// <summary>
        ///     Creates a new <see cref="Cluster{TInstance}" /> which is an exact copy of the given cluster.
        /// </summary>
        /// <param name="cluster">The cluster to be copied into the new cluster.</param>
        public Cluster(Cluster<TInstance> cluster)
        {
            this._cluster = cluster._cluster.ToArray();
            this.Parent1 = cluster.Parent1;
            this.Parent2 = cluster.Parent2;
            this.Dissimilarity = cluster.Dissimilarity;
            this._hashCode = cluster._hashCode;
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets the number of elements in this cluster.
        /// </summary>
        public int Count => this._cluster.Length;

        /// <summary>
        ///     Gets the dissimilarity / distance at which this cluster was found by the clustering algorithm.
        /// </summary>
        public double Dissimilarity { get; }

        /// <summary>
        ///     Gets this cluster's first parent, if the cluster was formed by joining two existing clusters. Otherwise returns
        ///     <c>null</c>.
        /// </summary>
        public Cluster<TInstance> Parent1 { get; }

        /// <summary>
        ///     Gets this cluster's second parent, if the cluster was formed by joining two existing clusters. Otherwise returns
        ///     <c>null</c>.
        /// </summary>
        public Cluster<TInstance> Parent2 { get; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            return !(other is null) &&
                   (ReferenceEquals(this, other) ||
                    other.GetType() == this.GetType() && this.Equals((Cluster<TInstance>) other));
        }

        /// <inheritdoc />
        public override int GetHashCode() => this._hashCode;

        /// <summary>
        ///     Gets a string representing this cluster in the form (item1;item2;...;itemN).
        /// </summary>
        /// <returns>A string representing the cluster.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("(");
            foreach (var instance in this._cluster)
                sb.Append($"{instance};");
            if (this._cluster.Length > 0) sb.Remove(sb.Length - 1, 1);
            sb.Append(")");
            return sb.ToString();
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Creates a new <see cref="Cluster{TInstance}" /> which is an exact copy of this cluster.
        /// </summary>
        /// <returns>A new <see cref="Cluster{TInstance}" /> which is an exact copy of this cluster.</returns>
        public Cluster<TInstance> Clone() => new Cluster<TInstance>(this);

        /// <summary>
        ///     Checks whether this cluster contains the given item.
        /// </summary>
        /// <param name="item">The item whose presence in the cluster we want to check.</param>
        /// <returns><c>true</c> if the cluster contains the given item, <c>false</c> otherwise.</returns>
        public bool Contains(TInstance item) => this._cluster.Contains(item);

        /// <summary>
        ///     Compares this cluster with another cluster instance. Comparison is performed by count (number of items) first, then
        ///     by string representation of the items.
        /// </summary>
        /// <param name="other">The cluster to compare to.</param>
        /// <returns>
        ///     <c>-1</c> if <paramref name="other" /> is <c>null</c>, the result of <see cref="Count" /> comparison between
        ///     the clusters, or the result of <c>string.CompareOrdinal"</c> if the clusters have the same count.
        /// </returns>
        public int CompareTo(Cluster<TInstance> other)
        {
            // compares by count first, then by string representation of the elements
            if (other == null) return -1;
            var countCompare = this._cluster.Length.CompareTo(other._cluster.Length);
            return countCompare == 0 ? string.CompareOrdinal(this.ToString(), other.ToString()) : countCompare;
        }

        /// <inheritdoc />
        public IEnumerator<TInstance> GetEnumerator() => ((IEnumerable<TInstance>) this._cluster).GetEnumerator();

        /// <summary>
        ///     Checks whether this cluster is equal to another. Equality between cluster occurs when they are the same object or
        ///     when the clusters contain the same elements, were created based on the same parent clusters and have the same
        ///     associated dissimilarity.
        /// </summary>
        /// <param name="other">The other cluster to verify equality.</param>
        /// <returns><c>true</c> if the clusters are equal, <c>false</c> otherwise.</returns>
        public bool Equals(Cluster<TInstance> other)
        {
            return !(other is null) &&
                   (ReferenceEquals(this, other) ||
                    this._hashCode == other._hashCode &&
                    Math.Abs(this.Dissimilarity - other.Dissimilarity) < MIN_DISSIMILARITY &&
                    (this.Parent1 != null && this.Parent2 != null &&
                     Equals(this.Parent1, other.Parent1) && Equals(this.Parent2, other.Parent2) ||
                     this._cluster.SequenceEqual(other._cluster)));
        }

        #endregion

        #region Private & Protected Methods

        private int ProduceHashCode()
        {
            unchecked
            {
                var hashCode = this.Dissimilarity.GetHashCode();
                foreach (var instance in this) hashCode += (hashCode * 397) ^ instance.GetHashCode();
                return hashCode;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion
    }
}