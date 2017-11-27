using System;
using System.Web.UI;
using MidlandsFly.Sql;
using MidlandsFly;
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
            ChangeGrid_Cargo(sender, e);
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
            if (SqlMidlandsFly.Instance.TableExists(SqlMidlandsFly.Instance.Table_Cargo, SqlMidlandsFly.Instance.Table_Passenger))
            {
                MidlandsFly_AircraftData.SelectCommand = command;
                GridViewTable.DataSourceID = MidlandsFly_AircraftData.ID;
                GridViewTable.DataBind();
            }
            else
            {
                this.Master.ErrMessage = (String.Format("Table {0} and {1} does not exist. Please restart the simulation or contact an administrator.", SqlMidlandsFly.Instance.Table_Passenger.Name, SqlMidlandsFly.Instance.Table_Cargo.Name));
            }
        }
        catch (Exception ex)
        {
            this.Master.ErrMessage = String.Format("Message: {0}", ex.Message);
        }
    }

    protected void ChangeGrid_Cargo(object sender, EventArgs e)
    {
        command = String.Format("SELECT {0},{1},{2} FROM [{3}]",
            Database.Enums.Parameter.regNumber,
            Database.Enums.Parameter.flyHours,
            Database.Enums.Parameter.capacity_mTonnes,
            SqlMidlandsFly.Instance.Table_Cargo.Name);
        ChangeGrid(sender, e);
    }

    protected void ChangeGrid_Passenger(object sender, EventArgs e)
    {
        command = String.Format("SELECT {0},{1},{2} FROM [{3}]",
            Database.Enums.Parameter.regNumber,
            Database.Enums.Parameter.flyHours,
            Database.Enums.Parameter.capacity_seating,
            SqlMidlandsFly.Instance.Table_Passenger.Name);
        ChangeGrid(sender, e);
    }

    protected void ChangeGrid_Maintenance(object sender, EventArgs e)
    {
        command = "SELECT * FROM [" + SqlMidlandsFly.Instance.Table_Maintenance.Name + "]";
        ChangeGrid(sender, e);
    }
}