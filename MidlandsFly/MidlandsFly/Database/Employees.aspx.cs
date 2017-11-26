using System;
using System.Web.UI;
using MidlandsFly.Sql;
using MidlandsFly;
using Database.Enums;
using System.Web.UI.WebControls;

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

    protected void GridView_Align(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.HorizontalAlign = HorizontalAlign.Left;
        }
    }

    protected void ChangeGrid(object sender, EventArgs e)
    {
        try
        {
            if (SqlMidlandsFly.Instance.TableExists(SqlMidlandsFly.Instance.Table_Employees))
            {
                MidlandsFly_Employees.SelectCommand = command;
                GridViewTable.DataSourceID = MidlandsFly_Employees.ID;
                GridViewTable.DataBind();
            }
            else
            {
                this.Master.ErrMessage = (String.Format("Table {0} does not exist. Please restart the simulation or contact an administrator.", SqlMidlandsFly.Instance.Table_Employees.Name));
            }
        }
        catch (Exception ex)
        {
            this.Master.ErrMessage = (String.Format("An error has occured! Error message: {0}", ex.Message));
        }        
    }

    protected void ChangeGrid_FlightDeck(object sender, EventArgs e)
    {
        command = "SELECT " + Database.Enums.Parameter.id + ",convert(varchar(64),DECRYPTBYPASSPHRASE('12'," + Database.Enums.Parameter.name + ")) AS " + Database.Enums.Parameter.name + "," + Database.Enums.Parameter.employeeType + " FROM [" + SqlMidlandsFly.Instance.Table_Employees.Name + "] WHERE " + Database.Enums.Parameter.employeeType + " = 'Flight_Deck';";
        ChangeGrid(sender, e);
    }

    protected void ChangeGrid_CabinCrew(object sender, EventArgs e)
    {
        command = "SELECT " + Database.Enums.Parameter.id + ",convert(varchar(64),DECRYPTBYPASSPHRASE('12'," + Database.Enums.Parameter.name + ")) AS " + Database.Enums.Parameter.name + "," + Database.Enums.Parameter.employeeType + " FROM [" + SqlMidlandsFly.Instance.Table_Employees.Name + "] WHERE " + Database.Enums.Parameter.employeeType + " = 'Cabin_Crew';";
        ChangeGrid(sender, e);
    }

    protected void ChangeGrid_GroundCrew(object sender, EventArgs e)
    {
        command = "SELECT " + Database.Enums.Parameter.id + ",convert(varchar(64),DECRYPTBYPASSPHRASE('12'," + Database.Enums.Parameter.name + ")) AS "  +Database.Enums.Parameter.name + "," + Database.Enums.Parameter.employeeType + " FROM [" + SqlMidlandsFly.Instance.Table_Employees.Name + "] WHERE " + Database.Enums.Parameter.employeeType + " = 'Ground_Crew';";
        ChangeGrid(sender, e);
    }
}