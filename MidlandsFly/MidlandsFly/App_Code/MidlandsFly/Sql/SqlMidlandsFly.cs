using System;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using Database.Enums;
using Database.Sql;

/// <summary>
/// MidlandsFly SqlBase class for communication.
/// </summary>
namespace MidlandsFly
{
    namespace Sql
    {
        public class SqlMidlandsFly : SqlBase
        {
            private static SqlMidlandsFly instance;
            public SqlTable Table_Cargo;
            public SqlTable Table_Passenger;
            public SqlTable Table_Employees;
            public SqlTable Table_FlightHours;
            public SqlTable Table_Assignment;
            public SqlTable Table_MaintenanceHistory;
            public SqlTable Table_Maintenance;

            // Implementing Singleton for accessing the class using a static variable.
            // With a Singleton the programmer doesn't need to declare the class variable,
            // instances are limited to one, the class can use inheritance and etc.
            // Implementing Singleton in C# - https://msdn.microsoft.com/en-gb/library/ff650316.aspx
            public static SqlMidlandsFly Instance
            {
                get
                {
                    if (instance == null)
                        instance = new SqlMidlandsFly();
                    return instance;
                }
            }

            private SqlMidlandsFly(
                string dataSource = "airlineservercovuni.database.windows.net",
                string userID = "serveradministrator",
                string password = "Passw0rd2017",
                string initialCatalog = "ponyairline")
                : base(dataSource, userID, password, initialCatalog)
            {
                InitTables();
            }

            #region Initialize Tables
            public void InitTables()
            {
                Table_Cargo = new SqlTable("Cargo_Aircraft",
                    new SqlParameter(Parameter.regNumber, System.Data.SqlDbType.VarChar, Aircraft.RegNumber_symbolCount),
                    new SqlParameter(Parameter.flyHours, System.Data.SqlDbType.Int),
                    new SqlParameter(Parameter.lastMaintenance, System.Data.SqlDbType.Int),
                    new SqlParameter(Parameter.capacity_mTonnes, System.Data.SqlDbType.Int));
                Table_Passenger = new SqlTable("Passenger_Aircraft",
                    new SqlParameter(Parameter.regNumber, System.Data.SqlDbType.VarChar, Aircraft.RegNumber_symbolCount),
                    new SqlParameter(Parameter.flyHours, System.Data.SqlDbType.Int),
                    new SqlParameter(Parameter.lastMaintenance, System.Data.SqlDbType.Int),
                    new SqlParameter(Parameter.capacity_seating, System.Data.SqlDbType.Int));
                Table_Employees = new SqlTable("Employees",
                    new SqlParameter(Parameter.id, System.Data.SqlDbType.VarChar, Employee.IdLength),
                    new SqlParameter(Parameter.name, System.Data.SqlDbType.VarChar, Employee.name_length),
                    new SqlParameter(Parameter.employeeType, System.Data.SqlDbType.VarChar, Employee.employeeType_length));
                Table_FlightHours = new SqlTable("Employees_FlightHours",
                    new SqlParameter(Parameter.id, System.Data.SqlDbType.VarChar, Employee.IdLength),
                    new SqlParameter(Parameter.flyHours, System.Data.SqlDbType.Int));
                Table_Assignment = new SqlTable("Employees_Assignment",
                    new SqlParameter(Parameter.id, System.Data.SqlDbType.VarChar, Employee.IdLength),
                    new SqlParameter(Parameter.regNumber, System.Data.SqlDbType.VarChar, Aircraft.RegNumber_symbolCount));
                Table_MaintenanceHistory = new SqlTable("MaintenanceHistory",
                    new SqlParameter(Parameter.regNumber, System.Data.SqlDbType.VarChar, Aircraft.RegNumber_symbolCount),
                    new SqlParameter(Parameter.id, System.Data.SqlDbType.VarChar, Employee.IdLength),
                    new SqlParameter(Parameter.description, System.Data.SqlDbType.VarChar, MaintenanceHistory.description_length));
                Table_Maintenance = new SqlTable("Maintenance",
                    new SqlParameter(Parameter.regNumber, System.Data.SqlDbType.VarChar, Aircraft.RegNumber_symbolCount),
                    new SqlParameter(Parameter.lastMaintenance_date, System.Data.SqlDbType.DateTime));
            }
            #endregion

