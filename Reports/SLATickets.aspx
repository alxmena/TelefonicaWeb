<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="SLATickets.aspx.cs" Inherits="ATCPortal.Reports.SLATickets" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <asp:Label runat="server" ID="labelTest" Text="Hola"></asp:Label>     
    <dx:ASPxGridView runat="server" ClientInstanceName="gvMain" ID="gvMain" AutoGenerateColumns="false" SettingsPopup-EditForm-Modal="false" KeyFieldName="ID" Settings-VerticalScrollableHeight="580"  Settings-HorizontalScrollBarMode="Hidden" Width="2500" OnHtmlDataCellPrepared="gvMain_HtmlDataCellPrepared" OnToolbarItemClick="gvMain_ToolbarItemClick" ForeColor="Navy" OnHtmlFooterCellPrepared="gvMain_HtmlFooterCellPrepared" OnHtmlRowCreated="gvMain_HtmlRowCreated" OnHtmlRowPrepared="gvMain_HtmlRowPrepared">

<SettingsPopup>
<HeaderFilter MinHeight="140px"></HeaderFilter>
</SettingsPopup>

        <SettingsSearchPanel Visible="True" />
        <Settings ShowGroupPanel="True" ShowFooter="True" ShowHeaderFilterButton="true"/>
        <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="WYSIWYG" FileName="Report" />
        <SettingsBehavior AllowEllipsisInText="true" />
        <Toolbars>
            <dx:GridViewToolbar>
                <Items>
                    <dx:GridViewToolbarItem Command="ExportToXlsx" />
                    <dx:GridViewToolbarItem Command="ExportToCsv" />
                </Items>
                
            </dx:GridViewToolbar>
        </Toolbars>
        <Columns>
            <dx:GridViewDataTextColumn Caption="Ticket" FieldName="ID" EditFormSettings-Visible="False" VisibleIndex="0" Width=90px>
<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataComboBoxColumn FieldName="SeverityID" Caption="Severity" VisibleIndex="1" Width=90px>
                <PropertiesComboBox DataSourceID="sqlSeverity" TextField="Name" ValueField="ID"></PropertiesComboBox>
                <SettingsHeaderFilter Mode="CheckedList"/>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="BranchID" Caption="Branch" VisibleIndex="1" Width=90px>
                <EditFormSettings Visible="False" />
                <PropertiesComboBox DataSourceID="sqlBranch" TextField="Name" ValueField="ID"></PropertiesComboBox>
                <SettingsHeaderFilter Mode="CheckedList"/>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataDateColumn FieldName="CreationDate" ReadOnly="true" VisibleIndex="2" MinWidth="157">
                <PropertiesDateEdit EditFormat="DateTime" DisplayFormatString="g">
                    <DropDownButton Enabled="False"/>
                </PropertiesDateEdit>
                <SettingsHeaderFilter Mode="DateRangePicker">
                    <DateRangePeriodsSettings ShowFuturePeriods="False"/>
                </SettingsHeaderFilter>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="UserName" ReadOnly="true" Caption="Ticket Creator" VisibleIndex="3" Width=115px></dx:GridViewDataTextColumn>
            <dx:GridViewDataComboBoxColumn FieldName="OEMID" Caption="OEM" VisibleIndex="4" Width=90px>
                <PropertiesComboBox DataSourceID="sqlOEM" TextField="Name" ValueField="ID" >
                    <ClientSideEvents SelectedIndexChanged="function(s, e) {gvMain.PerformCallback('1=' + s.GetValue());}" />
                </PropertiesComboBox>
                <SettingsHeaderFilter Mode="CheckedList"/>
             </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="TechnologyID" Caption="Technology" VisibleIndex="5" Width=80px>
                <PropertiesComboBox DataSourceID="sqlTech" TextField="Name" ValueField="ID">
                </PropertiesComboBox>
                <SettingsHeaderFilter Mode="CheckedList"/>
                <EditItemTemplate>
                    <dx:ASPxComboBox ID="cbTech" runat="server" ValueType="System.String" ValueField="ID" TextField="Name" AllowNull="true" NullText="not defined" Width="100%"></dx:ASPxComboBox>
                </EditItemTemplate>
             </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="NetworkElementID" Caption="Network Element" VisibleIndex="6" PropertiesComboBox-AllowNull="true" PropertiesComboBox-NullText="not defined" Width=100px>
                <PropertiesComboBox DataSourceID="sqlNetwork" TextField="Name" ValueField="ID"></PropertiesComboBox>
                <SettingsHeaderFilter Mode="CheckedList"/>
                 <EditItemTemplate>
                    <dx:ASPxComboBox ID="cbNetwork" runat="server" ValueType="System.String" ValueField="ID" TextField="Name" AllowNull="true" NullText="not defined" Width="100%"></dx:ASPxComboBox>
                </EditItemTemplate>
             </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataTextColumn FieldName="ProblemTitle" VisibleIndex="7" ></dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="ResponseDate" ReadOnly="true" VisibleIndex="8" MinWidth="157" >
                <PropertiesDateEdit EditFormat="DateTime" DisplayFormatString="g">
                    <DropDownButton Enabled="False"/>
                </PropertiesDateEdit>
                <SettingsHeaderFilter Mode="DateRangePicker">
                    <DateRangePeriodsSettings ShowFuturePeriods="False"/>
                </SettingsHeaderFilter>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn Caption="Time Response" FieldName="TimeResponse" EditFormSettings-Visible="False" VisibleIndex="9" Width="110">
