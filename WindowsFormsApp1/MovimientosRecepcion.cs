using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class MovimientosRecepcion: Form
    {
        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";
        public MovimientosRecepcion()
        {
            InitializeComponent();
            DateTime dateTime = DateTime.UtcNow.Date;
            time.Text = dateTime.ToString("dd/MM/yyyy");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Vehiculos WHERE estado = 'Reservado'", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            vehiculo.Items.Add($"{reader["id_vehiculo"]}. {getMarca(Convert.ToInt32(reader["marca"]))} {getModelo(Convert.ToInt32(reader["modelo"]))} {reader["año"]}");
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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    int vehiculos = Convert.ToInt32(vehiculo.Text.Split('.')[0]);


                    string insert = $"INSERT INTO Recepcion_Vehiculos VALUES({Convert.ToInt32(vehiculo.Text.Split('.')[0])}, '{time.Text}', '{observaciones.Text}')";

                    SqlCommand cmd = new SqlCommand( insert, conn);
                    cmd.ExecuteNonQuery();
                    setState(Convert.ToInt32(vehiculo.Text.Split('.')[0]));
                    MessageBox.Show($"Vehiculo recibido correctamente!");
                    Clear();
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

        private void Clear()
        {
            vehiculo.SelectedIndex = 0;
            observaciones.Clear();
        }
    }
}
