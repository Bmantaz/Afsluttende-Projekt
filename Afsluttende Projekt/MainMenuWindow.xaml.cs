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