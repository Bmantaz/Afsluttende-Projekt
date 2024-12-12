using System;
using System.Collections.Generic;
using System.Windows;
using static Afsluttende_Projekt.DataAccess;


namespace Afsluttende_Projekt
{
    public partial class LoginWindow : Window
    {
        private List<Bruger> brugere;        // Liste til at gemme alle brugere hentet fra Excel
        private int loginAttempts = 0;       // Tæller antal loginforsøg
        private const int MaxLoginAttempts = 3; // Maksimalt antal tilladte loginforsøg

        public LoginWindow()
        {
            InitializeComponent();           // Initialiserer UI-komponenter defineret i XAML
            DataAccess dataAccess = new DataAccess(); // Opretter DataAccess-objekt for at hente brugerdata
            brugere = dataAccess.HentBrugere();       // Henter alle brugere fra Excel-filen
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string brugernavn = txtBrugernavn.Text.Trim();  // Læser og trim'er brugernavn fra tekstboks
            string adgangskode = txtAdgangskode.Password.Trim(); // Læser adgangskode fra password-feltet

            // Tjekker om brugernavn og adgangskode er udfyldt
            if (string.IsNullOrEmpty(brugernavn) || string.IsNullOrEmpty(adgangskode))
            {
                MessageBox.Show("Angiv både brugernavn og adgangskode.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Afbryder metoden, indtil brugeren angiver gyldige værdier
            }

            // Validerer brugeren med de indtastede legitimationsoplysninger
            Bruger gyldigBruger = ValiderBruger(brugernavn, adgangskode);

            // Hvis der blev fundet en gyldig bruger
            if (gyldigBruger != null)
            {
                MessageBox.Show("Login succesfuldt!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                // Opretter hovedmenuvinduet med den gyldige bruger
                MainMenuWindow mainMenu = new MainMenuWindow(gyldigBruger);
                mainMenu.Show(); // Viser hovedmenuen

                Application.Current.MainWindow = mainMenu; // Sætter hovedmenuen som applikationens MainWindow
                this.Close(); // Lukker loginvinduet
            }
            else
            {
                // Hvis bruger ikke blev fundet, tælles loginforsøg op
                loginAttempts++;
                // Hvis antal forsøg overstiger det maksimale
                if (loginAttempts >= MaxLoginAttempts)
                {
                    MessageBox.Show($"For mange mislykkede loginforsøg.\nDu har brugt alle {MaxLoginAttempts} forsøg.\nKontakt en administrator.",
                                    "Adgang nægtet", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close(); // Lukker loginvinduet da alle forsøg er opbrugt
                }
                else
                {
                    // Beregner hvor mange forsøg der er tilbage
                    int forsøgTilbage = MaxLoginAttempts - loginAttempts;
                    MessageBox.Show($"Forkert brugernavn eller adgangskode.\nDu har i alt {MaxLoginAttempts} forsøg.\nDu har nu {forsøgTilbage} forsøg tilbage.",
                                    "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private Bruger ValiderBruger(string brugernavn, string adgangskode)
        {
            // Gennemløber listen af brugere og sammenligner med indtastede legitimationsoplysninger
            foreach (Bruger bruger in brugere)
            {
                if (bruger.Brugernavn == brugernavn && bruger.Adgangskode == adgangskode)
                {
                    return bruger; // Returnerer brugeren, hvis match fundet
                }
            }
            return null; // Returnerer null, hvis ingen matchende bruger fundet
        }

        private void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            // Åbner vinduet til nulstilling af adgangskode
            ForgotPasswordWindow fpw = new ForgotPasswordWindow();
            fpw.ShowDialog(); // Viser vinduet som en dialog, der blokerer indtil det lukkes
        }



    }

}
