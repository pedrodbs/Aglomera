// ------------------------------------------
// <copyright file="MainForm.cs" company="Pedro Sequeira">
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
//    Last updated: 05/25/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Aglomera;
using Aglomera.Linkage;
using ExamplesUtil;
using ChartDataPoint = System.Windows.Forms.DataVisualization.Charting.DataPoint;
using DataPoint = ExamplesUtil.DataPoint;

namespace ClusteringVisualizer
{
    public partial class MainForm : Form
    {
        #region Fields

        private readonly IDictionary<DataPoint, ChartDataPoint> _chartDataPoints =
            new Dictionary<DataPoint, ChartDataPoint>();

        private ClusteringResult<DataPoint> _clusteringResult;
        private ISet<DataPoint> _dataPoints;
        private IDissimilarityMetric<DataPoint> _dissimilarityMetric;
        private int _numClusters;

        #endregion

        #region Constructors

        public MainForm()
        {
            InitializeComponent();

            // sets default values for combo-boxes
            this.linkageCriterionCBox.Text = this.linkageCriterionCBox.Items[0].ToString();
            this.linkageCriterionCBox.SelectionLength = 0;
            this.colorSchemeCBox.Text = this.colorSchemeCBox.Items[0].ToString();
            this.colorSchemeCBox.SelectionLength = 0;

            // makes so button is not selected by default
            this.numClustersTrackBar.Select();
        }

        #endregion

        #region Properties & Indexers

        private DataPointCollection ChartPoints => this.datasetChart.Series[0].Points;

        #endregion

        #region Private & Protected Methods

        private static Color GetGrayscale(Color color)
        {
            var grayScale = (int) (color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
            return Color.FromArgb(color.A, grayScale, grayScale, grayScale);
        }

        private void LoadDataSet()
        {
            // loads data-points
            var parser = new CsvParser();
            this._dataPoints = parser.Load(Path.GetFullPath(this.openFileDialog.FileName));

            // clears series
            this._chartDataPoints.Clear();
            this.ChartPoints.Clear();

            // adds points to series
            var maxX = double.MinValue;
            var minX = double.MaxValue;
            foreach (var dataPoint in this._dataPoints)
            {
                var chartDataPoint = new ChartDataPoint(dataPoint.Value[0], dataPoint.Value[1]) {Label = dataPoint.ID};
                this.ChartPoints.Add(chartDataPoint);
                this._chartDataPoints.Add(dataPoint, chartDataPoint);
                maxX = Math.Max(maxX, dataPoint.Value[0]);
                minX = Math.Min(minX, dataPoint.Value[0]);
            }

            // resets
            this._numClusters = int.MinValue;
            this._clusteringResult = null;
            this._dissimilarityMetric = new CachedDissimilarityMetric<DataPoint>(new DataPoint(), this._dataPoints);

            // adjusts track-bar according to num clusters
            this.numClustersTrackBar.SmallChange = (uint) (this.numClustersTrackBar.Maximum / this._dataPoints.Count);
            this.numClustersTrackBar.LargeChange = this.numClustersTrackBar.SmallChange * 5;

            this.datasetChart.ChartAreas[0].AxisX.Maximum = Math.Ceiling(maxX);
            this.datasetChart.ChartAreas[0].AxisX.Minimum = Math.Floor(minX);
        }

        private void SetInterfaceEnable(bool enable)
        {
            this.groupBox1.Enabled = this.groupBox2.Enabled = this.groupBox3.Enabled = this.groupBox4.Enabled = enable;
            this.Cursor = enable ? Cursors.Arrow : Cursors.WaitCursor;
        }

        private void UpdateClustering()
        {
            // checks data points
            if (this._dataPoints == null || this._dataPoints.Count == 0) return;
            this.SetInterfaceEnable(false);
            this.clusteringWorker.RunWorkerAsync(this.linkageCriterionCBox.SelectedIndex);
        }

        private void UpdateClusters()
        {
            // checks clustering
            if (this._clusteringResult == null || this._clusteringResult.Count == 0) return;

            // checks for same num clusters, ignore
            var numClusters = Math.Max(1,
                this.numClustersTrackBar.Value * this._clusteringResult.Count / this.numClustersTrackBar.Maximum);
            if (this._numClusters == numClusters) return;
            this._numClusters = numClusters;

            // gets palette
            Color[] palette;
            var numColors = Math.Max(2, this._numClusters);
            switch (this.colorSchemeCBox.SelectedIndex)
            {
                case 1:
                    palette = TolPalettes.CreateTolRainbowPalette(numColors);
                    break;
                case 2:
                    palette = TolPalettes.CreateTolSeqPalette(numColors);
                    break;
                default:
                    palette = TolPalettes.CreateTolDivPalette(numColors);
                    break;
            }

            // updates data-point's colors according to clusters
            var clusterSet = this._clusteringResult[this._clusteringResult.Count - this._numClusters];
            for (var i = 0; i < clusterSet.Count; i++)
                foreach (var dataPoint in clusterSet[i])
                    this._chartDataPoints[dataPoint].Color =
                        this.grayscaleCheckBox.Checked ? GetGrayscale(palette[i]) : palette[i];
        }

        #endregion

        #region UI event handlers

        private void OpenFileBtnClick(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() != DialogResult.OK) return;
            this.fileNameLabel.Text = Path.GetFileName(this.openFileDialog.FileName);

            // loads data-points and updates clustering
            this.LoadDataSet();
            this.UpdateClustering();
        }

        private void ClusteringWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            // checks data points
            if (this._dataPoints == null || this._dataPoints.Count == 0) return;

