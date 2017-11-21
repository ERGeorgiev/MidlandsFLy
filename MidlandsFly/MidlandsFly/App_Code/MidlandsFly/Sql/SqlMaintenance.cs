using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Database.Sql;
using Database.Enums;
using System.Data.SqlClient;
using MidlandsFly.Sql;

/// <summary>
/// Summary description for SqlMaintenance
/// </summary>
namespace MidlandsFly
{
    namespace Sql
    {
        public class SqlMaintenance
        {
            private static SqlMaintenance instance;

            public static SqlMaintenance Instance
            {
                get
                {
                    if (instance == null)
                        instance = new SqlMaintenance();
                    return instance;
                }
            }

            public void Insert(string regNumber, string date = "")
            {
                string maintenanceDate = date;
                if (maintenanceDate == "")
                {
                    maintenanceDate = Aircraft.DateToString(DateTime.Now);
                }
                SqlMidlandsFly.Instance.AddCommand(InsertCmd(regNumber, maintenanceDate));
            }
            public void Insert(string regNumber, DateTime date)
            {
                SqlMidlandsFly.Instance.AddCommand(InsertCmd(regNumber, Aircraft.DateToString(date)));
            }
            public SqlCommand InsertCmd(string regNumber, string date)
            {
                string text;
                text = String.Format("INSERT INTO {0} VALUES('{1}','{2}');",
                    SqlMidlandsFly.Instance.Table_Maintenance.Name,
                    regNumber, 
                    date);
                return new SqlCommand(text);
            }

            public List<string> GetPlanesThatNeedMaintenance(byte columnNumber = 0)
            {
                List<string> result = SqlMidlandsFly.Instance.Execute(GetPlanesThatNeedMaintenanceCmd(), columnNumber);
                return result;
            }

            public SqlCommand GetPlanesThatNeedMaintenanceCmd()
            {
                string text = string.Empty;

                text +=
                    "SELECT " + Parameter.regNumber + " " +
                    "FROM " + SqlMidlandsFly.Instance.Table_Cargo.Name + " " +
                    "WHERE " + Parameter.flyHours + " - " + Parameter.lastMaintenance + " >= " + Aircraft.MaintenanceRepetition_hours + " ";
                text +=
                    "union " +
                    "SELECT " + Parameter.regNumber + " " +
                    "FROM " + SqlMidlandsFly.Instance.Table_Passenger.Name + " " +
                    "WHERE " + Parameter.flyHours + " - " + Parameter.lastMaintenance + " >= " + Aircraft.MaintenanceRepetition_hours + ";";
                return new SqlCommand(text);
            }

            public bool PlaneNeedsMaintenance(string regNumber, byte columnNumber = 0)
            {
                // Output count from command Execute should be 1, so Last() can be used to assign it into a string.
                List<string> result = new List<string>();
                result = SqlMidlandsFly.Instance.Execute(PlaneNeedsMaintenanceCmd(regNumber), columnNumber);
                if (result.Count == 0)
                {
                    return false;
                }
                else if (result.Last() == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            public SqlCommand PlaneNeedsMaintenanceCmd(string regNumber)
            {
                string text = string.Empty;

                text +=
                    "SELECT CASE " +
                    "WHEN " + Parameter.flyHours + " - " + Parameter.lastMaintenance + " >= " + Aircraft.MaintenanceRepetition_hours + " " +
                    "THEN 1 " +
                    "ELSE 0 " +
                    "END as TOTFORMAIN, * " +
                    "FROM " + SqlMidlandsFly.Instance.Table_Cargo.Name + " " +
                    "WHERE " + Parameter.regNumber + "='" + regNumber + "' ";
                text +=
                    "union " +
                    "SELECT CASE " +
                    "WHEN " + Parameter.flyHours + " - " + Parameter.lastMaintenance + " >= " + Aircraft.MaintenanceRepetition_hours + " " +
                    "THEN 1 " +
                    "ELSE 0 " +
                    "END as TOTFORMAIN, * " +
                    "FROM " + SqlMidlandsFly.Instance.Table_Passenger.Name + " " +
                    "WHERE " + Parameter.regNumber + "='" + regNumber + "';";
                return new SqlCommand(text);
            }
        }
    }
}