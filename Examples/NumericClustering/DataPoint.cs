// ------------------------------------------
// <copyright file="DataPoint.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: SmallExample
//    Last updated: 2017/03/10
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Linq;
using Agnes;

namespace NumericClustering
{
    public struct DataPoint : IEquatable<DataPoint>, IDissimilarityMetric<DataPoint>, IComparable<DataPoint>
    {
        #region Properties & Indexers

        public string ID { get; }

        public double[] Value { get; }

        #endregion

        #region Constructors

        public DataPoint(string id, double[] value)
        {
            this.ID = id;
            this.Value = value;
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            return obj is DataPoint && this.Equals((DataPoint) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return this.Value.Aggregate(17, (current, d) => current * 23 + d.GetHashCode());
            }
        }

        public override string ToString()
        {
            return this.ID;
        }

        #endregion

        #region Public Methods

        public static bool operator ==(DataPoint left, DataPoint right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DataPoint left, DataPoint right)
        {
            return !left.Equals(right);
        }

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

        public int CompareTo(DataPoint other)
        {
            return string.Compare(this.ID, other.ID, StringComparison.Ordinal);
        }

        public double Calculate(DataPoint instance1, DataPoint instance2)
        {
            return instance1.DistanceTo(instance2);
        }

        public bool Equals(DataPoint other)
        {
            return string.Equals(this.ID, other.ID) && this.Value.SequenceEqual(other.Value);
        }

        #endregion
    }
}