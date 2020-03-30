using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ATCPortal.Questios
{
    public partial class Questions : System.Web.UI.Page
    {
        DataTable tblQuestions;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
                renderQuestions();
            //}
        }

        private DataTable getQuestions()
        {
            DataTable dt = new DataTable();
            if (Session["tblQuestions"] == null)
            {
                string query = "SELECT * FROM tblQuestions";
                dt = DataBase.GetDT(query);
                Session["tblQuestions"] = dt;
            }
            else
            {
                dt = Session["tblQuestions"] as DataTable;
            }

            return dt;
        }

        private Int64 validateTicket()
        {
            Int64 ticketID = -1;
            string ticket = Request.QueryString.Get("ticket");
            if (!string.IsNullOrEmpty(ticket))
            {
                string query = "SELECT ID FROM tblTicket WHERE QuestionUI=@QuestionUI";
                List<SqlParameter> sp = new List<SqlParameter>()
                {
                    new SqlParameter() { ParameterName = "@QuestionUI", SqlDbType = SqlDbType.NVarChar, Value = ticket}
                };

                DataTable dt = DataBase.GetDT(sp, query);
                if (dt.Rows.Count > 0)
                {
                    ticketID = Convert.ToInt64(dt.Rows[0][0]);
                }
            }

            return ticketID;
        }

        private void renderQuestions()
        {
            tblQuestions = getQuestions();
            if (validateTicket() > -1)
            {
                foreach (DataRow dr in tblQuestions.Rows)
                {
                    int id = Convert.ToInt32(dr["ID"]);
                    string detail = dr["Detail"].ToString();
                    string typeInput = dr["TypeInput"].ToString();
                    int options = Convert.ToInt32(dr["Options"]);

                    ASPxPanel pnlQuestion = new ASPxPanel();
                    pnlQuestion.ID = "pnl" + id;
                    pnlQuestion.Style["width"] = "100%";
                    pnlQuestion.Style["margin-bottom"] = "15px";

                    ASPxLabel lblDetail = new ASPxLabel();
                    lblDetail.ID = "Q" + id;
                    lblDetail.Text = detail;
                    lblDetail.Style["font-weight"] = "bold";
                    lblDetail.Style["margin-bottom"] = "15px";
                    lblDetail.Style["font-size"] = "15px;";
                    pnlQuestion.Controls.Add(lblDetail);

                    if (typeInput == "TEXT")
                    {
                        ASPxMemo memo = new ASPxMemo();
                        memo.ID = "memo" + id;
                        memo.Style["width"] = "100%";
                        memo.Height = 70;
                        pnlQuestion.Controls.Add(memo);
                    }

                    if (typeInput == "RADIOBUTTON")
                    {
                        ASPxRadioButtonList radio = new ASPxRadioButtonList();
                        radio.ID = "radio" + id;
                        radio.Style["width"] = "100%";
                        radio.Style["border"] = "none";
                        for (int i = 1; i <= options; i++)
                        {
                            radio.Items.Add(new ListEditItem(i.ToString(), i));
                        }
                        radio.RepeatDirection = RepeatDirection.Horizontal;
                        radio.SelectedIndex = 0;
                        pnlQuestion.Controls.Add(radio);
                    }

                    phContent.Controls.Add(pnlQuestion);
                }

            }
            else
            {
                lblMsg.Text = "Survey does not exist or has already been registered";
                popMsg.ShowOnPageLoad = true;
            }
        }

        protected void btnSaveQuestions_Click(object sender, EventArgs e)
        {
            Int64 ticketID = validateTicket();
            if (ticketID > -1)
            {
                tblQuestions = getQuestions();
                List<SqlParameter> sp;
                string query = "INSERT INTO tblAnswers values(@Aswer, @OptionValue, @UserAnswer, @DateAnswer, @QuestionsID, @TicketID)";
                foreach (DataRow dr in tblQuestions.Rows)
                {
                    int id = Convert.ToInt32(dr["ID"]);
                    string typeInput = dr["TypeInput"].ToString();
                    string value = string.Empty;
                    int optionValue = 0;


                    ASPxPanel pnlQuestion = (ASPxPanel)phContent.FindControl("pnl" + id);

                    if (typeInput == "TEXT")
                    {
                        ASPxMemo memo = (ASPxMemo)pnlQuestion.FindControl("memo" + id);
                        if (memo != null)
                        {
                            value = memo.Text;
                        }
                    }

                    if (typeInput == "RADIOBUTTON")
                    {
                        ASPxRadioButtonList radio = (ASPxRadioButtonList)pnlQuestion.FindControl("radio" + id);
                        if (radio != null)
                        {
                            optionValue = Convert.ToInt32(radio.SelectedItem.Value);
                        }
                    }
                    sp = new List<SqlParameter>()
                    {
                        new SqlParameter() { ParameterName = "@Aswer", SqlDbType = SqlDbType.NVarChar, Value = value },
                        new SqlParameter() { ParameterName = "@OptionValue", SqlDbType = SqlDbType.NVarChar, Value = optionValue },
                        new SqlParameter() { ParameterName = "@UserAnswer", SqlDbType = SqlDbType.NVarChar, Value = User.Identity.Name },
                        new SqlParameter() { ParameterName = "@DateAnswer", SqlDbType = SqlDbType.NVarChar, Value = DateTime.Now },
                        new SqlParameter() { ParameterName = "@QuestionsID", SqlDbType = SqlDbType.NVarChar, Value = id },
                        new SqlParameter() { ParameterName = "@TicketID", SqlDbType = SqlDbType.BigInt, Value = ticketID }
                    };

                    DataBase.UpdateDB(sp, query);
                }

                query = "UPDATE tblTicket set QuestionUI = null WHERE ID = @ID";
                sp = new List<SqlParameter>()
                {
                    new SqlParameter() { ParameterName = "@ID", SqlDbType = SqlDbType.BigInt, Value = ticketID }
                };
                DataBase.UpdateDB(sp, query);

                lblMsg.Text = "Answers Saved Successfully";
            }
            else
            {
                lblMsg.Text = "Survey does not exist or has already been registered";
            }

            popMsg.ShowOnPageLoad = true;
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/");
        }
    }
}