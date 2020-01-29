using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LigaManager
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
    
        }

        private void onClickSalir(object sender, RoutedEventArgs e)
        {
            /** Cerrará la aplicación completamente */
            Application.Current.Shutdown();
        }

        void InitializeDataGrid()
        {
            MySqlConnection connection =
                new MySqlConnection(@"server=localhost;userid=dbuser;password=s$cret;database=testdb");
         
            connection.Open();

            MySqlCommand command = new MySqlCommand("SELECT * FROM jugador", connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable dataTable = new DataTable("Jugadores");
            adapter.Fill(dataTable);
        }
    }
}
