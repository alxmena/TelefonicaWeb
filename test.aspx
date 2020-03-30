<%@ Page Title="" Language="C#" MasterPageFile="~/Light.master" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="ATCPortal.test" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <dx:aspxradiobuttonlist ID="ASPxRadioButtonList1" runat="server">
        <items>
            <dx:listedititem Text="Yes" Value="1" />
            <dx:listedititem Text="No" Value="0" />
        </items>
        <clientsideevents SelectedIndexChanged="function(s, e) {
	var test = s.GetValue();
	if (test == '0')
	 {
	    textbox.SetEnabled(false);
	 }
	 else
	 {
	   textbox.SetEnabled(true);
	 }
}" />
    </dx:aspxradiobuttonlist>
    <dx:aspxtextbox ID="ASPxTextBox1" runat="server" ClientInstanceName="textbox" 
        Width="170px">
    </dx:aspxtextbox>
</asp:Content>
