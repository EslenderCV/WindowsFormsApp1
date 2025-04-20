using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Mantenimientos: Form
    {
        HomeScreen screeen;

        public Mantenimientos(HomeScreen N)
        {
            InitializeComponent();

            screeen = N;
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form formBckg = new Form();

            using (usuariosMantenimientos modal = new usuariosMantenimientos())
            {
                formBckg.StartPosition = FormStartPosition.Manual;
                formBckg.FormBorderStyle = FormBorderStyle.None;
                formBckg.Opacity = .50d;
                formBckg.BackColor = Color.Black;
                formBckg.Size = screeen.Size;
                formBckg.Location = screeen.Location;
                formBckg.ShowInTaskbar = false;
                formBckg.Show();

                modal.Owner = formBckg;
                modal.ShowDialog();
                formBckg.Dispose();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            screeen.addBackToPanel(this);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form formBckg = new Form();

            using (marcasMantenimientos modal = new marcasMantenimientos())
            {
                formBckg.StartPosition = FormStartPosition.Manual;
                formBckg.FormBorderStyle = FormBorderStyle.None;
                formBckg.Opacity = .50d;
                formBckg.BackColor = Color.Black;
                formBckg.Size = screeen.Size;
                formBckg.Location = screeen.Location;
                formBckg.ShowInTaskbar = false;
                formBckg.Show();

                modal.Owner = formBckg;
                modal.ShowDialog();
                formBckg.Dispose();
            }
        }
    }
}
