using System; // Giver adgang til grundlæggende funktionalitet som exceptions og eventhåndtering
using System.Windows; // Giver adgang til WPF-funktionalitet som vinduer og kontroller
using System.Windows.Media.Imaging; // Bruges til at indlæse og vise billeder

namespace Afsluttende_Projekt
{
    public partial class MainMenuWindow : Window
    {
        private Bruger aktueltBruger; // Holder referencen til den bruger, der er logget ind

        public MainMenuWindow(Bruger bruger)
        {
            InitializeComponent(); // Initialiserer komponenter defineret i XAML-filen
            aktueltBruger = bruger; // Gemmer den aktuelle bruger

            // Sætter en velkomstbesked med brugerens navn og ID
            txtVelkommen.Text = $"Velkommen, {bruger.Brugernavn} (ID: {bruger.MedarbejderID})";

            // Indlæser profilbilledet for den aktuelle bruger
            IndlæsProfilbillede();
        }

        // Event-handler for CEO-adgang der åbner et loginvindue for CEO-adgang.
        private void btnCEOAccess_Click(object sender, RoutedEventArgs e)
        {
            // Åbner CEO-login vinduet
            CEOLoginWindow ceoLogin = new CEOLoginWindow();
            ceoLogin.ShowDialog(); // Viser loginvinduet som en modal dialog

            // Hvis adgangskoden er korrekt, bliver CEO-sektionen gjort synlig
            if (ceoLogin.AccessGranted)
            {
                spCEOData.Visibility = Visibility.Visible; // Gør sektionen med CEO-data synlig
                txtCEORestricted.Visibility = Visibility.Collapsed; // Skjuler restriktionsbeskeden
                btnKPI.Visibility = Visibility.Visible; // Gør KPI-knappen synlig
            }
        }

        // Event-handler for KPI-knappen, åbner KPI-vinduet for at vise KPI-data.
        private void btnKPI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Åbner KPI-vinduet
                KPIWindow kpiWindow = new KPIWindow();
                kpiWindow.ShowDialog(); // Viser KPI-vinduet som en modal dialog
            }
            catch (Exception ex)
            {
                // Viser en fejlbesked, hvis der opstår problemer, eksempelvis hvis filen mangler/er beskadiget/ikke kan åbnes
                MessageBox.Show($"Der opstod en fejl under åbning af KPI-vinduet:\n{ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event-handler for Banko-knappen der åbner et separat vindue til banko funktionen
        private void Banko_Click(object sender, RoutedEventArgs e)
        {
            // Åbner Banko-vinduet
            BankoWindow bankoWindow = new BankoWindow();
            bankoWindow.ShowDialog(); // Viser Banko-vinduet som en modal dialog
        }

        /* Event-handler for Log Ud-knappen
           Her vil vi gerne sendes tilbage til vores MainWindow, så der kan logges ind på ny.
        */
        private void LogUd_Click(object sender, RoutedEventArgs e)
        {
            this.Hide(); // Skjuler MainMenuWindow, så det ikke lukkes (se næste eventhandler hvorfor hide og ikke close)

            // Åbner et nyt loginvindue
            MainWindow newMainWindow = new MainWindow();
            newMainWindow.Show(); // Viser loginvinduet
        }

        // Event-handler, der aktiveres, når MainMenuWindow lukkes
        private void MainMenuWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown(); // Lukker hele applikationen
        }

        // Metode til at indlæse profilbilledet for den aktuelle bruger
        private void IndlæsProfilbillede()
        {
            string medarbejderID = aktueltBruger.MedarbejderID; // Henter medarbejder-ID'et fra den aktuelle bruger
            DataAccess da = new DataAccess(); // Opretter en instans af DataAccess til at hente data
            string billedeSti = da.HentProfilbilledeSti(medarbejderID); // Henter stien til profilbilledet baseret på ID'et

            // Hvis der findes et billede for brugeren
            if (!string.IsNullOrEmpty(billedeSti))
            {
                profilImage.Source = new BitmapImage(new Uri(billedeSti)); // Indlæser billedet og viser det i UI
            }
        }
    }
}


/*
  Forventet funktionalitet
  Når en bruger logger ind, vises deres navn, ID og profilbillede på hovedmenuen.
  CEO-specifikke elementer forbliver skjulte, indtil en korrekt adgangskode indtastes.
  Banko- og KPI-vinduer kan åbnes uden problemer.
  Brugeren kan logge ud, hvilket bringer dem tilbage til loginvinduet.
  Applikationen lukkes korrekt, når hovedmenuen lukkes.
 */

