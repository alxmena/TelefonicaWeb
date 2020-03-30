<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="AssignRoles2.aspx.cs" Inherits="ATCPortal.UsersAdmin.AssignRoles2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <dx:ASPxGridView ID="grdRoles" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" EnableTheming="True" Theme="Mulberry" KeyFieldName="UserName" OnRowUpdating="grdRoles_RowUpdating" OnCustomColumnDisplayText="grdRoles_CustomColumnDisplayText">
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
        SelectCommand="SELECT UserName, CompanyID FROM aspnet_Users WHERE CompanyID = @CompanyID">
        <SelectParameters>
            <asp:SessionParameter SessionField="CompanyID" Name="CompanyID" Type="Int32"/>
        </SelectParameters>
    </asp:SqlDataSource>
    
</asp:Content>
