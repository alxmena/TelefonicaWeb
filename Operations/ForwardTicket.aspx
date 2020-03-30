<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ForwardTicket.aspx.cs" Inherits="ATCPortal.FieldTechnician.ForwardTicket" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function OnOEMChanged(cbOEM) {
            cbTech.PerformCallback(cbOEM.GetValue().toString());
            cbElement.PerformCallback("0");
        }
        function OnTechChanged(cbTech) {
            cbElement.PerformCallback(cbTech.GetValue().toString());
        }
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
    </script>
    <br />
    <dx:ASPxRoundPanel ID="Panel1" runat="server" Width="700pt" HeaderText="Ticket and Contact Details">
        <PanelCollection>
            <dx:PanelContent runat="server">
                <table style="width:100%;">
                    <tr>
                        <td>Ticket Date:
                            <dx:ASPxDateEdit ID="deTicketDate" Enabled="false" runat="server" Width="90%">
                            </dx:ASPxDateEdit>
                        </td>
                        <td>Ticket #:
                            <dx:ASPxTextBox ID="tbTicket" Enabled="false" runat="server" Width="90%">
                            </dx:ASPxTextBox>
                        </td>
                        <td>Customer - Branch:
                            <dx:ASPxTextBox ID="tbBranch" Enabled="false" runat="server" Width="100%">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center" style="padding-bottom:10px; padding-top:10px;">
                            <dx:ASPxImage ID="imgDivider" runat="server" ImageUrl="~/Images/divider.png">
                            </dx:ASPxImage>
                        </td>
                    </tr>
                    <tr>
                        <td>Creator Name:
                            <dx:ASPxTextBox ID="tbName" Enabled="false" runat="server" Width="90%">
                            </dx:ASPxTextBox>
                        </td>
                        <td>Creator Title:
                            <dx:ASPxTextBox ID="tbTitle" Enabled="false" runat="server" Width="90%">
                            </dx:ASPxTextBox>
                        </td>
                        <td>Creator Email:
                            <dx:ASPxTextBox ID="tbEmail" Enabled="false" runat="server" Width="100%">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="padding-bottom:10px; padding-top:10px;">Notify the following Email Distribution Lists / Groups:
                            <dx:ASPxGridLookup ID="glGroup" ClientInstanceName="glGroup" SelectionMode="Multiple" ClearButton-DisplayMode="OnHover" runat="server" Width="100%" IncrementalFilteringMode="Contains" DataSourceID="sqlGroup" AutoGenerateColumns="False" KeyFieldName="ID" NullText="not defined - expand combobox for multiple selection" MultiTextSeparator=" & ">
                                <GridViewProperties>
                                    <Templates>
                                        <StatusBar>
                                            <table style="float: right">
                                                <tr>
                                                    <td>
                                                        <dx:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseglGroup" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </StatusBar>
                                    </Templates>
                                    <SettingsBehavior AllowSelectByRowClick="True" />
                                    <Settings AutoFilterCondition="Contains" ShowFilterRow="True" ShowStatusBar="Visible"/>
                                </GridViewProperties>
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="GroupEmail" Width="100%" VisibleIndex="1">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                            </dx:ASPxGridLookup>
                        </td>
                    </tr>
                    <tr>
                        <td>Creator Work Phone:
                            <dx:ASPxTextBox ID="tbWork" runat="server" Width="90%">
                            </dx:ASPxTextBox>
                        </td>
                        <td>Creator Mobile Phone:
                            <dx:ASPxTextBox ID="tbMobile" runat="server" Width="90%">
                            </dx:ASPxTextBox>
                        </td>
                        <td>
                            <dx:ASPxCheckBox ID="chWhats" Style="float:left; font-size: small;" Text="Has Whatsapp?" runat="server" TextAlign="Left">
                            </dx:ASPxCheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="padding-top:10px;">Ticket Additional Contact Instruction:
                            <dx:ASPxTextBox ID="tbContactInstructions" runat="server" Width="100%">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <br />
    <dx:ASPxRoundPanel ID="Panel2" runat="server" Width="700pt" HeaderText="Site Details">
        <PanelCollection>
            <dx:PanelContent runat="server">
                <table style="width:100%">
                    <tr>
                        <td style="width: 300px">OEM:*
                            <dx:ASPxComboBox ID="cbOEM" ClientInstanceName="cbOEM" Width="90%" runat="server" ValueType="System.String" DataSourceID="sqlOEM" ValueField="ID" TextField="Name" AllowNull="true" NullText="not defined" EnableSynchronization="False">
                                <ClientSideEvents SelectedIndexChanged="function(s, e) {OnOEMChanged(s); }" />
                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                <ValidationSettings ValidationGroup="Ticket" ErrorTextPosition="Right" ErrorDisplayMode="ImageWithTooltip">
                                    <RequiredField ErrorText="OEM selection is required" IsRequired="true" />
                                </ValidationSettings>
                            </dx:ASPxComboBox>
                        </td>
                        <td style="width: 300px">Technology:*
                            <dx:ASPxComboBox ID="cbTech" ClearButton-DisplayMode="OnHover" ClientInstanceName="cbTech" Width="90%" runat="server" ValueType="System.String" ValueField="ID" TextField="Name" AllowNull="true" NullText="not defined" OnCallback="cbTech_Callback" EnableSynchronization="False">
                                <ClientSideEvents SelectedIndexChanged="function(s, e) {OnTechChanged(s); }" />
                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                <ValidationSettings ValidationGroup="Ticket" ErrorTextPosition="Right" ErrorDisplayMode="ImageWithTooltip">
                                    <RequiredField ErrorText="Technology selection is required" IsRequired="true" />
                                </ValidationSettings>
                            </dx:ASPxComboBox>
                        </td>
                        <td>Network Element:
                            <dx:ASPxComboBox ID="cbElement" ClearButton-DisplayMode="OnHover" ClientInstanceName="cbElement" Width="90%" runat="server" ValueType="System.String" ValueField="ID" TextField="Name" AllowNull="true" NullText="not defined" EnableSynchronization="False" OnCallback="cbElement_Callback">
                                <ClearButton DisplayMode="OnHover"></ClearButton>
                            </dx:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">Controller:*
                            <dx:ASPxComboBox ID="cbController" ClearButton-DisplayMode="OnHover" Width="95%" runat="server" ValueType="System.String" AllowNull="true" NullText="not defined" DataSourceID="sqlController" ValueField="ID" TextFormatString="{0} - {1}" AutoPostBack="True" OnSelectedIndexChanged="cbController_SelectedIndexChanged">
                                <Columns>
                                    <dx:ListBoxColumn FieldName="Code"/>
                                    <dx:ListBoxColumn FieldName="Name" Width="300px"/>
                                </Columns>
                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                <ValidationSettings ValidationGroup="Ticket" ErrorTextPosition="Right" ErrorDisplayMode="ImageWithTooltip">
                                    <RequiredField ErrorText="Controller selection is required" IsRequired="true" />
                                </ValidationSettings>
                            </dx:ASPxComboBox>
                        </td>
                        <td>Controller IP Access Information:*
                            <dx:ASPxTextBox ID="txtControllerIP" runat="server" Width="100%">
                            <ValidationSettings ValidationGroup ="Ticket" ErrorTextPosition="Right" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField ErrorText="Controller IP Access Information is required" IsRequired="true" />
                            </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">Sites:
                            <dx:ASPxGridLookup ID="glSites" ClientInstanceName="glSites" ClearButton-DisplayMode="OnHover" DataSourceID="sqlSite" runat="server" AutoGenerateColumns="False" KeyFieldName="ID" NullText="not defined - expand combobox for multiple selection" TextFormatString="{0} - {1}" SelectionMode="Multiple" Width="90%" MultiTextSeparator=" & " >
                                <GridViewProperties>
                                    <Templates>
                                        <StatusBar>
                                            <table style="float: right">
                                                <tr>
                                                    <td>
                                                        <dx:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </StatusBar>
                                    </Templates>
                                    <SettingsBehavior AllowSelectByRowClick="True" AllowDragDrop="false" />
                                    <Settings AutoFilterCondition="Contains" ShowFilterRow="True" ShowStatusBar="Visible"/>
                                    <SettingsPager NumericButtonCount="3" EnableAdaptivity="true" />
                                </GridViewProperties>
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="Code" ShowInCustomizationForm="True" VisibleIndex="1" Width="100" />
                                    <dx:GridViewDataTextColumn FieldName="Name" ShowInCustomizationForm="True" VisibleIndex="2" Width="250" />
                                </Columns>
                                <ClearButton DisplayMode="OnHover"></ClearButton>
                            </dx:ASPxGridLookup>
                        </td>
                        <td>Site IP Access Information:
                            <dx:ASPxTextBox ID="txtSiteIP" runat="server" Width="90%"></dx:ASPxTextBox>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <br />
    <dx:ASPxRoundPanel ID="Panel3" ClientInstanceName="Panel3" runat="server" Width="700pt" HeaderText="Problem Details">
        <PanelCollection>
            <dx:PanelContent runat="server">
                <table style="width:100%">
                    <tr>
                        <td>Severity:*
                            <dx:ASPxComboBox ID="cbSeverity" ClearButton-DisplayMode="OnHover" Width="90%" runat="server" ValueType="System.String" ValueField="ID" TextField="Name" DataSourceID="sqlSeverity">
                            <ValidationSettings ValidationGroup="Ticket" ErrorTextPosition="Right" ErrorDisplayMode="ImageWithTooltip">
                                    <RequiredField ErrorText="Severity selection is required" IsRequired="true" />
                                </ValidationSettings>
                            </dx:ASPxComboBox>
                        </td>
                        <td colspan="2">Problem Title:*
                            <dx:ASPxTextBox ID="tbProblem" runat="server" Width="100%" NullText="mandatory">
                            <ValidationSettings ValidationGroup ="Ticket" ErrorTextPosition="Right" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField ErrorText="Problem Title is required" IsRequired="true" />
                            </ValidationSettings>
                            </dx:ASPxTextBox>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">Problem Description:
                            <dx:ASPxMemo ID="meProblem" runat="server" Height="100px" Width="100%">
                            </dx:ASPxMemo>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center" style="padding-bottom:10px; padding-top:10px;">
                            <dx:ASPxImage ID="ASPxImage1" runat="server" ImageUrl="~/Images/divider.png">
                            </dx:ASPxImage>
                        </td>
                    </tr>
                    <tr>
                        <td>Remedy:
                            <dx:ASPxTextBox ID="tbRemedy" runat="server" Width="90%"></dx:ASPxTextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbRemedy" ErrorMessage="Remedy # is Required" ValidationGroup="Ticket"></asp:RequiredFieldValidator>--%>
                        </td>
                        <td>Software Release:
                            <dx:ASPxTextBox ID="tbRelease" runat="server" Width="90%">
                            </dx:ASPxTextBox>
                        </td>
                        <td>Upload File (images, pdf, txt, excel, word / 5 MB max):
                            <dx:ASPxUploadControl ID="ucFile" ClientInstanceName="ucFile" runat="server" ShowUploadButton="true" UploadButton-Text="click here to upload all files" NullText="you can select multiple files" UploadMode="Auto" Width="100%" OnFilesUploadComplete="ucFile_FilesUploadComplete">
                                <AdvancedModeSettings EnableMultiSelect="true"></AdvancedModeSettings>
                                <ValidationSettings AllowedFileExtensions=".jpg,.jpeg,.png,.tif,.tiff,.gif,.pdf,.txt,.xls,.xlsx,.doc,.docx,.log,.msg,.tmf,.zip,.rar" MaxFileSize="5242880" NotAllowedFileExtensionErrorText="Extensions allowed are: jpg, jpeg, png, tif, tiff, gif, pdf, txt, xls, xlsx, doc, docx, log, msg, .tmf, .zip and .rar" MaxFileSizeErrorText="Maximum allowed size is 5 Mbyte"></ValidationSettings>
                            </dx:ASPxUploadControl>
                        </td>
                    </tr>
                </table>
                <br />
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <br />
    <dx:ASPxButton ID="btOpen" ClientInstanceName="btOpen" runat="server" Text="Open Ticket" ValidationGroup="Ticket" OnClick="btOpen_Click" CausesValidation="True"></dx:ASPxButton>
    <asp:SqlDataSource ID="sqlOEM" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT DISTINCT t2.ID, t2.Name FROM tblOEMBranch t1 INNER JOIN tblOEM t2 ON t2.ID = t1.OEMID WHERE t1.BranchID = @BranchID">
        <SelectParameters>
            <asp:SessionParameter Name="BranchID" SessionField="BranchID" DbType="Int32" DefaultValue="0"  />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlSite" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Code, Name FROM tblSite WHERE BranchID = @BranchID">
        <SelectParameters>
            <asp:SessionParameter Name="BranchID" SessionField="BranchID" DbType="Int32" DefaultValue="0"  />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlController" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Code, Name FROM tblController WHERE BranchID = @BranchID">
        <SelectParameters>
            <asp:SessionParameter Name="BranchID" SessionField="BranchID" DbType="Int32" DefaultValue="0"  />
        </SelectParameters>
    </asp:SqlDataSource>
     <asp:SqlDataSource ID="sqlSeverity" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, Name FROM tblSeverity WHERE CompanyID = @CompanyID">
        <SelectParameters>
            <asp:SessionParameter Name="CompanyID" SessionField="CompanyID" DbType="Int32" DefaultValue="0"  />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlGroup" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT ID, GroupEmail FROM tblGroupEmail WHERE BranchID = @BranchID">
        <SelectParameters>
            <asp:SessionParameter Name="BranchID" SessionField="BranchID" DbType="Int32" DefaultValue="0"  />
        </SelectParameters>
    </asp:SqlDataSource>
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
