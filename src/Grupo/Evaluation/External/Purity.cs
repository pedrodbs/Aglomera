// ------------------------------------------
// <copyright file="Purity.cs" company="Pedro Sequeira">
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

using System.Collections.Generic;

namespace Grupo.Evaluation.External
{
    /// <summary>
    ///     Evaluates the given partition according to the purity criterion, where each cluster is assigned to its most
    ///     frequent class, and then the accuracy of this assignment is measured by counting the number of correctly assigned
    ///     instances and dividing by the total number of instances.
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    /// <typeparam name="TClass">The type of class considered.</typeparam>
    /// <remarks>
    ///     see: <a href="https://nlp.stanford.edu/IR-book/html/htmledition/evaluation-of-clustering-1.html" />
    /// </remarks>
    public class Purity<TInstance, TClass> : IExternalEvaluationCriterion<TInstance, TClass>

    {
        #region Public Methods

        public double Evaluate(ClusterSet<TInstance> clusterSet, IDictionary<TInstance, TClass> instanceClasses)
        {
            // for each cluster 
            var classCount = 0d;
            foreach (var cluster in clusterSet)
            {
                // gets the class with the highest frequency
                var clusterClassCounts = new Dictionary<TClass, uint>();
                var maxClassCount = 0u;
                foreach (var idx in cluster)
                {
                    var pointClass = instanceClasses[idx];
                    if (clusterClassCounts.ContainsKey(pointClass))
                        clusterClassCounts[pointClass]++;
                    else clusterClassCounts[pointClass] = 1;
                    if (clusterClassCounts[pointClass] > maxClassCount)
                        maxClassCount = clusterClassCounts[pointClass];
                }

                // add max count to purity
                classCount += maxClassCount;
            }

            // divide by total number of points
            return classCount / instanceClasses.Count;
        }

        #endregion
    }
}