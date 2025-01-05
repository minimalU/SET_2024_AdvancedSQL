// FILE: MainWindow.xaml.cs
// PROJECT : PROG3070 - Assignment #3
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-10-24
// DESCRIPTION: This is the code behind the file of MainWindows.xaml and includes
// the Trasfer_Click function that calls the related functions to complete the data copy from the source table and transfer to the destination table,
// the DataLinkDialButton_Click function that brings the Data Link Property Dialog and makes the user enter SQL database connection information.
// https://weblogs.asp.net/jongalloway/showing-a-connection-string-prompt-in-a-winform-application
// https://www.codeproject.com/Articles/6080/Using-DataLinks-to-get-or-edit-a-connection-string
// https://learn.microsoft.com/en-us/dotnet/api/system.data.datatable?view=net-8.0
// https://learning.oreilly.com/library/view/ado-net-cookbook/0596004397/ch02s02.html
// https://learn.microsoft.com/en-us/dotnet/api/system.data.datacolumn?view=net-8.0

using System;
using System.Data.OleDb;
using System.Text;
using System.Windows;
using MSDASC;
using ADODB;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using DataTransferA3;
using System.Xml;
using System.Globalization;

namespace DataTransferA3
{
    // NAME     : MainWindow
    // PURPOSE  : The MainWindow class is a code behind of MainWindows.xaml. 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string sDatabase;
        private string sTable;
        private string dDatabase;
        private string dTable;
        private string sConnectionString;
        private string dConnectionString;

        private SqlConnection sConnection;
        private SqlTransaction sTransaction;

        private SqlConnection dConnection;
        private SqlTransaction dTransaction;
        private string dError = "Please re-enter the correct database";
        private string tError = "Please re-enter the correct table";

        // FUNCTION     : TransferClick
        // DESCRIPTION  : This function proceeds the following process by calling the related functions.
        // Building a connection string from the user input, validating if the input database and tables exist,
        // verifying if the source and target tables have the same schema,
        // and copying the data from the source to the destination table.
        // PARAMETERS   : object sender, RoutedEventArgs e
        // RETURNS      : void
        public void TransferClick(object sender, RoutedEventArgs e)
        {
            sTable = SourceTable.Text;
            dTable = DestinationTable.Text;

            try
            {
                if ( string.IsNullOrEmpty(sDatabase) | string.IsNullOrEmpty(dDatabase) | string.IsNullOrEmpty(sTable) | string.IsNullOrEmpty(dTable))
                {
                    throw new InvalidOperationException($"Please complete database and table information");
                }

                // convert the connectionstring from the datalink (OleDB) for SqlClient connection string.
                string sCS = ConnectionString.BuildConnectionString(sConnectionString) + ';' ;
                string dCS = ConnectionString.BuildConnectionString(dConnectionString) + ';' ;
                // Source verification
                try
                {
                    sConnection = new SqlConnection(sCS);
                    sConnection.Open();
                    sTransaction = sConnection.BeginTransaction();
                    StatusMessage.Text = $"Messages: Source {sDatabase} is connected.";
                }
                catch (Exception ex)
                {
                    // Console.WriteLine(ex.Message);
                    throw new InvalidOperationException($"SOURCE Connection String: {dError}");
                }
                if (!SqlOperation.ValidateTable(sConnection, sTransaction, sTable))
                {
                    throw new InvalidOperationException($"Source table {sTable} does not exist.");
                }
                // Destination verification
                try
                {
                    dConnection = new SqlConnection(dCS);
                    dConnection.Open();
                    dTransaction = dConnection.BeginTransaction();
                    StatusMessage.Text += Environment.NewLine + $"Messages: Destination {dDatabase} is connected.";
                }
                catch (Exception ex)
                {
                    // Console.WriteLine(ex.Message);
                    throw new InvalidOperationException($"DESTINATION Connection String: {dError}");
                }
                bool destinationExist = SqlOperation.ValidateTable(dConnection, dTransaction, dTable);
                if (destinationExist)
                {
                    // if it matches the column and datatypes of the source table
                    bool tablesMatch = SqlOperation.ValidateSchema(sConnection, sTransaction, sTable, dConnection, dTransaction, dTable);
                    if (tablesMatch) // table exists, matches the columns and datatype
                    {
                        StatusMessage.Text += Environment.NewLine + $"Messages: {dDatabase}.{dTable} exist.";
                        // copy all records from the source to the destination
                        bool copyResult = SqlOperation.CopyRecords(sConnection, sTransaction, sTable, dConnection, dTransaction, dTable);
                        if (copyResult)
                        {
                            StatusMessage.Text += Environment.NewLine + $"Messages: {sTable} record is copied to {dTable}";
                        }
                    }
                    else // table exists, no matches the columns and datatype
                    {
                        // report the error and allow the user to correct the table name
                        throw new InvalidOperationException($"DESTINATION Table: {tError}");
                    }
                }
                else                
                {
                    StatusMessage.Text = $"Messages: {dTable} does not exist at {dDatabase}, {dTable} will be created.";
                    // create the destination table
                    DataTable sourceTableSchema = SqlOperation.TableSchema(sConnection, sTransaction, sTable);
                    Dictionary<string, string> tableinfo = SqlOperation.SchemaTableReader(sourceTableSchema);
                    bool IsTableCreated = SqlOperation.CreateEmptyTable(dConnection, dTransaction, dTable, tableinfo);
                    StatusMessage.Text += Environment.NewLine + $"Messages: {dTable} is created.";
                    if (IsTableCreated)
                    {
                        // copy all records from the source to the destination
                        bool copyResult = SqlOperation.CopyRecords(sConnection, sTransaction, sTable, dConnection, dTransaction, dTable);
                        if (copyResult) 
                        {
                            StatusMessage.Text += Environment.NewLine + $"Messages: {dTable} is created at {dDatabase} and {sTable} of {sDatabase} record is copied to {dTable}.";
                        }
                        else // detination database takes time for table's ready, retry
                        {
                            StatusMessage.Text += Environment.NewLine + $"Incomplete {sTable} copy to {dTable}. The {dTable} is created but not ready for writing, Please click Transfer again.";
                        }
                    }
                }

                sTransaction.Commit();
                dTransaction.Commit();

                sConnection.Close();
                dConnection.Close();
            }
            catch (Exception ex)
            {
                if (sTransaction != null && sTransaction.Connection != null)
                {
                    sTransaction.Rollback();
                }
                StatusMessage.Text = "Error: " + ex.Message;
            }
        }

