<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="AssignRoles.aspx.cs" Inherits="ATCPortal.Master.AssignRoles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <dx:ASPxGridView ID="grdRoles" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" EnableTheming="True" Theme="Moderno" KeyFieldName="UserName" OnRowUpdating="grdRoles_RowUpdating" OnCustomColumnDisplayText="grdRoles_CustomColumnDisplayText">
        <Columns>
            <dx:GridViewCommandColumn ShowEditButton="True" VisibleIndex="0">
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="UserName" VisibleIndex="1" ReadOnly="True">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="RoleNames" VisibleIndex="2" Caption="Role Names" UnboundType="String">
                <EditItemTemplate>
                    <dx:ASPxCheckBoxList ID="chlRoles" runat="server" ValueType="System.String" OnLoad="chlRoles_Load">
                    </dx:ASPxCheckBoxList>
                </EditItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataComboBoxColumn FieldName="CompanyID" Caption="Company" VisibleIndex="11">
                <PropertiesComboBox DataSourceID="sqlCompany" TextField="Name" ValueField="ID">
                </PropertiesComboBox>
                <EditFormSettings Visible="False" />
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="BranchID" Caption="Branch" VisibleIndex="12">
                <PropertiesComboBox DataSourceID="sqlBranch" TextField="Name" ValueField="ID">
                </PropertiesComboBox>
                <EditFormSettings Visible="False" />
            </dx:GridViewDataComboBoxColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" />
        <SettingsDataSecurity AllowDelete="False" AllowInsert="False" />
    </dx:ASPxGridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT UserName, CompanyID, BranchID FROM aspnet_Users"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlCompany" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT [ID], [Name] FROM [tblCompany]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlBranch" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT [ID], [Name] FROM [tblBranch]"></asp:SqlDataSource>
</asp:Content>
