using Database.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;

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

            private string connectionString;
            private static bool busy = false;

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
                this.connectionString = builder.ConnectionString;
            }

            public void AddCommand(SqlCommand sqlcommand)
            {
                commands.Add(sqlcommand);
            }

            public void AddCommand(string sqlcommand)
            {
                commands.Add(new SqlCommand(sqlcommand));
            }

            // The following method should be used with exception handling:
            public List<string> Execute(byte columnNumber = 0)
            {
                List<string> received = new List<string>();

                if (busy)
                    throw new Exception("The Server is busy, please try again later.");
                else
                    busy = true;

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        try
                        {
                            SqlCommand commandBurst = new SqlCommand(CommandsToString(), connection);
                            using (SqlDataReader reader = commandBurst.ExecuteReader())
                            {
                                received = ExecuteRead(reader, columnNumber);
                            }
                        }
                        catch (Exception exBurst)
                        {
                            // Uncomment for easy debug:
                            //foreach (SqlCommand command in commands)
                            //{
                            //    command.Connection = connection;
                            //    using (SqlDataReader reader = command.ExecuteReader())
                            //    {
                            //        received = ExecuteRead(reader, columnNumber);
                            //    }
                            //}
                            throw;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == ErrorCode.NoConnectionA || ex.Number == ErrorCode.NoConnectionB)
                    {
                        throw new Exception("No connection with the database. Please try again or contact the administrator.");
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Collection was modified; enumeration operation may not execute.")
                    {
                        throw new Exception("The Server is busy, please try again later.");
                    }
                    throw;
                }
                finally
                {
                    ClearCommands();
                    busy = false;
                }
                
                return received;
            }

            public List<string> Execute(SqlCommand command, byte columnNumber = 0)
            {
                List<string> received = new List<string>();

                if (busy)
                    throw new Exception("The Server is busy, please try again later.");
                else
                    busy = true;

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        command.Connection = connection;
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            received = ExecuteRead(reader, columnNumber);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == ErrorCode.NoConnectionA || ex.Number == ErrorCode.NoConnectionB)
                    {
                        throw new Exception("No connection with the database. Please try again or contact the administrator.");
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Collection was modified; enumeration operation may not execute.")
                    {
                        throw new Exception("The Server is busy, please try again later.");
                    }
                    throw;
                }
                finally
                {
                    ClearCommands();
                    busy = false;
                }

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

            public void CreateTable(params SqlTable[] tables)
            {
                foreach (SqlTable table in tables)
                {
                    AddCommand(table.Create());
                }
            }

            public void DropTable(params SqlTable[] tables)
            {
                foreach (SqlTable table in tables)
                {
                    AddCommand(table.Drop());
                }
            }

            public void ResetTable(params SqlTable[] tables)
            {
                foreach (SqlTable table in tables)
                {
                    AddCommand(table.Delete());
                }
            }

            public void RecreateTable(params SqlTable[] tables)
            {
                foreach (SqlTable table in tables)
                {
                    DropTable(table);
                    CreateTable(table);
                }
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
                        throw;
                }
            }
        }
    }
}