// FILE: SqlOperation.cs
// PROJECT : PROG3070 - Assignment #3
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-10-24
// DESCRIPTION: This SqlOperation.cs provides the Sql Operational functions.
// https://learn.microsoft.com/en-us/dotnet/api/system.data.datatable?view=net-8.0
// https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace DataTransferA3
{
    // NAME     : SqlOperation
    // PURPOSE  : The SqlOperation class provides functions related to the database's Sql Client operation.
    public static class SqlOperation
    {
        // FUNCTION     : ValidateTable
        // DESCRIPTION  : This function takes SqlConnection, SqlTransaction, and table name and checks if the table exists by trying the query for the table.
        // PARAMETERS   : SqlConnection conn, SqlTransaction transaction, string table
        // RETURNS      : bool
        public static bool ValidateTable(SqlConnection conn, SqlTransaction transaction, string table)
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }

            string query = $"SELECT COUNT(1) FROM {table};";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                cmd.Transaction = transaction;
                object queryresult = cmd.ExecuteScalar();
                return true;
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
                return false;
            }
        }

        // FUNCTION     : TableSchema
        // DESCRIPTION  : This function takes SqlConnection, SqlTransaction, and table name and makes the DataTable that has the target table's schema.
        // PARAMETERS   : SqlConnection conn, SqlTransaction transaction, string table
        // RETURNS      : DataTable
        public static DataTable TableSchema(SqlConnection conn, SqlTransaction transaction, string table)
        {
            SqlCommand command = new SqlCommand($"SELECT * FROM {table} WHERE 1=0", conn, transaction);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            DataTable tbl = new DataTable();
            adapter.Fill(tbl);
            
            return tbl;
        }

        // FUNCTION     : SchemaTableReader
        // DESCRIPTION  : This function takes DataTable, reads the schema, and makes a Dictionary with column name and the SQL datatype.
        // PARAMETERS   : DataTable schemaTable
        // RETURNS      : Dictionary<string, string>
        public static Dictionary<string, string> SchemaTableReader(DataTable schemaTable)
        {

            Dictionary<string, string> tabledatainfo = new Dictionary<string, string>();
            using (DataTableReader reader = new DataTableReader(schemaTable))
            {
                DataTable schematable = reader.GetSchemaTable();
                for (int i = 0; i < schematable.Rows.Count; i++)
                {
                    string columnname = schematable.Rows[i]["ColumnName"].ToString();
                    Type columntype = (Type)schematable.Rows[i]["DataType"];
                    int columnsize = Convert.ToInt32(schematable.Rows[i]["ColumnSize"]);
                    string ukey = schematable.Rows[i]["IsUnique"].ToString();
                    string key = schematable.Rows[i]["IsKey"].ToString();
                    //string autoincrement = schematable.Rows[i]["IsAutoIncrement"].ToString();
                    //object r = schematable.Rows[i];

                    //ntext(1073741823), image byte[]
                    string sqldatatype = "";
                    if (columntype == typeof(int)) sqldatatype = "INT";
                    if (columntype == typeof(short)) sqldatatype = "SMALLINT";
                    if (columntype == typeof(decimal)) sqldatatype = "MONEY";
                    if (columntype == typeof(double)) sqldatatype = "FLOAT";
                    if (columntype == typeof(float)) sqldatatype = "REAL";
                    if (columntype == typeof(bool)) sqldatatype = "BIT";
                    if (columntype == typeof(byte[])) sqldatatype = "IMAGE";
                    if (columntype == typeof(DateTime)) sqldatatype = "DATETIME";
                    if (columntype == typeof(string) && key == "True") sqldatatype = $"NCHAR({columnsize})";
                    if (columntype == typeof(string) && key == "False" && columnsize < 1073741823) sqldatatype = $"NVARCHAR({columnsize})";
                    if (columntype == typeof(string) && columnsize == 1073741823) sqldatatype = "NTEXT";

                    tabledatainfo.Add(columnname, sqldatatype);
                }
            }
            return tabledatainfo;
        }

        // FUNCTION     : ValidateSchema
        // DESCRIPTION  : This function takes source SqlConnection, source SqlTransaction, source table name, destination SqlConnection, destination SqlTransaction, and destination table name.
        // The function compares the source and target's table schema and returns if it's some or not as the boolean result.
        // PARAMETERS   : SqlConnection sconn, SqlTransaction strans, string stable, SqlConnection dconn, SqlTransaction dtrans, string dtable
        // RETURNS      : bool
        public static bool ValidateSchema(SqlConnection sconn, SqlTransaction strans, string stable, SqlConnection dconn, SqlTransaction dtrans, string dtable)
        {
            DataTable stschema = TableSchema(sconn, strans, stable);
            DataTable dtschema = TableSchema(dconn, dtrans, dtable);

            Dictionary<string, string> sourcetableschema = SchemaTableReader(stschema);
            Dictionary<string, string> destinationtableschema = SchemaTableReader(dtschema);

            if (sourcetableschema.Count != destinationtableschema.Count)
            {
                return false;
            }
            else
            {
                foreach (var keyvalue in sourcetableschema)
                { 
                    if(!destinationtableschema.TryGetValue(keyvalue.Key, out var value) || value == keyvalue.Value)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // FUNCTION     : CreateEmptyTable
        // DESCRIPTION  : This function takes SqlConnection, SqlTransaction, table name, table information dictionary.
        // The function creates the table with the schema information in the SqlConnection's database.
        // PARAMETERS   : SqlConnection sconn, SqlTransaction strans, string tablename, Dictionary<string, string> tableinfo
        // RETURNS      : bool
        public static bool CreateEmptyTable(SqlConnection conn, SqlTransaction trans, string tablename, Dictionary<string, string> tableinfo)
        {
            string query = "";
            string columntype = "";

            foreach (var keyvalue in tableinfo)
            {
                columntype += $"{keyvalue.Key} {keyvalue.Value}, ";
            }
            columntype = columntype.TrimEnd(',',  ' ');
            query = $"CREATE TABLE {tablename} ({columntype});";

            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, conn, trans))
                {
                    sqlCommand.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreateEmptyTable-{ex.Message}");
                return false;
            }
        }

        // FUNCTION     : CopyRecord
        // DESCRIPTION  : This function takes source SqlConnecion, source SqlTransaction source table name, destination SqlConnection, destination SqlTransaction, destination table name.
        // The function copies source table data to the destination table by making DataSet for source and destination, filling the source dataset with the source table data, copying the data to the destination dataset, and updating the destination table with the destination dataset. 
        // PARAMETERS   : string sconnstring, string stable, string dconnstring, string dtable
        // RETURNS      : bool
        public static bool CopyRecords(SqlConnection sconn, SqlTransaction strans, string stable, SqlConnection dconn, SqlTransaction dtrans, string dtable)
        {

            string sQuery = $"SELECT * FROM {stable}";
            DataSet sourceDataset;
            SqlDataAdapter sourceDataAdapter, destinationDataAdapter;

            try
            {
                sourceDataAdapter = new SqlDataAdapter(sQuery, sconn);
                sourceDataAdapter.SelectCommand.Transaction = strans;
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(sourceDataAdapter);
                sourceDataset = new DataSet();

                sourceDataAdapter.FillSchema(sourceDataset, SchemaType.Source, stable);
                sourceDataAdapter.Fill(sourceDataset, stable);

                destinationDataAdapter = new SqlDataAdapter($"SELECT * FROM {dtable}", dconn);
                destinationDataAdapter.SelectCommand.Transaction = dtrans;
                SqlCommandBuilder destCommandBuilder = new SqlCommandBuilder(destinationDataAdapter);

                DataTable sourceTable = sourceDataset.Tables[stable];
                DataTable destinationTable = new DataTable(dtable);

                destinationDataAdapter.Fill(destinationTable);

                foreach (DataRow sourceRow in sourceTable.Rows)
                {
                    DataRow row = destinationTable.NewRow();
                    foreach (DataColumn column in sourceTable.Columns)
                    {
                        row[column.ColumnName] = sourceRow[column.ColumnName];
                    }
                    destinationTable.Rows.Add(row);
                }
                destinationDataAdapter.Update(destinationTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            return true;
        }
    }
}