            public void CreateTables()
            {
                Instance.CreateTable(Table_Cargo);
                Instance.CreateTable(Table_Passenger);
                Instance.CreateTable(Table_Employees);
                Instance.CreateTable(Table_FlightHours);
                Instance.CreateTable(Table_Assignment);
                Instance.CreateTable(Table_MaintenanceHistory);
                Instance.CreateTable(Table_Maintenance);
            }
            public void ClearTables()
            {
                Instance.ResetTable(Table_Cargo);
                Instance.ResetTable(Table_Passenger);
                Instance.ResetTable(Table_Employees);
                Instance.ResetTable(Table_FlightHours);
                Instance.ResetTable(Table_Assignment);
                Instance.ResetTable(Table_MaintenanceHistory);
                Instance.ResetTable(Table_Maintenance);
            }
            public void DropTables()
            {
                Instance.DropTable(Table_Cargo);
                Instance.DropTable(Table_Passenger);
                Instance.DropTable(Table_Employees);
                Instance.DropTable(Table_FlightHours);
                Instance.DropTable(Table_Assignment);
                Instance.DropTable(Table_MaintenanceHistory);
                Instance.DropTable(Table_Maintenance);
            }
            public void RecreateTables()
            {
                Instance.RecreateTable(Table_Cargo);
                Instance.RecreateTable(Table_Passenger);
                Instance.RecreateTable(Table_Employees);
                Instance.RecreateTable(Table_FlightHours);
                Instance.RecreateTable(Table_Assignment);
                Instance.RecreateTable(Table_MaintenanceHistory);
                Instance.RecreateTable(Table_Maintenance);
            }

            public void AddHours(uint hours, string regNumber)
            {
                AddCommand(AddHoursCmd(hours, regNumber));
            }
            public SqlCommand AddHoursCmd(uint hours, string regNumber)
            {
                string text;

                text = String.Format(
                    "UPDATE {0} SET {1}={1} + '{2}' WHERE {3}='{4}';",
                    Table_Cargo.Name,
                    Parameter.flyHours,
                    hours,
                    Parameter.regNumber,
                    regNumber);
                text += String.Format(
                    "UPDATE {0} SET {1}={1} + '{2}' WHERE {3}='{4}';",
                    Table_Passenger.Name,
                    Parameter.flyHours,
                    hours,
                    Parameter.regNumber,
                    regNumber);
                text += String.Format(
                    "UPDATE f " +
                    "SET f.{0} = f.{0} + {1} " +
                    "FROM {2} as f " +
                    "INNER JOIN {3} as a " +
                    "ON f.{4} = a.{4} " +
                    "WHERE a.{5} = '{6}';",
                    Parameter.flyHours, hours,
                    Table_FlightHours.Name,
                    Table_Assignment.Name,
                    Parameter.id,
                    Parameter.regNumber,
                    regNumber);
                return new SqlCommand(text);
            }

            public string RegNumber_last(SqlTable table, string regNumber_zero)
            {
                // The first letter of regNumber_begin is used as an indicator for all items in the table
                // by having a first letter, the need to check for regNumber in more than one table is eliminated
                // TODO: Option for advanced search were it searches the whole table for holes between regnumbers too
                string regNumber_id = String.Empty;
                byte digitsLength = Aircraft.RegNumber_digitCount;
                byte lettersLength = Aircraft.RegNumber_letterCount;

                regNumber_id += regNumber_zero[0]; // Assign ID letter
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM " + table.Name + " ORDER BY " + Parameter.regNumber + " DESC");
                List<string> output = new List<string>();
                string regNumber = string.Empty;

                output = Execute(cmd);
                if (output.Count == 0)
                    regNumber = regNumber_zero;
                else
                    regNumber = output.Last();
                return regNumber;
            }
        }
    }
}