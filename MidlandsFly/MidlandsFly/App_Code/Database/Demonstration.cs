using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

// TODO: 
// BaseCompareValidator class
// ReuiredField Validator
// Compare Validator
// Range Validator
// RegularExpression Validator
// Custom Validator
// Validation Summary* - for errors

public class Demonstration
{
    public MidlandsFlySQL sql_ed = new MidlandsFlySQL("airlinedatabaseserver.database.windows.net", "ponyAdmin", "aaaa11!!", "airlineDatabase");
    public MidlandsFlySQL sql_default = new MidlandsFlySQL();

    const byte perPlane_FlightDeck = 3;
    const byte perPlane_CabinCrew = 3;
    const byte perPlane_Ground = 5;

    public void Demo(MidlandsFlySQL sql)
    {
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

        CargoAircraft.RegNumber_end = sql.RegNumber_last(MidlandsFlySQL.Tables.Cargo_Aircraft, CargoAircraft.RegNumber_zero);
        PassengerAircraft.RegNumber_end = sql.RegNumber_last(MidlandsFlySQL.Tables.Passenger_Aircraft, PassengerAircraft.RegNumber_zero);
        for (ushort i = 0; i < airplanesToAdd; i++)
        {
            // Add a limited random number of airplanes with random parameters.
            addAircraft_Cargo();
            addAircraft_Passenger();
        }
        sql.Execute(cmd);

        void addAircraft_Cargo()
        {
            // Constant, determining the amount of crew per plane, as noted in the assignment.

            CargoAircraft aircraft = CargoAircraft.CreateInstance();
            aircraft.FlyHours = (uint)random.Next(0, 100000);
            // FlyHours modulo 200 would give us exactly when the last maintenance was,
            // (if a maintenance was done exactly at every 200 hours, which is unlikely,
            // but it will look aesthetic in the table
            aircraft.LastMaintenance = (uint)(aircraft.FlyHours - random.Next(0, 199));
            aircraft.Capacity_metricTonnes = (ushort)(random.Next(37, 92));

            try
            {
                cmd += MidlandsFlySQL.InsertCmd(aircraft);
                addEmployees(Aircraft.Type.Cargo, aircraft.RegNumber);
            }
            catch (SqlException e)
            {
                // Display SQL exception to user, dont throw afterwards.
            }
            catch (Exception)
            {
                throw;
            }
        }

        void addAircraft_Passenger()
        {
            // Constants, determining the amount of crew per plane, as noted in the assignment.

            PassengerAircraft aircraft = PassengerAircraft.CreateInstance();
            aircraft.FlyHours = (uint)random.Next(0, 50000);
            // FlyHours modulo 200 would give us exactly when the last maintenance was,
            // (if a maintenance was done exactly at every 200 hours, which is unlikely,
            // but it will look aesthetic in the table
            aircraft.LastMaintenance = (uint)(aircraft.FlyHours - random.Next(0, 199));
            aircraft.Capacity_seating = (ushort)(random.Next(4, 853));

            try
            {
                cmd += MidlandsFlySQL.InsertCmd(aircraft);
                addEmployees(Aircraft.Type.Passenger, aircraft.RegNumber);
                cmd += MidlandsFlySQL.AddHoursCmd((uint)random.Next(0, 50000), aircraft.RegNumber);
            }
            catch (SqlException e)
            {
                // Display SQL exception to user, dont throw afterwards.
            }
            catch (Exception)
            {
                throw;
            }
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
                    cmd += MidlandsFlySQL.InsertCmd(employee, regNumber);
                }
            }
            void addFlightDeck()
            {
                for (int i = 0; i < perPlane_FlightDeck; i++)
                {
                    Employee employee = Employee.CreateInstance(EmployeeType.Flight_Deck);
                    cmd += MidlandsFlySQL.InsertCmd(employee, regNumber);
                }
            }
            void addGroundCrew()
            {
                for (int i = 0; i < perPlane_Ground; i++)
                {
                    Employee employee = Employee.CreateInstance(EmployeeType.Ground_Crew);
                    cmd += MidlandsFlySQL.InsertCmd(employee, regNumber);
                    for (int m = 0; m <= random.Next(1,3); m++)
                        cmd += MidlandsFlySQL.AddMaintenanceCmd(employee.Id, MaintenanceHistory.GenerateRandomDescription(), regNumber);
                }
            }
        }
        #endregion

        void Maintenance()
        {
            string[] planes = sql_default.GetPlanesThatNeedMaintenance().Split(';');
            string cmdMaint = string.Empty;

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