<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Fulfillment SLA Response" FieldName="SlaResponse" EditFormSettings-Visible="False" VisibleIndex="10" Width="110">
<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="RestorationDate" ReadOnly="true" VisibleIndex="11" MinWidth="157" >
                <PropertiesDateEdit EditFormat="DateTime" DisplayFormatString="g">
                    <DropDownButton Enabled="False"/>
                </PropertiesDateEdit>
                <SettingsHeaderFilter Mode="DateRangePicker">
                    <DateRangePeriodsSettings ShowFuturePeriods="False"/>
                </SettingsHeaderFilter>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn Caption="Time Restoration" FieldName="TimeRestoration" EditFormSettings-Visible="False" VisibleIndex="12" Width="110">
<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Fulfillment SLA Restoration" FieldName="SlaRestoration" EditFormSettings-Visible="False" VisibleIndex="13" Width="110">
<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Total time Telefónica" FieldName="TotalPendingInfoRestoration" EditFormSettings-Visible="False" VisibleIndex="14" Width="110">
<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Total time Amerinode" FieldName="TotalAmerinodeRestoration" EditFormSettings-Visible="False" VisibleIndex="15" Width="110">
<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="ResolutionDate" ReadOnly="true" VisibleIndex="16" MinWidth="157" >
                <PropertiesDateEdit EditFormat="DateTime" DisplayFormatString="g">
                    <DropDownButton Enabled="False"/>
                </PropertiesDateEdit>
                <SettingsHeaderFilter Mode="DateRangePicker">
                    <DateRangePeriodsSettings ShowFuturePeriods="False"/>
                </SettingsHeaderFilter>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn Caption="TimeResolution" FieldName="TimeResolution" EditFormSettings-Visible="False" VisibleIndex="17" Width="110">
<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Fulfillment SLA Resolution" FieldName="SlaResolution" EditFormSettings-Visible="False" VisibleIndex="18" Width="110">
<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Total time Telefónica" FieldName="TotalPendingInfoResolution" EditFormSettings-Visible="False" VisibleIndex="19" Width="110">
<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Total time Amerinode" FieldName="TotalAmerinodeRestoration" EditFormSettings-Visible="False" VisibleIndex="20" Width="110">
<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="ClosureDate" ReadOnly="true" VisibleIndex="21" MinWidth="157" >
                <PropertiesDateEdit EditFormat="DateTime" DisplayFormatString="g">
                    <DropDownButton Enabled="False"/>
                </PropertiesDateEdit>
                <SettingsHeaderFilter Mode="DateRangePicker">
                    <DateRangePeriodsSettings ShowFuturePeriods="False"/>
                </SettingsHeaderFilter>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataComboBoxColumn FieldName="SolutionTypeID" Caption="Type Of Solution" VisibleIndex="22" PropertiesComboBox-AllowNull="true" PropertiesComboBox-NullText="not defined">
                <PropertiesComboBox DataSourceID="sqlSolution" TextField="Name" ValueField="ID"></PropertiesComboBox>
                <SettingsHeaderFilter Mode="CheckedList"/>
                 <EditItemTemplate>
                    <dx:ASPxComboBox ID="cbSolution" runat="server" ValueType="System.String" ValueField="ID" TextField="Name" AllowNull="true" NullText="not defined" Width="100%">
                    </dx:ASPxComboBox>
                </EditItemTemplate>
             </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="StatusID" ReadOnly="false" Caption="Status" EditFormSettings-Visible="False" VisibleIndex="23" Width=90px>
                <PropertiesComboBox DataSourceID="sqlStatus" TextField="Name" ValueField="ID"></PropertiesComboBox>
                <PropertiesComboBox>
                    <DropDownButton Enabled="false" />
                </PropertiesComboBox>
                <SettingsHeaderFilter Mode="CheckedList"/>
<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataTextColumn Caption="Advances" FieldName="Advances" EditFormSettings-Visible="False" VisibleIndex="24" Width="110">
<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsResizing  ColumnResizeMode="Control"  Visualization="Live" />        
        <Styles>
            <header font-size=14px Font-Bold="true" ForeColor="#336699">
            </header>
            <AdaptiveHeaderPanel Font-Bold="false" ForeColor="#336699">
            </AdaptiveHeaderPanel>
            <AlternatingRow Enabled="true" />  
        </Styles>
    </dx:ASPxGridView>
    <asp:SqlDataSource ID="sqlBranch" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblBranch"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlSeverity" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblSeverity">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlStatus" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblStatus"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlSolution" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblSolutionType"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlNetwork" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" 
        SelectCommand="SELECT ID, Name FROM tblNetworkElement">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlOEM" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>"
        SelectCommand="SELECT ID, Name FROM tblOEM"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlTech" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" 
        SelectCommand="SELECT ID, Name FROM tblTechnology">
    </asp:SqlDataSource>
    
</asp:Content>
