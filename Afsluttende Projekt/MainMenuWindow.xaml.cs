using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

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

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectDir = Directory.GetParent(Directory.GetParent(Directory.GetParent(baseDir).FullName).FullName).FullName;

            string imagePath = Path.Combine(projectDir, "Assets", "Profilbilleder", bruger.MedarbejderID + ".png");

            if (!File.Exists(imagePath))
            {
                imagePath = Path.Combine(projectDir, "Assets", "Profilbilleder", "default.png");
            }

            profilImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
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

        private void Banko_Click(object sender, RoutedEventArgs e)
        {
            BankoWindow bw = new BankoWindow();
            bw.ShowDialog();
        }

        private void MainMenuWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LogUd_Click(object sender, RoutedEventArgs e)
        {
            // Luk dette vindue
            this.Hide();

            // Opret en ny instans af MainWindow
            MainWindow newMainWindow = new MainWindow();
            newMainWindow.Show();
        }
    }
}