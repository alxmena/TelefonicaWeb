using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;

namespace ATCPortal
{
    public partial class RootMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ctrlname = Page.Request.Params["__EVENTTARGET"];
            if (!string.IsNullOrEmpty(ctrlname) && ctrlname.Contains("HeadLoginStatus"))
            {
                FormsAuthentication.SignOut();
                Response.Redirect("~/");
                Session["CompanyID"] = null;
                Session["BranchID"] = null;
            }

            if (!Page.IsCallback && !Page.IsPostBack)
            {
                if (Session["CompanyID"] != null && Session["CompanyID"].ToString() != "0")
                {
                    //update customer logo after login
                    //look for the file
                    string sourceDirectory = Server.MapPath("~/Images/UploadedLogos/");
                    var files = from fullFilename
                                in Directory.EnumerateFiles(sourceDirectory)
                                select Path.GetFileName(fullFilename);

                    string filefound = "";
                    foreach (string file in files)
                    {
                        int CompanyID = int.Parse(Session["CompanyID"].ToString());
                        if (file.StartsWith("CompanyID-" + CompanyID.ToString("D10")))
                        {
                            filefound = file;
                            break;
                        }
                    }
                    if (filefound == "")
                        imgCustomer.ImageUrl = "~/Images/Q-Logo2.gif";
                    else
                        imgCustomer.ImageUrl = "~/Images/UploadedLogos/" + filefound;
                }
            }
        }
    }
}