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
        public static string ltype;
        public static int id;
        public static string email1;
        public Form1()
        {
            InitializeComponent();
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
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
            email1 = emailtb.Text.ToString();
            string password = passwordtb.Text.ToString();
            if(string.IsNullOrEmpty(ltype))
            {
                MessageBox.Show("Please Select a Log In Catagory");
            }
            else if (string.IsNullOrEmpty(email1) || string.IsNullOrEmpty(password))
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
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = conn;
                if (ltype == "Buyer")
                {
                    cmd.CommandText = "select user_id from users where u_email = '" + email1 + "' and u_password = '" + password + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            id = (int)reader.GetInt32(0);
                        }
                        FormHome formhome = new FormHome();
                        formhome.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid email or password!");
                    }
                }
                else
                {
                    cmd1.CommandText = "select owner_id from owner where o_email = '" + email1 + "' and o_password = '" + password + "'";
                    SqlDataReader reader = cmd1.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            id = (int)reader.GetInt32(0);
                        }    
                        S_Home s_Home = new S_Home();
                        s_Home.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid email or password!");
                    }
                }
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ltype = "Seller";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ltype = "Buyer";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            passwordtb.UseSystemPasswordChar = !checkBox1.Checked;
        }
    }
}
