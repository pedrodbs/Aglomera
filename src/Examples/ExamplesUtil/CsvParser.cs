// ------------------------------------------
// <copyright file="CsvParser.cs" company="Pedro Sequeira">
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
//    Last updated: 04/27/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExamplesUtil
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
            using (var fs = new FileStream(filePath, FileMode.Create))
            using (var sw = new StreamWriter(fs, Encoding.UTF8) {AutoFlush = true})
            {
                foreach (var transaction in dataPoints)
                    sw.WriteLine(this.GetDataPointString(transaction));
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
            using (var fs = new FileStream(filePath, FileMode.Open))
            using (var sr = new StreamReader(fs))
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
                        if (double.TryParse(field, out double val)) list.Add(val);
                    }

                    // creates data-point, assume last field is ID
                    if (list.Count > 0) dataPoints.Add(new DataPoint(fields[fields.Length - 1], list.ToArray()));
                }
            }

            return dataPoints;
        }

        #endregion
    }
}