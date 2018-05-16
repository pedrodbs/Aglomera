// ------------------------------------------
// <copyright file="Combinatorics.cs" company="Pedro Sequeira">
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

namespace Aglomera
{
    /// <summary>
    ///     A utility class containing combinatorics methods.
    /// </summary>
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
        /// <remarks>See <a href="http://www.mathwords.com/c/combination_formula.htm" />.</remarks>
        public static long GetCombinations(long n, long k)
        {
            if (n == k) return 1;
            return GetPermutations(n, k) / GetFactorial(k);
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
        ///     Returns the number of possible permutations of k elements from a set of n (without repetition).
        /// </summary>
        /// <param name="n">The number of elements in the set.</param>
        /// <param name="k">The number of elements to choose from the set (permutation size).</param>
        /// <returns></returns>
        /// <remarks>See <a href="http://www.mathwords.com/p/permutation_formula.htm" />.</remarks>
        public static long GetPermutations(long n, long k)
        {
            long permutations = 1;
            for (var i = n - k + 1; i <= n; i++)
                permutations *= i;
            return permutations;
        }

        #endregion
    }
}