// ------------------------------------------
// <copyright file="Cluster.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes
//    Last updated: 2018/01/18
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agnes
{
    /// <summary>
    ///     Represents a set of <see cref="TInstance" /> elements arranged in a hierarchical form.
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class Cluster<TInstance> :
        IEnumerable<TInstance>, IEquatable<Cluster<TInstance>>, IComparable<Cluster<TInstance>>
    {
        #region Static Fields & Constants

        /// <summary>
        ///     Gets an empty cluster.
        /// </summary>
        public static readonly Cluster<TInstance> EmptySet = new Cluster<TInstance>(new List<TInstance>());

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
            this.Parent1 = parent1;
            this.Parent2 = parent2;
            this.Dissimilarity = dissimilarity;
            this._cluster = new TInstance[parent1._cluster.Length + parent2._cluster.Length];
            parent1._cluster.CopyTo(this._cluster, 0);
            parent2._cluster.CopyTo(this._cluster, parent1._cluster.Length);
            this._hashCode = this.ProduceHashCode();
        }

        /// <summary>
        ///     Creates a new <see cref="Cluster{TInstance}" /> with a single <see cref="TInstance" /> element.
        /// </summary>
        /// <param name="instance">The single element in the new cluster.</param>
        /// <param name="dissimilarity">The dissimilarity/distance at which the new cluster was found.</param>
        public Cluster(TInstance instance, double dissimilarity = 0) : this(new[] {instance}, dissimilarity)
        {
        }

        /// <summary>
        ///     Creates a new <see cref="Cluster{TInstance}" /> with the given <see cref="TInstance" /> elements.
        /// </summary>
        /// <param name="instances">The elements in the new cluster.</param>
        /// <param name="dissimilarity">The dissimilarity/distance at which the new cluster was found.</param>
        public Cluster(IEnumerable<TInstance> instances, double dissimilarity = 0)
        {
            this.Dissimilarity = dissimilarity;
            this._cluster = instances as TInstance[] ?? instances.ToArray();
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

        public override bool Equals(object obj)
        {
            return !(obj is null) &&
                   (ReferenceEquals(this, obj) ||
                    obj.GetType() == this.GetType() && this.Equals((Cluster<TInstance>) obj));
        }

        public override int GetHashCode() => this._hashCode;

        public override string ToString()
        {
            var sb = new StringBuilder("(");
            foreach (var instance in this._cluster)
                sb.Append($"{instance},");
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
        public Cluster<TInstance> Clone()
        {
            return new Cluster<TInstance>(this);
        }

        /// <summary>
        ///     Checks whether this cluster contains the given item.
        /// </summary>
        /// <param name="item">The item whose presence in the cluster we want to check.</param>
        /// <returns><c>true</c> if the cluster contains the given item, <c>false</c> otherwise.</returns>
        public bool Contains(TInstance item) => this._cluster.Contains(item);

        public int CompareTo(Cluster<TInstance> other)
        {
            // compares by count first, then by string representation of the elements
            if (other == null) return -1;
            var countCompare = this._cluster.Length.CompareTo(other._cluster.Length);
            return countCompare == 0 ? string.CompareOrdinal(this.ToString(), other.ToString()) : countCompare;
        }

        public IEnumerator<TInstance> GetEnumerator() => ((IEnumerable<TInstance>) this._cluster).GetEnumerator();

        public bool Equals(Cluster<TInstance> other)
        {
            return !(other is null) &&
                   (ReferenceEquals(this, other) ||
                    this._hashCode == other._hashCode &&
                    this.Dissimilarity.Equals(other.Dissimilarity) &&
                    (this.Parent1 != null && this.Parent2 != null &&
                     Equals(this.Parent1, other.Parent1) && Equals(this.Parent2, other.Parent2) ||
                     new HashSet<TInstance>(this._cluster).SetEquals(other)));
        }

        #endregion

        #region Private & Protected Methods

        private int ProduceHashCode()
        {
            unchecked
            {
                var hashCode = this.Dissimilarity.GetHashCode();
                if (this.Parent1 != null)
                {
                    hashCode = (hashCode * 397) ^ this.Parent1.GetHashCode();
                    hashCode = (hashCode * 397) ^ this.Parent2.GetHashCode();
                }
                else
                {
                    foreach (var instance in this) hashCode += (hashCode * 397) ^ instance.GetHashCode();
                }
                return hashCode;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}