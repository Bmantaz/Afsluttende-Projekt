using System;
using System.Windows;


namespace Afsluttende_Projekt
{
    public partial class MainMenuWindow : Window
    {
        private Bruger aktueltBruger;

        public MainMenuWindow(Bruger bruger)
        {
            InitializeComponent();
            aktueltBruger = bruger;
            txtVelkommen.Text = $"Velkommen, {bruger.Brugernavn} (ID: {bruger.MedarbejderID})";
        }

        private void btnCEOAccess_Click(object sender, RoutedEventArgs e)
        {
            CEOLoginWindow ceoLogin = new CEOLoginWindow();
            ceoLogin.ShowDialog();

            if (ceoLogin.AccessGranted)
            {
                txtCEORestricted.Visibility = Visibility.Collapsed;
                spCEOData.Visibility = Visibility.Visible;
            }
        }
 


    }
}