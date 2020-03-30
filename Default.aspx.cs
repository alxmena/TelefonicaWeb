using DevExpress.Web;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ATCPortal
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsCallback && !Page.IsPostBack)
            {
                if (User.Identity.IsAuthenticated) 
                {
                    string query = "SELECT CompanyID, BranchID from aspnet_Users WHERE UserName = @UserName";                    
                    List<SqlParameter> sp = new List<SqlParameter>()
                                {
                                    new SqlParameter() {ParameterName = "@UserName", SqlDbType = SqlDbType.NVarChar, Value = User.Identity.Name}
                                };
                    DataTable dt = DataBase.GetDT(sp, query, "ApplicationServices");
                    try
                    {
                        Session["CompanyID"] = Convert.ToInt32(dt.Rows[0][0].ToString());
                        Session["BranchID"] = Convert.ToInt32(dt.Rows[0][1].ToString());
                        if (Convert.ToInt32(dt.Rows[0][0].ToString()) == 7)
                        {
                            pnlBranch.Visible = true;
                        }
                    }
                    catch
                    {
                        Session["CompanyID"] = 0;
                        Session["BranchID"] = 0;
                    }
                    pnlImage.Visible = false;
                    pnlCharts.Visible = true;
                }
                else
                {
                    Session["CompanyID"] = 0;
                    Session["BranchID"] = 0;
                    pnlImage.Visible = true;
                    pnlCharts.Visible = false;
                }
                WebChartControl1_Init();
                WebChartControl3g_Init();
            }
        }

        private void WebChartControl1_Init()
        {
            int branchID = 7;
            if (Convert.ToInt32(Session["CompanyID"]) != 7)
            {
                branchID = Convert.ToInt32(Session["BranchID"]);
            }
            if (aglBranch.Value != null)
            {
                branchID = Convert.ToInt32(aglBranch.Value);
            }
            pnl2g.Visible = true;
            string queryp1p2 = @"
                SELECT 
                'P1 and P2' as Severity,
                SUM(CASE WHEN SeverityID IN (1, 3) THEN 1 ELSE 0 END) as Total,
                20 - SUM(CASE WHEN SeverityID IN (1, 3) THEN 1 ELSE 0 END) as TotalAval,
                20 as TotalPack
                FROM tblTicket
                WHERE YEAR(CreationDate) = YEAR(GETDATE()) and TechnologyID = {0} and BranchId = {1}
                union all
                select 
                'P3 and P4' AS Severity,
                SUM(CASE WHEN SeverityID IN (4, 5) THEN 1 ELSE 0 END) Total,
                5 - SUM(CASE WHEN SeverityID IN (4, 5) THEN 1 ELSE 0 END) as TotalAval,
                5 as TotalPack
                FROM tblTicket
                WHERE YEAR(CreationDate) = YEAR(GETDATE()) and TechnologyID = {2} and BranchId = {3}
            ";

            DataTable dt = DataBase.GetDT(string.Format(queryp1p2, 1007, branchID, 1007, branchID));             
            List<Series> listSeries2g = new List<Series>();
            Series series2g1 = new Series("TCK CONSUMIDO 2020", ViewType.Bar);
            series2g1.ArgumentDataMember = "Severity";
            series2g1.ValueDataMembers.AddRange("Total");
            series2g1.DataSource = dt;
            series2g1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            listSeries2g.Add(series2g1);

            Series series2g2 = new Series("TCK DISPONIBLES 2020", ViewType.Bar);
            series2g2.ArgumentDataMember = "Severity";
            series2g2.ValueDataMembers.AddRange("TotalAval");
            series2g2.DataSource = dt;
            series2g2.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            listSeries2g.Add(series2g2);

            Series series2g3 = new Series("PAQUETE CONTRATADO", ViewType.Bar);
            series2g3.ArgumentDataMember = "Severity";
            series2g3.ValueDataMembers.AddRange("TotalPack");
            series2g3.DataSource = dt;
            series2g3.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            listSeries2g.Add(series2g3);

            listSeries2g.Add(series2g1);
            WebChartControl2g.Series.Clear();
            WebChartControl2g.Series.AddRange(listSeries2g.ToArray());
        }

        private void WebChartControl3g_Init()
        {
            int branchID = 7;
            if (Convert.ToInt32(Session["CompanyID"]) != 7)
            {
                branchID = Convert.ToInt32(Session["BranchID"]);
            }
            if (aglBranch.Value != null)
            {
                branchID = Convert.ToInt32(aglBranch.Value);
            }
            if (DataBase.GetDT("select TechnologyID from tblOEMBranch where BranchID = " + branchID + " and TechnologyID = 1008 group by TechnologyID").Rows.Count >= 1)
            {
                pnl3g.Visible = true;
                string queryp1p2 = @"
                    SELECT 
                    'P1 and P2' as Severity,
                    SUM(CASE WHEN SeverityID IN (1, 3) THEN 1 ELSE 0 END) as Total,
                    30 - SUM(CASE WHEN SeverityID IN (1, 3) THEN 1 ELSE 0 END) as TotalAval,
                    30 as TotalPack
                    FROM tblTicket
                    WHERE YEAR(CreationDate) = YEAR(GETDATE()) and TechnologyID = {0} and BranchId = {1}
                    union all
                    select 
                    'P3 and P4' AS Severity,
                    SUM(CASE WHEN SeverityID IN (4, 5) THEN 1 ELSE 0 END) Total,
                    10 - SUM(CASE WHEN SeverityID IN (4, 5) THEN 1 ELSE 0 END) as TotalAval,
                    10 as TotalPack
                    FROM tblTicket
                    WHERE YEAR(CreationDate) = YEAR(GETDATE()) and TechnologyID = {2} and BranchId = {3}
                ";
                
                DataTable dt = DataBase.GetDT(string.Format(queryp1p2, 1008, branchID, 1008, branchID));                
                List<Series> listSeries = new List<Series>();
                Series series1 = new Series("TCK CONSUMIDO 2020", ViewType.Bar);
                series1.ArgumentDataMember = "Severity";
                series1.ValueDataMembers.AddRange("Total");
                series1.DataSource = dt;
                series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                listSeries.Add(series1);

                Series series2 = new Series("TCK DISPONIBLES 2020", ViewType.Bar);
                series2.ArgumentDataMember = "Severity";
                series2.ValueDataMembers.AddRange("TotalAval");
                series2.DataSource = dt;
                series2.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                listSeries.Add(series2);

                Series series3 = new Series("PAQUETE CONTRATADO", ViewType.Bar);
                series3.ArgumentDataMember = "Severity";
                series3.ValueDataMembers.AddRange("TotalPack");
                series3.DataSource = dt;
                series3.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                listSeries.Add(series3);

                listSeries.Add(series1);
                WebChartControl3g.Series.Clear();
                WebChartControl3g.Series.AddRange(listSeries.ToArray());
            }
            else
            {
                pnl3g.Visible = false;
            }
        }

        protected void aglBranch_TextChanged(object sender, EventArgs e)
        {
            WebChartControl1_Init();
            WebChartControl3g_Init();
        }
    }
}