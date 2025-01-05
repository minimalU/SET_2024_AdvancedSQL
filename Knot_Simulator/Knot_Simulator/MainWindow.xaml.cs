// FILE: MainWindow.xaml.cs
// PROJECT : PROG3070-Proejct-Knot
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-11-25
// DESCRIPTION: This is the code behind for the file of MainWindows.xaml of Knot_Simulator.
// The Simulator gets the configuration from the database for user's selection for their simulation.
// Upon the selection of configuration id, the simulator runs the simulating operations by updating the data of the db tables.
// https://learn.microsoft.com/en-us/dotnet/api/system.timers.timer?view=net-9.0
// https://stackoverflow.com/questions/10694271/c-sharp-multiple-backgroundworkers
// https://forums.codeguru.com/showthread.php?512040-Multiple-timers-calling-the-same-EventHandler-which-one-called-it

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows.Threading;
using System.ComponentModel;
using System.Timers;
using System.Linq;
using System.Windows.Input;


namespace Knot_Simulator
{
    // NAME     : MainWindow
    // PURPOSE  : The MainWindow class is a code behind of MainWindows.xaml.
    public partial class MainWindow : Window
    {
        HubConnection connection;
        ObservableCollection<Configuration> ObsConfigurations = new ObservableCollection<Configuration>();
        List<Configuration> configurations = new List<Configuration>();
        ObservableCollection<SimulationItem> Simulations = new ObservableCollection<SimulationItem>();
        public string SelectedConfiguration { get; set; }
        private const string signalSenderName = "Simulator";
        private BackgroundWorker simulationWoker;

        private static System.Timers.Timer stationTimer;
        private static System.Timers.Timer binSensorTimer;
        private static System.Timers.Timer runnerTimer;
        Dictionary<string, System.Timers.Timer> stationTimers = new Dictionary<string, System.Timers.Timer>();
        Dictionary<string, System.Timers.Timer> binTimers = new Dictionary<string, System.Timers.Timer>();
        Dictionary<string, System.Timers.Timer> runnerTimers = new Dictionary<string, System.Timers.Timer>();
        private const double MILLISECOND = 60000;

        private static int simulationTaskIndex;
        bool sendBinMessage = false;
        int runnerTimerInterval = 1; // min


        public MainWindow()
        {
            InitializeComponent();
            // on initialization, get a list of configurations from db
            ApiHelper.InitializeClient();
            // Api call to get the Configuration lists from the Configuration Table of KnotDB
            GetList();
            // Bind the data to UI combobox
            ConfListCombobox.ItemsSource = Simulations;
            simulationListbox.DataContext = Simulations;
            ConfListCombobox.ItemsSource = ObsConfigurations;
            ConfListCombobox.DataContext = ObsConfigurations;

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

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            simulationWoker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };

            simulationWoker.DoWork += simulationBackgroundWorker_DoWork;
            simulationWoker.ProgressChanged += simulationBackgroundWorker_ProgressChanged;
            simulationWoker.RunWorkerCompleted += simulationBackgroundWorker_RunWorkerCompleted;

            // var signalTask = new Task(() => Signal(connection), TaskCreationOptions.LongRunning | TaskCreationOptions.PreferFairness);
        }

