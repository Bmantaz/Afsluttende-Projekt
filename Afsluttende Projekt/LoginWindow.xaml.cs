using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data;
using System.Data.OleDb;

namespace Afsluttende_Projekt
{
    public partial class LoginWindow : Window
    {
        private List<Bruger> brugere;

        public LoginWindow()
        {
            InitializeComponent();
            // Hent brugere fra Excel-filen
            DataAccess dataAccess = new DataAccess();
            brugere = dataAccess.HentBrugere();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string brugernavn = txtBrugernavn.Text;
            string adgangskode = txtAdgangskode.Password;

            // Valider brugeren
            Bruger gyldigBruger = ValiderBruger(brugernavn, adgangskode);

            if (gyldigBruger != null)
            {
                MessageBox.Show("Login succesfuldt!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                // Åbn hovedmenuen og overfør brugeroplysninger
                MainMenuWindow mainMenu = new MainMenuWindow(gyldigBruger);
                mainMenu.Show();

                // Opdater applikationens MainWindow
                Application.Current.MainWindow = mainMenu;

                // Luk LoginWindow
                this.Close();
            }
            else
            {
                MessageBox.Show("Forkert brugernavn eller adgangskode.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Bruger ValiderBruger(string brugernavn, string adgangskode)
        {
            // Søg efter brugeren i listen
            foreach (Bruger bruger in brugere)
            {
                if (bruger.Brugernavn == brugernavn && bruger.Adgangskode == adgangskode)
                {
                    return bruger;
                }
            }
            return null;
        }
    }
}
