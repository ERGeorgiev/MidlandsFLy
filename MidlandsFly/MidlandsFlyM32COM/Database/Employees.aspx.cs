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
            this.Master.ErrMessage = String.Format("Message: {0}", ex.Message);
        }        
    }

    protected void ChangeGrid_FlightDeck(object sender, EventArgs e)
    {
        command = String.Format("select e.{0},convert(varchar(64),DECRYPTBYPASSPHRASE('12',{1})) {1},{2},{3}",
            Database.Enums.Parameter.id,
            Database.Enums.Parameter.name,
            Database.Enums.Parameter.employeeType,
            Database.Enums.Parameter.flyHours);
        command += String.Format(" from {0} e, {1} ehrs",
            SqlMidlandsFly.Instance.Table_Employees.Name,
            SqlMidlandsFly.Instance.Table_FlightHours.Name);
        command += String.Format(" where e.{0} = ehrs.{0}",
            Database.Enums.Parameter.id);
        command += String.Format(" AND e.{0} = 'Flight_Deck'",
            Database.Enums.Parameter.employeeType);
        ChangeGrid(sender, e);
    }

    protected void ChangeGrid_CabinCrew(object sender, EventArgs e)
    {
        command = String.Format("select e.{0},convert(varchar(64),DECRYPTBYPASSPHRASE('12',{1})) {1},{2},{3}",
            Database.Enums.Parameter.id,
            Database.Enums.Parameter.name,
            Database.Enums.Parameter.employeeType,
            Database.Enums.Parameter.flyHours);
        command += String.Format(" from {0} e, {1} ehrs",
            SqlMidlandsFly.Instance.Table_Employees.Name,
            SqlMidlandsFly.Instance.Table_FlightHours.Name);
        command += String.Format(" where e.{0} = ehrs.{0}",
            Database.Enums.Parameter.id);
        command += String.Format(" AND e.{0} = 'Cabin_Crew'",
            Database.Enums.Parameter.employeeType);
        ChangeGrid(sender, e);
    }

    protected void ChangeGrid_GroundCrew(object sender, EventArgs e)
    {
        command = "SELECT " + Database.Enums.Parameter.id + ",convert(varchar(64),DECRYPTBYPASSPHRASE('12'," + Database.Enums.Parameter.name + ")) AS " + Database.Enums.Parameter.name + "," + Database.Enums.Parameter.employeeType + " FROM [" + SqlMidlandsFly.Instance.Table_Employees.Name + "] WHERE " + Database.Enums.Parameter.employeeType + " = 'Ground_Crew';";
        ChangeGrid(sender, e);
    }
}