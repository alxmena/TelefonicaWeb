<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="TicketsPackage.aspx.cs" Inherits="ATCPortal.Financial.TicketsPackage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .g1 .dxgvDataRow_Moderno td {  
            border-bottom: 1px solid #d1d1d1 !important;  
        }  
    </style>
    <dx:ASPxPanel ID="pnlBranch" runat="server" Visible="false">
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxLabel runat="server" Text="Select Branch:" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
                <dx:ASPxGridLookup Width="20%" AutoPostBack="true" ClientInstanceName="aglBranch" runat="server" ID="aglBranch" SelectionMode="Single" DataSourceID="Branch" KeyFieldName="ID" TextFormatString="{0}" OnTextChanged="aglBranch_TextChanged">
                    <Columns>
                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" />
                        <dx:GridViewDataColumn FieldName="Name" />
                    </Columns>
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
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>
    <br />
    <br />
    <dx:ASPxLabel runat="server" Text="Contract Tracking Amerinode" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
    <br />
    <br />
    <dx:ASPxGridView ID="Grid2g" runat="server" Width="100%" AutoGenerateColumns="False" OnHtmlRowPrepared="Grid2g_HtmlRowPrepared" OnHtmlDataCellPrepared="Grid2g_HtmlDataCellPrepared" CssClass="g1">
        <Columns>
            <dx:GridViewDataTextColumn  FieldName="Month" CellStyle-Font-Bold="true" Width="400px" HeaderStyle-Font-Bold="true"/>
            <dx:GridViewBandColumn Caption="2G" ExportCellStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true">
                <Columns>
                    <dx:GridViewDataTextColumn  FieldName="P1P22g" Caption="P1 y P2" CellStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center" Width="100px" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center"/>
                    <dx:GridViewDataTextColumn  FieldName="P3P42g" Caption="P3 y P4" CellStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center" Width="100px" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center"/>
                </Columns>
                <HeaderStyle HorizontalAlign="Center" />
            </dx:GridViewBandColumn>
            <dx:GridViewBandColumn Name="Brand3G" Caption="3G" ExportCellStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true">
                <Columns>
                    <dx:GridViewDataTextColumn  FieldName="P1P23g" Caption="P1 y P2" CellStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center" Width="100px" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center"/>
                    <dx:GridViewDataTextColumn  FieldName="P3P43g" Caption="P3 y P4" CellStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center" Width="100px" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center"/>
                </Columns>
                <HeaderStyle HorizontalAlign="Center" />
            </dx:GridViewBandColumn>
        </Columns>
        <Settings GridLines="Vertical"/>
    </dx:ASPxGridView>
</asp:Content>
