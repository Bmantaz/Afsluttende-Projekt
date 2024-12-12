using System;
using System.Windows;

namespace Afsluttende_Projekt
{
    public partial class MainMenuWindow : Window
    {
        private Bruger aktueltBruger;

        public MainMenuWindow(Bruger bruger)
        {
            InitializeComponent();
            aktueltBruger = bruger;

            // Sæt velkomstbesked
            txtVelkommen.Text = $"Velkommen, {bruger.Brugernavn} (ID: {bruger.MedarbejderID})";
        }

        private void btnCEOAccess_Click(object sender, RoutedEventArgs e)
        {
            // Åbn CEO-login vinduet
            CEOLoginWindow ceoLogin = new CEOLoginWindow();
            ceoLogin.ShowDialog();

            // Tjek om adgang blev givet
            if (ceoLogin.AccessGranted)
            {
                // Debug: Synlighed opdateres korrekt
                MessageBox.Show("Adgang givet. Gør CEO-sektionen synlig.", "Debug");

                spCEOData.Visibility = Visibility.Visible; // Gør CEO-sektionen synlig
                txtCEORestricted.Visibility = Visibility.Collapsed; // Skjul restriktionsbeskeden
                btnKPI.Visibility = Visibility.Visible; // Gør KPI-knappen synlig
            }
            else
            {
                // Debug: Adgang nægtet
                MessageBox.Show("Adgang nægtet. CEO-sektionen forbliver skjult.", "Debug");
            }
        }

        private void btnKPI_Click(object sender, RoutedEventArgs e)
        {
            // Debug: Tjek om KPI-knappen fungerer
            MessageBox.Show("KPI-knappen blev trykket. Åbner KPI-vinduet.", "Debug");

            try
            {
                // Åbn KPI-vinduet
                KPIWindow kpiWindow = new KPIWindow();
                kpiWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                // Debug: Hvis der opstår fejl, vis fejlbeskeden
                MessageBox.Show($"Der opstod en fejl under åbning af KPI-vinduet:\n{ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Banko_Click(object sender, RoutedEventArgs e)
        {
            // Debug: Banko-knappen fungerer
            MessageBox.Show("Banko-knappen blev trykket.", "Debug");

            // Åbn Banko-vinduet
            BankoWindow bankoWindow = new BankoWindow();
            bankoWindow.ShowDialog();
        }

        private void LogUd_Click(object sender, RoutedEventArgs e)
        {
            // Debug: Log Ud-knappen fungerer
            MessageBox.Show("Log Ud-knappen blev trykket. Logger ud.", "Debug");

            // Luk dette vindue
            this.Hide();

            // Opret en ny instans af MainWindow
            MainWindow newMainWindow = new MainWindow();
            newMainWindow.Show();
        }

        private void MainMenuWindow_Closed(object sender, EventArgs e)
        {
            // Debug: Hovedmenuen blev lukket
            MessageBox.Show("Hovedmenuen blev lukket. Applikationen lukkes.", "Debug");

            // Luk applikationen
            Application.Current.Shutdown();
        }
    }
}