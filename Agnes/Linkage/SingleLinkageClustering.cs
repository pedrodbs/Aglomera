// ------------------------------------------
// <copyright file="SingleLinkageClustering.cs" company="Pedro Sequeira">
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
using System.Linq;

namespace Agnes.Linkage
{
    /// <summary>
    ///     Implements the minimum or single-linkage clustering method.
    /// </summary>
    /// <typeparam name="TInstance">The type of instace considered.</typeparam>
    public class SingleLinkageClustering<TInstance> : ILinkageCriterion<TInstance>
        where TInstance : IEquatable<TInstance>
    {
        #region Fields

        private readonly IDissimilarityMetric<TInstance> _metric;

        #endregion

        #region Constructors

        public SingleLinkageClustering(IDissimilarityMetric<TInstance> metric)
        {
            this._metric = metric;
        }

        #endregion

        #region Public Methods

        public double Calculate(Cluster<TInstance> cluster1, Cluster<TInstance> cluster2)
        {
            return cluster1.Min(instance1 => cluster2.Min(instance2 => this._metric.Calculate(instance1, instance2)));
        }

        #endregion
    }
}