using RandomGen.Fluent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Test3
{
    public partial class ForgotPass : Form
    {
        private string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Tonoy\source\repos\Test3\Test3\Project_DB\TestDB.mdf;Integrated Security=True;Connect Timeout=30";
        private Random random = new Random();
        string otp;
        string emailc;
        string pass;
        string acctype;
        public ForgotPass()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void confirm_button_Click(object sender, EventArgs e)
        {
            emailc = emailtb.Text.ToString();
            pass = passwordtb.Text.ToString();
            string rpass = rpasswordtb.Text.ToString();
            if(string.IsNullOrEmpty(acctype))
            {
                MessageBox.Show("Please Select a User Category.");
            }
            else if(string.IsNullOrEmpty(emailc) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(rpass))
            {
                MessageBox.Show("All fields must be filled!");
            }
            else
            {

                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = connection_string;
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = conn;
                if (acctype == "Buyer")
                {
                    cmd.CommandText = "select * from users where u_email = '" + emailc + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Close();
                        otp = GenerateOTP();
                        SendOtpByEmail(emailc, otp);
                        groupBox1.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("This email address is invalid.\nEnter a valid Email Address.");
                    }
                }
                else
                {
                    cmd1.CommandText = "select * from owner where o_email = '" + emailc + "'";
                    SqlDataReader reader = cmd1.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Close();
                        otp = GenerateOTP();
                        SendOtpByEmail(emailc, otp);
                        groupBox1.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("This email address is invalid.\nEnter a valid Email Address.");
                    }
                }
                conn.Close();
            }
        }

        private string GenerateOTP()
        {
            return random.Next(100000, 999999).ToString();
        }
        private void SendOtpByEmail(string email, string otp)
        {
            string senderEmail = "mazharul141008@gmail.com";
            string senderPassword = "srgf bdvz yswu grnm";

            MailMessage mail = new MailMessage();
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress(senderEmail);
            mail.To.Add(email);
            mail.Subject = "OTP Verification for FISH MART";
            mail.Body = "Your OTP for Signing Up to FISH MART is: " + otp;

            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtpServer.EnableSsl = true;


            try
            {
                smtpServer.Send(mail);
                MessageBox.Show("OTP has been sent to " + email + ".\nPlease check your email and enter the code.") ;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending email: " + ex.Message);
            }
        }

        private void verify_button_Click(object sender, EventArgs e)
        {
            if(otp == otptb.Text.ToString())
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = connection_string;
                conn.Open();
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = conn;
                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = conn;
                if (acctype == "Buyer")
                {
                    cmd1.CommandText = "update users set u_password = '" + pass + "' where u_email = '" + emailc + "'";
                    cmd1.ExecuteNonQuery();
                }
                else
                {
                    cmd2.CommandText = "update owner set o_password = '" + pass + "' where o_email = '" + emailc + "'";
                    cmd2.ExecuteNonQuery();
                }
                
                conn.Close();
                MessageBox.Show("Verification Completed!\nYour Password is changed.");
                Form1 form = new Form1();
                form.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Incorrect Verification Code!");
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            acctype = "Seller";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            acctype = "Buyer";
        }
    }
}
