using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class ConsultasVehiculos: Form
    {
        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";
        private HomeScreen screen;

        public ConsultasVehiculos(HomeScreen screeen)
        {
            screen = screeen;
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
            screen.openForm(new Consultas(screen));
        }

        private void price_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void marcas_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            if (marcas.SelectedIndex != 0)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        dataGridView1.Rows.Clear();


                        using (SqlCommand cmd = new SqlCommand($"SELECT * FROM Vehiculos WHERE marca = {Convert.ToInt32(marcas.Text.ToString().Split('.')[0])}", conn))
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
            } else
            {
                loadGridView();
            }
        }

        private void state_SelectedIndexChanged(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();

            if (state.SelectedIndex != 0)
            { 
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand($"SELECT * FROM Vehiculos WHERE estado = '{state.Text}'", conn))
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
            } else
            {
                loadGridView();
            }
        }
    }
}
