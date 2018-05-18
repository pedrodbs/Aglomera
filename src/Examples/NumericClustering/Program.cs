// ------------------------------------------
// <copyright file="Program.cs" company="Pedro Sequeira">
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
//    Project: NumericClustering
//    Last updated: 04/27/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using ExamplesUtil;
using Aglomera;
using Aglomera.D3;
using Aglomera.Linkage;
using Newtonsoft.Json;

namespace NumericClustering
{
    internal class Program
    {
        #region Static Fields & Constants

        private const string DATASET_FILE = "../../../../../datasets/iris.csv";

        #endregion

        #region Private & Protected Methods

        private static void Main(string[] args)
        {
            var dataSetFile = args.Length > 0 ? args[0] : DATASET_FILE;
            var parser = new CsvParser();
            var instances = parser.Load(Path.GetFullPath(dataSetFile));

            var metric = new DataPoint(null, null);
            var linkages = new Dictionary<ILinkageCriterion<DataPoint>, string>
                           {
                               {new AverageLinkage<DataPoint>(metric), "average"},
                               {new CompleteLinkage<DataPoint>(metric), "complete"},
                               {new SingleLinkage<DataPoint>(metric), "single"},
                               {new MinimumEnergyLinkage<DataPoint>(metric), "min-energy"},
                               {new CentroidLinkage<DataPoint>(metric, DataPoint.GetCentroid), "centroid"},
                               {new WardsMinimumVarianceLinkage<DataPoint>(metric, DataPoint.GetCentroid), "ward"}
                           };
            foreach (var linkage in linkages)
                PrintClusters(instances, linkage.Key, linkage.Value);

            Console.WriteLine("\nDone!");
            Console.ReadKey();
        }

        private static void PrintClusters(ISet<DataPoint> instances, ILinkageCriterion<DataPoint> linkage, string name)
        {
            var perfMeasure = new PerformanceMeasure();
            perfMeasure.Start();
            var clusteringAlg = new AgglomerativeClusteringAlgorithm<DataPoint>(linkage);
            var clustering = clusteringAlg.GetClustering(instances);
            perfMeasure.Stop();

            Console.WriteLine("_____________________________________________");
            Console.WriteLine(name);
            Console.WriteLine(perfMeasure);
            foreach (var clusterSet in clustering)
            {
                Console.WriteLine($"Clusters at distance: {clusterSet.Dissimilarity:0.00} ({clusterSet.Count})");
                foreach (var cluster in clusterSet)
                    Console.WriteLine($" - {cluster}");
            }

            clustering.SaveD3DendrogramFile(Path.GetFullPath($"{name}.json"), formatting: Formatting.Indented);
        }

        #endregion
    }
}