// ------------------------------------------
// <copyright file="Recall.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes
//    Last updated: 2017/07/26
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Agnes.Evaluation
{
    /// <summary>
    ///     Evaluates the given partition according to the recall criterion, given by the percentage of true positives over
    ///     all relevant cases (true positives + false negatives).
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    /// <typeparam name="TClass">The type of class considered.</typeparam>
    /// <remarks>
    ///     see: <a href="https://nlp.stanford.edu/IR-book/html/htmledition/evaluation-of-clustering-1.html" />
    /// </remarks>
    public class Recall<TInstance, TClass> : IExternalCriterion<TInstance, TClass>
        where TInstance : IComparable<TInstance>
    {
        #region Public Methods

        public double Evaluate(ClusterSet<TInstance> clusterSet, IDictionary<TInstance, TClass> instanceClasses)
        {
            // counts the positives for each cluster 
            var truePositives = 0L;
            var falseNegatives = 0L;
            for (var i = 0; i < clusterSet.Count; i++)
            {
                var cluster = clusterSet[i];

                // gets class counts
                var clusterClassCounts = new Dictionary<TClass, int>();
                foreach (var instance in cluster)
                {
                    var instanceClass = instanceClasses[instance];
                    if (clusterClassCounts.ContainsKey(instanceClass))
                        clusterClassCounts[instanceClass]++;
                    else clusterClassCounts[instanceClass] = 1;

                    // updates false negatives (pairs of same class in diff clusters)
                    for (var j = i + 1; j < clusterSet.Count; j++)
                        falseNegatives += clusterSet[j]
                            .Count(instance2 => instanceClass.Equals(instanceClasses[instance2]));
                }

                // updates true positives (pairs of same class within cluster)
                truePositives += clusterClassCounts.Values
                    .Where(count => count > 1)
                    .Sum(count => Combinatorics.GetCombinations(count, 2));
            }

            // returns recall
            return (double) truePositives / (truePositives + falseNegatives);
        }

        #endregion
    }
}