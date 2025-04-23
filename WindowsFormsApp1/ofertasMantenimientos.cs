using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    

    public partial class ofertasMantenimientos : Form
    {
        bool editing = false;
        int editingID;
        public ofertasMantenimientos()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            comboBox2.SelectedIndex = 0;
            des.Clear();
            price.Clear();
            date.Clear();
            editing = false;
            editingID = 0;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            editing = true;
            editingID = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());
        }
    }
}
