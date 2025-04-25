using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class usuariosMantenimientos : Form
    {
        bool editing = false;
        string editingID = "";

        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";
        HomeScreen screen;
        Form1 log;
        public usuariosMantenimientos(HomeScreen screen, Form1 log)
        {
            this.screen = screen;
            this.log = log;
            InitializeComponent();
            loadGridView();
            
        }

        private void loadGridView()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Usuarios", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {

                            dataGridView1.Rows.Add(reader["login_usuario"], reader["nombre_usuario"], reader["apellidos_usuario"], reader["email_usuario"],
                                Convert.ToInt32(reader["nivel_acceso"].ToString()) == 1 ? "Estandar" : "Administrador");
                        }

                        reader.Close();
                    }

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

                    string insert = $"INSERT INTO Usuarios VALUES('{user}', '{password}', {access}, '{first}', '{last}', '{email}')";
                    string update = $"UPDATE Usuarios SET pass_usuario = '{password}', nivel_acceso = {access}, nombre_usuario = '{first}', apellidos_usuario = '{last}', email_usuario = '{email}' WHERE login_usuario = '{user}'";

                    SqlCommand cmd = new SqlCommand(editing ? update : insert, conn);
                    cmd.ExecuteNonQuery();

                    string mess = editing ? "actualizado" : "ingresado";

                    MessageBox.Show($"Usuario {mess} correctamente!");
                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();
                    loadGridView();
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
            editing = false;
            editingID = "";
        }

        private void loginUser_Leave(object sender, EventArgs e)
        {
        }

        private void loginUser_TextChanged(object sender, EventArgs e)
        {
            
            bool isAuthenticated = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM Usuarios WHERE login_usuario = @username";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", loginUser.Text);

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
                }
                finally
                {
                    conn.Close();
                }
            }

            if (isAuthenticated)
            {
                DialogResult dialogResult = MessageBox.Show("Usuario existente, desea iniciar sesion?", "??", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    log.Show();
                    screen.Close();
                    this.Close();
                }
            }

            

        }

        private void label9_Click(object sender, EventArgs e)
        {
            loginUser.Text = dataGridView1.SelectedCells[0].Value.ToString();
            name.Text = dataGridView1.SelectedCells[1].Value.ToString();
            last.Text = dataGridView1.SelectedCells[2].Value.ToString();
            email.Text = dataGridView1.SelectedCells[3].Value.ToString();
            access.SelectedItem = dataGridView1.SelectedCells[4].Value.ToString();
            editing = true;
            editingID = dataGridView1.SelectedCells[0].Value.ToString();
        }
    }

}
