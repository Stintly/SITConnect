using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class HomePage : System.Web.UI.Page
    {
        string MYDBConnectionString =
        System.Configuration.ConfigurationManager.ConnectionStrings["SITConnection"].ConnectionString;
        byte[] Key;
        byte[] IV;
        byte[] nric = null;
        string userID = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                
                }
                else
                {
                    lblMessage.Text = "Congratulations you are logged in.";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    btnLogout.Visible = true;
                    userID = (string)Session["loggedIn"];
                    displayUserProfile(userID);
                    
                }
                
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }

        }

        protected void displayUserProfile(string userid)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if(reader["Email"] != DBNull.Value)
                        {
                            lbl_useremail.Text = reader["Email"].ToString();
                        }
                        if(reader["IV"] != DBNull.Value)
                        {
                            IV = Convert.FromBase64String(reader["IV"].ToString());
                        }
                        if(reader["Key"] != DBNull.Value)
                        {
                            Key = Convert.FromBase64String(reader["Key"].ToString());
                        }
                    }
                    
                }
            }//try
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }


        protected void LogoutMe (object sender, EventArgs e )
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            createLog();

            Response.Redirect("Login.aspx", false);

            if(Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if(Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
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
                            cmd.Parameters.AddWithValue("@LogAction", "Successfully logged out");
                            cmd.Parameters.AddWithValue("@LogDT", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Loggeduser", userID);
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


//using (MemoryStream msDecrypt = new MemoryStream(cipherText))
//{
  //  using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
    //{
      //  using (StreamReader srDecrypt = new StreamReader(csDecrypt))
        //{
          //  plainText = srDecrypt.ReadToEnd();
        //}
    //}
//}