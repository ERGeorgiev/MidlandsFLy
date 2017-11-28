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

        public static void Demo()
        {
            SqlMidlandsFly sql = SqlMidlandsFly.Instance;
            ushort airplanesToAdd = 10;
            string cmd = String.Empty;
            string regNumber_Cargo = string.Empty;
            string regNumber_Passenger = string.Empty;

            
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
                    SqlAircraft.Instance.InsertCargo();
                    SqlAircraft.Instance.InsertPassenger();
                }
                sql.Execute();

                #endregion Add Airplanes
        }
    }
}