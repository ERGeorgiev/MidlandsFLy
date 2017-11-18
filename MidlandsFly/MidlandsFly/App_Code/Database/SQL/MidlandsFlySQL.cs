using System;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

/// <summary>
/// MidlandsFly SQL class for communication.
/// </summary>
public class MidlandsFlySQL : SQL
{
    public MidlandsFlySQL(
        string dataSource = "airlineservercovuni.database.windows.net", 
        string userID = "serveradministrator", 
        string password = "Passw0rd2017", 
        string initialCatalog = "ponyairline")
        : base(dataSource, userID, password, initialCatalog)
    {
        //
    }

    // Setting up enum of tables to have already set names, reducing the chance for a programming error.
    // I'm planning to use ToString() to get table names.
    public enum Tables
    {
        Cargo_Aircraft,
        Passenger_Aircraft,
        Employees,
        Employees_FlightHours,
        Employees_MaintenanceHistory,
        Employees_Assignment
    }

    public enum Param
    {
        // To Update ColumnName after updating Param
        regNumber,
        flyHours,
        lastMaintenance,
        lastMaintenance_date,
        capacity_mTonnes,
        capacity_seating,
        name,
        id,
        employeeType,
        description
    }

    public static string ColumnName(Param param)
    {
        switch (param)
        {
            case Param.regNumber:
                return "Aircraft_Number";
            case Param.lastMaintenance:
                return "Last_Maintenance";
            case Param.lastMaintenance_date:
                return "Last_Maintenance_Date";
            case Param.flyHours:
                return "Flying_Hours";
            case Param.capacity_mTonnes:
                return "Capacity_MetricTonnes";
            case Param.capacity_seating:
                return "Capacity_Seating";
            case Param.name:
                return "Employee_Name";
            case Param.id:
                return "Employee_ID";
            case Param.employeeType:
                return "Employee_Type";
            case Param.description:
                return "Description";
            default:
                throw new ArgumentException("Invalid Param.");
        }
    }

    public void CreateTable(Tables table)
    {
        string cmd = CreateTableCmd(table);
        Execute(cmd);
    }

    public string CreateTableCmd(Tables table)
    {
        string cmd = String.Empty;
        
        if (table == Tables.Cargo_Aircraft
            || table == Tables.Passenger_Aircraft)
        {
            string capacity = String.Empty;

            switch (table)
            {
                case Tables.Cargo_Aircraft:
                    capacity = ColumnName(Param.capacity_mTonnes);
                    break;
                case Tables.Passenger_Aircraft:
                    capacity = ColumnName(Param.capacity_seating);
                    break;
                default:
                    throw new ArgumentException("Wrong table type " + table.ToString() + " in 'CreateTableCmd'");
            }

            cmd = String.Format("CREATE TABLE {0} ({1} varchar({2}), {3} int, {4} int, {5} datetime, {6} int)",
                table.ToString(),
                ColumnName(Param.regNumber),
                (Aircraft.RegNumber_digitCount + Aircraft.RegNumber_letterCount),
                ColumnName(Param.flyHours),
                ColumnName(Param.lastMaintenance),
                ColumnName(Param.lastMaintenance_date),
                capacity);
        }
        else
        {
            switch (table)
            {
                case Tables.Employees:
                    cmd = String.Format("CREATE TABLE {0} ({1} varchar({2}), {3} varchar({4}), {5} varchar({6}));",
                        table.ToString(),
                        ColumnName(Param.id), Employee.IdLength,
                        ColumnName(Param.name), Employee.name_length,
                        ColumnName(Param.employeeType), Employee.employeeType_length);
                    break;
                case Tables.Employees_FlightHours:
                    cmd = String.Format("CREATE TABLE {0} ({1} varchar({2}), {3} int);",
                        table.ToString(),
                        ColumnName(Param.id), Employee.IdLength,
                        ColumnName(Param.flyHours));
                    break;
                case Tables.Employees_Assignment:
                    cmd = String.Format("CREATE TABLE {0} ({1} varchar({2}), {3} varchar({4}));",
                        table.ToString(),
                        ColumnName(Param.id), Employee.IdLength,
                        ColumnName(Param.regNumber), (Aircraft.RegNumber_digitCount + Aircraft.RegNumber_letterCount));
                    break;
                case Tables.Employees_MaintenanceHistory:
                    cmd = String.Format("CREATE TABLE {0} ({1} varchar({2}), {3} varchar({4}), {5} varchar({6}));",
                        table.ToString(),
                        ColumnName(Param.id), Employee.IdLength,
                        ColumnName(Param.description), MaintenanceHistory.description_length,
                        ColumnName(Param.regNumber), (Aircraft.RegNumber_digitCount + Aircraft.RegNumber_letterCount));
                    break;
                default:
                    throw new ArgumentException("Wrong table type " + table.ToString() + " in 'CreateTableCmd'");
            }
        }

        return cmd;
    }

