// ------------------------------------------
// <copyright file="AverageLinkageClustering.cs" company="Pedro Sequeira">
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
    ///     Implements the unweighted pair-group average method or UPGMA.
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class AverageLinkageClustering<TInstance> : ILinkageCriterion<TInstance>
        where TInstance : IComparable<TInstance>
    {
        #region Fields

        private readonly IDissimilarityMetric<TInstance> _metric;

        #endregion

        #region Constructors

        public AverageLinkageClustering(IDissimilarityMetric<TInstance> metric)
        {
            this._metric = metric;
        }

        #endregion

        #region Public Methods

        public double Calculate(Cluster<TInstance> cluster1, Cluster<TInstance> cluster2)
        {
            var sum = cluster1.Sum(instance1 => cluster2.Sum(instance2 => this._metric.Calculate(instance1, instance2)));
            return sum / (cluster1.Count * cluster2.Count);
        }

        #endregion
    }
}