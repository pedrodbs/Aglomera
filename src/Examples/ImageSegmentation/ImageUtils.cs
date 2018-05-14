// ------------------------------------------
// <copyright file="ImageUtils.cs" company="Pedro Sequeira">
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
//    Project: ImageSegmentation
//    Last updated: 04/27/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Aglomera;

namespace ImageSegmentation
{
    public static class ImageUtils
    {
        #region Public Methods

        /// <summary>
        /// </summary>
        /// <remarks>
        ///     <see href="http://csharpexamples.com/fast-image-processing-c/" />
        /// </remarks>
        /// <param name="clusterSet"></param>
        /// <param name="oldImage"></param>
        /// <param name="centroidFunc"></param>
        /// <returns></returns>
        public static Bitmap GetBitmapFromClusterSet(
            ClusterSet<DataPoint> clusterSet, Bitmap oldImage, CentroidFunction<DataPoint> centroidFunc)
        {
            // first converts cluster-set into data-point map
            var map = new DataPoint[oldImage.Width][];
            foreach (var cluster in clusterSet)
            {
                var centroid = centroidFunc(cluster);
                foreach (var point in cluster)
                {
                    // creates a new pixel point with same coordinates and average color
                    if (map[point.X] == null) map[point.X] = new DataPoint[oldImage.Height];
                    map[point.X][point.Y] = new DataPoint(point.X, point.Y, centroid.R, centroid.G, centroid.B);
                }
            }

            unsafe
            {
                // then convert pixel point map into bitmap
                var image = new Bitmap(oldImage.Width, oldImage.Height, oldImage.PixelFormat);

                //lock bits
                var bitmapData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);

                // gets info
                var bytesPerPixel = (uint) (Image.GetPixelFormatSize(image.PixelFormat) / 8);
                var widthInBytes = bitmapData.Width * bytesPerPixel;
                var ptr = (byte*) bitmapData.Scan0;

                for (var y = 0u; y < bitmapData.Height; y++)
                {
                    var line = ptr + y * bitmapData.Stride;
                    for (var x = 0u; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        // colors according to info in point
                        var dataPoint = map[x][y];
                        line[x] = dataPoint.B;
                        line[x + 1] = dataPoint.G;
                        line[x + 2] = dataPoint.R;
                    }
                }

                image.UnlockBits(bitmapData);
                return image;
            }
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        ///     <see href="http://csharpexamples.com/fast-image-processing-c/" />
        /// </remarks>
        /// <param name="image"></param>
        /// <returns></returns>
        public static ISet<DataPoint> GetDatasetFromImage(Bitmap image)
        {
            var dataSet = new HashSet<DataPoint>();

            unsafe
            {
                //lock bits
                var bitmapData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);

                // gets info
                var bytesPerPixel = (uint) (Image.GetPixelFormatSize(image.PixelFormat) / 8);
                var widthInBytes = bitmapData.Width * bytesPerPixel;
                var ptr = (byte*) bitmapData.Scan0;

                for (var y = 0u; y < bitmapData.Height; y++)
                {
                    var line = ptr + y * bitmapData.Stride;
                    for (var x = 0u; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        // adds data-point
                        var b = line[x];
                        var g = line[x + 1];
                        var r = line[x + 2];
                        dataSet.Add(new DataPoint(x, y, r, g, b));
                    }
                }

                image.UnlockBits(bitmapData);
            }

            return dataSet;
        }

        #endregion
    }
}