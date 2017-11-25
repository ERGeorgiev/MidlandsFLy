using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// All employee classes should inherit from this class.
/// </summary>

// The following enum was created with the idea of using
// a single class for all employees and class them using "enum".
// I have decided that method is not optimal for the task.

public enum EmployeeType
{
    Flight_Deck,
    Cabin_Crew,
    Ground_Crew
}

public class Employee
{
    static List<string> employeeFirstNames = new List<string>();
    static List<string> employeeLastNames = new List<string>();
    static Random random = new Random();
    // A variable to track the employeeCount, in order to assign a unique number to each employee.
    protected static uint employeeCount;
    private uint id;
    private const byte idDigits = 5;  // The maximum number of digits in an id.
    private string name;
    public static byte name_length = 30;
    EmployeeType employeeType;
    public static byte employeeType_length = 30;

    public uint Id { get => id; private set => id = value; }
    public string Name { get => name; private set => name = value; }
    internal EmployeeType EmployeeType { get => employeeType; set => employeeType = value; }

    public static byte IdLength => idDigits;

    private Employee(EmployeeType employeeType, string name = "random")
    {
        employeeCount++;
        this.EmployeeType = employeeType;
        this.Id = employeeCount;
        if (String.Equals(name, "random", StringComparison.InvariantCultureIgnoreCase) == true)
        {
            this.Name = GetRandomName();
        }
        else
            this.Name = name;
    }

    public static Employee CreateInstance(EmployeeType employeeType, string name = "random")
    {
        if (LimitReached() == false)
            return new Employee(employeeType, name);
        else
            throw new OverflowException("The maximum number of empoyees (" + employeeCount + ") has been reached.");
    }

    public static bool LimitReached()
    {
        if (employeeCount <= (9 * Math.Pow(10, (idDigits - 1))))
            return false;
        else
            return true;
    }

    static string GetRandomName()
    {
        if (employeeFirstNames.Count == 0 || employeeLastNames.Count == 0)
        {
            fillEmployeeNames();
        }

        int firstNameIndex = random.Next(0, (employeeFirstNames.Count - 1));
        int lastNameIndex = random.Next(0, (employeeFirstNames.Count - 1));
        string name = employeeFirstNames[firstNameIndex] + " " + employeeLastNames[lastNameIndex];

        return name;

        void fillEmployeeNames()
        {
            #region EmployeeFirstNames
            employeeFirstNames.Add("Alma");
            employeeFirstNames.Add("Andrew");
            employeeFirstNames.Add("Bessie");
            employeeFirstNames.Add("Bob");
            employeeFirstNames.Add("Brian");
            employeeFirstNames.Add("Charles");
            employeeFirstNames.Add("Chia");
            employeeFirstNames.Add("Christopher");
            employeeFirstNames.Add("Dale");
            employeeFirstNames.Add("Diana");
            employeeFirstNames.Add("Erik");
            employeeFirstNames.Add("Eva");
            employeeFirstNames.Add("Glen");
            employeeFirstNames.Add("Gretchen");
            employeeFirstNames.Add("Herbert");
            employeeFirstNames.Add("Irene");
            employeeFirstNames.Add("Jason");
            employeeFirstNames.Add("Jean");
            employeeFirstNames.Add("Jesse");
            employeeFirstNames.Add("John");
            employeeFirstNames.Add("John");
            employeeFirstNames.Add("Karie");
            employeeFirstNames.Add("Kevil");
            employeeFirstNames.Add("Maurice");
            employeeFirstNames.Add("Mercedez");
            employeeFirstNames.Add("Monica");
            employeeFirstNames.Add("Nathan");
            employeeFirstNames.Add("Phyllis");
            employeeFirstNames.Add("Rebecca");
            employeeFirstNames.Add("Ronald");
            employeeFirstNames.Add("Scott");
            employeeFirstNames.Add("Shirley");
            employeeFirstNames.Add("Sonya");
            employeeFirstNames.Add("Stanley");
            employeeFirstNames.Add("Steve");
            employeeFirstNames.Add("Tracey");
            employeeFirstNames.Add("Tracy");
            employeeFirstNames.Add("Vivian");
            employeeFirstNames.Add("Will");
            #endregion
            #region EmployeeLastNames
            employeeLastNames.Add("Allen");
            employeeLastNames.Add("Burrow");
            employeeLastNames.Add("Carroll");
            employeeLastNames.Add("Chavarria");
            employeeLastNames.Add("Clemons");
            employeeLastNames.Add("Crawford");
            employeeLastNames.Add("Dehaan");
            employeeLastNames.Add("Duff");
            employeeLastNames.Add("Easley");
            employeeLastNames.Add("Fisher");
            employeeLastNames.Add("Fleming");
            employeeLastNames.Add("Hardy");
            employeeLastNames.Add("Harrell");
            employeeLastNames.Add("Hayes");
            employeeLastNames.Add("Isbell");
            employeeLastNames.Add("Jones");
            employeeLastNames.Add("Jones");
            employeeLastNames.Add("Kohr");
            employeeLastNames.Add("Ladd");
            employeeLastNames.Add("Marsh");
            employeeLastNames.Add("Merrill");
            employeeLastNames.Add("Montejano");
            employeeLastNames.Add("Murley");
            employeeLastNames.Add("Otero");
            employeeLastNames.Add("Paik");
            employeeLastNames.Add("Quinones");
            employeeLastNames.Add("Rawlings");
            employeeLastNames.Add("Robinson");
            employeeLastNames.Add("Roby");
            employeeLastNames.Add("Scott");
            employeeLastNames.Add("Smith");
            employeeLastNames.Add("Smith");
            employeeLastNames.Add("Snyder");
            employeeLastNames.Add("Sorrells");
            employeeLastNames.Add("Stone");
            employeeLastNames.Add("Stratton");
            employeeLastNames.Add("Walter");
            employeeLastNames.Add("Wise");
            employeeLastNames.Add("Ziegler");
            #endregion
            // Names generated by http://www.fakenamegenerator.com/
        }
    }
}