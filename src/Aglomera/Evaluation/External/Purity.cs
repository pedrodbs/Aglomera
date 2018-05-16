// ------------------------------------------
// <copyright file="Purity.cs" company="Pedro Sequeira">
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

namespace Aglomera.Evaluation.External
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
        where TInstance : IComparable<TInstance>

    {
        #region Public Methods

        /// <inheritdoc />
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