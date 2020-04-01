<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="TicketDetails.aspx.cs" Inherits="ATCPortal.Amerinode.TicketDetails" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    Choose Ticket #: 
    <dx:ASPxGridLookup ID="gvTickets" runat="server" DataSourceID="sqlTickets" AutoPostBack="false" AutoGenerateColumns="False" KeyFieldName="ID" TextFormatString="{0}" OnTextChanged="gvTickets_TextChanged" ForeColor="Navy">
        <GridViewProperties>
        <SettingsBehavior AllowFocusedRow="False" AllowSelectSingleRowOnly="True"></SettingsBehavior>
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
                <PropertiesComboBox DataSourceID="sqlSeverity" TextField="Name" ValueField="ID">
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
        <Border BorderStyle="Solid" />
    </dx:ASPxGridLookup>
    <br />
    <asp:Label runat="server" id="Status_tck" Text="" Font-Size="Large"></asp:Label>
    <br />
    <asp:SqlDataSource ID="sqlTickets" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT [ID], [CreationDate], [SeverityID], [OEMID], [TechnologyID], [NetworkElementID], [ProblemTitle], [StatusID], BranchID FROM [tblTicket]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlSeverity" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name, CompanyID FROM tblSeverity"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlOEM" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblOEM"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlTechnology" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblTechnology"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlNetwork" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblNetworkElement"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlController" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblController"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlStatus" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblStatus"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlBranch" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblBranch"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SolutionTypeSql" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT * FROM [tblSolutionType]"></asp:SqlDataSource>
    <br />
    <dx:ASPxButton ID="ASPxButton1" runat="server" Text="View Details and Upload Ticket Files" OnClick="ASPxButton1_Click"></dx:ASPxButton>
    <br />
    <br />
    <dx:ASPxRoundPanel ID="Panel1" Visible="false" runat="server" HeaderText="Ticket Information" ShowCollapseButton="true" AllowCollapsingByHeaderClick="true" Width="100%">
        <PanelCollection>
        <dx:PanelContent runat="server">
            <dx:ASPxFormLayout ID="flDetails" Width="100%" runat="server" DataSourceID="sqlDetails" ColCount="2" Enabled="false">
                <Items>
                <dx:LayoutItem FieldName="ID">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxSpinEdit ID="ASPxFormLayout1_E1" runat="server" Number="0" Width="100%" ReadOnly="true">
                            </dx:ASPxSpinEdit>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="CreationDate">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxDateEdit ID="ASPxFormLayout1_E2" runat="server" Width="100%" DisplayFormatString="g" ReadOnly="true">
                            </dx:ASPxDateEdit>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="ResponseDate">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxDateEdit ID="ASPxFormLayout1_E3" runat="server" Width="100%" DisplayFormatString="g" ReadOnly="true">
                            </dx:ASPxDateEdit>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="RestorationDate">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxDateEdit ID="ASPxFormLayout1_E4" runat="server" Width="100%" DisplayFormatString="g" ReadOnly="true">
                            </dx:ASPxDateEdit>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="ResolutionDate">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxDateEdit ID="ASPxFormLayout1_E5" runat="server" Width="100%" DisplayFormatString="g" ReadOnly="true">
                            </dx:ASPxDateEdit>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="ClosureDate">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxDateEdit ID="ASPxFormLayout1_E6" runat="server" Width="100%" DisplayFormatString="g" ReadOnly="true">
                            </dx:ASPxDateEdit>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="UserName">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxTextBox ID="ASPxFormLayout1_E7" runat="server" Width="100%" ReadOnly="true">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="SeverityID" Caption="Severity">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxComboBox ID="cbSeverity" runat="server" DataSourceID="sqlSeverity" TextField="Name" ValueField="ID" Width="100%">
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="OEMID" Caption="OEM">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxComboBox ID="cbOem" runat="server" AutoPostBack="true" DataSourceID="sqlOEM" TextField="Name" ValueField="ID" Width="100%" OnSelectedIndexChanged="cbOem_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="TechnologyID" Caption="Technology">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxComboBox ID="cbTechnology" runat="server" AutoPostBack="true" DataSourceID="sqlTechnology" TextField="Name" ValueField="ID" Width="100%" OnSelectedIndexChanged="cbTechnology_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="NetworkElementID" Caption="Network Element">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxComboBox ID="cbNetwork" runat="server" DataSourceID="sqlNetwork" TextField="Name" ValueField="ID" Width="100%">
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="RadioControllerID" Caption="Controller">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxComboBox ID="cbRadioController" AutoPostBack="true" runat="server" DataSourceID="sqlController" TextField="Name" ValueField="ID" OnSelectedIndexChanged="cbRadioController_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="StatusID" Caption="Status">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxComboBox ID="flDetails_E11" runat="server" DataSourceID="sqlStatus" TextField="Name" ValueField="ID" Width="100%" ReadOnly="true">
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="ProblemTitle">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxTextBox ID="txtProblemTitle" runat="server"  Width="100%">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="ProblemDescription">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxMemo ID="memoProblemDescription" runat="server" Width="100%" Height="300px">
                            </dx:ASPxMemo>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="SoftwareRelease">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxTextBox ID="txtSoftwareRelease" runat="server" Width="100%">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="FormalAnswerAccepted">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxCheckBox ID="cbFormalAnswer" runat="server" CheckState="Unchecked">
                            </dx:ASPxCheckBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="ContactInstructions">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxTextBox ID="txtContactInstructions" runat="server"  Width="100%">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="Remedy">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxTextBox ID="txtRemedy" runat="server" Width="100%">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="CreationDateUtc">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxDateEdit ID="ASPxFormLayout1_E20" runat="server" Width="100%" DisplayFormatString="g" ReadOnly="true">
                            </dx:ASPxDateEdit>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="BranchID" Caption="Branch">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxComboBox ID="flDetails_E15" runat="server" DataSourceID="sqlBranch" TextField="Name" ValueField="ID" Width="100%" ReadOnly="true">
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="SiteIP">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxTextBox ID="txtSiteIP" runat="server" Width="100%">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="ControllerIP">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxTextBox ID="txtControllerIP" runat="server" Width="100%">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="Sites">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxMemo ID="memoSites" runat="server" Width="100%" ReadOnly="true">
                            </dx:ASPxMemo>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem FieldName="SolutionTypeID" Caption="Type Of Solution">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server">
                            <dx:ASPxComboBox ID="ASPxComboBox1" runat="server" DataSourceID="SolutionTypeSql" TextField="Name" ValueField="ID" Width="100%" ReadOnly="true">
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
            </Items>
            </dx:ASPxFormLayout>
            <dx:ASPxButton runat="server" Text="Update" CssClass="float-right" ID="btnUpdate" OnClick="btnUpdate_Click" Visible="false"></dx:ASPxButton>
            <br /><br />
            <asp:SqlDataSource ID="sqlDetails" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT * FROM [tblTicket] WHERE ID = @ID">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvTickets" PropertyName="Text" DbType="Int64" Name="ID" />
        </SelectParameters>
    </asp:SqlDataSource>
        </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <br />
    <dx:ASPxRoundPanel ID="Panel2" runat="server" ShowCollapseButton="true" Width="100%" HeaderText="Uploaded Files" AllowCollapsingByHeaderClick="true" Visible="false">
        <PanelCollection>
            <dx:PanelContent runat="server">
                <dx:ASPxFileManager ID="fmCustomer" runat="server" SettingsEditing-AllowDownload="true" SettingsEditing-AllowRename="true">
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
                            
                            <asp:Label ID="lbHistLog" runat="server" Text="Ticket History Log:" ForeColor="Blue"></asp:Label>
                            <br /><br />
                            <dx:ASPxMemo EncodeHtml="true" ID="meHistoryLog" runat="server" ReadOnly="true" Height="600px" Width="100%" BackColor="#CCFFCC">
                                <Border BorderColor="Navy" BorderStyle="Solid" />
                            </dx:ASPxMemo>
                            <dx:ASPxMemo EncodeHtml="true" ID="meRespNote" runat="server" Height="10px" Width="100%" BackColor="#CCFFCC" Visible="false">
                                <Border BorderColor="Black" BorderStyle="Solid" />                                
                            </dx:ASPxMemo>
                            <br />                            
                            
                            <asp:Label ID="Label3" runat="server" Text="Hold Time History:" ForeColor="Blue" Visible="false"></asp:Label>                             
                            <br /><br />
                            <dx:ASPxMemo EncodeHtml="true" ID="memoRest" runat="server" Height="350px" Width="100%" ReadOnly="true" BackColor="#F5F5F5" Visible="false"></dx:ASPxMemo>                                                                
                            <br /><br />                           
                        </div>
                        <div class="col-6" style="position:static">
                           <br /><br />
                             <asp:label ID="lbRespDateTime" runat="server" Text="Response Date and Time:" ForeColor="Blue"></asp:label>
                           
                            <dx:ASPxDateEdit ID="deRespDate" runat="server" Width="350px" EditFormat="DateTime" TimeSectionProperties-Visible="true" PopupVerticalAlign="Below" style="z-index:150">
                                <ClearButton DisplayMode="OnHover"/>
                                <TimeSectionProperties Visible="True"></TimeSectionProperties>
                                <ClientSideEvents DropDown="initDate"/>
                            </dx:ASPxDateEdit>
                            <br /><br />
                            <asp:label ID="lbRestDateTime" runat="server" Text="Restoration Date and Time:" ForeColor="Blue" Visible="true"></asp:label>
                            
                            <dx:ASPxDateEdit ID="deRestDate" runat="server" Width="350px" EditFormat="DateTime" Visible="true" TimeSectionProperties-Visible="true">
                                    <ClearButton DisplayMode="OnHover"/>
                                    <TimeSectionProperties Visible="True"></TimeSectionProperties>
                                    <ClientSideEvents DropDown="initDate"/>
                            </dx:ASPxDateEdit>
                            <br /><br />
                            <asp:label ID="lbResoDateTime" runat="server" Text="Resolution Date and Time:" ForeColor="Blue" Visible="true"></asp:label>
                            
                            <dx:ASPxDateEdit ID="deResoDate" runat="server" Width="350px" EditFormat="DateTime" Visible="true" TimeSectionProperties-Visible="true">
                                    <ClearButton DisplayMode="OnHover"/>
                                    <TimeSectionProperties Visible="True"></TimeSectionProperties>
                                    <ClientSideEvents DropDown="initDate"/>
                            </dx:ASPxDateEdit>
                            <br /><br />
                            <asp:label ID="lbCloseDateTime" runat="server" Text="Closure Date and Time:" ForeColor="Blue" Visible="true"></asp:label>
                            
                            <dx:ASPxDateEdit ID="deClosDate" runat="server" Width="350px" EditFormat="DateTime" Visible="true" TimeSectionProperties-Visible="true">
                                            <ClearButton DisplayMode="OnHover"/>
                                            <TimeSectionProperties Visible="True"></TimeSectionProperties>
                                            <ClientSideEvents DropDown="initDate"/>
                                        </dx:ASPxDateEdit>                               
                            <asp:Label runat="server" Text=""></asp:Label><asp:Label ID="lblTest" runat="server" Text=""></asp:Label>
                        </div>
                    </div>                    
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <br />
    <dx:ASPxRoundPanel ID="Panel4" runat="server" ShowCollapseButton="true" Width="100%" HeaderText="Upload Ticket Files" AllowCollapsingByHeaderClick="true" Visible="false">
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
            <dx:ASPxLabel ID ="lblMsg" runat="server" Text="ASPxLabel"></dx:ASPxLabel><br /><br />
            <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False" ClientInstanceName="btnCancel" Text="Ok" Width="80px">
                <ClientSideEvents Click="function(s, e) {popMsg.Hide();}" />
            </dx:ASPxButton>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>    
</asp:Content>
