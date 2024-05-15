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
    public partial class FreshwaterFish : Form
    {
        private string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Tonoy\source\repos\Test3\Test3\Project_DB\TestDB.mdf;Integrated Security=True;Connect Timeout=30";
        public static string selectedFishName;
        private Dictionary<Button, Label> buttonLabelMap = new Dictionary<Button, Label>();
        private int price;
        public static string selectedSellerName;

        public FreshwaterFish()
        {
            InitializeComponent();
        }

        private void FreshwaterFish_Load(object sender, EventArgs e)
        {
            LoadFishButtons();
        }

        private void LoadFishButtons()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connection_string;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select distinct p_name from product where p_type = 'Freshwater'";
            SqlDataReader reader = cmd.ExecuteReader();
            int buttonTop = 65;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string fishName = reader.GetString(0);
                    Button fishButton = new Button();
                    fishButton.Text = fishName;
                    fishButton.Size = new Size(150, 35);
                    fishButton.Click += FishButton_Click;
                    fishButton.Cursor = Cursors.Hand;
                    fishButton.Location = new Point(30, buttonTop);
                    fishButton.Font = new Font("Maiandra GD", 14, FontStyle.Regular);
                    buttonTop += 35 + 10;
                    this.Controls.Add(fishButton);
                }
            }
            conn.Close();
        }

        private void FishButton_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            Button clickedButton = (Button)sender;
            setSelectedFishName(clickedButton.Text.ToString());
            panel1.Visible = true;

            Label label1 = new Label();
            label1.Text = "Sellers And Prices: ";
            label1.Location = new Point(25, 25);
            label1.Font = new Font("Maiandra GD", 14, FontStyle.Regular);
            label1.Size = new Size(350, 35);

            panel1.Controls.Add(label1);
            Load_Seller_Buttons();
        }

        public void Load_Seller_Buttons()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connection_string;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT o.o_name FROM product_owner po JOIN product p ON po.product_id = p.product_id JOIN owner o ON po.owner_id = o.owner_id WHERE p.p_name = '" + selectedFishName + "';";
            SqlDataReader reader = cmd.ExecuteReader();
            int buttonTop = 70;

            List<string> sellerNames = new List<string>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string Seller_Name = reader.GetString(0);
                    sellerNames.Add(Seller_Name);
                }
            }
            reader.Close();

            foreach (string Seller_Name in sellerNames)
            {
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = conn;
                cmd1.CommandText = "SELECT p.price FROM product_owner po JOIN product p ON po.product_id = p.product_id JOIN owner o ON po.owner_id = o.owner_id WHERE p.p_name = '" + selectedFishName + "' and o.o_name = '" + Seller_Name + "';";
                SqlDataReader reader1 = cmd1.ExecuteReader();

                if (reader1.HasRows)
                {
                    while (reader1.Read())
                    {
                        price = reader1.GetInt32(0);
                    }
                }

                reader1.Close();

                Button Seller_Button = new Button();
                Seller_Button.Text = Seller_Name;
                Seller_Button.Size = new Size(150, 35);
                Seller_Button.Click += Seller_Button_Click;
                Seller_Button.Location = new Point(25, buttonTop);
                Seller_Button.Cursor = Cursors.Hand;
                Seller_Button.Font = new Font("Maiandra GD", 14, FontStyle.Regular);
                buttonTop += 35 + 10;
                panel1.Controls.Add(Seller_Button);

                Label label = new Label();
                label.Text = "Price per kg: " + price;
                label.AutoSize = true;
                label.Location = new Point(Seller_Button.Right + 10, Seller_Button.Top + 5);
                label.Font = new Font("Maiandra GD", 14, FontStyle.Regular);
                panel1.Controls.Add(label); 
                buttonLabelMap.Add(Seller_Button, label);
            }
            conn.Close();
        }

        public void Seller_Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            selectedSellerName = clickedButton.Text;

            FishPage fishPage = new FishPage();
            fishPage.Show();
            this.Hide();
        }
        public void setSelectedFishName(string fName)
        {
            selectedFishName = fName;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FormHome formHome = new FormHome();
            formHome.Show();
            this.Hide();
        }
    }
}
