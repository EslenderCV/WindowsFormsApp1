using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace WindowsFormsApp1
{
    public partial class HomeScreen: Form
    {
        public string namelast;
        public string emailFirst;
        public string access;

        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";

        public HomeScreen(string user)
        {
            

            InitializeComponent();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand($"SELECT * FROM Usuarios WHERE login_usuario = '{user}'", conn);
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        namelast = $"{rdr["nombre_usuario"].ToString()} {rdr["apellidos_usuario"].ToString()}";
                        emailFirst = $"{rdr["email_usuario"].ToString()}";
                        access = Convert.ToInt32(rdr["nivel_acceso"]) == 0 ? "Administrador" : "Estandar";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                } finally
                {
                    conn.Close();
                }
            }

            name.Text = namelast;
            email.Text = emailFirst;
            level.Text = access;

            DateTime dateTime = DateTime.UtcNow.Date;
            time.Text = dateTime.ToString("dd/MM/yyyy");

            label4.Text = getClientesQty();
            label5.Text = getVehiculosDiponiblesQty();
        }

        private string getClientesQty()
        {
            string cantidad = "0";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand($"SELECT COUNT(id_cliente) FROM Clientes", conn);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    cantidad = count.ToString();
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

            return cantidad;
        }

        private string getVehiculosDiponiblesQty()
        {
            string cantidad = "0";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand($"SELECT COUNT(id_vehiculo) FROM Vehiculos WHERE estado = 'Disponible'", conn);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    cantidad = count.ToString();
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

            return cantidad;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (panel1.Width == 250)
                panel1.Width = 70;
            else
                panel1.Width = 250;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox4_Click(object sender, EventArgs e)                                                                                                                                                                                                  
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public void openForm(object from)
        {
            if (this.panel3.Controls.Count > 0) {
                this.panel3.Controls.RemoveAt(0);
            }

            Form f = from as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.panel3.Controls.Add(f);
            this.panel3.Tag = f;
            f.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openForm(new Mantenimientos(this, access));
        }

        private void level_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            openForm(new Movimientos(this));
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            openForm(new Consultas(this));
        }

        public void addBackToPanel(Form winForm)
        {
            this.panel3.Controls.Add(panel5);

            winForm.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
