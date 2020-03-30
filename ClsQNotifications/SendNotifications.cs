using System;
using System.Data;

namespace ClsQNotifications
{
    public class SendNotifications
    {
        public void Start()
        {
            string query = "SELECT * FROM tblNotifications WHERE Status = 'CREATED'";
            DataTable dt = DataBase.GetDT(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string responseMail = Email.SendEmail(dr["Subject"].ToString(), dr["Message"].ToString(), dr["ToMail"].ToString());
                    if (!responseMail.Contains("Error"))
                    {
                        Notifications.updateStatus("SENT", Convert.ToInt32(dr["ID"]));
                    }
                }
            }

        }
    }
}
