// ------------------------------------------
// <copyright file="Delegates.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes
//    Last updated: 2018/01/18
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

namespace Agnes
{
    /// <summary>
    ///     Represents a delegate for functions calculating the centroids of <see cref="Cluster{TInstance}" /> objects, i.e.,
    ///     they calculate the representative element of a given cluster.
    /// </summary>
    /// <param name="cluster">The cluster whose representative we want to retrieve.</param>
    /// <returns>The representative of the given cluster according to some criterion defined by this function.</returns>
    /// <typeparam name="TInstance">The type of instance considered.</typeparam>
    public delegate TInstance CentroidFunction<TInstance>(Cluster<TInstance> cluster);
}