using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test3
{
    public partial class FormHome : Form
    {
        public static string fishtype;
        public FormHome()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void saltwaterFish_Click(object sender, EventArgs e)
        {
            fishtype = "Saltwater";
            SaltWaterFish saltwater = new SaltWaterFish();
            saltwater.Show();
            this.Hide();
        }

        private void freshwaterFish_Click(object sender, EventArgs e)
        {
            fishtype = "Freshwater";
            FreshwaterFish freshwater = new FreshwaterFish();
            freshwater.Show();
            this.Hide();
        }

        private void riverFish_Click(object sender, EventArgs e)
        {
            fishtype = "River";
            RiverFish river = new RiverFish();
            river.Show();
            this.Hide();
        }

        private void cultivatedFish_Click(object sender, EventArgs e)
        {
            fishtype = "Cultivated";
            CultivatedFish cultivated = new CultivatedFish();
            cultivated.Show();
            this.Hide();
        }
    }
}
