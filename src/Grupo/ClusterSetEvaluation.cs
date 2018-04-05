// ------------------------------------------
// <copyright file="ClusterSetEvaluation.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Grupo
//    Last updated: 2018/01/19
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

namespace Grupo
{
    /// <summary>
    ///     Represents the result of evaluating some <see cref="ClusterSet{TInstance}" /> according to some criterion.
    /// </summary>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public struct ClusterSetEvaluation<TInstance>
    {
        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="ClusterSetEvaluation{TInstance}" />.
        /// </summary>
        /// <param name="clusterSet">The cluster-set that was evaluated.</param>
        /// <param name="evaluationValue">The value of the evaluation.</param>
        public ClusterSetEvaluation(ClusterSet<TInstance> clusterSet, double evaluationValue)
        {
            this.ClusterSet = clusterSet;
            this.EvaluationValue = evaluationValue;
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets the cluster-set that was evaluated.
        /// </summary>
        public ClusterSet<TInstance> ClusterSet { get; }

        /// <summary>
        ///     Gets the value of the evaluation.
        /// </summary>
        public double EvaluationValue { get; }

        #endregion

        #region Public Methods

        public override string ToString() => $"[{this.ClusterSet}][{this.EvaluationValue:0.00}]";

        #endregion
    }
}