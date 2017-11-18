using System;
using System.Data.SqlClient;
using System.Linq;

/// <summary>
/// Summary description for SQL
/// </summary>
public class SQL
{
    private string query = string.Empty;

    public enum ErrorNumbers
    {
        WrongColumnName = 207,
        NoTable = 208,
        Duplicate = 2627
    }

    private SqlConnection connection;

    public SQL(
        string dataSource = "airlineservercovuni.database.windows.net", 
        string userID = "serveradministrator", 
        string password = "Passw0rd2017", 
        string initialCatalog = "ponyairline")
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
        {
            DataSource = dataSource,
            UserID = userID,
            Password = password,
            InitialCatalog = initialCatalog
        };
        this.connection = new SqlConnection(builder.ConnectionString);
    }

    // TODO: To Improve
    private bool Connect()
    {
        if (connection.State == System.Data.ConnectionState.Closed)
            connection.Open();
        return true;
    }

    private bool Disconnect()
    {
        if (connection.State != System.Data.ConnectionState.Closed)
            connection.Close();
        return true;
    }

    // The following method should be used with exception handling:
    public string Execute(string cmd, byte columnNumber = 0)
    {
        string received = String.Empty;

        Connect();
        using (SqlCommand command = new SqlCommand(cmd, connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                string type = string.Empty;
                while (reader.Read())
                {
                    type = reader.GetDataTypeName(columnNumber);
                    switch (type)
                    {
                        case "int":
                            received += reader.GetInt32(columnNumber).ToString();
                            received += ";";
                            break;
                        case "varchar":
                            received += reader.GetString(columnNumber);
                            received += ";";
                            break;
                        default:
                            try
                            {
                                received = reader.GetString(columnNumber);
                                received += ";";
                            }
                            catch (SqlException)
                            {
                                received = string.Empty;
                            }
                            break;
                    }
                }
            }
        }
        Disconnect();
        return received;
    }

    public void ResetTable(string table_name)
    {
        if (TableExists(table_name) == true)
        {
            query += "DELETE " + table_name + ";";
        }
    }

    public void DropTable(string table_name)
    {
        if (TableExists(table_name) == true)
        {
            query += "DROP TABLE " + table_name + ";";
        }
    }

    // TODO: Check if database exists before usage
    public bool TableExists(params string[] table_name)
    {
        string cmd = string.Empty;

        foreach (string table in table_name)
        {
            cmd = "SELECT TOP 1 * FROM " + table + ";";
        }
        try
        {
            Execute(cmd);
            return true;
        }
        catch (SqlException e)
        {
            if (e.Number == (int)ErrorNumbers.NoTable)
            {
                // Here it's possible to add the option to create a new table
                // by passing a function as an argument and using it
                // to create the new table.
                return false;
            }
            else
                throw;
        }
    }

    // By making the CreateTable function a bool, it's possible to understand
    // if the table was created or not without using exception handling.
    public bool CreateTable(string table_name, params string[] columns)
    {
        string cmd = CreateTableCmd(table_name, columns);

        try
        {
            Execute(cmd);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static string CreateTableCmd(string table_name, params string[] columns)
    {
        string cmd = String.Empty;

        cmd = "CREATE TABLE " + table_name + " (";
        if (columns.Length != 0)
        {
            cmd += columns[0];
            for (int i = 1; i < columns.Length; i++)
            {
                cmd += ", " + columns[i];
            }
            cmd += ")";
        }

        return cmd;
    }
}