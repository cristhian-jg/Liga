using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace LigaManager
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static Button button;

        public MainWindow()
        {
            InitializeComponent();
            refreshDataGrid();
            button = btnActualizar;
        }

        private void onClickSalir(object sender, RoutedEventArgs e)
        {
            /** Cerrará la aplicación completamente */
            Application.Current.Shutdown();
        }


        /**
         *  Consulta los datos de la tabla de la base de datos 
         *  y las muestra/almacena en un DataGrid
         * */
        public void refreshDataGrid()
        {
            string query = "SELECT jugador.ID," +
                " jugador.NOMBRE," +
                " jugador.APELLIDO," +
                " jugador.POSICION," +
                " jugador.FECHA_ALTA," +
                " jugador.SALARIO, " +
                " jugador.ALTURA," +
                " equipo.NOMBRE AS EQUIPO " +
                "FROM jugador, equipo WHERE jugador.EQUIPO = equipo.ID";

            MySqlConnection connection =
                new MySqlConnection(@"server=localhost;userid=root;password=interfaces;database=liga");

            connection.Open();

            dataGridJugadores.ItemsSource = null;

            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable dataTable = new DataTable("jugador");

            adapter.Fill(dataTable);
            dataGridJugadores.ItemsSource = dataTable.DefaultView;

            connection.Close();
        }

        /** Metodo que abre una nueva ventana que servirá para 
         *  insertar jugadores en la base de datos 
         */
        private void onClickBtnNuevo(object sender, RoutedEventArgs e)
        {
            JugadorDialog dialog = new JugadorDialog();
            dialog.Title = "Añadir jugador";
            dialog.ShowDialog();

        }

        /**
         * Reutilizo el dialogo para crear un jugador, pero esta vez le paso los valores del registro 
         * para luego mostrarlos en los campos correspondiente, además pondrá una variable booleana (_isUpdate) a true
         * debido al constructor utilizado, por lo que actualizará en lugar de insertar.
         * */
        private void onClickBtnModificar(object sender, RoutedEventArgs e)
        {
            if (dataGridJugadores.SelectedItem != null)
            {
                DataRowView dataRow = (DataRowView)dataGridJugadores.SelectedItem;
                //int index = dataGridJugadores.CurrentCell.Column.DisplayIndex;
                string id = dataRow.Row.ItemArray[0].ToString();
                string nombre = dataRow.Row.ItemArray[1].ToString();
                string apellido = dataRow.Row.ItemArray[2].ToString();
                string posicion = dataRow.Row.ItemArray[3].ToString();
                string fecha = dataRow.Row.ItemArray[4].ToString();
                string salario = dataRow.Row.ItemArray[5].ToString();
                string altura = dataRow.Row.ItemArray[6].ToString();
                string equipo = dataRow.Row.ItemArray[7].ToString();

                JugadorDialog dialog = new JugadorDialog(id, nombre, apellido, posicion, fecha, salario, equipo, altura);

                dialog.Title = "Actualizar jugador";

                dialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("Selecciona a un jugador para modificarlo.", "Operación no permitida",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void onClickBtnEliminar(object sender, RoutedEventArgs e)
        {

            if (dataGridJugadores.SelectedItem != null)
            {

                string messageBoxText = "Estas seguro que deseas eliminar este jugador?";
                string caption = "Eliminar jugador";
                MessageBoxButton boton = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;

                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, boton, icon);

                switch (result)
                {
                    case MessageBoxResult.Yes:

                        try
                        {

                            /**
                             *  Con el siguiente código el registro seleccionado para 
                             *  posteriormete obtener su id, es decir su primera columna. 
                             * */

                            DataRowView dataRow = (DataRowView)dataGridJugadores.SelectedItem;
                            //int index = dataGridJugadores.CurrentCell.Column.DisplayIndex;
                            string cellValue = dataRow.Row.ItemArray[0].ToString();
                            string query = "DELETE FROM jugador WHERE jugador.ID = " + cellValue;

                            MySqlConnection connection =
                                new MySqlConnection(@"server=localhost;userid=root;password=interfaces;database=liga");

                            MySqlCommand command = new MySqlCommand(query, connection);

                            MySqlDataReader reader;
                            connection.Open();

                            reader = command.ExecuteReader();
                            MessageBox.Show("Jugador con ID " + cellValue + " eliminado correctamente.");
                            while (reader.Read())
                            {
                            }
                            connection.Close();

                            /**
                            * Utilizo un botón oculto en MainWindow para poder 
                            * actualizar el Data Grid al agregar un nuevo jugador y lanzo su
                            * evento onClick que consiste en refrescarlo.
                            * */
                            button.RaiseEvent(
                                new RoutedEventArgs(Button.ClickEvent)
                                );
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Cancel:
                        MessageBox.Show("Se ha cancelado la operación.");
                        break;
                }
            }
            else
            {
                MessageBox.Show("Selecciona a un jugador para eliminarlo.", "Operación no permitida",
    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /**
         *  Botón oculto que accionaré al realizar cambios en el Data Grid 
         *  para que realice los cambios.
         * */
        private void onClickBtnActualizar(object sender, RoutedEventArgs e)
        {
            refreshDataGrid();
        }

        private void onClickBtnInforme(object sender, RoutedEventArgs e)
        {
            InformeDialog dialog = new InformeDialog();
            dialog.ShowDialog();
        }
    }
}
