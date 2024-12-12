using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace Afsluttende_Projekt
{
    public class DataAccess
    {
        private string excelFilePath;    // Variabel til at gemme stien til Excel-filen

        public DataAccess()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;                                             // Finder den mappe, hvor applikationen kører fra (typisk bin/Debug)
            string projectDir = Directory.GetParent(Directory.GetParent(Directory.GetParent(baseDir).FullName).FullName).FullName; // Går flere niveauer op for at nå projektmappen
            excelFilePath = Path.Combine(projectDir, "Assets/Brugerdata.xlsx");                               // Sammensætter stien til Excel-filen i Assets-mappen
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;                                          // Sætter licenskonteksten for EPPlus til ikke-kommerciel brug

        }



        public List<Bruger> HentBrugere()
        {
            List<Bruger> brugere = new List<Bruger>(); // Opretter en liste til at gemme brugere           

            using (var package = new ExcelPackage(new FileInfo(excelFilePath))) // Åbner Excel-filen med EPPlus
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Ark1"]; // Henter arket ved navn "Ark1"

                int rowCount = worksheet.Dimension.Rows; // Antal rækker med data i arket
                for (int row = 2; row <= rowCount; row++) // Starter ved række 2, da række 1 antages at være overskrifter
                {
                    Bruger bruger = new Bruger
                    {
                        Brugernavn = worksheet.Cells[row, 1].Text,    // Læser brugernavn fra kolonne 1
                        MedarbejderID = worksheet.Cells[row, 2].Text,  // Læser medarbejder ID fra kolonne 2
                        Adgangskode = worksheet.Cells[row, 3].Text      // Læser adgangskoden fra kolonne 3
                    };
                    brugere.Add(bruger); // Tilføjer den læste bruger til listen
                }
            }
            return brugere; // Returnerer listen med alle fundne brugere
        }

        public void OpdaterKodeord(string medarbejderID, string nyKode)
        {


            using (var package = new ExcelPackage(new FileInfo(excelFilePath))) // Åbn Excel-filen
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Ark1"]; // Hent "Ark1"

                int rowCount = worksheet.Dimension.Rows; // Antal rækker med data
                for (int row = 2; row <= rowCount; row++) // Iterer gennem rækkerne med data
                {
                    string id = worksheet.Cells[row, 2].Text; // Læs medarbejder ID fra kolonne 2
                    if (id == medarbejderID) // Hvis det matchende ID findes
                    {
                        worksheet.Cells[row, 3].Value = nyKode; // Sæt ny adgangskode i kolonne 3
                        package.Save(); // Gem ændringerne i Excel-filen
                        break; // Afslut løkken, da vi har opdateret koden
                    }
                }
            }
        }
    }

    public class Bruger
    {
        public string Brugernavn { get; set; }    // Gemmer brugerens navn
        public string MedarbejderID { get; set; } // Gemmer brugerens medarbejder ID
        public string Adgangskode { get; set; }    // Gemmer brugerens adgangskode
    }
}