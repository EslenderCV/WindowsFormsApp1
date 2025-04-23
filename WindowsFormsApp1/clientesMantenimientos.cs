using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace WindowsFormsApp1
{
    public partial class clientesMantenimientos : Form
    {
        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";
        bool editing = false;
        int editingID;

        public clientesMantenimientos()
        {
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
                                reader["direccion"], reader["fecha_nacimiento"], reader["tipo_documento"], reader["numero_documento"]);
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

        private void kilom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

        }

        private void plate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nombre = name.Text;
            string email = eaddress.Text;
            string number = phonenum.Text;
            string add = address.Text;
            string date = birthDate.Text;
            string docuType = type.Text;
            string docNumber = docNum.Text;

            if (!(string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(number) || string.IsNullOrEmpty(add) ||
                string.IsNullOrEmpty(date) || string.IsNullOrEmpty(docuType) || string.IsNullOrEmpty(docNumber)))
            {
                insertCustomer(nombre, email, number, add, date, docuType, docNumber);
            }
            else
            {
                MessageBox.Show("Uno o mas campos estan vacios!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void insertCustomer(string nombre, string email, string number, string add, string date, string docuType, string docNumber)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string insert = $"INSERT INTO Clientes VALUES({nombre}, {email}, {number}, '{add}', '{date}', '{docuType}', '{docNumber}')";
                    string update = $"UPDATE Clientes SET nombre = {nombre}, email = {email}, telefono = {number}, direccion = '{add}', " +
                        $"fecha_nacimiento = '{date}', tipo_documento = '{docuType}', numero_documento = '{docNumber}' WHERE id_cliente = {editingID}";

                    string messs = editing ? "actualizado" : "ingresado";

                    SqlCommand cmd = new SqlCommand(editing ? update : insert, conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Cliente {messs} correctamente!");
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

        private void Clear()
        {
            name.Clear();
            eaddress.Clear();
            phonenum.Clear();
            address.Clear();
            birthDate.Clear();
            type.SelectedIndex = 0;
            docNum.Clear();
            editing = false;
            editingID = 0;

        }

        private void label7_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            name.Text = dataGridView1.SelectedCells[1].Value.ToString();
            eaddress.Text = dataGridView1.SelectedCells[2].Value.ToString();
            phonenum.Text = dataGridView1.SelectedCells[3].Value.ToString();
            address.Text = dataGridView1.SelectedCells[4].Value.ToString();
            birthDate.Text = dataGridView1.SelectedCells[5].Value.ToString();
            type.Text = dataGridView1.SelectedCells[6].Value.ToString();
            docNum.Text = dataGridView1.SelectedCells[7].Value.ToString();
            editing = true;
            editingID = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());
        }
    }
}
