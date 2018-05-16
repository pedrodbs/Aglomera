// ------------------------------------------
// <copyright file="CachedDissimilarityMetric.cs" company="Pedro Sequeira">
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
//    Last updated: 05/14/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Aglomera.Linkage;

namespace Aglomera
{
    /// <summary>
    ///     Represents a cache to store dissimilarities between all instances of a known set, as dictated by a base
    ///     <see cref="IDissimilarityMetric{TInstance}" />.
    /// </summary>
    /// <remarks>
    ///     This class is useful to use during the execution of <see cref="AgglomerativeClusteringAlgorithm{TInstance}" /> as many
    ///     <see cref="ILinkageCriterion{TInstance}" /> classes rely on pair-wise dissimilarities between the
    ///     instances.
    ///     In that sense, the set of instances has to be known beforehand and must not change and no verification is done in
    ///     <see cref="Calculate" />.
    ///     This means that if cluster centroids are used to measure dissimilarities, they have to be included in the original
    ///     set, otherwise the value will not be present in the cache.
    /// </remarks>
    /// <typeparam name="TInstance"></typeparam>
    public class CachedDissimilarityMetric<TInstance> : IDissimilarityMetric<TInstance>, IDisposable
    {
        #region Fields

        private readonly IDissimilarityMetric<TInstance> _dissimilarityMeasure;
        private readonly IDictionary<TInstance, int> _elementIdxs = new Dictionary<TInstance, int>();
        private double[][] _dissimilarities;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="CachedDissimilarityMetric{TInstance}" /> according to the given base dissimilarity metric
        ///     and the known set of instances.
        /// </summary>
        /// <param name="dissimilarityMeasure">The metric to be used to cache the dissimilarities between all instances.</param>
        /// <param name="allInstances">The set of instances for which to calculate the pair-wise dissimilarities.</param>
        public CachedDissimilarityMetric(
            IDissimilarityMetric<TInstance> dissimilarityMeasure, ISet<TInstance> allInstances)
        {
            this._dissimilarityMeasure = dissimilarityMeasure;

            // registers instances' indexes
            var instances = allInstances.ToArray();
            for (var i = 0; i < instances.Length; i++)
                this._elementIdxs.Add(instances[i], i);

            // initializes cache with distances between all instances
            this._dissimilarities = new double[instances.Length][];
            for (var i = 0; i < allInstances.Count; i++)
            for (var j = i; j < allInstances.Count; j++)
            {
                if (i == 0) this._dissimilarities[j] = new double[instances.Length];
                this.Store(i, j, instances);
            }
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void Dispose()
        {
            this._elementIdxs.Clear();
            this._dissimilarities = null;
        }

        /// <inheritdoc />
        public double Calculate(TInstance instance1, TInstance instance2) =>
            this._dissimilarities[this._elementIdxs[instance1]][this._elementIdxs[instance2]];

        #endregion

        #region Private & Protected Methods

        private void Store(int i, int j, IList<TInstance> elementList)
        {
            var d = this._dissimilarityMeasure.Calculate(elementList[i], elementList[j]);
            this._dissimilarities[i][j] = this._dissimilarities[j][i] = d;
        }

        #endregion
    }
}