// ------------------------------------------
// <copyright file="MinimumEnergyClustering.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes
//    Last updated: 2017/03/14
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
    ///     Implements the by minimum (energy) E-distance method.
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class MinimumEnergyClustering<TInstance> : ILinkageCriterion<TInstance>
        where TInstance : IComparable<TInstance>
    {
        #region Fields

        private readonly IDissimilarityMetric<TInstance> _metric;

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets or sets the distance exponent.
        /// </summary>
        public double DistanceExponent { get; set; } = 2;

        #endregion

        #region Constructors

        public MinimumEnergyClustering(IDissimilarityMetric<TInstance> metric)
        {
            this._metric = metric;
        }

        #endregion

        #region Public Methods

        public double Calculate(Cluster<TInstance> cluster1, Cluster<TInstance> cluster2)
        {
            var c1c2Energy = this.GetEnergy(cluster1, cluster2);
            var c1c1Energy = this.GetEnergy(cluster1, cluster1);
            var c2c2Energy = this.GetEnergy(cluster2, cluster2);
            return (double) (cluster1.Count * cluster2.Count) / (cluster1.Count + cluster2.Count) *
                   (2 * c1c2Energy - c1c1Energy - c2c2Energy);
        }

        #endregion

        #region Private & Protected Methods

        private double GetEnergy(Cluster<TInstance> cluster1, Cluster<TInstance> cluster2)
        {
            var sum = cluster1.Sum(instance1 => cluster2.Sum(instance2 => this._metric.Calculate(instance1, instance2)));
            var dist = Math.Pow(sum, this.DistanceExponent);
            return dist / (cluster1.Count * cluster2.Count);
        }

        #endregion
    }
}