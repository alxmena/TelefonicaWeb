<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Light.master" CodeBehind="ChangePassword.aspx.cs" Inherits="ATCPortal.ChangePassword" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountHeader">
        <h2>Change Password</h2>
        <p>
            Use the form below to change your password.</p>
        <p>
            New passwords are required to be a minimum of <%= Membership.MinRequiredPasswordLength %> characters in length.</p>
    </div>
    <br />
    <dx:ASPxLabel ID="lblUser" runat="server" Text="User Name:" />
    <div class="form-field">
        <dx:ASPxTextBox ID="txtUser" runat="server" Width="200px">
            <ValidationSettings ValidationGroup="ChangeUserPasswordValidationGroup">
                <RequiredField ErrorText="User Name is required." IsRequired="true" />
            </ValidationSettings>
        </dx:ASPxTextBox>
    </div>
    <dx:ASPxLabel ID="lblCurrentPassword" runat="server" Text="Old Password:" />
    <div class="form-field">
        <dx:ASPxTextBox ID="tbCurrentPassword" runat="server" Password="true" Width="200px">
            <ValidationSettings ValidationGroup="ChangeUserPasswordValidationGroup">
                <RequiredField ErrorText="Old Password is required." IsRequired="true" />
            </ValidationSettings>
        </dx:ASPxTextBox>
    </div>
    <dx:ASPxLabel ID="lblPassword" runat="server" AssociatedControlID="tbPassword" Text="Password:" />
    <div class="form-field">
        <dx:ASPxTextBox ID="tbPassword" ClientInstanceName="Password" Password="true" runat="server"
        Width="200px" NullText="Enter New Password">
            <ValidationSettings ValidationGroup="ChangeUserPasswordValidationGroup">
                <RequiredField ErrorText="Password is required." IsRequired="true" />
            </ValidationSettings>
        </dx:ASPxTextBox>
    </div>
    <dx:ASPxLabel ID="lblConfirmPassword" runat="server" AssociatedControlID="tbConfirmPassword"
    Text="Password:" />
    <div class="form-field">
        <dx:ASPxTextBox ID="tbConfirmPassword" Password="true" runat="server" Width="200px" NullText="Confirm New Password">
            <ValidationSettings ValidationGroup="ChangeUserPasswordValidationGroup">
                <RequiredField ErrorText="Confirm Password is required." IsRequired="true" />
            </ValidationSettings>
            <ClientSideEvents Validation="function(s, e) {
            var originalPasswd = Password.GetText();
            var currentPasswd = s.GetText();
            e.isValid = (originalPasswd  == currentPasswd );
            e.errorText = 'The Password and Confirmation Password must match.';
        }" />
        </dx:ASPxTextBox>
    </div>
    <dx:ASPxButton ID="btnChangePassword" runat="server" Text="Change Password" ValidationGroup="ChangeUserPasswordValidationGroup"
    OnClick="btnChangePassword_Click">
    </dx:ASPxButton>
    <dx:ASPxPopupControl ID="popMsg" runat="server" CloseOnEscape="True" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowHeader="False" Theme="Mulberry" Width="326px">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
               <dx:ASPxLabel ID="lblMsg" runat="server" Text="ASPxLabel" Theme="Mulberry" Width="100%"></dx:ASPxLabel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

</asp:Content>