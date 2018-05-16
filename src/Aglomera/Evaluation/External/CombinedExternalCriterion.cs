// ------------------------------------------
// <copyright file="CombinedExternalCriterion.cs" company="Pedro Sequeira">
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

namespace Aglomera.Evaluation.External
{
    /// <summary>
    ///     Implements an external clustering evaluation criterion as a combination (weighted average) of other
    ///     <see cref="IExternalEvaluationCriterion{TInstance,TClass}" />.
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    /// <typeparam name="TClass">The type of class considered.</typeparam>
    public class CombinedExternalCriterion<TInstance, TClass> : IExternalEvaluationCriterion<TInstance, TClass>
        where TInstance : IComparable<TInstance>

    {
        #region Fields

        private readonly IDictionary<IExternalEvaluationCriterion<TInstance, TClass>, double> _criteria;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="CombinedExternalCriterion{TInstance,TClass}" /> according to the given criteria and
        ///     respective weights.
        /// </summary>
        /// <param name="criteria">
        ///     A dictionary containing the several criteria to be used and how should they be combined, i.e., their associated
        ///     weights.
        /// </param>
        public CombinedExternalCriterion(IDictionary<IExternalEvaluationCriterion<TInstance, TClass>, double> criteria)
        {
            this._criteria = criteria;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public double Evaluate(ClusterSet<TInstance> clusterSet, IDictionary<TInstance, TClass> instanceClasses) =>
            this._criteria.Sum(
                criterion => criterion.Key.Evaluate(clusterSet, instanceClasses) * criterion.Value);

        #endregion
    }
}