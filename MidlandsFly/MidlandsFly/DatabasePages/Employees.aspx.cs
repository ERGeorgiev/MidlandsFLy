using System;
using System.Web.UI;
using MidlandsFly.Sql;
using MidlandsFly;
using Database.Enums;

public partial class Homepage : System.Web.UI.Page
{
    // To try catch table in HTML or smth like that in case there is no such table
    private static Demonstration Demonstration = new Demonstration();
    private static string command = "unassigned";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (command == "unassigned")
        {
            ChangeGrid_CabinCrew(sender, e);
        }
        else
        {
            ChangeGrid(sender, e);
        }
    }

    protected void ChangeGrid(object sender, EventArgs e)
    {
        if (SqlMidlandsFly.Instance.TableExists(SqlMidlandsFly.Instance.Table_Employees))
        {
            MidlandsFly_Employees.SelectCommand = command;
            GridViewTable.DataSourceID = MidlandsFly_Employees.ID;
            GridViewTable.DataBind();
        }
        else
        {
            // Warning
        }
    }

    protected void ChangeGrid_FlightDeck(object sender, EventArgs e)
    {
        command = "SELECT " + Parameter.id + ",convert(varchar(64),DECRYPTBYPASSPHRASE('12'," + Parameter.name + ")) AS " + Parameter.name + "," + Parameter.employeeType + " FROM [" + SqlMidlandsFly.Instance.Table_Employees.Name + "] WHERE " + Parameter.employeeType + " = 'Flight_Deck';";
        ChangeGrid(sender, e);
    }

    protected void ChangeGrid_CabinCrew(object sender, EventArgs e)
    {
        command = "SELECT " + Parameter.id + ",convert(varchar(64),DECRYPTBYPASSPHRASE('12'," + Parameter.name + ")) AS " + Parameter.name + "," + Parameter.employeeType + " FROM [" + SqlMidlandsFly.Instance.Table_Employees.Name + "] WHERE " + Parameter.employeeType + " = 'Cabin_Crew';";
        ChangeGrid(sender, e);
    }

    protected void ChangeGrid_GroundCrew(object sender, EventArgs e)
    {
        command = "SELECT " + Parameter.id + ",convert(varchar(64),DECRYPTBYPASSPHRASE('12'," + Parameter.name + ")) AS "  +Parameter.name + "," + Parameter.employeeType + " FROM [" + SqlMidlandsFly.Instance.Table_Employees.Name + "] WHERE " + Parameter.employeeType + " = 'Ground_Crew';";
        ChangeGrid(sender, e);
    }
}