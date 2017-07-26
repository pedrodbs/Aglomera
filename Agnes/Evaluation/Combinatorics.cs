// ------------------------------------------
// <copyright file="Combinatorics.cs" company="Pedro Sequeira">
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

namespace Agnes.Evaluation
{
    public static class Combinatorics
    {
        #region Public Methods

        /// <summary>
        ///     Gets the number of possible combinations (without repetition) according to the given number of elements and
        ///     combination size.
        /// </summary>
        /// <param name="n">The number of elements in the set.</param>
        /// <param name="k">The number of elements to choose from the set (combination size).</param>
        /// <returns>The number of possible combinations (without repetition).</returns>
        public static long GetCombinations(long n, long k)
        {
            if (n == k) return 1;
            return GetProduct(n - k + 1, n) / GetFactorial(k);
        }

        /// <summary>
        ///     Computes the factorial function n! of a given integer number > 0.
        /// </summary>
        /// <param name="n">The number whose factorial we want to determine.</param>
        /// <returns>The factorial n! of the given number.</returns>
        public static long GetFactorial(long n)
        {
            var factorial = 1;
            for (var i = 1; i <= n; i++)
                factorial *= i;
            return factorial;
        }

        /// <summary>
        ///     Returns the product of a sequence or partial sequence, according to the given start and end parameters.
        /// </summary>
        /// <param name="start">The starting number of the sequence.</param>
        /// <param name="end">The end number of the sequence.</param>
        /// <returns></returns>
        public static long GetProduct(long start, long end)
        {
            long mult = 1;
            for (var i = start; i <= end; i++)
                mult *= i;
            return mult;
        }

        #endregion
    }
}