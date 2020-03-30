using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ClsQNotifications;

namespace ATCPortal.FieldTechnician
{
    public partial class ForwardTicket : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null || Session["BranchID"] == null)
            {
                ValidateRequest.SetDataSession(User.Identity.Name);
            }

            if (!IsPostBack)
            {
               //get ticket date and time
                deTicketDate.TimeSectionProperties.Visible = true;
                deTicketDate.DisplayFormatString = "g";
                deTicketDate.Date = System.DateTime.Now;
                //get the next ticket number
                string query = "SELECT MAX(ID) + 1 FROM tblTicket";
                DataTable dt = DataBase.GetDT(query);
                string NextTicket = dt.Rows[0][0].ToString();
                tbTicket.Text = NextTicket;
                Session["Ticket"] = NextTicket;
                //get customer and branch
                try
                {
                    query = "SELECT t2.Name + ' - ' + t1.Name FROM tblBranch t1 INNER JOIN tblCompany t2 ON t2.ID = t1.CompanyID WHERE t1.ID = @ID";
                    List<SqlParameter> sp1 = new List<SqlParameter>()
                                {
                                    new SqlParameter() {ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value= Session["BranchID"]}
                                };
                    dt = DataBase.GetDT(sp1, query);
                    tbBranch.Text = dt.Rows[0][0].ToString();

                }
                catch
                {
                    tbBranch.Enabled = true;
                    tbBranch.NullText = "enter your Country here";
                }
                //get Creator
                query = "SELECT Name, Title, WorkPhone, MobilePhone, HasWhatsapp FROM aspnet_Users WHERE UserName = @UserName";
                List<SqlParameter> sp = new List<SqlParameter>
                {
                    new SqlParameter() {ParameterName="@UserName", SqlDbType=SqlDbType.NVarChar, Value=User.Identity.Name}
                };
                dt = DataBase.GetDT(sp, query, "ApplicationServices");
                tbName.Text = dt.Rows[0][0].ToString();
                tbTitle.Text = dt.Rows[0][1].ToString();
                tbEmail.Text = Membership.GetUser().Email;

                tbWork.Text = dt.Rows[0][2].ToString();
                tbMobile.Text = dt.Rows[0][3].ToString();
                chWhats.Checked = dt.Rows[0][4].ToString() == "True" ? true:false;
            }
        }

        protected void cbTech_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string query = "SELECT t2.ID, t2.Name FROM tblOEMBranch t1 INNER JOIN tblTechnology t2 ON t2.ID = t1.TechnologyID WHERE t1.BranchID = @BranchID AND t1.OEMID = @OEMID";
            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="@BranchID", SqlDbType=SqlDbType.Int, Value=Session["BranchID"]},
                new SqlParameter() {ParameterName="@OEMID", SqlDbType=SqlDbType.Int, Value=e.Parameter},
            };
            DataTable dt = DataBase.GetDT(sp, query);
            cbTech.DataSource = dt;
            cbTech.DataBind();
        }

        protected void cbElement_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            int OEMID = Convert.ToInt32(cbOEM.SelectedItem.Value);
            string sql = "SELECT ID, NAME FROM tblNetworkElement WHERE TechID = " + e.Parameter + " AND OEMID = " + OEMID;
            DataTable dt = DataBase.GetDT(sql);
            cbElement.DataSource = dt;
            cbElement.DataBind();
        }

        protected void btOpen_Click(object sender, EventArgs e)
        {
            //Update the contact info on the user
            string sql= "UPDATE aspnet_Users SET WorkPhone = @work, MobilePhone = @mobile, HasWhatsapp = @whats WHERE UserName = @UserName";
            List<SqlParameter> sp = new List<SqlParameter>()
                                {
                                    new SqlParameter() {ParameterName = "@work", SqlDbType = SqlDbType.NVarChar, Value=tbWork.Text},
                                    new SqlParameter() {ParameterName = "@mobile", SqlDbType = SqlDbType.NVarChar, Value=tbMobile.Text},
                                    new SqlParameter() {ParameterName = "@whats", SqlDbType = SqlDbType.Bit, Value=chWhats.Checked},
                                    new SqlParameter() {ParameterName = "@UserName", SqlDbType = SqlDbType.NVarChar, Value = User.Identity.Name}
                                };
            DataBase.UpdateDB(sp, sql, "ApplicationServices");
            //Send email with ticket data
            string test2 = DateTime.Now.Subtract(DateTime.UtcNow).ToString();
            string test3 = test2.Substring(0, test2.IndexOf(":"));
            string msg = @"
<b><h3>TICKET #: " + tbTicket.Text + @"</h3></b>
<b><h3>SEVERITY: " + cbSeverity.Text + @"</h3></b>
Ticket and Contact Details: <br/><br/>
<table style = ""width: 80 %; "" border = ""2"" cellpadding = ""4"">
<tr>
    <td style = ""width: 26 %; "">Creation Date and Time (local)</td>
    <td style = ""width: 73 %;""> " + DateTime.Now.ToLocalTime() + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "">Creation Date and Time (utc)</td>
    <td style = ""width: 73 %;""> " + DateTime.Now.ToUniversalTime() + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "">Creation Time Zone</td>
    <td style = ""width: 73 %;""> " + test3 + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "">Customer - Branch</td>
    <td style = ""width: 73 %; ""> " + tbBranch.Text + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "">Creator Name</td></td>
    <td style = ""width: 73 %; ""> " + tbName.Text + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "" >Creator Title</td>
    <td style = ""width: 73 %; "" > " + tbTitle.Text + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "" >Creator Email</td>
    <td style = ""width: 73 %; "" > " + tbEmail.Text + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "" >Group Emails</td>
    <td style = ""width: 73 %; "" > " + glGroup.Text + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "">Work Phone</td></td>
    <td style = ""width: 73 %; ""> " + tbWork.Text + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "" >Mobile Phone</td>
    <td style = ""width: 73 %; "" > " + tbMobile.Text + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "" >Has WhatsApp</td>
    <td style = ""width: 73 %; "" > " + chWhats.Checked.ToString() + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "" >Additional Contact Instruction</td>
    <td style = ""width: 73 %; "" > " + tbContactInstructions.Text + @" </td>
</tr>

</table> ";
            msg += "<br/>";
            msg += "<br/>";
            msg += "Site Details:<br/><br/>";
            msg += @"
<table style = ""width: 80 %; "" border = ""2"" cellpadding = ""4"">
<tr>
    <td style = ""width: 26 %; "">OEM</td>
    <td style = ""width: 73 %;""> " + cbOEM.SelectedItem.Text + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "">Technology</td>
    <td style = ""width: 73 %; ""> " + cbTech.SelectedItem.Text + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "">Network Elements</td>
    <td style = ""width: 73 %; ""> " + cbElement.Text + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "">Controller</td>
    <td style = ""width: 73 %; ""> " + cbController.SelectedItem.Text + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "" >Controller IP Access Information</td>
    <td style = ""width: 73 %; "" > " + txtControllerIP.Text + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "" >Site</td>
    <td style = ""width: 73 %; "" > " + glSites.Text + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "" >Site IP Access Information</td>
    <td style = ""width: 73 %; "" > " + txtSiteIP.Text + @" </td>
</tr>
</table> ";
            msg += "<br/>";
            msg += "<br/>";
            msg += "Problem Details:<br/><br/>";
            msg += @"
<table style = ""width: 80 %; "" border = ""2"" cellpadding = ""4"">
<tr>
    <td style = ""width: 26 %; "">Problem Title</td>
    <td style = ""width: 73 %;""> " + tbProblem.Text + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "">Problem</td>
    <td style = ""width: 73 %; ""> " + meProblem.Text + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "">Remedy</td>
    <td style = ""width: 73 %; ""> " + tbRemedy.Text + @" </td>
</tr>
<tr>
    <td style = ""width: 26 %; "">Software Release</td>
    <td style = ""width: 73 %; ""> " + tbRelease.Text + @" </td>
</tr>
</table> ";

            //saving ticket to the DB
            sql = @"INSERT INTO tblTicket
(ID, CreationDate,CreationDateUtc,UserName,SeverityID,OEMID,TechnologyID,NetworkElementID,RadioControllerID
,ProblemTitle,ProblemDescription,SoftwareRelease,ContactInstructions,Remedy, SiteIP, ControllerIP, BranchID, Sites)
VALUES
(@ID, @CreationDate,@CreationDateUtc,@UserName,@SeverityID,@OEMID,@TechnologyID,@NetworkElementID,@RadioControllerID,
@ProblemTitle,@ProblemDescription,@SoftwareRelease,@ContactInstructions,@Remedy, @SiteIP, @ControllerIP, @BranchID, @Sites)";
            sp = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName = "@ID", SqlDbType = SqlDbType.BigInt, Value=Session["Ticket"]},
                new SqlParameter() {ParameterName = "@CreationDate", SqlDbType = SqlDbType.DateTime, Value=DateTime.Now.ToLocalTime()},
                new SqlParameter() {ParameterName = "@CreationDateUtc", SqlDbType = SqlDbType.DateTime, Value=DateTime.Now.ToUniversalTime()},
                new SqlParameter() {ParameterName = "@UserName", SqlDbType = SqlDbType.NVarChar, Value=User.Identity.Name},
                new SqlParameter() {ParameterName = "@SeverityID", SqlDbType = SqlDbType.Int, Value=cbSeverity.Value},
                new SqlParameter() {ParameterName = "@OEMID", SqlDbType = SqlDbType.Int, Value=cbOEM.Value},
                new SqlParameter() {ParameterName = "@TechnologyID", SqlDbType = SqlDbType.Int, Value=cbTech.Value},
                new SqlParameter() {ParameterName = "@RadioControllerID", SqlDbType = SqlDbType.Int, Value=cbController.Value},
                new SqlParameter() {ParameterName = "@ProblemTitle", SqlDbType = SqlDbType.NVarChar, Value=tbProblem.Text},
                new SqlParameter() {ParameterName = "@ProblemDescription", SqlDbType = SqlDbType.NVarChar, Value=meProblem.Text},
                new SqlParameter() {ParameterName = "@SoftwareRelease", SqlDbType = SqlDbType.NVarChar, Value=tbRelease.Text},
                new SqlParameter() {ParameterName = "@ContactInstructions", SqlDbType = SqlDbType.NVarChar, Value=tbContactInstructions.Text},
                new SqlParameter() {ParameterName = "@Remedy", SqlDbType = SqlDbType.NVarChar, Value=tbRemedy.Text},
                new SqlParameter() {ParameterName = "@SiteIP", SqlDbType = SqlDbType.NVarChar, Value=txtSiteIP.Text},
                new SqlParameter() {ParameterName = "@ControllerIP", SqlDbType = SqlDbType.NVarChar, Value=txtControllerIP.Text},
                new SqlParameter() {ParameterName = "@BranchID", SqlDbType = SqlDbType.Int, Value=Session["BranchID"]},
                new SqlParameter() {ParameterName = "@Sites", SqlDbType = SqlDbType.NVarChar, Value=glSites.Text}
            };

            //not required comboboxes
            SqlParameter singleparameter = new SqlParameter()
            {
                ParameterName = "@NetworkElementID",
                SqlDbType = SqlDbType.Int,
            };
            if (cbElement.Value == null)
                singleparameter.Value = DBNull.Value;
            else
                singleparameter.Value = cbElement.Value;
            sp.Add(singleparameter);
            
            DataBase.UpdateDB(sp, sql);

            //update the groups
            //go through the list of selected group emails (if any)
            for (int i = 0; i < glGroup.GridView.Selection.Count; i++)
            {
                sql = "INSERT INTO tblGroupSelection (TicketID, GroupID) VALUES (@TicketID, @GroupID)";
                sp = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@TicketID", SqlDbType = SqlDbType.BigInt, Value=Session["Ticket"]},
                    new SqlParameter() {ParameterName = "@GroupID", SqlDbType = SqlDbType.BigInt, Value=glGroup.GridView.GetSelectedFieldValues("ID")[i]}

                };
                DataBase.UpdateDB(sp, sql);
            }

            //update the sites
            //go through the list of selected sites (if any)
            for (int i = 0; i < glSites.GridView.Selection.Count; i++)
            {
                sql = "INSERT INTO tblSiteSelection (TicketID, SiteID) VALUES (@TicketID, @SiteID)";
                sp = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@TicketID", SqlDbType = SqlDbType.BigInt, Value=Session["Ticket"]},
                    new SqlParameter() {ParameterName = "@SiteID", SqlDbType = SqlDbType.BigInt, Value=glSites.GridView.GetSelectedFieldValues("ID")[i]}
                
                };
                DataBase.UpdateDB(sp, sql);
            }

            //Set up title

            string queryCompanyBranch = "SELECT SUBSTRING(t2.Name, 1, 3) as company, case when CHARINDEX('-', t1.Name) > 0 then SUBSTRING(t1.Name, CHARINDEX('-', t1.Name) + 1, 3)";
            queryCompanyBranch += " when len(t1.Name) = 4 then SUBSTRING(t1.Name, 1, 4) else SUBSTRING(t1.Name, 1, 3) end as branch FROM tblBranch t1 INNER JOIN tblCompany t2 ON t2.ID = t1.CompanyID WHERE t1.ID = @ID";
            List<SqlParameter> parCB = new List<SqlParameter>()
                                {
                                    new SqlParameter() {ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value= Session["BranchID"]}
                                };
            DataTable dtCompanyBranch = DataBase.GetDT(parCB, queryCompanyBranch);

            string priority = cbSeverity.Text.Split(' ').Length > 1 ? cbSeverity.Text.Split(' ')[1].Trim() : cbSeverity.Text;
            string title = string.Format("{0} {1} Ticket # {2} {3} {4}", dtCompanyBranch.Rows[0]["company"].ToString().ToUpper(), dtCompanyBranch.Rows[0]["branch"].ToString().ToUpper(), tbTicket.Text, tbProblem.Text, priority);

            //sending to Amerinode internally
            //Notifications.add(title, msg, "q@amerinode.com", "sw_support@amerinode.com", Convert.ToInt64(Session["Ticket"]));
            lblMsg.Text = "Email sent to Amerinode Support at sw_support@amerinode.com for immediate processing.";
            Email.SendEmail(title, msg, "sw_support@amerinode.com");
            //sending to the tiket creator
            //Notifications.add(title, msg, "q@amerinode.com", tbEmail.Text, Convert.ToInt64(Session["Ticket"]));
            Email.SendEmail(title, msg, tbEmail.Text);
            //Sending email to selected groups
            for (int i = 0; i < glGroup.GridView.Selection.Count; i++)
            {
                //Notifications.add(title, msg, "q@amerinode.com", glGroup.GridView.GetSelectedFieldValues("GroupEmail")[i].ToString(), Convert.ToInt64(Session["Ticket"]));
                Email.SendEmail(title, msg, glGroup.GridView.GetSelectedFieldValues("GroupEmail")[i].ToString());
            }
            lblMsg.Text += System.Environment.NewLine + "Q also sent emails to Ticket Creator: " + tbEmail.Text + " and designated groups: " + glGroup.Text;
            popMsg.ShowOnPageLoad = true;
            btOpen.Enabled = false;
        }

        protected void ucFile_FilesUploadComplete(object sender, DevExpress.Web.FilesUploadCompleteEventArgs e)
        {
            string newpath = MapPath("~/Content/Tickets/") + Session["Ticket"].ToString() + "\\";

            if (!Directory.Exists(newpath))
                Directory.CreateDirectory(newpath);

               foreach(DevExpress.Web.UploadedFile file in ucFile.UploadedFiles)
            {
                file.SaveAs(newpath + file.FileName);
            }

            lblMsg.Text = "Files Uploaded Successfully";
            popMsg.ShowOnPageLoad = true;
        }

        protected void cbController_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "SELECT IPAddress FROM tblController WHERE ID = " + cbController.SelectedItem.Value;
            DataTable dt = DataBase.GetDT(sql);
            try
            {
                txtControllerIP.Text = dt.Rows[0][0].ToString();
            }
            catch { }
        }
    }
}