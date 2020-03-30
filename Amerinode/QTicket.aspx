<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="QTicket.aspx.cs" Inherits="ATCPortal.Amerinode.QTicket" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function showImagesInfo(link) {
            if (!link) {
                return;
            }
            clientPopupControl.SetContentUrl(link);
            clientPopupControl.Show();
        }
    </script>
            <h2>
                Q Ticketing Module - Continuous Process Improvement
            </h2>
            <dx:ASPxGridView ID="gvMain" runat="server" DataSourceID="sqlMain" ClientInstanceName="gvMain" AutoGenerateColumns="False" KeyFieldName="ID" EnableTheming="True" Theme="Mulberry" Width="2100px" Settings-VerticalScrollBarMode="Hidden" Settings-VerticalScrollableHeight="480" OnRowUpdating="gvMain_RowUpdating" OnInitNewRow="gvMain_InitNewRow" OnRowInserted="gvMain_RowInserted" OnCustomUnboundColumnData="gvMain_CustomUnboundColumnData" OnInit="gvMain_Init">
                <ClientSideEvents CustomButtonClick="" BeginCallback="function(s, e) {gvMain.cpMessage='';}" EndCallback="function(s, e) {if(gvMain.cpMessage) {alert(gvMain.cpMessage);}}" />
                <SettingsPager PageSize="15"/>
                <Settings ShowGroupPanel="True" ShowFooter="True" ShowHeaderFilterButton="true" />
                <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="WYSIWYG" />
                <SettingsDataSecurity AllowDelete="False" />
                <SettingsSearchPanel Visible="True" />
                <SettingsBehavior AllowEllipsisInText="true" />
                <SettingsEditing EditFormColumnCount="2" Mode="PopupEditForm" />
                <SettingsPopup EditForm-Modal="true" EditForm-Width="800" EditForm-VerticalAlign="WindowCenter" EditForm-HorizontalAlign="WindowCenter" >
                <EditForm Width="800px" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Modal="True"></EditForm>
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
                    <dx:GridViewCommandColumn ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0" ShowClearFilterButton="True">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn FieldName="ID" Caption="Ticket" ReadOnly="True" VisibleIndex="1">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FieldName="DateOpen" Caption="Open" ReadOnly="true" Settings-AllowHeaderFilter="True" Settings-AllowAutoFilter="False" VisibleIndex="6">
                        <PropertiesDateEdit>
                            <DropDownButton Enabled="False"></DropDownButton>
                        </PropertiesDateEdit>
                        <Settings AllowAutoFilter="False" AllowHeaderFilter="True"></Settings>
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataDateColumn FieldName="DateClose" Caption="Close" ReadOnly="true" Settings-AllowHeaderFilter="True" Settings-AllowAutoFilter="False"  VisibleIndex="8">
                        <PropertiesDateEdit>
                            <DropDownButton Enabled="False">
                            </DropDownButton>
                        </PropertiesDateEdit>
                        <Settings AllowAutoFilter="False" AllowHeaderFilter="True"></Settings>
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataCheckColumn FieldName="CloseApproved" Caption="Approved" VisibleIndex="10">
                    </dx:GridViewDataCheckColumn>
                    <dx:GridViewDataComboBoxColumn Caption="Priority" FieldName="PriorityID" VisibleIndex="3">
                        <PropertiesComboBox DataSourceID="sqlPriority" TextField="Name" ValueField="ID"></PropertiesComboBox>
                        <SettingsHeaderFilter Mode="CheckedList"/>
                    </dx:GridViewDataComboBoxColumn>
                    <dx:GridViewDataComboBoxColumn Caption="Status" FieldName="StatusID" VisibleIndex="2">
                        <PropertiesComboBox DataSourceID="sqlStatus" TextField="Name" ValueField="ID"></PropertiesComboBox>
                        <SettingsHeaderFilter Mode="CheckedList"/>
                    </dx:GridViewDataComboBoxColumn>
                    <dx:GridViewDataMemoColumn FieldName="Description" Width="300" VisibleIndex="4">
                        <PropertiesMemoEdit Height="71px" />
                    </dx:GridViewDataMemoColumn>
                    <dx:GridViewDataMemoColumn FieldName="Resolution" Width="300" VisibleIndex="5">
                        <PropertiesMemoEdit Height="71px" />
                    </dx:GridViewDataMemoColumn>
                    <dx:GridViewDataTextColumn FieldName="QVersion" VisibleIndex="12">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataCheckColumn FieldName="CustomerImpacting" VisibleIndex="13" Width="180">
                        <PropertiesCheckEdit ValueGrayed="False">
                        </PropertiesCheckEdit>
                    </dx:GridViewDataCheckColumn>
                    <dx:GridViewDataTextColumn FieldName="OpenBy" ReadOnly="True" VisibleIndex="7">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="CloseBy" ReadOnly="True" VisibleIndex="9">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ApprovedBy" ReadOnly="True" VisibleIndex="11" Width="130px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Visible="false">
                        <EditFormSettings Visible="True" VisibleIndex="14"/>
                        <EditItemTemplate>
                            <asp:UpdatePanel ID="pnlUpload" runat="server" Visible="false">
                                <ContentTemplate>
                                    Upload File (images, pdf, txt, excel, word / 5 MB max):
                                    <dx:ASPxUploadControl ID="ucFile" ClientInstanceName="ucFile" runat="server" ShowUploadButton="true" UploadButton-Text="click here to upload all files" NullText="you can select multiple files" UploadMode="Auto" Width="100%" OnFilesUploadComplete="ucFile_FilesUploadComplete">
                                        <AdvancedModeSettings EnableMultiSelect="true"></AdvancedModeSettings>
                                        <ValidationSettings AllowedFileExtensions=".jpg,.jpeg,.png,.tif,.tiff,.gif,.pdf,.txt,.xls,.xlsx,.doc,.docx,.log,.msg,.tmf,.zip,.rar" MaxFileSize="5242880" NotAllowedFileExtensionErrorText="Extensions allowed are: jpg, jpeg, png, tif, tiff, gif, pdf, txt, xls, xlsx, doc, docx, log, msg, .tmf, .zip and .rar" MaxFileSizeErrorText="Maximum allowed size is 5 Mbyte"></ValidationSettings>
                                    </dx:ASPxUploadControl>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </EditItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
                <GroupSummary>
                    <dx:ASPxSummaryItem FieldName="ID" SummaryType="Count" />
                </GroupSummary>
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="ID" SummaryType="Count" />
                </TotalSummary>
            </dx:ASPxGridView>
            <asp:ScriptManager runat="server" ID="smPanel"></asp:ScriptManager>
            <asp:SqlDataSource ID="sqlMain" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>"
                SelectCommand="SELECT base_ticket.ID, Description, PriorityID, StatusID, DateOpen, DateClose, OpenBy, CloseBy, CloseApproved, Resolution, ApprovedBy, QVersion, CustomerImpacting
