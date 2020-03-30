using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ATCPortal.Reports
{
    public partial class ReportTicketView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null || Session["BranchID"] == null)
            {
                ValidateRequest.SetDataSession(User.Identity.Name);
            }
            
            if (Session["CompanyID"].ToString() != "7")
            {
                aglBranch.Value = Convert.ToInt32(Session["BranchID"]);
                aglBranch.Enabled = false;
                Status.SelectCommand +=  " where id not in (3, 4)";
            }
            if (!IsPostBack)
            {
                if (aglBranch.Value != null)
                {
                    OEM.SelectCommand = "SELECT DISTINCT t2.ID, t2.Name FROM tblOEMBranch t1 inner join tblOEM t2 ON t2.ID = t1.OEMID WHERE t1.BranchID = " + aglBranch.Value;
                    Technology.SelectCommand = "SELECT DISTINCT t2.ID, t2.Name FROM tblOEMBranch t1 inner join tblTechnology t2 ON t2.ID = t1.TechnologyID WHERE t1.BranchID = " + aglBranch.Value;
                }
            }
        }

        private void renderBox(string sql2)
        { 
            //aglOEM.DataSource = DataBase.GetDT(sql1);
            //aglOEM.DataBind();
            aglTechnology.DataSource = DataBase.GetDT(sql2);
            aglTechnology.DataBind();
        }

        protected void btnViewReport_Click(object sender, EventArgs e)
        {
            DateTime DateInit = deDateInit.Date;
            DateTime DateFinish = deDateFinish.Date;
            int BranchID = Convert.ToInt32(aglBranch.Value);
            List<object> TechnologyID = aglTechnology.GridView.GetSelectedFieldValues("ID");
            List<object> StatusID = aglStatus.GridView.GetSelectedFieldValues("ID");
            List<object> OEM = aglOEM.GridView.GetSelectedFieldValues("ID");
            List<object> SeverityID = aglSeverity.GridView.GetSelectedFieldValues("ID");
            List<string> error = new List<string>();

            if (aglBranch.Value == null)
            {
                error.Add("* Branch is required.");
            }

            if (deDateInit.Text == "")
            {
                error.Add("* Start Date is required.");
            }

            if (deDateFinish.Text == "")
            {
                error.Add("* End Date is required.");
            }

            if (error.Count > 0)
            {
                popMsg.ShowOnPageLoad = true;
                lblMsg.Text = string.Join("\n", error.ToArray());
            }
            else
            {
                //ReportTicket rpt = new ReportTicket(BranchID, OEM, TechnologyID, StatusID, SeverityID, DateInit, DateFinish);
                ReportsGeneral rpt = new ReportsGeneral(BranchID, OEM, TechnologyID, StatusID, SeverityID, DateInit, DateFinish);
                rpt.DisplayName = "Report";
                rpt.DataSource = rpt.customQuery();
                Ticketport.OpenReport(rpt);
            }
        }

        protected void aglBranch_TextChanged(object sender, EventArgs e)
        {
            if (aglBranch.Value != null)
            {
                OEM.SelectCommand = "SELECT DISTINCT t2.ID, t2.Name FROM tblOEMBranch t1 inner join tblOEM t2 ON t2.ID = t1.OEMID WHERE t1.BranchID = " + aglBranch.Value;
                Technology.SelectCommand = "SELECT DISTINCT t2.ID, t2.Name FROM tblOEMBranch t1 inner join tblTechnology t2 ON t2.ID = t1.TechnologyID WHERE t1.BranchID = " + aglBranch.Value;
            }
        }
    }
}