<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="AssignRoles.aspx.cs" Inherits="ATCPortal.UsersAdmin.AssignRoles" %>
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
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" />
        <SettingsDataSecurity AllowDelete="False" AllowInsert="False" />
    </dx:ASPxGridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT UserName FROM aspnet_Users GROUP BY UserName"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>
</asp:Content>
