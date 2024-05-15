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
    public partial class AddProduct : Form
    {
        private string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Tonoy\source\repos\Test3\Test3\Project_DB\TestDB.mdf;Integrated Security=True;Connect Timeout=30";
        public AddProduct()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.ToString();
            string quantity = textBox2.Text.ToString();
            string price = textBox3.Text.ToString();
            string type = comboBox1.SelectedItem.ToString();
            SqlConnection conn = new SqlConnection(connection_string);
            conn.ConnectionString = connection_string;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO product (p_name, quantity, price, p_type) VALUES " + 
                "('" + name + "', " + quantity + ", " + price + ", '" + type + "');" +
                "DECLARE @product_id INT; SET @product_id = SCOPE_IDENTITY();" + 
                "INSERT INTO product_owner (product_id, owner_id) VALUES (@product_id, " + Form1.id + ");";
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Added Successfully!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            S_Home s_Home = new S_Home();
            s_Home.Show();
            this.Hide();
        }
    }
}
