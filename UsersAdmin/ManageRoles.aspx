<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ManageRoles.aspx.cs" Inherits="ATCPortal.UsersAdmin.ManageRoles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <dx:ASPxGridView ID="grdRoles" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" KeyFieldName="RoleId" Theme="Moderno" EnableTheming="True" OnRowDeleting="grdRoles_RowDeleting" OnRowInserting="grdRoles_RowInserting" OnCellEditorInitialize="grdRoles_CellEditorInitialize">
        <Columns>
            <dx:GridViewCommandColumn ShowDeleteButton="True" ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0">
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="RoleName" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Description" VisibleIndex="4">
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsPager Visible="False">
        </SettingsPager>
    </dx:ASPxGridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT * FROM [vw_aspnet_Roles]"
        UpdateCommandType="StoredProcedure" UpdateCommand="aspnet_Roles_UpdateRole">
        <UpdateParameters>
            <asp:Parameter Name="RoleName" DbType="String" />
            <asp:Parameter Name="Description" DbType="String" />
            <asp:Parameter Name="RoleId" DbType="Guid" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>
</asp:Content>
