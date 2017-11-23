using System;
using System.Text;

/// <summary>
/// All aircaft classes should inherit from this class.
/// </summary>

public abstract class Aircraft
{
    #region Fields

    private const byte regNumber_digitCount = 3;
    private const byte regNumber_idCount = 1;
    // regNumber digit and letter count requirement variables.
    // As stated in the assignment, the regNumber should consist of 3 digits and 3 letters.
    private const byte regNumber_letterCount = 3;
    private uint flyHours;
    private uint lastMaintenance;
    private static uint maintenanceRepetition_hours = 200; // How often each plane needs maintenance.
    private static uint maintenanceDuration_hours = 24; // How often each plane needs maintenance.
    private DateTime lastMaintenance_date;
    private string regNumber;

    #endregion Fields

    #region Constructors

    public Aircraft(string regNumber, uint flyHours = 0, uint lastMaintenance = 0)
    {
        this.RegNumber = regNumber;
        this.FlyHours = flyHours;
        this.LastMaintenance = lastMaintenance;
    }

    #endregion Constructors

    #region Enums

    public enum Type
    {
        Cargo,
        Passenger,
        Unknown
    }

    #endregion Enums

    // recorded using flight hours

    #region Properties

    public static byte RegNumber_digitCount => regNumber_digitCount;

    public static byte RegNumber_idCount => regNumber_idCount;

    public static byte RegNumber_letterCount => regNumber_letterCount;

    public static byte RegNumber_symbolCount => (byte)(RegNumber_digitCount + RegNumber_letterCount);

    public uint FlyHours { get => flyHours; set => flyHours = value; }

    public uint LastMaintenance
    {
        get => lastMaintenance;
        set
        {
            if (value >= 0)
            {
                if (value <= FlyHours)
                {
                    lastMaintenance = value;
                    LastMaintenance_date = DateTime.Now;
                    LastMaintenance_date = LastMaintenance_date.Subtract(TimeSpan.FromHours(flyHours - lastMaintenance));
                }
                else
                {
                    lastMaintenance = FlyHours;
                    LastMaintenance_date = DateTime.Now;
                }
            }
            else
            {
                lastMaintenance = 0;
                LastMaintenance_date = DateTime.MinValue;
            }
        }
    }

    public string RegNumber
    {
        get
        {
            return this.regNumber;
        }
        set
        {
            this.regNumber = value;
            // The following code is commented out, as I have decided to implement registration
            // number check in the instance creators of any classes that inherit this abstract class.

            // Check if the given registration number is valid and throw an exception if it's not.
            //if (RegNumber_IsValid(value) == true)
            //    this.regNumber = value;
            //else
            //    ThrowRegFormatException();
        }
    }

    public DateTime LastMaintenance_date { get => lastMaintenance_date; set => lastMaintenance_date = value; }
    public string LastMaintenance_date_YMDstring { get => lastMaintenance_date.Year.ToString() + "-" + lastMaintenance_date.Month.ToString() + "-" + lastMaintenance_date.Day.ToString(); }
    public string LastMaintenance_date_YMDHMSstring
    {
        get
        {
            return DateToString(LastMaintenance_date);
        }
    }

    public static uint MaintenanceRepetition_hours { get => maintenanceRepetition_hours; }
    public static uint MaintenanceDuration_hours { get => maintenanceDuration_hours; }

    #endregion Properties

    #region Methods

    public static string DateToString(DateTime date)
    {
        return date.Year.ToString()
            + "-" + date.Month.ToString()
            + "-" + date.Day.ToString()
            + " " + date.Hour.ToString()
            + ":" + date.Minute.ToString()
            + ":" + "0";
    }

    public bool NeedsMaintenance()
    {
        if ((DateTime.Now - this.LastMaintenance_date).Hours > maintenanceRepetition_hours)
        {
            return true;
        }
        else
            return false;
    }

    public static bool NeedsMaintenance(DateTime lastMaintenance_date)
    {
        if ((DateTime.Now - lastMaintenance_date).Hours > maintenanceRepetition_hours)
        {
            return true;
        }
        else
            return false;
    }

    public bool InMaintenance()
    {
        if ((DateTime.Now - this.LastMaintenance_date).Hours > maintenanceDuration_hours)
        {
            return true;
        }
        else
            return false;
    }

    public static bool InMaintenance(DateTime lastMaintenance_date)
    {
        if ((DateTime.Now - lastMaintenance_date).Hours > maintenanceDuration_hours)
        {
            return true;
        }
        else
            return false;
    }

    public static string RegNumber_Increment(string regNumber)
    {
        string output = regNumber;
        if (output.Length == (RegNumber_digitCount + RegNumber_letterCount))
        {
            StringBuilder regNumber_letters = new StringBuilder(output.Substring(0, RegNumber_letterCount));
            int regNumber_digits = Int32.Parse(output.Substring(RegNumber_letterCount, regNumber_digitCount));

            if (regNumber_digits < Math.Pow(10, regNumber_digitCount))
            {
                regNumber_digits++;
                output = regNumber_letters.ToString() + MathExt.IntToFixedString(regNumber_digits, regNumber_digitCount);
                return output;
            }
            else // Letters incrementation
            {
                for (int i = (RegNumber_letterCount - 1); i >= RegNumber_idCount; i--)
                {
                    if (regNumber_letters[i] < 'Z')
                    {
                        regNumber_digits = 0;
                        regNumber_letters[i]++;
                        output = regNumber_letters.ToString() + MathExt.IntToFixedString(regNumber_digits, regNumber_digitCount);
                        return output;
                    }
                }
                // If the "for" didn't return, that would mean that the database is full.
                throw new IndexOutOfRangeException("RegNumber cannot be incremented further.");
            }
        }
        else
        {
            throw new FormatException("Wrong RegNumber format.");
        }
    }
    public static bool RegNumber_IsValid(string regNumber)
    {
        // Check if the length of the input matches the number of required and letters and digits,
        // return false if it does not.
        if (regNumber.Length != (regNumber_digitCount + RegNumber_letterCount))
        {
            return false;
        }

        // Create variables to track digit and letter count in order to check if the input contains
        // the required amount.
        byte digitCount = 0;
        byte letterCount = 0;

        // Iterate through the regNumber string in order to check each symbol and count the digits and letters.
        foreach (char symbol in regNumber)
        {
            if (Char.IsDigit(symbol))
            {
                // Increment the tracking variable for the number of digits in the input string.
                digitCount++;
            }
            else if (Char.IsLetter(symbol))
            {
                // Increment the tracking variable for the number of letters in the input string.
                letterCount++;
            }
            else
            {
                // Return false in case there is a symbol that is neither a letter nor a digit.
                return false;
            }
        }
        // Perform checks to see if digit and letter count within the input string matches, return
        // false if it does not and true if it does.
        if (digitCount != regNumber_digitCount)
            return false;
        else if (letterCount != RegNumber_letterCount)
            return false;
        else
            return true;
    }
    public static void ThrowRegFormatException()
    {
        // Using a function to throw a general but detailed exception, instead of throwing different
        // exceptions at different places. This improves memory and time usage.
        throw new FormatException("Registration Number must consist of " + regNumber_digitCount + " digits and " + RegNumber_letterCount + " letters.");
    }

    #endregion Methods
}