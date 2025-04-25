using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class mantMantenimientos: Form
    {
        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";

        public mantMantenimientos()
        {
            InitializeComponent();
            loadGridView();

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
                            vehicles.Items.Add($"{reader["id_vehiculo"]}. {getMarca(Convert.ToInt32(reader["marca"]))} {getModelo(Convert.ToInt32(reader["modelo"]))} {reader["año"]}");
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumer(e);
        }
        private void OnlyNumer(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loadGridView()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Mantenimiento", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(reader["id_mantenimiento"], getVehiculo(Convert.ToInt32(reader["id_vehiculo"])), reader["descripcion"], reader["fecha_inicio"].ToString().Split(' ')[0], 
                                reader["fecha_fin"].ToString().Split(' ')[0], reader["costo"]);
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

        private string getVehiculo(int ID)
        {
            string toReturn = "";

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
                            toReturn = $"{getMarca(Convert.ToInt32(reader["marca"]))} {getModelo(Convert.ToInt32(reader["modelo"]))} {reader["año"]}";
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

            return toReturn;
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
            string vehicle = vehicles.Text;
            string desc = des.Text;
            string incio = start.Text;
            string final = end.Text;
            string precios = price.Text;

            if (!(string.IsNullOrEmpty(vehicle) || string.IsNullOrEmpty(desc) || string.IsNullOrEmpty(incio) || string.IsNullOrEmpty(final) ||
                string.IsNullOrEmpty(precios)))
            {
                insertMantenimiento(vehicle, desc, incio, final, precios);
            }
            else
            {
                MessageBox.Show("Uno o mas campos estan vacios!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void insertMantenimiento(string vehicle, string desc, string incio, string final, string precios)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    int vehiculo = Convert.ToInt32(vehicle.Split('.')[0]);
                    int price = Convert.ToInt32(precios);

                    string insert = $"INSERT INTO Mantenimiento VALUES({vehiculo}, '{desc}', '{incio}', '{final}', {precios})";
                    setState(Convert.ToInt32(vehicles.Text.Split('.')[0]));

                    SqlCommand cmd = new SqlCommand(insert, conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Mantenimiento ingresado correctamente!");
                    Clear();
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

                    string update = $"UPDATE Vehiculos SET estado = 'En mantenimieto' WHERE id_vehiculo = {ID}";

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

        private void Clear()
        {
            vehicles.SelectedIndex = 0;
            des.Clear();
            price.Clear();
            start.Clear();
            end.Clear();
        }

        private void mantMantenimientos_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}
