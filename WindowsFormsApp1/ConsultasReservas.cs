using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ConsultasReservas : Form
    {
        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";
        HomeScreen screen;
        string access;

        public ConsultasReservas(HomeScreen screen, string access = "Estandar")
        {
            this.access = access;
            this.screen = screen;
            InitializeComponent();
            loadGridView();

            if (this.access != "Administrador")
            {
                button2.Enabled = false;
            }
        }

        private void loadGridView()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Reservas", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(reader["id_reserva"], getCliente(Convert.ToInt32(reader["id_cliente"])), getVehiculo(Convert.ToInt32(reader["id_vehiculo"])), reader["num_documento"], reader["fecha_reserva"]
                            , reader["fecha_inicio"], reader["fecha_fin"], reader["estado"]);
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

        private string getCliente(int ID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand($"SELECT * FROM Clientes WHERE id_cliente = {ID}", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {

                            return reader["nombre"].ToString();

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

            return "";
        }

        private string getVehiculo(int ID)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand($"SELECT * FROM Vehiculos WHERE id_vehiculo = {ID}", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string toReturn = $"{reader["id_vehiculo"]}. {getMarca(Convert.ToInt32(reader["marca"]))} {getModelo(Convert.ToInt32(reader["modelo"]))} {reader["año"]}";
                            return toReturn;
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

            return "";
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


        private void button1_Click(object sender, EventArgs e)
        {
            screen.openForm(new Consultas(screen, access));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Va a eliminar una reserva, esta seguro?", "??", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
                deleteReserva(Convert.ToInt32(dataGridView1.SelectedCells[0].Value));
        }

        private void deleteReserva(int ID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string insert = $"DELETE FROM Reservas WHERE id_reserva = {ID}";
                    setState(Convert.ToInt32(dataGridView1.SelectedCells[2].Value.ToString().Split('.')[0]));

                    SqlCommand cmd = new SqlCommand(insert, conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Reserva eliminada");
                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();
                    loadGridView();
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
    
        private void setState(int ID)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        string update = $"UPDATE Vehiculos SET estado = 'Disponible' WHERE id_vehiculo = {ID}";

                        SqlCommand cmd = new SqlCommand(update, conn);
                        cmd.ExecuteNonQuery();

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
        }
    }
