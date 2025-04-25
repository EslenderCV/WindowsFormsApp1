using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MovimientoReserva: Form
    {

        string connectionString = "Data Source=DESKTOP-I9FPIBD;Initial Catalog=RentACar;Integrated Security=True";
        decimal precioOferta = 0;
        bool ofertando = false;

        public MovimientoReserva()
        {


            InitializeComponent();
            DateTime dateTime = DateTime.UtcNow.Date;
            time.Text = dateTime.ToString("dd/MM/yyyy");

            clientes.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            clientes.AutoCompleteSource = AutoCompleteSource.ListItems;

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

                            clientes.Items.Add($"{reader["id_cliente"]}.{reader["nombre"]}");

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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Vehiculos WHERE estado = 'Disponible'", conn))
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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int cliente = Convert.ToInt32(clientes.Text.Split('.')[0]);
            int veh = Convert.ToInt32(modelos.Text.Split('.')[0]);

            if (!(string.IsNullOrEmpty(clientes.Text) || string.IsNullOrEmpty(modelos.Text) || string.IsNullOrEmpty(start.Text) || string.IsNullOrEmpty(end.Text)))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        string insert = $"INSERT INTO Reservas VALUES({cliente}, {veh}, '{doc.Text}', '{time.Text}', '{start.Text}', '{end.Text}', '{status.Text}')";
                        
                        SqlCommand cmd = new SqlCommand(insert, conn);
                        cmd.ExecuteNonQuery();

                        string[] dd1 = start.Text.Split('-');

                        DateTime d1 = DateTime.ParseExact($"{dd1[0]}{dd1[1]}{dd1[2]}", "yyyyMMdd", CultureInfo.InvariantCulture);
                        string[] dd2 = end.Text.Split('-');
                        DateTime d2 = DateTime.ParseExact($"{dd2[0]}{dd2[1]}{dd2[2]}", "yyyyMMdd", CultureInfo.InvariantCulture);

                        double days = (d1 - d2).TotalDays;

                        decimal defPrice = ofertando ? precioOferta : (decimal)days * getPrecio(veh);

                        insertAlquiler(cliente, veh, start.Text, end.Text, defPrice, "Activo");

                        MessageBox.Show($"Reserva ingresada correctamente!");
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
                setState(veh);
            }

        }

        private void insertAlquiler(int cliente, int vehiculo, string inicio, string fin, decimal total, string stado)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string insert = $"INSERT INTO Alquileres VALUES({cliente}, {vehiculo}, '{inicio}', '{fin}', {total}, '{stado}')";

                    SqlCommand cmd = new SqlCommand(insert, conn);
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
            clientes.SelectedIndex = 0;
            modelos.SelectedIndex = 0;
            doc.Text = "-";
            time.Text = "-";
            start.Clear();
            end.Clear();
            precioOferta = 0;
            ofertando = false;
        }

        private void clientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(clientes.Text))
            {
                doc.Text = getDocumento(Convert.ToInt32(clientes.Text.Split('.')[0]));
            }
        }

        private string getDocumento(int ID)
        {
            string toReturn = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand($"SELECT * FROM Clientes WHERE id_cliente = {ID}", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            toReturn = reader["num_documento"].ToString();
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

        private void modelos_SelectedIndexChanged(object sender, EventArgs e)
        {

            ofertas.Items.Clear();

            if (string.IsNullOrEmpty(modelos.Text))
            {
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand($"SELECT * FROM Ofertas WHERE limite_oferta > GETDATE() AND id_vehiculo = {Convert.ToInt32(modelos.Text.Split('.')[0])}", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            ofertas.Items.Add($"{reader["descripcion_oferta"]}");
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

        private void setState(int ID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string update = $"UPDATE Vehiculos SET estado = 'Reservado' WHERE id_vehiculo = {ID}";

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

        private decimal getPrecio(int ID)
        {
            decimal toReturn = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand($"SELECT * FROM Ofertas WHERE id_vehiculo = {ID}", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            toReturn = Convert.ToDecimal(reader["precio_oferta"].ToString());
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

        private void ofertas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ofertas.Text))
            {
                ofertando = false;
                precioOferta = 0;
            }

            if (string.IsNullOrEmpty(start.Text) || string.IsNullOrEmpty(end.Text))
            {
                MessageBox.Show("Ingrese fecha entrada y fecha salida antes de elegir oferta");
                return;
            }

            decimal precio = Convert.ToDecimal(getPrecio(Convert.ToInt32(modelos.Text.Split('.')[0])));

            string[] dd1 = start.Text.Split('-');
            DateTime d1 = DateTime.ParseExact($"20250505", "yyyyMMdd", CultureInfo.InvariantCulture);
            string[] dd2 = end.Text.Split('-');
            DateTime d2 = DateTime.ParseExact($"{dd2[0]}{dd2[1]}{dd2[2]}", "yyyyMMdd", CultureInfo.InvariantCulture); 

            double days = (d1 - d2).TotalDays;
            decimal newPrice = (decimal)days * precio;
            ofertando = true;
            precioOferta = newPrice;
            MessageBox.Show($"Precio ofertado es: ${newPrice}");
        }
    }
}
