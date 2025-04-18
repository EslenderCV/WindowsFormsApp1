using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class usuariosMantenimientos : Form
    {
        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";
        public usuariosMantenimientos()
        {
            InitializeComponent();
        }


        private void usuariosMantenimientos_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int nivel = access.SelectedIndex;

            if (!(string.IsNullOrEmpty(access.Text) || string.IsNullOrEmpty(loginUser.Text) || string.IsNullOrEmpty(password.Text) ||
                string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(last.Text) || string.IsNullOrEmpty(email.Text)))
            {
                insertUser(loginUser.Text, password.Text, name.Text, last.Text, email.Text, nivel);
            } else
            {
                MessageBox.Show("Uno o mas campos estan vacios!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        public void insertUser(string user, string password, string first, string last, string email, int access)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand($"INSERT INTO Usuarios VALUES('{user}', '{password}', {access}, '{first}', '{last}', '{email}')", conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Usuario ingresado correctamente!");

                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close();

                }


            }

        }

        private void label7_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void clear()
        {
            password.Text = "";
            loginUser.Text = "";
            name.Text = "";
            last.Text = "";
            email.Text = "";
            access.Text = "";
        }
    }

}
