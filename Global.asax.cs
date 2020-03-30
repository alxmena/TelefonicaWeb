using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using DevExpress.Web;

namespace ATCPortal
{
    public class Global_asax : System.Web.HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            DevExpress.Web.ASPxWebControl.CallbackError += new EventHandler(Application_Error);
            DevExpress.XtraReports.Web.ASPxWebDocumentViewer.StaticInitialize();
        }

        void Application_End(object sender, EventArgs e)
        {
            // Code that runs on application shutdown
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();
            string ErrorMessage = ex.Message;
            string StackTrace = ex.StackTrace;
            string ExceptionType = ex.GetType().FullName;
            string UserName = User.Identity.Name;
            string Message = "Error Message: " + ex.Message + "<br/>";
            Message += "Exception Type: " + ex.GetType().FullName + "<br/>";
            Message += "Stack Trace: " + ex.StackTrace + "<br/>";
            Message += "User Name: " + User.Identity.Name + "<br/>";

            if (HttpContext.Current != null)
            {
                string url = HttpContext.Current.Request.Url.ToString();
                System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
                Message += "<br/>";
                Message += "Url: " + url + "<br/>";
                Message += "<br/>";
                Message += "Page: " + page.ToString() + "<br/>";
            }

            Email.SendEmail("Telefonica Portal Error", Message, "it_support@amerinode.com");

            if (ErrorMessage== "Object reference not set to an instance of an object.")
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
                Response.Redirect("~/Deafult.aspx");
            }


        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
        }
    }
}