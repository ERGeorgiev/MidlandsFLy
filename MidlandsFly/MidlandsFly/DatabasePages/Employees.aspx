<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Employees.aspx.cs" Inherits="Homepage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <header>
            <h1>EMPLOYEES</h1>
        </header>

        <article>
            <center>
            <asp:UpdatePanel ID="UpdatePanelTable" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="Button_FlightDeck" runat="server" OnClick="ChangeGrid_FlightDeck" Text="Flight Deck" />
                    <asp:Button ID="Button_CabinCrew" runat="server" OnClick="ChangeGrid_CabinCrew" Text="Cabin Crew" />
                    <asp:Button ID="Button_GroundCrew" runat="server" OnClick="ChangeGrid_GroundCrew" Text="Ground Crew" />

                    <asp:GridView ID="GridViewTable" runat="server" Style="margin-top: 1px" AllowPaging="True" AllowSorting="True" Width="687px" OnPageIndexChanging="ChangeGrid" CellPadding="4" ForeColor="#333333" GridLines="None" OnSorting="ChangeGrid" PageSize="25">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="MidlandsFly_Employees" runat="server" ConnectionString="<%$ ConnectionStrings:midlandsFlyConnectionString %>" SelectCommand="SELECT * FROM [employees]" SelectCommandType="Text"></asp:SqlDataSource>
                </ContentTemplate>
            </asp:UpdatePanel>
            </center>
        </article>
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