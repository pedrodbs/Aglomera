// ------------------------------------------
// <copyright file="DataPoint.cs" company="Pedro Sequeira">
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
//    Project: ExamplesUtil
//    Last updated: 05/15/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Linq;
using System.Text;
using Aglomera;

namespace ExamplesUtil
{
    public class DataPoint : IEquatable<DataPoint>, IDissimilarityMetric<DataPoint>, IComparable<DataPoint>
    {
        #region Constructors

        public DataPoint(string id, double[] value)
        {
            this.ID = id;
            this.Value = value;
        }

        public DataPoint()
        {
        }

        #endregion

        #region Properties & Indexers

        public string ID { get; }

        public double[] Value { get; }

        #endregion

        #region Public Methods

        public override bool Equals(object obj) => obj is DataPoint && this.Equals((DataPoint) obj);

        public override int GetHashCode() => this.ID.GetHashCode();

        public override string ToString() => this.ID;

        #endregion

        #region Public Methods

        public static DataPoint GetCentroid(Cluster<DataPoint> cluster)
        {
            if (cluster.Count == 1) return cluster.First();

            // gets sum for all variables
            var id = new StringBuilder();
            var sums = new double[cluster.First().Value.Length];
            foreach (var dataPoint in cluster)
            {
                id.Append(dataPoint.ID);
                for (var i = 0; i < sums.Length; i++)
                    sums[i] += dataPoint.Value[i];
            }

            // gets average of all variables (centroid)
            for (var i = 0; i < sums.Length; i++)
                sums[i] /= cluster.Count;

            return new DataPoint(id.ToString(), sums);
        }

        public static DataPoint GetMedoid(Cluster<DataPoint> cluster) => cluster.GetMedoid(new DataPoint());

        public static bool operator ==(DataPoint left, DataPoint right) => left.Equals(right);

        public static bool operator !=(DataPoint left, DataPoint right) => !left.Equals(right);

        public double DistanceTo(DataPoint other)
        {
            var sum2 = 0d;
            var length = Math.Min(this.Value.Length, other.Value.Length);
            for (var idx1 = 0; idx1 < length; ++idx1)
            {
                var delta = this.Value[idx1] - other.Value[idx1];
                sum2 += delta * delta;
            }

            return Math.Sqrt(sum2);
        }

        public int CompareTo(DataPoint other) => string.Compare(this.ID, other.ID, StringComparison.Ordinal);

        public double Calculate(DataPoint instance1, DataPoint instance2) => instance1.DistanceTo(instance2);

        public bool Equals(DataPoint other) => string.Equals(this.ID, other.ID);

        #endregion
    }
}