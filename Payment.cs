using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Windows.Forms;
using System.Net;

namespace Test3
{ 
    public partial class Payment : Form
    {
        private string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Tonoy\source\repos\Test3\Test3\Project_DB\TestDB.mdf;Integrated Security=True;Connect Timeout=30";
        int wallet;
        float tprice;
        public Payment()
        {
            InitializeComponent();
        }

        private void Payment_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connection_string;
            conn.Open();
            
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select u_wallet from users where user_id = " + Form1.id;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                    wallet = reader.GetInt32(0);
            }
            reader.Close();
            

            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = conn;
            cmd1.CommandText = "SELECT p.p_name AS \"Fish Name\", CONCAT(c.c_quantity, ' KG') AS \"Quantity\", c.c_quantity * p.price AS \"Price\" FROM Cart c JOIN product p ON c.product_id = p.product_id;";
            SqlDataAdapter adapter = new SqlDataAdapter(cmd1);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataSource = table;
            dataGridView1.Font = new Font("Maiandra GD", 14, FontStyle.Regular);
            int rowCount = dataGridView1.Rows.Count;
            int totalHeight = dataGridView1.ColumnHeadersHeight;
            for (int i = 0; i < rowCount; i++)
            {
                totalHeight += dataGridView1.Rows[i].Height;
            }
            dataGridView1.Height = totalHeight + 3;
            label4.Top = dataGridView1.Bottom + 10;
            label5.Top = dataGridView1.Bottom + 10;
            label6.Top = dataGridView1.Bottom + 10;

            SqlCommand cmd2 = new SqlCommand();
            cmd2.Connection = conn;
            cmd2.CommandText = "SELECT SUM(c.c_quantity * p.price) FROM Cart c JOIN product p ON c.product_id = p.product_id;";
            SqlDataReader reader1 = cmd2.ExecuteReader();
            while (reader1.Read())
            {
                tprice = (float)reader1.GetDouble(0);
            }
            label5.Text = tprice.ToString();

            conn.Close();
            label3.Text = wallet.ToString();
        }

        private void SendOtpByEmail(string email)
        {
            string senderEmail = "mazharul141008@gmail.com";
            string senderPassword = "srgf bdvz yswu grnm";

            MailMessage mail = new MailMessage();
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress(senderEmail);
            mail.To.Add(email);
            mail.Subject = "Payment Successfull!";
            mail.Body = "Your Payment to FISH MART is Successful! \nYour Order is confirmed! \nThank you for shhopping with Fish Mart. \nStay Healthy.";

            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtpServer.EnableSsl = true;

            try
            {
                smtpServer.Send(mail);
                MessageBox.Show("Successfully Paid");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending email: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connection_string;
            conn.Open();

            SqlCommand cmd4 = new SqlCommand();
            cmd4.Connection = conn;
            cmd4.CommandText = "select * from cart";
            SqlDataReader reader2 = cmd4.ExecuteReader();
            if(!reader2.HasRows)
            {
                MessageBox.Show("You don't have anything in cart!");
            }
            else
            {
                reader2.Close();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE Owner SET o_wallet = o_wallet + (SELECT COALESCE(SUM(c.c_quantity * p.price), 0) AS total_price FROM Cart c " +
                                    "JOIN Product p ON c.product_id = p.product_id JOIN Product_Owner po ON p.product_id = po.product_id WHERE po.owner_id = Owner.owner_id);";
                cmd.ExecuteNonQuery();
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = conn;
                cmd1.CommandText = "update users set u_wallet = u_wallet - " + tprice + " where user_id = " + Form1.id;
                cmd1.ExecuteNonQuery();
                SendOtpByEmail(Form1.email1);

                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = conn;
                cmd2.CommandText = "select u_wallet from users where user_id = " + Form1.id;
                SqlDataReader reader = cmd2.ExecuteReader();
                if (reader.Read())
                {
                    wallet = reader.GetInt32(0);
                }
                reader.Close();
                label3.Text = wallet.ToString();

                SqlCommand cmd5 = new SqlCommand();
                cmd5.Connection = conn;
                cmd5.CommandText = "UPDATE Product SET quantity = quantity - ISNULL((SELECT SUM(ISNULL(c.c_quantity, 0)) FROM Cart c WHERE c.product_id = Product.product_id), 0);";
                cmd5.ExecuteNonQuery();
                
                SqlCommand cmd7 = new SqlCommand();
                cmd7.Connection = conn;
                cmd7.CommandText = "INSERT INTO payment (user_id, product_id, owner_id, price) SELECT 1 AS user_id, c.product_id, po.owner_id, c.c_quantity * p.price AS price FROM cart c " +
                                    "JOIN product p ON c.product_id = p.product_id JOIN product_owner po ON c.product_id = po.product_id;";
                cmd7.ExecuteNonQuery();

                B_Review breview = new B_Review();
                breview.Show();
                this.Hide();
            }
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cart cart = new Cart();
            cart.Show();
            this.Hide();
        }
    }
}
