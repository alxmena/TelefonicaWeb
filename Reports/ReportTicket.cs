using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using ATCPortal;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using DevExpress.XtraCharts;
using System.Text;

/// <summary>
/// Summary description for ReportTicket
/// </summary>
public class ReportTicket : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRChart xrChart1;
    //private DataTable DT;
    private XRLabel xrLabel1;
    private PageHeaderBand PageHeader;
    private XRTable xrTable2;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell6;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell3;
    private ArrayList listDataSource;
    private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
    private XRControlStyle HeaderStyle;
    private XRControlStyle OddStyle;
    private XRControlStyle EventStyle;
    private XRChart xrChart2;
    private XRChart xrChart3;
    private XRChart xrChart4;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public ReportTicket(int BranchID, List<object> OEMID, List<object> TechnologyID, List<object> StatusID, List<object> SeverityID, DateTime DateInit, DateTime DateFinish)
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
        LoadReportTicketsVendor(BranchID, OEMID, TechnologyID, StatusID, SeverityID, DateInit,  DateFinish);
        //LoadDoughnut(BranchID, OEMID, SeverityID, StatusID, DateInit, DateFinish);
        LoadReportTicketsTechnology(BranchID, OEMID, TechnologyID, StatusID, SeverityID, DateInit, DateFinish);
    }
    
    private void LoadReportTicketsVendor(int BranchID, List<object> OEMID, List<object> TechnologyID, List<object> StatusID, List<object> SeverityID, DateTime DateInit, DateTime DateFinish)
    {
        if (OEMID.Count > 0)
        {
            string query = string.Empty;
            string query2 = string.Empty;
            string branch = string.Format("select * from tblBranch where ID = {0}", BranchID);
            DataTable tblBranch = DataBase.GetDT(branch);
        
            #region OEM
            query = string.Format("SELECT * FROM tblOEM WHERE ID in({0})", string.Join(", ", OEMID.ToArray()));
            DataTable dt = DataBase.GetDT(query);
            List<Series> seriesList = new List<Series>();
            List<SqlParameter> sp = new List<SqlParameter>();
            query = "select count(1) as Total, t2.Name, t1.OEMID from tblTicket t1 inner join {0} t2 on t2.ID = t1.{1} where t1.BranchID = @BranchID";
            query += " and t1.CreationDate >= @DateInit and t1.CreationDate <= @DateFinish and t2.ID in ({2}) and t1.OEMID = @OEMID group by t2.Name, t1.OEMID";
            ArrayList listDataSource = new ArrayList();
            Series serieDou = new Series("Serie 1", ViewType.Doughnut);
            foreach (DataRow dr in dt.Rows)
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();

                Series series = new Series(dr["Name"].ToString(), ViewType.Bar);
                series.ArgumentDataMember = "Name";
                series.ValueDataMembers.AddRange("Total");

                //if (TechnologyID.Count > 0)
                //{
                //    sb.Append(string.Format(query, "tblTechnology", "TechnologyID", string.Join(", ", TechnologyID.ToArray())));
                //}

                if (StatusID.Count > 0)
                {
                    if (sb.Length > 1) sb.Append(" union all ");
                    sb.Append(string.Format(query, "tblStatus", "StatusID", string.Join(", ", StatusID.ToArray())));
                }

                if (SeverityID.Count > 0)
                {
                    if (sb.Length > 1) sb.Append(" union all ");
                    sb.Append(string.Format(query, "tblSeverity", "SeverityID", string.Join(", ", SeverityID.ToArray())));
                }

                if (StatusID.Count == 0 && SeverityID.Count == 0)
                {
                    query = "select count(1) as Total, t2.Name from tblTicket t1 inner join tblOEM t2 on t2.ID = t1.OEMID where t1.OEMID = @OEMID group by t2.Name";
                    sb.Append(string.Format(query, "tblTechnology", "TechnologyID", string.Join(", ", SeverityID.ToArray())));
                }
                
                sp = new List<SqlParameter>()
                {
                    new SqlParameter() { ParameterName = "@DateInit", SqlDbType = SqlDbType.Date, Value = DateInit },
                    new SqlParameter() { ParameterName = "@DateFinish", SqlDbType = SqlDbType.Date, Value = DateFinish },
                    new SqlParameter() { ParameterName = "@BranchID", SqlDbType = SqlDbType.Int, Value = tblBranch.Rows[0]["ID"] },
                    new SqlParameter() { ParameterName = "@OEMID", SqlDbType = SqlDbType.Int, Value = dr["ID"] },
                };
                DataTable table = DataBase.GetDT(sp, sb.ToString());
                foreach (DataRow data in table.Rows)
                {
                    listDataSource.Add(new ClsReport(data["Name"].ToString(), dr["Name"].ToString(), Convert.ToInt32(data["Total"])));
                    if (StatusID.Count == 0 && SeverityID.Count == 0)
                    {
                        serieDou.Points.Add(new SeriesPoint(data["Name"], Convert.ToInt32(data["Total"])));
                    }
                    else
                    {
                        serieDou.Points.Add(new SeriesPoint(data["Name"] + " " + dr["Name"], Convert.ToInt32(data["Total"])));
                    }
                }
                series.DataSource = table;
                seriesList.Add(series);
            }
            xrChart2.Series.Add(serieDou);
            xrChart1.Series.AddRange(seriesList.ToArray());
            this.listDataSource = listDataSource;
            serieDou.Label.TextPattern = "{VP:P0} ({V})";
            serieDou.LegendTextPattern = "{A}";
            ((DoughnutSeriesLabel)serieDou.Label).Position = PieSeriesLabelPosition.TwoColumns;
            ((DoughnutSeriesLabel)serieDou.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;
            ((DoughnutSeriesLabel)serieDou.Label).ResolveOverlappingMinIndent = 5;
            #endregion
        }
    }

    private void LoadReportTicketsTechnology(int BranchID, List<object> OEMID, List<object> TechnologyID, List<object> StatusID, List<object> SeverityID, DateTime DateInit, DateTime DateFinish)
    {
        if (TechnologyID.Count > 0)
        {
            string query = string.Empty;
            string query2 = string.Empty;
            string branch = string.Format("select * from tblBranch where ID = {0}", BranchID);
            DataTable tblBranch = DataBase.GetDT(branch);

            #region OEM
            query = string.Format("select t1.TechnologyID as ID, t2.Name + '-' + t3.Name as Name from tblOEMBranch t1 inner join tblTechnology t2 on t2.ID = t1.TechnologyID inner join tblOEM t3 on t3.ID = t1.OEMID where t1.BranchID = {0} and t1.TechnologyID in({1})", BranchID, string.Join(", ", TechnologyID.ToArray()));//string.Format("select t1.ID, t1.Name + '-' + t2.Name as Name from tblTechnology t1 inner join tblOEM t2 on t2.ID = t1.OEMID where t1.ID in({0})", string.Join(", ", TechnologyID.ToArray()));
            DataTable dt = DataBase.GetDT(query);
            List<Series> seriesList = new List<Series>();
            List<SqlParameter> sp = new List<SqlParameter>();
            query = "select count(1) as Total, t2.Name, t1.TechnologyID from tblTicket t1 inner join {0} t2 on t2.ID = t1.{1} where t1.BranchID = @BranchID";
            query += " and t1.CreationDate >= @DateInit and t1.CreationDate <= @DateFinish and t2.ID in ({2}) and t1.TechnologyID = @TechnologyID group by t2.Name, t1.TechnologyID";
            ArrayList listDataSource = new ArrayList();
            Series serieDou = new Series("Serie 1", ViewType.Doughnut);
            foreach (DataRow dr in dt.Rows)
            {
                StringBuilder sb = new StringBuilder();

                Series series = new Series(dr["Name"].ToString(), ViewType.Bar);
                series.ArgumentDataMember = "Name";
                series.ValueDataMembers.AddRange("Total");
            
                if (StatusID.Count > 0)
                {
                    if (sb.Length > 1) sb.Append(" union all ");
                    sb.Append(string.Format(query, "tblStatus", "StatusID", string.Join(", ", StatusID.ToArray())));
                }

                if (SeverityID.Count > 0)
                {
                    if (sb.Length > 1) sb.Append(" union all ");
                    sb.Append(string.Format(query, "tblSeverity", "SeverityID", string.Join(", ", SeverityID.ToArray())));
                }

                if (StatusID.Count == 0 && SeverityID.Count == 0)
                {
                    query = "select count(1) as Total, t2.Name from tblTicket t1 inner join tblTechnology t2 on t2.ID = t1.TechnologyID where t1.TechnologyID = @TechnologyID group by t2.Name";
                    sb.Append(string.Format(query, "tblTechnology", "TechnologyID", string.Join(", ", SeverityID.ToArray())));
                }
                
                sp = new List<SqlParameter>()
                {
                    new SqlParameter() { ParameterName = "@DateInit", SqlDbType = SqlDbType.Date, Value = DateInit },
                    new SqlParameter() { ParameterName = "@DateFinish", SqlDbType = SqlDbType.Date, Value = DateFinish },
                    new SqlParameter() { ParameterName = "@BranchID", SqlDbType = SqlDbType.Int, Value = tblBranch.Rows[0]["ID"] },
                    new SqlParameter() { ParameterName = "@TechnologyID", SqlDbType = SqlDbType.Int, Value = dr["ID"] },
                };
                DataTable table = DataBase.GetDT(sp, sb.ToString());
                foreach (DataRow data in table.Rows)
                {
                    listDataSource.Add(new ClsReport(data["Name"].ToString(), dr["Name"].ToString(), Convert.ToInt32(data["Total"])));
                    if (StatusID.Count == 0 && SeverityID.Count == 0)
                    {
                        serieDou.Points.Add(new SeriesPoint(dr["Name"], Convert.ToInt32(data["Total"])));
                    }
                    else
                    {
                        serieDou.Points.Add(new SeriesPoint(data["Name"] + " " + dr["Name"], Convert.ToInt32(data["Total"])));
                    }
                    
                }
                series.DataSource = table;
                seriesList.Add(series);
            }
            xrChart3.Series.Add(serieDou);
            xrChart4.Series.AddRange(seriesList.ToArray());
            serieDou.Label.TextPattern = "{VP:P0} ({V})";
            serieDou.LegendTextPattern = "{A}";
            ((DoughnutSeriesLabel)serieDou.Label).Position = PieSeriesLabelPosition.TwoColumns;
            ((DoughnutSeriesLabel)serieDou.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;
            ((DoughnutSeriesLabel)serieDou.Label).ResolveOverlappingMinIndent = 5;
            //this.listDataSource = listDataSource;
            #endregion
        }
    }

    private void LoadDoughnut(int BranchId, List<object> OEMID, List<object> SeverityID, List<object> StatusID, DateTime DateInit, DateTime DateFinish)
    {
        string query = string.Empty;
        List<SqlParameter> sp = new List<SqlParameter>();
        DataTable dt = new DataTable();
        #region oem
        Series series = new Series("Series 1", ViewType.Doughnut);
        query = "select count(1) as Total, t2.Name from tblTicket t1 inner join tblOEM t2 on t2.ID = t1.OEMID where t2.ID in ({0}) and t1.BranchID = @BranchID and t1.CreationDate >= @DateInit and t1.CreationDate <= @DateFinish group by t2.Name";
        query = string.Format(query, string.Join(", ", OEMID.ToArray()));
        sp = new List<SqlParameter>()
        {
            new SqlParameter() { ParameterName = "@DateInit", SqlDbType = SqlDbType.Date, Value = DateInit },
            new SqlParameter() { ParameterName = "@DateFinish", SqlDbType = SqlDbType.Date, Value = DateFinish },
            new SqlParameter() { ParameterName = "@BranchID", SqlDbType = SqlDbType.Int, Value = BranchId}
        };

        dt = DataBase.GetDT(sp, query);
        foreach(DataRow dr in dt.Rows)
        {
            series.Points.Add(new SeriesPoint(dr["Name"].ToString(), Convert.ToInt32(dr["Total"])));
        }
        series.Label.TextPattern = "{VP:P0} ({V})";
        series.LegendTextPattern = "{A}";
        ((DoughnutSeriesLabel)series.Label).Position = PieSeriesLabelPosition.TwoColumns;
        ((DoughnutSeriesLabel)series.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;
        ((DoughnutSeriesLabel)series.Label).ResolveOverlappingMinIndent = 5;
        ChartTitle vendorTitle = new ChartTitle();
        vendorTitle.Text = "Total Vendor";
        xrChart2.Titles.Add(vendorTitle);
        xrChart2.Series.Add(series);
        #endregion

        #region Severity
        if (SeverityID.Count > 0 )
        {
            Series seriesSeverity = new Series("Series 1", ViewType.Doughnut);
            query = "select count(1) as Total, t2.Name from tblTicket t1 inner join tblSeverity t2 on t2.ID = t1.SeverityID where t2.ID in ({0}) and t1.BranchID = @BranchID and t1.CreationDate >= @DateInit and t1.CreationDate <= @DateFinish group by t2.Name";
            query = string.Format(query, string.Join(", ", SeverityID.ToArray()));
            sp = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@DateInit", SqlDbType = SqlDbType.Date, Value = DateInit },
                new SqlParameter() { ParameterName = "@DateFinish", SqlDbType = SqlDbType.Date, Value = DateFinish },
                new SqlParameter() { ParameterName = "@BranchID", SqlDbType = SqlDbType.Int, Value = BranchId}
            };

            dt = DataBase.GetDT(sp, query);
            foreach (DataRow dr in dt.Rows)
            {
                seriesSeverity.Points.Add(new SeriesPoint(dr["Name"].ToString(), Convert.ToInt32(dr["Total"])));
            }
            seriesSeverity.Label.TextPattern = "{VP:P0} ({V})";
            seriesSeverity.LegendTextPattern = "{A}";
            ((DoughnutSeriesLabel)seriesSeverity.Label).Position = PieSeriesLabelPosition.TwoColumns;
            ((DoughnutSeriesLabel)seriesSeverity.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;
            ((DoughnutSeriesLabel)seriesSeverity.Label).ResolveOverlappingMinIndent = 5;
            ChartTitle severityTitle = new ChartTitle();
            severityTitle.Text = "Total Severity";
            xrChart3.Titles.Add(severityTitle);
            xrChart3.Series.Add(seriesSeverity);
        }
        else if (StatusID.Count > 0)
        {
            Series seriesStatus = new Series("Series 1", ViewType.Doughnut);
            query = "select count(1) as Total, t2.Name from tblTicket t1 inner join tblStatus t2 on t2.ID = t1.StatusID where t2.ID in ({0}) and t1.BranchID = @BranchID and t1.CreationDate >= @DateInit and t1.CreationDate <= @DateFinish group by t2.Name";
            query = string.Format(query, string.Join(", ", StatusID.ToArray()));
            sp = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@DateInit", SqlDbType = SqlDbType.Date, Value = DateInit },
                new SqlParameter() { ParameterName = "@DateFinish", SqlDbType = SqlDbType.Date, Value = DateFinish },
                new SqlParameter() { ParameterName = "@BranchID", SqlDbType = SqlDbType.Int, Value = BranchId}
            };

            dt = DataBase.GetDT(sp, query);
            foreach (DataRow dr in dt.Rows)
            {
                seriesStatus.Points.Add(new SeriesPoint(dr["Name"].ToString(), Convert.ToInt32(dr["Total"])));
            }
            seriesStatus.Label.TextPattern = "{VP:P0} ({V})";
            seriesStatus.LegendTextPattern = "{A}";
            ((DoughnutSeriesLabel)seriesStatus.Label).Position = PieSeriesLabelPosition.TwoColumns;
            ((DoughnutSeriesLabel)seriesStatus.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;
            ((DoughnutSeriesLabel)seriesStatus.Label).ResolveOverlappingMinIndent = 5;
            ChartTitle severityTitle = new ChartTitle();
            severityTitle.Text = "Total Status";
            xrChart3.Titles.Add(severityTitle);
            xrChart3.Series.Add(seriesStatus);
        }
        #endregion
    }

    public ArrayList customQuery()
    {
        return this.listDataSource;
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrChart1 = new DevExpress.XtraReports.UI.XRChart();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrChart4 = new DevExpress.XtraReports.UI.XRChart();
            this.xrChart3 = new DevExpress.XtraReports.UI.XRChart();
            this.xrChart2 = new DevExpress.XtraReports.UI.XRChart();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.HeaderStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.OddStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.EventStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.BackColor = System.Drawing.Color.Transparent;
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.Detail.HeightF = 25F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseBackColor = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable1
            // 
            this.xrTable1.EvenStyleName = "EventStyle";
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(150.1667F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.OddStyleName = "OddStyle";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(501.3889F, 25F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.BorderColor = System.Drawing.Color.Transparent;
            this.xrTableCell1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Name]")});
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 3, 3, 0, 100F);
            this.xrTableCell1.StylePriority.UseBorderColor = false;
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.StylePriority.UsePadding = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "xrTableCell1";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell1.Weight = 0.90473458689040809D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.BorderColor = System.Drawing.Color.Transparent;
            this.xrTableCell2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[OEM]")});
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 3, 3, 0, 100F);
            this.xrTableCell2.StylePriority.UseBorderColor = false;
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.StylePriority.UsePadding = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "xrTableCell2";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell2.Weight = 0.69016207814362285D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.BorderColor = System.Drawing.Color.Transparent;
            this.xrTableCell3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Total]")});
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 3, 3, 0, 100F);
            this.xrTableCell3.StylePriority.UseBorderColor = false;
            this.xrTableCell3.StylePriority.UseBorders = false;
            this.xrTableCell3.StylePriority.UsePadding = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "xrTableCell3";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell3.Weight = 0.41706994188445867D;
            // 
            // xrChart1
            // 
            this.xrChart1.BorderColor = System.Drawing.Color.Black;
            this.xrChart1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrChart1.Legend.Name = "Default Legend";
            this.xrChart1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 63.19449F);
            this.xrChart1.Name = "xrChart1";
            this.xrChart1.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.xrChart1.SizeF = new System.Drawing.SizeF(789F, 200F);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 11.11111F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 20F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(300F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(201.3889F, 38.27777F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Report Tikets";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 140.9722F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrChart4,
            this.xrChart3,
            this.xrChart2,
            this.xrTable2,
            this.xrChart1,
            this.xrLabel1});
            this.PageHeader.HeightF = 751.3889F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrChart4
            // 
            this.xrChart4.BorderColor = System.Drawing.Color.Black;
            this.xrChart4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrChart4.Legend.Name = "Default Legend";
            this.xrChart4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 275F);
            this.xrChart4.Name = "xrChart4";
            this.xrChart4.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.xrChart4.SizeF = new System.Drawing.SizeF(789F, 200F);
            // 
            // xrChart3
            // 
            this.xrChart3.BorderColor = System.Drawing.Color.Black;
            this.xrChart3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrChart3.Legend.Name = "Default Legend";
            this.xrChart3.LocationFloat = new DevExpress.Utils.PointFloat(400.8055F, 500.0001F);
            this.xrChart3.Name = "xrChart3";
            this.xrChart3.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.xrChart3.SizeF = new System.Drawing.SizeF(388.1945F, 200.0001F);
            // 
            // xrChart2
            // 
            this.xrChart2.BorderColor = System.Drawing.Color.Black;
            this.xrChart2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrChart2.Legend.Name = "Default Legend";
            this.xrChart2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 500.0002F);
            this.xrChart2.Name = "xrChart2";
            this.xrChart2.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.xrChart2.SizeF = new System.Drawing.SizeF(388.824F, 200F);
            // 
            // xrTable2
            // 
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(150.1667F, 725.6945F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(501.3889F, 25F);
            this.xrTable2.StyleName = "HeaderStyle";
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell6});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 3, 3, 0, 100F);
            this.xrTableCell4.StylePriority.UseBorders = false;
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UsePadding = false;
            this.xrTableCell4.Text = "Name";
            this.xrTableCell4.Weight = 1.3490304428415429D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 3, 3, 0, 100F);
            this.xrTableCell5.StylePriority.UseBorders = false;
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.StylePriority.UsePadding = false;
            this.xrTableCell5.Text = "Vendor";
            this.xrTableCell5.Weight = 1.029085984360703D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 3, 3, 0, 100F);
            this.xrTableCell6.StylePriority.UseBorders = false;
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.StylePriority.UsePadding = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "Total";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell6.Weight = 0.62188357279775408D;
            // 
            // HeaderStyle
            // 
            this.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.HeaderStyle.BorderColor = System.Drawing.Color.Gray;
            this.HeaderStyle.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.HeaderStyle.BorderWidth = 1F;
            this.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.HeaderStyle.Name = "HeaderStyle";
            this.HeaderStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0, 100F);
            // 
            // OddStyle
            // 
            this.OddStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.OddStyle.BorderColor = System.Drawing.Color.Transparent;
            this.OddStyle.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.OddStyle.BorderWidth = 1F;
            this.OddStyle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OddStyle.ForeColor = System.Drawing.Color.Black;
            this.OddStyle.Name = "OddStyle";
            this.OddStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            // 
            // EventStyle
            // 
            this.EventStyle.BackColor = System.Drawing.Color.White;
            this.EventStyle.BorderColor = System.Drawing.Color.Transparent;
            this.EventStyle.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.EventStyle.BorderWidth = 1F;
            this.EventStyle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EventStyle.ForeColor = System.Drawing.Color.Black;
            this.EventStyle.Name = "EventStyle";
            this.EventStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(ATCPortal.ClsReport);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // ReportTicket
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.Margins = new System.Drawing.Printing.Margins(29, 32, 11, 141);
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.HeaderStyle,
            this.OddStyle,
            this.EventStyle});
            this.Version = "17.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
    
}
