<%@ Page Title="Allocation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Allocation.aspx.cs" Inherits="Homepage"%>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">

        <article>
            <asp:UpdatePanel ID="UpdatePanelTable" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    
                    Plane Registration Number:
                    <asp:TextBox ID="TextBox_RegNumber" runat="server" Width="74px"></asp:TextBox>
                    &nbsp;<asp:Button ID="Button_Filter" runat="server" OnClick="Filter" Text="Filter" />
                    &nbsp;<asp:Button ID="Button_FilterReset" runat="server" OnClick="FilterReset" Text="Reset" />
                    <br />
                    <asp:RegularExpressionValidator CssClass="text-danger" Display="Dynamic" ID="RegularExpressionValidator1" runat="server" ErrorMessage="Only 3 numbers and 3 letters are allowed. (Ex. AAA111)" ValidationExpression="^[a-zA-Z]{3}\d{3}$" ControlToValidate="TextBox_RegNumber"></asp:RegularExpressionValidator>                    
                    <br />
                    <asp:GridView ID="GridViewTable" runat="server" Style="margin-top: 0px" OnRowDataBound="GridView_Align" ItemStyle-HorizontalAlign="Left" HorizontalAlign="Center" AllowPaging="True" AllowSorting="True" Width="687px" CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="ChangeGrid" OnSorting="ChangeGrid" PageSize="25" EnableModelValidation="False">
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
                        <HeaderStyle CssClass="colHeader-LeftAlign" />
                    </asp:GridView>

                    <asp:SqlDataSource ID="MidlandsFly_History" runat="server" ConnectionString="<%$ ConnectionStrings:midlandsFlyConnectionString %>" SelectCommandType="Text"></asp:SqlDataSource>
                </ContentTemplate>
            </asp:UpdatePanel>
        </article>
    </div>
</asp:Content>