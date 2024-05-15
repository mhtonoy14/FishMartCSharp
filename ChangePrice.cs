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

namespace Test3
{
    public partial class ChangePrice : Form
    {
        private string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Tonoy\source\repos\Test3\Test3\Project_DB\TestDB.mdf;Integrated Security=True;Connect Timeout=30";
        string pname;
        int price;
        public ChangePrice()
        {
            InitializeComponent();
        }

        private void ChangePrice_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connection_string;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select p_name from product where product_id = " + Stock.productId;
            SqlDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                pname = reader.GetString(0);
            }
            reader.Close();

            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = conn;
            cmd1.CommandText = "select price from product where product_id = " + Stock.productId;
            SqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                price = reader1.GetInt32(0);
            }
            reader1.Close();
            conn.Close();
            label1.Text = "Change the Price of " + pname + " Fish";
            label3.Text = "Price of " + pname + " Fish:";
            textBox1.Text = price.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string textValue = textBox1.Text;
            if (int.TryParse(textValue, out int pricetb))
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = connection_string;
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "update product set price = " + pricetb + " where product_id = " + Stock.productId;
                cmd.ExecuteNonQuery();
            }
            else
            {
                MessageBox.Show("Input is not a valid integer.");
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Stock stock = new Stock();
            stock.Show();
            this.Hide();
        }
    }
}
