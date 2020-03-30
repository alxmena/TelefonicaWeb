<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Light.master" CodeBehind="Login.aspx.cs" Inherits="ATCPortal.Login" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <dx:ASPxRoundPanel ID="Panel1" runat="server" ShowCollapseButton="true" Width="200px" HeaderText="Log in">
        <PanelCollection>
            <dx:PanelContent>
                <div>
    <p>
        Please enter your username and password. 
        <a href="Register.aspx">Register</a> if you don't have an account.
    </p>
</div>
<dx:ASPxLabel ID="lblUserName" runat="server" AssociatedControlID="tbUserName" Text="User Name:" />
<div class="form-field">
    <dx:ASPxTextBox ID="tbUserName" runat="server" Width="200px">
        <ValidationSettings ValidationGroup="LoginUserValidationGroup">
            <RequiredField ErrorText="User Name is required." IsRequired="true" />
        </ValidationSettings>
        <ClientSideEvents Init="function(s, e) {s.Focus();}" />
    </dx:ASPxTextBox>
</div>
<dx:ASPxLabel ID="lblPassword" runat="server" AssociatedControlID="tbPassword" Text="Password:" />
<div class="form-field">
    <dx:ASPxTextBox ID="tbPassword" runat="server" Password="true" Width="200px">
        <ValidationSettings ValidationGroup="LoginUserValidationGroup">
            <RequiredField ErrorText="Password is required." IsRequired="true" />
        </ValidationSettings>
    </dx:ASPxTextBox>
</div>
<dx:ASPxButton ID="btnLogin" runat="server" Text="Log In" ValidationGroup="LoginUserValidationGroup"
    OnClick="btnLogin_Click">
</dx:ASPxButton>
&nbsp;<dx:ASPxHyperLink ID="hypReset" runat="server" EnableViewState="False" NavigateUrl="~/Account/ResetPassword.aspx" Text="Reset your Password" Theme="Mulberry" Visible="False" />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>


</asp:Content>