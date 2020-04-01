using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClsQNotifications;
using System.Web.Security;
using System.Drawing;

namespace ATCPortal.Amerinode
{
    public partial class UpdateTicket : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["utTblTicket"] = null;
            }
        }

        private void loadTblTicket()
        {
            DataTable dt = new DataTable();
            if (Session["utTblTicket"] == null)
            {
                string query = "SELECT [ID], [CreationDate], [SeverityID], [OEMID], [TechnologyID], [NetworkElementID], [ProblemTitle], [StatusID], BranchID FROM [tblTicket]";
                dt = DataBase.GetDT(query);
                Session["utTblTicket"] = dt;
            }
            else
            {
                dt = Session["utTblTicket"] as DataTable;
            }
            gvTickets.DataSource = dt;
            gvTickets.DataBind();
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)  //Details
        {
            if (string.IsNullOrEmpty(gvTickets.Text))
            {
                lblMsg.Text = "Choose Ticket first.";
                popMsg.ShowOnPageLoad = true;
            }
            else
            {
                //Panel0.Visible = true;
                Panel1.Visible = true;
                Panel2.Visible = true;
                Panel3.Visible = true;
                Panel4.Visible = true;
                Session["ticket"] = gvTickets.Text;
                //Update the Pending Info Panel
                string sql = "SELECT t2.Name, t2.ID, t1.BranchID FROM tblTicket t1 INNER JOIN tblStatus t2 ON t2.ID = t1.StatusID WHERE t1.ID = " + gvTickets.Text;
                DataTable dtStatus = DataBase.GetDT(sql);
                string query = "SELECT * FROM tblSlaInfo WHERE SlaID = (SELECT SlaID FROM tblBranch WHERE ID = " + dtStatus.Rows[0][2].ToString() + ") ORDER BY Name";
                Session["SlaInfo"] = DataBase.GetDT(query);
                updateStatusTicket(Convert.ToInt32(dtStatus.Rows[0][1]));
                PerformUpdates(0);
                UpdatePendingInfo();

                //AM - Code For Label Status_tck


            }
        }

        protected void ucFile_FilesUploadComplete(object sender, DevExpress.Web.FilesUploadCompleteEventArgs e)
        {
            string newpath = MapPath("~/Content/Tickets/") + Session["ticket"] + "\\Resolution\\";

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
            //Panel0.Visible = false;
            Panel1.Visible = false;
            Panel2.Visible = false;
            Panel3.Visible = false;
            Panel4.Visible = false;
            //Panel0.Collapsed = true;
            Panel1.Collapsed = true;
            Panel2.Collapsed = true;
            Panel3.Collapsed = true;
            Panel4.Collapsed = true;
            //ASPxButton1_Click(null, null);
            //ASPxButton1_Click(null, null);
        }

        private void PerformUpdates(int X)
        {
            
            //update form layout details with ticket info
            flDetails.DataBind();
            Session["ResponseDate"] = null;
            Session["RestorationDate"] = null;
            Session["ClosureDate"] = null;
            Session["HoldTime"] = false;
            Session["SolutionTypeID"] = null;
            Session["RejectAceptBy"] = null;
            Session["LogsOpen"] = null;
            Session["LogsRestored"] = null;
            //Session["PreviousStatus"] = null;
            //update file manager
            SetRoot();
            memoRest.Text = "";
            meRespNote.Enabled = true;

            
            //get all dates and notes
            string sql = "SELECT ResponseDate, ResponseNote, RestorationDate, RestorationNote, ResolutionDate, ResolutionNote, ClosureDate, ClosureNote, ResponseBy, RestorationBy, ResolutionBy, ClosureBy, CreationDate, RejectNote, RejectAceptBy, SolutionTypeID from tblTicket WHERE ID = " + gvTickets.Text;

            DataTable dt = DataBase.GetDT(sql);
            Session["CreationDate"] = dt.Rows[0][12].ToString();
            Session["RejectAceptBy"] = dt.Rows[0][14].ToString();
            String aa = Session["_StatusID"].ToString();
            //String bb = Session["PreviousStatus"].ToString();
            pnlReject.Visible = false;
            if (Session["_StatusID"].ToString() != "7")  //  7. Status Pending Info
            {
                if (Session["_StatusID"].ToString() == "6")  //6. Status Rejected
                {
                    pnlReject.Visible = true;
                    memoRejectNote.Enabled = false;
                    rbOpen.Enabled = false;
                    rbReject.Enabled = false;
                    rbOpen.Checked = false;
                    rbReject.Checked = true;
                    btnSaveStatus.Enabled = false;
                    memoRejectNote.Text = dt.Rows[0][13].ToString();
                }
                else if (Convert.ToInt32(Session["_StatusID"]) >= 1 && Convert.ToInt32(Session["_StatusID"]) != 6) // Status Different Created or Rejected
                {
                    
                    if (Session["RejectAceptBy"].ToString() != "")
                    {
                        rbOpen.Enabled = false;
                        rbReject.Enabled = false;
                        btnSaveStatus.Enabled = false;
                        btClose.Enabled = true;
                        btSaveData.Enabled = true;
                        deRespDate.Enabled = true;
                    }
                    else
                    {
                        rbOpen.Enabled = true;
                        rbReject.Enabled = true;
                        btnSaveStatus.Enabled = true;
                        btClose.Enabled = false;
                        btSaveData.Enabled = false;
                        deRespDate.Enabled = false;
                    }
                    
                    btnToPendingRest.Enabled = false; //Am Disable To and From Pending
                    btnFromPendingRest.Enabled = false;
                    deDateRest.Enabled = false;
                    deDateRest.Value = null;
                    pnlSolution.Visible = false;
                    btClose.Text = "Close Status";
                    
                }

                lblRejectBy.Text = dt.Rows[0][14].ToString();

                if (Convert.ToInt32(Session["_StatusID"]) == 1)
                {
                    Status_tck.Text = "Status: Created";
                    Status_tck.ForeColor = Color.Lime;
                    deRespDate.Visible = true;
                    lbRespDateTime.Visible = true;
                    deRestDate.Visible = false;
                    lbRestDateTime.Visible = false;
                    deResoDate.Visible = false;
                    lbResoDateTime.Visible = false;
                    deClosDate.Visible = false;
                    lbCloseDateTime.Visible = false;
                    lbMsg.Text = "Enter Response Note:";
                    pnlTickRest.Visible = false;
                    pnlTickReso.Visible = false;
                    btSaveData.Enabled = false;
                }
                if (Convert.ToInt32(Session["_StatusID"]) == 2)
                {
                    Status_tck.Text = "Status: Open";
                    Status_tck.ForeColor = Color.Lime;
                    deRespDate.Visible = true; //Date Time SLA
                    deRespDate.Enabled = false;
                    lbRespDateTime.Visible = true;
                    deRestDate.Visible = true;
                    deRestDate.Enabled = true;
                    lbRestDateTime.Visible = true;
                    deResoDate.Visible = false;
                    deResoDate.Enabled = false;
                    lbResoDateTime.Visible = false;                    
                    deClosDate.Visible = false;
                    deClosDate.Enabled = false;
                    lbCloseDateTime.Visible = false;
                    btnToPendingRest.Enabled = true; //Enable To Pending    
                    deDateRest.Enabled = true;
                    lbMsg.Text = "Enter Restoration Note:";
                    pnlTickRest.Visible = true;
                    pnlTickReso.Visible = false;
                }
                if (Convert.ToInt32(Session["_StatusID"]) == 3)
                {
                    Status_tck.Text = "Status: Restored";
                    Status_tck.ForeColor = Color.Lime;
                    deRespDate.Visible = true; //Date Time SLA
                    deRespDate.Enabled = false;
                    lbRespDateTime.Visible = true;                    
                    deRestDate.Visible = true;
                    deRestDate.Enabled = false;
                    lbRestDateTime.Visible = true;
                    deResoDate.Enabled = true;
                    deResoDate.Visible = true;
                    lbResoDateTime.Visible = true;
                    deClosDate.Visible = false;
                    deClosDate.Enabled = false;
                    lbCloseDateTime.Visible = false;
                    btnToPendingRest.Enabled = true; //AM Enable To Pending
                    deDateRest.Enabled = true;
                    lbMsg.Text = "Enter Resolution Note:";
                    pnlTickRest.Visible = true;
                    pnlTickReso.Visible = true;
                }
                if (Convert.ToInt32(Session["_StatusID"]) == 4)
                {
                    Status_tck.Text = "Status: Resolved";
                    Status_tck.ForeColor = Color.Lime;
                    deRespDate.Visible = true; //AM Date Time SLA
                    deRespDate.Enabled = false;
                    lbRespDateTime.Visible = true;
                    deRestDate.Visible = true;
                    deRestDate.Enabled = false;
                    lbRestDateTime.Visible = true;
                    deResoDate.Visible = true;
                    deResoDate.Enabled = false;
                    lbResoDateTime.Visible = true;
                    deClosDate.Visible = true;
                    deClosDate.Enabled = true;
                    lbCloseDateTime.Visible = true;
                    pnlTickRest.Visible = true;
                    pnlTickReso.Visible = true;
                    lbMsg.Text = "Enter Closure Note:";
                    btClose.Text = "Close Ticket";
                    pnlSolution.Visible = true;
                }
                if (Convert.ToInt32(Session["_StatusID"]) == 5)
                {
                    Status_tck.Text = "Status: Closed";
                    Status_tck.ForeColor = Color.Lime;
                    deRespDate.Visible = true; //AM Date Time SLA
                    deRespDate.Enabled = false;
                    lbRespDateTime.Visible = true;
                    deRestDate.Visible = true;
                    deRestDate.Enabled = false;
                    lbRestDateTime.Visible = true;
                    deResoDate.Visible = true;
                    deResoDate.Enabled = false;
                    lbResoDateTime.Visible = true;
                    deClosDate.Visible = true;
                    deClosDate.Enabled = false;
                    lbCloseDateTime.Visible = true;
                    lbMsg.Text = "Enter Closure Note:";
                    meRespNote.Enabled = false;
                    btSaveData.Enabled = false;
                    btClose.Enabled = false;
                    pnlTickRest.Visible = true;
                    pnlTickReso.Visible = true;
                }

                if (dt.Rows[0][0].ToString() != "")
                {
                    deRespDate.Date = DateTime.Parse(dt.Rows[0][0].ToString());
                    Session["ResponseDate"] = dt.Rows[0][0].ToString();
                }
                else
                    deRespDate.Value = null;
                if (dt.Rows[0][2].ToString() != "")
                {
                    deRestDate.Date = DateTime.Parse(dt.Rows[0][2].ToString());
                    Session["RestorationDate"] = dt.Rows[0][2].ToString();
                }
                else
                    deRestDate.Value = null;
                if (dt.Rows[0][4].ToString() != "")
                {
                    deResoDate.Date = DateTime.Parse(dt.Rows[0][4].ToString());
                    Session["ResolutionDate"] = dt.Rows[0][4].ToString();
                }
                else
                    deResoDate.Value = null;
                if (dt.Rows[0][6].ToString() != "")
                {
                    deClosDate.Date = DateTime.Parse(dt.Rows[0][6].ToString());
                    Session["ClosureDate"] = dt.Rows[0][6].ToString();
                }
                else
                    deClosDate.Value = null;
                

                //Response



                //lbResp.Text = dt.Rows[0][8].ToString()+ dt.Rows[0][9].ToString()+ dt.Rows[0][10].ToString()+ dt.Rows[0][11].ToString();


            }
            else
            {
                btnToPendingRest.Enabled = false;
                btnFromPendingRest.Enabled = true;
                Status_tck.Text = "Status: Pending Info";
                Status_tck.ForeColor = Color.Red;
                deDateRest.Enabled = true;
            }
            
            if (X == 2)
            {
                btnToPendingRest.Enabled = false;
                btnFromPendingRest.Enabled = true;
                btClose.Enabled = false;
                btSaveData.Enabled = false;
                deDateRest.Enabled = true;
                Status_tck.Text = "Status: Pending Info";
                Status_tck.ForeColor = Color.Red;
               
            }
            if (X == 3)
            {
                btnToPendingRest.Enabled = false;
                btnFromPendingRest.Enabled = true;
                btClose.Enabled = false;
                btSaveData.Enabled = false;
                deDateRest.Enabled = true;
                Status_tck.Text = "Status: Pending Info";
                Status_tck.ForeColor = Color.Red;                
            }
            meHistoryLog.Text = dt.Rows[0][1].ToString() + dt.Rows[0][3].ToString() + dt.Rows[0][5].ToString() + dt.Rows[0][7].ToString();
            meRespNote.Text = "";

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

        protected void btClose_Click(object sender, EventArgs e)   //AM- Close Status
        {
            int prevStatus = Convert.ToInt32(Session["PreviousStatus"]);
            string error = string.Empty;
            string sql = string.Empty;
            List<SqlParameter> sp = new List<SqlParameter>();
            sql = "UPDATE tblTicket SET StatusID=@StatusID, RejectAceptBy = @RejectAceptBy";
            sp = new List<SqlParameter>()
                    {
                        new SqlParameter() {ParameterName = "@ID", SqlDbType = SqlDbType.BigInt, Value=gvTickets.Text},
                        new SqlParameter() { ParameterName = "@StatusID", SqlDbType = SqlDbType.BigInt, Value=rbOpen.Checked ? prevStatus : 6 },
                        new SqlParameter() {ParameterName = "@RejectAceptBy", SqlDbType = SqlDbType.NVarChar, Value=User.Identity.Name}
                    };
            
            if (rbReject.Checked)
            {                
                sql += ", RejectNote = @RejectNote, SolutionTypeID=4";
                sp.Add(new SqlParameter() { ParameterName = "@RejectNote", SqlDbType = SqlDbType.NVarChar, Value = memoRejectNote.Text });
                
            }
            sql += " WHERE ID = @ID";
            DataBase.UpdateDB(sp, sql);
            updateStatusTicket(rbOpen.Checked ? prevStatus : 6);
            Panel3.Visible = true;
            switch (prevStatus)
            {
                    case 1:
                        
                        if (deRespDate.Text != "")
                        {
                            sql = "UPDATE tblTicket SET ResponseDate = @Date, RestorationNote='*** Status OPEN ***\n\n', ResponseBy=@By, StatusID=2 WHERE ID = @ID";
                            sp = new List<SqlParameter>()
                            {
                                new SqlParameter() {ParameterName = "@Date", SqlDbType = SqlDbType.DateTime, Value=deRespDate.Date},
                                new SqlParameter() {ParameterName = "@By", SqlDbType = SqlDbType.NVarChar, Value=User.Identity.Name},
                                new SqlParameter() {ParameterName = "@ID", SqlDbType = SqlDbType.BigInt, Value=gvTickets.Text},
                            };
                            updateStatusTicket(2);//Pasar a Estado Siguiente
                            DataBase.UpdateDB(sp, sql);
                        }
                        break;
                    case 2:
                        if (deRestDate.Text != "")
                        {
                            sql = "UPDATE tblTicket SET RestorationDate = @Date, RestorationBy=@By, ResolutionNote='*** Status RESTORED ***\n\n', StatusID=3 WHERE ID = @ID";
                            sp = new List<SqlParameter>()
                            {
                                new SqlParameter() {ParameterName = "@Date", SqlDbType = SqlDbType.DateTime, Value=deRestDate.Date},
                                new SqlParameter() {ParameterName = "@By", SqlDbType = SqlDbType.NVarChar, Value=User.Identity.Name},
                                new SqlParameter() {ParameterName = "@ID", SqlDbType = SqlDbType.BigInt, Value=gvTickets.Text}
                            };
                            updateStatusTicket(3);
                            DataBase.UpdateDB(sp, sql);
                        }
                        else
                        {
                            error = "Restoration Date and Time is required";
                        }
                    break;
                    case 3:
                    if (deResoDate.Text != "")
                    {
                        sql = "UPDATE tblTicket SET ResolutionDate = @Date, ResolutionBy=@By, ClosureNote='*** Status RESOLVED ***\n\n', StatusID=4 WHERE ID = @ID";
                        sp = new List<SqlParameter>()
                            {
                                new SqlParameter() {ParameterName = "@Date", SqlDbType = SqlDbType.DateTime, Value=deResoDate.Date},
                                new SqlParameter() {ParameterName = "@By", SqlDbType = SqlDbType.NVarChar, Value=User.Identity.Name},
                                new SqlParameter() {ParameterName = "@ID", SqlDbType = SqlDbType.BigInt, Value=gvTickets.Text}
                            };
                        updateStatusTicket(4);
                        DataBase.UpdateDB(sp, sql);
                    }
                    else
                    {
                        error = "Resolution Date and Time is required";
                        
                    }
                    break;
                    case 4:
                        if (deClosDate.Text != "")
                        {
                            if (SolutionType.SelectedItem != null)
                            {
                                Guid uid = Guid.NewGuid();
                                sql = "UPDATE tblTicket SET ClosureDate = @Date, ClosureBy=@By, StatusID=5, SolutionTypeID=@SolutionTypeID, QuestionUI=@QuestionUI WHERE ID = @ID";
                                sp = new List<SqlParameter>()
                                {
                                    new SqlParameter() {ParameterName = "@Date", SqlDbType = SqlDbType.DateTime, Value=deClosDate.Date },
                                    new SqlParameter() {ParameterName = "@By", SqlDbType = SqlDbType.NVarChar, Value=User.Identity.Name},
                                    new SqlParameter() {ParameterName = "@ID", SqlDbType = SqlDbType.BigInt, Value=gvTickets.Text},
                                    new SqlParameter() {ParameterName = "@SolutionTypeID", SqlDbType = SqlDbType.BigInt, Value=SolutionType.SelectedItem.Value},
                                    new SqlParameter() {ParameterName = "@QuestionUI", SqlDbType = SqlDbType.NVarChar, Value=uid.ToString("D")}
                                };
                                updateStatusTicket(5);
                                sendMail(uid.ToString("D"));
                            }
                            else
                            {
                                error = "Type Of Solution is required";
                            }
                            if (string.IsNullOrEmpty(error))
                            {
                                DataBase.UpdateDB(sp, sql);
                            }
                        }
                    else
                    {
                        error = "Closure Date and Time is required";

                    }
                    break;
            }
            
            if (string.IsNullOrEmpty(error))
            {
                lblMsg.Text = "Dates and Notes Updated Successfully";
                popMsg.ShowOnPageLoad = true;
                //Update Notes
                btSaveData_Click(null, null, 0);
                UpdatePendingInfo();

            }
            else
            {
                lblMsg.Text = error;
                popMsg.ShowOnPageLoad = true;
            }
            /*
            popMsg.ShowOnPageLoad = true;
            //Update Notes
            btSaveData_Click(null, null);
            UpdatePendingInfo();
            */
            
        }

        protected void btToPending_Click(object sender, EventArgs e)
        {
            if (deDateRest.Text != "")
            {
                string _meRespNote1 = string.Empty;
                //string head = "Saved by: {0}\nDate: {1}\n{2}\n------------------------------------------------------------------------------------\n\n";
                string head1 = "{2}\nSaved by: {0}\nDate: {1}\n\n*** Status PENDING INFO ***\n\n";
                _meRespNote1 += string.Format(head1, User.Identity.Name, DateTime.Now.ToString(), Server.HtmlEncode(memoTextRest.Text));

                string query = "INSERT INTO tblPendingInfo values(@TicketID, @PendingInfoOn, NULL, @PreviousStatus, @UsernameOn, NULL, @Detail, NULL)";
                string queryTicket = "UPDATE tblTicket SET StatusID=7";
                List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@TicketID", SqlDbType = SqlDbType.BigInt, Value = Session["ticket"] },
                new SqlParameter() { ParameterName = "@PreviousStatus", SqlDbType = SqlDbType.Int, Value = Session["_StatusID"]},
                new SqlParameter() { ParameterName = "@UsernameOn", SqlDbType = SqlDbType.NVarChar, Value = User.Identity.Name }
            };
                List<SqlParameter> spUpdate = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@ID", SqlDbType = SqlDbType.BigInt, Value = Session["ticket"] }
            };
                int statusPrev = Convert.ToInt32(Session["_StatusID"]);
                switch (statusPrev)
                {
                    case 1:
                        break;
                    case 2:
                        sp.Add(new SqlParameter() { ParameterName = "@PendingInfoOn", SqlDbType = SqlDbType.DateTime, Value = deDateRest.Text });
                        sp.Add(new SqlParameter() { ParameterName = "@Detail", SqlDbType = SqlDbType.NVarChar, Value = memoTextRest.Text });
                        spUpdate.Add(new SqlParameter() { ParameterName = "@PendNote", SqlDbType = SqlDbType.NVarChar, Value = _meRespNote1 });
                        queryTicket += ", RestorationNote = (Select ISNULL(RestorationNote,'') + @PendNote From tblTicket Where ID = @ID) WHERE ID = @ID";
                        memoTextRest.Text = "";
                        Status_tck.Text = "Pending Info";
                        Status_tck.ForeColor = Color.Red;
                        btnFromPendingRest.Enabled = true;
                        btnToPendingRest.Enabled = false;
                        btClose.Enabled = false;
                        btSaveData.Enabled = false;
                        deDateRest.Value = null;
                        break;
                    case 3:
                        sp.Add(new SqlParameter() { ParameterName = "@PendingInfoOn", SqlDbType = SqlDbType.DateTime, Value = deDateRest.Text });
                        sp.Add(new SqlParameter() { ParameterName = "@Detail", SqlDbType = SqlDbType.NVarChar, Value = memoTextRest.Text });
                        spUpdate.Add(new SqlParameter() { ParameterName = "@PendNote", SqlDbType = SqlDbType.NVarChar, Value = _meRespNote1 });
                        queryTicket += ", ResolutionNote = (Select ISNULL(ResolutionNote,'') + @PendNote From tblTicket Where ID = @ID) WHERE ID = @ID";
                        memoTextRest.Text = "";
                        Status_tck.Text = "Pending Info";
                        Status_tck.ForeColor = Color.Red;
                        btnFromPendingRest.Enabled = true;
                        btnToPendingRest.Enabled = false;
                        btClose.Enabled = false;
                        btSaveData.Enabled = false;
                        deDateRest.Value = null;
                        break;
                    case 4:
                        break;

                }
                
                DataBase.UpdateDB(sp, query);
                DataBase.UpdateDB(spUpdate, queryTicket);
                ASPxButton1_Click(null, null);
                //UpdatePendingInfo();
               
            }
            else
            {
                lblMsg.Text = "Date Time on hold is required";
                popMsg.ShowOnPageLoad = true;
                //memoTextRest.Text = "";
            }
        }

        private void UpdatePendingInfo()
        {
            
            string query = "SELECT * FROM tblPendingInfo WHERE TicketID = @TicketID Order By PendingInfoOn ASC";
            string severity = flDetails_E2.Text.Split(' ')[1];
            TimeSpan acumTotal;
            StringBuilder logs;
            DataRow[] info = null;
            DateTime statusDate = DateTime.Now;//Session["StatusDate"] != null ? DateTime.Parse(Session["StatusDate"].ToString()) : DateTime.Now;
            DataRow SlaInfo = getSeverityData(severity);
            Session["acumTotal"] = null;
            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@TicketID", SqlDbType = SqlDbType.Int, Value = Session["ticket"] }
            };
            DataTable pendingInfo = DataBase.GetDT(sp, query);
            #region update time status
            int statusTicket = Convert.ToInt32(Session["_StatusID"]);
            if (statusTicket == 7)
            {
                statusTicket = Convert.ToInt32(pendingInfo.Rows[pendingInfo.Rows.Count - 1]["PreviousStatus"]);
                Session["_StatusID"] = statusTicket;                
                int prevStatus = Convert.ToInt32(Session["_StatusID"]);
                
                if (statusTicket == 2)
                {
                    deRespDate.Visible = true;
                    deRespDate.Enabled = true;
                    lbRespDateTime.Visible = true;
                    deRestDate.Visible = true;
                    deRestDate.Enabled = true;
                    deRestDate.Value = null;
                    lbRestDateTime.Visible = true;
                    deResoDate.Visible = true;
                    deResoDate.Enabled = false;
                    deResoDate.Value = null;
                    lbResoDateTime.Visible = true;
                    deClosDate.Visible = true;
                    deClosDate.Enabled = false;
                    deClosDate.Value = null;
                    lbCloseDateTime.Visible = true;
                    btClose.Enabled = false;
                    btSaveData.Enabled = false;
                }
                if (statusTicket == 3)
                {
                    deRespDate.Visible = true;
                    deRespDate.Enabled = true;
                    lbRespDateTime.Visible = true;
                    deRestDate.Visible = true;
                    deRestDate.Enabled = true;                    
                    lbRestDateTime.Visible = true;
                    deResoDate.Visible = true;
                    deResoDate.Enabled = true;
                    deResoDate.Value = null;
                    lbResoDateTime.Visible = true;
                    deClosDate.Visible = false;
                    deClosDate.Enabled = false;
                    lbCloseDateTime.Visible = false;
                    deClosDate.Value = null;
                    btClose.Enabled = false;
                    btSaveData.Enabled = false;
                }
                PerformUpdates(statusTicket);
            }

            

            

            for (int i = 1; i < 6; i++)
            {
                DateTime currentDate = DateTime.Now;
                switch (i)
                {
                    #region Created
                    
                    case 1:
                        //Created                     
                        statusDate = DateTime.Parse(Session["CreationDate"].ToString());
                        if (Session["ResponseDate"] != null)
                        {
                            currentDate = DateTime.Parse(Session["ResponseDate"].ToString());
                        }
                        deDateRest.Enabled = true;
                        lblTotalResp.Text = SlaInfo["ResponseTime"].ToString();
                        acumTotal = new TimeSpan();
                        logs = new StringBuilder();
                        info = pendingInfo.Select("PreviousStatus = 2 OR PreviousStatus = 3");
                        if (info.Length > 0)
                        {
                            if (statusTicket == 2)
                            {
                                if (string.IsNullOrEmpty(info[info.Length - 1]["PendingInfoOff"].ToString()))
                                {
                                    //btnFromPendingRest.Enabled = true;
                                    //btnToPendingRest.Enabled = false;
                                    Session["HoldTime"] = true;
                                    //pnlFileReso.Visible = true;                                    
                                }
                                else
                                {
                                    //btnToPendingRest.Enabled = true;
                                    //btnFromPendingRest.Enabled = false;
                                    Session["HoldTime"] = false;                                    
                                    //pnlFileReso.Visible = true;
                                }
                            }
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
                                    logs.Append("\n");
                                }
                            }
                            lblTotalTimeResp.Text = acumTotal.ToString(@"dd\.hh\:mm\:ss");
                            memoRest.Text = logs.ToString();
                            Session["LogsOpen"] = logs.ToString();
                            TimeSpan timeTotal = currentDate - statusDate - acumTotal;
                            lblChroResp.Text = timeTotal.ToString(@"dd\.hh\:mm\:ss");
                            Session["StatusAcum"] = acumTotal;
                        }
                        
                        else
                        {
                            
                            Session["HoldTime"] = false;                            
                            //pnlFileReso.Visible = true;
                            timerResp.Enabled = true;
                            acumTotal = new TimeSpan();
                            lblTotalTimeResp.Text = acumTotal.ToString(@"dd\.hh\:mm\:ss");
                            TimeSpan timeTotal = currentDate - statusDate;
                            lblChroResp.Text = timeTotal.ToString(@"dd\.hh\:mm\:ss");
                            Session["StatusAcum"] = acumTotal;

                        }

                        if (statusTicket == 1)
                        {
                            timerResp.Enabled = true;
                            Session["StatusDate"] = statusDate;
                        }
                        else
                        {
                            //deDateRest.Enabled = false;
                            //btnFromPendingRest.Enabled = false;
                            //btnToPendingRest.Enabled = false;
                            //pnlFileReso.Visible = false;
                            timerResp.Enabled = false;
                        }
                        break;
                        
                    #endregion
                    #region Open
                    case 2:
                        // Open
                        if (Session["ResponseDate"] == null)
                        {
                            break;
                        }
                        statusDate = DateTime.Parse(Session["ResponseDate"].ToString());
                        if (Session["RestorationDate"] != null)
                        {
                            currentDate = DateTime.Parse(Session["RestorationDate"].ToString());
                        }
                        deDateRest.Enabled = true;
                        lblTotalRest.Text = SlaInfo["RecoveryTime"].ToString();
                        acumTotal = new TimeSpan();
                        logs = new StringBuilder();
                        info = pendingInfo.Select("PreviousStatus = 2");
                        if (info.Length > 0)
                        {
                            if (statusTicket == 2)
                            {
                                if (string.IsNullOrEmpty(info[info.Length - 1]["PendingInfoOff"].ToString()))
                                {
                                    //btnFromPendingRest.Enabled = true;
                                    //btnToPendingRest.Enabled = false;
                                    Session["HoldTime"] = true;
                                    //pnlFileReso.Visible = true;
                                }
                                else
                                {
                                    //btnToPendingRest.Enabled = true;
                                    //btnFromPendingRest.Enabled = false;
                                    Session["HoldTime"] = false;
                                    //pnlFileReso.Visible = true;
                                }
                            }

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
                                    logs.Append("\n");
                                }
                            }
                            lblTotalTimeRest.Text = acumTotal.ToString(@"dd\.hh\:mm\:ss");
                            memoRest.Text = logs.ToString();
                            TimeSpan timeTotal = currentDate - statusDate - acumTotal;
                            lblChroRest.Text = timeTotal.ToString(@"dd\.hh\:mm\:ss");
                            Session["StatusAcum"] = acumTotal;
                        }
                        else
                        {
                            //btnToPendingRest.Enabled = true;
                            Session["HoldTime"] = false;
                            //pnlFileReso.Visible = true;
                            acumTotal = new TimeSpan();
                            lblTotalTimeRest.Text = acumTotal.ToString(@"dd\.hh\:mm\:ss");
                            TimeSpan timeTotal = currentDate - statusDate;
                            lblChroRest.Text = timeTotal.ToString(@"dd\.hh\:mm\:ss");
                            Session["StatusAcum"] = acumTotal;
                        }
                        if (statusTicket == 2)
                        {
                            timerRest.Enabled = true;
                            Session["StatusDate"] = statusDate;
                        }
                        else
                        {
                            deDateRest.Enabled = false;
                            //btnFromPendingRest.Enabled = false;
                            //btnToPendingRest.Enabled = false;
                            //pnlFileReso.Visible = false;
                            timerRest.Enabled = false;
                        }
                        break;
                    #endregion
                    #region Restored
                    case 3:
                        // Restored
                        if (Session["RestorationDate"] == null)
                        {
                            break;
                        }
                        statusDate = DateTime.Parse(Session["RestorationDate"].ToString());
                        if (Session["ResolutionDate"] != null)
                        {
                            currentDate = DateTime.Parse(Session["ResolutionDate"].ToString());
                        }
                        deDateRest.Enabled = true;
                        lblTotalReso.Text = SlaInfo["ResolutionTime"].ToString();
                        acumTotal = new TimeSpan();
                        logs = new StringBuilder();
                        info = pendingInfo.Select("PreviousStatus = 3");
                        if (info.Length > 0)
                        {
                            if (statusTicket == 3)
                            {
                                if (string.IsNullOrEmpty(info[info.Length - 1]["PendingInfoOff"].ToString()))
                                {
                                    //btnFromPendingRest.Enabled = false;  //Estaba true
                                    //btnToPendingRest.Enabled = false;
                                    Session["HoldTime"] = true;
                                    //pnlFileReso.Visible = true;                                    
                                }
                                else
                                {
                                    btnToPendingRest.Enabled = true;  //estaba true
                                    //btnFromPendingRest.Enabled = false;
                                    Session["HoldTime"] = false;                                    
                                    //pnlFileReso.Visible = true;
                                } 
                            }
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
                                    logs.Append("\n");
                                }
                            }
                            lblTotalTimeReso.Text = acumTotal.ToString(@"dd\.hh\:mm\:ss");
                            memoRest.Text = logs.ToString();
                            Session["LogsRestored"] = logs.ToString();
                            TimeSpan timeTotal = currentDate - statusDate - acumTotal;
                            lblChroReso.Text = timeTotal.ToString(@"dd\.hh\:mm\:ss");
                            Session["StatusAcum"] = acumTotal;
                        }
                        else
                        {
                            //btnToPendingRest.Enabled = true;
                            Session["HoldTime"] = false;
                            //pnlFileReso.Visible = true;
                            acumTotal = new TimeSpan();
                            lblTotalTimeReso.Text = acumTotal.ToString(@"dd\.hh\:mm\:ss");
                            TimeSpan timeTotal = currentDate - statusDate;
                            lblChroReso.Text = timeTotal.ToString(@"dd\.hh\:mm\:ss");
                            Session["StatusAcum"] = acumTotal;
                        }
                        if (statusTicket == 3)
                        {
                            timerReso.Enabled = true;
                            Session["StatusDate"] = statusDate;
                        }
                        else
                        {
                            deDateRest.Enabled = false;
                            //btnFromPendingRest.Enabled = false;
                            //btnToPendingRest.Enabled = false;
                            //pnlFileReso.Visible = false;                            
                            timerReso.Enabled = false;
                        }
                        break;
                    #endregion
                    case 4:
                        // Resolved
                        break;
                    case 8:
                    // Canceled
                    default:
                        break;
                }
                
            }

            #endregion
            Session["PreviousStatus"] = statusTicket;
            
        }

        private DataRow getSeverityData(string severity)
        {
            DataTable SlaInfo = Session["SlaInfo"] as DataTable;
            DataRow[] info = SlaInfo.Select("Name = '" + severity + "'");
            return info[0];
        }


        protected void btFromPending_Click(object sender, EventArgs e)
        {
            if (deDateRest.Text != "")
            {
                
                string queryPrevStatus = "SELECT top 1 t1.PreviousStatus, t2.Name, t1.PendingInfoOn FROM tblPendingInfo t1 INNER JOIN tblStatus t2 ON t2.ID = t1.PreviousStatus WHERE t1.TicketID = @TicketID AND t1.PendingInfoOff is NULL ORDER BY t1.PendingInfoOn DESC";
                string query = "UPDATE tblPendingInfo SET PendingInfoOff = @PendingInfoOff, UsernameOff = @UsernameOff, DetailInfoOff = @DetailInfoOff WHERE TicketID = @TicketID AND PendingInfoOff IS NULL";
                string queryTicket = "UPDATE tblTicket SET StatusID = @StatusID";
                int prevStatus = Convert.ToInt32(Session["PreviousStatus"]);
                string _meRespNote2 = string.Empty;
                //string head = "Saved by: {0}\nDate: {1}\n{2}\n------------------------------------------------------------------------------------\n\n";
                string head2 = "{2}\nSaved by: {0}\nDate: {1}\n\n*** Status OPEN ***\n\n";
                if (prevStatus==3)
                    head2 = "{2}\nSaved by: {0}\nDate: {1}\n\n*** Status RESTORED ***\n\n";
                _meRespNote2 += string.Format(head2, User.Identity.Name, DateTime.Now.ToString(), Server.HtmlEncode(memoTextRest.Text));

                List<SqlParameter> sp = new List<SqlParameter>()
                {
                    new SqlParameter() { ParameterName = "@TicketID", SqlDbType = SqlDbType.BigInt, Value = Session["ticket"] }
                };
                DataTable dtStatus = DataBase.GetDT(sp, queryPrevStatus);
                List<SqlParameter> spUpdate = new List<SqlParameter>()
                {
                    new SqlParameter() { ParameterName = "@StatusID", SqlDbType = SqlDbType.Int, Value = dtStatus.Rows[0][0] },
                    new SqlParameter() { ParameterName = "@ID", SqlDbType = SqlDbType.BigInt, Value = Session["ticket"] }
                };

                sp = new List<SqlParameter>()
                {
                    new SqlParameter() { ParameterName = "@TicketID", SqlDbType = SqlDbType.BigInt, Value = Session["ticket"] },
                    new SqlParameter() { ParameterName = "@UsernameOff", SqlDbType = SqlDbType.NVarChar, Value = User.Identity.Name }
                };

                switch (prevStatus)
                {
                    case 1:
                    case 2:
                        sp.Add(new SqlParameter() { ParameterName = "@PendingInfoOff", SqlDbType = SqlDbType.DateTime, Value = deDateRest.Text });
                        sp.Add(new SqlParameter() { ParameterName = "@DetailInfoOff", SqlDbType = SqlDbType.NVarChar, Value = memoTextRest.Text });
                        spUpdate.Add(new SqlParameter() { ParameterName = "@PendNote", SqlDbType = SqlDbType.NVarChar, Value = _meRespNote2 });
                        queryTicket += ", RestorationNote = (Select ISNULL(RestorationNote,'') + @PendNote From tblTicket Where ID = @ID) WHERE ID = @ID";
                        memoTextRest.Text = "";
                        Status_tck.Text = "Status: Open";
                        Status_tck.ForeColor = Color.Lime;
                        btClose.Enabled = true;
                        btSaveData.Enabled = true;
                        deDateRest.Value = null;
                        break;
                    case 3:
                        sp.Add(new SqlParameter() { ParameterName = "@PendingInfoOff", SqlDbType = SqlDbType.DateTime, Value = deDateRest.Text });
                        sp.Add(new SqlParameter() { ParameterName = "@DetailInfoOff", SqlDbType = SqlDbType.NVarChar, Value = memoTextRest.Text });
                        spUpdate.Add(new SqlParameter() { ParameterName = "@PendNote", SqlDbType = SqlDbType.NVarChar, Value = _meRespNote2 });
                        queryTicket += ", ResolutionNote = (Select ISNULL(ResolutionNote,'') + @PendNote From tblTicket Where ID = @ID) WHERE ID = @ID";
                        memoTextRest.Text = "";
                        Status_tck.Text = "Status: Restored";
                        Status_tck.ForeColor = Color.Lime;
                        btClose.Enabled = true;
                        btSaveData.Enabled = true;
                        deDateRest.Value = null;
                        break;

                    case 4:
                        break;
                }
                
                DataBase.UpdateDB(sp, query);
                DataBase.UpdateDB(spUpdate, queryTicket);
                Session["_StatusID"] = prevStatus;
                //UpdatePendingInfo();
                ASPxButton1_Click(null, null);
                btnToPendingRest.Enabled = true;
                btnFromPendingRest.Enabled = false;
            }
            else
            {
                lblMsg.Text = "Date Time on hold is required";
                popMsg.ShowOnPageLoad = true;
            }

        }

        private void updateStatusTicket(int idStatus)
        {
            //string query = "SELECT Name FROM tblStatus WHERE ID = " + idStatus;
            //lbStatus.Text = DataBase.GetDT(query).Rows[0][0].ToString();
            Session["_StatusID"] = idStatus;
        }

        private string getStatusById(int idStatus)
        {
            string query = "SELECT Name FROM tblStatus WHERE ID = " + idStatus;
            return DataBase.GetDT(query).Rows[0][0].ToString();
        }

        protected void tTick_Tick(object sender, EventArgs e)
        {
            try
            {
                bool holdTime = Boolean.Parse(Session["HoldTime"].ToString());                
                if (Session["PreviousStatus"] != null && !holdTime)
                {
                    int prevStatus = Convert.ToInt32(Session["PreviousStatus"]);
                    DateTime statusDate = Convert.ToDateTime(Session["StatusDate"]);
                    DateTime currentDate = DateTime.Now;
                    TimeSpan time = currentDate - statusDate;
                    if (Session["StatusAcum"] != null)
                    {
                        time = time - TimeSpan.Parse(Session["StatusAcum"].ToString());
                    }
                    switch (prevStatus)
                    {
                        
                        case 1:
                            lblChroResp.Text = time.ToString(@"dd\.hh\:mm\:ss");
                            break;
                        case 2:
                            lblChroRest.Text = time.ToString(@"dd\.hh\:mm\:ss");
                            break;
                        case 3:
                            lblChroReso.Text = time.ToString(@"dd\.hh\:mm\:ss");
                            break;
                    }
                }
            }
            catch (Exception)
            {

            }

        }

        protected void UpdatePanel_Unload(object sender, EventArgs e)
        {
            RegisterUpdatePanel((UpdatePanel)sender);
        }
        protected void RegisterUpdatePanel(UpdatePanel panel)
        {
            var sType = typeof(ScriptManager);
            var mInfo = sType.GetMethod("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel", BindingFlags.NonPublic | BindingFlags.Instance);
            if (mInfo != null)
                mInfo.Invoke(ScriptManager.GetCurrent(Page), new object[] { panel });
        }

        protected void On_FilesUploadComplete(object server, DevExpress.Web.FilesUploadCompleteEventArgs e)
        {
            string prevStatus = getStatusById(Convert.ToInt32(Session["PreviousStatus"])).Replace(" ", "_");
            int status = Convert.ToInt32(Session["PreviousStatus"]);
            string newpath = MapPath("~/Content/Tickets/") + Session["ticket"] + "\\" + prevStatus + "\\";

            if (!Directory.Exists(newpath))
                Directory.CreateDirectory(newpath);

            switch (status)
            {
                case 1:
                case 2:
                    foreach (DevExpress.Web.UploadedFile file in ucReso.UploadedFiles)
                    {
                        file.SaveAs(newpath + file.FileName);
                    }
                    break;
                case 3:
                    foreach (DevExpress.Web.UploadedFile file in ucReso.UploadedFiles)
                    {
                        file.SaveAs(newpath + file.FileName);
                    }
                    break;
                case 4:
                    foreach (DevExpress.Web.UploadedFile file in ucReso.UploadedFiles)
                    {
                        file.SaveAs(newpath + file.FileName);
                    }
                    break;
            }
        }

        protected void ASPxButton1_Click1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(gvTickets.Text))
            {
                lblMsg.Text = "Choose Ticket first.";
            }
            else if (cbAmerinode.Checked || cbCreator.Checked || cbGroups.Checked || cbYourself.Checked)
            {
                //get creator
                string query = "SELECT Name, Title, aspnet_Membership.Email, WorkPhone, MobilePhone, HasWhatsapp FROM aspnet_Users INNER JOIN aspnet_Membership ON aspnet_Users.UserId=aspnet_Membership.UserId WHERE UserName = @UserName";
                List<SqlParameter> sp = new List<SqlParameter>
                            {
                                new SqlParameter() {ParameterName="@UserName", SqlDbType=SqlDbType.NVarChar, Value=ASPxFormLayout1_E7.Text}
                            };
                DataTable dt = DataBase.GetDT(sp, query, "ApplicationServices");
                string Name = dt.Rows[0][0].ToString();
                string Title = dt.Rows[0][1].ToString();
                string UserEmail = dt.Rows[0][2].ToString(); ;
                string Work = dt.Rows[0][3].ToString();
                string Mobile = dt.Rows[0][4].ToString();
                string Whats = dt.Rows[0][5].ToString();
                //get group email
                query = "SELECT GroupEmail FROM tblGroupSelection INNER JOIN tblGroupEmail ON tblGroupSelection.GroupID=tblGroupEmail.ID WHERE TicketID = @TicketID";
                sp = new List<SqlParameter>
                            {
                                new SqlParameter() {ParameterName="@TicketID", SqlDbType=SqlDbType.Int, Value=ASPxFormLayout1_E1.Text}
                            };
                dt = DataBase.GetDT(sp, query);
                string Group = "";
                foreach (DataRow dr in dt.Rows)
                {
                    Group += dr[0].ToString() + " & ";
                }
                if (Group != "")
                    Group = Group.Substring(0, Group.Length - 3);
                //send emails
                //Send email with ticket data
                string test2 = DateTime.Now.Subtract(DateTime.UtcNow).ToString();
                string test3 = test2.Substring(0, test2.IndexOf(":"));
                string msg = @"
            <b><h3>TICKET #: " + ASPxFormLayout1_E1.Text + @"</h3></b>
            <b><h3>SEVERITY: " + flDetails_E2.Text + @"</h3></b>
            Ticket and Contact Details: <br/><br/>
            <table style = ""width: 80 %; "" border = ""2"" cellpadding = ""4"">
            <tr>
                <td style = ""width: 26 %; "">Creation Date and Time (local)</td>
                <td style = ""width: 73 %;""> " + ASPxFormLayout1_E2.Value.ToString() + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "">Creation Date and Time (utc)</td>
                <td style = ""width: 73 %;""> " + ASPxFormLayout1_E20.Value.ToString() + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "">Creation Time Zone</td>
                <td style = ""width: 73 %;""> " + test3 + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "">Customer - Branch</td>
                <td style = ""width: 73 %; ""> " + flDetails_E15.Text + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "">Creator Name</td></td>
                <td style = ""width: 73 %; ""> " + Name + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "" >Creator Title</td>
                <td style = ""width: 73 %; "" > " + Title + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "" >Creator Email</td>
                <td style = ""width: 73 %; "" > " + UserEmail + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "" >Group Emails</td>
                <td style = ""width: 73 %; "" > " + Group + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "">Work Phone</td></td>
                <td style = ""width: 73 %; ""> " + Work + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "" >Mobile Phone</td>
                <td style = ""width: 73 %; "" > " + Mobile + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "" >Has WhatsApp</td>
                <td style = ""width: 73 %; "" > " + Whats + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "" >Additional Contact Instruction</td>
                <td style = ""width: 73 %; "" > " + ASPxFormLayout1_E18.Text + @" </td>
            </tr>

            </table> ";
                msg += "<br/>";
                msg += "<br/>";
                msg += "Site Details:<br/><br/>";
                msg += @"
            <table style = ""width: 80 %; "" border = ""2"" cellpadding = ""4"">
            <tr>
                <td style = ""width: 26 %; "">OEM</td>
                <td style = ""width: 73 %;""> " + flDetails_E3.Text + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "">Technology</td>
                <td style = ""width: 73 %; ""> " + flDetails_E5.SelectedItem.Text + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "">Network Elements</td>
                <td style = ""width: 73 %; ""> " + flDetails_E7.Text + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "">Controller</td>
                <td style = ""width: 73 %; ""> " + flDetails_E9.SelectedItem.Text + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "" >Controller IP Access Information</td>
                <td style = ""width: 73 %; "" > " + ASPxFormLayout1_E23.Text + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "" >Site</td>
                <td style = ""width: 73 %; "" > " + flDetails_E4.Text + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "" >Site IP Access Information</td>
                <td style = ""width: 73 %; "" > " + ASPxFormLayout1_E22.Text + @" </td>
            </tr>
            </table> ";
                msg += "<br/>";
                msg += "<br/>";
                msg += "Problem Details:<br/><br/>";
                msg += @"
            <table style = ""width: 80 %; "" border = ""2"" cellpadding = ""4"">
            <tr>
                <td style = ""width: 26 %; "">Problem Title</td>
                <td style = ""width: 73 %;""> " + ASPxFormLayout1_E14.Text + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "">Problem</td>
                <td style = ""width: 73 %; ""> " + flDetails_E13.Text + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "">Remedy</td>
                <td style = ""width: 73 %; ""> " + ASPxFormLayout1_E19.Text + @" </td>
            </tr>
            <tr>
                <td style = ""width: 26 %; "">Software Release</td>
                <td style = ""width: 73 %; ""> " + ASPxFormLayout1_E16.Text + @" </td>
            </tr>
            </table> ";
                //Set up title
                string queryCompanyBranch = "SELECT SUBSTRING(t2.Name, 1, 3) as company, case when CHARINDEX('-', t1.Name) > 0 then SUBSTRING(t1.Name, CHARINDEX('-', t1.Name) + 1, 3)";
                queryCompanyBranch += " when len(t1.Name) = 4 then SUBSTRING(t1.Name, 1, 4) else SUBSTRING(t1.Name, 1, 3) end as branch FROM tblBranch t1 INNER JOIN tblCompany t2 ON t2.ID = t1.CompanyID WHERE t1.ID = @ID";
                List<SqlParameter> parCB = new List<SqlParameter>()
                                            {
                                                new SqlParameter() {ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value= flDetails_E15.Value}
                                            };
                DataTable dtCompanyBranch = DataBase.GetDT(parCB, queryCompanyBranch);

                string priority = flDetails_E2.Text.Split(' ').Length > 1 ? flDetails_E2.Text.Split(' ')[1].Trim() : flDetails_E2.Text;
                string title = string.Format("{0} {1} Ticket # {2} {3} {4}", dtCompanyBranch.Rows[0]["company"].ToString().ToUpper(), dtCompanyBranch.Rows[0]["branch"].ToString().ToUpper(), ASPxFormLayout1_E1.Text, ASPxFormLayout1_E14.Text, priority);

                lblMsg.Text = "";
                //sending to the ticket creator
                if (cbCreator.Checked)
                {
                    lblMsg.Text += "Email sent to " + Name + " at " + UserEmail + "." + System.Environment.NewLine;
                    Email.SendEmail(title, msg, UserEmail);
                }
                //Sending email to selected groups
                if (cbGroups.Checked)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Email.SendEmail(title, msg, dr[0].ToString());
                    }
                    if (Group != "")
                        lblMsg.Text += "Email sent to " + Group + "." + System.Environment.NewLine;
                }
                //sending to Amerinode internally
                if (cbAmerinode.Checked)
                {
                    lblMsg.Text += "Email sent to Amerinode Support at sw_support@amerinode.com." + System.Environment.NewLine;
                    Email.SendEmail(title, msg, "sw_support@amerinode.com");
                }
                //sending to yourself
                if (cbYourself.Checked)
                {
                    MembershipUser mu = Membership.GetUser();
                    lblMsg.Text += "Email sent to yourself at " + mu.Email + "." + System.Environment.NewLine;
                    Email.SendEmail(title, msg, mu.Email);
                }
            }
            else
            {
                lblMsg.Text = "At least one of the checkboxes must be checked to resend email";
            }
            popMsg.ShowOnPageLoad = true;
        }

        protected void btSaveData_Click(object sender, EventArgs e, int X)   //AM-Save Data After Acepted
        {

            string sql = "UPDATE tblTicket SET ";
            List<string> sb = new List<string>();
            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName = "@ID", SqlDbType = SqlDbType.BigInt, Value=gvTickets.Text}
            };


            string _meRespNote = string.Empty;            
            //string head = "Saved by: {0}\nDate: {1}\n{2}\n------------------------------------------------------------------------------------\n\n";
            string head = "{2}\nSaved by: {0}\nDate: {1}\n\n";

            //AM-Inicio    (Select ResponseNote + 'aaaa' From tblTicket Where ID = 5802)       

            if (Convert.ToInt32(Session["_StatusID"]) == 2)
            {
                _meRespNote += string.Format(head, User.Identity.Name, DateTime.Now.ToString(), Server.HtmlEncode(meRespNote.Text));
                sb.Add("ResponseNote = (Select ISNULL(ResponseNote,'*** Status CREATED ***\n\n') + @Note1 From tblTicket Where ID = @ID)");
                sp.Add(new SqlParameter() { ParameterName = "@Note1", SqlDbType = SqlDbType.NVarChar, Value = _meRespNote });                
            }
            if (Convert.ToInt32(Session["_StatusID"]) == 3)
            {
                _meRespNote += string.Format(head, User.Identity.Name, DateTime.Now.ToString(), Server.HtmlEncode(meRespNote.Text));
                sb.Add("RestorationNote = (Select ISNULL(RestorationNote,'') + @Note2 From tblTicket Where ID = @ID)");
                sp.Add(new SqlParameter() { ParameterName = "@Note2", SqlDbType = SqlDbType.NVarChar, Value = _meRespNote });                
            }
            if (Convert.ToInt32(Session["_StatusID"]) == 4)
            {
                _meRespNote += string.Format(head, User.Identity.Name, DateTime.Now.ToString(), Server.HtmlEncode(meRespNote.Text));
                sb.Add("ResolutionNote = (Select ISNULL(ResolutionNote,'') + @Note3 From tblTicket Where ID = @ID)");
                sp.Add(new SqlParameter() { ParameterName = "@Note3", SqlDbType = SqlDbType.NVarChar, Value = _meRespNote });
                
            }
            if (Convert.ToInt32(Session["_StatusID"]) == 5)
            {
                _meRespNote += string.Format(head, User.Identity.Name, DateTime.Now.ToString(), Server.HtmlEncode(meRespNote.Text));
                sb.Add("ClosureNote= (Select ISNULL(ClosureNote,'') + @Note4 From tblTicket Where ID = @ID)");
                sp.Add(new SqlParameter() { ParameterName = "@Note4", SqlDbType = SqlDbType.NVarChar, Value = _meRespNote + "*** Status CLOSED ***" });                
            }       
            
            if (sb.Count > 0)
            {
                sql += string.Join(", ", sb.ToArray()) + " WHERE ID = @ID";               
                DataBase.UpdateDB(sp, sql);
            }
            PerformUpdates(0);
        }

        protected void btSaveData_Click(object sender, EventArgs e)   //AM-Save Data After Acepted
        {

            string sql = "UPDATE tblTicket SET ";
            List<string> sb = new List<string>();
            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName = "@ID", SqlDbType = SqlDbType.BigInt, Value=gvTickets.Text}
            };


            string _meRespNote = string.Empty;
            //string head = "Saved by: {0}\nDate: {1}\n{2}\n------------------------------------------------------------------------------------\n\n";
            string head = "{2}\nSaved by: {0}\nDate: {1}\n\n";

            //AM-Inicio    (Select ResponseNote + 'aaaa' From tblTicket Where ID = 5802)       

            if (Convert.ToInt32(Session["_StatusID"]) == 2)
            {
                _meRespNote += string.Format(head, User.Identity.Name, DateTime.Now.ToString(), Server.HtmlEncode(meRespNote.Text));
                sb.Add("RestorationNote = (Select ISNULL(RestorationNote,'') + @Note1 From tblTicket Where ID = @ID)");
                sp.Add(new SqlParameter() { ParameterName = "@Note1", SqlDbType = SqlDbType.NVarChar, Value = _meRespNote + "*** Status OPEN ***\n\n" });
            }
            if (Convert.ToInt32(Session["_StatusID"]) == 3)
            {
                _meRespNote += string.Format(head, User.Identity.Name, DateTime.Now.ToString(), Server.HtmlEncode(meRespNote.Text));                
                sb.Add("ResolutionNote = (Select ISNULL(ResolutionNote,'') + @Note2 From tblTicket Where ID = @ID)");
                sp.Add(new SqlParameter() { ParameterName = "@Note2", SqlDbType = SqlDbType.NVarChar, Value = _meRespNote + "*** Status RESTORED ***\n\n" });
            }
            if (Convert.ToInt32(Session["_StatusID"]) == 4)
            {
                _meRespNote += string.Format(head, User.Identity.Name, DateTime.Now.ToString(), Server.HtmlEncode(meRespNote.Text));
                sb.Add("ClosureNote = (Select ISNULL(ClosureNote,'') + @Note3 From tblTicket Where ID = @ID)");
                sp.Add(new SqlParameter() { ParameterName = "@Note3", SqlDbType = SqlDbType.NVarChar, Value = _meRespNote + "*** Status RESOLVED ***\n\n" });
            }   
            
            if (sb.Count > 0)
            {
                sql += string.Join(", ", sb.ToArray()) + " WHERE ID = @ID";                
                DataBase.UpdateDB(sp, sql);
            }
            meRespNote.Text = "";
            PerformUpdates(0);
        }

        protected void rbReject_CheckedChanged(object sender, EventArgs e)
        {
            if (rbReject.Checked)
            {
                pnlReject.Visible = true;
            }

        }

        protected void rbOpen_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOpen.Checked)
            {
                pnlReject.Visible = false;
            }
        }

        protected void SolutionType_DataBound(object sender, EventArgs e)
        {
            if (Session["SolutionTypeID"] != null)
            {
                SolutionType.SelectedIndex = SolutionType.Items.IndexOfValue(Session["SolutionTypeID"].ToString());
            }
        }

        private void sendMail(string uid)
        {
            Int64 ticket = Convert.ToInt64(Session["ticket"]);
            string link = @"https://telefonica.amerinode.net/Training/Questions.aspx?ticket=" + uid;
            string title = "Ticket # " + ticket.ToString() + " Closed";
            string body = "<b><p>The Ticket #: " + ticket.ToString() + " <br/>Para Amerinode es importante conocer el grado de satisfacción en la solución del ticket, por lo que pedimos 1 minuto de su tiempo para completar la siguiente encuesta:</p></b>";
            body += "<a href=\"" + link + "\">Survey ticket# " + ticket.ToString() + "</a>";
            string query = "SELECT t2.Email FROM TelefonicaPortalUsers..aspnet_Users t1 INNER JOIN TelefonicaPortalUsers..aspnet_Membership t2 on t2.UserId = t1.UserId";
            query += " WHERE UserName = (SELECT UserName COLLATE Latin1_General_CI_AS FROM tblTicket WHERE ID = " + ticket + ")";
            string mail = string.Empty;
            DataTable dt = DataBase.GetDT(query);
            if (dt.Rows.Count > 0)
            {
                mail = dt.Rows[0]["Email"].ToString();
                Notifications.add(title, body, "q@amerinode.com", mail, ticket);
            }
        }

        protected void gvTickets_Init(object sender, EventArgs e)
        {
            loadTblTicket();
        }

        protected void gvTickets_TextChanged1(object sender, EventArgs e)
        {


        }
    }
}