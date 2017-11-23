using Database.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

/// <summary>
/// Summary description for SqlBase
/// </summary>
namespace Database
{
    namespace Sql
    {
        public class SqlBase
        {
            private List<SqlCommand> commands = new List<SqlCommand>();

            private SqlConnection connection;

            public SqlBase(
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
                DateTime startTime = DateTime.Now;
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                TimeSpan span = new TimeSpan();
                while (connection.State == System.Data.ConnectionState.Connecting)
                {
                    span = DateTime.Now.Subtract(startTime);
                    if (span.Seconds > 5)
                    {
                        throw new TimeoutException("Unable to connect");
                    }
                }
                return true;
            }

            private bool Disconnect()
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
                return true;
            }

            public void AddCommand(SqlCommand sqlcommand)
            {
                SqlCommand sqlcommandMod = sqlcommand;
                sqlcommandMod.Connection = connection;
                commands.Add(sqlcommandMod);
            }

            public void AddCommand(string sqlcommand)
            {
                SqlCommand sqlcommandMod = new SqlCommand(sqlcommand);
                sqlcommandMod.Connection = connection;
                commands.Add(sqlcommandMod);
            }

            // The following method should be used with exception handling:
            public List<string> Execute(byte columnNumber = 0)
            {
                List<string> received = new List<string>();
                SqlCommand commandBurst = new SqlCommand(CommandsToString(), connection);
                List<SqlCommand> commandsExecuted = new List<SqlCommand>();

                Connect();
                try // Try burst method for speed
                {
                    using (SqlDataReader reader = commandBurst.ExecuteReader())
                    {
                        received = ExecuteRead(reader, columnNumber);
                    }
                }
                catch (SqlException) // Slow method for easy debug
                {
                    foreach (SqlCommand command in commands)
                    {
                        if (command == commands.Last()) // Read only the last output
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                received = ExecuteRead(reader, columnNumber);
                            }
                        }
                        else
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
                finally
                {
                    Disconnect();
                    ClearCommands();
                }
                return received;
            }

            public List<string> Execute(SqlCommand command, byte columnNumber = 0)
            {
                List<string> received = new List<string>();

                command.Connection = connection;
                Connect();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    received = ExecuteRead(reader, columnNumber);
                }
                Disconnect();

                return received;
            }

            private List<string> ExecuteRead(SqlDataReader reader, byte columnNumber = 0)
            {
                List<string> received = new List<string>();

                while (reader.Read())
                {
                    string type = reader.GetDataTypeName(columnNumber);
                    switch (type)
                    {
                        case "int":
                            received.Add(reader.GetInt32(columnNumber).ToString());
                            break;
                        case "varchar":
                            received.Add(reader.GetString(columnNumber));
                            break;
                        default:
                            try
                            {
                                received.Add(reader.GetString(columnNumber));
                            }
                            catch (SqlException)
                            {
                                received.Add(String.Empty);
                            }
                            break;
                    }
                }
                return received;
            }

            public void ClearCommands()
            {
                commands.Clear();
            }

            private string CommandsToString()
            {
                string output = string.Empty;
                foreach (SqlCommand command in commands)
                {
                    output += command.CommandText;
                }
                return output;
            }

            public void CreateTable(SqlTable table)
            {
                AddCommand(table.Create());
            }

            public void DropTable(SqlTable table)
            {
                AddCommand(table.Drop());
            }

            public void ResetTable(SqlTable table)
            {
                AddCommand(table.Delete());
            }

            public void RecreateTable(SqlTable table)
            {
                DropTable(table);
                CreateTable(table);
            }

            // Using params to be able to chech multiple tables at once
            public bool TableExists(params SqlTable[] tables)
            {
                SqlCommand cmd = new SqlCommand();

                foreach (SqlTable table in tables)
                {
                    cmd.CommandText += table.Top(1).CommandText; // Checking for table existence by getting the first row.
                }
                try
                {
                    Execute(cmd); // Check for all tables at once
                    return true;
                }
                catch (SqlException e)
                {
                    if (e.Number == ErrorCode.InvalidName || e.Number == ErrorCode.TableDoesNotExist)
                    {
                        return false;
                    }
                    else
                        return true;
                }
            }
        }
    }
}