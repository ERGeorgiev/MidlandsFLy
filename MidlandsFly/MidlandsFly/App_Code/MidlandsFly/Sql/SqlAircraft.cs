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
            private static Random random = new Random();

            public static SqlAircraft Instance
            {
                get
                {
                    if (instance == null)
                        instance = new SqlAircraft();
                    return instance;
                }
            }
            public void InsertCargo(byte columnNumber = 0)
            {
                CargoAircraft aircraft = CargoAircraft.CreateInstance();
                aircraft.FlyHours = (uint)random.Next(0, 100000);
                aircraft.LastMaintenance = (uint)(aircraft.FlyHours - random.Next(0, 199));
                aircraft.Capacity_metricTonnes = (ushort)(random.Next(37, 92));

                Instance.Insert(aircraft);
            }
            public void InsertPassenger(byte columnNumber = 0)
            {
                PassengerAircraft aircraft = PassengerAircraft.CreateInstance();
                aircraft.FlyHours = (uint)random.Next(0, 50000);
                aircraft.LastMaintenance = (uint)(aircraft.FlyHours - random.Next(0, 250));
                aircraft.Capacity_seating = (ushort)(random.Next(4, 853));

                Instance.Insert(aircraft);
            }
            public void Insert(CargoAircraft aircraft, byte columnNumber = 0)
            {
                SqlMidlandsFly.Instance.AddCommand(InsertCmd(aircraft));
                SqlStage.Instance.Insert(aircraft.RegNumber);
                SqlEmployees.Instance.Insert(EmployeeType.Cabin_Crew, aircraft, aircraft.CabinCrew);
                SqlEmployees.Instance.Insert(EmployeeType.Flight_Deck, aircraft, aircraft.FlightCrew);
                SqlEmployees.Instance.Insert(EmployeeType.Ground_Crew, number: aircraft.GroundCrew);
            }
            public void Insert(PassengerAircraft aircraft, byte columnNumber = 0)
            {
                SqlMidlandsFly.Instance.AddCommand(InsertCmd(aircraft));
                SqlStage.Instance.Insert(aircraft.RegNumber);
                SqlEmployees.Instance.Insert(EmployeeType.Cabin_Crew, aircraft, aircraft.CabinCrew);
                SqlEmployees.Instance.Insert(EmployeeType.Flight_Deck, aircraft, aircraft.FlightCrew);
                SqlEmployees.Instance.Insert(EmployeeType.Ground_Crew, number: aircraft.GroundCrew);
            }
            public SqlCommand InsertCmd(CargoAircraft aircraft)
            {
                string text;
                text = String.Format("INSERT INTO {0} VALUES('{1}',{2},{3},{4},{5});",
                    SqlMidlandsFly.Instance.Table_Cargo.Name,
                    aircraft.RegNumber,
                    aircraft.FlyHours,
                    aircraft.LastMaintenance,
                    aircraft.Capacity_metricTonnes,
                     "SYSDATETIME()");
                return new SqlCommand(text);
            }
            public SqlCommand InsertCmd(PassengerAircraft aircraft)
            {
                string text;
                text = String.Format("INSERT INTO {0} VALUES('{1}',{2},{3},{4},{5});",
                    SqlMidlandsFly.Instance.Table_Passenger.Name,
                    aircraft.RegNumber,
                    aircraft.FlyHours,
                    aircraft.LastMaintenance,
                    aircraft.Capacity_seating,
                     "SYSDATETIME()");
                return new SqlCommand(text);
            }
        }
    }
}