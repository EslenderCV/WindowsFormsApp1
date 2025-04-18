using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ConsultasModelos: Form
    {
        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";

        private HomeScreen screen;

        public ConsultasModelos(HomeScreen screenn)
        {
            InitializeComponent();

            screen = screenn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            screen.openForm(new Consultas(screen));
        }

        private void ConsultasModelos_Load(object sender, EventArgs e)
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

                    using (SqlCommand cmd = new SqlCommand($"SELECT * FROM Marcas WHERE id_marca = 1", conn))
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            screen.openForm(new Consultas(screen));
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
