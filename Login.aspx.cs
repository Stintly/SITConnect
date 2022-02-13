using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Net.Mail;

namespace SITConnect
{
    public partial class Login : System.Web.UI.Page
    {

        byte[] Key;
        byte[] IV;

        public string action = null;
        public string success { get; set; }
        public List<string> ErrorMessage { get; set; }

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SITConnection"].ConnectionString;

        static string RandomNum;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Visible = false;
            }

        }

        protected void LoginMe(object sender, EventArgs e)
        {
            string pwd = tb_pwd.Text.ToString().Trim();
            string userid = tb_userid.Text.ToString().Trim();

            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(userid);
            string dbSalt = getDBSalt(userid);

            try
            {
                if (validateCaptcha())
                {

                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    //if (tb_userid.Text.Trim().Equals("u") && tb_pwd.Text.Trim().Equals("p"))
                    {

                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);

                        if (userHash.Equals(dbHash))
                        {
                            Session["LoggedIn"] = userid;
                            Response.Redirect("VerifyOTP.aspx", false);

                            //Create a new GUID save into the session
                            string guid = Guid.NewGuid().ToString();
                            Session["AuthToken"] = guid;

                            //now create a new cookie with this guid value
                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                            // Logging
                            action = "Successfully logged in";
                            //createLog();

                            //OTP Generation
                            Random random = new Random();
                            RandomNum = random.Next(000000, 999999).ToString();
                            createOTP(userid,RandomNum);
                            SendVCode(RandomNum);


                            
                        }
                        else
                        {
                            lblMessage.Text = "Userid or password is not valid. Please try again.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Visible = true;

                        }
                    }
                }
                //else
                //{
                    //lblMessage.Text = "Wrong username or password";
                //}
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            
        }

        public bool validateCaptcha()
        {
            bool result = true;

            string captchaResponse = Request.Form["g-recaptcha-response"];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(" https://www.google.com/recaptcha/api/siteverify?secret=6Lcxk2UeAAAAADrYthgygQl4bbRRkvHNyUAkMvj_ &response=" + captchaResponse);

            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        //gScore.Text = jsonResponse;
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        Login jsonObject = js.Deserialize<Login>(jsonResponse);

                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        protected string getDBSalt(string userid)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }

        protected string getDBHash(string userid)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0,
               plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }

        //protected void createLog()
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(MYDBConnectionString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("INSERT INTO Logs VALUES(@LogAction, @LogDT, @Loggeduser)"))
        //            {
        //                using (SqlDataAdapter sda = new SqlDataAdapter())
        //                {
        //                    cmd.CommandType = CommandType.Text;
        //                    cmd.Parameters.AddWithValue("@LogAction", action.ToString());
        //                    cmd.Parameters.AddWithValue("@LogDT", DateTime.Now);
        //                    cmd.Parameters.AddWithValue("@Loggeduser", tb_userid.Text.Trim());
        //                    cmd.Connection = con;
        //                    con.Open();
        //                    cmd.ExecuteNonQuery();
        //                    con.Close();
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.ToString());
        //    }
        //}

        protected string createOTP(string userid ,string randomnum)
        {
            
            SqlConnection con = new SqlConnection(MYDBConnectionString);
            string otp = null;

            string query = "update Account set OTP = @OTP where Email=@USERID";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@OTP", randomnum);
            cmd.Parameters.AddWithValue("@USERID", userid);

            try
            {
                con.Open();
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if(reader["OTP"] != null)
                        {
                            otp = reader["OTP"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { con.Close(); }
            return otp;
        }

        protected string SendVCode(string vcode)
        {
            string from = "SITConnect <sitconnect1219@gmail.com>";
            string str = null;
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("sitconnect1219@gmail.com", "SITConnect@123"),
                EnableSsl = true
            };
            var Message = new MailMessage
            {
                Subject = "SITConnect Login OTP",
                Body = "Dear User, your OTP code is " + vcode + "\nThank you for using SIT Connect"
            };
            Message.To.Add(tb_userid.Text.ToString());
            Message.From = new MailAddress("SITConnect <sitconnect1219@gmail.com>");
            try
            {
                smtpClient.Send(Message);
                return str;
            }
            catch
            {
                throw;
            }
        }
    }
}