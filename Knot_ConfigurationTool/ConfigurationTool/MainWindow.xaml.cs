// FILE: MainWindow.xaml.cs
// PROJECT : PROG3070-Proejct-Knot
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-11-18
// DESCRIPTION: This is the code behind for the file of MainWindows.xaml of Simulator Configuration Tool.
// The Simulator Configuration Manager Tool takes user input for the configuration into the Datagrid.
// Upon the user's configuration data entry into the Datagrid, the configuration data can be inserted into the configuration table of the database for Manufacturing Simulation.
// https://learn.microsoft.com/en-us/answers/questions/177198/datagrid-data-from-excel?orderBy=Helpful
// https://stackoverflow.com/questions/3046003/adding-a-button-to-a-wpf-datagrid
// https://www.youtube.com/watch?v=aWePkE2ReGw
// https://www.youtube.com/watch?v=VSAlIE2SFHw
// https://www.youtube.com/watch?v=ufHlJLPK5CA
// https://www.conradakunga.com/blog/using-httpclient-to-post-json-in-c-net/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ConfigurationTool
{
    // NAME     : MainWindow
    // PURPOSE  : The MainWindow class is a code behind of MainWindows.xaml. 
    public partial class MainWindow : Window
    {
        ObservableCollection<Configuration> ConfigData = new ObservableCollection<Configuration>();
        List<Configuration> DataDbAdded = new List<Configuration>();
        Configuration initialConf = new Configuration();
        public string ConfigIdSelected { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ApiHelper.InitializeClient();

            Guid uid = Guid.NewGuid();
            initialConf.ConfigurationId = "CID45630-1";
            initialConf.SimulationTime = 60;
            initialConf.StationName = "Station1";
            initialConf.OrderNumber = "Order1";
            initialConf.OrderQuantity = 150;
            initialConf.OrderAmount = 3750.00M;
            initialConf.ProductName = "Fog Lamp";
            initialConf.BatchNumber = "lot-CID45630-1";
            initialConf.FinishedGoodsTrayMaxQty = 20;
            initialConf.PartName1 = "Harness";
            initialConf.PartName2 = "Reflector";
            initialConf.PartName3 = "Housing";
            initialConf.PartName4 = "Lens";
            initialConf.PartName5 = "Bulb";
            initialConf.PartName6 = "Bezel";
            initialConf.ReplQtyPart1 = 55;
            initialConf.ReplQtyPart2 = 35;
            initialConf.ReplQtyPart3 = 24;
            initialConf.ReplQtyPart4 = 40;
            initialConf.ReplQtyPart5 = 60;
            initialConf.ReplQtyPart6 = 75;
            initialConf.PartThresholdQty = 5;
            initialConf.EmployeeName = "Yoda";
            initialConf.EmployeeSkillLevel = 0.85; // yield per min

            ConfigData.Add(initialConf);

            ConfigCombobox.ItemsSource = ConfigData;
            ConfigDatagrid.ItemsSource = ConfigData;
            this.DataContext = this;
        }

        // FUNCTION     : CommandBinding_PasteFromExcelClick_CanExecute
        // AUTHOR       : DaisyTian-1203
        // DATE         : 2020-11-26
        // DESCRIPTION  : This function binds the command for the clipboard input using ctrl+v, set e.CanExcecution status
        // REFERENCE    : https://learn.microsoft.com/en-us/answers/questions/177198/datagrid-data-from-excel?orderBy=Helpful
        // PARAMETERS   : object sender, CanExecuteRoutedEventArg e
        // RETURNS      : void
        private void CommandBinding_PasteFromExcelClick_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // FUNCTION     : CommandBinding_PasteFromExcelClick_Executed
        // AUTHOR       : DaisyTian-1203
        // DATE         : 2020-11-26
        // DESCRIPTION  : This function gets the Clipboard text and pastes data to the individual column of the datagrid, and updates the datagrid binded object.
        // REFERENCE    : https://learn.microsoft.com/en-us/answers/questions/177198/datagrid-data-from-excel?orderBy=Helpful
        // PARAMETERS   : object sender, CanExecuteRoutedEventArg e
        // RETURNS      : void
        private void CommandBinding_PasteFromExcelClick_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var clipboardContent = Clipboard.GetText();

            if (string.IsNullOrEmpty(clipboardContent)) return;
            var rows = clipboardContent
                .Split(new string[] { "\r\n" }, StringSplitOptions.None)
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            int selectedIndex = this.ConfigDatagrid.SelectedIndex;
            if (selectedIndex == ConfigDatagrid.Items.Count - 1)
            {
                foreach (var row in rows)
                {
                    var columns = row.Split('\t');
                    // if (columns.Count() < 24)
                    if (columns.Length < 24)
                    {
                        MessageBox.Show("Please select 24 columns in Excel.", "Clipboard content incorrect", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    var config_para = new Configuration
                    {
                        ConfigurationId = columns[0],
                        SimulationTime = Convert.ToInt32(columns[1]),
                        StationName = columns[2],
                        OrderNumber = columns[3],
                        OrderQuantity = Convert.ToInt32(columns[4]),
                        OrderAmount = Convert.ToDecimal(columns[5]),
                        ProductName = columns[6],
                        BatchNumber = columns[7],
                        FinishedGoodsTrayMaxQty = Convert.ToInt32(columns[8]),
                        PartName1 = columns[9],
                        PartName2 = columns[10],
                        PartName3 = columns[11],
                        PartName4 = columns[12],
                        PartName5 = columns[13],
                        PartName6 = columns[14],
                        ReplQtyPart1 = Convert.ToInt32(columns[15]),
                        ReplQtyPart2 = Convert.ToInt32(columns[16]),
                        ReplQtyPart3 = Convert.ToInt32(columns[17]),
                        ReplQtyPart4 = Convert.ToInt32(columns[18]),
                        ReplQtyPart5 = Convert.ToInt32(columns[19]),
                        ReplQtyPart6 = Convert.ToInt32(columns[20]),
                        PartThresholdQty = Convert.ToInt32(columns[21]),
                        EmployeeName = columns[22],
                        EmployeeSkillLevel = Convert.ToDouble(columns[23]),
                    };

                    this.ConfigData.Add(config_para);
                }
            }
        }

        // FUNCTION     : setConfigButton_Click
        // DESCRIPTION  : This function gets the Clipboard text, splits the text for individual data columns, and adds the object with the data into ObservableCollection.
        // PARAMETERS   : object sender, RoutedEventArgs e
        // RETURNS      : void
        private async void setConfigButton_Click(object sender, RoutedEventArgs e)
        {
            StatusMessage.Text = "";
            try
            {
                // No configuration id is selected
                if (string.IsNullOrEmpty(ConfigIdSelected))
                {
                    MessageBox.Show("ConfigurationId needs to be selected", "Configuration Tool", MessageBoxButton.OK);
                    return;
                }

                StatusMessage.Text += $"Messages: Requesting {ConfigIdSelected} to be added into the Database.";
                var cSelected = ConfigData.FirstOrDefault(c => c.ConfigurationId == ConfigIdSelected);
                if (cSelected == null)
                {
                    MessageBox.Show("Error: ConfigurationId does not exist", "Configuration Tool", MessageBoxButton.OK);
                    return;
                }
                // copy the setected data to the object for post call
                initialConf = new Configuration
                {
                    ConfigurationId = cSelected.ConfigurationId,
                    SimulationTime = cSelected.SimulationTime,
                    StationName = cSelected.StationName,
                    OrderNumber = cSelected.OrderNumber,
                    OrderQuantity = cSelected.OrderQuantity,
                    OrderAmount = cSelected.OrderAmount,
                    ProductName = cSelected.ProductName,
                    BatchNumber = cSelected.BatchNumber,
                    FinishedGoodsTrayMaxQty = cSelected.FinishedGoodsTrayMaxQty,
                    PartName1 = cSelected.PartName1,
                    PartName2 = cSelected.PartName2,
                    PartName3 = cSelected.PartName3,
                    PartName4 = cSelected.PartName4,
                    PartName5 = cSelected.PartName5,
                    PartName6 = cSelected.PartName6,
                    ReplQtyPart1 = cSelected.ReplQtyPart1,
                    ReplQtyPart2 = cSelected.ReplQtyPart2,
                    ReplQtyPart3 = cSelected.ReplQtyPart3,
                    ReplQtyPart4 = cSelected.ReplQtyPart4,
                    ReplQtyPart5 = cSelected.ReplQtyPart5,
                    ReplQtyPart6 = cSelected.ReplQtyPart6,
                    PartThresholdQty = cSelected.PartThresholdQty,
                    EmployeeName = cSelected.EmployeeName,
                    EmployeeSkillLevel = cSelected.EmployeeSkillLevel
                };
                // call api
                int apiRes = await ConfigOperator.PostConfig(initialConf);
                if (apiRes == 0)
                {
                    StatusMessage.Text += Environment.NewLine + $"Messages: {ConfigIdSelected} is added.";
                    MessageBox.Show("Configuration is added", "Configuration Tool", MessageBoxButton.OK);
                    StatusMessage.Text = "";
                    DataDbAdded.Add(initialConf);
                    ConfigData.Remove(cSelected);
                }
                else if (apiRes == 1)
                {
                    MessageBox.Show("Error: Api is not running. Retry Later.", "Configuration Tool", MessageBoxButton.OK, MessageBoxImage.Error);
                    StatusMessage.Text = "";
                }
                else
                {
                    MessageBox.Show("Error: Configuration Setting", "Configuration Tool", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // Api call for calling stored proceedure to insert initial data to the tables for the selected configuration
                var result = await ConfigOperator.PostInitialSP(initialConf.ConfigurationId);

            }
            catch (Exception ex)
            {
                StatusMessage.Text += $"Error: {ex.Message}";
            }
        }
    }
}