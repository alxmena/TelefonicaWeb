<%@ Page Language="C#" %>

<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8" />
    <title></title>    
</head>
<body>
    <form id="form1" runat="server"> 
        
        <dx:ASPxGridLookup ID="ASPxGridLookup1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" KeyFieldName="ID" Theme="Glass" Height="35px" ForeColor="Black">
<GridViewProperties>
<SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True"></SettingsBehavior>

<SettingsPopup EditForm-MinHeight="40" HeaderFilter-Height="40" HeaderFilter-MinHeight="40">
<EditForm MinHeight="40px"></EditForm>

<HeaderFilter MinHeight="140px"></HeaderFilter>
</SettingsPopup>
</GridViewProperties>
            <Columns>
                <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="0">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="CreationDate" VisibleIndex="1">
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataDateColumn FieldName="ResponseDate" VisibleIndex="2">
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataDateColumn FieldName="RestorationDate" VisibleIndex="3">
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataDateColumn FieldName="ResolutionDate" VisibleIndex="4">
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataDateColumn FieldName="ClosureDate" VisibleIndex="5">
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="UserName" VisibleIndex="6">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SeverityID" VisibleIndex="7">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="OEMID" VisibleIndex="8">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="TechnologyID" VisibleIndex="9">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="NetworkElementID" VisibleIndex="10">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="RadioControllerID" VisibleIndex="11">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="StatusID" VisibleIndex="12">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ProblemTitle" VisibleIndex="13">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ProblemDescription" VisibleIndex="14">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SoftwareRelease" VisibleIndex="15">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataCheckColumn FieldName="FormalAnswerAccepted" VisibleIndex="16">
                </dx:GridViewDataCheckColumn>
                <dx:GridViewDataTextColumn FieldName="ContactInstructions" VisibleIndex="17">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Remedy" VisibleIndex="18">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="CreationDateUtc" VisibleIndex="19">
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="BranchID" VisibleIndex="20">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SiteIP" VisibleIndex="21">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ControllerIP" VisibleIndex="22">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Sites" VisibleIndex="23">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ResponseNote" VisibleIndex="24">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="RestorationNote" VisibleIndex="25">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ResolutionNote" VisibleIndex="26">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ClosureNote" VisibleIndex="27">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ResponseBy" VisibleIndex="28">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="RestorationBy" VisibleIndex="29">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ResolutionBy" VisibleIndex="30">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ClosureBy" VisibleIndex="31">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="RejectNote" VisibleIndex="32">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="RejectAceptBy" VisibleIndex="33">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SolutionTypeID" VisibleIndex="34">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="QuestionUI" VisibleIndex="35">
                </dx:GridViewDataTextColumn>
            </Columns>
            <DropDownWindowStyle Font-Bold="True">
            </DropDownWindowStyle>
            <gridviewstyles>
                <header font-size="Large" font-bold="false" ForeColor="Blue" >
                </header>
                <HeaderFilterItem Font-Bold="True" ForeColor="Black">
                </HeaderFilterItem>
                <SearchPanel Font-Bold="True" ForeColor="Black">
                </SearchPanel>
            </gridviewstyles>
            <GridViewStylesPopup>
                <CustomizationDialog>
                    <PopupControl>
                    </PopupControl>
                </CustomizationDialog>
            </GridViewStylesPopup>
        </dx:ASPxGridLookup>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnection %>" SelectCommand="SELECT * FROM [tblTicket]"></asp:SqlDataSource>
        
    </form>
</body>
</html>
