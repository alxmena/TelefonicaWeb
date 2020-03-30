<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Questions.aspx.cs" Inherits="ATCPortal.Questios.Questions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <dx:ASPxPanel ID="pnlContentQuestions" runat="server">
        <PanelCollection>
            <dx:PanelContent>
                <div style="max-width: 700px; margin: 0 auto;">
                    <div class="text-center" style="margin-bottom: 20px;">
                        <dx:ASPxLabel Width="40%" runat="server" ID="lblTitlePage" Text="Satisfaction Survey" Font-Size="X-Large" Font-Bold="true"></dx:ASPxLabel>
                    </div>
                    <asp:PlaceHolder ID="phContent" runat="server"></asp:PlaceHolder>
                    <dx:ASPxButton ID="btnSaveQuestions" runat="server" Text="Save Questions" OnClick="btnSaveQuestions_Click"></dx:ASPxButton>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>
    <dx:ASPxPopupControl ID="popMsg" ClientInstanceName="popMsg" runat="server" CloseOnEscape="True" Modal="True" AllowDragging="True" PopupAnimationType="Fade" PopupVerticalAlign="WindowCenter" EnableViewState="False" CssClass="auto-style7" HeaderText="Q MESSAGE" PopupHorizontalAlign="WindowCenter" Width="500px">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxLabel ID ="lblMsg" runat="server" Text="ASPxLabel"></dx:ASPxLabel><br /><br />
                <dx:ASPxButton ID="btnOk" runat="server" AutoPostBack="True" Text="Ok" Width="80px" OnClick="btnOk_Click"></dx:ASPxButton>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
