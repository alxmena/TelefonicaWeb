<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ManageQuestions.aspx.cs" Inherits="ATCPortal.Questions.ManageQuestions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <dx:ASPxGridView ID="gvQuestions" runat="server" ClientInstanceName="gvQuestions" AutoGenerateColumns="False" KeyFieldName="ID" Theme="Moderno" EnableTheming="True" DataSourceID="SqlQuestions" Width="100%" OnInitNewRow="gvQuestions_InitNewRow" OnRowDeleting="gvQuestions_RowDeleting">
        <Columns>
            <dx:GridViewCommandColumn ShowDeleteButton="True" ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0"></dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" Visible="false" VisibleIndex="4">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Detail" VisibleIndex="1">
                <PropertiesTextEdit>
                    <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                        <RequiredField IsRequired="True"></RequiredField>
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Options" VisibleIndex="2">
                <PropertiesTextEdit>
                    <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                        <RequiredField IsRequired="True"></RequiredField>
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataComboBoxColumn FieldName="TypeInput" VisibleIndex="3">
                <PropertiesComboBox>
                    <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic">
                        <RequiredField IsRequired="True"></RequiredField>
                    </ValidationSettings>
                    <Items>
                        <dx:ListEditItem Text="TEXT" Value="TEXT" />
                        <dx:ListEditItem Text="RADIOBUTTON" Value="RADIOBUTTON" Selected="true"/>
                    </Items>
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
        </Columns>
    </dx:ASPxGridView>
    <asp:SqlDataSource 
        ID="SqlQuestions" 
        runat="server" 
        ConnectionString="<%$ ConnectionStrings:DBConnection %>" 
        SelectCommand="SELECT * FROM [tblQuestions]" 
        UpdateCommandType="Text" 
        InsertCommandType="Text" 
        UpdateCommand="UPDATE tblQuestions set Detail=@Detail, @TypeInput=TypeInput, Options=@Options WHERE ID=@ID" 
        InsertCommand="INSERT INTO tblQuestions VALUES(@Detail, @TypeInput, @Options)">
        <UpdateParameters>
            <asp:Parameter Name="Detail" DbType="String" />
            <asp:Parameter Name="TypeInput" DbType="String" />
            <asp:Parameter Name="Options" DbType="Int32" />
            <asp:Parameter Name="ID" DbType="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="Detail" DbType="String" />
            <asp:Parameter Name="TypeInput" DbType="String" />
            <asp:Parameter Name="Options" DbType="Int32" />
        </InsertParameters>
    </asp:SqlDataSource>
    <dx:ASPxPopupControl ID="popMsg" ClientInstanceName="popMsg" runat="server" CloseOnEscape="True" Modal="True" AllowDragging="True" PopupAnimationType="Fade" PopupVerticalAlign="WindowCenter" EnableViewState="False" CssClass="auto-style7" HeaderText="Q MESSAGE" PopupHorizontalAlign="WindowCenter" Width="500px">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxLabel ID ="lblMsg" ClientInstanceName="lblMsg" runat="server" Text="ASPxLabel"></dx:ASPxLabel><br /><br />
                <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False" ClientInstanceName="btnCancel" Text="Ok" Width="80px">
                    <ClientSideEvents Click="function(s, e) {popMsg.Hide();}" />
                </dx:ASPxButton>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
