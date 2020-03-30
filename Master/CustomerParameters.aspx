<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="CustomerParameters.aspx.cs" Inherits="ATCPortal.Master.CustomerLogo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <dx:ASPxRoundPanel ID="Panel1" runat="server" Width="100%" ShowHeader="False" Theme="Mulberry">
        <PanelCollection>
            <dx:PanelContent>
             Select the Customer:<br />
            <dx:ASPxComboBox ID="cmbCustomer" runat="server" ValueType="System.Int32" Width="400px" DataSourceID="sqlCustomer" EnableTheming="True" Theme="Mulberry" ValueField="ID" TextFormatString="{0}">
                <Columns>
                    <dx:ListBoxColumn FieldName="Customer" Width="220px">
                    </dx:ListBoxColumn>
                    <dx:ListBoxColumn FieldName="Branch">
                    </dx:ListBoxColumn>
                </Columns>
            </dx:ASPxComboBox>
            <dx:ASPxButton ID="btnNext1" runat="server" Text="Next Step" OnClick="btnNext1_Click" Theme="Mulberry"></dx:ASPxButton>
            <asp:SqlDataSource ID="sqlCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT tblCompany.Name AS Customer, tblBranch.Name AS Branch, tblCompany.ID FROM tblCompany INNER JOIN tblBranch ON tblBranch.CompanyID = tblCompany.ID"></asp:SqlDataSource>
            </dx:PanelContent>
        </PanelCollection>    
    </dx:ASPxRoundPanel>
    <br />
    <br />
    <dx:ASPxRoundPanel ID="Panel2" runat="server" Width="100%" Visible="false" ShowHeader="False" Theme="Mulberry">
        <PanelCollection>
            <dx:PanelContent>
            Upload Customer Logo:<br />
            <dx:ASPxBinaryImage ID="ASPxBinaryImage1" runat="server" ToolTip="Maximum Image size is 50kb" EnableServerResize="True" Height="90px" ShowLoadingImage="True" Theme="Mulberry" Width="200px" OnDataBound="ASPxBinaryImage1_DataBound">
                <EditingSettings Enabled="true">
                        <UploadSettings>
                            <UploadValidationSettings MaxFileSize="50000"></UploadValidationSettings>
                        </UploadSettings>
                    </EditingSettings>
            </dx:ASPxBinaryImage>
            <dx:ASPxButton ID="btnSave" runat="server" Text="Upload Image" OnClick="ASPxButton1_Click" Theme="Mulberry"></dx:ASPxButton>
            <dx:ASPxButton ID="btnBack" runat="server" Text="Back" Theme="Mulberry" OnClick="btnBack_Click"></dx:ASPxButton>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <dx:ASPxPopupControl ID="pupMessage" runat="server" CloseOnEscape="True" ShowHeader="False" Theme="Mulberry" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        
        <ContentCollection>
<dx:PopupControlContentControl runat="server">
    <dx:ASPxLabel ID="lblMessage" runat="server" Text="ASPxLabel">
    </dx:ASPxLabel>
            </dx:PopupControlContentControl>
</ContentCollection>
        
    </dx:ASPxPopupControl>
</asp:Content>
