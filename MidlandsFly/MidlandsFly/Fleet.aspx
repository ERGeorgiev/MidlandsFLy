<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Fleet.aspx.cs" Inherits="Homepage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <header>
            <h1>FLEET</h1>
            <input id="Submit1" property="og:audio:type" type="submit" value="Search" />
        </header>
        <nav>
            <ul>
                <li><a href="Fleet.aspx">Fleet</a></li>            
                <li>-> In Flight</li>
                <li>-> Cargo</li>
                <li>-> Passenger</li>
                <li>-> Maintenance</li>    
                <li><a href="Employees.aspx">Employees</a></li>
                <li>-> Flying Hours</li>
                <li>-> Allocation - Aircrew</li>
                <li>-> Allocation - Ground Crew</li>
                <li></li>
                <li>Simulation</li>
                <li>-> 
                    <asp:LinkButton ID="Button_AddHours_24" OnClick="AddHours_24" runat="server">Add 24 Hours</asp:LinkButton></li>
                <li>-> 
                    <asp:LinkButton ID="Button_AddHours_100" OnClick="AddHours_100" runat="server">Add 100 Hours</asp:LinkButton></li>
                <li>-> 
                    <asp:LinkButton ID="Button_Demo2" OnClick="NewSimulation" runat="server">New Simulation</asp:LinkButton></li>
                <li>-> 
                    <asp:LinkButton ID="Button_Reset2" OnClick="ClearTables" runat="server">Clear Tables</asp:LinkButton></li>
            </ul>
        </nav>

        
        <article>
            <asp:UpdatePanel ID="UpdatePanelTable" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="Button_PassengerTable" runat="server" OnClick="ChangeGrid_Passenger" Text="Passenger" />
                    <asp:Button ID="Button_CargoTable" runat="server" OnClick="ChangeGrid_Cargo" Text="Cargo" />
                    

        <asp:GridView ID="GridViewTable" runat="server" AutoGenerateColumns="True" DataSourceID="" Style="margin-top: 0px" AllowPaging="True" AllowSorting="True" Width="687px" EnableModelValidation="True"></asp:GridView>    

                    <asp:SqlDataSource ID="MidlandsFly_AircraftData" runat="server" ConnectionString="<%$ ConnectionStrings:ponyairlineConnectionString %>" SelectCommand="SELECT * FROM [cargo_aircraft]" SelectCommandType="Text"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="AirlineDatabase_Ed" runat="server" ConnectionString="<%$ ConnectionStrings:airlineDatabaseConnectionString_Ed %>" SelectCommand="SELECT * FROM [cargo_aircraft]" ProviderName="<%$ ConnectionStrings:airlineDatabaseConnectionString_Ed.ProviderName %>"></asp:SqlDataSource>
                    <br />
                    Simulate flight:<br />
                    <asp:Button ID="Button_AddHoursToPlane" runat="server" OnClick="AddHoursToPlane" Text="Add" />
                    <asp:TextBox ID="TextBox_AddHoursToPlane_Hours" runat="server" OnTextChanged="TextBox1_TextChanged" Width="40px"></asp:TextBox>
                    hours to plane
                        <asp:TextBox ID="TextBox_AddHoursToPlane_RegNumber" runat="server" Width="80px"></asp:TextBox>
                </ContentTemplate>
            </asp:UpdatePanel>
        </article >
            
    </div>
</asp:Content>

<%--
/////////////////////////////////////////////////////////////////////////////
References:

# Display time on ASP.NET server side (CSASPNETServerClock)
https://code.msdn.microsoft.com/CSASPNETServerClock-23c659d4
    # MSDN: Calling Web Services from Client Script
    http://msdn.microsoft.com/en-us/library/bb398995.aspx

////////////////////////////////////////////////////////////////////////////
--%>