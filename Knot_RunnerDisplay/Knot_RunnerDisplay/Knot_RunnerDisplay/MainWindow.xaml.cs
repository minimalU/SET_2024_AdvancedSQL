// FILE: MainWindow.xaml.cs
// PROJECT : PROG3070-Proejct-Knot
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-12-05
// DESCRIPTION: This is the code behind for the file of MainWindows.xaml of Knot_RunnerDisplay.
// Along with the backend messages, the RunnerDisplay sets the runner's timer and makes the Api call for bins and workstation, and updates the data info on UI.

using Knot_RunnerDisplay.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Knot_RunnerDisplay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatchertimer?view=windowsdesktop-9.0
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;
        private static string line_message;
        List<BinWithWorkstationjob> binworkstationjobs = new List<BinWithWorkstationjob>();
        List<BinWithWorkstationjob> fullBins = new List<BinWithWorkstationjob>();
        List<BinWithWorkstationjob> watchBins = new List<BinWithWorkstationjob>();
        List<BinWithWorkstationjob> replenishBins = new List<BinWithWorkstationjob>();

        ObservableCollection<BinWithWorkstationjob> entireWorkstations = new ObservableCollection<BinWithWorkstationjob>();
        public string SelectedStation { get; set; }
        ObservableCollection<Bin> stationBins = new ObservableCollection<Bin>();


        private readonly DispatcherTimer runnertimer = new DispatcherTimer();
        private TimeSpan endTime;
        private int runnerCenteralTime;


        // NAME     : MainWindow
        // PURPOSE  : The MainWindow class is a code behind of MainWindows.xaml.
        public MainWindow()
        {
            InitializeComponent();

            // SignalR-connect to the SinglR hub
            connection = new HubConnectionBuilder()
               .WithUrl("https://localhost:7148/hub")
               .WithAutomaticReconnect()
            .Build();
            // SignalR-reconnecting to the SinglR hub
            connection.Reconnecting += (sender) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = "Reconnecting...";
                    // greenListbox.Items.Add(newMessage);
                });
                return Task.CompletedTask;
            };
            // SignalR-reconnected to the SinglR hub
            connection.Reconnected += (sender) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = "Reconnected!";
                    
                    // greenListbox.Items.Add(newMessage);
                });
                return Task.CompletedTask;
            };
            // SignalR-Closed to the SinglR hub
            connection.Closed += (sender) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = "Closed!";
                    // greenListbox.Items.Add(newMessage);
                });
                return Task.CompletedTask;
            };
            
        }

        // FUNCTION: dispatcherTimer_Tick()
        // DESCRIPTION: dispatcherTimer_Tick is called when the timer ticks, and updates the timerLabel with the countdown time.
        // PARAMETER: none
        // RETURN: void
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan now = DateTime.Now.TimeOfDay;

            if (endTime > now)
            {
                timerLabel.Content = (endTime - now).ToString(@"mm\:ss");
            }
            
        }

        // FUNCTION: runnerTimer()
        // DESCRIPTION: runnerTimer sets interval and tick of the runner timer.
        // PARAMETER: none
        // RETURN: void
        private void runnerTimer(int timerinterval)
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            int timerinterval_sec = timerinterval * 60;
            // endTime = now + TimeSpan.FromSeconds(300);
            endTime = now + TimeSpan.FromSeconds(timerinterval_sec);
            runnertimer.Stop();
            runnertimer.Interval = TimeSpan.FromSeconds(1);
            runnertimer.Tick += new EventHandler(dispatcherTimer_Tick);
            runnertimer.Start();
        }

        // FUNCTION: connectionButton_Click()
        // DESCRIPTION: connectionButton_Click starts the connection to SignalR hub in the backend,
        // and calls API to get the data for workstation and its bins and updates the UI with the data.
        // PARAMETER: none
        // RETURN: void
        private async void connectionButton_Click(object sender, RoutedEventArgs e)
        {
            connection.On<string, string>("ReceiveMessage", async (app, message) =>
            {
                if (app == "Simulator")
                {
                    try
                    {
                        var apiResponseObj = await ApiOperator.GetViewBinWithWorkstation();
                        fullBins.Clear();
                        watchBins.Clear();
                        replenishBins.Clear();
                        foreach (var obj in apiResponseObj)
                        {
                            if (obj.IsReplenishmentNeeded == true)
                            {
                                replenishBins.Add(obj);
                            }
                            else if (obj.IsReplenishmentNeeded == false && obj.PartQuantity < 10)
                            {
                                watchBins.Add(obj);
                            }
                            else
                            {
                                fullBins.Add(obj);
                            }
                        }

                        // check runner timer
                        var msg = message.Split('='); //$"runner=runnerCentralTimer=start={starttime}"
                        if (msg.Length > 3)
                        {
                            if (msg[0] == "runner")
                            {
                                runnerCenteralTime = Int32.Parse(msg[3]);
                                runnerTimer(runnerCenteralTime);
                            }
                        }

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            if (greenListbox.Items.Count != 0)
                            {
                                greenListbox.Items.Clear();
                            }
                            if (orangeListbox.Items.Count != 0)
                            {
                                orangeListbox.Items.Clear();
                            }
                            if (redListbox.Items.Count != 0)
                            {
                                redListbox.Items.Clear();
                            }
                            for (int i = 0; i < fullBins.Count; i++)
                            {
                                greenListbox.Items.Add($"{fullBins[i].StationName} > {fullBins[i].ProductName} : {fullBins[i].PartQuantity}");
                            }
                            for (int i = 0; i < watchBins.Count; i++)
                            {
                                orangeListbox.Items.Add($"{watchBins[i].StationName} > {watchBins[i].ProductName} : {watchBins[i].PartQuantity}");
                            }
                            for (int i = 0; i < replenishBins.Count; i++)
                            {
                                redListbox.Items.Add($"{replenishBins[i].StationName} > {replenishBins[i].ProductName} : {replenishBins[i].PartQuantity}");
                            }

                            // centralTimerLabel.Content = runnerCenteralTime;
                        });

                        

                    }
                    catch (Exception ex)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            greenListbox.Items.Add(ex);
                        });
                    }
                }

                try
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        string new_message = $"{app}: {message}";
                        // statusListbox.Items.Add(new_message);
                    });
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        // statusListbox.Items.Add($"Error:StartSimulation_SignalR_ConnectionOn{ex.Message}");
                    });
                }

            });
            try
            {
                await connection.StartAsync();
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // greenListbox.Items.Add($"Error:SignalR_StartAsync{ex.Message}");
                });
            }
        }
    }
}
