<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ViewTickets.aspx.cs" Inherits="ATCPortal.FieldTechnician.ViewTickets" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

     <script type="text/javascript">
        function CloseGridLookup() {
            glSites.ConfirmCurrentSelection();
            glSites.HideDropDown();
            glSites.Focus();
        }
        function CloseglGroup() {
            glGroup.ConfirmCurrentSelection();
            glGroup.HideDropDown();
            glGroup.Focus();
         }
         function EndCallBackGv(s, e) {
             if (gvMain.cpMessage) {
                 alert(gvMain.cpMessage);
             }
         }

    </script>

   <dx:ASPxGridView ID="gvMain" ClientInstanceName="gvMain" runat="server" AutoGenerateColumns="False" DataSourceID="sqlMain" SettingsPopup-EditForm-Modal="false" KeyFieldName="ID" Settings-VerticalScrollableHeight="580"  Settings-HorizontalScrollBarMode="Hidden" Width="2900px" OnStartRowEditing="gvMain_StartRowEditing" OnRowValidating="gvMain_RowValidating" OnCustomCallback="gvMain_CustomCallback" OnRowUpdating="gvMain_RowUpdating" OnToolbarItemClick="gvMain_ToolbarItemClick" ForeColor="Navy">
        <ClientSideEvents BeginCallback="function(s, e) {gvMain.cpMessage='';}" EndCallback="EndCallBackGv"/> 
        <SettingsPager PageSize="10" />
        <Settings ShowGroupPanel="True" ShowFooter="True" ShowHeaderFilterButton="true"/>
        <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="WYSIWYG" FileName="Report" />
        <SettingsDataSecurity AllowDelete="False" AllowInsert="False" />
        <SettingsSearchPanel Visible="True" />
        <SettingsResizing ColumnResizeMode="Control" Visualization="Postponed"></SettingsResizing>
        <SettingsBehavior AllowEllipsisInText="true" />
        <SettingsEditing EditFormColumnCount="2" Mode="PopupEditForm" />
        <SettingsPopup EditForm-Modal="true">
            <EditForm Width="950px" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Modal="True"></EditForm>
        </SettingsPopup>
        <Toolbars>
            <dx:GridViewToolbar>
                <Items>
                    <dx:GridViewToolbarItem Command="ExportToXlsx" />
                    <dx:GridViewToolbarItem Command="ExportToCsv" />
                </Items>
                
            </dx:GridViewToolbar>
        </Toolbars>
        <Columns>
            <dx:GridViewDataTextColumn Caption="Ticket" FieldName="ID" EditFormSettings-Visible="False" VisibleIndex="0" Width="110">
                <EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataComboBoxColumn FieldName="BranchID" Caption="Branch" VisibleIndex="1">
                <EditFormSettings Visible="False" />
                <PropertiesComboBox DataSourceID="sqlBranch" TextField="Name" ValueField="ID"></PropertiesComboBox>
                <SettingsHeaderFilter Mode="CheckedList"/>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="OEMID" Caption="OEM" VisibleIndex="2">
                <PropertiesComboBox DataSourceID="sqlOEM" TextField="Name" ValueField="ID">
                    <ClientSideEvents SelectedIndexChanged="function(s, e) {gvMain.PerformCallback('1=' + s.GetValue());}" />
                </PropertiesComboBox>
                <SettingsHeaderFilter Mode="CheckedList"/>
             </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="TechnologyID" Caption="Technology" VisibleIndex="3">
                <PropertiesComboBox DataSourceID="sqlTech" TextField="Name" ValueField="ID">
                </PropertiesComboBox>
                <SettingsHeaderFilter Mode="CheckedList"/>
                <EditItemTemplate>
                    <dx:ASPxComboBox ID="cbTech" runat="server" ValueType="System.String" ValueField="ID" TextField="Name" AllowNull="true" NullText="not defined" Width="100%"></dx:ASPxComboBox>
                </EditItemTemplate>
             </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="SeverityID" Caption="Severity" VisibleIndex="4">
                <PropertiesComboBox DataSourceID="sqlSeverity" TextField="Name" ValueField="ID"></PropertiesComboBox>
                <SettingsHeaderFilter Mode="CheckedList"/>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="StatusID" ReadOnly="true" Caption="Status" VisibleIndex="5">
                <PropertiesComboBox>
                    <DropDownButton Enabled="false" />
                </PropertiesComboBox>
                <PropertiesComboBox DataSourceID="sqlStatus" TextField="Name" ValueField="ID"></PropertiesComboBox>
                <SettingsHeaderFilter Mode="CheckedList"/>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataDateColumn FieldName="CreationDate" ReadOnly="true" VisibleIndex="5" MinWidth="157">
                <PropertiesDateEdit EditFormat="DateTime" DisplayFormatString="g">
                    <DropDownButton Enabled="False"/>
                </PropertiesDateEdit>
                <SettingsHeaderFilter Mode="DateRangePicker">
                    <DateRangePeriodsSettings ShowFuturePeriods="False"/>
                </SettingsHeaderFilter>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn FieldName="ResponseDate" ReadOnly="true" VisibleIndex="6" MinWidth="157" >
                <PropertiesDateEdit EditFormat="DateTime" DisplayFormatString="g">
                    <DropDownButton Enabled="False"/>
                </PropertiesDateEdit>
                <SettingsHeaderFilter Mode="DateRangePicker">
                    <DateRangePeriodsSettings ShowFuturePeriods="False"/>
                </SettingsHeaderFilter>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn FieldName="RestorationDate" ReadOnly="true" VisibleIndex="7" MinWidth="157" >
                <PropertiesDateEdit EditFormat="DateTime" DisplayFormatString="g">
                    <DropDownButton Enabled="False"/>
                </PropertiesDateEdit>
                <SettingsHeaderFilter Mode="DateRangePicker">
                    <DateRangePeriodsSettings ShowFuturePeriods="False"/>
                </SettingsHeaderFilter>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn FieldName="ResolutionDate" ReadOnly="true" VisibleIndex="8" MinWidth="157" >
                <PropertiesDateEdit EditFormat="DateTime" DisplayFormatString="g">
                    <DropDownButton Enabled="False"/>
                </PropertiesDateEdit>
                <SettingsHeaderFilter Mode="DateRangePicker">
                    <DateRangePeriodsSettings ShowFuturePeriods="False"/>
                </SettingsHeaderFilter>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn FieldName="ClosureDate" ReadOnly="true" VisibleIndex="9" MinWidth="157" >
                <PropertiesDateEdit EditFormat="DateTime" DisplayFormatString="g">
                    <DropDownButton Enabled="False"/>
                </PropertiesDateEdit>
                <SettingsHeaderFilter Mode="DateRangePicker">
                    <DateRangePeriodsSettings ShowFuturePeriods="False"/>
                </SettingsHeaderFilter>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="UserName" ReadOnly="true" Caption="Ticket Creator" VisibleIndex="10">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataComboBoxColumn FieldName="NetworkElementID" Caption="Network Element" VisibleIndex="11" PropertiesComboBox-AllowNull="true" PropertiesComboBox-NullText="not defined">
                <PropertiesComboBox DataSourceID="sqlNetwork" TextField="Name" ValueField="ID"></PropertiesComboBox>
                <SettingsHeaderFilter Mode="CheckedList"/>
                 <EditItemTemplate>
                    <dx:ASPxComboBox ID="cbNetwork" runat="server" ValueType="System.String" ValueField="ID" TextField="Name" AllowNull="true" NullText="not defined" Width="100%" OnInit="cbNetwork_Init">
                         <ClientSideEvents SelectedIndexChanged="function(s, e) {gvMain.PerformCallback('3=' + s.GetValue());}" />
                    </dx:ASPxComboBox>
                </EditItemTemplate>
             </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="RadioControllerID" Caption="Controller" VisibleIndex="12" >
                <PropertiesComboBox DataSourceID="sqlRadio" TextFormatString="{0} - {1}" ValueField="ID">
                    <Columns>
                        <dx:ListBoxColumn FieldName="Code"/>
                        <dx:ListBoxColumn FieldName="Name" Width="300px"/>
                    </Columns>
                    <ClientSideEvents SelectedIndexChanged="function(s, e) {gvMain.PerformCallback('4=' + s.GetValue());}" />
                </PropertiesComboBox>
                <SettingsHeaderFilter Mode="CheckedList"/>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataTextColumn FieldName="ControllerIP" Caption="Controller IP Access" VisibleIndex="13">
                <EditItemTemplate>
                    <dx:ASPxTextBox ID="tbControllerIP" runat="server" Width="100%" OnInit="tbControllerIP_Init"></dx:ASPxTextBox>
                </EditItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Sites" ReadOnly="true" VisibleIndex="14">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="SiteIP" Caption="Site IP Access" VisibleIndex="15">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ProblemTitle" VisibleIndex="17" >
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataMemoColumn FieldName="ProblemDescription" VisibleIndex="18">
                <PropertiesMemoEdit Height="110">
                </PropertiesMemoEdit>
            </dx:GridViewDataMemoColumn>
            <dx:GridViewDataTextColumn FieldName="ResolutionNote" Caption="Solution" VisibleIndex="19">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataComboBoxColumn FieldName="SolutionTypeID" Caption="Type Of Solution" VisibleIndex="20" PropertiesComboBox-AllowNull="true" PropertiesComboBox-NullText="not defined">
                <PropertiesComboBox DataSourceID="sqlSolution" TextField="Name" ValueField="ID"></PropertiesComboBox>
                <SettingsHeaderFilter Mode="CheckedList"/>
                 <EditItemTemplate>
                    <dx:ASPxComboBox ID="cbSolution" runat="server" ValueType="System.String" ValueField="ID" TextField="Name" AllowNull="true" NullText="not defined" Width="100%">
                    </dx:ASPxComboBox>
                </EditItemTemplate>
             </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataTextColumn FieldName="SoftwareRelease" VisibleIndex="21">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ContactInstructions" VisibleIndex="22">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Remedy" VisibleIndex="22">
            </dx:GridViewDataTextColumn>
        </Columns>
       <Styles>
            <header font-size=14px Font-Bold="true" ForeColor="#336699">
            </header>
            <AdaptiveHeaderPanel Font-Bold="false" ForeColor="#336699">
            </AdaptiveHeaderPanel>
            <AlternatingRow Enabled="true" />  
        </Styles>
        <GroupSummary>
            <dx:ASPxSummaryItem FieldName="Days" SummaryType="Average" />
            <dx:ASPxSummaryItem FieldName="ReceivedDate" SummaryType="Count" />
        </GroupSummary>
        <TotalSummary>
            <dx:ASPxSummaryItem FieldName="ReceivedDate" SummaryType="Count" />
            <dx:ASPxSummaryItem FieldName="Days" SummaryType="Average" />
        </TotalSummary>
    </dx:ASPxGridView>
    <asp:SqlDataSource ID="sqlMain" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="
        SELECT ID, CreationDate, ResponseDate, RestorationDate, ResolutionDate, ClosureDate, UserName, SeverityID, OEMID, TechnologyID, NetworkElementID, RadioControllerID, StatusID, ProblemTitle, ProblemDescription,
        SoftwareRelease, FormalAnswerAccepted, ContactInstructions, Remedy, DATEDIFF(hour, CreationDateUtc, CreationDate) as TimeZone, BranchID, SiteIP, ControllerIP, Sites, ResolutionNote, SolutionTypeID
        FROM tblTicket">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlSeverity" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblSeverity WHERE CompanyID = @CompanyID">
        <SelectParameters>
            <asp:SessionParameter Name="CompanyID" SessionField="CompanyID" DbType="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlOEM" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>"
        SelectCommand="SELECT ID, Name FROM tblOEM"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlTech" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" 
        SelectCommand="SELECT ID, Name FROM tblTechnology">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlNetwork" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" 
        SelectCommand="SELECT ID, Name FROM tblNetworkElement">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlRadio" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name, Code FROM tblController"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlStatus" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblStatus"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlBranch" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblBranch"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlSolution" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblSolutionType"></asp:SqlDataSource>
    

</asp:Content>
