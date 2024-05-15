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
    public partial class Restock : Form
    {
        private string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Tonoy\source\repos\Test3\Test3\Project_DB\TestDB.mdf;Integrated Security=True;Connect Timeout=30";
        string productName;
        float quantity;
        public Restock()
        {
            InitializeComponent();
        }

        private void Restock_Load(object sender, EventArgs e)
        {
            
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connection_string;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select p_name, quantity from product where product_id = " + Stock.productId.ToString();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                productName = reader.GetString(0);
                if (!float.TryParse(reader["quantity"].ToString(), out quantity))
                {
                    quantity = 0.0f;
                }

            }
            reader.Close();
            conn.Close();

            label2.Text = productName + " Fish Available in Stock: " + quantity + " KG";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string textValue = textBox1.Text.ToString();
            float q;
            if (float.TryParse(textValue, out q))
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = connection_string;
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "update product set quantity = quantity + " + q + " where product_id = " + Stock.productId;
                cmd.ExecuteNonQuery();
                conn.Close();
                FormLoad();
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connection_string;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "delete from product where product_id = " + Stock.productId;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Stock stock = new Stock();
            stock.Show();
            this.Hide();
        }

        private void FormLoad()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connection_string;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select p_name, quantity from product where product_id = " + Stock.productId.ToString();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                productName = reader.GetString(0);
                if (!float.TryParse(reader["quantity"].ToString(), out quantity))
                {
                    quantity = 0.0f;
                }

            }
            reader.Close();
            conn.Close();

            label2.Text = productName + " Fish Available in Stock: " + quantity + " KG";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
