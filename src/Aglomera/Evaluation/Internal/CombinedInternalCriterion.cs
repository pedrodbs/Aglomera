// ------------------------------------------
// <copyright file="CombinedInternalCriterion.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Grupo
//    Last updated: 2018/01/18
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Aglomera.Evaluation.Internal
{
    /// <summary>
    ///     Implements an internal clustering evaluation criterion as a combination (weighted average) of other
    ///     <see cref="IInternalEvaluationCriterion{TInstance}" />.
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public class CombinedInternalCriterion<TInstance> : IInternalEvaluationCriterion<TInstance> where TInstance : IComparable<TInstance>

    {
        #region Fields

        private readonly IDictionary<IInternalEvaluationCriterion<TInstance>, double> _criteria;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="CombinedInternalCriterion{TInstance}" /> according to the given criteria and respective
        ///     weights.
        /// </summary>
        /// <param name="criteria">
        ///     A dictionary containing the several criteria to be used and how should they be combined, i.e., their associated
        ///     weights.
        /// </param>
        public CombinedInternalCriterion(IDictionary<IInternalEvaluationCriterion<TInstance>, double> criteria)
        {
            this._criteria = criteria;
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     This class does not have a dissimilarity metric associated, so <c>null</c> is always returned.
        /// </summary>
        public IDissimilarityMetric<TInstance> DissimilarityMetric => null;

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public double Evaluate(ClusterSet<TInstance> clusterSet) =>
            this._criteria.Sum(criterion => criterion.Key.Evaluate(clusterSet) * criterion.Value);

        #endregion
    }
}