using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Data.Filtering;
using DevExpress.Web;
using ClsQNotifications;

namespace ATCPortal.Amerinode
{
    public partial class QTicket : System.Web.UI.Page
    {

        public ASPxMemo memo;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CriteriaOperator filter = CriteriaOperator.Parse("[StatusID] = ? OR [StatusID] = ?", "1", "2");
                gvMain.ApplyFilterToColumn(gvMain.DataColumns["StatusID"], filter);

            }
        }

        protected void gvMain_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            //get Priority
            GridViewDataComboBoxColumn column = gvMain.Columns["PriorityID"] as GridViewDataComboBoxColumn;
            string priority = column.PropertiesComboBox.Items.FindByValue(e.NewValues["PriorityID"]).Text;
            //get the Creator's email
            string sql = "SELECT t1.Email FROM aspnet_Membership t1 INNER JOIN aspnet_Users t2 ON t2.UserId = t1.UserId WHERE t2.UserName = @username";
            List<SqlParameter> sp = new List<SqlParameter>()
                    {
                        new SqlParameter() {ParameterName = "@username", SqlDbType = SqlDbType.NVarChar, Value=e.OldValues["OpenBy"].ToString()}
                    };
            string CreatorEmail = DataBase.GetDT(sp,sql,"ApplicationServices").Rows[0][0].ToString();

            //Close Approved
            if (e.OldValues["CloseApproved"].ToString() != e.NewValues["CloseApproved"].ToString())
            {
                string ApprovedBy = "";
                if (e.NewValues["CloseApproved" +
                    ""].ToString() == "True") //final approval, sending email to Creator
                {
                    //stamp of approved by
                    ApprovedBy = User.Identity.Name;

                    gvMain.JSProperties["cpMessage"] = "Email sent to Amerinode Support at "+ e.NewValues["CustomerImpacting"].ToString() + " for immediate processing.";
                    Notifications.add("Q Ticket Implementation has been Approved", "<b>Q Ticket # " + e.Keys[0].ToString() + "</b> has just been given a final stamp of approval.<br/>"
                        + "Description: " + e.NewValues["Description"].ToString() + "<br/>"
                        + "Priority: " + priority + "<br/>"
                        + "Customer Impacting: " + e.NewValues["CustomerImpacting"].ToString()
                        , "q@amerinode.com", CreatorEmail, -1);
                }

                sql = "UPDATE base_ticket SET ApprovedBy = @ApprovedBy WHERE (ID = @ID)";
                sp = new List<SqlParameter>()
                    {
                        new SqlParameter() {ParameterName = "@ApprovedBy", SqlDbType = SqlDbType.NVarChar, Value= ApprovedBy},
                        new SqlParameter() {ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value= e.Keys[0].ToString()}
                    };
                DataBase.UpdateDB(sp, sql);
            }

            //Status Changed
            if (e.OldValues["StatusID"].ToString() != e.NewValues["StatusID"].ToString())
            {

                if (e.NewValues["StatusID"].ToString() == "1") //back to Created, sending email to executive team
                {

                    gvMain.JSProperties["cpMessage"] = "Email sent to Amerinode Support at ps_executive@amerinode.com for immediate processing.";
                    Notifications.add("Q Ticket Status has been changed to Created", "<b>Q Ticket # " + e.Keys[0].ToString() + "</b> has just been changed to Created.<br/>Login into Q and approve this ticket to proceed to development.<br/>"
                        + "Description: " + e.NewValues["Description"].ToString() + "<br/>"
                        + "Priority: " + priority + "<br/>"
                        + "Customer Impacting: " + e.NewValues["CustomerImpacting"].ToString(),
                        "q@amerinode.com", "ps_executive@amerinode.com", -1);
                }

                if (e.NewValues["StatusID"].ToString() == "2")//Proceed, sending email to IT
                {
                    gvMain.JSProperties["cpMessage"] = "Email sent to Amerinode Support at it_support@amerinode.com for immediate processing.";
                    Notifications.add("New Q Ticket has been approved to proceed", "<b>Q Ticket # " + e.Keys[0].ToString() + "</b> has just been approved to proceed to Development.<br/>"
                        + "Description: " + e.NewValues["Description"].ToString() + "<br/>"
                        + "Priority: " + priority + "<br/>"
                        + "Customer Impacting: " + e.NewValues["CustomerImpacting"].ToString() + "<br/>"
                        + "Resolution: " + e.NewValues["Resolution"]
                        ,"q@amerinode.com", "it_support@amerinode.com", -1);

                }

                if (e.NewValues["StatusID"].ToString() == "6") //published sending email to Creator
                {
                    //update ticket with whom and when it was published
                    sql = "UPDATE base_ticket SET DateClose = @DateClose, CloseBy = @CloseBy WHERE (ID = @ID)";
                    sp = new List<SqlParameter>()
                        {
                            new SqlParameter() {ParameterName = "@DateClose", SqlDbType = SqlDbType.Date, Value= DateTime.Today.ToShortDateString()},
                            new SqlParameter() {ParameterName = "@Closeby", SqlDbType = SqlDbType.NVarChar, Value= User.Identity.Name},
                            new SqlParameter() {ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value= e.Keys[0].ToString()}
                        };
                    DataBase.UpdateDB(sp, sql);

                    //Send email to Creator
                    gvMain.JSProperties["cpMessage"] = "Email sent to Amerinode Support at "+ CreatorEmail + " for immediate processing.";
                    Notifications.add("Q Ticket has been published", "<b>Q Ticket # " + e.Keys[0].ToString() + "</b> has just been published.<br/>"
                       + "Description: " + e.NewValues["Description"].ToString() + "<br/>"
                       + "Priority: " + priority + "<br/>"
                       + "Customer Impacting: " + e.NewValues["CustomerImpacting"].ToString() + "<br/>"
                       + "Resolution: " + e.NewValues["Resolution"]
                       , "q@amerinode.com", CreatorEmail, -1);
                }
                if (e.NewValues["StatusID"].ToString() == "7") //rejected, sending email to Creator
                {

                    //Send Email to Creator and Executive
                    gvMain.JSProperties["cpMessage"] = "Email sent to Amerinode Support at " + CreatorEmail + " for immediate processing.";
                    Notifications.add("Q Ticket has been rejected", "<b>Q Ticket # " + e.Keys[0].ToString() + "</b> has just been rejected.<br/>"
                        + "Description: " + e.NewValues["Description"].ToString() + "<br/>"
                        + "Priority: " + priority + "<br/>"
                        + "Customer Impacting: " + e.NewValues["CustomerImpacting"].ToString() + "<br/>"
                        + "Resolution: " + e.NewValues["Resolution"]
                        , "q@amerinode.com", CreatorEmail, -1);
                }
                if (e.NewValues["StatusID"].ToString() != "6")
                {
                    //clean DateClose, CloseBy, ApprovedBy to NULL and CloseApproved to 0
                    sql = "UPDATE base_ticket SET DateClose = NULL, CloseBy = NULL, ApprovedBy = NULL, CloseApproved = 0 WHERE ID = " + e.Keys[0].ToString();
                    DataBase.UpdateDB(null, sql);
                }
            }
        }

        protected void gvMain_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            gvMain.Columns["StatusID"].Visible = false;
            gvMain.Columns["DateOpen"].Visible = false;
            gvMain.Columns["DateClose"].Visible = false;
            gvMain.Columns["OpenBy"].Visible = false;
            gvMain.Columns["CloseBy"].Visible = false;
            gvMain.Columns["ApprovedBy"].Visible = false;
            gvMain.Columns["QVersion"].Visible = false;
            gvMain.Columns["Resolution"].Visible = false;
            gvMain.Columns["CloseApproved"].Visible = false;
            gvMain.Columns["Files"].Visible = false;
            UpdatePanel pnlUpload = gvMain.FindEditRowCellTemplateControl((GridViewDataColumn)gvMain.Columns[14], "pnlUpload") as UpdatePanel;
            pnlUpload.Visible = true;
        }

        protected void gvMain_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            //get priority
            GridViewDataComboBoxColumn column = gvMain.Columns["PriorityID"] as GridViewDataComboBoxColumn;
            string priority = column.PropertiesComboBox.Items.FindByValue(e.NewValues["PriorityID"]).Text;
            //get last id
            string sql = "SELECT MAX(ID) FROM base_ticket";
            string MAXID = DataBase.GetDT(sql).Rows[0][0].ToString();
            //update when and who created
            sql = "UPDATE base_ticket SET DateOpen = @DateOpen, OpenBy = @OpenBy WHERE (ID = @ID)";
            List<SqlParameter> sp = new List<SqlParameter>()
                    {
                        new SqlParameter() {ParameterName = "@DateOpen", SqlDbType = SqlDbType.Date, Value= DateTime.Today.ToShortDateString()},
                        new SqlParameter() {ParameterName = "@OpenBy", SqlDbType = SqlDbType.NVarChar, Value= User.Identity.Name},
                        new SqlParameter() {ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value= MAXID}
                    };
            DataBase.UpdateDB(sp, sql);
            
            //send email to executive team
            gvMain.JSProperties["cpMessage"] = "Email sent to Amerinode Support at ps_executive@amerinode.com for immediate processing.";
            Notifications.add("Q Ticket has been created", "<b>Q Ticket # " + MAXID + "</b> has just been created in Q by " + User.Identity.Name + ".<br/>"
                    + "Login into Q and change its Status to PROCEED as an authorization for IT to proceed with implementation.<br/>"
                    + "Description: " + e.NewValues["Description"].ToString() + "<br/>"
                    + "Priority: " + priority + "<br/>"
                    + "Customer Impacting: " + e.NewValues["CustomerImpacting"].ToString()
                    , "q@amerinode.com", "ps_executive@amerinode.com", -1);
            //re-enable columns
            gvMain.Columns["StatusID"].Visible = true;
            gvMain.Columns["DateOpen"].Visible = true;
            gvMain.Columns["DateClose"].Visible = true;
            gvMain.Columns["OpenBy"].Visible = true;
            gvMain.Columns["CloseBy"].Visible = true;
            gvMain.Columns["ApprovedBy"].Visible = true;
            gvMain.Columns["QVersion"].Visible = true;
            gvMain.Columns["Resolution"].Visible = true;
            gvMain.Columns["CloseApproved"].Visible = true;
            gvMain.Columns["Files"].Visible = true;
            if (Session["tempDir"] != null)
            {
                changePath(MAXID);
            } 
        }

        protected void ucFile_FilesUploadComplete(object sender, DevExpress.Web.FilesUploadCompleteEventArgs e)
        {
            var rand = new Random();
            string tempDir = rand.Next(1, 999).ToString();
            string newpath = MapPath("~/Content/Q/") + tempDir + "_\\";
            ASPxUploadControl fup = sender as ASPxUploadControl;
            
            if (!Directory.Exists(newpath))
                Directory.CreateDirectory(newpath);

            foreach (DevExpress.Web.UploadedFile file in fup.UploadedFiles)
            {
                file.SaveAs(newpath + file.FileName);
            }
            Session["tempDir"] = tempDir;
        }

        private void changePath(string id)
        {
            string tempDir = Session["tempDir"].ToString();
            string tempPath = MapPath("~/Content/Q/") + tempDir + "_\\";
            string newPath = MapPath("~/Content/Q/") + id + "\\";
            if (Directory.Exists(tempPath))
            {
                Directory.Move(tempPath, newPath);
            }
            Session["tempDir"] = null;
        }

        protected void gvMain_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Files")
            {
                int pat = Convert.ToInt32(e.GetListSourceFieldValue("ID"));
                e.Value = pat;
            }
        }

        protected void gvMain_Init(object sender, EventArgs e)
        {
            GridViewDataHyperLinkColumn cl = new GridViewDataHyperLinkColumn();
            cl.FieldName = "Files";
            cl.VisibleIndex = 15;
            cl.PropertiesHyperLinkEdit.NavigateUrlFormatString = "javascript:showImagesInfo('QTicketImages.aspx?qticket={0}');";
            cl.PropertiesHyperLinkEdit.TextFormatString = "View files ticket {0}";
            cl.UnboundType = DevExpress.Data.UnboundColumnType.String;
            cl.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
            cl.PropertiesHyperLinkEdit.TextField = "Files";
            gvMain.Columns.Add(cl);
        }

    }
}