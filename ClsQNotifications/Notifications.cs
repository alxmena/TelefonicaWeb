using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;

namespace ClsQNotifications
{
    public class Notifications
    {
        public static void add(string Subject, string Message, string fromMail, string ToMail, Int64 TicketID, string Status = "CREATED")
        {
            string query = "insert into tblNotifications values(@Subject, @Message, @FromMail, @ToMail, @TicketID, @Status, @CreatedAt, @UpdatedAt)";
            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@Subject", SqlDbType = SqlDbType.NVarChar, Value = Subject},
                new SqlParameter() { ParameterName = "@Message", SqlDbType = SqlDbType.NVarChar, Value = Message},
                new SqlParameter() { ParameterName = "@FromMail", SqlDbType = SqlDbType.NVarChar, Value = fromMail},
                new SqlParameter() { ParameterName = "@ToMail", SqlDbType = SqlDbType.NVarChar, Value = ToMail},
                new SqlParameter() { ParameterName = "@Status", SqlDbType = SqlDbType.NVarChar, Value =  Status},
                new SqlParameter() { ParameterName = "@CreatedAt", SqlDbType = SqlDbType.DateTime, Value = DateTime.Now},
                new SqlParameter() { ParameterName = "@UpdatedAt", SqlDbType = SqlDbType.DateTime, Value = DateTime.Now}
            };
            if (TicketID > 0)
            {
                sp.Add(new SqlParameter() { ParameterName = "@TicketID", SqlDbType = SqlDbType.BigInt, Value = TicketID });
            }
            else
            {
                query = "insert into tblNotifications values(@Subject, @Message, @FromMail, @ToMail, NULL, @Status, @CreatedAt, @UpdatedAt)";
            }

            DataBase.UpdateDB(sp, query);
        }

        public static void updateStatus(string Status, int Id)
        {
            string query = "UPDATE tblNotifications SET Status = @Status, UpdatedAt = @UpdatedAt WHERE ID = @ID";
            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@Status", SqlDbType = SqlDbType.NVarChar, Value = Status},
                new SqlParameter() { ParameterName = "@ID", SqlDbType = SqlDbType.NVarChar, Value = Id},
                new SqlParameter() { ParameterName = "@UpdatedAt", SqlDbType = SqlDbType.DateTime, Value = DateTime.Now}
            };
            DataBase.UpdateDB(sp, query);
        }
    }
}
