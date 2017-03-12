// ------------------------------------------
// <copyright file="ILinkageCriterion.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: AgnesClusteringTest
//    Last updated: 2017/03/10
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;

namespace Agnes.Linkage
{
    public interface ILinkageCriterion<TInstance> where TInstance : IEquatable<TInstance>
    {
        #region Public Methods

        double Calculate(Cluster<TInstance> cluster1, Cluster<TInstance> cluster2);

        #endregion
    }
}