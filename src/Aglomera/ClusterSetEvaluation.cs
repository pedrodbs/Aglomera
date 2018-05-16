// ------------------------------------------
// <copyright file="ClusterSetEvaluation.cs" company="Pedro Sequeira">
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

namespace Aglomera
{
    /// <summary>
    ///     Represents the result of evaluating some <see cref="ClusterSet{TInstance}" /> according to some criterion.
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public struct ClusterSetEvaluation<TInstance> where TInstance : IComparable<TInstance>
    {
        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="ClusterSetEvaluation{TInstance}" />.
        /// </summary>
        /// <param name="clusterSet">The cluster-set that was evaluated.</param>
        /// <param name="evaluationValue">The value of the evaluation.</param>
        public ClusterSetEvaluation(ClusterSet<TInstance> clusterSet, double evaluationValue)
        {
            this.ClusterSet = clusterSet;
            this.EvaluationValue = evaluationValue;
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets the cluster-set that was evaluated.
        /// </summary>
        public ClusterSet<TInstance> ClusterSet { get; }

        /// <summary>
        ///     Gets the value of the evaluation.
        /// </summary>
        public double EvaluationValue { get; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override string ToString() => $"[{this.ClusterSet}][{this.EvaluationValue:0.00}]";

        #endregion
    }
}