using System.Windows.Forms;

namespace A4_DataVisualization
{
    partial class DataVisualization
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
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.btn_loadcsv = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btn_chart = new System.Windows.Forms.Button();
            this.dateTimePicker_start = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_end = new System.Windows.Forms.DateTimePicker();
            this.theChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.comboBox_chart = new System.Windows.Forms.ComboBox();
            this.comboBox_region = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.theChart)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_loadcsv
            // 
            this.btn_loadcsv.BackColor = System.Drawing.Color.DarkGray;
            this.btn_loadcsv.FlatAppearance.BorderSize = 0;
            this.btn_loadcsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_loadcsv.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_loadcsv.Location = new System.Drawing.Point(16, 12);
            this.btn_loadcsv.Name = "btn_loadcsv";
            this.btn_loadcsv.Size = new System.Drawing.Size(101, 32);
            this.btn_loadcsv.TabIndex = 2;
            this.btn_loadcsv.Text = "Load CSV file";
            this.btn_loadcsv.UseVisualStyleBackColor = false;
            this.btn_loadcsv.Click += new System.EventHandler(this.LoadFile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btn_chart
            // 
            this.btn_chart.BackColor = System.Drawing.Color.DarkGray;
            this.btn_chart.FlatAppearance.BorderSize = 0;
            this.btn_chart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_chart.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_chart.Location = new System.Drawing.Point(634, 12);
            this.btn_chart.Name = "btn_chart";
            this.btn_chart.Size = new System.Drawing.Size(101, 32);
            this.btn_chart.TabIndex = 4;
            this.btn_chart.Text = "Load Chart";
            this.btn_chart.UseVisualStyleBackColor = false;
            this.btn_chart.Click += new System.EventHandler(this.btnChart_Click);
            // 
            // dateTimePicker_start
            // 
            this.dateTimePicker_start.CalendarMonthBackground = System.Drawing.Color.WhiteSmoke;
            this.dateTimePicker_start.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker_start.Location = new System.Drawing.Point(137, 12);
            this.dateTimePicker_start.Name = "dateTimePicker_start";
            this.dateTimePicker_start.Size = new System.Drawing.Size(176, 27);
            this.dateTimePicker_start.TabIndex = 7;
            this.dateTimePicker_start.ValueChanged += new System.EventHandler(this.dateTimePicker_start_ValueChanged);
            // 
            // dateTimePicker_end
            // 
            this.dateTimePicker_end.CalendarMonthBackground = System.Drawing.Color.WhiteSmoke;
            this.dateTimePicker_end.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker_end.Location = new System.Drawing.Point(319, 12);
            this.dateTimePicker_end.Name = "dateTimePicker_end";
            this.dateTimePicker_end.Size = new System.Drawing.Size(176, 27);
            this.dateTimePicker_end.TabIndex = 8;
            this.dateTimePicker_end.ValueChanged += new System.EventHandler(this.dateTimePicker_end_ValueChanged);
            // 
            // theChart
            // 
            this.theChart.BackColor = System.Drawing.Color.Gainsboro;
            this.theChart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.theChart.BorderlineColor = System.Drawing.Color.Gainsboro;
            this.theChart.BorderlineWidth = 0;
            chartArea1.BackColor = System.Drawing.Color.Gainsboro;
            chartArea1.BackSecondaryColor = System.Drawing.Color.WhiteSmoke;
            chartArea1.Name = "ChartArea1";
            this.theChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.theChart.Legends.Add(legend1);
            this.theChart.Location = new System.Drawing.Point(22, 122);
            this.theChart.Name = "theChart";
            this.theChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            this.theChart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.theChart.Size = new System.Drawing.Size(962, 551);
            this.theChart.TabIndex = 9;
            this.theChart.Text = "Epidemiology Update";
            this.theChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.theChart_MouseMove);
            // 
            // comboBox_chart
            // 
            this.comboBox_chart.BackColor = System.Drawing.Color.WhiteSmoke;
            this.comboBox_chart.DropDownWidth = 160;
            this.comboBox_chart.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_chart.FormattingEnabled = true;
            this.comboBox_chart.IntegralHeight = false;
            this.comboBox_chart.Items.AddRange(new object[] {
            "Covid19 Cases",
            "Test&Confirmed Cases"});
            this.comboBox_chart.Location = new System.Drawing.Point(17, 12);
            this.comboBox_chart.Name = "comboBox_chart";
            this.comboBox_chart.Size = new System.Drawing.Size(114, 28);
            this.comboBox_chart.TabIndex = 10;
            this.comboBox_chart.Text = "Select Chart";
            this.comboBox_chart.SelectedIndexChanged += new System.EventHandler(this.comboBox_chart_SelectedIndexChanged);
            // 
            // comboBox_region
            // 
            this.comboBox_region.BackColor = System.Drawing.Color.WhiteSmoke;
            this.comboBox_region.DropDownWidth = 200;
            this.comboBox_region.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_region.FormattingEnabled = true;
            this.comboBox_region.Items.AddRange(new object[] {
            "Canada",
            "Alberta",
            "British Columbia",
            "Manitoba",
            "New Brunswick",
            "Newfoundland and Labrador",
            "Northwest Territories",
            "Nova Scotia",
            "Nunavut",
            "Ontario",
            "Prince Edward Island",
            "Quebec",
            "Saskatchewan",
            "Yukon"});
            this.comboBox_region.Location = new System.Drawing.Point(501, 12);
            this.comboBox_region.Name = "comboBox_region";
            this.comboBox_region.Size = new System.Drawing.Size(127, 28);
            this.comboBox_region.TabIndex = 11;
            this.comboBox_region.Text = "Select Region";
            this.comboBox_region.SelectedIndexChanged += new System.EventHandler(this.comboBox_region_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox1.Controls.Add(this.btn_loadcsv);
            this.groupBox1.Location = new System.Drawing.Point(26, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(132, 56);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox2.Controls.Add(this.comboBox_chart);
            this.groupBox2.Controls.Add(this.dateTimePicker_start);
            this.groupBox2.Controls.Add(this.comboBox_region);
            this.groupBox2.Controls.Add(this.btn_chart);
            this.groupBox2.Controls.Add(this.dateTimePicker_end);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Location = new System.Drawing.Point(234, 25);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(750, 56);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            // 
            // DataVisualization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1013, 703);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.theChart);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.Desktop;
            this.Name = "DataVisualization";
            this.Text = "COVID-19 epidemiology update - CANADA";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.theChart)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_loadcsv;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btn_chart;
        private System.Windows.Forms.DateTimePicker dateTimePicker_start;
        private System.Windows.Forms.DateTimePicker dateTimePicker_end;
        private System.Windows.Forms.DataVisualization.Charting.Chart theChart;
        private System.Windows.Forms.ComboBox comboBox_chart;
        private System.Windows.Forms.ComboBox comboBox_region;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;

        public TextBox PointLabelToolTip { get; private set; }
    }
}

