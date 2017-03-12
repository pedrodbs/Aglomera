// ------------------------------------------
// <copyright file="Cluster.cs" company="Pedro Sequeira">
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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Agnes
{
    public class Cluster<TInstance> : IEnumerable<TInstance>, IDisposable, IEquatable<Cluster<TInstance>>
        where TInstance : IEquatable<TInstance>
    {
        #region Static Fields & Constants

        public static readonly Cluster<TInstance> EmptySet = new Cluster<TInstance>(new List<TInstance>());

        #endregion

        #region Fields

        private readonly int _hashCode;
        private SortedSet<TInstance> _cluster;

        #endregion

        #region Properties & Indexers

        public IComparer<TInstance> Comparer
        {
            get { return this._cluster.Comparer; }
            set
            {
                if (!this._cluster.Comparer.Equals(value))
                    this._cluster = new SortedSet<TInstance>(this._cluster, value);
            }
        }

        public uint Count => (uint) this._cluster.Count;

        #endregion

        #region Constructors

        public Cluster(TInstance instance) : this(new List<TInstance> {instance}, Comparer<TInstance>.Default)
        {
        }

        public Cluster(TInstance instance, IComparer<TInstance> instanceComparer)
            : this(new List<TInstance> {instance}, instanceComparer)
        {
        }

        public Cluster(IEnumerable<TInstance> instances) : this(instances, Comparer<TInstance>.Default)
        {
        }

        public Cluster(IEnumerable<TInstance> instances, IComparer<TInstance> instanceComparer)
        {
            this._cluster = new SortedSet<TInstance>(instances, instanceComparer ?? Comparer<TInstance>.Default);
            this._hashCode = this.ProduceHashCode();
        }

        public Cluster(Cluster<TInstance> cluster)
        {
            this._cluster = new SortedSet<TInstance>(cluster, cluster.Comparer);
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) &&
                   (ReferenceEquals(this, obj) ||
                    obj.GetType() == this.GetType() && this.Equals((Cluster<TInstance>) obj));
        }

        public override int GetHashCode() => this._hashCode;

        public override string ToString()
        {
            var sb = new StringBuilder("(");
            foreach (var instance in this._cluster)
                sb.Append($"{instance},");
            if (this._cluster.Count > 0) sb.Remove(sb.Length - 1, 1);
            sb.Append(")");
            return sb.ToString();
        }

        #endregion

        #region Public Methods

        public void Clear()
        {
            this._cluster.Clear();
        }

        public Cluster<TInstance> Clone()
        {
            return new Cluster<TInstance>(this);
        }

        public bool Contains(TInstance item)
        {
            return this._cluster.Contains(item);
        }

        public Cluster<TInstance> IntersectWith(IEnumerable<TInstance> other)
        {
            var itemSet = this.Clone();
            itemSet._cluster.IntersectWith(other);
            return itemSet;
        }

        public bool IsProperSubsetOf(IEnumerable<TInstance> other)
        {
            return this._cluster.IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<TInstance> other)
        {
            return this._cluster.IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<TInstance> other)
        {
            return this._cluster.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<TInstance> other)
        {
            return this._cluster.IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<TInstance> other)
        {
            return this._cluster.Overlaps(other);
        }

        public Cluster<TInstance> UnionWith(IEnumerable<TInstance> other)
        {
            var cluster = this.Clone();
            cluster._cluster.UnionWith(other);
            return cluster;
        }

        public IEnumerator<TInstance> GetEnumerator()
        {
            return this._cluster.GetEnumerator();
        }

        public bool Equals(Cluster<TInstance> other)
        {
            return !ReferenceEquals(null, other) && (ReferenceEquals(this, other) || this._cluster.SetEquals(other));
        }

        #endregion

        #region Private & Protected Methods

        private int ProduceHashCode()
        {
            unchecked
            {
                var hash = 0;
                foreach (var instance in this) hash += instance.GetHashCode();
                return 31 * hash + this.Count.GetHashCode();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IDisposable Support

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed) return;
            if (disposing) this.Clear();
            this._disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        #endregion
    }
}