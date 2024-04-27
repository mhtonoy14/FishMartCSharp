using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using RandomGen;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;

namespace Test3
{
    public partial class SignUp : Form
    {
        private string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Tonoy\source\repos\Test3\Project_DB\TestDB.mdf;Integrated Security=True;Connect Timeout=30";
        private Random random = new Random();
        string otp;
        string name;
        string email;
        string phone;
        string password;
        string address;
        public SignUp()
        {
            InitializeComponent();
        }

        private void SignUp_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            name = nametb.Text.ToString();
            email = emailtb.Text.ToString();
            phone = phonetb.Text.ToString();
            password = passwordtb.Text.ToString();
            address = addresstb.Text.ToString();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(address))
            {
                DialogResult dialogResult = MessageBox.Show("No empty fields allowed, \nPlease fill up all the fields", "Can't Register", MessageBoxButtons.OK);
            }
            else
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = connection_string;
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from users where u_email = '" + email + "'";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    MessageBox.Show("The email is already been used.\nEnter a different valid email");
                }
                else
                {
                    reader.Close();
                    groupBox1.Visible = true;
                    otp = GenerateOTP();
                    SendOtpByEmail(email, otp);
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
                MessageBox.Show("OTP has been sent to your email. Please check your inbox and verify.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending email: " + ex.Message);
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(otp == otptb.Text.ToString())
            {

                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = connection_string;
                conn.Open();
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = conn;
                cmd1.CommandText = "insert into users (u_name, u_phone, u_email, u_address, u_password) values ('" + name + "', '" + phone + "', '" + email + "', '" + address + "', '" + password + "')";
                cmd1.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Verification Completed!");
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("OTP is incorrect.");
            }
        }
    }
}
