using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace WindowsFormsApp1
{
    public partial class vehiculosMantenimientos: Form
    {
        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";
        bool editing = false;
        int editingID;

        public vehiculosMantenimientos()
        {
            InitializeComponent();
            loadGridView();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Marcas", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {

                            marcas.Items.Add($"{reader["id_marca"]}.{reader["nombre_marca"]}");

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
            
        private void button1_Click(object sender, EventArgs e)
        {
            string marca = marcas.Text;
            string modelo = modelos.Text;
            string yearr = year.Text;
            string chasis = chas.Text;
            string placa = plate.Text;
            string color = colorr.Text;
            string tipo = type.Text;
            string precio = price.Text;
            string estado = state.Text;
            string km = kilom.Text;

            if (!(string.IsNullOrEmpty(marca) || string.IsNullOrEmpty(modelo) || string.IsNullOrEmpty(yearr) || string.IsNullOrEmpty(chasis) ||
                string.IsNullOrEmpty(placa) || string.IsNullOrEmpty(color) || string.IsNullOrEmpty(tipo) || string.IsNullOrEmpty(precio) ||
                string.IsNullOrEmpty(estado) || string.IsNullOrEmpty(km)))
            {
                insertVehiculo(name.Text, state.Text, type.Text);
            }
            else
            {
                MessageBox.Show("Uno o mas campos estan vacios!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void insertVehiculo(string namee, string statee, string marca)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    int marcas = Convert.ToInt32(marca.Split('.')[0]);


                    string insert = $"INSERT INTO Modelos VALUES('{namee}', '{statee}', {Convert.ToInt32(marcas)})";
                    string update = $"UPDATE Modelos SET nombre_modelo = '{namee}', estado = '{statee}', id_marca={marcas} WHERE id_modelo = {editingID}";
                    string messs = editing ? "actualizado" : "ingresado";

                    SqlCommand cmd = new SqlCommand(editing ? update : insert, conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Modelo {messs} correctamente!");
                    Clear();
                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();
                    loadmodelosData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                    editing = false;
                }


            }
        }

        private string getMarca(int ID)
        {
            string toReturn = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand($"SELECT * FROM Marcas WHERE id_marca = {ID}", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            toReturn = reader["nombre_marca"].ToString();
                        }

                        reader.Close();
                        return toReturn;
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

            return null;

        }

        private string getModelo(int ID)
        {
            string toReturn = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand($"SELECT * FROM Modelos WHERE id_modelo = {ID}", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            toReturn = reader["nombre_modelo"].ToString();
                        }

                        reader.Close();
                        return toReturn;
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

            return null;

        }

        private void loadGridView()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Vehiculos", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(reader["id_vehiculo"], getMarca(Convert.ToInt32(reader["marca"])), getModelo(Convert.ToInt32(reader["modelo"])), reader["año"], 
                                reader["chasis"], reader["placa"], reader["color"], reader["tipo_vehiculo"], reader["precio_diario"], 
                                reader["estado"], reader["kilometraje"]);
                        }
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

        private void marcas_SelectedIndexChanged(object sender, EventArgs e)
        {
            modelos.Items.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand($"SELECT * FROM Modelos WHERE id_marca = {Convert.ToInt32(marcas.Text.Split('.')[0])}", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {

                            modelos.Items.Add($"{reader["id_modelo"]}.{reader["nombre_modelo"]}");

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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void vehiculosMantenimientos_Load(object sender, EventArgs e)
        {

        }
    }
}
