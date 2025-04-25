using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Mantenimientos: Form
    {
        HomeScreen screeen;
        Form1 log;

        public Mantenimientos(HomeScreen N, string access, Form1 log)
        {
            InitializeComponent();

            if (access == "Estandar")
            {
                panel2.Controls.Remove(pictureBox1);
                panel2.Controls.Remove(label2);
                panel2.Controls.Remove(pictureBox7);
                panel2.Controls.Remove(label8);
                panel2.Controls.Remove(pictureBox4);
                panel2.Controls.Remove(label5);
            }

            screeen = N;
            this.log = log;
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            openForm(new usuariosMantenimientos(screeen, log));
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

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            openForm(new vehiculosMantenimientos());
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            openForm(new clientesMantenimientos());
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            openForm(new ofertasMantenimientos());
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            openForm(new mantMantenimientos());
        }
    }
}
