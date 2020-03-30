using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace ATCPortal
{
    public partial class MainMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string path = "/menu/*[@Role='None']";
            MembershipUser user = Membership.GetUser();
            if (user != null)
            {
                string[] userroles = Roles.GetRolesForUser(user.UserName);
                path = "/menu/*[";
                foreach (string role in userroles)
                {
                    path += "@Role='" + role + "' or ";
                }
                if (path.Length == 8)
                    path = "/menu/*[@Role='None']";
                else
                    path = path.Substring(0, path.Length - 4) + "]";
            }
            XmlDataSourceLeft.XPath = path;
        }
    }
}