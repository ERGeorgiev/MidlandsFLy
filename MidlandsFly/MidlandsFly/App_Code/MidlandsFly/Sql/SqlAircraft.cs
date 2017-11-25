using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Database.Sql;
using Database.Enums;
using System.Data.SqlClient;
using MidlandsFly.Sql;

/// <summary>
/// Provides methods for manipulating aircraft-related tables.
/// </summary>
namespace MidlandsFly
{
    namespace Sql
    {
        public class SqlAircraft
        {
            private static SqlAircraft instance;

            public static SqlAircraft Instance
            {
                get
                {
                    if (instance == null)
                        instance = new SqlAircraft();
                    return instance;
                }
            }
            // TODO: Do more researach on <T> and obj, maybe this code can be improved.
            public void Insert(CargoAircraft aircraft, byte columnNumber = 0)
            {
                SqlMidlandsFly.Instance.AddCommand(InsertCmd(aircraft));
                SqlStage.Instance.Insert(aircraft.RegNumber);
            }
            public void Insert(PassengerAircraft aircraft, byte columnNumber = 0)
            {
                SqlMidlandsFly.Instance.AddCommand(InsertCmd(aircraft));
                SqlStage.Instance.Insert(aircraft.RegNumber);
            }
            public SqlCommand InsertCmd(CargoAircraft aircraft)
            {
                string text;
                text = String.Format("INSERT INTO {0} VALUES('{1}',{2},{3},{4});",
                    SqlMidlandsFly.Instance.Table_Cargo.Name,
                    aircraft.RegNumber,
                    aircraft.FlyHours,
                    aircraft.LastMaintenance,
                    aircraft.Capacity_metricTonnes);
                return new SqlCommand(text);
            }
            public SqlCommand InsertCmd(PassengerAircraft aircraft)
            {
                string text;
                text = String.Format("INSERT INTO {0} VALUES('{1}',{2},{3},{4});",
                    SqlMidlandsFly.Instance.Table_Passenger.Name,
                    aircraft.RegNumber,
                    aircraft.FlyHours,
                    aircraft.LastMaintenance,
                    aircraft.Capacity_seating);
                return new SqlCommand(text);
            }
        }
    }
}