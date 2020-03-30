﻿<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ReportTicketView.aspx.cs" Inherits="ATCPortal.Reports.ReportTicketView" %>

<%@ Register Assembly="DevExpress.XtraReports.v19.2.Web.WebForms, Version=19.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function CloseBarch() {
            aglBranch.ConfirmCurrentSelection();
            aglBranch.HideDropDown();
            aglBranch.Focus();
        }
        function CloseTechnology() {
            aglTechnology.ConfirmCurrentSelection();
            aglTechnology.HideDropDown();
            aglTechnology.Focus();
        }
        function CloseStatus() {
            aglStatus.ConfirmCurrentSelection();
            aglStatus.HideDropDown();
            aglStatus.Focus();
        }
        function CloseOEM() {
            aglOEM.ConfirmCurrentSelection();
            aglOEM.HideDropDown();
            aglOEM.Focus();
        }
        function CloseSeverity() {
            aglSeverity.ConfirmCurrentSelection();
            aglSeverity.HideDropDown();
            aglSeverity.Focus();
        }
    </script>
    <dx:ASPxPanel ID="ASPxPanel2" runat="server" Width="100%">
        <PanelCollection>
            <dx:PanelContent>
                <div class="row" style="margin-bottom: 15px;">
                    <div class="col-md-6 " style="margin-bottom: 10px">
                        <dx:ASPxLabel runat="server" Text="Start Date:*" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
                        <dx:ASPxDateEdit ID="deDateInit" Width="100%" runat="server" EditFormat="Date">
                            <ValidationSettings>
                                <RequiredField ErrorText="Start Date is required."/>
                            </ValidationSettings>
                        </dx:ASPxDateEdit>
                    </div>
                    <div class="col-md-6 " style="margin-bottom: 10px">
                        <dx:ASPxLabel runat="server" Text="End Date:*" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
                        <dx:ASPxDateEdit ID="deDateFinish" Width="100%" runat="server" EditFormat="Date" >
                            <ValidationSettings>
                                <RequiredField ErrorText="End Date is required."/>
                            </ValidationSettings>
                        </dx:ASPxDateEdit>
                    </div>
                    <dx:ASPxPanel runat="server" ID="pnlBranch" Visible="true" CssClass="col-md-6" style="margin-bottom: 10px">
                        <PanelCollection>
                            <dx:PanelContent>
                                <div class="">
                                    <dx:ASPxLabel runat="server" Text="Select Branch:" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
                                    <dx:ASPxGridLookup Width="100%" AutoPostBack="true" ClientInstanceName="aglBranch" runat="server" ID="aglBranch" SelectionMode="Single" DataSourceID="Branch" KeyFieldName="ID" TextFormatString="{0}" OnTextChanged="aglBranch_TextChanged">
                                        <Columns>
                                            <dx:GridViewCommandColumn ShowSelectCheckbox="True" />
                                            <dx:GridViewDataColumn FieldName="Name" />
                                        </Columns>
                                        <gridviewstyles>
                                                <header font-size="Medium" Font-Bold="true" ForeColor="#336699">
                                                </header>
                                                <HeaderFilterItem Font-Bold="True" ForeColor="Navy">
                                                </HeaderFilterItem>
                                                <SearchPanel Font-Bold="True" ForeColor="Navy">
                                                </SearchPanel>               
                                                <AlternatingRow Enabled="true" />        
                                        </gridviewstyles>
                                        <GridViewProperties>
                                            <Templates>
                                                <StatusBar>
                                                    <table class="OptionsTable" style="float: right">
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseBarch" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </StatusBar>
                                            </Templates>
                                            <Settings ShowFilterRow="True" ShowStatusBar="Visible" />

            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True"></SettingsBehavior>

                                            <SettingsPager PageSize="7" EnableAdaptivity="true" />
                                        </GridViewProperties>
                                    </dx:ASPxGridLookup>
                                    <asp:SqlDataSource ID="Branch" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT [ID], [Name] FROM [tblBranch]"></asp:SqlDataSource>
                                </div>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxPanel>
                    
                    <div class="col-md-6" style="margin-bottom: 10px">
                        <dx:ASPxLabel runat="server" Text="Select Vendor:*" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
                        <dx:ASPxGridLookup Width="100%" runat="server" ID="aglOEM" ClientInstanceName="aglOEM" SelectionMode="Multiple" KeyFieldName="ID" TextFormatString="{0}" MultiTextSeparator="," DataSourceID="OEM">
                            <Columns>
                                <dx:GridViewCommandColumn ShowSelectCheckbox="True" ShowClearFilterButton="true" SelectAllCheckboxMode="Page" />
                                <dx:GridViewDataColumn FieldName="Name" />
                            </Columns>
                            <GridViewProperties>
                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseOEM" />
                                                </td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>
                                <Settings ShowFilterRow="True" ShowStatusBar="Visible" />

<SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True"></SettingsBehavior>

                                <SettingsPager PageSize="7" EnableAdaptivity="true" />
                            </GridViewProperties>
                        </dx:ASPxGridLookup>
                        <asp:SqlDataSource ID="OEM" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT DISTINCT t2.ID, t2.Name FROM tblOEMBranch t1 inner join tblOEM t2 ON t2.ID = t1.OEMID"></asp:SqlDataSource>
                    </div>
                    <div class="col-md-6" style="margin-bottom: 10px">
                        <dx:ASPxLabel runat="server" Text="Select Technology:" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
                        <dx:ASPxGridLookup Width="100%" runat="server" ID="aglTechnology" ClientInstanceName="aglTechnology" SelectionMode="Multiple" KeyFieldName="ID" TextFormatString="{0}" MultiTextSeparator="," DataSourceID="Technology">
                            <Columns>
                                <dx:GridViewCommandColumn ShowSelectCheckbox="True" ShowClearFilterButton="true" SelectAllCheckboxMode="Page" />
                                <dx:GridViewDataColumn FieldName="Name" />
                            </Columns>
                            <GridViewProperties>
                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseTechnology" />
                                                </td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>
                                <Settings ShowFilterRow="True" ShowStatusBar="Visible" />

<SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True"></SettingsBehavior>

                                <SettingsPager PageSize="7" EnableAdaptivity="true" />
                            </GridViewProperties>
                        </dx:ASPxGridLookup>
                        <asp:SqlDataSource ID="Technology" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT DISTINCT t2.ID, t2.Name FROM tblOEMBranch t1 inner join tblTechnology t2 ON t2.ID = t1.TechnologyID"></asp:SqlDataSource>
                    </div>
                    <div class="col-md-6" style="margin-bottom: 10px">
                        <dx:ASPxLabel runat="server" Text="Select Status:" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
                        <dx:ASPxGridLookup Width="100%" runat="server" ID="aglStatus" ClientInstanceName="aglStatus" SelectionMode="Multiple" DataSourceID="Status" KeyFieldName="ID" TextFormatString="{0}" MultiTextSeparator=",">
                            <Columns>
                                <dx:GridViewCommandColumn ShowSelectCheckbox="True" ShowClearFilterButton="true" SelectAllCheckboxMode="Page" />
                                <dx:GridViewDataColumn FieldName="Name" />
                            </Columns>
                            <GridViewProperties>
                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseStatus" />
                                                </td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>
                                <Settings ShowFilterRow="True" ShowStatusBar="Visible" />

<SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True"></SettingsBehavior>

                                <SettingsPager PageSize="7" EnableAdaptivity="true" />
                            </GridViewProperties>
                        </dx:ASPxGridLookup>
                        <asp:SqlDataSource ID="Status" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT [ID], [Name] FROM [tblStatus]"></asp:SqlDataSource>
                    </div>
                    <div class="col-md-6" style="margin-bottom: 10px">
                        <dx:ASPxLabel runat="server" Text="Select Severity:" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
                        <dx:ASPxGridLookup Width="100%" runat="server" ID="aglSeverity" ClientInstanceName="aglSeverity" SelectionMode="Multiple" DataSourceID="Severity" KeyFieldName="ID" TextFormatString="{0}" MultiTextSeparator=",">
                            <Columns>
                                <dx:GridViewCommandColumn ShowSelectCheckbox="True" ShowClearFilterButton="true" SelectAllCheckboxMode="Page" />
                                <dx:GridViewDataColumn FieldName="Name" />
                            </Columns>
                            <GridViewProperties>
                                <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseSeverity" />
                                                </td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                                </Templates>
                                <Settings ShowFilterRow="True" ShowStatusBar="Visible" />

<SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True"></SettingsBehavior>

                                <SettingsPager PageSize="7" EnableAdaptivity="true" />
                            </GridViewProperties>
                        </dx:ASPxGridLookup>
                        <asp:SqlDataSource ID="Severity" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT [ID], [Name] FROM [tblSeverity]"></asp:SqlDataSource>
                    </div>
                    <div class="col-md-6">
                        <dx:ASPxButton AutoPostBack="true" runat="server" Text="View Report" ID="btnViewReport" OnClick="btnViewReport_Click"></dx:ASPxButton>
                    </div>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>
    <dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%">
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxWebDocumentViewer Width="100%" ID="Ticketport" runat="server"></dx:ASPxWebDocumentViewer>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>
    <dx:ASPxPopupControl ID="popMsg" ClientInstanceName="popMsg" runat="server" CloseOnEscape="True" Modal="True" AllowDragging="True" PopupAnimationType="Fade" PopupVerticalAlign="WindowCenter" EnableViewState="False" CssClass="auto-style7" HeaderText="Q MESSAGE" PopupHorizontalAlign="WindowCenter" Width="500px">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxLabel ID ="lblMsg" runat="server" Text="ASPxLabel"></dx:ASPxLabel><br /><br />
                <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False" ClientInstanceName="btnCancel" Text="Ok" Width="80px">
                    <ClientSideEvents Click="function(s, e) {popMsg.Hide();}" />
                </dx:ASPxButton>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>  
</asp:Content>