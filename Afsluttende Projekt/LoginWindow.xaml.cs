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
        private int loginAttempts = 0;
        private const int MaxLoginAttempts = 3;

        public LoginWindow()
        {
            InitializeComponent();
            DataAccess dataAccess = new DataAccess();
            brugere = dataAccess.HentBrugere();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string brugernavn = txtBrugernavn.Text.Trim();
            string adgangskode = txtAdgangskode.Password.Trim();

            // Tjek om felterne er udfyldt
            if (string.IsNullOrEmpty(brugernavn) || string.IsNullOrEmpty(adgangskode))
            {
                MessageBox.Show("Angiv både brugernavn og adgangskode.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Valider brugeren
            Bruger gyldigBruger = ValiderBruger(brugernavn, adgangskode);

            if (gyldigBruger != null)
            {
                MessageBox.Show("Login succesfuldt!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                MainMenuWindow mainMenu = new MainMenuWindow(gyldigBruger);
                mainMenu.Show();

                Application.Current.MainWindow = mainMenu;
                this.Close();
            }
            else
            {
                loginAttempts++;
                if (loginAttempts >= MaxLoginAttempts)
                {
                    MessageBox.Show($"For mange mislykkede loginforsøg.\nDu har brugt alle {MaxLoginAttempts} forsøg.\nKontakt en administrator.",
                                    "Adgang nægtet", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
                else
                {
                    int forsøgTilbage = MaxLoginAttempts - loginAttempts;
                    MessageBox.Show($"Forkert brugernavn eller adgangskode.\nDu har i alt {MaxLoginAttempts} forsøg.\nDu har nu {forsøgTilbage} forsøg tilbage.",
                                    "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private Bruger ValiderBruger(string brugernavn, string adgangskode)
        {
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