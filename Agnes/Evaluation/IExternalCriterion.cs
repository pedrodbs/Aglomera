// ------------------------------------------
// <copyright file="IExternalCriterion.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes
//    Last updated: 2017/07/25
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;

namespace Agnes.Evaluation
{
    /// <summary>
    ///     Represent an interface for external criteria to evaluate how well some particular clustering matches the
    ///     classification of instances according to a set of gold standard classes. Useful for when we have some known
    ///     partition over the instances and want to evaluate the quality of the clustering according to that partition.
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    /// <typeparam name="TClass">The type of class considered.</typeparam>
    public interface IExternalCriterion<TInstance, TClass> where TInstance : IComparable<TInstance>
    {
        #region Public Methods

        /// <summary>
        ///     Evaluates a given <see cref="ClusterSet{TInstance}"/> partition according to the given class partition.
        /// </summary>
        /// <param name="clusterSet">The clustering partition.</param>
        /// <param name="instanceClasses">The instances' classes.</param>
        /// <returns>The evaluation of the given clustering according to this criterion.</returns>
        double Evaluate(ClusterSet<TInstance> clusterSet, IDictionary<TInstance, TClass> instanceClasses);

        #endregion
    }
}