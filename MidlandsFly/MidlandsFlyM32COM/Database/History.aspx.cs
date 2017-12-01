using System;
using System.Web.UI;
using MidlandsFly.Sql;
using MidlandsFly;
using Database.Enums;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

public partial class Homepage : System.Web.UI.Page
{
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
            this.Master.ErrMessage = String.Format("Message: {0}", ex.Message);
        }
    }

    protected void ChangeGrid_Maintenance(object sender, EventArgs e)
    {
        command = "SELECT * FROM [" + SqlMidlandsFly.Instance.Table_MaintenanceHistory.Name + "];";
        ChangeGrid(sender, e);
    }

    protected void FilterReset(object sender, EventArgs e)
    {
        TextBox_RegNumber.Text = "";
        ChangeGrid_Maintenance(sender, e);
    }

    protected void Filter(object sender, EventArgs e)
    {
        string regNumber = string.Empty;
        if (Regex.IsMatch(TextBox_RegNumber.Text, @"^[a-zA-Z]{3}\d{3}$")
            && TextBox_RegNumber.Text.Length <= 6)
        {
            regNumber = TextBox_RegNumber.Text;
            command = "SELECT * FROM [" + SqlMidlandsFly.Instance.Table_MaintenanceHistory.Name + "]";
            command += String.Format(" where {0} = '{1}'",
                Database.Enums.Parameter.regNumber,
                regNumber);
            ChangeGrid(sender, e);
        }
        else
        {
            this.Master.ErrMessage = "Did you hack the validator? Only 3 numbers and 3 letters are allowed! (Ex. AAA111)";
        }
    }
}