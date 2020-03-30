using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DevExpress.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ATCPortal.Master
{
    public partial class Templates : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && !IsCallback)
            {
                //string path = MapPath("~/Templates/");
                //string[] files = Directory.GetFiles(path,"*.docx");
                //foreach(string file in files)
                //{
                //    ListEditItem li = rblTemplates.Items.Add();
                //    li.Text = Path.GetFileNameWithoutExtension(file);
                //    li.Value = file;
                //}
                DataTable dt = DataBase.GetDT("SELECT Id, FileDescription FROM tblTemplates");
                foreach (DataRow dr in dt.Rows)
                {
                    ListEditItem li = rblTemplates.Items.Add();
                    li.Value = dr["Id"].ToString();
                    li.Text = dr["FileDescription"].ToString();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //htmEditor.Export(DevExpress.Web.ASPxHtmlEditor.HtmlEditorExportFormat.Docx);
            string query = "UPDATE tblTemplates SET FileName = @Param1, HTML = @Param2 WHERE Id = @Id";
            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName = "@Param1", SqlDbType = SqlDbType.NVarChar, Size=128, Value= txtFileName.Text},
                new SqlParameter() {ParameterName = "@Param2", SqlDbType = SqlDbType.Text, Size = -1, Value = htmEditor.Html},
                new SqlParameter() {ParameterName = "@Id", SqlDbType = SqlDbType.Int, Value = rblTemplates.SelectedItem.Value}
            };
            int aux = DataBase.UpdateDB(sp, query);
            if (aux == 1)
            {
                lblMsg.Text = "Template Updated Successfully";
                popMsg.ShowOnPageLoad = true;
            }

        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            if (rblTemplates.SelectedIndex >= 0)
            {
                string query = string.Format("SELECT FileName, HTML FROM tblTemplates WHERE Id = {0}", rblTemplates.SelectedItem.Value.ToString());
                DataTable dt = DataBase.GetDT(query);
                if (dt != null)
                {
                    htmEditor.Html = dt.Rows[0]["HTML"].ToString();
                    txtFileName.Text = dt.Rows[0]["FileName"].ToString();
                    btnSave.Enabled = true;
                }

            }
        }


        protected void txtFileName_TextChanged(object sender, EventArgs e)
        {
            if (txtFileName.Text == "")
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;
        }
    }
}