using System;
using System.Web.UI;
using MidlandsFly.Sql;
using MidlandsFly;
using Database.Enums;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class Homepage : System.Web.UI.Page
{
    // To try catch table in HTML or smth like that in case there is no such table
    private static Demonstration Demonstration = new Demonstration();
    private static string command = "unassigned";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (command == "unassigned")
        {
            ChangeGrid_Allocation(sender, e);
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
            if (SqlMidlandsFly.Instance.TableExists(SqlMidlandsFly.Instance.Table_Assignment))
            {
                MidlandsFly_History.SelectCommand = command;
                GridViewTable.DataSourceID = MidlandsFly_History.ID;
                GridViewTable.DataBind();
            }
            else
            {
                this.Master.ErrMessage = (String.Format("Table {0} does not exist. Please restart the simulation or contact an administrator.", SqlMidlandsFly.Instance.Table_Assignment.Name));
            }
        }
        catch (Exception ex)
        {
            this.Master.ErrMessage = String.Format("Message: {0}", ex.Message);
        }
    }

    protected void ChangeGrid_Allocation(object sender, EventArgs e)
    {
        command = String.Format("select e.{0},convert(varchar(64),DECRYPTBYPASSPHRASE('12',{1})) {1},{2},{3}",
            Database.Enums.Parameter.id,
            Database.Enums.Parameter.name,
            Database.Enums.Parameter.employeeType,
            Database.Enums.Parameter.regNumber);
        command += String.Format(" from {0} e, {1} ehrs, {2} eloc",
            SqlMidlandsFly.Instance.Table_Employees.Name,
            SqlMidlandsFly.Instance.Table_FlightHours.Name,
            SqlMidlandsFly.Instance.Table_Assignment.Name);
        command += String.Format(" where e.{0} = ehrs.{0} AND e.{0} = eloc.{0}",
            Database.Enums.Parameter.id);

        //command = "SELECT * FROM [" + SqlMidlandsFly.Instance.Table_Assignment.Name + "];";
        ChangeGrid(sender, e);
    }

    protected void FilterReset(object sender, EventArgs e)
    {
        TextBox_RegNumber.Text = "";
        ChangeGrid_Allocation(sender, e);
    }

    protected void Filter(object sender, EventArgs e)
    {
        string regNumber = string.Empty;
        if (Regex.IsMatch(TextBox_RegNumber.Text, @"^[a-zA-Z]{3}\d{3}$")
            && TextBox_RegNumber.Text.Length <= 6)
        {
            regNumber = TextBox_RegNumber.Text;
            command = String.Format("select e.{0},convert(varchar(64),DECRYPTBYPASSPHRASE('12',{1})) {1},{2},{3}",
                Database.Enums.Parameter.id,
                Database.Enums.Parameter.name,
                Database.Enums.Parameter.employeeType,
                Database.Enums.Parameter.regNumber);
            command += String.Format(" from {0} e, {1} ehrs, {2} eloc",
                SqlMidlandsFly.Instance.Table_Employees.Name,
                SqlMidlandsFly.Instance.Table_FlightHours.Name,
                SqlMidlandsFly.Instance.Table_Assignment.Name);
            command += String.Format(" where e.{0} = ehrs.{0} AND e.{0} = eloc.{0}",
                Database.Enums.Parameter.id);
            command += String.Format(" AND eloc.{0} = '{1}'",
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