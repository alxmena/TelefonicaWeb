using System;
using System.Data;

namespace ClsQNotifications
{
    public class NotificationSla
    {
        public void NotificateRestoration()
        {
            string query = @"
                select
                t1.ID,
                t1.SeverityID,
                t1.ProblemTitle,
                t2.Name as Priority,
                t3.Name as Branch,
                t4.ResolutionTime,
                DATEDIFF(DAY, T1.RestorationDate, GETDATE()) as TotalDays
                from tblTicket t1
                inner join tblSeverity t2 on t2.ID = t1.SeverityID
                inner join tblBranch t3 on t3.ID = t1.BranchID
                inner join tblSlaInfo t4 on t4.SlaID = t3.SlaID and t4.Name = RIGHT(t2.Name, 2)
                where StatusID = 4 and DATEDIFF(DAY, T1.RestorationDate, GETDATE()) * 100 / CAST(LEFT(t4.ResolutionTime, CHARINDEX(':', t4.ResolutionTime) - 1) as int) > 75
                and  'Ticket #' + cast(t1.ID as nvarchar) + ' SLA Resolution' not in (select Subject from tblNotifications where TicketID = t1.ID)
            ";
            DataTable dt = DataBase.GetDT(query);

            foreach (DataRow dr in dt.Rows)
            {
                int TicketID = Convert.ToInt32(dr["ID"]);
                string Status = "SENT";
                string Subject = string.Format("Ticket #{0} SLA Resolution", TicketID);
                string Message = "<p>The date of the SLA Ticket # " + TicketID + " is about to end in Resolution</p><p>Ticket Tile: " + dr["ProblemTitle"].ToString() + "</p>";
                Message += "<p>Priority: " + dr["Priority"].ToString()  + "</p><p>Branch: " + dr["Branch"].ToString() + "</p>";
                Message += "<p>Resolution time SLA: " + dr["ResolutionTime"].ToString() + "</p><p>Total Days Ticket: " + dr["TotalDays"].ToString() + "</p>";
                string mailSend = Email.SendEmail(Subject, Message, "sandra.triana@amerinode.com");
                if (mailSend.Contains("Error"))
                {
                    Status = "CREATED";
                }
                Notifications.add(Subject, Message, "q@amerinode.com", "sandra.triana@amerinode.com", TicketID, Status);
            }
        }
    }
}
