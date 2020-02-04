using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LigaManager
{
    /// <summary>
    /// Lógica de interacción para JugadorDialog.xaml
    /// </summary>
    public partial class JugadorDialog : Window
    {
        public JugadorDialog()
        {
            InitializeComponent();

            _isUpdate = false;
        }

        public JugadorDialog(string id, string nombre, string apellido, string posicion,
            string fecha, string salario, string equipo, string altura)
        {
            InitializeComponent();

            _id = id;

            tbNombre.Text = nombre;
            tbApellido.Text = apellido;
            tbPosicion.Text = posicion;
            datePicker.Text = fecha;
            tbSalario.Text = salario;
            comboBox.Text = equipo;
          

            tbAltura.Text = altura;

            _isUpdate = true;
        }

        private bool _isUpdate;
        public bool isUpdate
        {
            get => _isUpdate;
            set => _isUpdate = value;
        }

        private string _id;
        public string Id
        {
            get => _id;
            set => _id = value;
        }

        private void onClickBtnAceptar(object sender, RoutedEventArgs e)
        {
            if (!_isUpdate)
            {
                try
                {
                    string query = "INSERT INTO jugador(" +
                        "NOMBRE, " +
                        "APELLIDO, " +
                        "POSICION, " +
                        "FECHA_ALTA, " +
                        "SALARIO, " +
                        "EQUIPO, " +
                        "ALTURA" +
                        ") VALUES ( " +
                        "'" + tbNombre.Text + "', " +
                        "'" + tbApellido.Text + "', " +
                        "'" + tbPosicion.Text + "', " +
                        "'" + datePicker.Text + "', " +
                        tbSalario.Text + ", " +
                        (comboBox.SelectedIndex + 1) + ", " +
                        tbAltura.Text + ");";

                    MySqlConnection connection =
                        new MySqlConnection(@"server=localhost;userid=root;password=interfaces;database=liga");

                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader;
                    connection.Open();
                    reader = command.ExecuteReader();
                    MessageBox.Show("Se ha insertado el jugador correctamente.");
                    while (reader.Read())
                    {
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Algo salió mal, compruebe que ha introducido los datos correctamente.");
                }
            }

            if (_isUpdate)
            {
                try
                {
                    string query = "UPDATE jugador SET " +
                        "NOMBRE = '" + tbNombre.Text + "', " +
                        "APELLIDO = '" + tbApellido.Text + "', " +
                        "POSICION = '" + tbPosicion.Text + "', " +
                        "FECHA_ALTA = '" + datePicker.Text + "', " +
                        "SALARIO = " + tbSalario.Text + ", " +
                        "EQUIPO = " + (comboBox.SelectedIndex + 1) + ", " +
                        /* Por alguna razón se me cambian los puntos por comas (supongo que es por el Material Desing), así que he 
                         tenido que reemplazarlo para que no diera error al lanzar la SQL.*/
                        "ALTURA = " + (tbAltura.Text).Replace(",", ".") + 
                        " WHERE jugador.ID = " + _id + "; ";

                    MySqlConnection connection =
                        new MySqlConnection(@"server=localhost;userid=root;password=interfaces;database=liga");

                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader;
                    connection.Open();
                    reader = command.ExecuteReader();
                    MessageBox.Show("Se han actualizado los datos del jugador correctamente.");
                    while (reader.Read())
                    {
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            /**
             * Utilizo un botón oculto en MainWindow para poder 
             * actualizar el Data Grid al agregar o actualizar un jugador, lanzo su
             * evento onClick que consiste en refrescarlo.
             * */

            MainWindow.button.RaiseEvent(
                new RoutedEventArgs(Button.ClickEvent)
                );

            Close();

        }

        private void onClickBtnCancelar(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
