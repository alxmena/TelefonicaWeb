using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace ATCPortal.UsersAdmin
{
    public partial class ManageRoles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void grdRoles_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string SelectedRole = grdRoles.GetRowValuesByKeyValue(e.Keys[0], "RoleName").ToString();
            if (SelectedRole != "Master")
                Roles.DeleteRole(SelectedRole);
            e.Cancel = true;
        }

        protected void grdRoles_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            Roles.CreateRole(e.NewValues["RoleName"].ToString());
            if (e.NewValues["Description"] != null)
            {
                //Add Description
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                SqlCommand cmd = new SqlCommand("aspnet_Roles_UpdateDescription", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Description", e.NewValues["Description"].ToString());
                cmd.Parameters.AddWithValue("@RoleName", e.NewValues["RoleName"].ToString());
                con.Open();
                int rowAffected = cmd.ExecuteNonQuery();
                con.Close();
            }
            e.Cancel = true;
            grdRoles.CancelEdit();
        }

        protected void grdRoles_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (!grdRoles.IsNewRowEditing && e.Column.FieldName == "RoleName" && e.Value.ToString() == "Master")
                e.Editor.Enabled = false;
        }
    }
}