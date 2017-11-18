using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Homepage : System.Web.UI.Page
{
    // To try catch table in HTML or smth like that in case there is no such table
    static Demonstration Demonstration = new Demonstration();
    static bool Ed = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        // Demonstration.sql_default.PlaneNeedsMaintenance("CAA001");
        Demonstration.sql_default.GetPlanesThatNeedMaintenance();
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void Button_Home(object sender, EventArgs e)
    {
        Response.Redirect("Homepage.aspx");
    }

    protected void LoadTable_Employees(object sender, EventArgs e)
    {
        //GridViewTable.DataSourceID = MidlandsFly_Aircraft_Passenger;
        MidlandsFly_Aircraft_Cargo.SelectCommand = "SELECT * FROM[" + MidlandsFlySQL.Tables.Employees.ToString() + "]";
        //GridViewTable.UpdateMethod = 
        GridViewTable.UpdateRow(0, true);
    }

    protected void LoadTable_Cargo(object sender, EventArgs e)
    {
        //GridViewTable.DataSourceID
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        Response.Redirect("About.aspx");
    }

    protected void MidlandsFly_Demo(object sender, EventArgs e)
    {
        if (Ed == false)
        {
            Demonstration.Demo(Demonstration.sql_default);
        }
        else
        {
            Demonstration.Demo(Demonstration.sql_ed);
        }

        UpdatePanelTable.Update();
    }

    protected void MidlandsFly_Reset(object sender, EventArgs e)
    {
        if (Ed == false)
        {
            Demonstration.sql_default.RecreateTable(MidlandsFlySQL.Tables.Cargo_Aircraft);
            Demonstration.sql_default.RecreateTable(MidlandsFlySQL.Tables.Passenger_Aircraft);
            Demonstration.sql_default.RecreateTable(MidlandsFlySQL.Tables.Employees);
            Demonstration.sql_default.RecreateTable(MidlandsFlySQL.Tables.Employees_Assignment);
            Demonstration.sql_default.RecreateTable(MidlandsFlySQL.Tables.Employees_FlightHours);
            Demonstration.sql_default.RecreateTable(MidlandsFlySQL.Tables.Employees_MaintenanceHistory);
        }
        else
        {
            Demonstration.sql_ed.RecreateTable(MidlandsFlySQL.Tables.Cargo_Aircraft);
            Demonstration.sql_ed.RecreateTable(MidlandsFlySQL.Tables.Passenger_Aircraft);
            Demonstration.sql_ed.RecreateTable(MidlandsFlySQL.Tables.Employees);
            Demonstration.sql_ed.RecreateTable(MidlandsFlySQL.Tables.Employees_Assignment);
            Demonstration.sql_ed.RecreateTable(MidlandsFlySQL.Tables.Employees_FlightHours);
            Demonstration.sql_ed.RecreateTable(MidlandsFlySQL.Tables.Employees_MaintenanceHistory);
        }

        UpdatePanelTable.Update();
    }

    protected void Button6_Click(object sender, EventArgs e)
    {
    }

    protected void Button7_Click(object sender, EventArgs e)
    {
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }

    protected void AddHoursToPlane(object sender, EventArgs e)
    {
        Demonstration.sql_default.AddHours((uint)Int32.Parse(TextBox_AddHoursToPlane_Hours.Text), TextBox_AddHoursToPlane_RegNumber.Text);
    }
}