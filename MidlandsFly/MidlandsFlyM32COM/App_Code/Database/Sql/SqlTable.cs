using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// A class that contains the Sql Table implementation.
/// </summary>
namespace Database
{
    namespace Sql
    {
        public class SqlTable
        {
            private const int nameLimit = 128;
            private string name = string.Empty;

            public string Name
            {
                get => name;
                set
                {
                    value.Replace(" ", "_"); // Replace all spaces with underscores, as spaces are not accepted by the T-Sql.
                    if (value.Length <= nameLimit)
                        name = value;
                    else
                        name = "NameTooLong";
                }
            }
            public List<SqlParameter> Columns { get; set; }

            public SqlTable(string name, params SqlParameter[] columns)
            {
                Name = name;
                Columns = new List<SqlParameter>(columns);
            }

            public SqlCommand Create()
            {
                SqlCommand command = new SqlCommand();

                command.CommandType = CommandType.Text;
                command.CommandText += String.Format("CREATE TABLE {0}", Name);
                if (Columns.Count > 0)
                {
                    SqlParameter last = Columns.Last();
                    command.CommandText += " (";
                    foreach (SqlParameter sqlparam in Columns)
                    {
                        command.CommandText += String.Format("{0} {1}", sqlparam.ParameterName, sqlparam.SqlDbType);
                        if (sqlparam.Size != 0) // Add size argument if size isn't 0. Note: "-1" is also accepted in T-Sql.
                        {
                            command.CommandText += String.Format("({0})", sqlparam.Size);
                        }
                        if (sqlparam != last) // Put a comma&space after every parameter except the last one
                        {
                            command.CommandText += ", ";
                        }
                    }
                    command.CommandText += ")"; // Ending bracket
                }
                command.CommandText += ";"; // End statement with ;

                return command;
            }

            public SqlCommand Drop()
            {
                SqlCommand command = new SqlCommand();

                command.CommandType = CommandType.Text;
                command.CommandText += String.Format("DROP TABLE {0};", Name);

                return command;
            }

            public SqlCommand Delete()
            {
                SqlCommand command = new SqlCommand();

                command.CommandType = CommandType.Text;
                command.CommandText += String.Format("DELETE {0};", Name);

                return command;
            }

            public SqlCommand Top(int count)
            {
                SqlCommand command = new SqlCommand();

                command.CommandType = CommandType.Text;
                command.CommandText += String.Format("SELECT TOP {0} * FROM {1};", count, Name);

                return command;
            }
        }
    }
}