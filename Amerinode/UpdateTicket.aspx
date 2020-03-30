<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="UpdateTicket.aspx.cs" Inherits="ATCPortal.Amerinode.UpdateTicket" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <script type="text/javascript">
        function initDate(s, e) {
            s.SetDate(new Date());
        }
        
        function scrollTo()
        {
            return;
        }
    </script><asp:ScriptManager ID="Manage1" runat="server"></asp:ScriptManager>   
    Choose Ticket #:
    <dx:ASPxGridLookup font-zise="Large" ID ="gvTickets" runat="server" ClientInstanceName="gvTickets" AutoGenerateColumns="False" KeyFieldName="ID" TextFormatString="{0}" OnValueChanged="gvTickets_TextChanged" OnInit="gvTickets_Init" ForeColor="Navy" OnTextChanged="gvTickets_TextChanged1">
        <GridViewProperties>
        <SettingsBehavior  AllowSelectSingleRowOnly="True" AllowSelectByRowClick="True"></SettingsBehavior>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="True" />

<SettingsPopup>
<HeaderFilter MinHeight="140px"></HeaderFilter>
</SettingsPopup>
        </GridViewProperties>
        <Columns>
            <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="0">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="CreationDate" VisibleIndex="1">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="ProblemTitle" VisibleIndex="6">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataComboBoxColumn Caption="Severity" FieldName="SeverityID" VisibleIndex="2">
                <PropertiesComboBox TextField="Name" ValueField="ID" DataSourceID="sqlSeverity">
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="OEMID" VisibleIndex="3">
                <PropertiesComboBox DataSourceID="sqlOEM" TextField="Name" ValueField="ID">
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="TechnologyID" VisibleIndex="4">
                <PropertiesComboBox DataSourceID="sqlTechnology" TextField="Name" ValueField="ID">
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="NetworkElementID" VisibleIndex="5">
                <PropertiesComboBox DataSourceID="sqlNetwork" TextField="Name" ValueField="ID">
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn Caption="Status" FieldName="StatusID" VisibleIndex="7">
                <PropertiesComboBox DataSourceID="sqlStatus" TextField="Name" ValueField="ID">
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn Caption="Branch" FieldName="BranchID" VisibleIndex="8">
                <PropertiesComboBox DataSourceID="sqlBranch" TextField="Name" ValueField="ID">
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
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
        <DropDownWindowStyle Font-Size="Large" Font-Bold="true">
        </DropDownWindowStyle>
        <GridViewStyles AdaptiveDetailButtonWidth="28">
        </GridViewStyles>
    </dx:ASPxGridLookup>
    <br />
    <asp:Label runat="server" id="Status_tck" Text="" Font-Size="Large"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label runat="server" id="Prev_Status" Text="" Font-Size="Large"></asp:Label>
    <br />
    <asp:SqlDataSource ID="sqlSeverity" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name, CompanyID FROM tblSeverity"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlOEM" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblOEM"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlTechnology" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblTechnology"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlNetwork" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblNetworkElement"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlController" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblController"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlStatus" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblStatus"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlBranch" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblBranch"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SolutionTypeSql" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT * FROM [tblSolutionType]"></asp:SqlDataSource>
    <br />
    <dx:ASPxButton ID="btEdit" ClientInstanceName="btEdit" runat="server" Text="View and Edit Ticket" Width="90px" OnClick="ASPxButton1_Click"></dx:ASPxButton>
    <br />
    <%--<br />
    <dx:ASPxRoundPanel ID="Panel0" runat="server" ShowCollapseButton="true" HeaderText="Edit Pending Info & Status History" AllowCollapsingByHeaderClick="true" Width="100%" Visible="false">
        <PanelCollection>
            <dx:PanelContent runat="server">
                
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>--%>
    <br />
    <dx:ASPxRoundPanel ID="Panel1" Visible="false" runat="server" HeaderText="Ticket Information" ShowCollapseButton="true" AllowCollapsingByHeaderClick="true" Width="100%">
        <PanelCollection>
        <dx:PanelContent runat="server">
            <dx:ASPxFormLayout ID="flDetails" Width="100%" runat="server" DataSourceID="sqlDetails" ColCount="2" Enabled="false">
                <Items>
                <dx:LayoutItem FieldName="ID">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxSpinEdit ID="ASPxFormLayout1_E1" runat="server" Number="0" Width="100%">
                            </dx:ASPxSpinEdit>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="SeverityID" Caption="Severity">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxComboBox ID="flDetails_E2" runat="server" DataSourceID="sqlSeverity" TextField="Name" ValueField="ID" Width="100%">
                        </dx:ASPxComboBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="CreationDate">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxDateEdit ID="ASPxFormLayout1_E2" runat="server" Width="100%" DisplayFormatString="g">
                            </dx:ASPxDateEdit>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="ResponseDate">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxDateEdit ID="ASPxFormLayout1_E3" runat="server" Width="100%" DisplayFormatString="g">
                            </dx:ASPxDateEdit>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="RestorationDate">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxDateEdit ID="ASPxFormLayout1_E4" runat="server" Width="100%" DisplayFormatString="g">
                            </dx:ASPxDateEdit>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="ResolutionDate">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxDateEdit ID="ASPxFormLayout1_E5" runat="server" Width="100%" DisplayFormatString="g">
                            </dx:ASPxDateEdit>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="ClosureDate">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxDateEdit ID="ASPxFormLayout1_E6" runat="server" Width="100%" DisplayFormatString="g">
                            </dx:ASPxDateEdit>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="BranchID" Caption="Branch">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxComboBox ID="flDetails_E15" runat="server" DataSourceID="sqlBranch" TextField="Name" ValueField="ID" Width="100%">
                        </dx:ASPxComboBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="UserName">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxTextBox ID="ASPxFormLayout1_E7" runat="server" Width="100%">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="ContactInstructions">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxTextBox ID="ASPxFormLayout1_E18" runat="server"  Width="100%">
                        </dx:ASPxTextBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="OEMID" Caption="OEM">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxComboBox ID="flDetails_E3" runat="server" DataSourceID="sqlOEM" TextField="Name" ValueField="ID" Width="100%">
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="TechnologyID" Caption="Technology">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxComboBox ID="flDetails_E5" runat="server" DataSourceID="sqlTechnology" TextField="Name" ValueField="ID" Width="100%">
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="NetworkElementID" Caption="Network Element">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxComboBox ID="flDetails_E7" runat="server" DataSourceID="sqlNetwork" TextField="Name" ValueField="ID" Width="100%">
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="RadioControllerID" Caption="Controller">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxComboBox ID="flDetails_E9" runat="server" DataSourceID="sqlController" TextField="Name" ValueField="ID" Width="100%">
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="ControllerIP">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxTextBox ID="ASPxFormLayout1_E23" runat="server" Width="100%">
                        </dx:ASPxTextBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="Sites">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxMemo ID="flDetails_E4" runat="server" Width="100%">
                        </dx:ASPxMemo>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="SiteIP">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxTextBox ID="ASPxFormLayout1_E22" runat="server" Width="100%">
                        </dx:ASPxTextBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="StatusID" Caption="Status">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxComboBox ID="flDetails_E11" runat="server" DataSourceID="sqlStatus" TextField="Name" ValueField="ID" Width="100%">
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="ProblemTitle">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxTextBox ID="ASPxFormLayout1_E14" runat="server"  Width="100%">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="ProblemDescription">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxMemo ID="flDetails_E13" runat="server" Width="100%" Height="300px">
                            </dx:ASPxMemo>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="Remedy">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxTextBox ID="ASPxFormLayout1_E19" runat="server" Width="100%">
                        </dx:ASPxTextBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="SoftwareRelease">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxTextBox ID="ASPxFormLayout1_E16" runat="server" Width="100%">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="FormalAnswerAccepted">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxCheckBox ID="ASPxFormLayout1_E17" runat="server" CheckState="Unchecked">
                            </dx:ASPxCheckBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="CreationDateUtc">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxDateEdit ID="ASPxFormLayout1_E20" runat="server" Width="100%" DisplayFormatString="g">
                            </dx:ASPxDateEdit>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="SolutionTypeID" Caption="Type Of Solution">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxComboBox ID="ASPxComboBox1" runat="server" DataSourceID="SolutionTypeSql" TextField="Name" ValueField="ID" Width="100%">
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
            </Items>
            </dx:ASPxFormLayout>
            <asp:SqlDataSource ID="sqlDetails" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT * FROM [tblTicket] WHERE ID = @ID">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvTickets" PropertyName="Text" DbType="Int64" Name="ID" />
            </SelectParameters>
        </asp:SqlDataSource>
            <div class="containter">
                <div class="col-6">
                    <br />
                    <asp:CheckBox ID="cbCreator" Text="Creator" runat="server" />
                    <asp:CheckBox ID="cbGroups" Text="Groups" runat="server" />
                    <asp:CheckBox ID="cbAmerinode" Text="Amerinode" runat="server" />
                    <asp:CheckBox ID="cbYourself" Text="Yourself" runat="server" />
                    <br />
                    <dx:ASPxButton ID="btnEmail2" runat="server" Text="Resend Email" Width="150px" OnClick="ASPxButton1_Click1"></dx:ASPxButton>
                    <br />
                    <br />
                    <dx:ASPxRadioButton ID="rbOpen" AutoPostBack="true" GroupName="Status" Text="Accept Ticket" Layout="Flow" runat="server" Checked="true" OnCheckedChanged="rbOpen_CheckedChanged"></dx:ASPxRadioButton>
                    <dx:ASPxRadioButton ID="rbReject" AutoPostBack="true" GroupName="Status" Text="Reject Ticket" Layout="Flow" runat="server" OnCheckedChanged="rbReject_CheckedChanged"></dx:ASPxRadioButton>
                    <div class="float-right">
                        Rejected Or Accepted By:&nbsp;
                        <dx:ASPxLabel runat="server" ID="lblRejectBy" CssClass="float-right"></dx:ASPxLabel>
                    </div>
                    <dx:ASPxPanel runat="server" Visible="false" ID="pnlReject">
                        <PanelCollection>
                            <dx:PanelContent>
                                <dx:ASPxLabel runat="server" Text="Enter Reject Note:"></dx:ASPxLabel>
                                <dx:ASPxMemo EncodeHtml="true" ID="memoRejectNote" runat="server" Width="100%" Height="71px"></dx:ASPxMemo>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxPanel>
                    <br />
                    <dx:ASPxButton runat="server" ID="btnSaveStatus" OnClick="btClose_Click" Width="150px" Text="Save data"></dx:ASPxButton>
                </div>
            </div>
        </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <br />
    <dx:ASPxRoundPanel ID="Panel2" runat="server" ShowCollapseButton="true" Width="100%" HeaderText="Customer Uploaded Files" AllowCollapsingByHeaderClick="true" Visible="false">
        <PanelCollection>
            <dx:PanelContent runat="server">
                <dx:ASPxFileManager ID="fmCustomer" runat="server" SettingsEditing-AllowDownload="true" SettingsEditing-AllowDelete="true" SettingsEditing-AllowRename="true">
                    <Settings RootFolder="~/Content/Tickets/" ThumbnailFolder="~/Thumb/" />
                    <SettingsUpload Enabled="False">
                    </SettingsUpload>
                </dx:ASPxFileManager>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <br />
    <dx:ASPxRoundPanel ID="Panel3" runat="server" ShowCollapseButton="true" Width="100%" HeaderText="Set Dates and Notes / Update Status" AllowCollapsingByHeaderClick="true" Visible="false">
        <PanelCollection>
            <dx:PanelContent runat="server">
                <div class="container-fluid">
                    <div class="row content-row">
                        <div class="col-6" style="position:static">
                            <asp:label ID="lbRespDateTime" runat="server" Text="Response Date and Time:" ForeColor="Blue"></asp:label>
                            <div class="float-right">
                                
                                <dx:ASPxLabel ID="lbResp" runat="server"></dx:ASPxLabel>
                            </div>
                            <dx:ASPxDateEdit ID="deRespDate" runat="server" Width="350px" EditFormat="DateTime" TimeSectionProperties-Visible="true" PopupVerticalAlign="Below" style="z-index:150">
                                <ClearButton DisplayMode="OnHover"/>
                                <TimeSectionProperties Visible="True"></TimeSectionProperties>
                                <ClientSideEvents DropDown="initDate"/>
                            </dx:ASPxDateEdit>
                            <asp:label ID="lbRestDateTime" runat="server" Text="Restoration Date and Time:" ForeColor="Blue" Visible="false"></asp:label>
                            <dx:ASPxDateEdit ID="deRestDate" runat="server" Width="350px" EditFormat="DateTime" Visible="false" TimeSectionProperties-Visible="true">
                                    <ClearButton DisplayMode="OnHover"/>
                                    <TimeSectionProperties Visible="True"></TimeSectionProperties>
                                    <ClientSideEvents DropDown="initDate"/>
                            </dx:ASPxDateEdit>
                            <asp:label ID="lbResoDateTime" runat="server" Text="Resolution Date and Time:" ForeColor="Blue" Visible="false"></asp:label>
                            <dx:ASPxDateEdit ID="deResoDate" runat="server" Width="350px" EditFormat="DateTime" Visible="false" TimeSectionProperties-Visible="true">
                                    <ClearButton DisplayMode="OnHover"/>
                                    <TimeSectionProperties Visible="True"></TimeSectionProperties>
                                    <ClientSideEvents DropDown="initDate"/>
                            </dx:ASPxDateEdit>
                            <asp:label ID="lbCloseDateTime" runat="server" Text="Closure Date and Time:" ForeColor="Blue" Visible="false"></asp:label>
                            <dx:ASPxDateEdit ID="deClosDate" runat="server" Width="350px" EditFormat="DateTime" Visible="false" TimeSectionProperties-Visible="true">
                                            <ClearButton DisplayMode="OnHover"/>
                                            <TimeSectionProperties Visible="True"></TimeSectionProperties>
                                            <ClientSideEvents DropDown="initDate"/>
                                        </dx:ASPxDateEdit>
                            <br />
                            
                            <asp:Label ID="lbMsg" runat="server" Text="" ForeColor="Blue"></asp:Label>
                            <br /><br />
                            <dx:ASPxMemo EncodeHtml="true" ID="meRespNote" runat="server" Height="171px" Width="100%" BackColor="#CCFFCC">
                                <Border BorderColor="Black" BorderStyle="Solid" />                                
                            </dx:ASPxMemo>
                            <br />                            
                            <dx:ASPxPanel runat="server" ID="pnlSolution" Visible="false" Width="100%">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <asp:Label ID="Label1" runat="server" Text="Type Of Solution:" ForeColor="Blue"></asp:Label>
                                        
                                        <dx:ASPxComboBox Width="30%" ID="SolutionType" runat="server" DataSourceID="SolutionTypeSql" ValueField="ID" TextField="Name" OnDataBound="SolutionType_DataBound"></dx:ASPxComboBox><br />
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxPanel>
                            <dx:ASPxButton ID="btSaveData" AutoPostBack="true" runat="server" Text="Save data" OnClick="btSaveData_Click"></dx:ASPxButton>
                            <dx:ASPxButton ID="btClose" runat="server" AutoPostBack="true" Text="Close status" OnClick="btClose_Click"></dx:ASPxButton>
                            <br /><br />
                            <asp:Label ID="Label5" runat="server" Text="Place Ticket hold time Rest:" ForeColor="Blue"></asp:Label>                            
                            <dx:ASPxDateEdit ID="deDateRest" runat="server" Width="350px" EditFormat="DateTime" TimeSectionProperties-Visible="true" Enabled="false">
                            <TimeSectionProperties Visible="True"></TimeSectionProperties>
                            <ClientSideEvents Init="initDate" DropDown="initDate"/>
                            </dx:ASPxDateEdit> 
                            <br /><br />
                            <asp:Label ID="Label2" runat="server" Text="Description Hold Note:" ForeColor="Blue" ></asp:Label>
                            <br /><br />
                            <dx:ASPxMemo EncodeHtml="true" runat="server" ID="memoTextRest" Height="170px" Width="100%" BackColor="#F0E68C"></dx:ASPxMemo>
                            <dx:ASPxPanel ID="pnlRespNote" runat="server" Width="100%">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <div style="overflow-y: auto; height: 18px; margin-top: 10px;">
                                            <asp:Label runat="server" ID="lblRespNote" ></asp:Label>
                                        </div>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxPanel>
                            <dx:ASPxButton ID="btnToPendingRest" runat="server" Text="TO Pending Info Customer" OnClick="btToPending_Click" Enabled="false"></dx:ASPxButton>
                            <dx:ASPxButton ID="btnFromPendingRest" runat="server" Text="FROM Customer Pending Info" OnClick="btFromPending_Click" Enabled="false"></dx:ASPxButton>
                            <br /><br />
                            <asp:Label ID="Label3" runat="server" Text="Hold Time History:" ForeColor="Blue"></asp:Label>                             
                            <br /><br />
                            <dx:ASPxMemo EncodeHtml="true" ID="memoRest" runat="server" Height="350px" Width="100%" ReadOnly="true" BackColor="#F5F5F5"></dx:ASPxMemo>                                                                
                            <br /><br />                           
                        </div>
                        <div class="col-6" style="position:static">
                            
                            <asp:UpdatePanel runat="server" ID="pnlTick" ViewStateMode="Enabled" OnUnload="UpdatePanel_Unload" Visible="true">
                                <ContentTemplate>
                                    Total status time:
                                    <dx:ASPxLabel runat="server" ID="lblTotalResp" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>&nbsp;
                                    
                                    <dx:ASPxLabel ID="lblTotalTimeResp" runat="server" Font-Bold="true" Font-Size="Medium" Visible="false"></dx:ASPxLabel>&nbsp;
                                    Chronometer:
                                    <dx:ASPxLabel ID="lblChroResp" runat="server" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
                                    <asp:Timer runat="server" ID="timerResp" Interval="1000" OnTick="tTick_Tick" Enabled="false"></asp:Timer>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="timerResp" EventName="Tick" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel runat="server" ID="pnlTickRest" ViewStateMode="Enabled" OnUnload="UpdatePanel_Unload" Visible="false">
                                            <ContentTemplate>
                                                Total status time:
                                                <dx:ASPxLabel runat="server" ID="lblTotalRest" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>&nbsp;-
                                                Total time hold:
                                                <dx:ASPxLabel ID="lblTotalTimeRest" runat="server" Font-Bold="true" Font-Size="Medium" ></dx:ASPxLabel>&nbsp;-
                                                Chronometer:
                                                <dx:ASPxLabel ID="lblChroRest" runat="server" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
                                                <asp:Timer runat="server" ID="timerRest" Interval="1000" OnTick="tTick_Tick" Enabled="false"></asp:Timer>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="timerRest" EventName="Tick" />
                                            </Triggers>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel runat="server" ID="pnlTickReso" ViewStateMode="Enabled" OnUnload="UpdatePanel_Unload" Visible="false">
                                            <ContentTemplate>
                                                Total ticket time:
                                                <dx:ASPxLabel runat="server" ID="lblTotalReso" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>&nbsp;-
                                                Total time hold:
                                                <dx:ASPxLabel ID="lblTotalTimeReso" runat="server" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>&nbsp;-
                                                Chronometer:
                                                <dx:ASPxLabel ID="lblChroReso" runat="server" Font-Bold="true" Font-Size="Medium"></dx:ASPxLabel>
                                                <asp:Timer runat="server" ID="timerReso" Interval="1000" OnTick="tTick_Tick" Enabled="false"></asp:Timer>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="timerReso" EventName="Tick" />
                                            </Triggers>
                            </asp:UpdatePanel>
                            <br />
                            <asp:Label ID="lbHistLog" runat="server" Text="Ticket History Log:" ForeColor="Blue"></asp:Label>
                            <br /><br />
                            <dx:ASPxMemo EncodeHtml="true" ID="meHistoryLog" runat="server" ReadOnly="true" Height="400px" Width="100%" BackColor="#F5F5F5">
                                <Border BorderColor="Navy" BorderStyle="Solid" />                                
                            </dx:ASPxMemo>
                            <dx:ASPxPanel runat="server" ID="pnlFileReso" Visible="true">
                                            <PanelCollection>
                                                <dx:PanelContent>
                                                    Upload File (images, pdf, txt, excel, word / 5 MB max):
                                                    <dx:ASPxUploadControl ID="ucReso" ClientInstanceName="ucFile" runat="server" ShowUploadButton="true" UploadButton-Text="click here to upload all files" NullText="you can select multiple files" UploadMode="Auto" Width="100%" OnFilesUploadComplete="On_FilesUploadComplete">
                                                        <UploadButton Text="click here to upload all files"></UploadButton>
                                                        <AdvancedModeSettings EnableMultiSelect="true"></AdvancedModeSettings>
                                                        <ValidationSettings AllowedFileExtensions=".jpg,.jpeg,.png,.tif,.tiff,.gif,.pdf,.txt,.xls,.xlsx,.doc,.docx,.log,.msg,.tmf,.zip,.rar" MaxFileSize="5242880" NotAllowedFileExtensionErrorText="Extensions allowed are: jpg, jpeg, png, tif, tiff, gif, pdf, txt, xls, xlsx, doc, docx, log, msg, .tmf, .zip and .rar" MaxFileSizeErrorText="Maximum allowed size is 5 Mbyte"></ValidationSettings>
                                                    </dx:ASPxUploadControl>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxPanel>  
                            
                        </div>
                    </div>                    
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <br />
    <dx:ASPxRoundPanel ID="Panel4" runat="server" ShowCollapseButton="true" Width="100%" HeaderText="Upload Restoration and Resolution Files" AllowCollapsingByHeaderClick="true" Visible="false">
        <PanelCollection>
            <dx:PanelContent runat="server">
                Upload File (images, pdf, txt, excel, word / 5 MB max):
                <dx:ASPxUploadControl ID="ucFile" ClientInstanceName="ucFile" runat="server" ShowUploadButton="true" UploadButton-Text="click here to upload all files" NullText="you can select multiple files" UploadMode="Auto" Width="100%" OnFilesUploadComplete="ucFile_FilesUploadComplete">
                    <AdvancedModeSettings EnableMultiSelect="true"></AdvancedModeSettings>
                    <ValidationSettings AllowedFileExtensions=".jpg,.jpeg,.png,.tif,.tiff,.gif,.pdf,.txt,.xls,.xlsx,.doc,.docx,.log,.msg,.tmf,.zip,.rar" MaxFileSize="5242880" NotAllowedFileExtensionErrorText="Extensions allowed are: jpg, jpeg, png, tif, tiff, gif, pdf, txt, xls, xlsx, doc, docx, log, msg, .tmf, .zip and .rar" MaxFileSizeErrorText="Maximum allowed size is 5 Mbyte"></ValidationSettings>
                </dx:ASPxUploadControl>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <dx:ASPxPopupControl ID="popMsg" ClientInstanceName="popMsg" runat="server" CloseOnEscape="True" Modal="True" AllowDragging="True" PopupAnimationType="Fade" PopupVerticalAlign="WindowCenter" EnableViewState="False" CssClass="auto-style7" HeaderText="Q MESSAGE" PopupHorizontalAlign="WindowCenter" Width="500px">
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <dx:ASPxLabel ID ="lblMsg" runat="server" Text="ASPxLabel"></dx:ASPxLabel><br />
            <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False" ClientInstanceName="btnCancel" Text="Ok" Width="80px">
                <ClientSideEvents Click="function(s, e) {popMsg.Hide();}" />
            </dx:ASPxButton>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
</asp:Content>
