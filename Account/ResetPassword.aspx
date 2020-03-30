<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Light.master" CodeBehind="ResetPassword.aspx.cs" Inherits="ATCPortal.ResetPassword" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountHeader">
        <h2>Reset Password</h2>
        <p>
            Use the form below to reset your password.</p>
        <p>
            You must enter a correct combination of user name and email address to reset your password. A new password will be emailed to you.</p>
    </div>
    <br />
    <dx:ASPxLabel ID="lblCurrentPassword" runat="server" Text="User Name:" />
    <div class="form-field">
        <dx:ASPxTextBox ID="txtUserName" runat="server" Width="200px">
            <ValidationSettings ValidationGroup="ResetPwd">
                <RequiredField ErrorText="User Name is required." IsRequired="true" />
            </ValidationSettings>
        </dx:ASPxTextBox>
    </div>
    <dx:ASPxLabel ID="lblPassword" runat="server" AssociatedControlID="tbPassword" Text="Email:" />
    <div class="form-field">
        <dx:ASPxTextBox ID="txtEmail" ClientInstanceName="Password" runat="server"
        Width="200px">
            <ValidationSettings ValidationGroup="ResetPwd">
                <RequiredField ErrorText="Email is required." IsRequired="true" />
            </ValidationSettings>
        </dx:ASPxTextBox>
    </div>
    <dx:ASPxButton ID="btnResetPassword" runat="server" Text="Reset Password" ValidationGroup="ResetPwd"
    OnClick="btnResetPassword_Click">
    </dx:ASPxButton>
&nbsp;
    <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click">
    </dx:ASPxButton>
    <dx:ASPxPopupControl ID="popMsg" runat="server" CloseOnEscape="True" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowHeader="False" Theme="Mulberry" Width="326px">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
               <dx:ASPxLabel ID="lblMsg" runat="server" Text="ASPxLabel" Theme="Mulberry" Width="100%"></dx:ASPxLabel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>