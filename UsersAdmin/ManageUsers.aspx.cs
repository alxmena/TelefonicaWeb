using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.Security;

namespace ATCPortal.UsersAdmin
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void chkIsApproved_Load(object sender, EventArgs e)
        {
            ASPxCheckBox cb = sender as ASPxCheckBox;
            if (grdUsers.IsNewRowEditing)
                cb.Checked = true;
            else
            {
                MembershipUser newuser = Membership.GetUser(grdUsers.GetRowValues(grdUsers.EditingRowVisibleIndex, "UserName").ToString());
                cb.Checked = newuser.IsApproved;
                if (newuser.UserName == "Master")
                    cb.Enabled = false;
            }
        }

        protected void grdUsers_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            ASPxTextBox tb1 = grdUsers.FindEditRowCellTemplateControl((GridViewDataColumn)grdUsers.Columns["Password"], "txtPWD1") as ASPxTextBox;
            ASPxTextBox tb2 = grdUsers.FindEditRowCellTemplateControl((GridViewDataColumn)grdUsers.Columns["Password"], "txtPWD2") as ASPxTextBox;

            if (grdUsers.IsNewRowEditing)
            {
                // Checks for null values.
                if (e.NewValues["UserName"] == null)
                    e.Errors[grdUsers.Columns["UserName"]] = "User Name cannot be null";

                if (e.NewValues["Email"] == null)
                    e.Errors[grdUsers.Columns["Email"]] = "Email cannot be null";

                if (e.Errors.Count > 0 || chkpwd(tb1.Text, tb2.Text) != null)
                    e.RowError = "Please, correct all errors";
                else
                {
                    string username = e.NewValues["UserName"].ToString();
                    string email = e.NewValues["Email"].ToString();
                    GridViewDataColumn dc = grdUsers.Columns["IsApproved"] as GridViewDataColumn;
                    ASPxCheckBox cb = grdUsers.FindEditRowCellTemplateControl(dc, "chkIsApproved") as ASPxCheckBox;
                    bool isapproved = cb.Checked;

                    if (username.Length < 3)
                        e.Errors[grdUsers.Columns["UserName"]] = "User Name must be at least 3 characters long";

                    if (!(email.Contains("@") && email.Contains(".")))
                        e.Errors[grdUsers.Columns["Email"]] = "Email is not valid";
                    if (e.Errors.Count > 0)
                        e.RowError = "Please, correct all errors";
                    else
                    {
                        //Add User
                        MembershipCreateStatus status;
                        try
                        {
                            MembershipUser newUser = Membership.CreateUser(username, tb1.Text, email, null, null, isapproved, out status);
                            if (newUser == null)
                                e.RowError = GetErrorMessage(status);
                        }
                        catch
                        {
                            e.RowError = "An exception occurred creating the user";
                        }
                    }
                }
            }
            else
            {
                // Checks for null values.
                if (e.NewValues["Email"] == null)
                    e.Errors[grdUsers.Columns["Email"]] = "Email cannot be null";

                if (e.Errors.Count > 0) e.RowError = "Please, fill all fields";
                else
                {
                    string email = e.NewValues["Email"].ToString();

                    if (!(email.Contains("@") && email.Contains(".")))
                        e.Errors[grdUsers.Columns["Email"]] = "Email is not valid";

                    if (e.Errors.Count > 0 || chkpwd(tb1.Text, tb2.Text) != null)
                        e.RowError = "Please, correct all errors";
                    else if (tb1.Text != "") //attempt to change password
                    {
                        string strName = grdUsers.GetRowValues(grdUsers.EditingRowVisibleIndex, "UserName").ToString();
                        MembershipUser newuser = Membership.GetUser(strName);
                        try
                        {
                            string oldpwd = newuser.ResetPassword();

                            if (!newuser.ChangePassword(oldpwd, tb1.Text))
                                e.RowError = "Password change failed. Please re-enter your values and try again.";
                        }
                        catch (Exception e2)
                        {
                            e.RowError = "An exception occurred: " + Server.HtmlEncode(e2.Message) + ". Please re-enter your values and try again.";
                        }
                    }
                }
            }
        }

        public string GetErrorMessage(MembershipCreateStatus status)
        {
            switch (status)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        protected void grdUsers_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
            grdUsers.CancelEdit();
        }

        protected void grdUsers_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string username = e.Values["UserName"].ToString();
            if (username != "Master")
                Membership.DeleteUser(username);
            e.Cancel = true;
        }

        protected void grdUsers_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string strName = grdUsers.GetRowValues(grdUsers.EditingRowVisibleIndex, "UserName").ToString();
            MembershipUser newuser = Membership.GetUser(strName);

            string newemail = e.NewValues["Email"].ToString();
            newuser.Email = newemail;

            GridViewDataColumn dc = grdUsers.Columns["IsApproved"] as GridViewDataColumn;
            ASPxCheckBox cb = grdUsers.FindEditRowCellTemplateControl(dc, "chkIsApproved") as ASPxCheckBox;
            newuser.IsApproved = cb.Checked;

            GridViewDataColumn dc2 = grdUsers.Columns["IsLockeOut"] as GridViewDataColumn;
            ASPxCheckBox cb2 = grdUsers.FindEditRowCellTemplateControl(dc2, "chkIsLockedout") as ASPxCheckBox;
            if (newuser.IsLockedOut && !cb2.Checked)
                newuser.UnlockUser();

            Membership.UpdateUser(newuser);
            e.Cancel = true;
            grdUsers.CancelEdit();
        }

        protected void chkIsLockedout_Load(object sender, EventArgs e)
        {
            ASPxCheckBox cb = sender as ASPxCheckBox;
            if (grdUsers.IsNewRowEditing)
                cb.Enabled = false;
            else
            {
                MembershipUser newuser = Membership.GetUser(grdUsers.GetRowValues(grdUsers.EditingRowVisibleIndex, "UserName").ToString());
                if (newuser.IsLockedOut)
                    cb.Checked = true;
                else
                    cb.Enabled = false;
            }
        }

        protected void txtPWD1_Load(object sender, EventArgs e)
        {
            if (!grdUsers.IsNewRowEditing)
            {
                ASPxTextBox tb1 = sender as ASPxTextBox;
                tb1.NullText = "To change password, enter NEW PASSWORD";
            }
        }

        protected void txtPWD1_Validation(object sender, ValidationEventArgs e)
        {
            e.IsValid = false;
            ASPxTextBox tb1 = grdUsers.FindEditRowCellTemplateControl((GridViewDataColumn)grdUsers.Columns["Password"], "txtPWD1") as ASPxTextBox;
            ASPxTextBox tb2 = grdUsers.FindEditRowCellTemplateControl((GridViewDataColumn)grdUsers.Columns["Password"], "txtPWD2") as ASPxTextBox;
            e.ErrorText = chkpwd(tb1.Text, tb2.Text);
            if (e.ErrorText == null)
                e.IsValid = true;
        }

        protected string chkpwd(string p1, string p2)
        {
            string error = null;
            if (grdUsers.IsNewRowEditing)
            {
                if (p1 == "")
                    error = "Password cannot be empty";
                else if (p1 != p2)
                    error = "Password and Password Confirmation do not match";
                else if (p1.Length < 6)
                    error = "Password must be at least 6 characters long";
            }
            else
            {
                if (p1.Length > 0)
                {
                    if (p1 != p2)
                        error = "Password and Password Confirmation do not match";
                    else if (p1.Length < 6)
                        error = "Password must be at least 6 characters long";
                }

            }
            return error;
        }

        protected void grdUsers_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "UserName" && !grdUsers.IsNewRowEditing)
                e.Editor.Enabled = false;
        }
    }
}