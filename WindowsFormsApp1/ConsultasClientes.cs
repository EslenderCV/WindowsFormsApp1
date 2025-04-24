using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ConsultasClientes : Form
    {
        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";
        HomeScreen screen;

        public ConsultasClientes(HomeScreen screeen)
        {
            screen = screeen;
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

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Clientes", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(reader["id_cliente"], reader["nombre"], reader["email"], reader["telefono"],
                                reader["direccion"], reader["fecha_nacimiento"].ToString().Split(' ')[0], reader["tipo_documento"], reader["num_documento"]);
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

        private void button1_Click(object sender, EventArgs e)
        {
            screen.openForm(new Consultas(screen));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        dataGridView1.Rows.Clear();


                        using (SqlCommand cmd = new SqlCommand($"SELECT * FROM Clientes WHERE id_cliente = {Convert.ToInt32(textBox1.Text)}", conn))
                        {
                            SqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                dataGridView1.Rows.Add(reader["id_cliente"], reader["nombre"], reader["email"], reader["telefono"],
                                reader["direccion"], reader["fecha_nacimiento"].ToString().Split(' ')[0], reader["tipo_documento"], reader["num_documento"]);
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
            else
            {
                loadGridView();
            }
        }

        private void price_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            if (!string.IsNullOrEmpty(text2.Text))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        dataGridView1.Rows.Clear();


                        using (SqlCommand cmd = new SqlCommand($"SELECT* FROM clientes WHERE id_cliente BETWEEN {Convert.ToInt32(text1.Text)} AND {Convert.ToInt32(text2.Text)}", conn))
                        {
                            SqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                dataGridView1.Rows.Add(reader["id_cliente"], reader["nombre"], reader["email"], reader["telefono"],
                                reader["direccion"], reader["fecha_nacimiento"].ToString().Split(' ')[0], reader["tipo_documento"], reader["num_documento"]);
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
            else
            {
                loadGridView();
            }
        }

        private void text2_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumer(e);
        }

        private void text1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumer(e);
        }
    }
}
