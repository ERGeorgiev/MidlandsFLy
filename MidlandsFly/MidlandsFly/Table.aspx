<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Table.aspx.cs" Inherits="Homepage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        div.container {
            width: 100%;
            border: 1px solid gray;
        }

        header, footer {
            padding: 1em;
            color: white;
            background-color: black;
            clear: left;
            text-align: center;
        }

        nav {
            float: left;
            height: 350px;
            max-width: 160px;
            margin: 0;
            padding: 1em;
        }

            nav ul {
                list-style-type: none;
                padding: 0;
            }

                nav ul a {
                    text-decoration: none;
                }

        article {
            margin-left: 250px;
            border-left: 1px solid gray;
            padding: 1em;
            overflow: hidden;
        }
    </style>
</head>
<body>
    <div class="container">
        <form id="form1" runat="server" draggable="true">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
            <header>
                <h1>Midlands Fly</h1>
                <p>Cargo Fleet</p>
                <input id="Submit1" property="og:audio:type" type="submit" value="Search" />
            </header>
            <nav>
                <ul>
                    <li><a href="Homepage.aspx">Home</a></li>
                    <li><a href="About.aspx">About</a></li>
                    <li></li>
                    <li><a href="#">Fleet</a></li>
                    <li><asp:LinkButton ID="Button_LoadTable_Cargo" OnClick="LoadTable_Cargo" runat="server">Cargo</asp:LinkButton></li>
                    <li>-> <a href="#">Passenger</a></li>
                    <li>-> <a href="#">Maintenance</a></li>
                    <li></li>
                    <li><asp:LinkButton ID="Button_LoadTable_Employees" OnClick="LoadTable_Employees" runat="server">Employees</asp:LinkButton></li>
                    <li>-> <a href="#">Flying Hours</a></li>
                    <li>-> <a href="#">Allocation - Aircrew</a></li>
                    <li>-> <a href="#">Allocation - Ground Crew</a></li>
                    <li></li>
                    <li><asp:LinkButton ID="Button_Demo2" OnClick="MidlandsFly_Demo" runat="server">Demonstration</asp:LinkButton></li>
                    <li><asp:LinkButton ID="Button_Reset2" OnClick="MidlandsFly_Reset" runat="server">Clear Tables</asp:LinkButton></li>
                </ul>
            </nav>

            <article>
                
                <asp:UpdatePanel ID="UpdatePanelTable" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <%--            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SkyFi" style="margin-top: 0px" DataKeyNames="regNumber">
                <Columns>
                    <asp:BoundField DataField="regNumber" HeaderText="regNumber" SortExpression="regNumber" ReadOnly="True" />
                    <asp:BoundField DataField="flyHours" HeaderText="flyHours" SortExpression="flyHours" />
                    <asp:BoundField DataField="lastmaintenance" HeaderText="lastmaintenance" SortExpression="lastmaintenance" />
                    <asp:BoundField DataField="capacityMtonnes" HeaderText="capacityMtonnes" SortExpression="capacityMtonnes" />
                </Columns>
            </asp:GridView>
    <ul>
                <li><asp:LinkButton ID="Button_Demo1" OnClick="MidlandsFly_Demo" runat="server">Generate Demo</asp:LinkButton></li>
                <li><asp:LinkButton ID="Button_Reset1" OnClick="MidlandsFly_Reset" runat="server">Clear Table</asp:LinkButton></li>
        </ul>
                <asp:SqlDataSource ID="SkyFi" runat="server" ConnectionString="<%$ ConnectionStrings:ponyairlineConnectionString %>" SelectCommand="SELECT * FROM [cargo_aircraft]"></asp:SqlDataSource>--%>
                        <asp:GridView ID="GridViewTable" runat="server" AutoGenerateColumns="False" DataSourceID="MidlandsFly_Aircraft_Cargo" Style="margin-top: 0px" AllowPaging="True" AllowSorting="True" Width="687px">
                            <Columns>
                                <asp:BoundField DataField="Aircraft_Number" HeaderText="Aircraft_Number" SortExpression="Aircraft_Number" />
                                <asp:BoundField DataField="Flying_Hours" HeaderText="Flying_Hours" SortExpression="Flying_Hours" />
                                <asp:BoundField DataField="Last_Maintenance" HeaderText="Last_Maintenance" SortExpression="Last_Maintenance" />
                                <asp:BoundField DataField="Last_Maintenance_Date" HeaderText="Last_Maintenance_Date" SortExpression="Last_Maintenance_Date" />
                                <asp:BoundField DataField="Capacity_MetricTonnes" HeaderText="Capacity_MetricTonnes" SortExpression="Capacity_MetricTonnes" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="MidlandsFly_Aircraft_Cargo" runat="server" ConnectionString="<%$ ConnectionStrings:ponyairlineConnectionString %>" SelectCommand="SELECT * FROM [cargo_aircraft]"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="MidlandsFly_Aircraft_Passenger" runat="server" ConnectionString="<%$ ConnectionStrings:ponyairlineConnectionString %>" SelectCommand="SELECT * FROM [passenger_aircraft]"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="AirlineDatabase_Ed" runat="server" ConnectionString="<%$ ConnectionStrings:airlineDatabaseConnectionString_Ed %>" SelectCommand="SELECT * FROM [cargo_aircraft]" ProviderName="<%$ ConnectionStrings:airlineDatabaseConnectionString_Ed.ProviderName %>"></asp:SqlDataSource>
                                <br />
                        Simulate flight:<br />
                                <asp:Button ID="Button_AddHoursToPlane" runat="server" OnClick="AddHoursToPlane" Text="Add" />
                                <asp:TextBox ID="TextBox_AddHoursToPlane_Hours" runat="server" OnTextChanged="TextBox1_TextChanged" Width="40px"></asp:TextBox>
                        hours to plane
                        <asp:TextBox ID="TextBox_AddHoursToPlane_RegNumber" runat="server" Width="80px"></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </article>
        </form>
    </div>
</body>
</html>