        ///// Development - Debug
        private async void openConnection_Click(object sender, RoutedEventArgs e)
        {
            connection.On<string, string>("ReceiveMessage", (app, message) =>
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        string newMeswsage = $"{app}: {message}";
                        statusListbox.Items.Add(newMeswsage);
                    });
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        statusListbox.Items.Add(ex.Message);
                    });
                }
            });
        }
        ///// Development - Debug
        private async void sendMessage_Click(object sender, RoutedEventArgs e)
        {
            connection.On<string, string>("ReceiveMessage", (app, message) =>
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        string newMeswsage = $"{app}: {message}";
                        statusListbox.Items.Add(newMeswsage);
                    });
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        statusListbox.Items.Add(ex.Message);
                    });
                }
            });

        }

        
        // FUNCTION: GetList()
        // DESCRIPTION: Call ApiOperator to Get the configuration list from Configuration table of Knot DB
        // and complete the Configuration object with the response
        // PARAMETER: none
        // RETURN: void
        private async void GetList()
        {
            try
            {
                configurations = await ApiOperator.GetConfigList();

                foreach (var configuration in configurations)
                {
                    ObsConfigurations.Add(configuration);
                }
            }
            catch (Exception ex)
            {
                statusListbox.Items.Add($"Error: ApiOperator-No connection. cannot complete configuration items." + Environment.NewLine + ex.Message);
            }
        }


        // FUNCTION: startSimulation_Click()
        // DESCRIPTION: The funcion starts the connection to SignalR Hub and completes configuration list for user's selection for the simulation,
        // and starts the timers for sending signals to operate bin qty and produced qty in the backend.                                                                                                                    
        // PARAMETER: none
        // RETURN: void
        private async void startSimulation_Click(object sender, RoutedEventArgs e)
        {
            // check if the input is valid
            try
            {
                SelectedConfiguration = ConfListCombobox.SelectedValue.ToString();
                if (string.IsNullOrEmpty(SelectedConfiguration))
                {
                    MessageBox.Show("ConfigurationId needs to be selected", "Knot Simulator", MessageBoxButton.OK);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ConfigurationId needs to be selected", "Knot Simulator", MessageBoxButton.OK);
            }
            // SingalR ConnectionON, StartAsync
            // if disconnected.
            
            connection.On<string, string>("ReceiveMessage", (app, message) =>
            {
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
                        statusListbox.Items.Add($"StartSimulation_SignalR_ConnectionOn{ex.Message}");
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
                    statusListbox.Items.Add($"StartSimulation_SignalR_StartAsync {ex.Message}");
                });
            }

            try
            {
                // Add the selected configuration item to simulation list
                var item = new SimulationItem() { SimulationId = (Simulations.Count + 1).ToString(), ConfigurationId = SelectedConfiguration };
                Simulations.Add(item);
                
                var simulationtarget = new Configuration();
                for (int i = 0; i < ObsConfigurations.Count; i++)
                {
                    if (SelectedConfiguration == ObsConfigurations[i].ConfigurationId)
                    {
                        simulationtarget.ConfigurationId = ObsConfigurations[i].ConfigurationId;
                        simulationtarget.SimulationTime = ObsConfigurations[i].SimulationTime;
                        simulationtarget.StationName = ObsConfigurations[i].StationName;
                        simulationtarget.OrderNumber = ObsConfigurations[i].OrderNumber;
                        simulationtarget.OrderQuantity = ObsConfigurations[i].OrderQuantity;
                        simulationtarget.OrderAmount = ObsConfigurations[i].OrderAmount;
                        simulationtarget.ProductName = ObsConfigurations[i].ProductName;
                        simulationtarget.BatchNumber = ObsConfigurations[i].BatchNumber;
                        simulationtarget.FinishedGoodsTrayMaxQty = ObsConfigurations[i].FinishedGoodsTrayMaxQty;
                        simulationtarget.PartName1 = ObsConfigurations[i].PartName1;
                        simulationtarget.PartName2 = ObsConfigurations[i].PartName2;
                        simulationtarget.PartName3 = ObsConfigurations[i].PartName3;
                        simulationtarget.PartName4 = ObsConfigurations[i].PartName4;
                        simulationtarget.PartName5 = ObsConfigurations[i].PartName5;
                        simulationtarget.PartName6 = ObsConfigurations[i].PartName6;
                        simulationtarget.ReplQtyPart1 = ObsConfigurations[i].ReplQtyPart1;
                        simulationtarget.ReplQtyPart2 = ObsConfigurations[i].ReplQtyPart2;
                        simulationtarget.ReplQtyPart3 = ObsConfigurations[i].ReplQtyPart3;
                        simulationtarget.ReplQtyPart4 = ObsConfigurations[i].ReplQtyPart4;
                        simulationtarget.ReplQtyPart5 = ObsConfigurations[i].ReplQtyPart5;
                        simulationtarget.ReplQtyPart6 = ObsConfigurations[i].ReplQtyPart6;
                        simulationtarget.PartThresholdQty = ObsConfigurations[i].PartThresholdQty;
                        simulationtarget.EmployeeName = ObsConfigurations[i].EmployeeName;
                        simulationtarget.EmployeeSkillLevel = ObsConfigurations[i].EmployeeSkillLevel;

                        simulationTaskIndex = i;
                    }
                }

                // Api call for calling stored proceedure to insert initial data to the tables for the selected configuration
                statusListbox.Items.Add($"{SelectedConfiguration}"); //DEBUG
                // var result = await ApiOperator.PostInitialSP(SelectedConfiguration); // move to the Configuration application

                // backgroundworker
                // statusListbox.Items.Add($"backgroundworker: started.");
                if (simulationWoker.IsBusy != true)
                {
                    simulationWoker.RunWorkerAsync(simulationtarget);
                    statusListbox.Items.Add($"Simulator:{simulationtarget.ConfigurationId} starts.");
                }

                // central timer - runner
                if (Simulations.Count == 1)
                {
                    System.Timers.Timer timer_runner = new System.Timers.Timer();
                    timer_runner.Elapsed += RunnerOnTimedEvent;
                    timer_runner.Interval = MILLISECOND * runnerTimerInterval; // 5 mins
                    // timer_runner.Interval = MILLISECOND; // timer_runner.Interval = MILLISECOND; // 1 min
                    timer_runner.Enabled = true;
                    runnerTimers.Add("runnerCentralTimer", timer_runner);
                    timer_runner.Start();
                    string starttime = DateTime.Now.ToString();
                    await connection.InvokeAsync("SendMessage", signalSenderName, $"runner=runnerCentralTimer=start={runnerTimerInterval}");
                }

            }
            catch (Exception ex)
            {
                statusListbox.Items.Add($"Error:StartSimulation_Simulation{ex.Message}");
            }
        }

        // FUNCTION: listboxRemoveSimulation_Click()
        // DESCRIPTION: Along with user's cancellation for the simulation of specific Configuration,
        // the funcion dispose the timers for the simulation.
        // PARAMETER: none
        // RETURN: void
        private void listboxRemoveSimulation_Click(object sender, RoutedEventArgs e)
        {
            // Dispose Timers and Cleanup Dictionary
            Button command = (Button)sender;
            string configuration_id = null;

            try
            {
                if (command.DataContext is SimulationItem)
                {
                    SimulationItem deleteitem = (SimulationItem)command.DataContext;
                    configuration_id = deleteitem.ConfigurationId;
                    Simulations.Remove(deleteitem);
                }

                var thetimer_bin = binTimers.Where(kvp => kvp.Key == configuration_id).Select(kvp => kvp.Value).FirstOrDefault();
                if (thetimer_bin != null)
                {
                    thetimer_bin.Stop();
                    thetimer_bin.Dispose();
                    binTimers.Remove(configuration_id);
                }

                //var thetimer_runner = runnerTimers.Where(kvp => kvp.Key == configuration_id).Select(kvp => kvp.Value).FirstOrDefault();
                //if (thetimer_runner != null)
                //{
                //    thetimer_runner.Stop();
                //    thetimer_runner.Dispose();
                //    runnerTimers.Remove(configuration_id);
                //}

                var thetimer_station = stationTimers.Where(kvp => kvp.Key == configuration_id).Select(kvp => kvp.Value).FirstOrDefault();
                if (thetimer_station != null)
                {
                    thetimer_station.Stop();
                    thetimer_station.Dispose();
                    stationTimers.Remove(configuration_id);
                }
                statusListbox.Items.Add($"Simulation - {configuration_id} is cancelled");

                // if the running simulation
                if (Simulations.Count < 1)
                {
                    var thetimer_runner = runnerTimers.Where(kvp => kvp.Key == "runnerCentralTimer").Select(kvp => kvp.Value).FirstOrDefault();
                    thetimer_runner.Stop();
                    thetimer_runner.Dispose();
                    runnerTimers.Remove("runnerCentralTimer");
                }

                // remove the background worker too
                if (simulationWoker.IsBusy)
                {
                    simulationWoker.CancelAsync();
                    simulationWoker.Dispose();
                }
                

            }
            catch (Exception ex)
            {
                statusListbox.Items.Add($"Error: RemoveSimulationCicked {ex.Message}");
            }
        }


        // FUNCTION: simulationBackgroundWorker_DoWork()
        // DESCRIPTION: BackgroundWorker function sets the timers for bin and runner, OnInitialized() backgroundworker instantiated and is started from startSimulation_Click
        // the funcion dispose the timers for the simulation.
        // PARAMETER: none
        // RETURN: void
        private void simulationBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Configuration simulation_target = (Configuration)e.Argument;

            // the Assembly Station simulation program which runs a timer, turns parts into products, and manages the resupply runner
            // set a timer
            // a. 1 min / employee skill level = transaction tick, supply tick = 5mins
            // b. BinSensorOnTimedEvent parts = -1 and product= +1
            // c. if the supply tick = bin's part qty + replenishmentQty

            // timerlist (configurationID, simuulationTime, skilllevel(ex.0.15), bintimer, runnerstimer)
            string conf_id = simulation_target.ConfigurationId;

            System.Timers.Timer timer_bin = new System.Timers.Timer();
            timer_bin.Elapsed += BinSensorOnTimedEvent;
            timer_bin.Interval = MILLISECOND * simulation_target.EmployeeSkillLevel;
            // timer_bin.Interval = 2000;
            timer_bin.Enabled = true;
            binTimers.Add(conf_id, timer_bin);
            timer_bin.Start();


            //System.Timers.Timer timer_runner = new System.Timers.Timer();
            //timer_runner.Elapsed += RunnerOnTimedEvent;
            //timer_runner.Interval = MILLISECOND * 5; // 5 mins
            //// timer_runner.Interval = MILLISECOND; // 1 min
            //timer_runner.Enabled = true;
            //runnerTimers.Add(conf_id, timer_runner);
            //timer_runner.Start();

            System.Timers.Timer timer_station = new System.Timers.Timer();
            timer_station.Elapsed += StationOnTimedEvent;
            timer_station.Interval = MILLISECOND * simulation_target.SimulationTime;
            timer_station.Enabled = true;
            stationTimers.Add(conf_id, timer_station);
            timer_station.Start();
        }

        // FUNCTION: simulationBackgroundWorker_ProgressChanged()
        // DESCRIPTION: Development test function
        // PARAMETER: none
        // RETURN: void
        private void simulationBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            statusListbox.Items.Add($"{e.ProgressPercentage.ToString()}%");
        }

        // FUNCTION: imulationBackgroundWorker_RunWorkerCompleted()
        // DESCRIPTION: Development test function
        // PARAMETER: none
        // RETURN: void
        private void simulationBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                MessageBox.Show("Cancel");
            }
            else if (e.Error != null)
            {
                statusListbox.Items.Add($"Error: {e.Error.Message}");
            }
            else
            {
                statusListbox.Items.Add($"Done");
            }
        }

        // FUNCTION: BinSensorOnTimedEvent()
        // DESCRIPTION: BinSensorOnTimeEvent() is called when the Bin timer elapsed.
        // PARAMETER: none
        // RETURN: void
        public async void BinSensorOnTimedEvent(Object source, ElapsedEventArgs e)
        {
            var thetimer = (from tmpTimer in binTimers where tmpTimer.Value.Equals(source) select tmpTimer).FirstOrDefault();
            
            ///// BinSensorOnTimedEvent parts = -1 and product = +1
            ///// MessageBox.Show("Binsensor");
            await connection.InvokeAsync("SendMessage", signalSenderName, $"bin={thetimer.Key}");
        }

        // FUNCTION: RunnerOnTimedEvent()
        // DESCRIPTION: RunnerOnTimedEvent() is called when the Runner timer elapsed.
        // PARAMETER: none
        // RETURN: void
        public async void RunnerOnTimedEvent(Object source, ElapsedEventArgs e)
        {

            var thetimer = (from tmpTimer in runnerTimers where tmpTimer.Value.Equals(source) select tmpTimer).FirstOrDefault();
            //string now = DateTime.Now.ToString();
            await connection.InvokeAsync("SendMessage", signalSenderName, $"runner=runnerCentralTimer=elapsed={runnerTimerInterval}");
            // await connection.InvokeAsync("SendMessage", signalSenderName, $"runner={thetimer.Key};timerstatus=elapsed;timerinterval={runnerTimerInterval}");
        }

        // FUNCTION: StationOnTimedEvent()
        // DESCRIPTION: StationOnTimedEvent() is called when the station simulation timer elapsed.
        // PARAMETER: none
        // RETURN: void
        public async void StationOnTimedEvent(Object source, ElapsedEventArgs e)
        {
            ///// MessageBox.Show("Station");
            var thetimer = (from tmpTimer in stationTimers where tmpTimer.Value.Equals(source) select tmpTimer).FirstOrDefault();
            await connection.InvokeAsync("SendMessage", signalSenderName, $"station={thetimer.Key}");
        }
    }
}
