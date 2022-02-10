using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class ChangePassword : System.Web.UI.Page
    {

        string MYDBConnectionString =
        System.Configuration.ConfigurationManager.ConnectionStrings["SITConnection"].ConnectionString;
        byte[] Key;
        byte[] IV;
        static string finalHash;
        static string salt;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void passwordchange(object sender, EventArgs e)
        {
            

            using (SqlConnection con = new SqlConnection(MYDBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE [Account] SET [PasswordHash] = @PasswordHash, [PasswordSalt] = @PasswordSalt WHERE Email = @USERID"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        string pwd = tb_newpassword.Text.ToString().Trim(); ;

                        //Generate random "salt"
                        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                        byte[] saltByte = new byte[8];

                        //Fills array of bytes with a cryptographically strong sequence of random values.
                        rng.GetBytes(saltByte);
                        salt = Convert.ToBase64String(saltByte);
                        SHA512Managed hashing = new SHA512Managed();
                        string pwdWithSalt = pwd + salt;
                        byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        finalHash = Convert.ToBase64String(hashWithSalt);
                        RijndaelManaged cipher = new RijndaelManaged();
                        cipher.GenerateKey();
                        Key = cipher.Key;
                        IV = cipher.IV;

                 
                        cmd.CommandType = CommandType.Text;
                        
                       
                        cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                        cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                        cmd.Parameters.AddWithValue("@USERID", Session["LoggedIn"].ToString());
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        Response.Redirect("HomePage.aspx");
                    }
                }
            }
           
        }

   
    }
}
        