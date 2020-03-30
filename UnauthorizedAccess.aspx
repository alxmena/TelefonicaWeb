<%@ Page Title="" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="UnauthorizedAccess.aspx.cs" Inherits="ATCPortal.UnauthorizedAccess" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="You have attempted to access a page that you are not authorized to view."  Font-Size="Larger" Theme="Mulberry" Width="100%"></dx:ASPxLabel>
        <div style="text-align: center">
            <br />
            Consult the System Administrator if you feel you have received this message in error
        </div>

</asp:Content>
