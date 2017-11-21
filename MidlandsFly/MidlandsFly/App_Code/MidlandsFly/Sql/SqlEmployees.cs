using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Database.Sql;
using Database.Enums;
using System.Data.SqlClient;
using MidlandsFly.Sql;

/// <summary>
/// Provides methods for manipulating employee-related tables.
/// </summary>
namespace MidlandsFly
{
    namespace Sql
    {
        public class SqlEmployees
        {
            private static SqlEmployees instance;
            private static string notAssignedText = "N/A";

            public static SqlEmployees Instance
            {
                get
                {
                    if (instance == null)
                        instance = new SqlEmployees();
                    return instance;
                }
            }

            public void Insert(Employee employee, string regNumber, byte columnNumber = 0)
            {
                SqlMidlandsFly.Instance.AddCommand(InsertCmd(employee, regNumber));
            }
            public SqlCommand InsertCmd(Employee employee, string regNumber)
            {
                string text = string.Empty;
                text = String.Format("INSERT INTO {0} VALUES('{1}','{2}','{3}');",
                    SqlMidlandsFly.Instance.Table_Employees.Name,
                    MathExt.IntToFixedString((int)employee.Id, Employee.IdLength),
                    employee.Name,
                    employee.EmployeeType.ToString());
                if (employee.EmployeeType == EmployeeType.Cabin_Crew
                    || employee.EmployeeType == EmployeeType.Flight_Deck)
                {
                    text += String.Format("INSERT INTO {0} VALUES('{1}','{2}');",
                        SqlMidlandsFly.Instance.Table_Assignment.Name,
                        MathExt.IntToFixedString((int)employee.Id, Employee.IdLength),
                        regNumber);
                    text += String.Format("INSERT INTO {0} VALUES('{1}',{2});",
                        SqlMidlandsFly.Instance.Table_FlightHours.Name,
                        MathExt.IntToFixedString((int)employee.Id, Employee.IdLength),
                        0);
                }
                else if (employee.EmployeeType == EmployeeType.Ground_Crew)
                {
                    text += String.Format("INSERT INTO {0} VALUES('{1}','{2}');",
                        SqlMidlandsFly.Instance.Table_Assignment.Name,
                        MathExt.IntToFixedString((int)employee.Id, Employee.IdLength),
                        notAssignedText);
                }
                return new SqlCommand(text);
            }

            public void AssignEmployee(string regNumber, params uint[] employee_id)
            {
                SqlCommand cmd = new SqlCommand();

                foreach (uint employee in employee_id)
                {
                    cmd.CommandText += AssignEmployeeCmd(regNumber, employee).CommandText;
                }
                SqlMidlandsFly.Instance.AddCommand(cmd);
            }
            public SqlCommand AssignEmployeeCmd(string regNumber, uint employee_id)
            {
                string text = string.Empty;

                text += String.Format("INSERT INTO {0} VALUES('{1}','{2}');",
                    SqlMidlandsFly.Instance.Table_Assignment.Name,
                    MathExt.IntToFixedString((int)employee_id, Employee.IdLength),
                    regNumber);
                return new SqlCommand(text);
            }

            public void UnassignEmployee(params uint[] employee_ids)
            {
                SqlCommand cmd = new SqlCommand();

                foreach (uint employee_id in employee_ids)
                {
                    cmd.CommandText += UnassignEmployeeCmd(employee_id).CommandText;
                }
                SqlMidlandsFly.Instance.AddCommand(cmd);
            }
            public SqlCommand UnassignEmployeeCmd(uint employee_id)
            {
                // While employees could be unassigned using AssignEmployeeCmd("", id),
                // it's best to use this method as some companies might prefer unassigned
                // employees to be assigned to regnumber "not assigned" or something similar.
                return AssignEmployeeCmd(notAssignedText, employee_id);
            }

            public void AddMaintenance(uint id, string desc, string regNumber, byte columnNumber = 0)
            {
                SqlMidlandsFly.Instance.AddCommand(AddMaintenanceCmd(id, desc, regNumber));
            }
            public SqlCommand AddMaintenanceCmd(uint id, string desc, string regNumber)
            {
                string text;

                text = String.Format(
                    "INSERT INTO {0} VALUES('{1}','{2}','{3}');",
                    SqlMidlandsFly.Instance.Table_MaintenanceHistory.Name,
                    regNumber,
                    MathExt.IntToFixedString((int)id, Employee.IdLength),
                    desc);
                return new SqlCommand(text);
            }
        }
    }
}