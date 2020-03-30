using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ATCPortal
{
    public class ValidateRequest
    {
        public static void SetDataSession(string username)
        {
            string query = "SELECT CompanyID, BranchID from aspnet_Users WHERE UserName = @UserName";
            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName = "@UserName", SqlDbType = SqlDbType.NVarChar, Value = username}
            };
            DataTable dt = DataBase.GetDT(sp, query, "ApplicationServices");
            try
            {
                HttpContext.Current.Session["CompanyID"] = Convert.ToInt32(dt.Rows[0][0].ToString());
                HttpContext.Current.Session["BranchID"] = Convert.ToInt32(dt.Rows[0][1].ToString());

            }
            catch
            {
                HttpContext.Current.Session["CompanyID"] = 0;
                HttpContext.Current.Session["BranchID"] = 0;
            }
        }
    }
}