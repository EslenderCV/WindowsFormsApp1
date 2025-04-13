using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ConsultasModelos: Form
    {
        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";

        private HomeScreen screen;
        public ConsultasModelos(HomeScreen screenn)
        {
            InitializeComponent();
            screen = screenn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
