using System; // Giver adgang til grundlæggende funktioner som filhåndtering og exceptions
using System.Collections.Generic; // Giver adgang til List<T>, som bruges til at håndtere en liste af brugere
using System.IO; // Bruges til filstier og filoperationer
using OfficeOpenXml; // EPPlus-biblioteket bruges til at arbejde med Excel-filer

namespace Afsluttende_Projekt
{
    public class DataAccess
    {
        private string excelFilePath; // Variabel til at gemme stien til Excel-filen

        public DataAccess()
        {
            // Finder den mappe, hvor applikationen kører fra
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            // Går op til projektmappen for at finde 'Assets'-mappen
            string projectDir = Directory.GetParent(Directory.GetParent(Directory.GetParent(baseDir).FullName).FullName).FullName;

            // Kombinerer stien til projektet med stien til Excel-filen
            excelFilePath = Path.Combine(projectDir, "Assets/Brugerdata.xlsx");

            // Sætter licenskonteksten for EPPlus til ikke-kommerciel brug
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        // Læser data fra Excel-filen og returnerer en liste af brugere.
        public List<Bruger> HentBrugere()
        {
            List<Bruger> brugere = new List<Bruger>(); // Opretter en tom liste til brugere
            using (var package = new ExcelPackage(new FileInfo(excelFilePath))) // Åbner Excel-filen
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Ark1"]; // Henter arket ved navn "Ark1"

                int rowCount = worksheet.Dimension.Rows; // Antal rækker i arket
                for (int row = 2; row <= rowCount; row++) // Starter ved række 2 (springer overskrifter over)
                {
                    // Opretter en ny Bruger-instans og fylder den med data fra Excel
                    Bruger bruger = new Bruger
                    {
                        Brugernavn = worksheet.Cells[row, 1].Text,    // Kolonne 1: Brugernavn
                        MedarbejderID = worksheet.Cells[row, 2].Text, // Kolonne 2: Medarbejder ID
                        Adgangskode = worksheet.Cells[row, 3].Text    // Kolonne 3: Adgangskode
                    };
                    brugere.Add(bruger); // Tilføjer brugeren til listen
                }
            }
            return brugere; // Returnerer listen med brugere
        }

        // Opdaterer adgangskoden for en bruger baseret på medarbejder-ID
        public void OpdaterKodeord(string medarbejderID, string nyKode)
        {
            using (var package = new ExcelPackage(new FileInfo(excelFilePath))) // Åbner Excel-filen
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Ark1"]; // Henter arket "Ark1"

                int rowCount = worksheet.Dimension.Rows; // Antal rækker i arket
                for (int row = 2; row <= rowCount; row++) // Itererer gennem rækkerne
                {
                    string id = worksheet.Cells[row, 2].Text; // Læser medarbejder-ID fra kolonne 2
                    if (id == medarbejderID) // Tjekker om ID'et matcher
                    {
                        worksheet.Cells[row, 3].Value = nyKode; // Opdaterer adgangskoden i kolonne 3
                        package.Save(); // Gemmer ændringerne i Excel-filen
                        break; // Stopper loopet, da opdateringen er færdig
                    }
                }
            }
        }

        // Henter stien til profilbilledet for en given bruger baseret på medarbejder-ID
        public string HentProfilbilledeSti(string medarbejderID)
        {
            // Finder projektets base directory
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            // Går op til projektmappen og tilføjer stien til mappen med profilbilleder
            string projectDir = Directory.GetParent(Directory.GetParent(Directory.GetParent(baseDir).FullName).FullName).FullName;
            string imagePath = Path.Combine(projectDir, "Assets", "Profilbilleder", $"{medarbejderID}.png");

            // Tjekker, om billedet findes på den specificerede sti
            if (File.Exists(imagePath))
            {
                return imagePath; // Returnerer stien, hvis filen findes
            }
            return null; // Returnerer null, hvis filen ikke findes
        }
    }

    // Definerer en klasse, der repræsenterer en bruger med felter for brugernavn, medarbejder-ID og adgangskode.
    public class Bruger
    {
        public string Brugernavn { get; set; }    // Brugernavn for brugeren
        public string MedarbejderID { get; set; } // Medarbejderens unikke ID
        public string Adgangskode { get; set; }   // Adgangskoden for brugeren
    }
}

/*
 Forventet funktionalitet
 Hent brugere: Læser alle brugere fra Excel-filen og returnerer en liste.
 Opdater adgangskode: Ændrer adgangskoden for en specifik bruger i Excel.
 Hent profilbillede: Returnerer stien til et profilbillede baseret på medarbejder-ID.
 */