    public string GetPlanesThatNeedMaintenance()
    {
        return Execute(GetPlanesThatNeedMaintenanceCMD());
    }
    public static string GetPlanesThatNeedMaintenanceCMD()
    {
        string cmd = string.Empty;

        cmd +=
            "SELECT " + ColumnName(Param.regNumber) + " " +
            "FROM " + Tables.Cargo_Aircraft + " " +
            "WHERE " + ColumnName(Param.flyHours) + " - " + ColumnName(Param.lastMaintenance) + " >= " + Aircraft.MaintenanceRepetition_hours + " ";
        cmd +=
            "union " +
            "SELECT " + ColumnName(Param.regNumber) + " " +
            "FROM " + Tables.Passenger_Aircraft + " " +
            "WHERE " + ColumnName(Param.flyHours) + " - " + ColumnName(Param.lastMaintenance) + " >= " + Aircraft.MaintenanceRepetition_hours + ";";
        return cmd;
    }
    public class Plane
    {

    }
    public bool PlaneNeedsMaintenance(string regNumber, byte columnNumber = 0)
    {
        string result = Execute(PlaneNeedsMaintenanceCMD(regNumber), columnNumber);
        if (result == "1")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static string PlaneNeedsMaintenanceCMD(string regNumber)
    {
        string cmd = string.Empty;

        cmd +=
            "SELECT CASE " +
            "WHEN " + ColumnName(Param.flyHours) + " - " + ColumnName(Param.lastMaintenance) + " >= " + Aircraft.MaintenanceRepetition_hours + " " +
            "THEN 1 " +
            "ELSE 0 " +
            "END as TOTFORMAIN, * " +
            "FROM " + Tables.Cargo_Aircraft + " " +
            "WHERE " + ColumnName(Param.regNumber) + "='" + regNumber + "' ";
        cmd +=
            "union " +
            "SELECT CASE " +
            "WHEN " + ColumnName(Param.flyHours) + " - " + ColumnName(Param.lastMaintenance) + " >= " + Aircraft.MaintenanceRepetition_hours + " " +
            "THEN 1 " +
            "ELSE 0 " +
            "END as TOTFORMAIN, * " +
            "FROM " + Tables.Passenger_Aircraft + " " +
            "WHERE " + ColumnName(Param.regNumber) + "='" + regNumber + "';";
        return cmd;
    }
    public bool AssignEmployee(string regNumber, params uint[] employee_id)
    {
        string result = string.Empty;
        string cmd = string.Empty;

        foreach (uint employee in employee_id)
        {
            cmd += AssignEmployeeCmd(regNumber, employee);
        }
        try
        {
            Execute(cmd);
            return true;
        }
        catch (SqlException)
        {
            return false;
        }
    }
    public static string AssignEmployeeCmd(string regNumber, uint employee_id)
    {
        string cmd = string.Empty;

        cmd += String.Format("INSERT INTO {0} VALUES('{1}','{2}');",
            Tables.Employees_Assignment.ToString(),
            MathExt.IntToFixedString((int)employee_id, Employee.IdLength),
            regNumber);
        return cmd;
    }
    public bool UnassignEmployee(params uint[] employee_id)
    {
        return UnassignEmployee(employee_id);
    }
    public static string UnassignEmployeeCmd(uint employee_id)
    {
        // While employees could be unassigned using AssignEmployeeCmd("", id),
        // it's best to use this method as some companies might prefer unassigned
        // employees to be assigned to regnumber "not assigned" or something similar.
        return AssignEmployeeCmd("", employee_id);
    }
    public string AddHours(uint hours, string regNumber, byte columnNumber = 0)
    {
        if (TableExists(Tables.Cargo_Aircraft.ToString()) == true
            && TableExists(Tables.Passenger_Aircraft.ToString()) == true
            && TableExists(Tables.Employees_FlightHours.ToString()) == true)
            return Execute(AddHoursCmd(hours, regNumber), columnNumber);
        else
            throw new MissingMemberException("Missing table. Unable to create a table.");
    }
    public static string AddHoursCmd(uint hours, string regNumber)
    {
        string cmd;

        cmd = String.Format(
            "UPDATE {0} SET {1}={1} + '{2}' WHERE {3}='{4}';",
            Tables.Cargo_Aircraft.ToString(),
            ColumnName(Param.flyHours),
            hours,
            ColumnName(Param.regNumber),
            regNumber);
        cmd += String.Format(
            "UPDATE {0} SET {1}={1} + '{2}' WHERE {3}='{4}';",
            Tables.Passenger_Aircraft.ToString(),
            ColumnName(Param.flyHours),
            hours,
            ColumnName(Param.regNumber),
            regNumber);
        cmd += String.Format(
            "UPDATE f " +
            "SET f.{0} = f.{0} + {1} " +
            "FROM {2} as f " +
            "INNER JOIN {3} as a " +
            "ON f.{4} = a.{4} " +
            "WHERE a.{5} = '{6}';",
            ColumnName(Param.flyHours), hours,
            Tables.Employees_FlightHours.ToString(),
            Tables.Employees_Assignment.ToString(),
            ColumnName(Param.id),
            ColumnName(Param.regNumber),
            regNumber);
        return cmd;
    }
    public string AddMaintenance(uint id, string desc, string regNumber, byte columnNumber = 0)
    {
        if (TableExists(Tables.Employees_MaintenanceHistory.ToString()) == true)
            return Execute(AddMaintenanceCmd(id, desc, regNumber), columnNumber);
        else
            throw new MissingMemberException("Missing table. Unable to create a table.");
    }
    public static string AddMaintenanceCmd(uint id, string desc, string regNumber)
    {
        string cmd;

        cmd = String.Format(
            "INSERT INTO {0} VALUES('{1}','{2}','{3}');",
            Tables.Employees_MaintenanceHistory.ToString(),
            MathExt.IntToFixedString((int)id, Employee.IdLength),
            desc,
            regNumber);
        return cmd;
    }
    // TODO: Do more researach on <T> and obj, maybe this code can be improved.
    #region Insert
    public string Insert(CargoAircraft aircraft, byte columnNumber = 0)
    {
        string table_name = Tables.Cargo_Aircraft.ToString();
        if (TableExists(table_name) == true)
            return Execute(InsertCmd(aircraft), columnNumber);
        else
            throw new MissingMemberException("No " + table_name + " table found. Unable to create a table.");
    }
    public string Insert(PassengerAircraft aircraft, byte columnNumber = 0)
    {
        string table_name = Tables.Passenger_Aircraft.ToString();
        if (TableExists(table_name) == true)
            return Execute(InsertCmd(aircraft), columnNumber);
        else
            throw new MissingMemberException("No " + table_name + " table found. Unable to create a table.");
    }
    public string Insert(Employee employee, string regNumber, byte columnNumber = 0)
    {
        if (TableExists(Tables.Employees.ToString()) == true 
            && TableExists(Tables.Employees_FlightHours.ToString()) == true
            && TableExists(Tables.Employees_MaintenanceHistory.ToString()) == true
            && TableExists(Tables.Employees_Assignment.ToString()) == true)
            return Execute(InsertCmd(employee, regNumber), columnNumber);
        else
            throw new MissingMemberException("Missing table. Unable to create a table.");
    }
    public static string InsertCmd(CargoAircraft aircraft)
    {
        string cmd;
        cmd = String.Format("INSERT INTO {0} VALUES('{1}',{2},{3},'{4}',{5});",
            Tables.Cargo_Aircraft.ToString(),
            aircraft.RegNumber,
            aircraft.FlyHours,
            aircraft.LastMaintenance,
            aircraft.LastMaintenance_date_YMDHMSstring,
            aircraft.Capacity_metricTonnes);
        return cmd;
    }
    public static string InsertCmd(PassengerAircraft aircraft)
    {
        string cmd;
        cmd = String.Format("INSERT INTO {0} VALUES('{1}',{2},{3},'{4}',{5});",
            Tables.Passenger_Aircraft.ToString(),
            aircraft.RegNumber,
            aircraft.FlyHours,
            aircraft.LastMaintenance,
            aircraft.LastMaintenance_date_YMDHMSstring,
            aircraft.Capacity_seating);
        return cmd;
    }
    public static string InsertCmd(Employee employee, string regNumber)
    {
        string cmd = string.Empty;
        cmd = String.Format("INSERT INTO {0} VALUES('{1}','{2}','{3}');",
            Tables.Employees.ToString(),
            MathExt.IntToFixedString((int)employee.Id, Employee.IdLength),
            employee.Name,
            employee.EmployeeType.ToString());
        if (employee.EmployeeType == EmployeeType.Cabin_Crew
            || employee.EmployeeType == EmployeeType.Flight_Deck)
        {
            cmd += String.Format("INSERT INTO {0} VALUES('{1}','{2}');",
                Tables.Employees_Assignment.ToString(),
                MathExt.IntToFixedString((int)employee.Id, Employee.IdLength),
                regNumber);
            cmd += String.Format("INSERT INTO {0} VALUES('{1}',{2});",
                Tables.Employees_FlightHours.ToString(),
                MathExt.IntToFixedString((int)employee.Id, Employee.IdLength),
                0);
        }
        else if (employee.EmployeeType == EmployeeType.Ground_Crew)
        {
            cmd += String.Format("INSERT INTO {0} VALUES('{1}','{2}');",
                Tables.Employees_Assignment.ToString(),
                MathExt.IntToFixedString((int)employee.Id, Employee.IdLength),
                "N/A");
        }
        return cmd;
    }
    #endregion
    
    public bool TableExists(Tables table, SqlConnection connection)
    {
        String cmd = "SELECT TOP 1 * FROM " + table.ToString();
        try
        {
            Execute(cmd);
            return true;
        }
        catch (SqlException e)
        {
            if (e.Number == (int)ErrorNumbers.NoTable)
            {
                try
                {
                    cmd = CreateTableCmd(table);
                    Execute(cmd);
                    cmd = "SELECT TOP 1 * FROM " + table.ToString();
                    Execute(cmd);
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }

    public string RegNumber_last(Tables table, string regNumber_zero)
    {
        // The first letter of regNumber_begin is used as an indicator for all items in the table
        // by having a first letter, the need to check for regNumber in more than one table is eliminated
        // TODO: Option for advanced search were it searches the whole table for holes between regnumbers too
        string tableName = table.ToString();
        string regNumber_id = String.Empty;
        byte digitsLength = Aircraft.RegNumber_digitCount;
        byte lettersLength = Aircraft.RegNumber_letterCount;

        regNumber_id += regNumber_zero[0]; // Assign ID letter
        String cmd = "SELECT TOP 1 * FROM " + tableName + " ORDER BY " + ColumnName(Param.regNumber) + " DESC";
        String output = "";

        if (TableExists(tableName) == false)
            throw new MissingFieldException("Table '" + tableName + "' does not exist.");

        output = Execute(cmd);
        if (output == "")
        {
            output = regNumber_zero;
        }
        return output;
    }

    public void RecreateTable(Tables table)
    {
        DropTable(table.ToString());
        CreateTable(table);
    }

    public void ResetTable(Tables table)
    {
        ResetTable(table.ToString());
    }

    public void DropTable(Tables table)
    {
        DropTable(table.ToString());
    }
}