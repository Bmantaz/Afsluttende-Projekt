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

namespace Afsluttende_Projekt
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string brugernavn = txtBrugernavn.Text;
            string adgangskode = txtAdgangskode.Password;

            // Valider brugeren (metode implementeres senere)
            bool erGyldig = ValiderBruger(brugernavn, adgangskode);

            if (erGyldig)
            {
                MessageBox.Show("Login succesfuldt!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                // Åbn hovedmenuen (skal oprettes senere)
                //PLACEHOLDER!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //MainMenuWindow mainMenu = new MainMenuWindow();
                //mainMenu.Show();
                //this.Close();
            }
            else
            {
                MessageBox.Show("Forkert brugernavn eller adgangskode.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValiderBruger(string brugernavn, string adgangskode)
        {
            // Midlertidig returnering - implementer faktisk validering senere
            return false;
        }
    }
}
