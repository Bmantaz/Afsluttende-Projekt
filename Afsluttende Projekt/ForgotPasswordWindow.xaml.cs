using System.Collections.Generic; // Giver adgang til List<T>, som bruges til at håndtere listen af brugere
using System.Windows; // Giver adgang til WPF-funktionalitet, som vinduer og kontroller
using System.Text.RegularExpressions; // Bruges til at validere adgangskodekrav med regulære udtryk

namespace Afsluttende_Projekt
{
    public partial class ForgotPasswordWindow : Window
    {
        private List<Bruger> brugere; // Liste til at holde alle brugere hentet fra data (f.eks. Excel)
        private Bruger matchendeBruger = null; // Holder den bruger, der matcher det indtastede medarbejder-ID

        public ForgotPasswordWindow()
        {
            InitializeComponent(); // Initialiserer komponenter defineret i XAML-filen
            DataAccess da = new DataAccess(); // Opretter en instans af DataAccess til at hente brugere
            brugere = da.HentBrugere(); // Henter listen over brugere fra datakilden
        }

        // Trin 1: Bruger indtaster medarbejder-ID for at identificere sig selv
        private void Step1_OK_Click(object sender, RoutedEventArgs e)
        {
            string medarbejderID = txtMedarbejderID.Text.Trim(); // Læser medarbejder-ID fra inputfeltet og fjerner mellemrum
            if (string.IsNullOrEmpty(medarbejderID)) // Kontrollerer, om medarbejder-ID'et er tomt
            {
                // Viser en fejlbesked, hvis feltet er tomt
                MessageBox.Show("Indtast et medarbejder ID.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Stopper yderligere eksekvering af metoden
            }

            matchendeBruger = null; // Nulstiller tidligere match
            foreach (var b in brugere) // Gennemgår listen af brugere for at finde en match
            {
                if (b.MedarbejderID == medarbejderID) // Sammenligner indtastet ID med brugernes ID
                {
                    matchendeBruger = b; // Finder en matchende bruger
                    break; // Stopper søgningen, da brugeren er fundet
                }
            }

            if (matchendeBruger == null) // Hvis ingen match findes
            {
                // Informerer brugeren om, at ID'et ikke blev fundet
                MessageBox.Show("Medarbejder ID ikke fundet.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Stopper yderligere eksekvering
            }

            // Skift til trin 2, hvor brugeren skal bekræfte nulstillingen
            GridStep1.Visibility = Visibility.Collapsed; // Skjuler trin 1
            txtBekræftelse.Text = $"Hej {matchendeBruger.Brugernavn} (ID: {matchendeBruger.MedarbejderID}).\nTryk 'Ja' for at nulstille dit kodeord. Tryk 'Nej' for at annullere."; // Viser besked med brugerens detaljer
            GridStep2.Visibility = Visibility.Visible; // Gør trin 2 synligt
        }

        // Trin 2: Brugeren bekræfter, om de vil nulstille adgangskoden
        private void Step2_Yes_Click(object sender, RoutedEventArgs e)
        {
            // Skifter til trin 3 for at angive ny adgangskode
            GridStep2.Visibility = Visibility.Collapsed; // Skjuler trin 2
            GridStep3.Visibility = Visibility.Visible; // Gør trin 3 synligt
        }

        private void Step2_No_Click(object sender, RoutedEventArgs e)
        {
            // Lukker vinduet, hvis brugeren vælger at annullere
            this.Close();
        }

        /* 
          Trin 3: Brugeren indtaster en ny adgangskode og gemmer den
          Opdaterer adgangskoden i datakilden, hvis den er gyldig.
          Informerer brugeren og lukker vinduet.
        */

        private void Step3_Gem_Click(object sender, RoutedEventArgs e)
        {
            string nyKode = txtNyAdgangskode.Password; // Læser den nye adgangskode fra inputfeltet

            if (ErAdgangskodeGyldig(nyKode)) // Kontrollerer, om adgangskoden opfylder kravene
            {
                DataAccess da = new DataAccess(); // Opretter en instans af DataAccess til at opdatere adgangskoden
                da.OpdaterKodeord(matchendeBruger.MedarbejderID, nyKode); // Opdaterer adgangskoden for den matchende bruger

                // Informerer brugeren om, at nulstillingen var succesfuld
                MessageBox.Show($"Her er dit nye kodeord: {nyKode}", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Lukker vinduet
            }
            else // Hvis adgangskoden ikke opfylder kravene
            {
                // Viser en fejlbesked med adgangskodekravene
                MessageBox.Show("Adgangskoden opfylder ikke kravene. Den skal indeholde mindst 7 tegn, herunder store og små bogstaver, mindst ét tal og ét specialtegn. Prøv venligst igen.",
                                "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Metode til at validere, om adgangskoden opfylder kravene
        private bool ErAdgangskodeGyldig(string kode)
        {
            // Adgangskodekrav:
            // - Mindst 7 tegn
            // - Mindst ét specialtegn
            // - Mindst ét tal
            // - Mindst én stor og én lille bogstav

            if (kode.Length < 7) // Kontrollerer længden
                return false;

            // Kontrollerer, om adgangskoden indeholder mindst ét tal
            bool harTal = Regex.IsMatch(kode, @"\d");

            // Kontrollerer, om adgangskoden indeholder mindst ét specialtegn
            bool harSpecial = Regex.IsMatch(kode, @"[\W_]");

            // Kontrollerer, om adgangskoden indeholder mindst ét stort bogstav
            bool harStore = Regex.IsMatch(kode, @"[A-Z]");

            // Kontrollerer, om adgangskoden indeholder mindst ét lille bogstav
            bool harSmå = Regex.IsMatch(kode, @"[a-z]");

            // Returnerer true, hvis alle krav er opfyldt
            return harTal && harSpecial && harStore && harSmå;
        }
    }
}


/*
 Forventet funktionalitet
 Brugeren indtaster deres medarbejder-ID.
 Hvis medarbejder-ID'et findes:
 Brugeren bekræfter nulstillingen.
 Indtaster en ny adgangskode.
 Den nye adgangskode valideres, opdateres i systemet, og processen afsluttes.
 */
