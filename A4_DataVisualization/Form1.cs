// https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.openfiledialog?view=windowsdesktop-8.0
// https://blog.naver.com/acrodev/222228956320
// https://learn.microsoft.com/en-us/dotnet/api/system.datetime.fromoadate?view=net-8.0

using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace A4_DataVisualization
{
    // NAME     : DataVisualization
    // PURPOSE  : The DataVisualization class provides the Windows Form.
    public partial class DataVisualization : Form
    {
        
        private int chartname = -1;
        private string startdate = null;
        private string enddate = null;
        private string province_territory = null;
        private System.Windows.Forms.ToolTip toolTip = null;
        private const string TABLE_COVID19CASES = "Covid19Cases";
        private const string TABLE_LABINDICATORS = "LabIndicators";
        private const int COLUMN_COUNTS_COVID19CASES = 23;
        private const int COLUMN_COUNTS_LABINDICATORS = 8;
        private DateTime earlist_cases = new DateTime(2020, 2, 29);
        private DateTime earlist_lab = new DateTime(2022, 9, 3);
        public DataVisualization()
        {
            InitializeComponent();
        }

        // FUNCTION     : Form1_Load
        // DESCRIPTION  : In Form1_Load block, Instantiation of ToolTip, Default value for DateTimePicker, Combobox Enabling is complted. 
        // PARAMETERS   : object sender, EventArgs e
        // RETURNS      : void
        private void Form1_Load(object sender, EventArgs e)
        {
            toolTip = new System.Windows.Forms.ToolTip();
            dateTimePicker_start.MinDate = new DateTime(2020, 2, 29);
            dateTimePicker_end.MinDate = new DateTime(2020, 2, 29);

            dateTimePicker_start.Value = DateTime.Now;
            dateTimePicker_end.Value = DateTime.Now;
            comboBox_region.Enabled = false;
            comboBox_region.SelectedItem = "Canada";
        }

        // FUNCTION     : LoadFile_Click
        // DESCRIPTION  : Along with LoadFile button clicked, openFileDialoge is initilized and gets the file path and call InsertFileData for copying csv data to the table in the sql server. 
        // PARAMETERS   : object sender, EventArgs e
        // RETURNS      : void
        private void LoadFile_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    if (FileOperator.CountCsvColumn(filePath) == COLUMN_COUNTS_COVID19CASES)
                    {
                        FileOperator.InsertFileData(filePath, TABLE_COVID19CASES);
                    }
                    else if (FileOperator.CountCsvColumn(filePath) == COLUMN_COUNTS_LABINDICATORS)
                    {
                        FileOperator.InsertFileData(filePath, TABLE_LABINDICATORS);
                    }
                    else
                    {
                        MessageBox.Show("Please choose the correct file \n" +
                            "Covid19Cases or LabIndicators");
                    }
                }
            }
        }

        // FUNCTION     : comboBox_chart_SelectedIndexChanged
        // DESCRIPTION  : Along with the chart combobox selected, the related region combobox is enabled when the Test and Confirmed case is selected. 
        // PARAMETERS   : object sender, EventArgs e
        // RETURNS      : void
        private void comboBox_chart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_chart.SelectedIndex == 1)
            {
                comboBox_region.Enabled = true;
                dateTimePicker_start.MinDate = earlist_lab;
                dateTimePicker_end.MinDate = earlist_lab;
            }
            else
            {
                comboBox_region.SelectedItem = "Canada";
                comboBox_region.Enabled = false;
                //dateTimePicker_start.MinDate = new DateTime(2020, 2, 29);
                //dateTimePicker_end.MinDate = new DateTime(2020, 2, 29);
            }

            if (sender is System.Windows.Forms.ComboBox comboBox)
            {
                chartname = comboBox.SelectedIndex;
            }
        }

        // FUNCTION     : btnChart_Click
        // DESCRIPTION  : Along with the chart load button clicked, DisplayChart is called. 
        // PARAMETERS   : object sender, EventArgs e
        // RETURNS      : void
        private void btnChart_Click(object sender, EventArgs e)
        {
            theChart.Series.Clear();
            theChart.Titles.Clear();
            theChart.ChartAreas[0].RecalculateAxesScale(); // Reset axis scale
            theChart.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            theChart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
            theChart.ChartAreas[0].Position.Auto = true; // Reset auto-sizing

            if (chartname != -1)
            {
                DateTime sd = DateTime.ParseExact(startdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime ed = DateTime.ParseExact(enddate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (sd <= ed)
                {
                    if (chartname == 0 || chartname == 1)
                    {

                        DataTable data = DataOperator.Load(chartname, startdate, enddate, province_territory);

                        if (data != null && data.Rows.Count > 0)
                        {
                            if (chartname == 0)
                            {
                                theChart.ChartAreas[0].Area3DStyle.Enable3D = true;
                            }
                            else
                            {
                                theChart.ChartAreas[0].Area3DStyle.Enable3D = false;
                            }
                            DataOperator.DisplayChart(chartname, data, theChart);
                            

                            data.Clear();
                        }
                        else
                        {
                            theChart.Series.Clear();
                            theChart.Titles.Clear();
                            theChart.ChartAreas[0].Area3DStyle.Enable3D = false;
                            MessageBox.Show("No data for the selected period.");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Please select the correct menu.\n" +
                    "Covid19 Cases: Start date is not later than End date. \n" +
                    "Test and Confirmed Cases: Start date is not later than End date. Province/Territorry needs to be selected.");

                    }
                }
                else
                {
                    MessageBox.Show("Start date cannot be later than End date.");
                }
            }
            else
            {
                MessageBox.Show($"Please select the Chart.");
            }
        }



        // FUNCTION     : dateTimePicker_start_ValueChanged
        // DESCRIPTION  : This ValueChanged method gets the picked date and sets the start date. 
        // PARAMETERS   : object sender, EventArgs e
        // RETURNS      : void
        private void dateTimePicker_start_ValueChanged(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.DateTimePicker dtpicker)
            {
                startdate = dtpicker.Value.ToShortDateString();
            }
        }

        // FUNCTION     : dateTimePicker_end_ValueChanged
        // DESCRIPTION  : This ValueChanged method gets the picked date and sets the end date. 
        // PARAMETERS   : object sender, EventArgs e
        // RETURNS      : void
        private void dateTimePicker_end_ValueChanged(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.DateTimePicker dtpicker)
            {
                enddate = dtpicker.Value.ToShortDateString();
            }
        }

        // FUNCTION     : comboBox_region_SelectedIndexChanged
        // DESCRIPTION  : This Combobox method gets the picked province/territory from the user and assigns the selected value to the province_territory value. 
        // PARAMETERS   : object sender, EventArgs e
        // RETURNS      : void
        private void comboBox_region_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.ComboBox comboBox)
            {
                province_territory = comboBox.SelectedItem.ToString();
            }
        }

        // FUNCTION     : theChart_MouseMove
        // DESCRIPTION  : This MouseMove function gets MouseEvent and displays Tooltip when the mouse point hovers over the datapoint on the chart. 
        // PARAMETERS   : object sender, EventArgs e
        // RETURNS      : void
        private void theChart_MouseMove(object sender, MouseEventArgs e)
        {
            var mousepointer = theChart.HitTest(e.X, e.Y);
            string xValue;
            string yValue;
            if (mousepointer.ChartElementType == ChartElementType.DataPoint)
            {
                var point = mousepointer.Series.Points[mousepointer.PointIndex];
                if (chartname == 1)
                {
                    DateTime date = DateTime.FromOADate(point.XValue);
                    xValue = date.ToShortDateString();
                }
                else
                {
                    xValue = point.AxisLabel;
                }
                yValue = point.YValues[0].ToString();
                toolTip.Show($"{xValue}\n{yValue}", theChart, e.X + 15, e.Y - 15);
            }
            else
            {
                toolTip.Hide(theChart);
            }
        }
    }
}
