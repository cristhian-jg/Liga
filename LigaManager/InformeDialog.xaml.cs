using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace LigaManager
{
    /// <summary>
    /// Lógica de interacción para FormDialog.xaml
    /// </summary>
    public partial class InformeDialog : Window
    {
        public InformeDialog()
        {
            InitializeComponent();
        }

        /**
         * Carga el informe en el Windows Forms Host una vez se selecciona
         * un equipo del ListBox. Tiene un fallo en el que una vez carga un informe
         * no vuelve a cargar otro hasta que se cierre la ventana y se vuelva a abrir,
         * he intentado solucionarlo pero sin resultados... mi idea era refrescar o
         * vaciar el WindowsFormsHost (o incluso la ventana WPF entera) pero no hayé metodo para hacerlo.
         * */
        private void listBoxChangedEvent(object sender, SelectionChangedEventArgs e)
        {

            informeDatos.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;
            Microsoft.Reporting.WinForms.ReportDataSource
             reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            ligaDataSet dsTodaLaLiga = new ligaDataSet();
            dsTodaLaLiga.BeginInit();
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = dsTodaLaLiga.jugador;
            informeDatos.LocalReport.DataSources.Add(reportDataSource1);
            informeDatos.LocalReport.ReportPath = "Jugadores.rdlc";
            dsTodaLaLiga.EndInit();
            try
            {
                String conexion =
                ConfigurationManager.ConnectionStrings["LigaManager.Properties.Settings.ligaConnectionString"].ConnectionString;
                MySql.Data.MySqlClient.MySqlDataAdapter JugadoreTableAdapter =
                new MySql.Data.MySqlClient.MySqlDataAdapter("Select * from jugador WHERE jugador.equipo = " + (listBox.SelectedIndex + 1), conexion);
                dsTodaLaLiga.Clear();
                JugadoreTableAdapter.Fill(dsTodaLaLiga.jugador);
                informeDatos.RefreshReport();

            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception);
            }
        }
    }
}
