using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class modelosMantenimientos : Form
    {

        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";
        bool editing = false;
        int editingID;

        public modelosMantenimientos()
        {
            InitializeComponent();
            loadmodelosData();

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

                            comboBox1.Items.Add($"{reader["id_marca"]}.{reader["nombre_marca"]}");

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


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void loadmodelosData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Modelos", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {

                            dataGridView1.Rows.Add(reader["id_modelo"], reader["nombre_modelo"], reader["estado"], getMarca(Convert.ToInt32(reader["id_marca"])));

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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            name.Text = dataGridView1.SelectedCells[1].Value.ToString();
            state.SelectedIndex = dataGridView1.SelectedCells[2].Value.ToString() == "Y" ? 1 : 2;
            comboBox1.SelectedItem = "Honda";
            editing = true;
            editingID = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(state.Text) || string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(comboBox1.Text)))
            {
                insertModelo(name.Text, state.Text, comboBox1.Text);
            }
            else
            {
                MessageBox.Show("Uno o mas campos estan vacios!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void insertModelo(string namee, string statee, string marca)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    int marcas = Convert.ToInt32(marca.Split('.')[0]);
                     

                    string insert = $"INSERT INTO Modelos VALUES('{namee}', '{statee}', {marcas})";
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

        private void label7_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            name.Text = "";
            comboBox1.SelectedIndex = 0;
            state.SelectedIndex = 0;
            editing = false;
            editingID = 0;
        }
    }
}
