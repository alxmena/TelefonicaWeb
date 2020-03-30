using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using System.Data;
using ATCPortal;
using DevExpress.XtraCharts;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ReportsGeneral
/// </summary>
public class ReportsGeneral : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRChart xrChart2;
    private XRChart xrChart1;
    private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
    private XRChart xrChart3;
    private XRChart xrChart4;
    private XRChart xrChart7;
    private XRChart xrChart6;
    private XRChart xrChart5;
    private ArrayList listDataSource;
    private XRControlStyle EventStyle;
    private XRControlStyle HeaderStyle;
    private XRControlStyle OddStyle;
    private XRLabel xrLabel1;
    private ReportHeaderBand ReportHeader;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell3;
    private XRTable xrTable2;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell6;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public ReportsGeneral(int BranchID, List<object> OEMID, List<object> TechnologyID, List<object> StatusID, List<object> SeverityID, DateTime DateInit, DateTime DateFinish)
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
        listDataSource = new ArrayList();
        LoadReportTicketsVendor(BranchID, OEMID, TechnologyID, StatusID, SeverityID, DateInit, DateFinish);
        LoadReportTicketsTechnology(BranchID, OEMID, TechnologyID, StatusID, SeverityID, DateInit, DateFinish);
    }

    private void LoadReportTicketsVendor(int BranchID, List<object> OEMID, List<object> TechnologyID, List<object> StatusID, List<object> SeverityID, DateTime DateInit, DateTime DateFinish)
    {
        //string sqlOem = "select count(1) as Total, t3.Name from tblTicket t1  inner join tblOEMBranch t2 on t2.OEMID = t1.OEMID  inner join tblOEM t3 on t3.ID = t1.OEMID where t1.BranchID = " + BranchID;
        string sql = "select distinct t2.ID, t2.Name from tblOEMBranch t1 inner join tblOEM t2 on t2.ID = t1.OEMID where t1.BranchID = " + BranchID;
        if (OEMID.Count > 0)
        {
            sql += string.Format(" and t1.OEMID in ({0})", string.Join(", ", OEMID.ToArray()));
        }
        DataTable tblOem = DataBase.GetDT(sql);
        List<Series> seriesList1 = new List<Series>();
        List<Series> seriesList2 = new List<Series>();
        List<Series> seriesList3 = new List<Series>();
        List<Series> seriesList4 = new List<Series>();
        DataTable table = new DataTable();
        List<SqlParameter> sp = new List<SqlParameter>();
        foreach (DataRow dr in tblOem.Rows)
        {
            sql = "select count(1) as Total, t3.Name from tblTicket t1 inner join tblOEM t3 on t3.ID = t1.OEMID where t1.BranchID = " + BranchID + " and t1.OEMID = " + dr["ID"].ToString() + " and CAST(t1.CreationDate AS DATE) >= @DateInit and CAST(t1.CreationDate AS DATE) <= @DateFinish group by t3.Name";
            Series series1 = new Series(dr["Name"].ToString(), ViewType.Bar);
            Series series2 = new Series(dr["Name"].ToString(), ViewType.Bar);
            Series series3 = new Series(dr["Name"].ToString(), ViewType.Bar);
            Series series4 = new Series(dr["Name"].ToString(), ViewType.Bar);
            series1.ArgumentDataMember = "Name";
            series1.ValueDataMembers.AddRange("Total");
            sp = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@DateInit", SqlDbType = SqlDbType.Date, Value = DateInit },
                new SqlParameter() { ParameterName = "@DateFinish", SqlDbType = SqlDbType.Date, Value = DateFinish }
            };
            table = DataBase.GetDT(sp, sql);
            series1.DataSource = table;
            seriesList1.Add(series1);
            if (table.Rows.Count > 0)
            {
                listDataSource.Add(new ClsReport(table.Rows[0]["Name"].ToString(), dr["Name"].ToString(), Convert.ToInt32(table.Rows[0]["Total"])));
            }
            if (TechnologyID.Count > 0)
            {
                sql = "select count(1) as Total, t3.Name from tblTicket t1 left join tblTechnology t3 on t3.ID = t1.TechnologyID where t1.BranchID = {0} and t1.OEMID = {1} and t1.TechnologyID in ({2}) and CAST(t1.CreationDate AS DATE) >= @DateInit and CAST(t1.CreationDate AS DATE) <= @DateFinish  group by t3.Name";
                sql = string.Format(sql, BranchID, dr["ID"].ToString(), string.Join(", ", TechnologyID.ToArray()));
                series2.ArgumentDataMember = "Name";
                series2.ValueDataMembers.AddRange("Total");
                sp = new List<SqlParameter>()
                {
                        new SqlParameter() { ParameterName = "@DateInit", SqlDbType = SqlDbType.Date, Value = DateInit },
                        new SqlParameter() { ParameterName = "@DateFinish", SqlDbType = SqlDbType.Date, Value = DateFinish }
                };
                table = DataBase.GetDT(sp, sql);
                series2.DataSource = table;
                seriesList2.Add(series2);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow dr1 in table.Rows)
                    {
                        listDataSource.Add(new ClsReport(dr1["Name"].ToString(), dr["Name"].ToString(), Convert.ToInt32(dr1["Total"])));
                    }
                }
            }

            if (StatusID.Count > 0)
            {
                sql = "select count(1) as Total, t2.Name from tblTicket t1 right join tblStatus t2 on t2.ID = t1.StatusID where t1.BranchID = {0} and t1.OEMID = {1} and t1.StatusID in ({2}) and CAST(t1.CreationDate AS DATE) >= @DateInit and CAST(t1.CreationDate AS DATE) <= @DateFinish group by t2.Name";
                sql = string.Format(sql, BranchID, dr["ID"].ToString(), string.Join(", ", StatusID.ToArray()));
                series3.ArgumentDataMember = "Name";
                series3.ValueDataMembers.AddRange("Total");
                sp = new List<SqlParameter>()
                {
                    new SqlParameter() { ParameterName = "@DateInit", SqlDbType = SqlDbType.Date, Value = DateInit },
                    new SqlParameter() { ParameterName = "@DateFinish", SqlDbType = SqlDbType.Date, Value = DateFinish }
                };
                table = DataBase.GetDT(sp, sql);
                series3.DataSource = table;
                seriesList3.Add(series3);

                if (table.Rows.Count > 0)
                {
                    foreach (DataRow dr1 in table.Rows)
                    {
                        listDataSource.Add(new ClsReport(dr1["Name"].ToString(), dr["Name"].ToString(), Convert.ToInt32(dr1["Total"])));
                    }
                }
            }

            if (SeverityID.Count > 0)
            {
                sql = "select count(1) as Total, t2.Name from tblTicket t1 right join tblSeverity t2 on t2.ID = t1.SeverityID where t1.BranchID = {0} and t1.OEMID = {1} and t1.SeverityID in ({2}) and CAST(t1.CreationDate AS DATE) >= @DateInit and CAST(t1.CreationDate AS DATE) <= @DateFinish group by t2.Name";
                sql = string.Format(sql, BranchID, dr["ID"].ToString(), string.Join(", ", SeverityID.ToArray()));
                series4.ArgumentDataMember = "Name";
                series4.ValueDataMembers.AddRange("Total");
                sp = new List<SqlParameter>()
                {
                    new SqlParameter() { ParameterName = "@DateInit", SqlDbType = SqlDbType.Date, Value = DateInit },
                    new SqlParameter() { ParameterName = "@DateFinish", SqlDbType = SqlDbType.Date, Value = DateFinish }
                };
                table = DataBase.GetDT(sp, sql);
                series4.DataSource = table;
                seriesList4.Add(series4);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow dr1 in table.Rows)
                    {
                        listDataSource.Add(new ClsReport(dr1["Name"].ToString(), dr["Name"].ToString(), Convert.ToInt32(dr1["Total"])));
                    }
                }
            }
        }

        xrChart1.Series.AddRange(seriesList1.ToArray());
        ChartTitle title1 = new ChartTitle();
        ChartTitle title2 = new ChartTitle();
        ChartTitle title3 = new ChartTitle();
        ChartTitle title4 = new ChartTitle();
        title1.Text = "Total Per Vendor";
        title1.Alignment = StringAlignment.Center;
        title1.Font = new Font("Tahoma", 10, FontStyle.Bold);
        xrChart1.Titles.Add(title1);
        XRChart chartDisp = null;
        if (seriesList2.Count > 0)
        {
            title2.Text = "Total Per Vendor - Technology";
            title2.Alignment = StringAlignment.Center;
            title2.Font = new Font("Tahoma", 10, FontStyle.Bold);
            chartDisp = getDisp();
            chartDisp.Series.AddRange(seriesList2.ToArray());
            chartDisp.Titles.Add(title2);
        }

        if (seriesList3.Count > 0)
        {
            title3.Text = "Total Per Vendor - Status";
            title3.Alignment = StringAlignment.Center;
            title3.Font = new Font("Tahoma", 10, FontStyle.Bold);
            chartDisp = getDisp();
            chartDisp.Series.AddRange(seriesList3.ToArray());
            chartDisp.Titles.Add(title3);

        }

        if (seriesList4.Count > 0)
        {
            title4.Text = "Total Per Vendor - Severity";
            title4.Alignment = StringAlignment.Center;
            title4.Font = new Font("Tahoma", 10, FontStyle.Bold);
            chartDisp = getDisp();
            chartDisp.Series.AddRange(seriesList4.ToArray());
            chartDisp.Titles.Add(title4);
        }
    }

    private void LoadReportTicketsTechnology(int BranchID, List<object> OEMID, List<object> TechnologyID, List<object> StatusID, List<object> SeverityID, DateTime DateInit, DateTime DateFinish)
    {
        string sql = string.Empty;
        if (TechnologyID.Count > 0)
        {
            sql = "select distinct t2.ID, t2.Name + '-' + t3.Name as Name, t3.Name as Vendor, t1.OEMID from tblOEMBranch t1 inner join tblTechnology t2 on t2.ID = t1.TechnologyID inner join tblOEM t3 on t3.ID = t1.OEMID where t1.TechnologyID in ({0}) and t1.BranchID = {1} order by t3.Name";
            sql = string.Format(sql, string.Join(", ", TechnologyID.ToArray()), BranchID);
            DataTable tblTechnology = DataBase.GetDT(sql);
            List<Series> seriesList1 = new List<Series>();
            List<Series> seriesList2 = new List<Series>();
            List<Series> seriesList3 = new List<Series>();
            DataTable table = new DataTable();
            List<SqlParameter> sp = new List<SqlParameter>();

            foreach (DataRow dr in tblTechnology.Rows)
            {
                sql = "select count(1) as Total, t2.Name + '-' + t3.Name as Name from tblTicket t1 inner join tblTechnology t2 on t2.ID = t1.TechnologyID inner join tblOEM t3 on t3.ID = t1.OEMID where t1.BranchID = {0} and t1.TechnologyID = {1} and t1.OEMID = {2} and CAST(t1.CreationDate AS DATE) >= @DateInit and CAST(t1.CreationDate AS DATE) <= @DateFinish group by t2.Name, t3.Name";
                sql = string.Format(sql, BranchID, dr["id"].ToString(), dr["OEMID"].ToString());
                Series series1 = new Series(dr["Name"].ToString(), ViewType.Bar);
                Series series2 = new Series(dr["Name"].ToString(), ViewType.Bar);
                Series series3 = new Series(dr["Name"].ToString(), ViewType.Bar);
                series1.ArgumentDataMember = "Name";
                series1.ValueDataMembers.AddRange("Total");
                sp = new List<SqlParameter>()
                {
                    new SqlParameter() { ParameterName = "@DateInit", SqlDbType = SqlDbType.Date, Value = DateInit },
                    new SqlParameter() { ParameterName = "@DateFinish", SqlDbType = SqlDbType.Date, Value = DateFinish }
                };
                table = DataBase.GetDT(sp, sql);
                series1.DataSource = table;
                seriesList1.Add(series1);

                //if (table.Rows.Count > 0)
                //{
                //    listDataSource.Add(new ClsReport(table.Rows[0]["Name"].ToString(), dr["Vendor"].ToString(), Convert.ToInt32(table.Rows[0]["Total"])));
                //}

                if (StatusID.Count > 0)
                {
                    sql = "select count(1) as Total, t2.Name from tblTicket t1 right join tblStatus t2 on t2.ID = t1.StatusID where t1.BranchID = {0} and t1.TechnologyID = {1} and t1.OEMID = {2} and t1.StatusID in ({3}) and CAST(t1.CreationDate AS DATE) >= @DateInit and CAST(t1.CreationDate AS DATE) <= @DateFinish group by t2.Name, t1.OEMID";
                    sql = string.Format(sql, BranchID, dr["ID"].ToString(), dr["OEMID"].ToString(), string.Join(", ", StatusID.ToArray()));
                    series2.ArgumentDataMember = "Name";
                    series2.ValueDataMembers.AddRange("Total");
                    sp = new List<SqlParameter>()
                    {
                        new SqlParameter() { ParameterName = "@DateInit", SqlDbType = SqlDbType.Date, Value = DateInit },
                        new SqlParameter() { ParameterName = "@DateFinish", SqlDbType = SqlDbType.Date, Value = DateFinish }
                    };
                    table = DataBase.GetDT(sp, sql);
                    series2.DataSource = table;
                    seriesList2.Add(series2);
                    //if (table.Rows.Count > 0)
                    //{
                    //    foreach (DataRow dr1 in table.Rows)
                    //    {
                    //        listDataSource.Add(new ClsReport(dr1["Name"].ToString(), dr["Name"].ToString(), Convert.ToInt32(dr1["Total"])));
                    //    }
                    //}
                }

                if (SeverityID.Count > 0)
                {
                    sql = "select count(1) as Total, t2.Name from tblTicket t1 right join tblSeverity t2 on t2.ID = t1.SeverityID where t1.BranchID = {0} and t1.TechnologyID = {1} and t1.OEMID = {2} and t1.SeverityID in ({3}) and CAST(t1.CreationDate AS DATE) >= @DateInit and CAST(t1.CreationDate AS DATE) <= @DateFinish group by t2.Name, t1.OEMID";
                    sql = string.Format(sql, BranchID, dr["ID"].ToString(), dr["OEMID"].ToString(), string.Join(", ", SeverityID.ToArray()));
                    series3.ArgumentDataMember = "Name";
                    series3.ValueDataMembers.AddRange("Total");
                    sp = new List<SqlParameter>()
                    {
                        new SqlParameter() { ParameterName = "@DateInit", SqlDbType = SqlDbType.Date, Value = DateInit },
                        new SqlParameter() { ParameterName = "@DateFinish", SqlDbType = SqlDbType.Date, Value = DateFinish }
                    };
                    table = DataBase.GetDT(sp, sql);
                    series3.DataSource = table;
                    seriesList3.Add(series3);
                    //if (table.Rows.Count > 0)
                    //{
                    //    foreach (DataRow dr1 in table.Rows)
                    //    {
                    //        listDataSource.Add(new ClsReport(dr1["Name"].ToString(), dr["Name"].ToString(), Convert.ToInt32(dr1["Total"])));
                    //    }
                    //}
                }
            }

            ChartTitle title1 = new ChartTitle();
            ChartTitle title2 = new ChartTitle();
            ChartTitle title3 = new ChartTitle();
            XRChart chartDisp = getDisp();
            chartDisp.Series.AddRange(seriesList1.ToArray());

            title1.Text = "Total Per Technology";
            title1.Alignment = StringAlignment.Center;
            title1.Font = new Font("Tahoma", 10, FontStyle.Bold);
            chartDisp.Titles.Add(title1);
            

            if (seriesList2.Count > 0)
            {
                title2.Text = "Total Per Technology - Status";
                title2.Alignment = StringAlignment.Center;
                title2.Font = new Font("Tahoma", 10, FontStyle.Bold);
                chartDisp = getDisp();
                chartDisp.Series.AddRange(seriesList2.ToArray());
                chartDisp.Titles.Add(title2);
            }

            if (seriesList3.Count > 0)
            {
                title3.Text = "Total Per Technology - Severity";
                title3.Alignment = StringAlignment.Center;
                title3.Font = new Font("Tahoma", 10, FontStyle.Bold);
                chartDisp = getDisp();
                chartDisp.Series.AddRange(seriesList3.ToArray());
                chartDisp.Titles.Add(title3);
            }
        }
    }

    private XRChart getDisp()
    {
        List<XRChart> list = new List<XRChart>()
        {
            xrChart1, xrChart2, xrChart3, xrChart4, xrChart5, xrChart6, xrChart7
        };
        XRChart disp = null;
        foreach (XRChart i in list)
        {
            if (i.Series.Count == 0)
            {
                disp = i;
                break;
            }
        }
        return disp;
    }

    private void moveDetailBand()
    {
        
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
            this.xrChart7 = new DevExpress.XtraReports.UI.XRChart();
            this.xrChart6 = new DevExpress.XtraReports.UI.XRChart();
            this.xrChart5 = new DevExpress.XtraReports.UI.XRChart();
            this.xrChart4 = new DevExpress.XtraReports.UI.XRChart();
            this.xrChart3 = new DevExpress.XtraReports.UI.XRChart();
            this.xrChart2 = new DevExpress.XtraReports.UI.XRChart();
            this.xrChart1 = new DevExpress.XtraReports.UI.XRChart();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.EventStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.HeaderStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.OddStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.Detail.Dpi = 96F;
            this.Detail.HeightF = 24F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 96F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable1
            // 
            this.xrTable1.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.xrTable1.Dpi = 96F;
            this.xrTable1.EvenStyleName = "EventStyle";
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(145.1467F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.OddStyleName = "OddStyle";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(497.9999F, 24F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3});
            this.xrTableRow1.Dpi = 96F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.CanGrow = false;
            this.xrTableCell1.Dpi = 96F;
            this.xrTableCell1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[OEM]")});
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 3, 3, 0, 96F);
            this.xrTableCell1.StylePriority.UsePadding = false;
            this.xrTableCell1.Text = "xrTableCell1";
            this.xrTableCell1.Weight = 1.6300008683328984D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.CanGrow = false;
            this.xrTableCell2.Dpi = 96F;
            this.xrTableCell2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Name]")});
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 3, 3, 0, 96F);
            this.xrTableCell2.StylePriority.UsePadding = false;
            this.xrTableCell2.Text = "xrTableCell2";
            this.xrTableCell2.Weight = 2.0297211971690876D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.CanGrow = false;
            this.xrTableCell3.Dpi = 96F;
            this.xrTableCell3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Total]")});
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 3, 3, 0, 96F);
            this.xrTableCell3.StylePriority.UsePadding = false;
            this.xrTableCell3.Text = "xrTableCell3";
            this.xrTableCell3.Weight = 1.5277769734375721D;
            // 
            // xrChart7
            // 
            this.xrChart7.BorderColor = System.Drawing.Color.Black;
            this.xrChart7.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrChart7.Dpi = 96F;
            this.xrChart7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Tag", "[Name]")});
            this.xrChart7.Legend.Name = "Default Legend";
            this.xrChart7.LocationFloat = new DevExpress.Utils.PointFloat(1.017253E-05F, 612.6667F);
            this.xrChart7.Name = "xrChart7";
            this.xrChart7.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.xrChart7.SizeF = new System.Drawing.SizeF(384F, 191.9999F);
            // 
            // xrChart6
            // 
            this.xrChart6.BorderColor = System.Drawing.Color.Black;
            this.xrChart6.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrChart6.Dpi = 96F;
            this.xrChart6.Legend.Name = "Default Legend";
            this.xrChart6.LocationFloat = new DevExpress.Utils.PointFloat(408.6667F, 406F);
            this.xrChart6.Name = "xrChart6";
            this.xrChart6.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.xrChart6.SizeF = new System.Drawing.SizeF(381.4134F, 192F);
            // 
            // xrChart5
            // 
            this.xrChart5.BorderColor = System.Drawing.Color.Black;
            this.xrChart5.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrChart5.Dpi = 96F;
            this.xrChart5.Legend.Name = "Default Legend";
            this.xrChart5.LocationFloat = new DevExpress.Utils.PointFloat(1.017253E-05F, 406F);
            this.xrChart5.Name = "xrChart5";
            this.xrChart5.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.xrChart5.SizeF = new System.Drawing.SizeF(384F, 192F);
            // 
            // xrChart4
            // 
            this.xrChart4.BorderColor = System.Drawing.Color.Black;
            this.xrChart4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrChart4.Dpi = 96F;
            this.xrChart4.Legend.Name = "Default Legend";
            this.xrChart4.LocationFloat = new DevExpress.Utils.PointFloat(408.6667F, 204F);
            this.xrChart4.Name = "xrChart4";
            this.xrChart4.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.xrChart4.SizeF = new System.Drawing.SizeF(381.4132F, 192F);
            // 
            // xrChart3
            // 
            this.xrChart3.BorderColor = System.Drawing.Color.Black;
            this.xrChart3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrChart3.Dpi = 96F;
            this.xrChart3.Legend.Name = "Default Legend";
            this.xrChart3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 204F);
            this.xrChart3.Name = "xrChart3";
            this.xrChart3.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.xrChart3.SizeF = new System.Drawing.SizeF(384F, 192F);
            // 
            // xrChart2
            // 
            this.xrChart2.BorderColor = System.Drawing.Color.Black;
            this.xrChart2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrChart2.Dpi = 96F;
            this.xrChart2.Legend.Name = "Default Legend";
            this.xrChart2.LocationFloat = new DevExpress.Utils.PointFloat(408.6667F, 0F);
            this.xrChart2.Name = "xrChart2";
            this.xrChart2.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.xrChart2.SizeF = new System.Drawing.SizeF(381.4134F, 192F);
            // 
            // xrChart1
            // 
            this.xrChart1.BorderColor = System.Drawing.Color.Black;
            this.xrChart1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrChart1.Dpi = 96F;
            this.xrChart1.Legend.Name = "Default Legend";
            this.xrChart1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrChart1.Name = "xrChart1";
            this.xrChart1.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.xrChart1.SizeF = new System.Drawing.SizeF(384F, 192F);
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
            this.TopMargin.Dpi = 96F;
            this.TopMargin.HeightF = 47F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 96F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 96F;
            this.xrLabel1.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(301.6267F, 9.599997F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(178.6667F, 22.08F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Report Ticket";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 96F;
            this.BottomMargin.HeightF = 43F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 96F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
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
            this.EventStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 96F);
            // 
            // HeaderStyle
            // 
            this.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.HeaderStyle.BorderColor = System.Drawing.Color.Gray;
            this.HeaderStyle.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.HeaderStyle.BorderWidth = 1F;
            this.HeaderStyle.Font = new System.Drawing.Font("Segoe UI Black", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.HeaderStyle.Name = "HeaderStyle";
            this.HeaderStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0, 96F);
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
            this.OddStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 96F);
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2,
            this.xrChart7,
            this.xrChart6,
            this.xrChart1,
            this.xrChart4,
            this.xrChart3,
            this.xrChart2,
            this.xrChart5});
            this.ReportHeader.Dpi = 96F;
            this.ReportHeader.HeightF = 847F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrTable2
            // 
            this.xrTable2.Dpi = 96F;
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(145.1467F, 823.0667F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(497.9999F, 24F);
            this.xrTable2.StyleName = "HeaderStyle";
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell6});
            this.xrTableRow2.Dpi = 96F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 96F;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 3, 3, 0, 96F);
            this.xrTableCell4.StylePriority.UsePadding = false;
            this.xrTableCell4.Text = "Vendor";
            this.xrTableCell4.Weight = 1.0572976677704904D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Dpi = 96F;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 3, 3, 0, 96F);
            this.xrTableCell5.StylePriority.UsePadding = false;
            this.xrTableCell5.Text = "Filter";
            this.xrTableCell5.Weight = 1.3165764313999724D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Dpi = 96F;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 3, 3, 0, 96F);
            this.xrTableCell6.StylePriority.UsePadding = false;
            this.xrTableCell6.Text = "Total";
            this.xrTableCell6.Weight = 0.99099061589297621D;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(ATCPortal.ClsReport);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // ReportsGeneral
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.Dpi = 96F;
            this.Margins = new System.Drawing.Printing.Margins(11, 14, 47, 43);
            this.PageHeight = 1056;
            this.PageWidth = 816;
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.Pixels;
            this.SnapGridSize = 12.5F;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.EventStyle,
            this.HeaderStyle,
            this.OddStyle});
            this.Version = "17.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
