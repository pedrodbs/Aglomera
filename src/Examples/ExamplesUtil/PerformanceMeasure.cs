// ------------------------------------------
// <copyright file="PerformanceMeasure.cs" company="Pedro Sequeira">
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
using System.Diagnostics;

namespace ExamplesUtil
{
    public class PerformanceMeasure
    {
        #region Fields

        private readonly Stopwatch _timer = new Stopwatch();
        private long _memoryStart;

        #endregion

        #region Constructors

        public PerformanceMeasure()
        {
            this.TimeElapsed = new TimeSpan();
        }

        #endregion

        #region Properties & Indexers

        public long MemoryUsage { get; protected set; }

        public TimeSpan TimeElapsed { get; protected set; }

        #endregion

        #region Public methods

        public virtual void Start()
        {
            //starts measures (time and memory)
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            this._memoryStart = Process.GetCurrentProcess().PrivateMemorySize64;
            this._timer.Start();
        }

        public virtual void Stop()
        {
            //stops timers and measures
            this._timer.Stop();
            var memoryEnd = Process.GetCurrentProcess().PrivateMemorySize64;

            this.TimeElapsed = this._timer.Elapsed;
            this.MemoryUsage += memoryEnd - this._memoryStart;
        }

        public void Reset()
        {
            // "zero"s all measures
            this._timer.Stop();
            this.MemoryUsage = 0;
            this.TimeElapsed = new TimeSpan();
        }

        public override string ToString()
        {
            return $"time elapsed: {this.TimeElapsed}, memory spent: {BytesToString(this.MemoryUsage)}";
        }

        /// <remarks>From <a href="http://stackoverflow.com/a/4975942" /></remarks>
        private static string BytesToString(long byteCount)
        {
            string[] suf = {"B", "KB", "MB", "GB", "TB", "PB", "EB"}; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return Math.Sign(byteCount) * num + suf[place];
        }

        #endregion
    }
}