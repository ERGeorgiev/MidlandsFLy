using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// This class is used for storting information about aircraft that are considered to be "Cargo".
/// </summary>

public class CargoAircraft : Aircraft
{
    private static string regNumber_zero = "CAA000";
    private static string regNumber_end = regNumber_zero;
    private uint capacity_mTonnes;

    public uint Capacity_metricTonnes { get => capacity_mTonnes; set => capacity_mTonnes = value; }
    public static string RegNumber_zero { get => regNumber_zero; private set => regNumber_zero = value; }
    public static string RegNumber_end { get => regNumber_end; set => regNumber_end = value; }

    private CargoAircraft(string regNumber, uint flyHours = 0, uint lastMaintenance = 0, uint capacity_mTonnes = 0) : base(regNumber, flyHours, lastMaintenance)
    {
        RegNumber_end = regNumber;
        this.Capacity_metricTonnes = capacity_mTonnes;
    }

    // CreateInstance method is used for instanstiating instead of the default constructor in order
    // to skip class resource allocation in case a given parameter is invalid. (Prevents memory leaks)
    public static CargoAircraft CreateInstance(string regNumber = "", uint flyHours = 0, uint lastMaintenance = 0, uint capacity_mTonnes = 0)
    {
        if (regNumber == "")
        {
            RegNumber_end = RegNumber_Increment(RegNumber_end);
            return new CargoAircraft(RegNumber_end, flyHours, lastMaintenance, capacity_mTonnes);
        }
        else if (RegNumber_IsValid(regNumber) == true)
            return new CargoAircraft(regNumber, flyHours, lastMaintenance, capacity_mTonnes);
        else
            ThrowRegFormatException();
        return null;
    }
}