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
    public partial class Wallet : Form
    {
        private string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Tonoy\source\repos\Test3\Test3\Project_DB\TestDB.mdf;Integrated Security=True;Connect Timeout=30";
        int wallet;
        int tprice;
        public Wallet()
        {
            InitializeComponent();
        }

        private void Wallet_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connection_string;
            conn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select o_wallet from owner where owner_id = "+ Form1.id;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                wallet = reader.GetInt32(0);
            }
            reader.Close();


            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = conn;
            cmd1.CommandText = "SELECT pr.p_name as \"Fish Name\", pa.price, u.u_name as \"Customer\" FROM payment pa JOIN product pr ON pa.product_id = pr.product_id JOIN users u ON pa.user_id = u.user_id WHERE pa.owner_id = " + Form1.id;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd1);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataSource = table;
            dataGridView1.Font = new Font("Maiandra GD", 14, FontStyle.Regular);
            dataGridView1.CurrentCell = null;
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
            cmd2.CommandText = "SELECT SUM(price) AS total_price FROM payment WHERE owner_id = " + Form1.id;
            SqlDataReader reader1 = cmd2.ExecuteReader();
            while (reader1.Read())
            {
                tprice = (int)reader1.GetInt32(0);
            }
            label5.Text = tprice.ToString();

            conn.Close();
            label3.Text = wallet.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            S_Home s_Home = new S_Home();
            s_Home.Show();
            this.Hide();
        }
    }
}
