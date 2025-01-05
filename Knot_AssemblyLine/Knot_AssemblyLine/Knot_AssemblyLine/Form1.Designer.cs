namespace Knot_AssemblyLine
{
    partial class AssemblyLine
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
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.totalOrderAmount_label = new System.Windows.Forms.Label();
            this.inProcessAmount_label = new System.Windows.Forms.Label();
            this.orderAmountProgressbar = new System.Windows.Forms.ProgressBar();
            this.orderAmountLabel = new System.Windows.Forms.Label();
            this.inProcessAmountLabel = new System.Windows.Forms.Label();
            this.qtyGroupbox = new System.Windows.Forms.GroupBox();
            this.quantityYiledLabel = new System.Windows.Forms.Label();
            this.qtyProducedLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.qtyyield_label = new System.Windows.Forms.Label();
            this.statusListbox = new System.Windows.Forms.ListBox();
            this.theChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.intervalComboBox = new System.Windows.Forms.ComboBox();
            this.setIntervalButton = new System.Windows.Forms.Button();
            this.IntervalLabel = new System.Windows.Forms.Label();
            this.qtyGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.theChart)).BeginInit();
            this.SuspendLayout();
            // 
            // totalOrderAmount_label
            // 
            this.totalOrderAmount_label.AutoSize = true;
            this.totalOrderAmount_label.Location = new System.Drawing.Point(391, 71);
            this.totalOrderAmount_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.totalOrderAmount_label.Name = "totalOrderAmount_label";
            this.totalOrderAmount_label.Size = new System.Drawing.Size(156, 20);
            this.totalOrderAmount_label.TabIndex = 0;
            this.totalOrderAmount_label.Text = "Total Order Amount";
            // 
            // inProcessAmount_label
            // 
            this.inProcessAmount_label.AutoSize = true;
            this.inProcessAmount_label.Location = new System.Drawing.Point(28, 71);
            this.inProcessAmount_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.inProcessAmount_label.Name = "inProcessAmount_label";
            this.inProcessAmount_label.Size = new System.Drawing.Size(151, 20);
            this.inProcessAmount_label.TabIndex = 1;
            this.inProcessAmount_label.Text = "In Process Amount";
            // 
            // orderAmountProgressbar
            // 
            this.orderAmountProgressbar.Location = new System.Drawing.Point(23, 159);
            this.orderAmountProgressbar.Margin = new System.Windows.Forms.Padding(4);
            this.orderAmountProgressbar.Name = "orderAmountProgressbar";
            this.orderAmountProgressbar.Size = new System.Drawing.Size(506, 18);
            this.orderAmountProgressbar.TabIndex = 4;
            // 
            // orderAmountLabel
            // 
            this.orderAmountLabel.AutoSize = true;
            this.orderAmountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderAmountLabel.Location = new System.Drawing.Point(389, 106);
            this.orderAmountLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.orderAmountLabel.Name = "orderAmountLabel";
            this.orderAmountLabel.Size = new System.Drawing.Size(67, 31);
            this.orderAmountLabel.TabIndex = 5;
            this.orderAmountLabel.Text = "0.00";
            // 
            // inProcessAmountLabel
            // 
            this.inProcessAmountLabel.AutoSize = true;
            this.inProcessAmountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inProcessAmountLabel.Location = new System.Drawing.Point(26, 106);
            this.inProcessAmountLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.inProcessAmountLabel.Name = "inProcessAmountLabel";
            this.inProcessAmountLabel.Size = new System.Drawing.Size(67, 31);
            this.inProcessAmountLabel.TabIndex = 6;
            this.inProcessAmountLabel.Text = "0.00";
            // 
            // qtyGroupbox
            // 
            this.qtyGroupbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.qtyGroupbox.Controls.Add(this.quantityYiledLabel);
            this.qtyGroupbox.Controls.Add(this.qtyProducedLabel);
            this.qtyGroupbox.Controls.Add(this.label3);
            this.qtyGroupbox.Controls.Add(this.qtyyield_label);
            this.qtyGroupbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.qtyGroupbox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.qtyGroupbox.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.qtyGroupbox.Location = new System.Drawing.Point(579, 30);
            this.qtyGroupbox.Margin = new System.Windows.Forms.Padding(4);
            this.qtyGroupbox.Name = "qtyGroupbox";
            this.qtyGroupbox.Padding = new System.Windows.Forms.Padding(4);
            this.qtyGroupbox.Size = new System.Drawing.Size(396, 147);
            this.qtyGroupbox.TabIndex = 7;
            this.qtyGroupbox.TabStop = false;
            this.qtyGroupbox.Text = "Production Total";
            // 
            // quantityYiledLabel
            // 
            this.quantityYiledLabel.AutoSize = true;
            this.quantityYiledLabel.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quantityYiledLabel.Location = new System.Drawing.Point(245, 76);
            this.quantityYiledLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.quantityYiledLabel.Name = "quantityYiledLabel";
            this.quantityYiledLabel.Size = new System.Drawing.Size(32, 38);
            this.quantityYiledLabel.TabIndex = 1;
            this.quantityYiledLabel.Text = "0";
            // 
            // qtyProducedLabel
            // 
            this.qtyProducedLabel.AutoSize = true;
            this.qtyProducedLabel.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.qtyProducedLabel.Location = new System.Drawing.Point(18, 76);
            this.qtyProducedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.qtyProducedLabel.Name = "qtyProducedLabel";
            this.qtyProducedLabel.Size = new System.Drawing.Size(32, 38);
            this.qtyProducedLabel.TabIndex = 0;
            this.qtyProducedLabel.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 35);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Quantity Produced";
            // 
            // qtyyield_label
            // 
            this.qtyyield_label.AutoSize = true;
            this.qtyyield_label.Location = new System.Drawing.Point(242, 38);
            this.qtyyield_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.qtyyield_label.Name = "qtyyield_label";
            this.qtyyield_label.Size = new System.Drawing.Size(117, 23);
            this.qtyyield_label.TabIndex = 3;
            this.qtyyield_label.Text = "Quantity Yield";
            // 
            // statusListbox
            // 
            this.statusListbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.statusListbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statusListbox.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.statusListbox.FormattingEnabled = true;
            this.statusListbox.ItemHeight = 20;
            this.statusListbox.Location = new System.Drawing.Point(23, 573);
            this.statusListbox.Name = "statusListbox";
            this.statusListbox.Size = new System.Drawing.Size(506, 240);
            this.statusListbox.TabIndex = 8;
            // 
            // theChart
            // 
            this.theChart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.theChart.BorderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.Name = "ChartArea1";
            this.theChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.theChart.Legends.Add(legend1);
            this.theChart.Location = new System.Drawing.Point(23, 214);
            this.theChart.Name = "theChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.theChart.Series.Add(series1);
            this.theChart.Size = new System.Drawing.Size(524, 307);
            this.theChart.TabIndex = 9;
            this.theChart.Text = "chart1";
            title1.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            title1.Name = "theChartTitle1";
            this.theChart.Titles.Add(title1);
            // 
            // intervalComboBox
            // 
            this.intervalComboBox.AccessibleDescription = "";
            this.intervalComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.intervalComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.intervalComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.intervalComboBox.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.intervalComboBox.FormattingEnabled = true;
            this.intervalComboBox.Items.AddRange(new object[] {
            "1",
            "5",
            "10"});
            this.intervalComboBox.Location = new System.Drawing.Point(97, 22);
            this.intervalComboBox.Name = "intervalComboBox";
            this.intervalComboBox.Size = new System.Drawing.Size(121, 33);
            this.intervalComboBox.TabIndex = 10;
            // 
            // setIntervalButton
            // 
            this.setIntervalButton.BackColor = System.Drawing.Color.Gray;
            this.setIntervalButton.FlatAppearance.BorderSize = 0;
            this.setIntervalButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.setIntervalButton.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setIntervalButton.Location = new System.Drawing.Point(234, 22);
            this.setIntervalButton.Name = "setIntervalButton";
            this.setIntervalButton.Size = new System.Drawing.Size(86, 33);
            this.setIntervalButton.TabIndex = 11;
            this.setIntervalButton.Text = "SET";
            this.setIntervalButton.UseVisualStyleBackColor = true;
            this.setIntervalButton.Click += new System.EventHandler(this.setIntervalButton_Click);
            // 
            // IntervalLabel
            // 
            this.IntervalLabel.AutoSize = true;
            this.IntervalLabel.Location = new System.Drawing.Point(28, 22);
            this.IntervalLabel.Name = "IntervalLabel";
            this.IntervalLabel.Size = new System.Drawing.Size(63, 20);
            this.IntervalLabel.TabIndex = 12;
            this.IntervalLabel.Text = "Interval";
            // 
            // AssemblyLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1000, 848);
            this.Controls.Add(this.IntervalLabel);
            this.Controls.Add(this.setIntervalButton);
            this.Controls.Add(this.intervalComboBox);
            this.Controls.Add(this.theChart);
            this.Controls.Add(this.statusListbox);
            this.Controls.Add(this.qtyGroupbox);
            this.Controls.Add(this.inProcessAmountLabel);
            this.Controls.Add(this.orderAmountLabel);
            this.Controls.Add(this.orderAmountProgressbar);
            this.Controls.Add(this.inProcessAmount_label);
            this.Controls.Add(this.totalOrderAmount_label);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "AssemblyLine";
            this.ShowIcon = false;
            this.Text = "Knot_Assembly Line Kanban";
            this.Load += new System.EventHandler(this.AssemblyLine_Load);
            this.qtyGroupbox.ResumeLayout(false);
            this.qtyGroupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.theChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label totalOrderAmount_label;
        private System.Windows.Forms.Label inProcessAmount_label;
        private System.Windows.Forms.ProgressBar orderAmountProgressbar;
        private System.Windows.Forms.Label orderAmountLabel;
        private System.Windows.Forms.Label inProcessAmountLabel;
        private System.Windows.Forms.GroupBox qtyGroupbox;
        private System.Windows.Forms.Label quantityYiledLabel;
        private System.Windows.Forms.Label qtyProducedLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label qtyyield_label;
        private System.Windows.Forms.ListBox statusListbox;
        private System.Windows.Forms.DataVisualization.Charting.Chart theChart;
        private System.Windows.Forms.ComboBox intervalComboBox;
        private System.Windows.Forms.Button setIntervalButton;
        private System.Windows.Forms.Label IntervalLabel;
    }
}

