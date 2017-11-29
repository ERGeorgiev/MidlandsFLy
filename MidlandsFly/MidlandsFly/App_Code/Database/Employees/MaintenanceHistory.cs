using System;
using System.Collections.Generic;
using System.Web;
using System.Xml.Linq;

/// <summary>
/// This class is used for storting information about maintenance performed by each employee.
/// </summary>

public class MaintenanceHistory
{
    static List<string> desc = new List<string>();
    static Random random = new Random();

    private string regNumber;
    private string id;
    private string description;
    public static byte description_length = 255;

    public string Id { get => id; private set => id = value; }
    public string RegNumber { get => regNumber; private set => regNumber = value; }
    public string Description { get => description; private set => description = value; }

    public MaintenanceHistory(string regNumber, string id, string description)
    {
        this.RegNumber = regNumber;
        this.Id = id;
        this.Description = description;
    }

    public static string GenerateRandomDescription()
    {
        if (desc.Count == 0)
        {
            XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/Maintenance.xml"));
            var descriptionList = doc.Descendants("description");
            foreach (var descriptionElement in descriptionList)
            {
                desc.Add(descriptionElement.Value);
            }
        }

        return desc[random.Next(0, desc.Count)];
    }

    //public MaintenanceHistory(string date, string description = "none", MaintenanceType maintenanceType = MaintenanceType.Unknown) : this(description, maintenanceType)
    //{
    //    if (DateTime.TryParse(date, out DateTime dateCheck) == true)
    //    {
    //        this.Date = dateCheck;
    //    }
    //    else
    //        throw new FormatException("Date must be entered in the following format: MM/dd/yyyy");
    //}
}