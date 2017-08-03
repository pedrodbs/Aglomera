// ------------------------------------------
// <copyright file="CsvParser.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes.Examples.NumericClustering
//    Last updated: 2017/07/27
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Agnes.Examples.NumericClustering
{
    public class CsvParser
    {
        #region Fields

        private readonly char[] _separator;

        #endregion

        #region Constructors

        public CsvParser(char separator = ',')
        {
            this._separator = new[] {separator};
        }

        #endregion

        #region Public Methods

        public ISet<DataPoint> Load(string filePath)
        {
            return !File.Exists(filePath) ? null : this.LoadData(filePath);
        }

        public void Save(string filePath, ISet<DataPoint> dataPoints)
        {
            // writes a line for each given transaction, converts to csv
            using (var sw = new StreamWriter(filePath) {AutoFlush = true})
            {
                foreach (var transaction in dataPoints)
                    sw.WriteLine(this.GetDataPointString(transaction));
                sw.Close();
            }
        }

        #endregion

        #region Private & Protected Methods

        private string GetDataPointString(DataPoint dataPoint)
        {
            var sb = new StringBuilder();
            foreach (var val in dataPoint.Value)
                sb.Append($"{val}{this._separator[0]}");
            sb.Append(dataPoint.ID);
            return sb.ToString();
        }

        private ISet<DataPoint> LoadData(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            var dataPoints = new HashSet<DataPoint>();
            using (var sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    // checks empty / incorrect line
                    var fields = line.Split(this._separator, StringSplitOptions.RemoveEmptyEntries);
                    if (fields.Length == 0) continue;

                    // tries to convert all fields into numbers
                    var list = new List<double>();
                    for (var i = 0; i < fields.Length - 1; i++)
                    {
                        var field = fields[i];
                        double val;
                        if (double.TryParse(field, out val)) list.Add(val);
                    }

                    // creates data-point, assume last field is ID
                    if (list.Count > 0) dataPoints.Add(new DataPoint(fields[fields.Length - 1], list.ToArray()));
                }
                sr.Close();
            }
            return dataPoints;
        }

        #endregion
    }
}