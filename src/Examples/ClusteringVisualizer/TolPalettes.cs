// ------------------------------------------
// <copyright file="TolPalettes.cs" company="Pedro Sequeira">
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
//    Project: ClusteringVisualizer
//    Last updated: 05/24/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using System.Drawing;
using MathNet.Numerics;

namespace ClusteringVisualizer
{
    /// <summary>
    ///     Allows the generation of <see cref="Color" /> palettes according to Paul Tol's color schemes.
    ///     <see href="http://www.sron.nl/~pault/colourschemes.pdf" />
    /// </summary>
    public static class TolPalettes
    {
        #region Public Methods

        /// <summary>
        ///     Creates a color palette according to Paul Tol's diverging (blue-yellow-red) colors scheme.
        ///     <see href="http://www.sron.nl/~pault/colourschemes.pdf" />, Fig. 8 and Eq. 2.
        /// </summary>
        /// <param name="numColors">The number of colors to be generated in the palette.</param>
        /// <returns>A <see cref="Color" /> array with the given number of colors.</returns>
        public static Color[] CreateTolDivPalette(int numColors)
        {
            var palette = new List<Color>();
            var d = 1d / (numColors - 1);
            for (var i = 0; i < numColors; i++)
            {
                var x = d * i;
                var x2 = x * x;
                var x3 = x2 * x;
                var x4 = x3 * x;
                var x5 = x4 * x;
                var r = (byte) ((0.237 - 2.13 * x + 26.92 * x2 - 65.5 * x3 + 63.5 * x4 - 22.36 * x5) * 255);
                var gComp = (0.572 + 1.524 * x - 1.811 * x2) / (1 - 0.291 * x + 0.1574 * x2);
                var g = (byte) (gComp * gComp * 255);
                var b = (byte) (1d / (1.579 - 4.03 * x + 12.92 * x2 - 31.4 * x3 + 48.6 * x4 - 23.36 * x5) * 255);
                palette.Add(Color.FromArgb(r, g, b));
            }

            return palette.ToArray();
        }

        /// <summary>
        ///     Creates a color palette according to Paul Tol's rainbow colors scheme.
        ///     <see href="http://www.sron.nl/~pault/colourschemes.pdf" />, Fig. 13 and Eq. 3.
        /// </summary>
        /// <param name="numColors">The number of colors to be generated in the palette.</param>
        /// <returns>A <see cref="Color" /> array with the given number of colors.</returns>
        public static Color[] CreateTolRainbowPalette(int numColors)
        {
            var palette = new List<Color>();
            var d = 1d / (numColors - 1);
            for (var i = 0; i < numColors; i++)
            {
                var x = d * i;
                var x2 = x * x;
                var x3 = x2 * x;
                var x4 = x3 * x;
                var x5 = x4 * x;
                var x6 = x5 * x;
                var r = (byte) ((0.472 - 0.567 * x + 4.05 * x2) / (1 + 8.72 * x - 19.17 * x2 + 14.1 * x3) * 255);
                var g = (byte) (
                    (0.108932 - 1.22635 * x + 27.284 * x2 - 98.577 * x3 + 163.3 * x4 - 131.395 * x5 + 40.634 * x6)
                    * 255);
                var b = (byte) (1d / (1.97 + 3.54 * x - 68.5 * x2 + 243 * x3 - 297 * x4 + 125 * x5) * 255);
                palette.Add(Color.FromArgb(r, g, b));
            }

            return palette.ToArray();
        }

        /// <summary>
        ///     Creates a color palette according to Paul Tol's sequential (yellow-orange-brown) colors scheme.
        ///     <see href="http://www.sron.nl/~pault/colourschemes.pdf" />, Fig. 7 and Eq. 1.
        /// </summary>
        /// <param name="numColors">The number of colors to be generated in the palette.</param>
        /// <returns>A <see cref="Color" /> array with the given number of colors.</returns>
        public static Color[] CreateTolSeqPalette(int numColors)
        {
            var palette = new List<Color>();
            var d = 1d / (numColors - 1);
            for (var i = 0; i < numColors; i++)
            {
                var x = d * i;
                var r = (byte) ((1 - 0.392 * (1 + SpecialFunctions.Erf((x - 0.869) / 0.255))) * 255);
                var g = (byte) ((1.021 - 0.456 * (1 + SpecialFunctions.Erf((x - 0.527) / 0.376))) * 255);
                var b = (byte) ((1 - 0.493 * (1 + SpecialFunctions.Erf((x - 0.272) / 0.309))) * 255);
                palette.Add(Color.FromArgb(r, g, b));
            }

            return palette.ToArray();
        }

        #endregion
    }
}