// FILE: DataOperator.cs
// PROJECT : PROG3070 - Assignment #4
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-11-07
// DESCRIPTION: This FileOperator.cs provides the functions related to the data copy operations from a CSV file.
// https://www.c-sharpcorner.com/article/execute-a-stored-procedure-programmatically/

using System;
using System.Data;
using System.IO;
using System.Windows.Forms;


namespace A4_DataVisualization
{
    // NAME     : FileOperator
    // PURPOSE  : The FileOperator class provides functions related to the CSV file operation.
    public static class FileOperator
    {
        // FUNCTION     : CountCsvColumn
        // DESCRIPTION  : This function takes file_path and checks the column length of a CSV file and returns the counted column number.
        // PARAMETERS   : string file_path
        // RETURNS      : int
        public static int CountCsvColumn(string file_path)
        {
            int numberOfColumn = 0;
            try
            {
                using (StreamReader sr = new StreamReader(file_path))
                {
                    string line = sr.ReadLine();
                    if (line != null)
                    {
                        numberOfColumn = line.Split(',').Length;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR: CountCsvColumn-{ex.Message}");
            }
            return numberOfColumn;
        }
        // FUNCTION     : MakeDataTableFromCSV
        // DESCRIPTION  : This function takes file_path and table_name and reads a CSV file and completes the DataTable with the data and returns DataTable.
        // PARAMETERS   : string file_path, string table_name
        // RETURNS      : DataTable
        public static DataTable MakeDataTableFromCSV(string file_path, string table_name)
        {
            DataTable datatable = null;
            try
            {
               datatable = new DataTable();
                using (StreamReader streamreader = new StreamReader(file_path))
                {
                    string[] headers = streamreader.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        datatable.Columns.Add(header);
                    }
                    while (!streamreader.EndOfStream)
                    {
                        string[] rows = streamreader.ReadLine().Split(',');

                        DataRow datarow = datatable.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            datarow[i] = rows[i];
                        }
                        datatable.Rows.Add(datarow);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR: MakeDataTableFromCSV-{ex.Message}");
            }
            return datatable;
        }

        // FUNCTION     : InsertFileData
        // DESCRIPTION  : This function takes file_path and table_name and calls the related functions
        //                to get data filled DataTable and insert the DataTable and call the StoredProcedure
        // PARAMETERS   : string file_path, string table_name
        // RETURNS      : void
        public static void InsertFileData(string file_path, string table_name)
        {
            string itablename = "New"+table_name;
            try
            {
                DataTable datatable = new DataTable();
                datatable = FileOperator.MakeDataTableFromCSV(file_path, table_name);
                DBHelper.InsertDatatable(datatable, itablename);
                DBHelper.ExecuteStoredProcedure(table_name);
                DBHelper.CleanTable(table_name);
                MessageBox.Show($"Data is updated to {table_name}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR: InsertFileData-{ex.Message}");
            }

        }
    }
}
