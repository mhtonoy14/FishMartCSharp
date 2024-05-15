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
using System.Windows.Forms;
using System.Xml.Linq;

namespace Test3
{
    public partial class BP_Review : Form
    {
        private string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Tonoy\source\repos\Test3\Test3\Project_DB\TestDB.mdf;Integrated Security=True;Connect Timeout=30";
        string fishName;
        string sellerName;
        int pID;
        public BP_Review()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void BP_Review_Load(object sender, EventArgs e)
        {
            if (FormHome.fishtype == "Saltwater")
            {
                fishName = SaltWaterFish.selectedFishName;
                sellerName = SaltWaterFish.selectedSellerName;
            }
            else if (FormHome.fishtype == "Freshwater")
            {
                fishName = FreshwaterFish.selectedFishName;
                sellerName = FreshwaterFish.selectedSellerName;
            }
            else if (FormHome.fishtype == "River")
            {
                fishName = RiverFish.selectedFishName;
                sellerName = RiverFish.selectedSellerName;
            }
            else if (FormHome.fishtype == "Cultivated")
            {
                fishName = CultivatedFish.selectedFishName;
                sellerName = CultivatedFish.selectedSellerName;
            }
            else if (FormHome.fishtype == "Imported")
            {
                fishName = ImportedFish.selectedFishName;
                sellerName = ImportedFish.selectedSellerName;
            }
            else
            {
                fishName = ExoticFish.selectedFishName;
                sellerName = ExoticFish.selectedSellerName;
            }
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connection_string;
            conn.Open();
            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = conn;
            cmd1.CommandText = "SELECT p.product_id FROM product p JOIN product_owner po ON p.product_id = po.product_id " + 
                    "JOIN owner o ON po.owner_id = o.owner_id WHERE p.p_name = '" + fishName + "' AND o.o_name = '" + sellerName + "' AND p.price = " + FishPage.price;
            SqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                pID = reader1.GetInt32(0);
            }
            reader1.Close();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT u.u_name as Users, r.rating as Rating, r.review as Review FROM review r JOIN users u ON r.user_id = u.user_id WHERE r.product_id = " + pID;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
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
                label.Text = "There is no review for your any product!" ;
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
            FishPage fishPage = new FishPage();
            fishPage.Show();
            this.Hide();
        }
    }
}
