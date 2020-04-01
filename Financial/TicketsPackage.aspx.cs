using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ATCPortal.Financial
{
    public partial class TicketsPackage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null || Session["BranchID"] == null)
            {
                ValidateRequest.SetDataSession(User.Identity.Name);
            }

            if (!IsPostBack)
            {
                if (Convert.ToInt32(Session["CompanyID"]) == 7)
                {
                    pnlBranch.Visible = true;
                }
                Load2g();
            }
        }

        private void Load2g()
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

            /*if (branchID == 3)
            {
                Grid2g.Columns["Grid2g"].SetColVisible(false);
            }*/

            DataTable dt = DataBase.GetDT("EXEC packageTickets @BranchId = " + branchID + @", @TotalP1P22g = 20, @TotalP3P42g = 5, @PromP1P22g = '1.7', @PromP3P42g = '0.4', @TotalP1P23g = 30, @TotalP3P43g = 10, @PromP1P23g = '2.5', @PromP3P43g = '0.8'");
            
            Grid2g.DataSource = dt;
            Grid2g.DataBind();
        }

        protected void aglBranch_TextChanged(object sender, EventArgs e)
        {
            Load2g();
        }

        protected void Grid2g_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;
                if (drv["ColumnName"].ToString().Equals("Something"))
                {
                    e.Row.BackColor = System.Drawing.Color.Red;
                    
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.Green;
                }
            }
        }

        protected void Grid2g_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;
            string month = e.GetValue("Month").ToString();
            if (string.Equals(month, "TCK DISPONIBLES 2020"))
                e.Row.BackColor = System.Drawing.Color.LightSteelBlue;
            if (string.Equals(month, "TCK CONSUMIDO 2020"))
                e.Row.BackColor = System.Drawing.Color.LightSalmon;
            if (string.Equals(month, "CONSUMO"))
                e.Row.BackColor = System.Drawing.Color.LightSalmon;
            if (string.Equals(month, "PAQUETE CONTRATADO"))
                e.Row.BackColor = System.Drawing.Color.DarkSeaGreen;
            if (string.Equals(month, "PROM MES"))
                e.Row.BackColor = System.Drawing.Color.DarkSeaGreen;

        }

        protected void Grid2g_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            
        }
    }
}