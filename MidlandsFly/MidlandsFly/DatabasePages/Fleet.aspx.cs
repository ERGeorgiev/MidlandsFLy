using System;
using System.Web.UI;
using MidlandsFly.Sql;
using MidlandsFly;

public partial class Homepage : System.Web.UI.Page
{
    // To try catch table in HTML or smth like that in case there is no such table
    private static Demonstration Demonstration = new Demonstration();
    private static string command = "unassigned";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (command == "unassigned")
        {
            ChangeGrid_Cargo(sender, e);
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
            MidlandsFly_AircraftData.SelectCommand = command;
            GridViewTable.DataSourceID = MidlandsFly_AircraftData.ID;
            GridViewTable.DataBind();
        }
        else
        {
            // Warning
        }
    }

    protected void ChangeGrid_Cargo(object sender, EventArgs e)
    {
        command = "SELECT * FROM [" + SqlMidlandsFly.Instance.Table_Cargo.Name + "]";
        ChangeGrid(sender, e);
    }

    protected void ChangeGrid_Passenger(object sender, EventArgs e)
    {
        command = "SELECT * FROM [" + SqlMidlandsFly.Instance.Table_Passenger.Name + "]";
        ChangeGrid(sender, e);
    }

    protected void ChangeGrid_Maintenance(object sender, EventArgs e)
    {
        command = "SELECT * FROM [" + SqlMidlandsFly.Instance.Table_Maintenance.Name + "]";
        ChangeGrid(sender, e);
    }
}