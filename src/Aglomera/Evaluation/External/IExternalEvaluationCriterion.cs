// ------------------------------------------
// <copyright file="IExternalEvaluationCriterion.cs" company="Pedro Sequeira">
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
using Aglomera.Linkage;

namespace Aglomera.Evaluation.External
{
    /// <summary>
    ///     Represents an interface for external criteria to evaluate how well the result of
    ///     <see cref="ClusteringAlgorithm{TInstance}" /> matches the classification of instances according to a set of gold
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