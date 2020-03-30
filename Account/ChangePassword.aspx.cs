using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace ATCPortal
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Q1"]))
                {
                    tbCurrentPassword.Visible = false;
                    lblCurrentPassword.Visible = false;
                }
                if (User.Identity.IsAuthenticated || !String.IsNullOrEmpty(Request.QueryString["Q2"]))
                {
                    txtUser.Visible = false;
                    lblUser.Visible = false;
                }
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string CurrentPwd;
            string CurrentUsr;
            if (!tbCurrentPassword.Visible)
                CurrentPwd = Request.QueryString["Q1"].ToString();
            else
                CurrentPwd = tbCurrentPassword.Text;

            if (!txtUser.Visible)
            {
                if (!User.Identity.IsAuthenticated)
                    CurrentUsr = Request.QueryString["Q2"].ToString();
                else
                {
                    CurrentUsr = User.Identity.Name;
                }
            }
            else
            {
                CurrentUsr = txtUser.Text;
            }


            if (!Membership.ValidateUser(CurrentUsr, CurrentPwd))
            {
                if (tbCurrentPassword.Visible)
                {
                    tbCurrentPassword.ErrorText = "User Name and Old Password combination is not valid";
                    tbCurrentPassword.IsValid = false;
                }
                else
                {
                    lblMsg.Text = "Link to reset password has been used. You must resubmit a new password reset. Try logging in again and once it fails, click on Reset Password.";
                    popMsg.ShowOnPageLoad = true;
                }
            }
            else
            {
                MembershipUser user = Membership.GetUser(CurrentUsr);
                DateTime lastchange = user.LastPasswordChangedDate.AddHours(2);
                if (System.DateTime.Now < lastchange || User.Identity.IsAuthenticated)
                {
                    bool ok = false;
                    try
                    {
                        ok = user.ChangePassword(CurrentPwd, tbPassword.Text);
                        if (!ok)
                        {
                            tbPassword.ErrorText = "Password is not valid";
                            tbPassword.IsValid = false;
                        }
                    }
                    catch (Exception e2)
                    {
                        tbPassword.ErrorText = e2.Message;
                        tbPassword.IsValid = false;
                    }

                    if (ok)
                        Response.Redirect("~/");
                }
                else
                {
                    lblMsg.Text = "Link to reset password has expired. You must resubmit a new password reset. Try logging in again and once it fails, click on Reset Password.";
                    popMsg.ShowOnPageLoad = true;
                }
            }
        }
    }
}