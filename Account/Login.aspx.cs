using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using DevExpress.Web;

namespace ATCPortal
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.IsAuthenticated && !string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                    // This is an unauthorized, authenticated request...
                    Response.Redirect("~/UnauthorizedAccess.aspx");
                if (Request.IsAuthenticated)
                {
                    Panel1.Visible = false;
                    Response.Redirect("~/Default.aspx");
                }

            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Membership.ValidateUser(tbUserName.Text, tbPassword.Text))
            {
                // Add CompanyID and BranchID first
                ValidateRequest.SetDataSession(tbUserName.Text);
                if (string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                {
                    FormsAuthentication.SetAuthCookie(tbUserName.Text, false);
                    Response.Redirect("~/");
                }
                else
                    FormsAuthentication.RedirectFromLoginPage(tbUserName.Text, false);
            }
            else
            {
                tbUserName.ErrorText = "Invalid combination of user and password";
                tbUserName.IsValid = false;
                hypReset.Visible = true;
            }
        }
    }
}