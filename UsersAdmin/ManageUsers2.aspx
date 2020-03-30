<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ManageUsers2.aspx.cs" Inherits="ATCPortal.UsersAdmin.ManageUsers2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <dx:ASPxGridView ID="grdUsers" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" KeyFieldName="UserId" Theme="Mulberry" OnRowInserting="grdUsers_RowInserting" OnRowValidating="grdUsers_RowValidating" OnRowDeleting="grdUsers_RowDeleting" OnRowUpdating="grdUsers_RowUpdating" OnCellEditorInitialize="grdUsers_CellEditorInitialize" Width="100%">
        <Columns>
            <dx:GridViewCommandColumn ShowDeleteButton="True" ShowNewButtonInHeader="True" VisibleIndex="0" ShowEditButton="True">
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="UserId" VisibleIndex="1" Visible="False" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="UserName" VisibleIndex="2" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Email" VisibleIndex="3" ShowInCustomizationForm="True">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn FieldName="IsApproved" VisibleIndex="4" ShowInCustomizationForm="True">
                <EditItemTemplate>
                    <dx:ASPxCheckBox ID="chkIsApproved" runat="server" OnLoad="chkIsApproved_Load" Theme="Mulberry">
                    </dx:ASPxCheckBox>
                </EditItemTemplate>
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="IsLockedOut" VisibleIndex="6" ShowInCustomizationForm="True">
                <EditItemTemplate>
                    <dx:ASPxCheckBox ID="chkIsLockedout" runat="server" Theme="Mulberry" OnLoad="chkIsLockedout_Load">
                    </dx:ASPxCheckBox>
                </EditItemTemplate>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataDateColumn FieldName="LastLoginDate" VisibleIndex="7" ShowInCustomizationForm="True">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn FieldName="LastLockoutDate" VisibleIndex="8" ShowInCustomizationForm="True">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="FailedPasswordAttemptCount" VisibleIndex="9" ShowInCustomizationForm="True">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="FailedPasswordAttemptWindowStart" VisibleIndex="10" ShowInCustomizationForm="True">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn Caption="Password" UnboundType="Integer" VisibleIndex="5" FieldName="Password" Visible="False" ShowInCustomizationForm="True">
                <EditItemTemplate>
                    <dx:ASPxTextBox ID="txtPWD1" Password="true" NullText="6 or more characters..." Width="320px" runat="server" OnLoad="txtPWD1_Load" Theme="Mulberry" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>" OnValidation="txtPWD1_Validation">
                    </dx:ASPxTextBox>
                    <br />
                    <dx:ASPxTextBox ID="txtPWD2"  Password="true" NullText="Password Confirmation" Width="320px" runat="server" Theme="Mulberry">
                    </dx:ASPxTextBox>
                </EditItemTemplate>
                <PropertiesTextEdit Password="True" NullDisplayText="enter 6 or more characters..."></PropertiesTextEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataComboBoxColumn FieldName="BranchID" Caption="Branch" VisibleIndex="11">
                <PropertiesComboBox DataSourceID="sqlBranch" TextField="Name" ValueField="ID">
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" />
    </dx:ASPxGridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT aspnet_Users.UserId, aspnet_Users.UserName, aspnet_Users.BranchID, aspnet_Membership.Email, aspnet_Membership.IsApproved, aspnet_Membership.IsLockedOut, aspnet_Membership.LastLoginDate, aspnet_Membership.LastLockoutDate, aspnet_Membership.FailedPasswordAttemptCount, aspnet_Membership.FailedPasswordAttemptWindowStart
        FROM aspnet_Users INNER JOIN aspnet_Membership ON aspnet_Users.UserId = aspnet_Membership.UserId WHERE CompanyID = @CompanyID">
        <SelectParameters>
            <asp:SessionParameter SessionField="CompanyID" Name="CompanyID" Type="Int32"/>
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlBranch" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT [ID], [Name] FROM [tblBranch] WHERE CompanyID = @CompanyID">
    <SelectParameters>
        <asp:SessionParameter Name="CompanyID" SessionField="CompanyID" DbType="Int32" />
    </SelectParameters>
    </asp:SqlDataSource>



</asp:Content>
