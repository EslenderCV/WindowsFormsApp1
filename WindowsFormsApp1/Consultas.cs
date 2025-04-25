using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Consultas : Form
    {
        private HomeScreen screen;
        string access;

        public Consultas(HomeScreen screenn, string access = "Estandar")
        {
            this.access = access;
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

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            screen.openForm(new ConsultasVehiculos(screen));
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            screen.openForm(new ConsultasClientes(screen));
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            screen.openForm(new ConsultasOfertas(screen));
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            screen.openForm(new ConsultasReservas(screen, access));
        }
    }
}
