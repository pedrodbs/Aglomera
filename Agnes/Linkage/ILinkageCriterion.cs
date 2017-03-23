// ------------------------------------------
// <copyright file="ILinkageCriterion.cs" company="Pedro Sequeira">
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

namespace Agnes.Linkage
{
    public interface ILinkageCriterion<TInstance> where TInstance : IComparable<TInstance>
    {
        #region Public Methods

        double Calculate(Cluster<TInstance> cluster1, Cluster<TInstance> cluster2);

        #endregion
    }
}