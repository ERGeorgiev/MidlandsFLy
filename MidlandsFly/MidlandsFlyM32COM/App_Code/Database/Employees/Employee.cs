using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

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
        int lastNameIndex = random.Next(0, (employeeLastNames.Count - 1));
        string name = employeeFirstNames[firstNameIndex] + " " + employeeLastNames[lastNameIndex];

        return name;

        void fillEmployeeNames()
        {
            XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/Employees.xml"));
            var firstNames = doc.Descendants("FirstName");
            foreach (var firstName in firstNames)
            {
                employeeFirstNames.Add(firstName.Value);
            }
            var lastNames = doc.Descendants("LastName");
            foreach (var lastName in lastNames)
            {
                employeeLastNames.Add(lastName.Value);
            }
        }
    }
}