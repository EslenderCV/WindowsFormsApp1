using System;
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
    }
}
