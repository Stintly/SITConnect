using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;


namespace SITConnect
{
    public partial class Registration : System.Web.UI.Page
    {

        string MYDBConnectionString = 
        System.Configuration.ConfigurationManager.ConnectionStrings["SITConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btn_checkPassword_Click(object sender, EventArgs e) 
        {
            int scores = checkPassword(tb_password.Text);
            string status = "Very Weak";
            switch (scores)
            {
                case -1:
                    status = "Password cannot be empty";
                    break;
                case 1:
                    status = "Very Weak";
                    break;
                case 2:
                    status = "Weak";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Very Strong";
                    break;
                
                default:
                    break;
            }
            lbl_pwdchecker.Text = "Status : " + status;
            if (scores < 4)
            {
                lbl_pwdchecker.ForeColor = Color.Red;
                return;
            }
            lbl_pwdchecker.ForeColor = Color.Green;

        }

        private int checkPassword(string password)
        {
            int score = 0;

            if (password.Length == 0)
            {
                return -1;
            }
            if (password.Length < 12)
            {
                return 1;
            }
            else
            {
                score = 1;
            }
            if(Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[!@#$%&?]"))
            {
                score++;
            }
            


            return score;
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            string pwd = tb_password.Text.ToString().Trim(); ;

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

            createAccount();



        }

        protected void createAccount()
        {
            if (tb_password != null) {

                try
                {
                    using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@Fname, @Lname, @CreditNum, @CreditDate, @CreditCVV, @Email, @DoB, @Photo, @PasswordHash, @PasswordSalt, @IV, @Key, @OTP)"))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@Fname", tb_firstname.Text.Trim());
                                cmd.Parameters.AddWithValue("@Lname", tb_lastname.Text.Trim());
                                cmd.Parameters.AddWithValue("@CreditNum", Convert.ToBase64String(encryptData(tb_creditnum.Text.Trim())));
                                cmd.Parameters.AddWithValue("@CreditDate", Convert.ToBase64String(encryptData(tb_creditdate.Text.Trim())));
                                cmd.Parameters.AddWithValue("@CreditCVV", Convert.ToBase64String(encryptData(tb_creditcvv.Text.Trim())));
                                cmd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                                cmd.Parameters.AddWithValue("@DoB", tb_dob.Text.Trim());
                                cmd.Parameters.AddWithValue("@Photo", tb_photo.Text.Trim());
                                //cmd.Parameters.AddWithValue("@Nric", encryptData(tb_nric.Text.Trim()));
                                cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                                cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                                //cmd.Parameters.AddWithValue("@DateTimeRegistered", DateTime.Now);
                                //cmd.Parameters.AddWithValue("@MobileVerified", DBNull.Value);
                                //cmd.Parameters.AddWithValue("@EmailVerified", DBNull.Value);
                                cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                                cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                                cmd.Parameters.AddWithValue("@OTP", DBNull.Value);
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
            else
            {
                
            }
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
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }
    }
}