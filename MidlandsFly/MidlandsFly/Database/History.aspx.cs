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
            ChangeGrid_Maintenance(sender, e);
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
            if (SqlMidlandsFly.Instance.TableExists(SqlMidlandsFly.Instance.Table_Maintenance))
            {
                MidlandsFly_History.SelectCommand = command;
                GridViewTable.DataSourceID = MidlandsFly_History.ID;
                GridViewTable.DataBind();
            }
            else
            {
                this.Master.ErrMessage = (String.Format("Table {0} does not exist. Please restart the simulation or contact an administrator.", SqlMidlandsFly.Instance.Table_Maintenance.Name));
            }
        }
        catch (Exception ex)
        {
            this.Master.ErrMessage = (String.Format("An error has occured! Error message: {0}", ex.Message));
        }
    }

    protected void ChangeGrid_Maintenance(object sender, EventArgs e)
    {
        command = "SELECT * FROM [" + SqlMidlandsFly.Instance.Table_MaintenanceHistory.Name + "];";
        ChangeGrid(sender, e);
    }
}