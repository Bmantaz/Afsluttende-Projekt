using System.Windows;


namespace Afsluttende_Projekt
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); // Initialiserer komponenterne i MainWindow (læser XAML og opretter UI-elementer)
        }

        private void btnMedarbejderLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Hide(); // Skjuler hovedvinduet (MainWindow), så det ikke længere er synligt

            LoginWindow loginWindow = new LoginWindow(); // Opretter en ny instans af LoginWindow
            loginWindow.Show(); // Viser loginvinduet ovenpå, så brugeren kan logge ind
        }
    }
    
}
/*
 Forventet Funktionalitet
Ved opstart initialiseres MainWindow og dets UI-elementer fra XAML-filen.

Når knappen "Medarbejder Login" trykkes, skjules hovedvinduet.
Et nyt loginvindue (LoginWindow) åbnes for at håndtere brugerens login.

Hovedvinduet fungerer som en startskærm, og brugeren navigerer til login for videre adgang.
 */

