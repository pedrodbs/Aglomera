// ------------------------------------------
// <copyright file="Extensions.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes
//    Last updated: 2018/01/19
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Agnes.Evaluation.Internal;

namespace Agnes
{
    /// <summary>
    ///     Contains several extension utility methods.
    /// </summary>
    public static class Extensions
    {
        #region Public Methods

        /// <summary>
        ///     Evaluates all given <see cref="ClusterSet{TInstance}" />s according to the given
        ///     <see cref="IInternalEvaluationCriterion{TInstance}" />. The maximum number of clusters allowed in a cluster-set for
        ///     it to be evaluated corresponds to sqrt(N), where N is the total number of instances clustered.
        /// </summary>
        /// <param name="clustering">The cluster-sets to be evaluated.</param>
        /// <param name="criterion">The criterion used to evaluate the cluster-sets.</param>
        /// <returns>A list containing the evaluations for each cluster-set.</returns>
        /// <typeparam name="TInstance">The type of instance considered.</typeparam>
        public static ICollection<ClusterSetEvaluation<TInstance>> EvaluateClustering<TInstance>(
            this ClusteringResult<TInstance> clustering, IInternalEvaluationCriterion<TInstance> criterion) =>
            EvaluateClustering(clustering, criterion, (uint) Math.Sqrt(clustering.Count));

        /// <summary>
        ///     Evaluates all given <see cref="ClusterSet{TInstance}" />s according to the given
        ///     <see cref="IInternalEvaluationCriterion{TInstance}" />.
        /// </summary>
        /// <param name="clustering">The cluster-sets to be evaluated.</param>
        /// <param name="criterion">The criterion used to evaluate the cluster-sets.</param>
        /// <param name="maxClusters">The maximum number of clusters allowed for a cluster-set for it to be evaluated.</param>
        /// <returns>A list containing the evaluations for each cluster-set.</returns>
        /// <typeparam name="TInstance">The type of instance considered.</typeparam>
        public static ICollection<ClusterSetEvaluation<TInstance>> EvaluateClustering<TInstance>(
            this ClusteringResult<TInstance> clustering, IInternalEvaluationCriterion<TInstance> criterion,
            uint maxClusters)
        {
            var evals = new List<ClusterSetEvaluation<TInstance>>();
            foreach (var clusterSet in clustering.Reverse())
            {
                if (clusterSet.Count < 2 || clusterSet.Count > maxClusters) continue;
                var eval = criterion.Evaluate(clusterSet);
                evals.Add(new ClusterSetEvaluation<TInstance>(clusterSet, eval));
            }
            return evals;
        }

        /// <summary>
        ///     Returns the medoid of a given <see cref="Cluster{TInstance}" />, i.e., a representative object whose dissimilarity
        ///     to all the instances in the cluster is minimal.
        /// </summary>
        /// <remarks>
        ///     "Medoids are representative objects of a data set or a cluster with a data set whose average dissimilarity to all
        ///     the objects in the cluster is minimal. Medoids are similar in concept to means or centroids, but medoids are
        ///     always restricted to be members of the data set. Medoids are most commonly used on data when a mean or centroid
        ///     cannot be defined, such as graphs. They are also used in contexts where the centroid is not representative of the
        ///     dataset like in images and 3-D trajectories and gene expression (where while the data is sparse the medoid need
        ///     not be). These are also of interest while wanting to find a representative using some distance other than squared
        ///     euclidean distance (for instance in movie-ratings)."
        ///     <see href="https://en.wikipedia.org/wiki/Medoid" />
        /// </remarks>
        /// <param name="cluster">The cluster whose medoid we want to retrieve.</param>
        /// <param name="metric">
        ///     The dissimilarity metric used to compare elements in the cluster, i.e., to calculate the distance between them.
        /// </param>
        /// <returns>The medoid of the given cluster. If the cluster has two elements, it returns the first element of the cluster.</returns>
        /// <typeparam name="TInstance">The type of instance considered.</typeparam>
        public static TInstance GetMedoid<TInstance>(
            this Cluster<TInstance> cluster, IDissimilarityMetric<TInstance> metric)
        {
            // if count is 1 or 2 return the first elem
            if (cluster.Count < 3) return cluster.First();

            // stores sum of all distances to each point
            var sumDists = new double[cluster.Count];
            var clusterList = cluster.ToList();
            for (var i = 0; i < clusterList.Count; i++)
            for (var j = i + 1; j < clusterList.Count; j++)
            {
                var dist = metric.Calculate(clusterList[i], clusterList[j]);
                sumDists[i] += dist;
                sumDists[j] += dist;
            }

            // selects elem index minimizing the mean distance (the medoid)
            var minSumDist = double.MaxValue;
            var minSumIdx = -1;
            for (var i = 0; i < clusterList.Count; i++)
                if (sumDists[i] < minSumDist)
                {
                    minSumDist = sumDists[i];
                    minSumIdx = i;
                }
            return clusterList[minSumIdx];
        }

        #endregion
    }
}