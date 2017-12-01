using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Enum-class for tracking parameter names
/// </summary>
namespace Database
{
    namespace Enums
    {
        public static class Parameter // This class plays the role of enum and dictionary at the same time.
        {
            public static string regNumber = "Aircraft_Number";
            public static string flyHours = "Flying_Hours";
            public static string lastMaintenance = "Last_Maintenance";
            public static string maintenance_hour = "Maintenance_Hour";
            public static string capacity_mTonnes = "Capacity_MetricTonnes";
            public static string capacity_seating = "Capacity_Seating";
            public static string name = "Employee_Name";
            public static string id = "Employee_ID";
            public static string employeeType = "Employee_Type";
            public static string description = "Description";
            public static string stage = "Stage";
            public static string stage_hour = "Stage_Hour";
            public static string stage_date = "Stage_Date";
            public static string flight_duration = "Flight_Duration";
            public static string last_update = "Last_Update";
        }
    }
}