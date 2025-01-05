// FILE: MainWindow.xaml.cs
// PROJECT : PROG3070-Proejct-Knot
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-12-05
// DESCRIPTION: This is the code behind for the file of MainWindows.xaml of Knot_WorkstationAndon.
// The Workstation Andon gets the data for bins and workstation and completes the UI with data.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.AspNetCore.SignalR.Client;

namespace Knot_WorkstationAndon
{
    
    public partial class MainWindow : Window
    {
        HubConnection connection;
        private static string line_message;
        List<BinWithWorkstationjob> binworkstationjobs = new List<BinWithWorkstationjob>();
        ObservableCollection<BinWithWorkstationjob> entireWorkstations = new ObservableCollection<BinWithWorkstationjob>();
        ObservableCollection<WorkStation> stationList = new ObservableCollection<WorkStation>();
        public string SelectedStation { get; set; }
        ObservableCollection<Bin> stationBins = new ObservableCollection<Bin>();


        public MainWindow()
        {
            InitializeComponent();

            GetEntireStations();
            

            // Bind the data to UI combobox
            StationsCombobox.ItemsSource = stationList;
            StationsCombobox.DataContext = stationList;

            HarnessLabel.DataContext = stationBins;

            ResetUI();

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
                    statusListbox.Items.Add(newMessage);
                });
                return Task.CompletedTask;
            };
            // SignalR-reconnected to the SinglR hub
            connection.Reconnected += (sender) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = "Reconnected!";
                    statusListbox.Items.Clear();
                    statusListbox.Items.Add(newMessage);
                });
                return Task.CompletedTask;
            };
            // SignalR-Closed to the SinglR hub
            connection.Closed += (sender) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = "Closed!";
                    statusListbox.Items.Add(newMessage);
                });
                return Task.CompletedTask;
            };
        }

        // FUNCTION: GetEntireStations()
        // DESCRIPTION: Call ApiOperator to Get the Bin and Workstation data for the entire stations
        // PARAMETER: none
        // RETURN: void
        private async void GetEntireStations()
        {
            try
            {
                var apiResponseObj = await ApiOperator.GetViewBinWithWorkstation();

                foreach (var obj in apiResponseObj)
                {
                    entireWorkstations.Add(obj);
                }

                var results = apiResponseObj.Select(obj => new { stName = obj.StationName }).Distinct();
                foreach (var result in results)
                {
                    stationList.Add(new WorkStation { StationName = result.stName });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please connect to the server");
                return;
                // statusListbox.Items.Add($"Error: Initial server connection error. Please connect when the server is ready" + Environment.NewLine + ex.Message);
            }
        }

        // FUNCTION: Button_Click()
        // DESCRIPTION: Along with user's start button click, the function makes the connection to the Signal hub.
        // Upon the message received, the function gets the bins and workstations data and completes the UI with the data
        // PARAMETER: none
        // RETURN: void
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (StationsCombobox.SelectedValue == null)
            {
                MessageBox.Show("Please select station");
                return;
            }

            SelectedStation = StationsCombobox.SelectedValue.ToString();
            statusListbox.Items.Add($"Debug: SelectedStation " + SelectedStation);
            ResetUI();

            // SingalR ConnectionON, StartAsync
            connection.On<string, string>("ReceiveMessage", async (app, message) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    statusListbox.Items.Add($"{app} : {message}");
                });
                try
                {
                    if (app == "Simulator")
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            statusListbox.Items.Add("processing simulator message");
                        });
                        var apiResponseObj = await ApiOperator.GetViewBinWithWorkstation();
                        
                        var selectedStationBins = apiResponseObj.Where(st => st.StationName == SelectedStation).ToList();

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            runnerListbox.Items.Clear();
                            HarnessLabel.Background = new SolidColorBrush(Colors.Green);
                            ReflectorLabel.Background = new SolidColorBrush(Colors.Green);
                            HousingLabel.Background = new SolidColorBrush(Colors.Green);
                            LensLabel.Background = new SolidColorBrush(Colors.Green);
                            BulbLabel.Background = new SolidColorBrush(Colors.Green);
                            BezelLabel.Background = new SolidColorBrush(Colors.Green);

                            yieldQtyLabel.Content = selectedStationBins[0].YieldQuantity;
                            targetQtyLabel.Content = selectedStationBins[0].OrderQuantity;
                            qtyProgressbar.Maximum = (double)selectedStationBins[0].OrderQuantity;
                            qtyProgressbar.Value = (double)selectedStationBins[0].YieldQuantity;

                            HarnessLabel.Content = selectedStationBins[0].PartQuantity;
                            ReflectorLabel.Content = selectedStationBins[1].PartQuantity;
                            HousingLabel.Content = selectedStationBins[2].PartQuantity;
                            LensLabel.Content = selectedStationBins[3].PartQuantity;
                            BulbLabel.Content = selectedStationBins[4].PartQuantity;
                            BezelLabel.Content = selectedStationBins[5].PartQuantity;
                            
                            if (selectedStationBins[0].IsReplenishmentNeeded == true)
                            {
                                HarnessLabel.Background = new SolidColorBrush(Colors.Red);
                                runnerListbox.Items.Add("Runner Signal: Bin ID-" + selectedStationBins[0].BinId + $" >> Harness: " + selectedStationBins[0].PartQuantity);
                            }
                            if (selectedStationBins[1].IsReplenishmentNeeded == true)
                            {
                                ReflectorLabel.Background = new SolidColorBrush(Colors.Red);
                                runnerListbox.Items.Add("Runner Signal: Bin ID-" + selectedStationBins[1].BinId + $" >> Reflector: " + selectedStationBins[1].PartQuantity);
                            }
                            if (selectedStationBins[2].IsReplenishmentNeeded == true)
                            {
                                HousingLabel.Background = new SolidColorBrush(Colors.Red);
                                runnerListbox.Items.Add("Runner Signal: Bin ID-" + selectedStationBins[2].BinId + $" >> Housing: " + selectedStationBins[2].PartQuantity);
                            }
                            if (selectedStationBins[3].IsReplenishmentNeeded == true)
                            {
                                LensLabel.Background = new SolidColorBrush(Colors.Red);
                                runnerListbox.Items.Add("Runner Signal: Bin ID-" + selectedStationBins[3].BinId + $" >> Lens: " + selectedStationBins[3].PartQuantity);
                            }
                            if (selectedStationBins[4].IsReplenishmentNeeded == true)
                            {
                                BulbLabel.Background = new SolidColorBrush(Colors.Red);
                                runnerListbox.Items.Add("Runner Signal: Bin ID-" + selectedStationBins[4].BinId + $" >> Bulb: " + selectedStationBins[4].PartQuantity);
                            }
                            if (selectedStationBins[5].IsReplenishmentNeeded == true)
                            {
                                BezelLabel.Background = new SolidColorBrush(Colors.Red);
                                runnerListbox.Items.Add("Runner Signal: Bin ID-" + selectedStationBins[5].BinId + $" >> Bezel: " + selectedStationBins[5].PartQuantity);
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        statusListbox.Items.Add(ex);
                    });
                }

                try
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        string new_message = $"{app}: {message}";
                        statusListbox.Items.Add(new_message);
                    });
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        statusListbox.Items.Add($"Error:StartSimulation_SignalR_ConnectionOn{ex.Message}");
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
                    statusListbox.Items.Add($"Error:StartSimulation_SignalR_StartAsync{ex.Message}");
                });
            }
        }

        // FUNCTION: ResetUI()
        // DESCRIPTION: ResetUI makes the default UI setting for the app.
        // PARAMETER: none
        // RETURN: void
        private void ResetUI()
        {
            runnerListbox.Items.Clear();
            HarnessLabel.Background = new SolidColorBrush(Colors.Gray);
            ReflectorLabel.Background = new SolidColorBrush(Colors.Gray);
            HousingLabel.Background = new SolidColorBrush(Colors.Gray);
            LensLabel.Background = new SolidColorBrush(Colors.Gray);
            BulbLabel.Background = new SolidColorBrush(Colors.Gray);
            BezelLabel.Background = new SolidColorBrush(Colors.Gray);

            yieldQtyLabel.Content = null;
            targetQtyLabel.Content = null;
            qtyProgressbar.Maximum = (double)100;
            qtyProgressbar.Value = (double)0;

            HarnessLabel.Content = null;
            ReflectorLabel.Content = null;
            HousingLabel.Content = null;
            LensLabel.Content = null;
            BulbLabel.Content = null;
            BezelLabel.Content = null;
        }

        private void connectionButton_Click(object sender, RoutedEventArgs e)
        {
            GetEntireStations();
        }
    }
}