        // FUNCTION     : DataLinkDialButton_Click
        // DESCRIPTION  : This function proceeds the following process by calling the related functions.
        // Opening a DataLinks Dialog and making the user complete connection information and getting the connection string
        // PARAMETERS   : object sender, RoutedEventArgs e
        // RETURNS      : void
        private void DataLinkDialButton_Click(object sender, System.EventArgs e)
        {
            Button clicked = sender as Button;

            ADODB.Connection conn = new ADODB.Connection();
            object oConn = (object)conn;

            MSDASC.DataLinks dlg = new MSDASC.DataLinks();  // dtatalink dialog
            dlg.PromptEdit(ref oConn);

            try
            {
                if (conn != null)
                {
                    if (clicked != null)
                    {
                        string server = "";
                        string db = "";
                        if (clicked.Name == "Source")
                        {
                            if (conn.ConnectionString != null)
                            {
                                sConnectionString = conn.ConnectionString;
                                server = ConnectionString.GetConnectionStringFieldValue(sConnectionString, "Data Source");
                                db = ConnectionString.GetConnectionStringFieldValue(sConnectionString, "Initial Catalog");
                                if (server == null || db == null)
                                {
                                    throw new InvalidOperationException($"SOURCE Data Link: please complete the correct connection string");
                                }
                                else
                                {
                                    SourceServer.Text = server;
                                    SourceDB.Text = db;
                                    sDatabase = db;
                                }
                            }
                            else
                            {
                                throw new InvalidOperationException($"SOURCE Data Link: please complete the correct connection string");
                            }
                        }
                        else if (clicked.Name == "Destination")
                        {
                            if (conn.ConnectionString != null)
                            {
                                dConnectionString = conn.ConnectionString;
                                server = ConnectionString.GetConnectionStringFieldValue(dConnectionString, "Data Source");
                                db = ConnectionString.GetConnectionStringFieldValue(dConnectionString, "Initial Catalog");
                                if (server == null || db == null)
                                {
                                    throw new InvalidOperationException($"DESTINATION Data Link: please complete the correct connection string");
                                }
                                else
                                {
                                    DestinationServer.Text = server;
                                    DestinationDB.Text = db;
                                    dDatabase = db;
                                }
                            }
                            else
                            {
                                throw new InvalidOperationException($"Destination Data Link: please complete the correct connection string");
                            }
                            
                        }


                        if (sConnectionString != null)
                        {
                            if (server == "\"\"" && db == "\"\"" || server == null && db == null)
                            {
                                StatusMessage.Text = $"Message: {clicked.Name.ToUpper()} {conn.ConnectionString} {Environment.NewLine}" +
                                    $"Server, Database are missed. {Environment.NewLine} " +
                                    $"Please click {clicked.Name} Data Link to set Server, Database.";
                            }
                            else if (server != null && db != null)
                            {
                                StatusMessage.Text = $"Message: {clicked.Name} {conn.ConnectionString} {Environment.NewLine}";
                            }
                            else
                            {
                                if (server == null)
                                {
                                    StatusMessage.Text = $"Error: {clicked.Name.ToUpper()} Connection String: {conn.ConnectionString} {Environment.NewLine}" +
                                        $"Server is missed. {Environment.NewLine}" +
                                        $"Please click {clicked.Name} Data Link to set Server, Database.";
                                }
                                else
                                {
                                    StatusMessage.Text = $"Error: {clicked.Name.ToUpper()} Connection String: {conn.ConnectionString} {Environment.NewLine}" +
                                        $"Database is missed. {Environment.NewLine}" +
                                        $"Please click {clicked.Name} Data Link to set Server, Database.";
                                }
                            }
                        }
                    }
                }
                else
                {
                    StatusMessage.Text = "Please select your connnections from DataLink";
                }

            }
            catch (Exception ex)
            {
                StatusMessage.Text = "Error: Data Link Dialog: " + ex.Message;
            }
        }
    }
}