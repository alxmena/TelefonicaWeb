using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ATCPortal.Amerinode
{
    public partial class QTicketImages : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string qticket = Request.QueryString.Get("qticket");
            if (!string.IsNullOrEmpty(qticket))
            {
                string path = MapPath("~/Content/Q/") + qticket + "\\";
                if (Directory.Exists(path))
                {
                    fmTicket.Settings.RootFolder = path;
                }
                else
                {
                    fmTicket.Visible = false;
                    lblResult.Text = "Ticket has no files";
                }
                fmTicket.Refresh();
            }
        }
    }
}