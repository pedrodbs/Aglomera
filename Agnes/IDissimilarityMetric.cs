// ------------------------------------------
// <copyright file="IDissimilarityMetric.cs" company="Pedro Sequeira">
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

namespace Agnes
{
    public interface IDissimilarityMetric<TInstance> where TInstance : IComparable<TInstance>
    {
        #region Public Methods

        double Calculate(TInstance instance1, TInstance instance2);

        #endregion
    }
}