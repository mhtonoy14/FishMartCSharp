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
    public partial class Stock : Form
    {
        private string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Tonoy\source\repos\Test3\Test3\Project_DB\TestDB.mdf;Integrated Security=True;Connect Timeout=30";
        public static int productId;
        public Stock()
        {
            InitializeComponent();
        }

        private void Stock_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connection_string;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT p.product_id, p.p_name, p.quantity, p.price FROM product p INNER JOIN product_owner po ON p.product_id = po.product_id WHERE po.owner_id = " + Form1.id;
            SqlDataReader reader = cmd.ExecuteReader();

            int yPos = 140;
            while (reader.Read())
            {
                string productName = reader["p_name"].ToString();
                float quantity;
                if (!float.TryParse(reader["quantity"].ToString(), out quantity))
                {
                    quantity = 0.0f;
                }
                int price = (int)reader["price"];
                int pId = (int)reader["product_id"];

                Label nameLabel = new Label();
                nameLabel.Text = productName + " Fish " + quantity + " KG, per KG " + price + " BDT";
                nameLabel.AutoSize = true;
                nameLabel.BackColor = Color.Transparent;
                nameLabel.Font = new Font("Maiandra GD", 14, FontStyle.Regular);
                nameLabel.Location = new Point(120, yPos);

                Button restockButton = new Button();
                restockButton.Text = "Restock";
                restockButton.Font = new Font("Maiandra GD", 14, FontStyle.Regular);
                restockButton.Location = new Point(450, yPos - 5);
                restockButton.Size = new Size(120, 35);
                restockButton.Tag = pId;

                Button priceButton = new Button();
                priceButton.Text = "Change Price";
                priceButton.Font = new Font("Maiandra GD", 14, FontStyle.Regular);
                priceButton.Location = new Point(580, yPos - 5);
                priceButton.Size = new Size(150, 35);
                priceButton.Tag = pId;

                PictureBox pictureBox = new PictureBox();
                pictureBox.Location = new Point(50, yPos - 20); 
                pictureBox.Size = new Size(60, 60);  
                pictureBox.BackColor = Color.Transparent;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage; 
                pictureBox.Image = Properties.Resources.fish; 

                restockButton.Click += RestockButton_Click;
                priceButton.Click += PriceButton_Click;

                this.Controls.Add(nameLabel);
                this.Controls.Add(restockButton);
                this.Controls.Add(priceButton);
                this.Controls.Add(pictureBox);

                yPos += 80;
            }
            reader.Close();
            conn.Close();
        }

        private void RestockButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            productId = (int)button.Tag; 

            Restock restock = new Restock();
            restock.Show();
            this.Hide();
        }

        private void PriceButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            productId = (int)button.Tag;

            ChangePrice cp = new ChangePrice();
            cp.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            S_Home s_Home = new S_Home();
            s_Home.Show();
            this.Hide();
        }
    }
}
