// ------------------------------------------
// <copyright file="D3Tests.cs" company="Pedro Sequeira">
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
//    Project: Aglomera.D3.Tests
//    Last updated: 05/15/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Aglomera.Linkage;
using ExamplesUtil;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Aglomera.D3.Tests
{
    [TestClass]
    public class D3Tests
    {
        #region Static Fields & Constants

        private const string FILE_NAME = "file.json";

        internal static readonly ISet<DataPoint> DataPoints = new HashSet<DataPoint>(
            new[]
            {
                new DataPoint("A1", new[] {2.0d, 2.0d}),
                new DataPoint("B2", new[] {5.5d, 4.0d}),
                new DataPoint("B3", new[] {5.0d, 5.0d}),
                new DataPoint("A4", new[] {1.5d, 2.5d}),
                new DataPoint("A5", new[] {1.0d, 1.0d}),
                new DataPoint("B6", new[] {7.0d, 5.0d}),
                new DataPoint("B7", new[] {5.75d, 6.5d})
            });

        #endregion

        #region Public Methods

        [TestMethod]
        public void SaveFileTest()
        {
            var clusteringAlg = new AgglomerativeClusteringAlgorithm<DataPoint>(new AverageLinkage<DataPoint>(new DataPoint()));
            var clustering = clusteringAlg.GetClustering(DataPoints);
            Console.WriteLine(clustering);

            var fullPath = Path.Combine(Path.GetFullPath("."), FILE_NAME);
            File.Delete(fullPath);

            clustering.SaveD3DendrogramFile(fullPath, formatting: Formatting.Indented);
            Console.WriteLine(fullPath);
            Assert.IsTrue(File.Exists(fullPath), $"D3 json file should exist in {fullPath}.");
            Assert.IsTrue(new FileInfo(fullPath).Length > 0, "Json file size should be > 0 bytes.");

#if !DEBUG
                File.Delete(fullPath);
#endif
        }

        #endregion
    }
}