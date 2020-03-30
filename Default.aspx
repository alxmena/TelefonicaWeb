<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="Default.aspx.cs" Inherits="ATCPortal._Default" %>

<%@ Register Assembly="DevExpress.XtraCharts.v19.2.Web, Version=19.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>


<%@ Register assembly="DevExpress.XtraCharts.v19.2, Version=19.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="dx" %>


<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <dx:ASPxPanel ID="pnlImage" runat="server">
        <PanelCollection>
            <dx:PanelContent>
                <dx:ASPxImage ID="ASPxImage1" runat="server" ShowLoadingImage="true" ImageUrl="~/Images/SplashAN.jpg" Width="650px"></dx:ASPxImage>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>
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

<SettingsPopup>
<HeaderFilter MinHeight="140px"></HeaderFilter>
</SettingsPopup>
                    </GridViewProperties>
                </dx:ASPxGridLookup>
                <br />
                <asp:SqlDataSource ID="Branch" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT [ID], [Name] FROM [tblBranch]"></asp:SqlDataSource>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>
    <dx:ASPxPanel ID="pnlCharts" runat="server" Visible="false">
        <PanelCollection>
            <dx:PanelContent>
                <table>
                    <tr>
                        <td style="padding: 10px">
                            <dx:ASPxPanel ID="pnl2g" runat="server" Visible="false">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <h5 class="text-center">Cases 2G</h5>
                                        <dx:WebChartControl ID="WebChartControl2g" Width="700px" Height="300px" runat="server">
<Legend Name="Default Legend"></Legend>
                                        </dx:WebChartControl>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxPanel>
                        </td>
                        <td style="padding: 10px">
                            <dx:ASPxPanel ID="pnl3g" runat="server" Visible="false">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <h5 class="text-center">Cases 3G</h5>
                                        <dx:WebChartControl ID="WebChartControl3g" Width="700px" Height="300px" runat="server">
<Legend Name="Default Legend"></Legend>
                                        </dx:WebChartControl>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxPanel>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>
</asp:Content>