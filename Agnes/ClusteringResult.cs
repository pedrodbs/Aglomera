// ------------------------------------------
// <copyright file="ClusteringResult.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes
//    Last updated: 2017/04/06
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;

namespace Agnes
{
    public class ClusteringResult<TInstance> : IEnumerable<ClusterSet<TInstance>>
        where TInstance : IComparable<TInstance>
    {
        #region Fields

        private readonly ClusterSet<TInstance>[] _clusterSets;

        #endregion

        #region Properties & Indexers

        public int Count => this._clusterSets.Length;

        public ClusterSet<TInstance> this[int index]
        {
            get { return this._clusterSets[index]; }
            set { this._clusterSets[index] = value; }
        }

        public Cluster<TInstance> SingleCluster => this._clusterSets[this._clusterSets.Length - 1][0];

        #endregion

        #region Constructors

        public ClusteringResult(int size)
        {
            this._clusterSets = new ClusterSet<TInstance>[size];
        }

        #endregion

        #region Public Methods

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