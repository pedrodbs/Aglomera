// ------------------------------------------
// <copyright file="MainForm.cs" company="Pedro Sequeira">
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
using System.Drawing;
using System.Windows.Forms;
using Agnes;
using Agnes.Linkage;

namespace ImageSegmentation
{
    public partial class MainForm : Form
    {
        #region Fields

        private Bitmap _image;

        #endregion

        #region Constructors

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private & Protected Methods

        private void EnableAfterClustering()
        {
            this.trackBar.Enabled = this.numCLustersLabel.Enabled = true;
        }

        private void EnableAfterImageOpen()
        {
            this.runButton.Enabled = true;
            this.openButton.Enabled = false;
        }

        private void OpenButtonClick(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() != DialogResult.OK) return;

            this.pictureBox.Image = this._image = new Bitmap(this.openFileDialog.FileName);
            this.EnableAfterImageOpen();
        }

        private void RunButtonClick(object sender, EventArgs e)
        {
            // gets data-set
            var dataPoints = ImageUtils.GetDatasetFromImage(this._image);

            // performs hierarchical clustering
            var metric = new DataPoint(); // Euclidean distance
            var linkage = new CompleteLinkage<DataPoint>(metric);
            var clusteringAlg = new ClusteringAlgorithm<DataPoint>(linkage);
            var clustering = clusteringAlg.GetClustering(dataPoints);

            CentroidFunction<DataPoint> centroidFunc = DataPoint.GetCentroid;
            this.pictureBox.Image =
                ImageUtils.GetBitmapFromClusterSet(clustering[2], this._image, centroidFunc);

            this.EnableAfterClustering();
        }

        #endregion
    }
}