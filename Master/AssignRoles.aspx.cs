using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.Security;
using System.Data.SqlClient;
using System.Data;

namespace ATCPortal.Master
{
    public partial class AssignRoles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void chlRoles_Load(object sender, EventArgs e)
        {
            ASPxCheckBoxList cbl = sender as ASPxCheckBoxList;
            if (grdRoles.IsEditing)
            {
                //get all Roles
                cbl.DataSource = Roles.GetAllRoles();
                cbl.DataBind();
                //get User
                string user = grdRoles.GetRowValues(grdRoles.EditingRowVisibleIndex, "UserName").ToString();
                //mark the Roles the User belongs to
                foreach (ListEditItem le in cbl.Items)
                {
                    le.Selected = Roles.IsUserInRole(user, le.Text);
                }
            }
        }

        protected void grdRoles_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            GridViewDataColumn dc = grdRoles.Columns["RoleName"] as GridViewDataColumn;
            string user = grdRoles.GetRowValues(grdRoles.EditingRowVisibleIndex, "UserName").ToString();
            ASPxCheckBoxList cbl = grdRoles.FindEditRowCellTemplateControl(dc, "chlRoles") as ASPxCheckBoxList;
            foreach (ListEditItem le in cbl.Items)
            {
                if (le.Selected)
                {
                    if (!Roles.IsUserInRole(user, le.Text))
                        Roles.AddUserToRole(user, le.Text);
                }
                else
                {
                    if (Roles.IsUserInRole(user, le.Text))
                        if (!((user == "Master") && (le.Text == "Master")))
                            Roles.RemoveUserFromRole(user, le.Text);
                }
            }
            e.Cancel = true;
            grdRoles.CancelEdit();
        }

        protected void grdRoles_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName != "RoleNames") return;
            //get all roles for the user
            string[] ListOfRoles = Roles.GetRolesForUser(e.GetFieldValue("UserName").ToString());
            string RoleNames = "";
            foreach (string role in ListOfRoles)
            {
                RoleNames += role + ", ";
            }
            if (RoleNames != "")
                RoleNames = RoleNames.Substring(0, RoleNames.Length - 2);
            e.DisplayText = RoleNames;
            e.Column.Settings.FilterMode = ColumnFilterMode.DisplayText;
        }
    }
}