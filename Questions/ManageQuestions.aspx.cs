using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ATCPortal.Questions
{
    public partial class ManageQuestions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gvQuestions_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["Options"] = "1";
        }

        protected void gvQuestions_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            
            int SelectedQuestion = Convert.ToInt32(gvQuestions.GetRowValuesByKeyValue(e.Keys[0], "ID"));
            string query = "DELETE FROM tblQuestions WHERE ID=@ID";
            if (validateDelete(SelectedQuestion))
            {
                List<SqlParameter> sp = new List<SqlParameter>()
                {
                    new SqlParameter() { ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value = SelectedQuestion }
                };
                DataBase.UpdateDB(sp, query);
            }
            e.Cancel = true;
        }

        private bool validateDelete(int id)
        {
            bool flag = true;
            string query = "SELECT count(1) FROM tblAnswers WHERE QuestionsID = @QuestionID";
            List<SqlParameter> sp = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@QuestionID", SqlDbType = SqlDbType.Int, Value = id }
            };
            DataTable dt = DataBase.GetDT(sp, query);
            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                flag = false;
            }

            return flag;
        }
    }
}