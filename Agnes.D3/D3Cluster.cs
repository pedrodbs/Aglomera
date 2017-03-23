// ------------------------------------------
// <copyright file="D3Cluster.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes.D3
//    Last updated: 2017/03/14
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Agnes.D3
{
    public class D3Cluster<TInstance> : IEquatable<D3Cluster<TInstance>> where TInstance : IComparable<TInstance>
    {
        #region Fields

        private double _dissimilarity;

        #endregion

        #region Properties & Indexers

        [JsonProperty("c")]
        public List<D3Cluster<TInstance>> Children { get; private set; }

        [JsonIgnore]
        public Cluster<TInstance> Cluster { get; }

        [JsonProperty("d")]
        public double Dissimilarity
        {
            get { return this._dissimilarity; }
            set { this._dissimilarity = Math.Round(value, 2); }
        }

        [JsonProperty("n")]
        public string Name { get; set; }

        #endregion

        #region Constructors

        public D3Cluster(Cluster<TInstance> cluster, double dissimilarity)
        {
            this.Cluster = cluster;
            this.Dissimilarity = dissimilarity;
            this.Children = new List<D3Cluster<TInstance>>();
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && this.Equals((D3Cluster<TInstance>) obj);
        }

        public override int GetHashCode()
        {
            return this.Cluster.GetHashCode();
        }

        public override string ToString()
        {
            return this.Cluster.ToString();
        }

        #endregion

        #region Public Methods

        public static bool operator ==(D3Cluster<TInstance> left, D3Cluster<TInstance> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(D3Cluster<TInstance> left, D3Cluster<TInstance> right)
        {
            return !Equals(left, right);
        }

        public bool Equals(D3Cluster<TInstance> other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || this.Cluster.Equals(other.Cluster);
        }

        #endregion
    }
}