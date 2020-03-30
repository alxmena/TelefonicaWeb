<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QTicketImages.aspx.cs" Inherits="ATCPortal.Amerinode.QTicketImages" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label runat="server" ID="lblResult"></asp:Label>
            <dx:ASPxFileManager ID="fmTicket" runat="server" SettingsEditing-AllowDownload="true" SettingsEditing-AllowRename="true">
                <Settings RootFolder="~/Content/Q/" ThumbnailFolder="~/Thumb/" />
                <SettingsUpload Enabled="False">
                </SettingsUpload>
            </dx:ASPxFileManager>
        </div>
    </form>
</body>
</html>
