using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ATCPortal.About
{
    public partial class Version : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int len = Assembly.GetExecutingAssembly().GetName().Version.ToString().Length;
            lblVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, len - 2);
        }
    }
}