using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ClsQNotifications
{
    public class DataBase
    {
        public static DataTable GetDT(string query)
        {
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString))
            using (var da = new SqlDataAdapter(query, con))
                da.Fill(table);
            return table;
        }

        public static DataTable GetDT(List<SqlParameter> parameters, string query)
        {
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.SelectCommand.Parameters.AddRange(parameters.ToArray());
            da.Fill(table);
            return table;
        }

        public static DataTable GetDT(List<SqlParameter> parameters, string query, string connection)
        {
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[connection].ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            if (parameters != null)
                da.SelectCommand.Parameters.AddRange(parameters.ToArray());
            da.Fill(table);
            return table;
        }

        public static int UpdateDB(List<SqlParameter> parameters, string query)
        {
            int aux;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString);
            SqlCommand com = new SqlCommand(query, con);
            if (parameters != null)
                com.Parameters.AddRange(parameters.ToArray());
            com.CommandType = CommandType.Text;
            con.Open();
            aux = com.ExecuteNonQuery();
            con.Close();
            return aux;
        }

        public static int UpdateDB(List<SqlParameter> parameters, string query, string connection)
        {
            int aux;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[connection].ConnectionString);
            SqlCommand com = new SqlCommand(query, con);
            com.Parameters.AddRange(parameters.ToArray());
            com.CommandType = CommandType.Text;
            con.Open();
            aux = com.ExecuteNonQuery();
            con.Close();
            return aux;
        }
    }
}