            // selects linkage criterion
            ILinkageCriterion<DataPoint> linkage;
            var selectedIndex = e.Argument;
            switch (selectedIndex)
            {
                case 1:
                    linkage = new CompleteLinkage<DataPoint>(this._dissimilarityMetric);
                    break;
                case 2:
                    linkage = new SingleLinkage<DataPoint>(this._dissimilarityMetric);
                    break;
                case 3:
                    linkage = new MinimumEnergyLinkage<DataPoint>(this._dissimilarityMetric);
                    break;
                case 4:
                    linkage = new CentroidLinkage<DataPoint>(this._dissimilarityMetric, DataPoint.GetMedoid);
                    break;
                case 5:
                    linkage = new WardsMinimumVarianceLinkage<DataPoint>(
                        this._dissimilarityMetric, DataPoint.GetMedoid);
                    break;
                default:
                    linkage = new AverageLinkage<DataPoint>(this._dissimilarityMetric);
                    break;
            }

            // clusters data-points
            var clusteringAlg = new AgglomerativeClusteringAlgorithm<DataPoint>(linkage);
            this._clusteringResult = clusteringAlg.GetClustering(this._dataPoints);
        }

        private void ClusteringWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // updates clusters based on clustering
            this.SetInterfaceEnable(true);
            this.UpdateClusters();

            // updates distance label
            var maxDist = this._clusteringResult.SingleCluster.Dissimilarity;
            var dist = this.distThreshTrackBar.Value * maxDist / this.distThreshTrackBar.Maximum;
            this.distThreshLabel.Text = dist.ToString("F", CultureInfo.InvariantCulture);
        }

        private void ShowLabelsCheckedChanged(object sender, EventArgs e)
        {
            this.datasetChart.Series[0].LabelForeColor =
                this.showLabelsCheckBox.Checked ? Color.Black : Color.Transparent;
        }

        private void NumClustersTrackBarScroll(object sender, ScrollEventArgs e)
        {
            // checks clustering
            if (this._clusteringResult == null || this._clusteringResult.Count == 0) return;

            // updates clusters
            this.UpdateClusters();

            // updates num clusters label
            this.numClustersLabel.Text = this._numClusters.ToString();

            // updates distance track-bars and label
            var maxDist = this._clusteringResult.SingleCluster.Dissimilarity;
            var dist = this._clusteringResult[this._clusteringResult.Count - this._numClusters].Dissimilarity;
            this.distThreshLabel.Text = dist.ToString("F", CultureInfo.InvariantCulture);
            this.distThreshTrackBar.Value = (int) (dist * this.distThreshTrackBar.Maximum / maxDist);
        }

        private void DistThreshTrackBarScroll(object sender, ScrollEventArgs e)
        {
            // checks clustering
            if (this._clusteringResult == null || this._clusteringResult.Count == 0) return;

            // updates distance label
            var maxDist = this._clusteringResult.SingleCluster.Dissimilarity;
            var dist = this.distThreshTrackBar.Value * maxDist / this.distThreshTrackBar.Maximum;
            this.distThreshLabel.Text = dist.ToString("F", CultureInfo.InvariantCulture);

            // updates num clusters track-bars and label
            var numCluters = 1;
            for (var i = 0; i < this._clusteringResult.Count; i++)
                if (this._clusteringResult[i].Dissimilarity >= dist)
                {
                    numCluters = this._clusteringResult.Count - i;
                    break;
                }

            this.numClustersTrackBar.Value =
                numCluters * this.numClustersTrackBar.Maximum / this._clusteringResult.Count;
            this.numClustersLabel.Text = numCluters.ToString();

            // updates clusters
            this.UpdateClusters();
        }

        private void LinkageCriterionCBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // resets and performs clustering with the new criterion
            this._numClusters = int.MinValue;
            this.UpdateClustering();
        }

        private void ColorSchemeCBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            // resets and updates clusters with new colors
            this._numClusters = int.MinValue;
            this.UpdateClusters();
        }

        private void GrayscaleCheckedChanged(object sender, EventArgs e)
        {
            // resets and updates clusters with new colors
            this._numClusters = int.MinValue;
            this.UpdateClusters();
        }

        private void BackColorPickerClick(object sender, EventArgs e)
        {
            // changes chart background color
            if (this.colorDialog.ShowDialog() != DialogResult.OK) return;
            this.UpdateBackColor();
        }

        private void UpdateBackColor()
        {
            // updates chart back color
            var newColor = this.colorDialog.Color;
            this.backColorPickerPanel.BackColor = this.datasetChart.BackColor = newColor;

            // updates axes colors
            var grayscale = GetGrayscale(newColor);
            var axesColor = grayscale.R > 128 ? Color.Black : Color.White;
            this.datasetChart.ChartAreas[0].AxisX.LineColor =
                this.datasetChart.ChartAreas[0].AxisY.LineColor =
                    this.datasetChart.ChartAreas[0].AxisX.LabelStyle.ForeColor =
                        this.datasetChart.ChartAreas[0].AxisY.LabelStyle.ForeColor =
                            this.datasetChart.ChartAreas[0].AxisX.MajorTickMark.LineColor =
                                this.datasetChart.ChartAreas[0].AxisY.MajorTickMark.LineColor= axesColor;
        }

        private void SaveImgBtnClick(object sender, EventArgs e)
        {
            // save chart image to user-selected file
            this.saveFileDialog.FileName = $"{Path.GetFileNameWithoutExtension(this.openFileDialog.FileName)}.png";
            if (this.saveFileDialog.ShowDialog() != DialogResult.OK) return;
            this.datasetChart.SaveImage(this.saveFileDialog.FileName, ChartImageFormat.Png);
        }

        #endregion
    }
}