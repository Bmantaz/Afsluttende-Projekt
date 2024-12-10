using System;
using System.Collections.Generic;
using System.Windows;
using System.Text.RegularExpressions;

namespace Afsluttende_Projekt
{
    public partial class ForgotPasswordWindow : Window
    {
        private List<Bruger> brugere;
        private Bruger matchendeBruger = null;

        public ForgotPasswordWindow()
        {
            InitializeComponent();
            DataAccess da = new DataAccess();
            brugere = da.HentBrugere();
        }

        // Trin 1: Indtast Medarbejder ID
        private void Step1_OK_Click(object sender, RoutedEventArgs e)
        {
            string medarbejderID = txtMedarbejderID.Text.Trim();
            if (string.IsNullOrEmpty(medarbejderID))
            {
                MessageBox.Show("Indtast et medarbejder ID.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            matchendeBruger = null;
            foreach (var b in brugere)
            {
                if (b.MedarbejderID == medarbejderID)
                {
                    matchendeBruger = b;
                    break;
                }
            }

            if (matchendeBruger == null)
            {
                MessageBox.Show("Medarbejder ID ikke fundet.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Gå til trin 2
            GridStep1.Visibility = Visibility.Collapsed;
            txtBekræftelse.Text = $"Hej {matchendeBruger.Brugernavn} (ID: {matchendeBruger.MedarbejderID}).\nTryk 'Ja' for at nulstille dit kodeord. Tryk 'Nej' for at annullere.";
            GridStep2.Visibility = Visibility.Visible;
        }

        // Trin 2: Bekræft nulstilling
        private void Step2_Yes_Click(object sender, RoutedEventArgs e)
        {
            // Gå til trin 3
            GridStep2.Visibility = Visibility.Collapsed;
            GridStep3.Visibility = Visibility.Visible;
        }

        private void Step2_No_Click(object sender, RoutedEventArgs e)
        {
            // Annuller og luk
            this.Close();
        }

        // Trin 3: Indtast ny adgangskode og gem
        private void Step3_Gem_Click(object sender, RoutedEventArgs e)
        {
            string nyKode = txtNyAdgangskode.Password;

            if (ErAdgangskodeGyldig(nyKode))
            {
                // Opdater adgangskoden i Excel
                DataAccess da = new DataAccess();
                da.OpdaterKodeord(matchendeBruger.MedarbejderID, nyKode);

                MessageBox.Show($"Her er dit nye kodeord: {nyKode}", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Adgangskoden opfylder ikke kravene. Den skal indeholde mindst 7 tegn, herunder store og små bogstaver, mindst ét tal og ét specialtegn. Prøv venligst igen.",
                                "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ErAdgangskodeGyldig(string kode)
        {
            // Krav:
            // Mindst 7 tegn
            // Mindst ét specialtegn
            // Mindst ét tal
            // Mindst én stor og én lille bogstav

            if (kode.Length < 7)
                return false;

            bool harTal = Regex.IsMatch(kode, @"\d");
            bool harSpecial = Regex.IsMatch(kode, @"[\W_]");
            bool harStore = Regex.IsMatch(kode, @"[A-Z]");
            bool harSmå = Regex.IsMatch(kode, @"[a-z]");

            return harTal && harSpecial && harStore && harSmå;
        }
    }
}