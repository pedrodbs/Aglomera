// ------------------------------------------
// <copyright file="DataPoint.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: ImageSegmentation
//    Last updated: 2018/01/12
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Linq;
using Agnes;

namespace ImageSegmentation
{
    public class DataPoint : IEquatable<DataPoint>, IDissimilarityMetric<DataPoint>, IComparable<DataPoint>
    {
        #region Fields

        private readonly int _hashCode;

        private readonly byte[] _value = new byte[3];

        #endregion

        #region Constructors

        public DataPoint()
        {
        }

        public DataPoint(uint x, uint y, byte r, byte g, byte b)
        {
            this.X = x;
            this.Y = y;
            this._value[0] = r;
            this._value[1] = g;
            this._value[2] = b;
            this._hashCode = this.GenerateHashCode();
        }

        #endregion

        #region Properties & Indexers

        public byte B => this._value[2];

        public byte G => this._value[1];

        public byte R => this._value[0];

        public uint X { get; }

        public uint Y { get; }

        #endregion

        #region Public Methods

        public override bool Equals(object obj) => obj is DataPoint && this.Equals((DataPoint) obj);

        public override int GetHashCode() => this._hashCode;

        public override string ToString() => $"({this.R},{this.G},{this.B})";

        #endregion

        #region Public Methods

        public static DataPoint GetCentroid(Cluster<DataPoint> cluster)
        {
            if (cluster.Count == 1) return cluster.First();

            // gets sum for all variables
            var colorSums = new double[3];
            var xSums = 0d;
            var ySums = 0d;
            foreach (var dataPoint in cluster)
            {
                for (var i = 0; i < 3; i++)
                    colorSums[i] += dataPoint._value[i];
                xSums += dataPoint.X;
                ySums += dataPoint.Y;
            }

            // gets average of all variables (centroid)
            for (var i = 0; i < colorSums.Length; i++)
                colorSums[i] /= cluster.Count;
            xSums /= cluster.Count;
            ySums /= cluster.Count;

            return new DataPoint(
                (uint) xSums, (uint) ySums, (byte) colorSums[0], (byte) colorSums[1], (byte) colorSums[2]);
        }

        public static DataPoint GetMedoid(Cluster<DataPoint> cluster) => cluster.GetMedoid(new DataPoint());

        public static bool operator ==(DataPoint left, DataPoint right) => left.Equals(right);

        public static bool operator !=(DataPoint left, DataPoint right) => !left.Equals(right);

        public int CompareTo(DataPoint other)
        {
            var rComp = this.R - other.R;
            var gComp = this.G - other.G;
            var bComp = this.B - other.B;
            return rComp != 0 ? rComp : gComp != 0 ? gComp : bComp;
        }

        public double Calculate(DataPoint instance1, DataPoint instance2)
        {
            var sum2 = 0d;
            for (var i = 0; i < 3; ++i)
            {
                var delta = instance1._value[i] - instance2._value[i];
                sum2 += delta * delta;
            }
            return Math.Sqrt(sum2);
        }

        public bool Equals(DataPoint other) => other != null && this._value.SequenceEqual(other._value);

        #endregion

        #region Private & Protected Methods

        private int GenerateHashCode()
        {
            unchecked
            {
                return (19 * 31 + this.X.GetHashCode()) * 31 + this.Y.GetHashCode();
            }
        }

        #endregion
    }
}