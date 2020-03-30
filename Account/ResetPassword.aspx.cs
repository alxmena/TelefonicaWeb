using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Net.Mail;
using System.Data;

namespace ATCPortal
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            MembershipUser user = Membership.GetUser(txtUserName.Text);
            //validate email
            if (user != null)
            {
                if (user.Email == txtEmail.Text)
                {
                    //String userName = "sirius@amerinode.com";
                    //String password = "Issachar61*";
                    String userName = "q@amerinode.com";
                    String password = "Ilikechocolate5!";
                    MailMessage msg = new MailMessage();
                    msg.To.Add(new MailAddress(txtEmail.Text));
                    msg.From = new MailAddress(userName);
                    string q = "SELECT HTML FROM tblTemplates WHERE Id = 1";
                    DataRow dr = DataBase.GetDT(q).Rows[0];
                    msg.Subject = "Password Reset Request from Amerinode.net Portal";
                    try
                    {
                        string urlpassword = Server.UrlEncode(user.ResetPassword());
                        string urluser = Server.UrlEncode(user.UserName);
                        msg.Body = dr[0].ToString().Replace("&lt;&lt;FirstName&gt;&gt;", user.UserName).Replace("&lt;&lt;PWD&gt;&gt;", urlpassword).Replace("&lt;&lt;USR&gt;&gt;", urluser);
                        msg.IsBodyHtml = true;
                        SmtpClient client = new SmtpClient();
                        client.Host = "smtp.office365.com";
                        client.Credentials = new System.Net.NetworkCredential(userName, password);
                        client.Port = 587;
                        client.EnableSsl = true;
                        client.Send(msg);
                        Response.Redirect(Request.QueryString["ReturnUrl"] ?? "~/Account/ResetSuccess.aspx");
                    }
                    catch (Exception e2)
                    {
                        lblMsg.Text = e2.Message + " Contact Network Administrator via email: network_admin@amerinode.com.";
                        popMsg.ShowOnPageLoad = true;
                    }
                }
                else
                {
                    txtEmail.ErrorText = "User Name & Email combination is not valid";
                    txtEmail.IsValid = false;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.QueryString["ReturnUrl"] ?? "~/Default.aspx");
        }
    }
}