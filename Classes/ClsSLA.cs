using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ATCPortal
{
    public class ClsSlaData
    {
        public string timeSla { get; set; }
        public string inSla { get; set; }
        public string slaFromPendingInfo { get; set; }
    }

    public class ClsSLA
    {
        private int IdTicket, SeverityID, BranchID;
        private DataTable _tblPendingInfo = null;
        private DataTable _tblSla = null;
        private bool trial;

        public ClsSLA(int idTicket, int severity, int branchID, bool trial = false)
        {
            this.SeverityID = severity;
            this.IdTicket = idTicket;
            this.BranchID = branchID;
            this.trial = trial;
        }

        #region getSla return SlaData
        public ClsSlaData getSla(DateTime time1, DateTime time2, int status)
        {
            bool sla = true;
            ClsSlaData returnData = new ClsSlaData();
            DataRow[] infoRow = null;
            DataRow[] slaRow = null;
            DataTable tblPending = tblPendingInfo(this.IdTicket);

            TimeSpan timeTotal = new TimeSpan();
            TimeSpan accumTotal = new TimeSpan();
            TimeSpan accumTotalClient = new TimeSpan();

            switch (status)
            {
                case 2:
                    returnData = new ClsSlaData();
                    infoRow = tblPending.Select("PreviousStatus = 1 OR PreviousStatus = 2");
                    slaRow = tblSla().Select("Name = '" + getSeverityByID(this.SeverityID) + "'");
                    if (slaRow.Length > 0 && !string.IsNullOrEmpty(slaRow[0]["ResponseTime"].ToString()))
                    {
                        string timeSlaCum = slaRow[0]["ResponseTime"].ToString();
                        if (infoRow.Length > 0)
                        {
                            foreach (DataRow dri in infoRow)
                            {
                                TimeSpan _time = new TimeSpan();
                                if (!string.IsNullOrEmpty(dri["PendingInfoOff"].ToString()))
                                {
                                    _time = DateTime.Parse(dri["PendingInfoOff"].ToString()) - DateTime.Parse(dri["PendingInfoOn"].ToString());
                                    accumTotal += _time;
                                    accumTotalClient += _time;
                                }
                                else
                                {
                                    accumTotal += time2 - DateTime.Parse(dri["PendingInfoOn"].ToString());
                                    accumTotalClient += DateTime.Now - DateTime.Parse(dri["PendingInfoOn"].ToString());
                                }
                            }
                        }


                        if (this.trial || slaRow[0]["Availability"].ToString() == "8*5")
                        {
                            timeTotal = calculate8x5(time1, time2, true) - accumTotal;
                        }
                        else
                        {
                            returnData.slaFromPendingInfo = accumTotal.ToString(@"dd\.hh\:mm\:ss");
                            timeTotal = time2 - time1 - accumTotal;
                        }

                        if (slaRow.Length > 0)
                        {

                            sla = getInSla(timeSlaCum, timeTotal);
                        }
                        returnData.timeSla = timeTotal.ToString(@"dd\.hh\:mm\:ss");
                        returnData.inSla = sla ? "OK" : "NO OK";
                    }
                    else
                    {
                        returnData.timeSla = "NA";
                        returnData.inSla = "NA";
                        returnData.slaFromPendingInfo = "NA";
                    }
                    
                    break;
                case 3:
                    returnData = new ClsSlaData();
                    infoRow = tblPending.Select("PreviousStatus = 3");
                    slaRow = tblSla().Select("Name = '" + getSeverityByID(this.SeverityID) + "'");
                    if (slaRow.Length > 0 && !string.IsNullOrEmpty(slaRow[0]["RecoveryTime"].ToString()))
                    {
                        string timeSlaCum = slaRow[0]["RecoveryTime"].ToString();
                        if (infoRow.Length > 0)
                        {
                            foreach (DataRow dri in infoRow)
                            {
                                TimeSpan _time = new TimeSpan();
                                if (!string.IsNullOrEmpty(dri["PendingInfoOff"].ToString()))
                                {
                                    _time = DateTime.Parse(dri["PendingInfoOff"].ToString()) - DateTime.Parse(dri["PendingInfoOn"].ToString());
                                    accumTotal += _time;

                                    if (slaRow[0]["Availability"].ToString() == "8*5")
                                    {
                                        accumTotalClient += calculate8x5(DateTime.Parse(dri["PendingInfoOn"].ToString()), DateTime.Parse(dri["PendingInfoOff"].ToString()));
                                    }
                                    else
                                    {
                                        accumTotalClient += _time;
                                    }
                                }
                                else
                                {
                                    accumTotal += time2 - DateTime.Parse(dri["PendingInfoOn"].ToString());
                                    if (slaRow[0]["Availability"].ToString() == "8*5")
                                    {
                                        accumTotalClient += calculate8x5(DateTime.Parse(dri["PendingInfoOn"].ToString()), DateTime.Now);
                                    }
                                    else
                                    {
                                        accumTotalClient += DateTime.Now - DateTime.Parse(dri["PendingInfoOn"].ToString());
                                    }

                                }
                            }
                        }

                        if (this.trial || slaRow[0]["Availability"].ToString() == "8*5")
                        {
                            timeTotal = calculate8x5(time1, time2) - accumTotal;
                        }
                        else
                        {
                            timeTotal = time2 - time1 - accumTotal;
                        }
                        if (this.trial)
                        {
                            returnData.slaFromPendingInfo = "NA";
                        }
                        else
                        {
                            returnData.slaFromPendingInfo = accumTotalClient.ToString(@"dd\.hh\:mm\:ss");
                        }

                        sla = getInSla(timeSlaCum, timeTotal);
                        returnData.timeSla = timeTotal.ToString(@"dd\.hh\:mm\:ss");
                        returnData.inSla = sla ? "OK" : "NO OK";
                    }
                    else
                    {
                        returnData.timeSla = "NA";
                        returnData.inSla = "NA";
                        returnData.slaFromPendingInfo = "NA";
                    }
                    
                    break;
                case 4:
                    returnData = new ClsSlaData();
                    infoRow = tblPending.Select("PreviousStatus = 4");
                    slaRow = tblSla().Select("Name = '" + getSeverityByID(this.SeverityID) + "'");
                    if (slaRow.Length > 0 && !string.IsNullOrEmpty(slaRow[0]["ResolutionTime"].ToString()))
                    {
                        string timeSlaCum = slaRow[0]["ResolutionTime"].ToString();
                        if (infoRow.Length > 0)
                        {
                            foreach (DataRow dri in infoRow)
                            {
                                TimeSpan _time = new TimeSpan();
                                if (!string.IsNullOrEmpty(dri["PendingInfoOff"].ToString()))
                                {
                                    _time = DateTime.Parse(dri["PendingInfoOff"].ToString()) - DateTime.Parse(dri["PendingInfoOn"].ToString());
                                    accumTotal += _time;
                                    if (slaRow[0]["Availability"].ToString() == "8*5")
                                    {
                                        accumTotalClient += calculate8x5(DateTime.Parse(dri["PendingInfoOn"].ToString()), DateTime.Parse(dri["PendingInfoOff"].ToString()));
                                    }
                                    else
                                    {
                                        accumTotalClient += _time;
                                    }
                                }
                                else
                                {
                                    accumTotal += time2 - DateTime.Parse(dri["PendingInfoOn"].ToString());
                                    if (slaRow[0]["Availability"].ToString() == "8*5")
                                    {
                                        accumTotalClient += calculate8x5(DateTime.Parse(dri["PendingInfoOn"].ToString()), DateTime.Now);
                                    }
                                    else
                                    {
                                        accumTotalClient += DateTime.Now - DateTime.Parse(dri["PendingInfoOn"].ToString());
                                    }
                                }
                            }
                        }

                        if (this.trial || slaRow[0]["Availability"].ToString() == "8*5")
                        {
                            timeTotal = calculate8x5(time1, time2) - accumTotal;
                        }
                        else
                        {
                            timeTotal = time2 - time1 - accumTotal;
                        }

                        if (this.trial)
                        {
                            returnData.slaFromPendingInfo = "NA";
                        }
                        else
                        {
                            returnData.slaFromPendingInfo = accumTotalClient.ToString(@"dd\.hh\:mm\:ss");
                        }

                        sla = getInSla(timeSlaCum, timeTotal);
                        returnData.timeSla = timeTotal.ToString(@"dd\.hh\:mm\:ss");
                        returnData.inSla = sla ? "OK" : "NO OK";
                    }
                    else
                    {
                        returnData.timeSla = "NA";
                        returnData.inSla = "NA";
                        returnData.slaFromPendingInfo = "NA";
                    }
                    
                    break;
                default:
                    break;
            }

            return returnData;
        }
        #endregion

        #region tblPendingInfo
        private DataTable tblPendingInfo(int id)
        {
            DataTable dt = null;
            if (this._tblPendingInfo == null)
            {
                string query = "SELECT * FROM tblPendingInfo WHERE TicketID = " + id;
                dt = DataBase.GetDT(query);
                this._tblPendingInfo = dt;
            }
            else
            {
                dt = this._tblPendingInfo;
            }

            return dt;
        }
        #endregion
        
        #region getSeverityByID
        private string getSeverityByID(int id)
        {
            string query = "select * from tblSeverity where id = " + id;
            string severity = string.Empty;
            DataTable dt = DataBase.GetDT(query);
            if (dt.Rows.Count > 0)
            {
                severity = dt.Rows[0]["Name"].ToString();
                severity = severity.Split(' ')[1];
            }
            return severity;
        }
        #endregion

        #region tblSla
        private DataTable tblSla()
        {
            DataTable dt = null;
            if (this._tblSla == null)
            {
                string query = "select t2.* from tblBranch t1 inner join tblSlaInfo t2 on t2.SlaID = t1.SlaID where t1.ID = " + this.BranchID;
                dt = DataBase.GetDT(query);
                this._tblSla = dt;
            }
            else
            {
                dt = this._tblSla;
            }

            return dt;
        }
        #endregion

        #region getInSla
        private bool getInSla(string slaTime, TimeSpan timeAcum)
        {
            int timeSla = Convert.ToInt32(slaTime.Split(':')[0]);
            bool flag = true;
            switch (slaTime.Split(':')[1])
            {
                case "d":
                    if (timeAcum.TotalDays > timeSla)
                    {
                        flag = false;
                    }
                    break;
                case "h":
                    if (timeAcum.TotalHours > timeSla)
                    {
                        flag = false;
                    }
                    break;
                case "m":
                    if (timeAcum.TotalMinutes > timeSla)
                    {
                        flag = false;
                    }
                    break;
            }
            return flag;
        }
        #endregion

        #region calculateHoursPerDays
        private TimeSpan calculateHoursPerDays(DateTime date1, DateTime date2, int hours, bool validateInitial)
        {
            TimeSpan timeTotal = new TimeSpan();
            if ((date1.Hour >= 17 || date1.Hour >= 0 && date1.Hour < 8) && date2.Month == date1.Month && validateInitial)
            {
                if (date2.Hour >= 8 && (date1.Day != date2.Day))
                {
                    int hour = 0;
                    if (date1.Hour >= 0 && date1.Hour < 9)
                    {
                        hour = 9 - date1.Hour;
                    }
                    else
                    {
                        hour = (24 - date1.Hour) + 8;
                    }
                    date1 = date1.AddHours(hour);
                    date1 = date1.AddMinutes(-date1.Minute);
                    date1 = date1.AddSeconds(-date1.Second);
                    if (date2 > date1)
                    {
                        timeTotal = date2 - date1;
                    }
                    else
                    {
                        timeTotal = date1 - date2;
                    }
                }

                if (timeTotal.Days > 0)
                {
                    timeTotal = timeTotal - TimeSpan.FromHours(timeTotal.TotalDays * hours);
                    timeTotal = timeTotal - TimeSpan.FromHours(calculateDaysWeekend(date1, date2));
                }
            }
            else
            {
                timeTotal = date2 - date1;
                if (timeTotal.TotalHours > 9 && timeTotal.TotalHours < 24)
                {
                    timeTotal = timeTotal - TimeSpan.FromHours(hours);
                }
                else
                {
                    timeTotal = timeTotal - TimeSpan.FromHours(timeTotal.Days * hours);
                }
                timeTotal = timeTotal - TimeSpan.FromHours(calculateDaysWeekend(date1, date2));
            }
            return timeTotal;
        }
        #endregion

        #region calculate8x5
        private TimeSpan calculate8x5(DateTime date1, DateTime date2, bool validateInitial = false)
        {
            return calculateHoursPerDays(date1, date2, 15, validateInitial);
        }
        #endregion

        #region calculateDaysWeekend
        private int calculateDaysWeekend(DateTime date1, DateTime date2)
        {
            int count = 0;
            while (date1 <= date2)
            {
                if (date1.DayOfWeek == DayOfWeek.Saturday || date1.DayOfWeek == DayOfWeek.Sunday)
                {
                    count += 1;
                }
                date1 = date1.AddDays(1);
            }

            return count * 16;
        }
        #endregion
    }
}