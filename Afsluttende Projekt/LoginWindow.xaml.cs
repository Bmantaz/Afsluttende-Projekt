using System.Collections.Generic; // Giver adgang til List<T>, som bruges til at håndtere en liste af brugere
using System.Windows; // Giver adgang til WPF-funktionalitet, som vinduer og knapper

namespace Afsluttende_Projekt
{
    public partial class LoginWindow : Window
    {
        private List<Bruger> brugere; // Liste til at holde alle brugere hentet fra data (f.eks. Excel)
        private int loginAttempts = 0; // Tæller antallet af mislykkede loginforsøg
        private const int MaxLoginAttempts = 3; // Maksimalt antal loginforsøg før login låses

        public LoginWindow()
        {
            InitializeComponent(); // Initialiserer komponenter defineret i XAML-filen
            DataAccess dataAccess = new DataAccess(); // Opretter en instans af DataAccess til at håndtere brugerdata
            brugere = dataAccess.HentBrugere(); // Henter listen over brugere fra datakilden (Excel eller database)
        }

        /* 
        Event-handler for login-knappen
        Denne metode håndterer loginprocessen:
        Validerer inputfelter for brugernavn og adgangskode.
        Matcher brugeren med data fra brugere.
        Hvis valid, åbner hovedmenuen; ellers øger den antallet af loginforsøg og informerer brugeren om resterende forsøg.
        */
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string brugernavn = txtBrugernavn.Text.Trim(); // Læser brugernavn fra inputfeltet og fjerner mellemrum
            string adgangskode = txtAdgangskode.Password.Trim(); // Læser adgangskode fra passwordfeltet og fjerner mellemrum

            // Tjekker, om brugernavn eller adgangskode er tomme
            if (string.IsNullOrEmpty(brugernavn) || string.IsNullOrEmpty(adgangskode))
            {
                // Viser en fejlbesked, hvis felterne ikke er udfyldt
                MessageBox.Show("Angiv både brugernavn og adgangskode.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Stopper yderligere eksekvering af metoden
            }

            // Validerer legitimationsoplysningerne mod listen over brugere
            Bruger gyldigBruger = ValiderBruger(brugernavn, adgangskode);

            // Hvis en gyldig bruger findes
            if (gyldigBruger != null)
            {
                // Opretter og åbner hovedmenuen med den validerede bruger
                MainMenuWindow mainMenu = new MainMenuWindow(gyldigBruger);
                mainMenu.Show(); // Viser hovedmenuen
                Application.Current.MainWindow = mainMenu; // Opdaterer hovedvinduet i applikationen til hovedmenuen
                this.Close(); // Lukker loginvinduet
            }
            else // Hvis brugerens legitimationsoplysninger er forkerte
            {
                loginAttempts++; // Øger antallet af loginforsøg

                // Hvis loginforsøgene overstiger det maksimalt tilladte antal
                if (loginAttempts >= MaxLoginAttempts)
                {
                    // Viser en fejlbesked og lukker loginvinduet
                    MessageBox.Show($"For mange mislykkede loginforsøg.\nKontakt en administrator.", "Adgang nægtet", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close(); // Lukker loginvinduet
                }
                else
                {
                    // Beregner antallet af resterende forsøg
                    int forsøgTilbage = MaxLoginAttempts - loginAttempts;
                    // Informerer brugeren om antallet af resterende forsøg
                    MessageBox.Show($"Forkert brugernavn eller adgangskode.\nDu har nu {forsøgTilbage} forsøg tilbage.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /*
         Metode til at validere brugerens legitimationsoplysninger
         Sammenligner brugernavn og adgangskode med de hentede brugerdata.
         Returnerer en Bruger-instans, hvis et match findes, ellers null.
        */
        private Bruger ValiderBruger(string brugernavn, string adgangskode)
        {
            // Itererer gennem listen over brugere for at finde et match
            foreach (Bruger bruger in brugere)
            {
                // Tjekker, om brugernavn og adgangskode matcher en bruger
                if (bruger.Brugernavn == brugernavn && bruger.Adgangskode == adgangskode)
                {
                    return bruger; // Returnerer den matchende bruger
                }
            }
            return null; // Returnerer null, hvis ingen matchende bruger findes
        }

        /*
        Event-handler for glemt adgangskode-knappen
        Åbner et separat vindue til nulstilling af adgangskode.
        Tillader brugeren at nulstille deres adgangskode uden at forlade loginvinduet.
        */
        private void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            // Opretter og viser vinduet til nulstilling af adgangskode
            ForgotPasswordWindow fpw = new ForgotPasswordWindow();
            fpw.ShowDialog(); // Åbner vinduet som en modal dialog
        }
    }

}


/*
  Forventet funktionalitet
Brugeren indtaster deres legitimationsoplysninger og klikker på "Login".
Hvis oplysningerne er korrekte:
Loginvinduet lukkes.
Hovedmenuen åbnes med den validerede bruger.
Hvis oplysningerne er forkerte:
Brugeren får en besked om fejl.
Antallet af loginforsøg øges.
Efter 3 mislykkede forsøg lukkes loginvinduet med en besked om at kontakte administratoren.
Brugeren kan nulstille deres adgangskode via knappen "Glemt adgangskode".
 
 */