using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test3
{
    public partial class Cart : Form
    {
        private string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Tonoy\source\repos\Test3\Test3\Project_DB\TestDB.mdf;Integrated Security=True;Connect Timeout=30";
        float tprice;
        public Cart()
        {
            InitializeComponent();
        }

        private void Cart_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connection_string;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT p.p_name AS \"Fish Name\", CONCAT(c.c_quantity, ' KG') AS \"Quantity\", c.c_quantity * p.price AS \"Price\" FROM Cart c JOIN product p ON c.product_id = p.product_id;";
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
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
            label2.Top = dataGridView1.Bottom + 10;
            label3.Top = dataGridView1.Bottom + 10;
            label4.Top = dataGridView1.Bottom + 10;
            
            SqlCommand cmd2 = new SqlCommand();
            cmd2.Connection = conn;
            cmd2.CommandText = "SELECT SUM(c.c_quantity * p.price) FROM Cart c JOIN product p ON c.product_id = p.product_id;";
            SqlDataReader reader = cmd2.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tprice = (float)reader.GetDouble(0);
                }
                label3.Text = tprice.ToString();
                
            }
            else
            {
                dataGridView1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                Label label = new Label();
                label.Text = "There is no product in your cart";
                label.Location = new Point(250, 150);
                label.Font = new Font("Maiandra GD", 14, FontStyle.Regular);
                label.AutoSize = true;
                label.BackColor = Color.Transparent;
                this.Controls.Add(label);
            }
            conn.Close();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            button1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            string quantitystr = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            string pricestr = dataGridView1.CurrentRow.Cells[2].Value.ToString();

            float quantity;
            int price;

            Match match = Regex.Match(quantitystr, @"\d+");
            if (match.Success)
            {
                string numericPart = match.Value;
                if (float.TryParse(numericPart, out quantity) && int.TryParse(pricestr, out price))
                {
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = connection_string;
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM Cart WHERE product_id IN (SELECT product_id FROM product WHERE p_name = '" + name + "' AND c_quantity * price = " + price + ") AND c_quantity = " + quantity;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(name + " Fish Deleted from the Cart");
                    
                    
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.Connection = conn;
                    cmd1.CommandText = "SELECT p.p_name AS \"Fish Name\", CONCAT(c.c_quantity, ' KG') AS \"Quantity\", c.c_quantity * p.price AS \"Price\" FROM Cart c JOIN product p ON c.product_id = p.product_id;";
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd1);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.DataSource = table;
                    dataGridView1.Font = new Font("Maiandra GD", 14, FontStyle.Regular);
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.Connection = conn;
                    cmd2.CommandText = "SELECT SUM(c.c_quantity * p.price) FROM Cart c JOIN product p ON c.product_id = p.product_id;";
                    SqlDataReader reader = cmd2.ExecuteReader();
                    while (reader.Read())
                    {
                        tprice = reader.GetInt32(0);
                    }
                    label3.Text = tprice.ToString();
                    conn.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FishPage fishPage = new FishPage();
            fishPage.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Payment payment = new Payment();
            payment.Show();
            this.Hide();
        }
    }
}
