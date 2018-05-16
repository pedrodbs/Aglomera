// ------------------------------------------
// <copyright file="IExternalEvaluationCriterion.cs" company="Pedro Sequeira">
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
using Aglomera.Linkage;

namespace Aglomera.Evaluation.External
{
    /// <summary>
    ///     Represents an interface for external criteria to evaluate how well the result of
    ///     <see cref="AgglomerativeClusteringAlgorithm{TInstance}" /> matches the classification of instances according to a set of gold
    ///     standard classes. We can think of this as supervised clustering evaluation methods, i.e., external validation
    ///     methods.
    /// </summary>
    /// <remarks>
    ///     These methods are useful for when we have some known partition over the instances and want to evaluate the quality
    ///     of the clustering according to that partition. It can also be used to select the most appropriate
    ///     <see cref="ILinkageCriterion{TInstance}" /> for a given annotated data-set.
    /// </remarks>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    /// <typeparam name="TClass">The type of class considered.</typeparam>
    public interface IExternalEvaluationCriterion<TInstance, TClass> where TInstance : IComparable<TInstance>
    {
        #region Public Methods

        /// <summary>
        ///     Evaluates a given <see cref="ClusterSet{TInstance}" /> partition according to the given class partition.
        /// </summary>
        /// <param name="clusterSet">The clustering partition.</param>
        /// <param name="instanceClasses">The instances' classes.</param>
        /// <returns>The evaluation of the given clustering according to this criterion.</returns>
        double Evaluate(ClusterSet<TInstance> clusterSet, IDictionary<TInstance, TClass> instanceClasses);

        #endregion
    }
}