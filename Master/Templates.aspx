﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Templates.aspx.cs" Inherits="ATCPortal.Master.Templates" %><%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    Choose a Template to work on:<br />
    <br />
    <dx:ASPxRadioButtonList ID="rblTemplates" runat="server">
    </dx:ASPxRadioButtonList>
    <dx:ASPxButton ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" ClientInstanceName="btnLoad">
    </dx:ASPxButton>
    <br />
    <br />
    <dx:ASPxTextBox ID="txtFileName" runat="server" NullText="Enter HTML File Name" Width="170px" AutoPostBack="True" OnTextChanged="txtFileName_TextChanged">
    </dx:ASPxTextBox>
    <dx:ASPxButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Enabled="False">
    </dx:ASPxButton>
    <br />
    <br />
    <dx:ASPxHtmlEditor ID="htmEditor" runat="server" Height="800px" Theme="Mulberry" Width="100%">
        <SettingsDocumentSelector Enabled="True">
        </SettingsDocumentSelector>
    </dx:ASPxHtmlEditor>
    <br />
    <dx:ASPxPopupControl ID="popMsg" runat="server" CloseOnEscape="True" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowHeader="False" Theme="Mulberry" Width="326px">
        <ContentCollection>
<dx:PopupControlContentControl runat="server">
    <dx:ASPxLabel ID="lblMsg" runat="server" Text="ASPxLabel" Theme="Mulberry" Width="100%">
    </dx:ASPxLabel>
            </dx:PopupControlContentControl>
</ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
