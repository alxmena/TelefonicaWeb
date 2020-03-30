using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ATCPortal.FieldTechnician
{
    public partial class ViewTickets : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null || Session["BranchID"] == null)
            {
                ValidateRequest.SetDataSession(User.Identity.Name);
            }

            if (Session["CompanyID"].ToString() != "7") //not Amerinode User
                sqlMain.SelectCommand += " WHERE BranchID = " + Session["BranchID"] + " ORDER BY ID DESC";
            else
                sqlMain.SelectCommand += " ORDER BY ID DESC"; 
        }

        protected void gvMain_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            ASPxGridView gv = sender as ASPxGridView;
            gv.SettingsText.PopupEditFormCaption = "Edit Form - Ticket # " + e.EditingKeyValue.ToString();
            SetMandatory();
            Session["Init"] = "0";

            //If user is manager, it can edit all records, otherwise only your own ticket
            string sql = "SELECT IsManager from aspnet_Users WHERE UserName = @UserName";
            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName="UserName", SqlDbType = SqlDbType.NVarChar, Value=User.Identity.Name }
            };
            bool IsManager = bool.Parse(DataBase.GetDT(sp, sql,"ApplicationServices").Rows[0][0].ToString());
            bool IsOwner = false;
            if (User.Identity.Name == gv.GetRowValuesByKeyValue(e.EditingKeyValue, "UserName").ToString())
                IsOwner = true;

            if(!IsManager && !IsOwner)
            {
                gvMain.JSProperties["cpMessage"] = "You may only edit tickets that you have created. Managers can edit any ticket -- if you feel you need to be assigned as Manager, please send a request to your administrator.";
                e.Cancel = true;
                gvMain.CancelEdit();
            }


        }

        protected void gvMain_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (Session["NewTech"] == null)
               e.RowError = "Technology must be selected. ";
            ASPxTextBox tb = gvMain.FindEditRowCellTemplateControl(gvMain.Columns["ControllerIP"] as GridViewDataColumn, "tbControllerIP") as ASPxTextBox;
            if (tb.Text == "")
                e.RowError += "Controller IP is required. ";
            if (e.NewValues["ProblemTitle"] == null)
                e.RowError += "Problem Title is required.";
            if (e.HasErrors)
            {
                SetMandatory();
            }
            else
            {
                //record TechnologyID and NetworkElementID into DB
                string sql = "UPDATE tblTicket SET SeverityID = @p1, OEMID=@p2, TechnologyID = @p3, NetworkElementID = @p4, RadioControllerID = @p5,  ProblemTitle = @p6, ProblemDescription = @p7, SoftwareRelease = @p8, ContactInstructions = @p9, Remedy = @p10, SiteIP = @p11, ControllerIP = @p12 WHERE ID = @ID";
                List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName = "@p1", SqlDbType = SqlDbType.Int, Value = e.NewValues["SeverityID"]},
                new SqlParameter() {ParameterName = "@p2", SqlDbType = SqlDbType.Int, Value = Session["NewOEM"]},
                new SqlParameter() {ParameterName = "@p3", SqlDbType = SqlDbType.Int, Value = Session["NewTech"]},
                new SqlParameter() {ParameterName = "@p4", SqlDbType = SqlDbType.Int, IsNullable = true, Value = Session["NewNetwork"] ?? DBNull.Value},
                new SqlParameter() {ParameterName = "@p5", SqlDbType = SqlDbType.Int, Value = e.NewValues["RadioControllerID"]},
                new SqlParameter() {ParameterName = "@p6", SqlDbType = SqlDbType.NVarChar, Value = e.NewValues["ProblemTitle"]},
                new SqlParameter() {ParameterName = "@p7", SqlDbType = SqlDbType.NVarChar, Value = e.NewValues["ProblemDescription"] ?? DBNull.Value},
                new SqlParameter() {ParameterName = "@p8", SqlDbType = SqlDbType.NVarChar, Value = e.NewValues["SoftwareRelease"] ?? DBNull.Value},
                new SqlParameter() {ParameterName = "@p9", SqlDbType = SqlDbType.NVarChar, Value = e.NewValues["ContactInstructions"]?? DBNull.Value},
                new SqlParameter() {ParameterName = "@p10", SqlDbType = SqlDbType.NVarChar, Value = e.NewValues["Remedy"] ?? string.Empty},
                new SqlParameter() {ParameterName = "@p11", SqlDbType = SqlDbType.NVarChar, Value = e.NewValues["SiteIP"] ?? DBNull.Value},
                new SqlParameter() {ParameterName = "@p12", SqlDbType = SqlDbType.NVarChar, Value = tb.Text},
                new SqlParameter() {ParameterName = "@ID", SqlDbType = SqlDbType.NVarChar, Value = e.Keys["ID"]}
            };
                int aux = DataBase.UpdateDB(sp, sql);
                if (aux == 1)
                    gvMain.JSProperties["cpMessage"] = "Update Successful";
                else if (aux == 0)
                    gvMain.JSProperties["cpMessage"] = "No Record was updated";
            }
    
        }

        protected void gvMain_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            SetMandatory();
            ASPxGridView gv = (ASPxGridView)sender;
            string[] commands = e.Parameters.Split('=');
            string command = commands[0];
            
            if (command == "1" || command == "2" || command == "3")
            {
                string value1 = "0"; //OEMID
                string value2 = "0"; //TechID

                if (command == "1") //OEM changed
                {
                    value1 = commands[1];
                    if (Session["NewTech"] != null)
                        value2 = Session["NewTech"].ToString();
                    Session["NewOEM"] = value1;
                }

                if (command == "2") //Tech changed
                {
                    value1 = Session["NewOEM"].ToString();
                    value2 = commands[1];
                    Session["NewTech"] = value2;
                    if (Session["NewTech"].ToString() == "null")
                        Session["NewTech"] = null;
                }

                if (command == "3") //Network Element changed
                {
                    value1 = Session["NewOEM"].ToString();
                    if (Session["NewTech"] != null)
                        value2 = Session["NewTech"].ToString();
                    else
                        value2 = "null";
                    Session["NewNetwork"] = commands[1];
                    if (Session["NewNetwork"].ToString() == "null")
                        Session["NewNetwork"] = null;
                }

                ASPxComboBox cb1 = gv.FindEditRowCellTemplateControl(gv.Columns["TechnologyID"] as GridViewDataColumn, "cbTech") as ASPxComboBox;
                ASPxComboBox cb2 = gv.FindEditRowCellTemplateControl(gv.Columns["NetworkElementID"] as GridViewDataColumn, "cbNetwork") as ASPxComboBox;

                string sql = "SELECT ID, NAME FROM tblTechnology WHERE OEMID = " + value1;
                DataTable dt = DataBase.GetDT(sql);
                cb1.DataSource = dt;
                cb1.DataBind();
                if (command == "1" || value2 == null)
                {
                    cb1.SelectedIndex = -1;
                    Session["NewTech"] = null;
                    cb2.Items.Clear();
                    cb2.SelectedIndex = -1;
                    Session["NewNetwork"] = null;
                }
                else if (command == "2")
                {
                    cb1.SelectedIndex = cb1.Items.FindByValue(value2).Index;
                    sql = "SELECT ID, NAME FROM tblNetworkElement WHERE TechID = " + value2;
                    dt = DataBase.GetDT(sql);
                    cb2.DataSource = dt;
                    cb2.DataBind();
                    cb2.SelectedIndex = -1;
                    Session["NewNetwork"] = null;
                }
                else if (command == "3")
                {
                    sql = "SELECT ID, NAME FROM tblNetworkElement WHERE TechID = " + value2;
                    dt = DataBase.GetDT(sql);
                    cb2.DataSource = dt;
                    cb2.DataBind();
                    if (Session["NewNetwork"] == null)
                        cb2.SelectedIndex = -1;
                    else
                        cb2.SelectedIndex = cb2.Items.FindByValue(Session["NewNetwork"].ToString()).Index;
                }
            }
            if (command == "4")
            {
                string value4 = commands[1]; 
                ASPxTextBox tb = gv.FindEditRowCellTemplateControl(gv.Columns["ControllerIP"] as GridViewDataColumn, "tbControllerIP") as ASPxTextBox;
                string sql = "SELECT IPAddress FROM tblController WHERE ID = " + value4;
                DataTable dt = DataBase.GetDT(sql);
                tb.Text = dt.Rows[0][0].ToString();
            }
            
        }

        protected void cbTech_Init(object sender, EventArgs e)
        {
            if (Session["Init"].ToString() == "0" || Session["NewTech"] == null)
            {
                if (Session["Init"].ToString() == "0")
                    Session["NewOEM"] = gvMain.GetRowValues(gvMain.EditingRowVisibleIndex, "OEMID").ToString();
                string sql = "SELECT ID, NAME FROM tblTechnology WHERE OEMID = " + Session["NewOEM"];
                DataTable dt = DataBase.GetDT(sql);
                ASPxComboBox cb = (ASPxComboBox)sender;
                cb.DataSource = dt;
                cb.DataBind();
                if (Session["NewTech"] == null && Session["Init"].ToString() == "1")
                    cb.SelectedIndex = -1;
                else
                    cb.SelectedIndex = cb.Items.FindByValue(gvMain.GetRowValues(gvMain.EditingRowVisibleIndex, "TechnologyID").ToString()).Index;
            }

        }

        protected void cbNetwork_Init(object sender, EventArgs e)
        {
            if (Session["Init"].ToString() == "0")
            {
                Session["NewTech"] = gvMain.GetRowValues(gvMain.EditingRowVisibleIndex, "TechnologyID").ToString();
                string sql = "SELECT ID, NAME FROM tblNetworkElement WHERE TechID = " + Session["NewTech"];
                DataTable dt = DataBase.GetDT(sql);
                ASPxComboBox cb = (ASPxComboBox)sender;
                cb.DataSource = dt;
                cb.DataBind();
                try
                {
                    cb.SelectedIndex = cb.Items.FindByValue(gvMain.GetRowValues(gvMain.EditingRowVisibleIndex, "NetworkElementID").ToString()).Index;
                }
                catch
                {
                    cb.SelectedIndex = -1;
                }
                
                Session["Init"] = "1";
            }
        }

        protected void gvMain_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
            gvMain.CancelEdit();
        }

        protected void SetMandatory()
        {
            gvMain.DataColumns["TechnologyID"].Caption = "Technology *";
            gvMain.DataColumns["ControllerIP"].Caption = "Controller IP Access *";
            gvMain.DataColumns["ProblemTitle"].Caption = "Problem Title *";
        }

        protected void tbControllerIP_Init(object sender, EventArgs e)
        {
            ASPxTextBox tb = (ASPxTextBox)sender;
            tb.Text = gvMain.GetRowValues(gvMain.EditingRowVisibleIndex, "ControllerIP").ToString();
        }
        
        protected void gvMain_ToolbarItemClick(object source, DevExpress.Web.Data.ASPxGridViewToolbarItemClickEventArgs e)
        {
            string filter = ((GridViewDataColumn)gvMain.Columns["BranchID"]).FilterExpression;
            string fileName = "Report_TCK_";
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.Replace("Branch", "");
                fileName += getBranchNameByString(filter);
            }
            else
            {
                if (Session["CompanyID"].ToString() != "7")
                {
                    fileName += getBranchNameByString("ID = " + Session["BranchID"]);
                }
            }
            fileName += "_" + DateTime.Now.ToString("dd_MM_yyyy");
            if (e.Item.Command == DevExpress.Web.GridViewToolbarCommand.ExportToXlsx)
            {
                gvMain.ExportXlsxToResponse(fileName);
            }

            if (e.Item.Command == DevExpress.Web.GridViewToolbarCommand.ExportToCsv)
            {
                gvMain.ExportCsvToResponse(fileName);
            }

        }

        private string getBranchNameByString(string filter)
        {
            string sql = "SELECT Name FROM tblBranch where " + filter;
            DataTable dt = DataBase.GetDT(sql);
            List<string> branch = new List<string>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    branch.Add(dr["Name"].ToString());
                }
            }
            return string.Join("_", branch.ToArray());
        }
    }
}