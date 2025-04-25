using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Movimientos: Form
    {
        private HomeScreen screen;
        public Movimientos(HomeScreen screen)
        {
            InitializeComponent();
            this.screen = screen;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            screen.addBackToPanel(this);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            openForm(new MovimientoReserva());
        }

        private void openForm(Form f)
        {
            Form formBckg = new Form();

            using (f)
            {
                formBckg.StartPosition = FormStartPosition.Manual;
                formBckg.FormBorderStyle = FormBorderStyle.None;
                formBckg.Opacity = .50d;
                formBckg.BackColor = Color.Black;
                formBckg.Size = screen.Size;
                formBckg.Location = screen.Location;
                formBckg.ShowInTaskbar = false;
                formBckg.Show();

                f.Owner = formBckg;
                f.ShowDialog();
                formBckg.Dispose();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            openForm(new MovimientoFactura());
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            openForm(new MovimientosRecepcion());
        }
    }
}
