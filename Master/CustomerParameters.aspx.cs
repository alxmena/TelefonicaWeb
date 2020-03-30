using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ATCPortal.Master
{
    public partial class CustomerLogo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            if (ASPxBinaryImage1.ContentBytes != null)
            {
                string myDir = Server.MapPath("~/Images/UploadedLogos/");
                int CustomerID = int.Parse(cmbCustomer.Value.ToString());
                string fileName = myDir + "CompanyID-" + CustomerID.ToString("D10") + "-" + ASPxBinaryImage1.GetUploadedFileName();
                try
                {
                    //delete previous logo for the company
                    foreach (string f in Directory.EnumerateFiles(myDir, "CompanyID-" + CustomerID.ToString("D10") + "-*.*"))
                    {
                        File.Delete(f);
                    }
                    File.WriteAllBytes(fileName, ASPxBinaryImage1.ContentBytes);
                    lblMessage.Text = "Image Upload was successful";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Image Upload Failed. Error Message: " + ex.Message;
                }
                pupMessage.ShowOnPageLoad = true;

            }
            
        }

        protected void ASPxBinaryImage1_DataBound(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
        }

        protected void btnNext1_Click(object sender, EventArgs e)
        {
            if (cmbCustomer.Text != "")
            {
                Panel1.Visible = false;
                Panel2.Visible = true;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Panel1.Visible = true;
            Panel2.Visible = false;
        }
    }
}