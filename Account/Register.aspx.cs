using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace ATCPortal
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            try
            {
                MembershipUser user = Membership.CreateUser(tbUserName.Text, tbPassword.Text, tbEmail.Text);
                //Add Full Name to DB
                string sql = "UPDATE aspnet_Users SET Name = @Name, Title = @Title, BranchID = @BranchID WHERE UserName = @UserName";
                List<SqlParameter> sp = new List<SqlParameter>()
                                {
                                    new SqlParameter() {ParameterName = "@Name", SqlDbType = SqlDbType.NVarChar, Value= tbName.Text},
                                    new SqlParameter() {ParameterName = "@Title", SqlDbType = SqlDbType.NVarChar, Value= tbTitle.Text},
                                    new SqlParameter() {ParameterName = "@UserName", SqlDbType = SqlDbType.NVarChar, Value=tbUserName.Text},
                                    new SqlParameter() {ParameterName = "@BranchID", SqlDbType = SqlDbType.Int, Value=cbBranch.SelectedItem.Value}
                                };
                DataBase.UpdateDB(sp, sql, "ApplicationServices");
                Response.Redirect(Request.QueryString["ReturnUrl"] ?? "~/Account/RegisterSuccess.aspx");
            }
            catch (MembershipCreateUserException exc)
            {
                if (exc.StatusCode == MembershipCreateStatus.DuplicateEmail || exc.StatusCode == MembershipCreateStatus.InvalidEmail)
                {
                    tbEmail.ErrorText = exc.Message;
                    tbEmail.IsValid = false;
                }
                else if (exc.StatusCode == MembershipCreateStatus.InvalidPassword)
                {
                    tbPassword.ErrorText = exc.Message;
                    tbPassword.IsValid = false;
                }
                else
                {
                    tbUserName.ErrorText = exc.Message;
                    tbUserName.IsValid = false;
                }
            }
        }
    }
}