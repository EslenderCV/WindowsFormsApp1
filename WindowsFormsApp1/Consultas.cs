using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Consultas : Form
    {
        private HomeScreen screen;

        public Consultas(HomeScreen screenn)
        {
            InitializeComponent();
            screen = screenn;
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            screen.openForm(new ConultaMarcas(screen));
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            screen.addBackToPanel(this);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            screen.openForm(new ConsultasModelos(screen));
        }
    }
}
