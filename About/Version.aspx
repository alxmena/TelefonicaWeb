<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Version.aspx.cs" Inherits="ATCPortal.About.Version" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="935px" ShowHeader="False" Theme="Mulberry">
        <ContentPaddings Padding="100px" />
        <Content BackColor="#99CCFF">
        </Content>
        <PanelCollection>
            <dx:PanelContent runat="server">
                <span class="auto-style1">Q - TECHNICAL ASSISTANCE PORTAL (TAP)</span><br />
                <br />
                <span class="auto-style1">Amerinode, LLC<br /> </span><span class="auto-style2">all rights reserved&nbsp;
                <dx:ASPxImage ID="ASPxImage1" runat="server" ImageUrl="~/Images/All_rights_reserved_logo.svg.png" ShowLoadingImage="True" Width="20px">
                </dx:ASPxImage>
                </span><span class="auto-style1">
                <br />
                <br />
                VERSION </span>
                <dx:ASPxLabel ID="lblVersion" runat="server" Font-Size="X-Large" ForeColor="Red" Text="ASPxLabel">
                </dx:ASPxLabel>
                <br />
                <br />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
</asp:Content>
