// FILE: MainWindow.xaml.cs
// PROJECT : PROG3070-Proejct-Knot
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-12-05
// DESCRIPTION: This is the code behind for the Form.
// The Assembly Line gets all workstations data and visualizes the workstation data.
// REFERENCE: https://www.youtube.com/watch?v=xxK1uIjfcqU
// https://learn.microsoft.com/en-us/visualstudio/get-started/csharp/tutorial-windows-forms-math-quiz-add-timer?view=vs-2022&tabs=csharp
// https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.timer?view=windowsdesktop-9.0

using Knot_AssemblyLine.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Knot_AssemblyLine
{
    delegate void SetTextDelegate(string text);
    delegate void SetLabelDelegate(Label label, string text);
    public partial class AssemblyLine : Form
    {
        private HubConnection connection;
        private static int intervalSelected;
        private static System.Timers.Timer refreshTimer;
        List<WorkstationJob> stationList = new List<WorkstationJob>();
        //Queue<List<ChartData>> staionJobs = new Queue<List<ChartData>>();
        // private static int orderqtytotal;

        int chartval1 = 0;
        
        public AssemblyLine()
        {
            InitializeComponent();

            // SignalR-connect to the SinglR hub
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7148/hub")
                .WithAutomaticReconnect()
                .Build();
            connection.Reconnecting += (sender) =>
            {
                var message = "Reconnecting";
                this.SetText(message);
                return Task.CompletedTask;
            };
            // SignalR-reconnected to the SinglR hub
            connection.Reconnected += (sender) =>
            {
                var message = "Reconnected";
                this.SetText(message);
                return Task.CompletedTask;
            };
            // SignalR-Closed to the SinglR hub
            connection.Closed += (sender) =>
            {
                var message = "Closed";
                this.SetText(message);
                return Task.CompletedTask;
            };
        }

        // FUNCTION: AssemblyLine_Load()
        // DESCRIPTION: AssemblyLine_Load makes the SignalR connection when the form is loaded 
        // and complete the Configuration object with the response
        // PARAMETER: none
        // RETURN: void
        private async void AssemblyLine_Load(object sender, EventArgs e)
        {
            // if user interval timer used then, SignalR no need
            try
            {
                connection.On<string, string>("ReceiveMessage", (AppContext, message) =>
                {
                    //this.SetText(message);
                });

                await connection.StartAsync();
            }
            catch (Exception ex)
            {
                this.SetText(ex.Message);
            }
            
        }

        // FUNCTION: SetText()
        // DESCRIPTION: SetText invokes the deligate and sets the UI object's (Listbox) text.
        // PARAMETER: none
        // RETURN: void
        
        public void SetText(string text)
        {
            if (this.statusListbox.InvokeRequired)
            {
                SetTextDelegate deli = new SetTextDelegate(SetText);
                this.Invoke(deli, new object[] { text });
            }
            else
            {
                this.statusListbox.Items.Add(text);
            }
        }

        // FUNCTION: SetLabelText()
        // DESCRIPTION: SetLabelText invokes the deligate and sets the UI object's(Labels) text.
        // PARAMETER: Label label, string text
        // RETURN: void
        public void SetLabelText(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                SetLabelDelegate d = new SetLabelDelegate(SetLabelText);
                this.Invoke(d, new object[] { label, text });
            }
            else
            {
                label.Text = text;
            }
        }

        // FUNCTION: setIntervalButton_Click()
        // DESCRIPTION: the function sets the refreshTimer with the combobox selection value.
        // PARAMETER: Label label, string text
        // RETURN: void
        private void setIntervalButton_Click(object sender, EventArgs e)
        {
            int[] interval = { 1000, 5000, 10000 }; // milliseconds
            intervalSelected = intervalComboBox.SelectedIndex;
            // setIntervalButton.Enabled = false;
            // index value [1,5,10], set timer
            refreshTimer = new System.Timers.Timer(interval[intervalSelected]);
            refreshTimer.Elapsed += OnTimedEvent;
            refreshTimer.AutoReset = true;
            refreshTimer.Enabled = true;

            // this.SetText($"{intervalSelected} is selected");

            theChart.Titles[0].Text = "Yield Quantity by Station";
            theChart.Series[0].ChartType = SeriesChartType.Column;
            theChart.Series[0].LegendText = "Workstation";

        }


        // FUNCTION: OnTimedEvent()
        // DESCRIPTION: the function calls GetData() when the refresh timer elapsed.
        // PARAMETER: Ojbect source, ElapsedEventArgs e
        // RETURN: void
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            // DEBUG messages - 
            this.SetText("ontime event");

            // get data and display chart and complete the UI
            stationList.Clear();
            GetData();     
        }

        // FUNCTION: GetData()
        // DESCRIPTION: the function gets the workstation data and completes the UI with the data.
        // PARAMETER: none
        // RETURN: void
        private async void GetData()
        {
            int[] seriesdata = new int[5];
            var apiResponseObj = await ApiOperator.GetWorkstationjob();
            var orderAmount = apiResponseObj.Sum(st => st.OrderAmount);
            var orderqtytotal = (int)apiResponseObj.Sum(st => st.OrderQuantity);
            var producetotal = (int)apiResponseObj.Sum(st => st.YieldQuantity);

            // inprocessAmount, totalOrderAmount, qtyProduced, qtyYield
            int yield = (int)((double)producetotal * 0.99);
            double processAmt = (double)(orderAmount / orderqtytotal * yield);
            double orderAmt = (double)orderAmount;

            this.SetLabelText(inProcessAmountLabel, processAmt.ToString("#.##"));
            this.SetLabelText(orderAmountLabel, orderAmt.ToString("#.##"));
            this.SetLabelText(qtyProducedLabel, producetotal.ToString());
            this.SetLabelText(quantityYiledLabel, yield.ToString());
            try
            {
                orderAmountProgressbar.Maximum = (int)orderAmt;
                if (processAmt > orderAmt)
                {
                    orderAmountProgressbar.Value = (int)orderAmt;
                }
                else
                {
                    orderAmountProgressbar.Value = (int)processAmt;
                }
                
            }
            catch (Exception ex)
            {

                this.SetText(ex.Message); // DEBUG
            }
            

            foreach (var obj in apiResponseObj)
            {
                if(obj.StationName == "Station1")
                {
                    seriesdata[0] = (int)obj.YieldQuantity;
                }
                if (obj.StationName == "Station2")
                {
                    seriesdata[1] = (int)obj.YieldQuantity;
                }
                if (obj.StationName == "Station3")
                {
                    seriesdata[2] = (int)obj.YieldQuantity;
                }
            }

            try
            {
                if (theChart.Series[0].Points.Count < 3)
                {
                    theChart.Series[0].Points.AddXY(1, (double)seriesdata[0]);
                    theChart.Series[0].Points.AddXY(2, (double)seriesdata[1]);
                    theChart.Series[0].Points.AddXY(3, (double)seriesdata[2]);
                }
                else 
                {
                    theChart.Series[0].Points[0].SetValueY((double)seriesdata[0]);
                    theChart.Series[0].Points[1].SetValueY((double)seriesdata[1]);
                    theChart.Series[0].Points[2].SetValueY((double)seriesdata[2]);
                }
            }
            catch (Exception ex)
            {
                this.SetText(ex.Message); // DEBUG
            }
        }

    }
}
