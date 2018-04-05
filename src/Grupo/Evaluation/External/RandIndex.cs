// ------------------------------------------
// <copyright file="RandIndex.cs" company="Pedro Sequeira">
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
using System.Linq;

namespace Grupo.Evaluation.External
{
    /// <summary>
    ///     Evaluates the given partition according to the Rand index, i.e., it measures the accuracy of the clustering by
    ///     measuring the percentage of decisions that are correct (true positives + true negatives).
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    /// <typeparam name="TClass">The type of class considered.</typeparam>
    /// <remarks>
    ///     see: <a href="https://nlp.stanford.edu/IR-book/html/htmledition/evaluation-of-clustering-1.html" />
    /// </remarks>
    public class RandIndex<TInstance, TClass> : IExternalEvaluationCriterion<TInstance, TClass>

    {
        #region Public Methods

        public double Evaluate(ClusterSet<TInstance> clusterSet, IDictionary<TInstance, TClass> instanceClasses)
        {
            // counts the positives for each cluster 
            var truePositives = 0L;
            var trueNegatives = 0L;
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

                    // updates true negatives (pairs of diff class in diff clusters)
                    for (var j = i + 1; j < clusterSet.Count; j++)
                        trueNegatives += clusterSet[j]
                            .Count(instance2 => !instanceClass.Equals(instanceClasses[instance2]));
                }

                // updates true positives (pairs of same class within cluster)
                truePositives += clusterClassCounts.Values
                    .Where(count => count > 1)
                    .Sum(count => Combinatorics.GetCombinations(count, 2));
            }

            // returns accuracy
            return (double) (truePositives + trueNegatives) /
                   Combinatorics.GetCombinations(instanceClasses.Count, 2);
        }

        #endregion
    }
}