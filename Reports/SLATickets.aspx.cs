using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ATCPortal.Reports
{
    public partial class SLATickets : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null || Session["BranchID"] == null)
            {
                ValidateRequest.SetDataSession(User.Identity.Name);
            }

            if (!IsPostBack)
            {
                Session["_tblTicket"] = null;
            }
            loadData();
        }
        #region tblReport
        private DataTable tblReport()
        {
            string trial = Request.QueryString.Get("trial");
            bool trialFlag = false;

            string sql = "SELECT *, (select top 1 Detail from tblPendingInfo where TicketID = t1.ID order by PendingInfoOn desc) as Detail FROM tblTicket t1";

            if (Session["CompanyID"].ToString() != "7")
            {
                sql += " WHERE BranchID = " + Session["BranchID"];
                if (!string.IsNullOrEmpty(trial))
                {
                    sql += " and CreationDate < '2020-01-01'";
                    trialFlag = true;
                }
                else
                {
                    sql += " and CreationDate >= '2020-01-01'";
                }
            }
            else if (!string.IsNullOrEmpty(trial))
            {
                sql += " where CreationDate < '2020-01-01'";
                trialFlag = true;
            }
            else
            {
                sql += " where CreationDate >= '2020-01-01'";
            }

            sql += " ORDER BY ID DESC";

            DataTable tblTicket = DataBase.GetDT(sql);
            DataTable table = new DataTable();
            table.Columns.Add("ID");
            table.Columns.Add("CreationDate");
            table.Columns.Add("ResponseDate");
            table.Columns.Add("TimeResponse");
            table.Columns.Add("SlaResponse");
            table.Columns.Add("RestorationDate");
            table.Columns.Add("TimeRestoration");
            table.Columns.Add("SlaRestoration");
            table.Columns.Add("TotalPendingInfoRestoration");
            table.Columns.Add("TotalAmerinodeRestoration");
            table.Columns.Add("ResolutionDate");
            table.Columns.Add("TimeResolution");
            table.Columns.Add("SlaResolution");
            table.Columns.Add("TotalPendingInfoResolution");
            table.Columns.Add("TotalAmerinodeResolution");
            table.Columns.Add("ClosureDate");
            table.Columns.Add("BranchID");
            table.Columns.Add("OEMID");
            table.Columns.Add("TechnologyID");
            table.Columns.Add("StatusID");
            table.Columns.Add("SeverityID");
            table.Columns.Add("SolutionTypeID");
            table.Columns.Add("NetworkElementID");
            table.Columns.Add("UserName");
            table.Columns.Add("ProblemTitle");
            table.Columns.Add("Advances");



            foreach (DataRow dr in tblTicket.Rows)
            {
                ClsSLA clsSla = new ClsSLA(Convert.ToInt32(dr["id"]), Convert.ToInt32(dr["SeverityID"]), Convert.ToInt32(dr["BranchID"]), trialFlag);
                DateTime currentTime = DateTime.Now;
                DataRow fila = table.NewRow();
                fila["ID"] = dr["id"];
                fila["CreationDate"] = dr["CreationDate"];

                fila["ResponseDate"] = dr["ResponseDate"];
                if (!string.IsNullOrEmpty(dr["ResponseDate"].ToString()))
                {
                    currentTime = DateTime.Parse(dr["ResponseDate"].ToString());
                }

                ClsSlaData tResponse = clsSla.getSla(DateTime.Parse(dr["CreationDate"].ToString()), currentTime, 2);
                if (Convert.ToInt32(dr["StatusID"]) != 6)
                {
                    fila["TimeResponse"] = tResponse.timeSla;
                    if (!string.IsNullOrEmpty(dr["ResponseDate"].ToString()))
                    {
                        fila["SlaResponse"] = tResponse.inSla;
                    }
                    else
                    {
                        fila["SlaResponse"] = "PENDING";
                    }
                    
                }
                else
                {
                    fila["TimeResponse"] = "NA";
                    fila["SlaResponse"] = "NA";
                }
                
                fila["RestorationDate"] = dr["RestorationDate"];
                
                if (Convert.ToInt32(dr["StatusID"]) > 2 && Convert.ToInt32(dr["StatusID"]) != 6)
                {
                    currentTime = DateTime.Now;
                    if (!string.IsNullOrEmpty(dr["RestorationDate"].ToString()))
                    {
                        currentTime = DateTime.Parse(dr["RestorationDate"].ToString());
                    }
                    try
                    {
                        ClsSlaData tRestoration = clsSla.getSla(DateTime.Parse(dr["ResponseDate"].ToString()), currentTime, 3);
                        fila["TimeRestoration"] = tRestoration.timeSla;
                        if (!string.IsNullOrEmpty(dr["RestorationDate"].ToString()))
                        {
                            fila["SlaRestoration"] = tRestoration.inSla;
                        }
                        else
                        {
                            fila["SlaRestoration"] = "PENDING";
                        }
                        
                        fila["TotalPendingInfoRestoration"] = tRestoration.slaFromPendingInfo;
                        fila["TotalAmerinodeRestoration"] = tRestoration.timeSla;
                    }
                    catch(Exception)
                    {
                        fila["TimeRestoration"] = "";
                        fila["SlaRestoration"] = "PENDING";
                        fila["TotalPendingInfoRestoration"] = "";
                        fila["TotalAmerinodeRestoration"] = "";
                    }
                }
                else if (Convert.ToInt32(dr["StatusID"]) == 6)
                {
                    fila["TimeRestoration"] = "NA";
                    fila["SlaRestoration"] = "NA";
                    fila["TotalPendingInfoRestoration"] = "NA";
                    fila["TotalAmerinodeRestoration"] = "NA";
                }
                else
                {
                    fila["TimeRestoration"] = "";
                    fila["SlaRestoration"] = "PENDING";
                    fila["TotalPendingInfoRestoration"] = "";
                    fila["TotalAmerinodeRestoration"] = "";
                }

                if (Convert.ToInt32(dr["StatusID"]) > 3 && Convert.ToInt32(dr["StatusID"]) != 6)
                {
                    currentTime = DateTime.Now;
                    if (!string.IsNullOrEmpty(dr["ResolutionDate"].ToString()))
                    {
                        currentTime = DateTime.Parse(dr["ResolutionDate"].ToString());
                    }
                    try
                    {
                        ClsSlaData tResolution = clsSla.getSla(DateTime.Parse(dr["RestorationDate"].ToString()), currentTime, 4);
                        fila["TimeResolution"] = tResolution.timeSla;
                        if (!string.IsNullOrEmpty(dr["ResolutionDate"].ToString()))
                        {
                            fila["SlaResolution"] = tResolution.inSla;
                        }
                        else
                        {
                            fila["SlaResolution"] = "PENDING";
                        }
                        
                        fila["TotalPendingInfoResolution"] = tResolution.slaFromPendingInfo;
                        fila["TotalAmerinodeResolution"] = tResolution.timeSla;
                    }
                    catch(Exception)
                    {
                        fila["TimeResolution"] = "";
                        fila["SlaResolution"] = "PENDING";
                        fila["TotalPendingInfoResolution"] = "";
                        fila["TotalAmerinodeResolution"] = "";
                    }
                }
                else if (Convert.ToInt32(dr["StatusID"]) == 6)
                {
                    fila["TimeResolution"] = "NA";
                    fila["SlaResolution"] = "NA";
                    fila["TotalPendingInfoResolution"] = "";
                    fila["TotalAmerinodeResolution"] = "";
                }
                else
                {
                    fila["TimeResolution"] = "";
                    fila["SlaResolution"] = "PENDING";
                    fila["TotalPendingInfoResolution"] = "";
                    fila["TotalAmerinodeResolution"] = "";
                }

                fila["ResolutionDate"] = dr["ResolutionDate"];
                fila["ClosureDate"] = dr["ClosureDate"];
                fila["BranchID"] = dr["BranchID"];
                fila["OEMID"] = dr["OEMID"];
                fila["TechnologyID"] = dr["TechnologyID"];
                fila["StatusID"] = dr["StatusID"];
                fila["SeverityID"] = dr["SeverityID"];
                fila["SolutionTypeID"] = dr["SolutionTypeID"];
                fila["NetworkElementID"] = dr["NetworkElementID"];
                fila["UserName"] = dr["UserName"];
                fila["ProblemTitle"] = dr["ProblemTitle"];
                if (Convert.ToInt32(dr["StatusID"]) == 5)
                {
                    string note = dr["ClosureNote"].ToString();
                    int indexInitial = note.IndexOf("Date:");
                    int indexFinish = note.IndexOf("\n-");
                    if (indexInitial >=0 && indexFinish >= 0)
                    {
                        string texto = note.Substring(indexInitial, indexFinish - 1);
                        indexInitial = texto.IndexOf("\n");
                        texto = texto.Substring(indexInitial);
                        fila["Advances"] = texto;
                    }
                    else
                    {
                        fila["Advances"] = note;
                    }
                }
                else
                {
                    fila["Advances"] = dr["Detail"];
                }

                table.Rows.Add(fila);
            }

            return table;
        }
        #endregion
        
        #region loadData
        private void loadData()
        {
            if (Session["_tblTicket"] != null)
            {
                gvMain.DataSource = Session["_tblTicket"] as DataTable;
            }
            else
            {
                DataTable dt = tblReport();
                gvMain.DataSource = dt;
                Session["_tblTicket"] = dt;
            }
            gvMain.DataBind();
        }
        #endregion

        #region gvMain_HtmlDataCellPrepared
        protected void gvMain_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "SlaResponse")
            {
                if (e.CellValue.ToString() == "NO OK")
                {
                    //e.Cell.BackColor = System.Drawing.Color.Red;                   

                    e.Cell.ForeColor = System.Drawing.Color.Red; 
                }
            }
            if (e.DataColumn.FieldName == "SlaResolution")
            {
                if (e.CellValue.ToString() == "NO OK")
                {
                    //e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.Red;
                }
            }
            if (e.DataColumn.FieldName == "SlaRestoration")
            {
                if (e.CellValue.ToString() == "NO OK")
                {
                    //e.Cell.BackColor = System.Drawing.Color.Red;
                    e.Cell.ForeColor = System.Drawing.Color.Red;
                }
            }
            //Code Created by AM**
            if (e.DataColumn.FieldName == "StatusID")
            {
                                
                if (e.CellValue.ToString() == "7")
                {
                    e.Cell.ForeColor = System.Drawing.Color.Red;                    
                }
                else
                {
                    e.Cell.ForeColor = System.Drawing.Color.LimeGreen;                    
                }
            }

            //End Code AM **

            //if (e.DataColumn.FieldName != "Budget") return;
            //if (Convert.ToInt32(e.CellValue) < 100000)
            //    e.Cell.BackColor = System.Drawing.Color.LightCyan;
        }
        #endregion

        protected void gvMain_ToolbarItemClick(object source, DevExpress.Web.Data.ASPxGridViewToolbarItemClickEventArgs e)
        {
            string filter = ((GridViewDataColumn)gvMain.Columns["BranchID"]).FilterExpression;
            string fileName = "Report_SLA_";
            string trial = Request.QueryString.Get("trial");
            if (!string.IsNullOrEmpty(trial))
            {
                fileName += "Trial_";
            }
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

        protected void gvMain_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {

            
        }

        protected void gvMain_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            
        }

        protected void gvMain_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            
        }
    }
}