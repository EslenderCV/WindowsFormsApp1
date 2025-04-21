using System;
using System.Drawing;
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
            openForm(new usuariosMantenimientos());
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
            openForm(new marcasMantenimientos());
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            openForm(new modelosMantenimientos());
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
                formBckg.Size = screeen.Size;
                formBckg.Location = screeen.Location;
                formBckg.ShowInTaskbar = false;
                formBckg.Show();

                f.Owner = formBckg;
                f.ShowDialog();
                formBckg.Dispose();
            }
        }
    }
}
