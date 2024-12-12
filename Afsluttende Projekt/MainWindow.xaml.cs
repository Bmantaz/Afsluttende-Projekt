using System;
using System.Windows;


namespace Afsluttende_Projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
