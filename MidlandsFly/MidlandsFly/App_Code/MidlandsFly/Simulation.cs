using MidlandsFly.Sql;
using System;
using System.Collections.Generic;

/// <summary>
/// Summary description for SimulationSQL
/// </summary>
namespace MidlandsFly
{
    public static class Simulation
    {
        private const byte perPlane_FlightDeck = 3;
        private const byte perPlane_CabinCrew = 3;
        private const byte perPlane_Ground = 5;

        public static void Demo()
        {
            SqlMidlandsFly sql = SqlMidlandsFly.Instance;
            ushort airplanesToAdd = 10;
            string cmd = String.Empty;
            string regNumber_Cargo = string.Empty;
            string regNumber_Passenger = string.Empty;

            Random random = new Random();

            #region Add Airplanes

            // Inserting Aircraft into the database was a very-slow process,
            // as I first created the code to check for table existence before
            // every single command.
            // Furthermore I sent the commands one after another, which
            // would take too much time (more than 5 minutes) to create
            // around 200 entries.
            // The current system instead creates a huge string of commands and
            // only then it connects and sends it for processing.
            // This has cut down process time 60 times (from 8 minutes to 8 seconds).
            // While faster, this method is a bit less safe if not handled correctly.
            CargoAircraft.RegNumber_end = sql.RegNumber_last(sql.Table_Cargo, CargoAircraft.RegNumber_zero);
            PassengerAircraft.RegNumber_end = sql.RegNumber_last(sql.Table_Passenger, PassengerAircraft.RegNumber_zero);
            for (ushort i = 0; i < airplanesToAdd; i++)
            {
                // Add a limited random number of airplanes with random parameters.
                addAircraft_Cargo();
                addAircraft_Passenger();
            }
            sql.Execute();
            List<string> PlanesThatNeedMaintenance = SqlMaintenance.Instance.GetPlanesThatNeedMaintenance();
            foreach (string plane in PlanesThatNeedMaintenance)
            {
                SqlMaintenance.Instance.Insert(plane);
            }
            sql.Execute();

            void addAircraft_Cargo()
            {
                // Constant, determining the amount of crew per plane, as noted in the assignment.

                CargoAircraft aircraft = CargoAircraft.CreateInstance();
                aircraft.FlyHours = (uint)random.Next(0, 100000);
                // FlyHours modulo 200 would give us exactly when the last maintenance was,
                // (if a maintenance was done exactly at every 200 hours, which is unlikely,
                // but it will look aesthetic in the table
                aircraft.LastMaintenance = (uint)(aircraft.FlyHours - random.Next(0, 250));
                aircraft.Capacity_metricTonnes = (ushort)(random.Next(37, 92));

                SqlAircraft.Instance.Insert(aircraft);
                addEmployees(Aircraft.Type.Cargo, aircraft.RegNumber);
            }

            void addAircraft_Passenger()
            {
                // Constants, determining the amount of crew per plane, as noted in the assignment.

                PassengerAircraft aircraft = PassengerAircraft.CreateInstance();
                aircraft.FlyHours = (uint)random.Next(0, 50000);
                // FlyHours modulo 200 would give us exactly when the last maintenance was,
                // (if a maintenance was done exactly at every 200 hours, which is unlikely,
                // but it will look aesthetic in the table
                aircraft.LastMaintenance = (uint)(aircraft.FlyHours - random.Next(0, 250));
                aircraft.Capacity_seating = (ushort)(random.Next(4, 853));

                SqlAircraft.Instance.Insert(aircraft);
                addEmployees(Aircraft.Type.Passenger, aircraft.RegNumber);
            }

            void addEmployees(Aircraft.Type aircraftType, string regNumber)
            {
                switch (aircraftType)
                {
                    case Aircraft.Type.Passenger:
                        addCabinCrew();
                        addFlightDeck();
                        addGroundCrew();
                        break;

                    case Aircraft.Type.Cargo:
                        addFlightDeck();
                        addGroundCrew();
                        break;

                    default:
                        throw new InvalidOperationException("Unknown aircraft type '" + aircraftType.ToString() + "', cannot add employees.");
                }

                void addCabinCrew()
                {
                    for (int i = 0; i < perPlane_CabinCrew; i++)
                    {
                        Employee employee = Employee.CreateInstance(EmployeeType.Cabin_Crew);
                        SqlEmployees.Instance.Insert(employee, regNumber);
                    }
                }
                void addFlightDeck()
                {
                    for (int i = 0; i < perPlane_FlightDeck; i++)
                    {
                        Employee employee = Employee.CreateInstance(EmployeeType.Flight_Deck);
                        SqlEmployees.Instance.Insert(employee, regNumber);
                    }
                }
                void addGroundCrew()
                {
                    for (int i = 0; i < perPlane_Ground; i++)
                    {
                        Employee employee = Employee.CreateInstance(EmployeeType.Ground_Crew);
                        SqlEmployees.Instance.Insert(employee, regNumber);
                        for (int m = 0; m <= random.Next(1, 3); m++)
                            SqlEmployees.Instance.AddMaintenance(employee.Id, MaintenanceHistory.GenerateRandomDescription(), regNumber);
                    }
                }
            }

            #endregion Add Airplanes

            void Maintenance()
            {
                List<string> planes = SqlMaintenance.Instance.GetPlanesThatNeedMaintenance();

                //foreach (string plane in planes)
                //{
                //    cmdMaint +=
                //}
                // ToDo:
                // get random ground employee assigned to "N/A" and assign it to the plane that needs maintenance
                // Todo:
                // on page load check if theres a plane that is no longer in maintenance and pick all ground crew assigned to it and assign them to "N/A"
            }
        }
    }
}