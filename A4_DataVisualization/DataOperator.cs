// FILE: DataOperator.cs
// PROJECT : PROG3070 - Assignment #4
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-11-07
// DESCRIPTION: This DataOperator.cs provides the functions for chart features.
// https://learn.microsoft.com/en-us/dotnet/api/system.data.datatable?view=net-8.0
// https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
// https://github.com/dotnet/winforms-datavisualization
// https://www.codeproject.com/Articles/274318/Line-Graph-Component-in-Csharp
// https://stackoverflow.com/questions/24736959/how-to-use-orderby-in-linq-for-datatable-in-asp-net

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace A4_DataVisualization
{
    // NAME     : DataOperator
    // PURPOSE  : The DataOperator class provides functions related to the chart visualization operation.
    public static class DataOperator
    {
        public static DataTable dt;

        // FUNCTION     : Load
        // DESCRIPTION  : This function takes chart_index (Covid19Cases or Test&Confirmed Cases) and start, end date, province/territory
        //                and call the QueryBuilder function and get the query result to the DataTable and return the DataTable
        // PARAMETERS   : int chart_index,string start_date, string end_date, string province_territory
        // RETURNS      : DataTable
        public static DataTable Load(int chart_index, string start_date, string end_date, string pr_tr)
        {
            string query = QueryBuilder(chart_index, start_date, end_date, pr_tr);
            dt = DBHelper.GetData(query);
            return dt;
        }

        // FUNCTION     : DisplayChart
        // DESCRIPTION  : This function takes chart_index (Covid19Cases or Test&Confirmed Cases) and DataTable, Chart
        //                and display the Pie or Line chart based on the chart_index
        // PARAMETERS   : int chart_index, DataTable datatable, Chart chart
        // RETURNS      : void
        public static void DisplayChart(int chart_index, DataTable datatable, Chart chart)
        {
            chart.Titles.Add("");
            chart.Titles[0].Font = new Font("Segoe UI", 14, FontStyle.Bold);

            if (chart_index == 0)
            {
                Series series = new Series()
                {
                    ChartType = SeriesChartType.Pie,
                    YValueType = ChartValueType.Double,
                    IsValueShownAsLabel = true,
                };
                series.SmartLabelStyle.Enabled = true;
                series.SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Left | LabelAlignmentStyles.Right | LabelAlignmentStyles.Top | LabelAlignmentStyles.Bottom;
                series.SmartLabelStyle.MinMovingDistance = 15;

                foreach (DataRow row in datatable.Rows)
                {
                    string label = row["prname"].ToString();
                    double value = row["totalcase"] != DBNull.Value ? Convert.ToDouble(row["totalcase"]) : 0; //DBNull error

                    DataPoint dataPoint = new DataPoint();
                    dataPoint.SetValueXY(label, value);
                    dataPoint.LegendText = $"{label}: {value:N0}";
                    series.Points.Add(dataPoint);
                }

                series.Font = new System.Drawing.Font("Segoe UI", 9F);
                series.Label = "#VAL{N0} (#PERCENT)";
                series["PieLabelStyle"] = "Outside";
                series["PieLineColor"] = "Black";
                chart.Series.Add(series);
                chart.Titles[0].Text = $"Total Number Of Cases";
            }
            if (chart_index == 1)
            {
                // line chart
                string prtr = datatable.Rows[1]["prname"].ToString();
                Series series_IndividualsTested = new Series("Number of Individuals Tested")
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 3
                };   
                Series series_ConfirmedCases = new Series("Number of Confirmed Cases")
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 3

                };

                foreach (DataRow row in datatable.Rows)
                {
                    DateTime date = Convert.ToDateTime(row["date"]);
                    double value_IndividualsTested = Convert.ToInt32(row["IndividualsTested"]);
                    series_IndividualsTested.Points.AddXY(date, value_IndividualsTested);
                    series_IndividualsTested.MarkerStyle = MarkerStyle.Circle;
                    series_IndividualsTested.MarkerSize = 4;
                    series_IndividualsTested.MarkerColor = Color.Blue;

                    double value_ConfirmedCases = Convert.ToInt32(row["ConfirmedCases"]);
                    series_ConfirmedCases.Points.AddXY(date, value_ConfirmedCases);
                    series_ConfirmedCases.MarkerStyle = MarkerStyle.Circle;
                    series_ConfirmedCases.MarkerSize = 4;
                    series_ConfirmedCases.MarkerColor = Color.Green;

                }

                chart.Series.Add(series_IndividualsTested);
                chart.Series.Add(series_ConfirmedCases);
                chart.Titles[0].Text = $"Individuals Tested and Confirmed Cases - {prtr}";
                
            }
            chart.DataBind();
        }

        // FUNCTION     : QueryBuilder
        // DESCRIPTION  : This function takes chart_index (Covid19Cases or Test&Confirmed Cases), start_date, end_date, and province_territory
        //                and completes the query for the chart_index and return the string of the query.
        // PARAMETERS   : int chart_index, string start_date, string end_date, string pr_tr
        // RETURNS      : string
        private static string QueryBuilder(int chart_index, string start_date, string end_date, string pr_tr)
        {
            string query = "";
            if (chart_index == 0)
            {
                query = $"SELECT prname, " +
               $"SUM(totalcases) AS totalcase " +
               $"FROM [A4DB].[dbo].[Covid19Cases] " +
               $"WHERE prname != 'Canada' " +
               $"AND date BETWEEN '{start_date}' AND '{end_date}'" +
               $"GROUP BY prname " +
               $"ORDER BY prname";
            }
            if (chart_index == 1)
            {
                query = $"SELECT prname, date, numtests_weekly AS IndividualsTested, " +
                    $"numtests_weekly * percentpositivity_weekly / 100 AS ConfirmedCases " +
                    $"FROM LabIndicators " +
                    $"WHERE date BETWEEN '{start_date}' AND '{end_date}' " +
                    $"AND (prname = '{pr_tr}') " +
                    $"ORDER BY 'date' ASC";
            }
            return query;
        }
    }
}
