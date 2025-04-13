using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{

    public partial class Form1: Form
    {
        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }
        private bool AuthenticateUser(string username, string password)
        {
            bool isAuthenticated = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM Usuarios WHERE login_usuario = @username AND pass_usuario = @password";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                try
                {
                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 1)
                        isAuthenticated = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                } finally
                {
                    conn.Close();
                }
            }

            return isAuthenticated;
        }


        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            login();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Enter)
            {
                login();
            }
        }

        private void login()
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (username != "" && password != "")
            {
                if (AuthenticateUser(username, password))
                {
                    new HomeScreen(username).Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Usuario y/o contraseña no valido. Intente nuevamente");
                }
            } else
            {
                MessageBox.Show("Hay campos vacios, complete y continue.");
            }

            textBox2.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
