using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MovimientoFactura: Form
    {
        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";

        public MovimientoFactura()
        {
            InitializeComponent();

            DateTime dateTime = DateTime.UtcNow.Date;
            time.Text = dateTime.ToString("dd/MM/yyyy");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand($"SELECT * FROM Alquileres", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {

                            alquileres.Items.Add($"{reader["id_alquiler"]}");

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

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void loadDatagridView(int ID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand($"SELECT * FROM Pagos_det WHERE id_alquiler = {ID}", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {

                            dataGridView1.Rows.Add(reader["id_pago"], reader["id_alquiler"], reader["fecha_pago"], reader["monto_pagado"], reader["metodo_pago"]);

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
            if (!(string.IsNullOrEmpty(alquileres.Text) || string.IsNullOrEmpty(start.Text) || string.IsNullOrEmpty(comboBox1.Text))) 
            {
                int IDAlquiler = Convert.ToInt32(alquileres.Text);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand($"INSERT INTO Pagos_det VALUES({IDAlquiler}, '{time.Text}', {Convert.ToDecimal(start.Text)}, '{comboBox1.Text}')", conn);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Pago ingresado correctamente!");
                        dataGridView1.Rows.Clear();
                        dataGridView1.Refresh();
                        loadDatagridView(IDAlquiler);
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
        }

        private void Clear()
        {
            comboBox1.SelectedIndex = 0;
            alquileres.SelectedIndex = 0;
            start.Clear();
        }

        private void clientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            if (!string.IsNullOrEmpty(alquileres.Text)) 
                loadDatagridView(Convert.ToInt32(alquileres.Text));
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void start_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
