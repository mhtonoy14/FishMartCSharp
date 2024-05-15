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
    public partial class B_Review : Form
    {
        private string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Tonoy\source\repos\Test3\Test3\Project_DB\TestDB.mdf;Integrated Security=True;Connect Timeout=30";
        private List<Product> cartProducts;
        Product selectedProduct;

        public B_Review()
        {
            InitializeComponent();
        }

        private void B_Review_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connection_string;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT p.product_id, p.p_name FROM cart c JOIN product p ON c.product_id = p.product_id ";
            SqlDataReader reader = cmd.ExecuteReader();

            cartProducts = new List<Product>();
            while(reader.Read())
            {
                Product product = new Product
                {
                    ProductId = reader.GetInt32(0),
                    ProductName = reader.GetString(1)
                };
                cartProducts.Add(product);
            }
            reader.Close();

            foreach (Product product in cartProducts)
            {
                listBox1.Items.Add(product);
            }
            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = conn;
            cmd1.CommandText = "delete from cart";
            cmd1.ExecuteNonQuery();

            conn.Close();

            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 5;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Visible = true;
            numericUpDown1.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            submitbtn.Visible = true;
            selectedProduct = (Product)listBox1.SelectedItem;
        }

        private void submitbtn_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value < 1)
            {
                numericUpDown1.Value = 1; 
            }
            else if (numericUpDown1.Value > 5)
            {
                numericUpDown1.Value = 5; 
            }
            int rating = (int)numericUpDown1.Value;
            string reviewText = richTextBox1.Text.ToString();

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connection_string;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO review (rating, review, product_id, user_id) VALUES (@Rating, @ReviewText, @ProductId, @UserId)";
            cmd.Parameters.AddWithValue("@Rating", rating);
            cmd.Parameters.AddWithValue("@ReviewText", reviewText);
            cmd.Parameters.AddWithValue("@ProductId", selectedProduct.ProductId);
            cmd.Parameters.AddWithValue("@UserId", Form1.id);
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Review Submitted Successfully!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormHome formHome = new FormHome();
            formHome.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public override string ToString()
        {
            return ProductName; 
        }
    }
}
