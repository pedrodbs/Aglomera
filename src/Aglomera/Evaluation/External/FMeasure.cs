// ------------------------------------------
// <copyright file="FMeasure.cs" company="Pedro Sequeira">
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
using System.Linq;

namespace Aglomera.Evaluation.External
{
    /// <summary>
    ///     Evaluates the given partition according to the F-measure, i.e., it measures the accuracy of the clustering by
    ///     measuring the percentage of decisions that are correct (true positives + true negatives).
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    /// <typeparam name="TClass">The type of class considered.</typeparam>
    /// <remarks>
    ///     see: <a href="https://nlp.stanford.edu/IR-book/html/htmledition/evaluation-of-clustering-1.html" />
    /// </remarks>
    public class FMeasure<TInstance, TClass> : IExternalEvaluationCriterion<TInstance, TClass>
        where TInstance : IComparable<TInstance>

    {
        #region Constructors

        /// <summary>
        ///     Creates a new instance of <see cref="FMeasure{TInstance,TClass}" /> with the given recall weight.
        /// </summary>
        /// <param name="recallWeight">The weight given to recall in comparison to precision.</param>
        public FMeasure(double recallWeight)
        {
            this.RecallWeight = recallWeight;
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets the weight given to recall in comparison to precision.
        /// </summary>
        public double RecallWeight { get; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public double Evaluate(ClusterSet<TInstance> clusterSet, IDictionary<TInstance, TClass> instanceClasses)
        {
            // counts the positives for each cluster 
            var truePositives = 0L;
            var falseNegatives = 0L;
            var positives = 0L;
            for (var i = 0; i < clusterSet.Count; i++)
            {
                var clusterGroup = clusterSet[i];

                // gets class counts
                var clusterClassCounts = new Dictionary<TClass, int>();
                foreach (var instance in clusterGroup)
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

                // updates positives
                positives += Combinatorics.GetCombinations(clusterGroup.Count, 2);

                // updates true positives (pairs of same class within cluster)
                truePositives += clusterClassCounts.Values
                    .Where(count => count > 1)
                    .Sum(count => Combinatorics.GetCombinations(count, 2));
            }

            var precision = (double) truePositives / positives;
            var recall = (double) truePositives / (truePositives + falseNegatives);

            // returns f-measure
            var weightSquare = this.RecallWeight * this.RecallWeight;
            return (weightSquare + 1) * precision * recall / (weightSquare * precision + recall);
        }

        #endregion
    }
}