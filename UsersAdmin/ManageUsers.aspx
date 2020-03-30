<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="ATCPortal.UsersAdmin.ManageUsers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <dx:ASPxGridView ID="grdUsers" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" KeyFieldName="UserId" Theme="Moderno" OnRowInserting="grdUsers_RowInserting" OnRowValidating="grdUsers_RowValidating" OnRowDeleting="grdUsers_RowDeleting" OnRowUpdating="grdUsers_RowUpdating" OnCellEditorInitialize="grdUsers_CellEditorInitialize">
        <Columns>
            <dx:GridViewCommandColumn ShowDeleteButton="True" ShowNewButtonInHeader="True" VisibleIndex="0" ShowEditButton="True">
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="UserId" VisibleIndex="1" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="UserName" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Email" VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn FieldName="IsApproved" VisibleIndex="4">
                <EditItemTemplate>
                    <dx:ASPxCheckBox ID="chkIsApproved" runat="server" OnLoad="chkIsApproved_Load" Theme="Mulberry">
                    </dx:ASPxCheckBox>
                </EditItemTemplate>
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="IsLockedOut" VisibleIndex="7">
                <EditItemTemplate>
                    <dx:ASPxCheckBox ID="chkIsLockedout" runat="server" Theme="Mulberry" OnLoad="chkIsLockedout_Load">
                    </dx:ASPxCheckBox>
                </EditItemTemplate>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataDateColumn FieldName="LastLoginDate" VisibleIndex="8">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn FieldName="LastLockoutDate" VisibleIndex="9">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="FailedPasswordAttemptCount" VisibleIndex="10">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="FailedPasswordAttemptWindowStart" VisibleIndex="11">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn Caption="Password" UnboundType="Integer" VisibleIndex="6" FieldName="Password" Visible="False">
                <EditItemTemplate>
                    <dx:ASPxTextBox ID="txtPWD1" Password="true" NullText="enter 6 or more characters..." runat="server" Width="330px" Height="21px" OnLoad="txtPWD1_Load" Theme="Mulberry" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>" OnValidation="txtPWD1_Validation">
                    </dx:ASPxTextBox>
                    <br />
                    <dx:ASPxTextBox ID="txtPWD2"  Password="true" NullText="Password Confirmation" runat="server" Width="330px" Height="21px" Theme="Mulberry">
                    </dx:ASPxTextBox>
                </EditItemTemplate>
                <PropertiesTextEdit Password="True" NullDisplayText="enter 6 or more characters...">
                </PropertiesTextEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" />
    </dx:ASPxGridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
        SelectCommand="SELECT aspnet_Users.UserId, aspnet_Users.UserName, aspnet_Membership.Email, aspnet_Membership.IsApproved, aspnet_Membership.IsLockedOut, aspnet_Membership.LastLoginDate, aspnet_Membership.LastLockoutDate, aspnet_Membership.FailedPasswordAttemptCount, aspnet_Membership.FailedPasswordAttemptWindowStart FROM aspnet_Users INNER JOIN aspnet_Membership ON aspnet_Users.UserId = aspnet_Membership.UserId"></asp:SqlDataSource>
</asp:Content>
