using System;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace WindowsFormsApp1
{
    

    public partial class ofertasMantenimientos : Form
    {
        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";
        bool editing = false;
        int editingID;
        public ofertasMantenimientos()
        {
            InitializeComponent();
            loadDatagridView();
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
                            modelos.Items.Add($"{reader["id_vehiculo"]}. {getMarca(Convert.ToInt32(reader["marca"]))} {getModelo(Convert.ToInt32(reader["modelo"]))} {reader["año"]}");
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

        private decimal getPrecio(int ID)
        {
            decimal toReturn = 0;

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
                            toReturn = Convert.ToDecimal(reader["precio_diario"].ToString());
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

            return 0;

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            modelos.SelectedIndex = 0;
            des.Clear();
            price.Clear();
            date.Clear();
            editing = false;
            editingID = 0;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            editing = true;
            editingID = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(modelos.Text) || string.IsNullOrEmpty(des.Text) || string.IsNullOrEmpty(price.Text) || string.IsNullOrEmpty(date.Text)))
            {
                insertOferta(modelos.Text, des.Text, price.Text, date.Text);
            }
            else
            {
                MessageBox.Show("Uno o mas campos estan vacios!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void insertOferta(string modelo, string des, string price, string date)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    int model = Convert.ToInt32(modelo.Split('.')[0]);
                    decimal pr = Convert.ToDecimal(price);


                    string insert = $"INSERT INTO Ofertas VALUES({model}, '{des}', {pr}, '{date}')";
                    string update = $"UPDATE Ofertas SET id_vehiculo = {model}, descripcion_oferta = '{des}', precio_oferta = {pr}, limite_oferta = '{date}' WHERE id_oferta = {editingID}";
                    string messs = editing ? "actualizada" : "ingresada";

                    if (pr < ((decimal)0.15 * getPrecio(model))) { 
                        MessageBox.Show("El precio de la oferta no puede ser menor al 15% del precio original");
                        return;
                    }

                    SqlCommand cmd = new SqlCommand(editing ? update : insert, conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Oferta {messs} correctamente!");
                    Clear();
                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();
                    loadDatagridView();
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

        private void loadDatagridView()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Ofertas", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {

                            dataGridView1.Rows.Add(reader["id_oferta"], getModelo(Convert.ToInt32(reader["id_vehiculo"])), reader["descripcion_oferta"], reader["precio_oferta"], reader["limite_oferta"].ToString().Split(' ')[0]);

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

        private void price_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            des.Text = dataGridView1.SelectedCells[2].Value.ToString(); 
            price.Text = dataGridView1.SelectedCells[3].Value.ToString();
            date.Text = dataGridView1.SelectedCells[4].Value.ToString();
            editing = true;
            editingID = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());
        }
    }
}
