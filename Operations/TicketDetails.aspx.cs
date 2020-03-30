using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace ATCPortal.Amerinode
{
    public partial class TicketDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null || Session["BranchID"] == null)
            {
                ValidateRequest.SetDataSession(User.Identity.Name);
            }

            //if (!Page.IsCallback && !Page.IsPostBack)
            //{
                if (Session["CompanyID"].ToString() != "7") //not Amerinode User
                    sqlTickets.SelectCommand += " WHERE BranchID = " + Session["BranchID"] + " ORDER BY ID DESC";
                else
                    sqlTickets.SelectCommand += " ORDER BY ID DESC";
            //}
        }

         protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(gvTickets.Text))
            {
                lblMsg.Text = "Choose Ticket first.";
                popMsg.ShowOnPageLoad = true;
            } 
            else
            {
                Panel1.Visible = true;
                Panel2.Visible = true;
                Panel3.Visible = true;
                Panel4.Visible = true;
                PerformUpdates();
                Session["ticket"] = gvTickets.Text;
                ActiveUpdate();
                UpdatePendingInfo();
                Status_tck.Text = "Status: " + flDetails_E11.Text;
                if (flDetails_E11.Text == "Pending Info")
                    Status_tck.ForeColor = Color.Red;
                else
                    Status_tck.ForeColor = Color.Lime;
            }
            
        }

        protected void ucFile_FilesUploadComplete(object sender, DevExpress.Web.FilesUploadCompleteEventArgs e)
        {
            string newpath = MapPath("~/Content/Tickets/") + Session["ticket"] + "\\";

            if (!Directory.Exists(newpath))
                Directory.CreateDirectory(newpath);

            foreach (DevExpress.Web.UploadedFile file in ucFile.UploadedFiles)
            {
                file.SaveAs(newpath + file.FileName);
            }

            lblMsg.Text = "Files Uploaded Successfully";
            popMsg.ShowOnPageLoad = true;

            SetRoot();
        }

        protected void gvTickets_TextChanged(object sender, EventArgs e)
        {
            Panel1.Visible = false;
            Panel2.Visible = false;
            Panel3.Visible = false;
            Panel4.Visible = false;
            Panel1.Collapsed = true;
            Panel2.Collapsed = true;
            Panel3.Collapsed = true;
            Panel4.Collapsed = true;           

        }

        private void PerformUpdates()
        {
            //update form layout details with ticke info
            flDetails.DataBind();
            //update file manager
            SetRoot();
            //get all dates and notes
            string sql = "SELECT ResponseDate, ResponseNote, RestorationDate, RestorationNote, ResolutionDate, ResolutionNote, ClosureDate, ClosureNote, ResponseBy, RestorationBy, ResolutionBy, ClosureBy from tblTicket WHERE ID = " + gvTickets.Text;
            DataTable dt = DataBase.GetDT(sql);
            if (dt.Rows[0][0].ToString() != "")
                deRespDate.Date = DateTime.Parse(dt.Rows[0][0].ToString());
            else
                deRespDate.Text = "";
            lblRespNote.Text = dt.Rows[0][1].ToString().Replace("\n", "<br/>");
            if (dt.Rows[0][2].ToString() != "")
                deRestDate.Date = DateTime.Parse(dt.Rows[0][2].ToString());
            else
                deRestDate.Text = "";
            lblRestNote.Text = dt.Rows[0][3].ToString().Replace("\n", "<br/>");
            if (dt.Rows[0][4].ToString() != "")
                deResoDate.Date = DateTime.Parse(dt.Rows[0][4].ToString());
            else
                deResoDate.Text = "";
            lblResoNote.Text = dt.Rows[0][5].ToString().Replace("\n", "<br/>");
            if (dt.Rows[0][6].ToString() != "")
                deClosDate.Date = DateTime.Parse(dt.Rows[0][6].ToString());
            else
                deClosDate.Text = "";
            lblClosNote.Text = dt.Rows[0][7].ToString().Replace("\n", "<br/>");
            lbRest.Text = dt.Rows[0][8].ToString();
            lbResp.Text = dt.Rows[0][9].ToString();
            lbReso.Text = dt.Rows[0][10].ToString();
            lbClos.Text = dt.Rows[0][11].ToString();
        }

        private void SetRoot()
        {
            string newpath = MapPath("~/Content/Tickets/") + gvTickets.Text + "\\";
            if (!Directory.Exists(newpath))
                fmCustomer.Visible = false;
            else
                fmCustomer.Settings.RootFolder = newpath;
            fmCustomer.Refresh();
        }

        private void ActiveUpdate()
        {
            //If user is manager, it can edit all records, otherwise only your own ticket
            string sql = "SELECT IsManager from aspnet_Users WHERE UserName = @UserName";
            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="UserName", SqlDbType = SqlDbType.NVarChar, Value=User.Identity.Name }
            };
            bool IsManager = bool.Parse(DataBase.GetDT(sp, sql, "ApplicationServices").Rows[0][0].ToString());
            bool IsOwner = false;
            DevExpress.Web.LayoutItem userField = flDetails.FindItemByFieldName("UserName");
            string userCreate = string.Empty;
            foreach(var ctl in userField.Controls)
            {
                if (ctl is ASPxTextBox)
                {
                    userCreate = (ctl as ASPxTextBox).Text;
                }
            }
            if (User.Identity.Name == userCreate)
                IsOwner = true;

            if (IsManager || IsOwner)
            {
                flDetails.Enabled = true;
                btnUpdate.Visible = true;
            }
        }

        private void setEnableEdit()
        {
            //SeverityID
           // OEMID
        }

        protected void cbOem_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "SELECT t2.ID, t2.Name FROM tblOEMBranch t1 INNER JOIN tblTechnology t2 ON t2.ID = t1.TechnologyID WHERE t1.BranchID = " + Session["BranchID"].ToString() + " AND t1.OEMID = " + cbOem.SelectedItem.Value;
            sqlTechnology.SelectCommand = sql;
            sqlTechnology.DataBind();
            cbTechnology.SelectedIndex = -1;
            cbNetwork.Items.Clear();
            cbNetwork.SelectedIndex = -1;
        }

        protected void cbTechnology_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "SELECT ID, NAME FROM tblNetworkElement WHERE TechID = " + cbTechnology.SelectedItem.Value;
            sqlNetwork.SelectCommand = sql;
            cbNetwork.DataBind();
            cbNetwork.SelectedIndex = -1;
        }

        protected void cbRadioController_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "SELECT IPAddress FROM tblController WHERE ID = " + cbRadioController.SelectedItem.Value;
            DataTable dt = DataBase.GetDT(sql);
            txtControllerIP.Text = dt.Rows[0][0].ToString();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (validateChangeSeverity())
            {
                string sql = "UPDATE tblTicket SET SeverityID = @p1, OEMID=@p2, TechnologyID = @p3, NetworkElementID = @p4, RadioControllerID = @p5,  ProblemTitle = @p6, ProblemDescription = @p7, SoftwareRelease = @p8, ContactInstructions = @p9, Remedy = @p10, SiteIP = @p11, ControllerIP = @p12 WHERE ID = @ID";
                List<SqlParameter> sp = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@p1", SqlDbType = SqlDbType.Int, Value = cbSeverity.SelectedItem.Value},
                    new SqlParameter() {ParameterName = "@p2", SqlDbType = SqlDbType.Int, Value = cbOem.SelectedItem.Value},
                    new SqlParameter() {ParameterName = "@p3", SqlDbType = SqlDbType.Int, Value = cbTechnology.SelectedItem.Value},
                    new SqlParameter() {ParameterName = "@p4", SqlDbType = SqlDbType.Int, IsNullable = true, Value = cbNetwork.SelectedItem.Value ?? DBNull.Value},
                    new SqlParameter() {ParameterName = "@p5", SqlDbType = SqlDbType.Int, Value = cbRadioController.SelectedItem.Value},
                    new SqlParameter() {ParameterName = "@p6", SqlDbType = SqlDbType.NVarChar, Value = txtProblemTitle.Text},
                    new SqlParameter() {ParameterName = "@p7", SqlDbType = SqlDbType.NVarChar, Value = memoProblemDescription.Text},
                    new SqlParameter() {ParameterName = "@p8", SqlDbType = SqlDbType.NVarChar, Value = txtSoftwareRelease.Text},
                    new SqlParameter() {ParameterName = "@p9", SqlDbType = SqlDbType.NVarChar, Value = txtContactInstructions.Text},
                    new SqlParameter() {ParameterName = "@p10", SqlDbType = SqlDbType.NVarChar, Value = txtRemedy.Text},
                    new SqlParameter() {ParameterName = "@p11", SqlDbType = SqlDbType.NVarChar, Value = txtSiteIP.Text},
                    new SqlParameter() {ParameterName = "@p12", SqlDbType = SqlDbType.NVarChar, Value = txtControllerIP.Text},
                    new SqlParameter() {ParameterName = "@ID", SqlDbType = SqlDbType.NVarChar, Value = Session["ticket"]}
                };
                int aux = DataBase.UpdateDB(sp, sql);
                if (aux == 1)
                    lblMsg.Text = "Update Successful";
                else if (aux == 0)
                    lblMsg.Text = "No Record was updated";
            }
            else
            {
                lblMsg.Text = "Severity cannot be less than current";
            }

            popMsg.ShowOnPageLoad = true;
        }

        private bool validateChangeSeverity()
        {
            bool flag = true;
            string query = "select SeverityID from tblTicket where ID = @ID";
            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter(){ParameterName = "@ID", SqlDbType = SqlDbType.NVarChar, Value = Session["ticket"]}
            };

            DataTable dt = DataBase.GetDT(sp, query);
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(cbSeverity.SelectedItem.Value) < Convert.ToInt32(dt.Rows[0]["SeverityID"]))
                {
                    flag = false;
                }
            }

            return flag;
        }

        private void UpdatePendingInfo()
        {
            string query = "SELECT * FROM tblPendingInfo WHERE TicketID = @TicketID";
            TimeSpan acumTotal;
            StringBuilder logs;
            DataRow[] info = null;
            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@TicketID", SqlDbType = SqlDbType.Int, Value = Session["ticket"] }
            };
            DataTable pendingInfo = DataBase.GetDT(sp, query);
            
            #region update time status
            int statusTicket = Convert.ToInt32(Session["_StatusID"]);

            for (int i = 2; i < 6; i++)
            {
                DateTime currentDate = DateTime.Now;
                switch (i)
                {
                    #region Open Created
                    case 1:
                    case 2:
                        // Open - Created
                        acumTotal = new TimeSpan();
                        logs = new StringBuilder();
                        info = pendingInfo.Select("PreviousStatus = 1 OR PreviousStatus = 2");
                        if (info.Length > 0)
                        {
                            foreach (DataRow dr in info)
                            {
                                logs.Append(string.Format("Place Ticket Timer on hold at {0} by {1} \n", dr["PendingInfoOn"].ToString(), dr["UsernameOn"].ToString()));
                                logs.Append("Detail:\n" + dr["Detail"].ToString() + "\n");
                                if (!string.IsNullOrEmpty(dr["PendingInfoOff"].ToString()))
                                {
                                    logs.Append(string.Format("\nRemove Ticket Timer from hold at {0} by {1} \n", dr["PendingInfoOff"].ToString(), dr["UsernameOff"].ToString()));
                                    logs.Append("Detail:\n" + dr["DetailInfoOff"].ToString());
                                    TimeSpan time = DateTime.Parse(dr["PendingInfoOff"].ToString()) - DateTime.Parse(dr["PendingInfoOn"].ToString());
                                    logs.Append("\nTime total hold " + time.ToString(@"dd\.hh\:mm\:ss") + "\n");
                                    acumTotal += time;
                                    logs.Append("-------------------------------------------------------------\n\n");
                                }
                            }
                            lblRespPending.Text = logs.ToString().Replace("\n", "<br/>"); ;
                        }
                        break;
                    #endregion
                    #region Restored
                    case 3:
                        // Restored
                        acumTotal = new TimeSpan();
                        logs = new StringBuilder();
                        info = pendingInfo.Select("PreviousStatus = 3");
                        if (info.Length > 0)
                        {
                            foreach (DataRow dr in info)
                            {
                                logs.Append(string.Format("Place Ticket Timer on hold at {0} by {1} \n", dr["PendingInfoOn"].ToString(), dr["UsernameOn"].ToString()));
                                logs.Append("Detail:\n" + dr["Detail"].ToString() + "\n");
                                if (!string.IsNullOrEmpty(dr["PendingInfoOff"].ToString()))
                                {
                                    logs.Append(string.Format("\nRemove Ticket Timer from hold at {0} by {1} \n", dr["PendingInfoOff"].ToString(), dr["UsernameOff"].ToString()));
                                    logs.Append("Detail:\n" + dr["DetailInfoOff"].ToString());
                                    TimeSpan time = DateTime.Parse(dr["PendingInfoOff"].ToString()) - DateTime.Parse(dr["PendingInfoOn"].ToString());
                                    logs.Append("\nTime total hold " + time.ToString(@"dd\.hh\:mm\:ss") + "\n");
                                    acumTotal += time;
                                    logs.Append("-------------------------------------------------------------\n\n");
                                }
                            }
                            lblRestPending.Text = logs.ToString().Replace("\n", "<br/>"); ;
                        }
                        break;
                    #endregion
                    #region Resolved
                    case 4:
                        // Resolved
                        acumTotal = new TimeSpan();
                        logs = new StringBuilder();
                        info = pendingInfo.Select("PreviousStatus = 4");
                        if (info.Length > 0)
                        {
                            foreach (DataRow dr in info)
                            {
                                logs.Append(string.Format("Place Ticket Timer on hold at {0} by {1} \n", dr["PendingInfoOn"].ToString(), dr["UsernameOn"].ToString()));
                                logs.Append("Detail:\n" + dr["Detail"].ToString() + "\n");
                                if (!string.IsNullOrEmpty(dr["PendingInfoOff"].ToString()))
                                {
                                    logs.Append(string.Format("\nRemove Ticket Timer from hold at {0} by {1} \n", dr["PendingInfoOff"].ToString(), dr["UsernameOff"].ToString()));
                                    logs.Append("Detail:\n" + dr["DetailInfoOff"].ToString());
                                    TimeSpan time = DateTime.Parse(dr["PendingInfoOff"].ToString()) - DateTime.Parse(dr["PendingInfoOn"].ToString());
                                    logs.Append("\nTime total hold " + time.ToString(@"dd\.hh\:mm\:ss") + "\n");
                                    acumTotal += time;
                                    logs.Append("-------------------------------------------------------------\n\n");
                                }
                            }
                            lblResoPending.Text = logs.ToString().Replace("\n", "<br/>"); ;
                        }
                        break;
                    #endregion
                    case 5:
                        // Closed
                        break;
                    case 8:
                    // Canceled
                    default:
                        break;
                }
            }
            #endregion
        }
        
    }
}