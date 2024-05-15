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
    public partial class FishPage : Form
    {
        private string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Tonoy\source\repos\Test3\Test3\Project_DB\TestDB.mdf;Integrated Security=True;Connect Timeout=30";
        public static int price;
        float quantity;

        int pid;
        public  static int q;

        string fishName;
        string sellerName;

        

        public FishPage()
        {
            InitializeComponent();
        }

        private void FishPage_Load(object sender, EventArgs e)
        {
            if (FormHome.fishtype == "Saltwater")
            {
                fishName = SaltWaterFish.selectedFishName;
                sellerName = SaltWaterFish.selectedSellerName;
            }
            else if(FormHome.fishtype == "Freshwater")
            {
                fishName = FreshwaterFish.selectedFishName;
                sellerName = FreshwaterFish.selectedSellerName;
            }
            else if(FormHome.fishtype == "River")
            {
                fishName = RiverFish.selectedFishName;
                sellerName = RiverFish.selectedSellerName;
            }
            else if(FormHome.fishtype == "Cultivated")
            {
                fishName = CultivatedFish.selectedFishName;
                sellerName = CultivatedFish.selectedSellerName;
            }
            else if(FormHome.fishtype == "Imported")
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
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT p.price FROM product_owner po JOIN product p ON po.product_id = p.product_id JOIN owner o ON po.owner_id = o.owner_id WHERE p.p_name = '" + fishName + "' and o.o_name = '" + sellerName + "';";
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    price = reader.GetInt32(0);
                }
            }
            reader.Close();

            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = conn;
            cmd1.CommandText = "SELECT p.quantity FROM product_owner po JOIN product p ON po.product_id = p.product_id JOIN owner o ON po.owner_id = o.owner_id WHERE p.p_name = '" + fishName + "' and o.o_name = '" + sellerName + "';";
            SqlDataReader reader1 = cmd1.ExecuteReader();
            if (reader1.HasRows)
            {
                while (reader1.Read())
                {
                    quantity = (float)reader1.GetDouble(0);
                }
            }
            reader.Close();
            Label label1 = new Label();
            label1.Text = fishName + " Fish";
            label1.AutoSize = true;
            label1.Font = new Font("Maiandra GD", 30, FontStyle.Regular);
            label1.BackColor = Color.Transparent;
            int textWidth = TextRenderer.MeasureText(label1.Text, label1.Font).Width;
            int centerx = (this.ClientSize.Width - textWidth) / 2;
            label1.Location = new Point(centerx, 30);
            
            this.Controls.Add(label1);
            label2.Text = fishName + " fish price per kg: " + price + " BDT";
            label3.Text = "Available in stock: " + quantity + " KG";

            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(orderkgtb.Text, out q))
            {
                if(q > quantity)
                {
                    MessageBox.Show("Insufficient Stock! Please order a valid amount");
                }
                else
                {
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = connection_string;
                    conn.Open();
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.Connection = conn;
                    cmd1.CommandText = "SELECT p.product_id FROM product_owner po JOIN product p ON po.product_id = p.product_id JOIN owner o ON po.owner_id = o.owner_id WHERE p.p_name = '" + fishName + "' and o.o_name = '" + sellerName + "';";
                    SqlDataReader reader1 = cmd1.ExecuteReader();
                    while (reader1.Read())
                    {
                        pid = reader1.GetInt32(0);
                    }
                    reader1.Close();
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.Connection = conn;
                    cmd2.CommandText = "insert into cart (c_quantity, product_id) values (@c_quantity, @product_id)";
                    cmd2.Parameters.AddWithValue("@c_quantity", q);
                    cmd2.Parameters.AddWithValue("@product_id", pid);
                    cmd2.ExecuteNonQuery();
                    MessageBox.Show(q + " KG " + fishName + " Fish Added to Cart");

                    conn.Close();
                }
            }
            else
            {
                // Conversion failed, handle invalid input
                MessageBox.Show("Invalid input! Please enter a valid integer value.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaltWaterFish sf = new SaltWaterFish();
            sf.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Cart cart = new Cart();
            cart.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BP_Review bP_Review = new BP_Review();
            bP_Review.Show();
            this.Hide();
        }
    }
}
