using System.Windows; // Giver adgang til WPF-funktionalitet som vinduer og kontroller

namespace Afsluttende_Projekt
{
    public partial class CEOLoginWindow : Window
    {
        // funktion der bestemmer, om adgang er givet
        public bool AccessGranted { get; private set; } = false;

        // Korrekt adgangskode (kan ændres efter behov uden at ændre logikken)
        private const string CorrectCode = "1234";

        public CEOLoginWindow()
        {
            InitializeComponent(); // Initialiserer UI-komponenter defineret i XAML-filen
        }

        // Event-handler for OK-knappen
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            // Tjekker, om indtastet adgangskode matcher den korrekte kode
            if (txtCEOCode.Password.Trim() == CorrectCode) // Password.Trim() fjerner unødvendige mellemrum
            {
                AccessGranted = true; // Sætter adgang som givet
                this.Close(); // Lukker vinduet
            }
            else
            {
                // Viser en fejlbesked, hvis adgangskoden er forkert
                MessageBox.Show("Forkert kode. Prøv igen.", "Adgang nægtet", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event-handler for Annuller-knappen
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Lukker vinduet uden at ændre adgangstilladelsen
        }
    }
}

/*
 Forventet funktionalitet
 Når vinduet åbnes, kan brugeren indtaste adgangskoden.
 Hvis adgangskoden er korrekt:
 Vinduet lukkes.
 Ejendommen AccessGranted sættes til true, hvilket tillader adgang til CEO-funktionalitet.
 Hvis adgangskoden er forkert:
 Brugeren får en fejlbesked og kan prøve igen.
 Hvis brugeren klikker på "Annuller":
 Vinduet lukkes uden at give adgang.
 */
