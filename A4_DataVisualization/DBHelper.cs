// FILE: DBHelper.cs
// PROJECT : PROG3070 - Assignment #4
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-11-07
// DESCRIPTION: This DBHelper.cs provides the helper functions for Sql Client Ooperaton.
// https://stackoverflow.com/questions/1050112/how-to-read-a-csv-file-into-a-net-datatable
// https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlbulkcopy?view=netframework-4.8.1
// https://www.c-sharpcorner.com/blogs/bulk-insert-in-sql-server-from-c-sharp


using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace A4_DataVisualization
{
    // NAME     : DBHelper
    // PURPOSE  : The DBHelper class provides functions related to the Sql Client operation.
    public class DBHelper
    {
        public static SqlConnection conn = new SqlConnection();
        public static SqlDataAdapter da;
        public static DataTable dt = new DataTable();

        // FUNCTION     : GetData
        // DESCRIPTION  : This function takes qeury and connects to the Sql Server and get the result to the DataTable and return the DataTable.
        // PARAMETERS   : string query
        // RETURNS      : DataTable
        public static DataTable GetData(string query)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["A4DB"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception("ERROR: GetData-" + ex.Message, ex);
            }
        }

        // FUNCTION     : InsertDatatable
        // DESCRIPTION  : This function takes DataTable and table name and connects to the Sql Server and insert the data of DataTable to the target table.
        // PARAMETERS   : DataTable datatable, string table
        // RETURNS      : void
        public static void InsertDatatable(DataTable datatable, string table)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["A4DB"].ConnectionString))
                {
                    conn.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                    {
                        bulkCopy.DestinationTableName = table;
                        bulkCopy.WriteToServer(datatable);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: InsertDatatable-" + ex.Message, ex);
            }
        }

        // FUNCTION     : ExecuteStoredProcedure
        // DESCRIPTION  : This function takes table_name and connects to the Sql Server and Call the StoredProcedure.
        // PARAMETERS   : string table_name
        // RETURNS      : void
        public static void ExecuteStoredProcedure (string table_name)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["A4DB"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("UpdateTable", conn))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@Table", table_name);
                        comm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: ExecuteStoredProcedure-" + ex.Message, ex);
            }
        }

        public static void CleanTable(string table)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["A4DB"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand($"DELETE FROM New{table}", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: CleanTable-" + ex.Message, ex);
            }
        }
    }
}
