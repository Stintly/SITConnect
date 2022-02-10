using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class VerifyOTP : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SITConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
            }

        }

        protected string getCodeOTP(string email)
        {
            string otp = null;
            SqlConnection con = new SqlConnection(MYDBConnectionString);
            string sql = "select OTP from Account where Email = @USERID";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@USERID", email);
            try
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        otp = reader["OTP"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return otp;
        }

        protected void VerifyCode(object sender, EventArgs e)
        {
            if (tb_OTPCode.Text.ToString() == getCodeOTP(Session["LoggedIn"].ToString()))
            {
                createLog();

                Response.Redirect("Homepage.aspx", false);

            }
            else
            {
                lblMessage.Text = "OTP code is incorrect";
            }
        }

        protected void createLog()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Logs VALUES(@LogAction, @LogDT, @Loggeduser)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@LogAction", "Successfully logged in");
                            cmd.Parameters.AddWithValue("@LogDT", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Loggeduser", Session["LoggedIn"].ToString());
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}