using ClusteringVisualizer;

namespace ClusteringVisualizer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.openFileBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.datasetChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.linkageCriterionCBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.distThreshLabel = new System.Windows.Forms.Label();
            this.numClustersLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numClustersTrackBar = new ClusteringVisualizer.ColorSlider();
            this.distThreshTrackBar = new ClusteringVisualizer.ColorSlider();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.grayscaleCheckBox = new System.Windows.Forms.CheckBox();
            this.backColorPickerPanel = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.saveImgBtn = new System.Windows.Forms.Button();
            this.colorSchemeCBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.showLabelsCheckBox = new System.Windows.Forms.CheckBox();
            this.clusteringWorker = new System.ComponentModel.BackgroundWorker();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datasetChart)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.fileNameLabel);
            this.groupBox1.Controls.Add(this.openFileBtn);
            this.groupBox1.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(180, 115);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data-set";
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileNameLabel.BackColor = System.Drawing.Color.White;
            this.fileNameLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.fileNameLabel.Location = new System.Drawing.Point(14, 23);
            this.fileNameLabel.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(152, 31);
            this.fileNameLabel.TabIndex = 0;
            this.fileNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openFileBtn
            // 
            this.openFileBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openFileBtn.AutoSize = true;
            this.openFileBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.openFileBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openFileBtn.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openFileBtn.Location = new System.Drawing.Point(15, 69);
            this.openFileBtn.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.openFileBtn.Name = "openFileBtn";
            this.openFileBtn.Size = new System.Drawing.Size(152, 31);
            this.openFileBtn.TabIndex = 1;
            this.openFileBtn.Text = "&Open...";
            this.openFileBtn.UseVisualStyleBackColor = false;
            this.openFileBtn.Click += new System.EventHandler(this.OpenFileBtnClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.datasetChart);
            this.groupBox2.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(200, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(810, 604);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data-point clusters";
            // 
            // datasetChart
            // 
            this.datasetChart.BackColor = System.Drawing.Color.Empty;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisX.LabelStyle.Format = "0.##";
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.LabelStyle.Format = "0.##";
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 100F;
            chartArea1.Position.Width = 100F;
            this.datasetChart.ChartAreas.Add(chartArea1);
            this.datasetChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datasetChart.Location = new System.Drawing.Point(3, 22);
            this.datasetChart.Name = "datasetChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Font = new System.Drawing.Font("Candara", 8.830189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.LabelForeColor = System.Drawing.Color.Transparent;
            series1.MarkerBorderColor = System.Drawing.Color.Black;
            series1.MarkerSize = 10;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Series1";
            series1.SmartLabelStyle.Enabled = false;
            this.datasetChart.Series.Add(series1);
            this.datasetChart.Size = new System.Drawing.Size(804, 579);
            this.datasetChart.TabIndex = 0;
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "csv";
            this.openFileDialog.FileName = "dataset.csv";
            this.openFileDialog.Filter = "CSV files|*.csv";
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Title = "Select the file containing the data-set to be clustered";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.linkageCriterionCBox);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.distThreshLabel);
            this.groupBox3.Controls.Add(this.numClustersLabel);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.numClustersTrackBar);
            this.groupBox3.Controls.Add(this.distThreshTrackBar);
            this.groupBox3.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(14, 135);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(180, 209);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Clustering options:";
            // 
            // linkageCriterionCBox
            // 
            this.linkageCriterionCBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkageCriterionCBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.linkageCriterionCBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.linkageCriterionCBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.linkageCriterionCBox.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkageCriterionCBox.ForeColor = System.Drawing.Color.Black;
            this.linkageCriterionCBox.Items.AddRange(new object[] {
            "Average",
            "Complete",
            "Single",
            "Min. Energy",
            "Centroid",
            "Ward\'s Min. Variance"});
            this.linkageCriterionCBox.Location = new System.Drawing.Point(13, 54);
            this.linkageCriterionCBox.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.linkageCriterionCBox.Name = "linkageCriterionCBox";
            this.linkageCriterionCBox.Size = new System.Drawing.Size(154, 26);
            this.linkageCriterionCBox.TabIndex = 6;
            this.linkageCriterionCBox.SelectedIndexChanged += new System.EventHandler(this.LinkageCriterionCBoxSelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Linkage criterion:";
            // 
            // distThreshLabel
            // 
            this.distThreshLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.distThreshLabel.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.distThreshLabel.Location = new System.Drawing.Point(136, 147);
            this.distThreshLabel.Margin = new System.Windows.Forms.Padding(5);
            this.distThreshLabel.Name = "distThreshLabel";
            this.distThreshLabel.Size = new System.Drawing.Size(36, 19);
            this.distThreshLabel.TabIndex = 5;
            this.distThreshLabel.Text = "1";
            this.distThreshLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // numClustersLabel
            // 
            this.numClustersLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numClustersLabel.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numClustersLabel.Location = new System.Drawing.Point(136, 88);
            this.numClustersLabel.Margin = new System.Windows.Forms.Padding(5);
            this.numClustersLabel.Name = "numClustersLabel";
            this.numClustersLabel.Size = new System.Drawing.Size(36, 19);
            this.numClustersLabel.TabIndex = 2;
            this.numClustersLabel.Text = "1";
            this.numClustersLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 88);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Num. clusters:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 147);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 19);
            this.label3.TabIndex = 4;
            this.label3.Text = "Dist. threshold:";
            // 
            // numClustersTrackBar
            // 
            this.numClustersTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numClustersTrackBar.BackColor = System.Drawing.Color.Transparent;
            this.numClustersTrackBar.BarInnerColor = System.Drawing.Color.White;
            this.numClustersTrackBar.BarOuterColor = System.Drawing.Color.White;
            this.numClustersTrackBar.BarPenColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.numClustersTrackBar.BorderRoundRectSize = new System.Drawing.Size(1, 1);
            this.numClustersTrackBar.DrawFocusRectangle = false;
            this.numClustersTrackBar.DrawSemitransparentThumb = false;
            this.numClustersTrackBar.ElapsedInnerColor = System.Drawing.Color.Transparent;
            this.numClustersTrackBar.ElapsedOuterColor = System.Drawing.Color.Transparent;
            this.numClustersTrackBar.LargeChange = ((uint)(500u));
            this.numClustersTrackBar.Location = new System.Drawing.Point(13, 115);
            this.numClustersTrackBar.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.numClustersTrackBar.Maximum = 10000;
            this.numClustersTrackBar.MouseEffects = false;
            this.numClustersTrackBar.Name = "numClustersTrackBar";
            this.numClustersTrackBar.Size = new System.Drawing.Size(154, 24);
            this.numClustersTrackBar.SmallChange = ((uint)(100u));
            this.numClustersTrackBar.TabIndex = 0;
            this.numClustersTrackBar.ThumbInnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.numClustersTrackBar.ThumbOuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.numClustersTrackBar.ThumbPenColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.numClustersTrackBar.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            this.numClustersTrackBar.ThumbSize = 22;
            this.numClustersTrackBar.Value = 1;
            this.numClustersTrackBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.NumClustersTrackBarScroll);
            // 
            // distThreshTrackBar
            // 
            this.distThreshTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.distThreshTrackBar.BackColor = System.Drawing.Color.Transparent;
            this.distThreshTrackBar.BarInnerColor = System.Drawing.Color.White;
            this.distThreshTrackBar.BarOuterColor = System.Drawing.Color.White;
            this.distThreshTrackBar.BarPenColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.distThreshTrackBar.BorderRoundRectSize = new System.Drawing.Size(1, 1);
            this.distThreshTrackBar.DrawFocusRectangle = false;
            this.distThreshTrackBar.DrawSemitransparentThumb = false;
            this.distThreshTrackBar.ElapsedInnerColor = System.Drawing.Color.Transparent;
            this.distThreshTrackBar.ElapsedOuterColor = System.Drawing.Color.Transparent;
            this.distThreshTrackBar.LargeChange = ((uint)(500u));
            this.distThreshTrackBar.Location = new System.Drawing.Point(12, 174);
            this.distThreshTrackBar.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.distThreshTrackBar.Maximum = 10000;
            this.distThreshTrackBar.MouseEffects = false;
            this.distThreshTrackBar.Name = "distThreshTrackBar";
            this.distThreshTrackBar.Size = new System.Drawing.Size(154, 24);
            this.distThreshTrackBar.SmallChange = ((uint)(100u));
            this.distThreshTrackBar.TabIndex = 3;
            this.distThreshTrackBar.ThumbInnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.distThreshTrackBar.ThumbOuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.distThreshTrackBar.ThumbPenColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.distThreshTrackBar.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            this.distThreshTrackBar.ThumbSize = 22;
            this.distThreshTrackBar.Value = 10000;
            this.distThreshTrackBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.DistThreshTrackBarScroll);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.grayscaleCheckBox);
            this.groupBox4.Controls.Add(this.backColorPickerPanel);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.saveImgBtn);
            this.groupBox4.Controls.Add(this.colorSchemeCBox);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.showLabelsCheckBox);
            this.groupBox4.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(14, 350);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(180, 226);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Visual options";
            // 
            // grayscaleCheckBox
            // 
            this.grayscaleCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grayscaleCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.grayscaleCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grayscaleCheckBox.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grayscaleCheckBox.Location = new System.Drawing.Point(13, 56);
            this.grayscaleCheckBox.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.grayscaleCheckBox.Name = "grayscaleCheckBox";
            this.grayscaleCheckBox.Size = new System.Drawing.Size(154, 21);
            this.grayscaleCheckBox.TabIndex = 12;
            this.grayscaleCheckBox.Text = "Grayscale:";
            this.grayscaleCheckBox.UseVisualStyleBackColor = true;
            this.grayscaleCheckBox.CheckedChanged += new System.EventHandler(this.GrayscaleCheckedChanged);
            // 
            // backColorPickerPanel
            // 
            this.backColorPickerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.backColorPickerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.backColorPickerPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.backColorPickerPanel.Location = new System.Drawing.Point(145, 87);
            this.backColorPickerPanel.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.backColorPickerPanel.Name = "backColorPickerPanel";
            this.backColorPickerPanel.Size = new System.Drawing.Size(22, 22);
            this.backColorPickerPanel.TabIndex = 11;
            this.backColorPickerPanel.Click += new System.EventHandler(this.BackColorPickerClick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(9, 87);
            this.label6.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 19);
            this.label6.TabIndex = 10;
            this.label6.Text = "Background color:";
            // 
            // saveImgBtn
            // 
            this.saveImgBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveImgBtn.AutoSize = true;
            this.saveImgBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.saveImgBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveImgBtn.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveImgBtn.Location = new System.Drawing.Point(13, 182);
            this.saveImgBtn.Margin = new System.Windows.Forms.Padding(10);
            this.saveImgBtn.Name = "saveImgBtn";
            this.saveImgBtn.Size = new System.Drawing.Size(154, 31);
            this.saveImgBtn.TabIndex = 9;
            this.saveImgBtn.Text = "&Save PNG...";
            this.saveImgBtn.UseVisualStyleBackColor = false;
            this.saveImgBtn.Click += new System.EventHandler(this.SaveImgBtnClick);
            // 
            // colorSchemeCBox
            // 
            this.colorSchemeCBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.colorSchemeCBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.colorSchemeCBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.colorSchemeCBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.colorSchemeCBox.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorSchemeCBox.ForeColor = System.Drawing.Color.Black;
            this.colorSchemeCBox.Items.AddRange(new object[] {
            "Tol\'s Diverging",
            "Tol\'s Rainbow",
            "Tol\'s Sequential"});
            this.colorSchemeCBox.Location = new System.Drawing.Point(13, 143);
            this.colorSchemeCBox.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.colorSchemeCBox.Name = "colorSchemeCBox";
            this.colorSchemeCBox.Size = new System.Drawing.Size(154, 26);
            this.colorSchemeCBox.TabIndex = 8;
            this.colorSchemeCBox.SelectedIndexChanged += new System.EventHandler(this.ColorSchemeCBoxSelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 116);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 19);
            this.label4.TabIndex = 7;
            this.label4.Text = "Color scheme: ";
            // 
            // showLabelsCheckBox
            // 
            this.showLabelsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showLabelsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.showLabelsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.showLabelsCheckBox.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showLabelsCheckBox.Location = new System.Drawing.Point(13, 25);
            this.showLabelsCheckBox.Margin = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.showLabelsCheckBox.Name = "showLabelsCheckBox";
            this.showLabelsCheckBox.Size = new System.Drawing.Size(154, 21);
            this.showLabelsCheckBox.TabIndex = 0;
            this.showLabelsCheckBox.Text = "Show labels:";
            this.showLabelsCheckBox.UseVisualStyleBackColor = true;
            this.showLabelsCheckBox.CheckedChanged += new System.EventHandler(this.ShowLabelsCheckedChanged);
            // 
            // clusteringWorker
            // 
            this.clusteringWorker.WorkerReportsProgress = true;
            this.clusteringWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ClusteringWorkerDoWork);
            this.clusteringWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ClusteringWorkerCompleted);
            // 
            // colorDialog
            // 
            this.colorDialog.Color = System.Drawing.Color.White;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "png";
            this.saveFileDialog.FileName = "clustering.png";
            this.saveFileDialog.Filter = "PNG image files|*.png";
            this.saveFileDialog.RestoreDirectory = true;
            this.saveFileDialog.Title = "Select the file to save the clustering image";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1019, 629);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Candara", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Aglomera.NET - Clustering Visualizer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datasetChart)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button openFileBtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataVisualization.Charting.Chart datasetChart;
        private System.Windows.Forms.Label fileNameLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private ClusteringVisualizer.ColorSlider numClustersTrackBar;
        private System.Windows.Forms.Label numClustersLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label distThreshLabel;
        private System.Windows.Forms.Label label3;
        private ClusteringVisualizer.ColorSlider distThreshTrackBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox linkageCriterionCBox;
        private System.ComponentModel.BackgroundWorker clusteringWorker;
        private System.Windows.Forms.CheckBox showLabelsCheckBox;
        private System.Windows.Forms.ComboBox colorSchemeCBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button saveImgBtn;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Panel backColorPickerPanel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.CheckBox grayscaleCheckBox;
    }
}

