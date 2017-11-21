using System;
using System.Web.UI;
using MidlandsFly.Sql;
using MidlandsFly;

public partial class Homepage : System.Web.UI.Page
{
    // To try catch table in HTML or smth like that in case there is no such table
    private static Demonstration Demonstration = new Demonstration();

    protected void Page_Load(object sender, EventArgs e)
    {
        // Demonstration.sql_default.PlaneNeedsMaintenance("CAA001");
        // Simulati.GetPlanesThatNeedMaintenance();
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
        MidlandsFly_AircraftData.SelectCommand = "SELECT * FROM[" + SqlMidlandsFly.Instance.Table_Employees.Name + "]";
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

    protected void NewSimulation(object sender, EventArgs e)
    {
        Simulation.Demo();
        UpdatePanelTable.Update();
    }

    protected void ClearTables(object sender, EventArgs e)
    {
        SqlMidlandsFly.Instance.RecreateTables();
        SqlMidlandsFly.Instance.Execute();
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
        SqlMidlandsFly.Instance.AddHours((uint)Int32.Parse(TextBox_AddHoursToPlane_Hours.Text), TextBox_AddHoursToPlane_RegNumber.Text);
        SqlMidlandsFly.Instance.Execute();
    }

    protected void ChangeGrid(object sender, EventArgs e)
    {
    }

    protected void ChangeGrid_Cargo(object sender, EventArgs e)
    {
        if (SqlMidlandsFly.Instance.TableExists(SqlMidlandsFly.Instance.Table_Cargo))
        {
            MidlandsFly_AircraftData.SelectCommand = "SELECT * FROM [" + SqlMidlandsFly.Instance.Table_Cargo.Name + "]";
            GridViewTable.DataSourceID = MidlandsFly_AircraftData.ID;
            GridViewTable.DataBind();
        }
        else
        {
            // Warning
        }
    }

    protected void ChangeGrid_Passenger(object sender, EventArgs e)
    {
        if (SqlMidlandsFly.Instance.TableExists(SqlMidlandsFly.Instance.Table_Passenger))
        {
            MidlandsFly_AircraftData.SelectCommand = "SELECT * FROM [" + SqlMidlandsFly.Instance.Table_Passenger.Name + "]";
            GridViewTable.DataSourceID = MidlandsFly_AircraftData.ID;
            GridViewTable.DataBind();
        }
        else
        {
            // Warning
        }
        }

    protected void AddHours_24(object sender, EventArgs e)
    {

    }

    protected void AddHours_100(object sender, EventArgs e)
    {

    }
}