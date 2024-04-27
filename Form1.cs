using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Test3
{
    public partial class Form1 : Form
    {
        private string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Tonoy\source\repos\Test3\Test3\Project_DB\TestDB.mdf;Integrated Security=True;Connect Timeout=30";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }

        private void loginbutton_Click(object sender, EventArgs e)
        {
            string email = emailtb.Text.ToString();
            string password = passwordtb.Text.ToString();
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                DialogResult dialogResult = MessageBox.Show("No empty fields allowed", "You cannot continue", MessageBoxButtons.OK);
            }
            else
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = connection_string;
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from users where u_email = '" + email + "' and u_password = '" + password + "'";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    FormHome formhome = new FormHome();
                    formhome.Show();
                    this.Hide();
                    //while (reader.Read())
                    //{
                    //MessageBox.Show("User found for id:" + reader.GetInt32(0).ToString());
                    //}
                }
                else
                {
                    MessageBox.Show("Invalid email or password!");
                }
                //conn.Open();MessageBox.Show("Connection Succeeded");
                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SignUp signUp = new SignUp();
            signUp.Show();
            this.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgotPass forgotPass = new ForgotPass();
            forgotPass.Show();
            this.Hide();
        }
    }
}
