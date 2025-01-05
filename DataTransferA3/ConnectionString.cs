// FILE: ConnectionString.cs
// PROJECT : PROG3070 - Assignment #3
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-10-24
// DESCRIPTION: This ConnectionString.cs provides the connection string processing functions.
// https://learn.microsoft.com/en-us/sql/connect/ado-net/connection-string-builders?view=sql-server-ver16
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace DataTransferA3
{
    // NAME     : ConnectionString
    // PURPOSE  : The ConnectionString class provides functions related to the connection string operation, such as building and verifying the connection string.
    public static class ConnectionString
    {
        // FUNCTION     : BuildConnectionString
        // DESCRIPTION  : This function takes the connection string, builds the connection string for SqlClient and returns it.
        // PARAMETERS   : string connectionstring
        // RETURNS      : string
        public static string BuildConnectionString(string connectionstring)
        {
            // if integrated security is null, Persist Security Info, User ID, Password(need to tick to show up), Initial Catalog, Data Source)
            // if integrated security is not null, Persist Security Info, Initial Catalog, Data Source)
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            bool integratedSecurity = connectionstring.Contains("Integrated Security");
            bool password = connectionstring.Contains("Password");
            var sqlCS = connectionstring.Split(';');
            string returnstring = "";

            if (!integratedSecurity && password)
            {
                foreach (var value in sqlCS)
                {
                    var valuesplited = value.Split('=');
                    if (value.StartsWith("data source=", StringComparison.OrdinalIgnoreCase))
                    {
                        builder.DataSource = valuesplited[1].ToString();
                    }
                    else if (value.StartsWith("Initial Catalog=", StringComparison.OrdinalIgnoreCase))
                    {
                        builder.InitialCatalog = valuesplited[1].ToString();
                    }
                    else if (value.StartsWith("User ID=", StringComparison.OrdinalIgnoreCase))
                    {
                        builder.UserID = valuesplited[1].ToString();
                    }
                    else if (value.StartsWith("Password=", StringComparison.OrdinalIgnoreCase))
                    {
                        builder.Password = valuesplited[1].ToString();
                    }
                }
                builder.PersistSecurityInfo = false;
            }
            else
            {
                foreach (var value in sqlCS)
                {
                    var valuesplited = value.Split('=');
                    if (value.StartsWith("data source=", StringComparison.OrdinalIgnoreCase))
                    {
                        builder.DataSource = valuesplited[1].ToString();
                    }
                    else if (value.StartsWith("Initial Catalog=", StringComparison.OrdinalIgnoreCase))
                    {
                        builder.InitialCatalog = valuesplited[1].ToString();
                    }
                }
                builder.IntegratedSecurity = true;
                builder.PersistSecurityInfo = false;
            }

            returnstring = builder.ConnectionString;

            return returnstring;
        }

        // FUNCTION     : GetConnectionStringFieldValue
        // DESCRIPTION  : This function takes the connection string, and the target field name and returns the value for the target field if it exists; otherwise returns null.
        // PARAMETERS   : string connectionstring, string targetfield
        // RETURNS      : string result
        public static string GetConnectionStringFieldValue(string connectionstring, string targetfield)
        {
            string[] parsedString = connectionstring.Split(';');
            string result = "";
            for (int i = 0; i < parsedString.Length; i++)
            {
                if (parsedString[i].Trim().StartsWith(targetfield, StringComparison.CurrentCulture))
                {
                    string[] keyValue = parsedString[i].Split('=');
                    if (keyValue.Length > 1)
                    {
                        result = keyValue[1].Trim();
                        return result;
                    }
                }
            }

            return null;
        }
    }
}