FROM base_ticket 
ORDER BY PriorityID"
                InsertCommand="INSERT INTO base_ticket (Description, PriorityID, CustomerImpacting) VALUES (@Description, @PriorityID, @CustomerImpacting)"
                UpdateCommand="UPDATE base_ticket SET Description = @Description, PriorityID = @PriorityID, StatusID = @StatusID, CloseApproved = @CloseApproved, Resolution = @Resolution, QVersion = @QVersion, CustomerImpacting = @CustomerImpacting WHERE ID = @ID">
                <InsertParameters>
                    <asp:Parameter Name="Description" Type="String" />
                    <asp:Parameter Name="PriorityID" Type="Int32" />
                    <asp:Parameter Name="CustomerImpacting" Type="Boolean" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="Description" Type="String" />
                    <asp:Parameter Name="PriorityID" Type="Int32" />
                    <asp:Parameter Name="StatusID" Type="Int32" />
                    <asp:Parameter Name="CloseApproved" Type="Boolean" />
                    <asp:Parameter Name="Resolution" Type="String" />
                    <asp:Parameter Name="ID" Type="Int32" />
                    <asp:Parameter Name="QVersion" Type="String" />
                    <asp:Parameter Name="CustomerImpacting" Type="Boolean"/>
                </UpdateParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="sqlPriority" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM base_ticketpriority"></asp:SqlDataSource>
            <asp:SqlDataSource ID="sqlStatus" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM base_ticketstatus"></asp:SqlDataSource>  
        <dx:ASPxPopupControl HeaderText="View files" runat="server" ClientInstanceName="clientPopupControl" CloseAction="CloseButton" Height="600px" Modal="True" Width="850px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
            <ContentCollection>
                <dx:PopupControlContentControl></dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
</asp:Content>
