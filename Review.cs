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
    public partial class Review : Form
    {
        private string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Tonoy\source\repos\Test3\Test3\Project_DB\TestDB.mdf;Integrated Security=True;Connect Timeout=30";
        public Review()
        {
            InitializeComponent();
        }

        private void Review_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connection_string;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT p.p_name as \"Fish Name\", r.rating as Rating, r.review as Review, u.u_name as Customer FROM product_owner po " +
                "JOIN product p ON po.product_id = p.product_id JOIN review r ON p.product_id = r.product_id JOIN users u ON r.user_id = u.user_iD " +
                "WHERE po.owner_id = " + Form1.id;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            SqlDataReader reader = cmd.ExecuteReader();
            if(reader.HasRows)
            {
                reader.Close();
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
                dataGridView1.Font = new Font("Maiandra GD", 14, FontStyle.Regular);
                dataGridView1.Visible = true;
                int rowCount = dataGridView1.Rows.Count;
                int totalHeight = dataGridView1.ColumnHeadersHeight;
                for (int i = 0; i < rowCount; i++)
                {
                    totalHeight += dataGridView1.Rows[i].Height;
                }
                dataGridView1.Height = totalHeight + 3;
            }
            else
            {
                Label label = new Label();
                label.Text = "There is no review for your any product!";
                label.Location = new Point(250, 150);
                label.Font = new Font("Maiandra GD", 14, FontStyle.Regular);
                label.AutoSize = true;
                label.BackColor = Color.Transparent;
                this.Controls.Add(label);
            }
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            S_Home s_Home = new S_Home();
            s_Home.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
