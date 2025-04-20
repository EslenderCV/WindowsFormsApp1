using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class marcasMantenimientos: Form
    {
        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";
        bool editing = false;
        int editingID;
        public marcasMantenimientos()
        {
            InitializeComponent();
            loadGridView();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(state.Text) || string.IsNullOrEmpty(name.Text)))
            {
                insertMarca(name.Text, state.Text);
            }
            else
            {
                MessageBox.Show("Uno o mas campos estan vacios!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void insertMarca(string namee, string statee)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string insert = $"INSERT INTO Marcas VALUES('{namee}', '{statee}')";
                    string update = $"UPDATE Marcas SET nombre_marca = '{namee}', estado = '{statee}' WHERE id_marca = {editingID}";
                    string messs = editing ? "actualizada" : "ingresada";

                    SqlCommand cmd = new SqlCommand(editing ? update : insert, conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Marca {messs} correctamente!");
                    name.Text = "";
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
                    editing = false;
                }


            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            name.Text = "";
        }

        private void loadGridView()
        {
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
                            dataGridView1.Rows.Add(reader["id_marca"], reader["nombre_marca"], reader["estado"]);
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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            name.Text = dataGridView1.SelectedCells[1].Value.ToString();
            state.SelectedIndex = dataGridView1.SelectedCells[2].Value.ToString() == "Y" ? 0 : 1;
            editing = true;
            editingID = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());
        }
    }
}
