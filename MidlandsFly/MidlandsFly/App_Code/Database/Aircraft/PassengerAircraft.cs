using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// This class is used for storting information about aircraft that are considered to be "Passenger".
/// </summary>

public class PassengerAircraft : Aircraft
{
    private static string regNumber_zero = "PAA000";
    private static string regNumber_end = regNumber_zero;
    private ushort capacity_seating;
    
    public ushort Capacity_seating { get => capacity_seating; set => capacity_seating = value; }
    public static string RegNumber_zero { get => regNumber_zero; private set => regNumber_zero = value; }
    public static string RegNumber_end { get => regNumber_end; set => regNumber_end = value; }

    private PassengerAircraft(string regNumber, uint flyHours = 0, uint lastMaintenance = 0, ushort capacity_seating = 0) : base(regNumber, flyHours, lastMaintenance)
    {
        RegNumber_end = regNumber;
        this.Capacity_seating = capacity_seating;
    }

    // CreateInstance method is used for instanstiating instead of the default constructor in order
    // to skip class resource allocation in case a given parameter is invalid. (Prevents memory leaks)
    public static PassengerAircraft CreateInstance(string regNumber = "", uint flyHours = 0, uint lastMaintenance = 0, ushort capacity_seating = 0)
    {
        if (regNumber == "")
        {
            RegNumber_end = RegNumber_Increment(RegNumber_end);
            return new PassengerAircraft(RegNumber_end, flyHours, lastMaintenance, capacity_seating);
        }
        if (RegNumber_IsValid(regNumber) == true)
            return new PassengerAircraft(regNumber, flyHours, lastMaintenance, capacity_seating);
        else
            ThrowRegFormatException();
        return null;
    }
}