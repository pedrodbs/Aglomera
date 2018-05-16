// ------------------------------------------
// <copyright file="NormalizedMutualInformation.cs" company="Pedro Sequeira">
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
    ///     Evaluates the given partition according to the normalized mutual information criterion that measures the amount of
    ///     information by which our knowledge about the classes increases when we are told what the clusters are.
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    /// <typeparam name="TClass">The type of class considered.</typeparam>
    /// <remarks>
    ///     see: <a href="https://nlp.stanford.edu/IR-book/html/htmledition/evaluation-of-clustering-1.html" />
    /// </remarks>
    public class NormalizedMutualInformation<TInstance, TClass> : IExternalEvaluationCriterion<TInstance, TClass>
        where TInstance : IComparable<TInstance>

    {
        #region Public Methods

        /// <inheritdoc />
        public double Evaluate(ClusterSet<TInstance> clusterSet, IDictionary<TInstance, TClass> instanceClasses)
        {
            var numPoints = instanceClasses.Count;

            // organizes by class counts
            var classCounts = new Dictionary<TClass, uint>();
            foreach (var pointClass in instanceClasses.Values)
                if (classCounts.ContainsKey(pointClass))
                    classCounts[pointClass]++;
                else
                    classCounts[pointClass] = 1;

            // gets class entropy
            var classEntropy = -classCounts
                .Where(classCount => classCount.Value > 0)
                .Sum(classCount => (double) classCount.Value / numPoints *
                                   Math.Log((double) classCount.Value / numPoints));

            // gets mutual information and cluster entropy
            var mi = 0d;
            var clusterEntropy = 0d;
            foreach (var cluster in clusterSet)
            {
                foreach (var classCount in classCounts)
                {
                    // gets intersection between class and group (num points in group that belong to the class)
                    var clusterClassCount = cluster.Count(idx => instanceClasses[idx].Equals(classCount.Key));

                    // updates mutual information
                    if (clusterClassCount > 0)
                        mi += (double) clusterClassCount / numPoints *
                              Math.Log((double) numPoints * clusterClassCount / (cluster.Count * classCount.Value));
                }

                // updates cluster entropy
                if (cluster.Count > 0)
                    clusterEntropy -= (double) cluster.Count / numPoints *
                                      Math.Log((double) cluster.Count / numPoints);
            }

            return mi / (0.5 * (clusterEntropy + classEntropy));
        }

        #endregion
    }
}