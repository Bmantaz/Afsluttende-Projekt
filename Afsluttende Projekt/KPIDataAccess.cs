using System; // Giver adgang til grundlæggende funktioner som filhåndtering og exceptions
using System.Collections.Generic; // Giver adgang til List<T>, som bruges til at håndtere en liste af KPI-data
using System.IO; // Bruges til filstier og filoperationer
using OfficeOpenXml; // EPPlus-biblioteket bruges til at arbejde med Excel-filer

namespace Afsluttende_Projekt
{
    // Modelklasse til at repræsentere KPI-data
    public class KPIData
    {
        public int OrdrePåDagen { get; set; } // Antal ordrer på dagen
        public double Præcision { get; set; } // Præcision som procentværdi
        public int Uheld { get; set; } // Antal uheld
    }

    // Klasse til at håndtere adgang til KPI-data fra Excel-filen
    public class KPIDataAccess
    {
        private string excelFilePath; // Variabel til at gemme stien til Excel-filen

        public KPIDataAccess()
        {
            // Finder den mappe, hvor applikationen kører fra.
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            // Går op til projektmappen for at finde 'Assets'-mappen
            string projectDir = Directory.GetParent(Directory.GetParent(Directory.GetParent(baseDir).FullName).FullName).FullName;

            // Kombinerer stien til projektet med stien til Excel-filen
            excelFilePath = Path.Combine(projectDir, "Assets", "KPI - Data.xlsx");

            // Tjekker, om Excel-filen findes
            if (!File.Exists(excelFilePath))
            {
                // Smider en undtagelse, hvis filen ikke findes
                throw new Exception($"Excel-filen blev ikke fundet på stien: {excelFilePath}");
            }

            // Sætter licenskonteksten for EPPlus til ikke-kommerciel brug
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        // Henter en liste af KPI-data fra Excel-filen
        public List<KPIData> HentKPIData()
        {
            var kpiList = new List<KPIData>(); // Opretter en tom liste til KPI-data

            using (var package = new ExcelPackage(new FileInfo(excelFilePath))) // Åbner Excel-filen
            {
                var worksheet = package.Workbook.Worksheets["Data"]; // Henter arket ved navn "Data"

                // Kontrollerer, om arket eksisterer
                if (worksheet == null)
                {
                    throw new Exception("Arket 'Data' blev ikke fundet i Excel-filen.");
                }

                int rowCount = worksheet.Dimension.Rows; // Antal rækker i arket

                // Starter ved række 2 (springer overskrifter over)
                for (int row = 2; row <= rowCount; row++)
                {
                    // Opretter en ny instans af KPIData og fylder den med data fra Excel
                    var kpi = new KPIData
                    {
                        OrdrePåDagen = int.Parse(worksheet.Cells[row, 1].Text), // Kolonne 1: Antal ordrer
                        Præcision = double.Parse(worksheet.Cells[row, 2].Text), // Kolonne 2: Præcision
                        Uheld = int.Parse(worksheet.Cells[row, 3].Text) // Kolonne 3: Uheld
                    };

                    kpiList.Add(kpi); // Tilføjer KPI-data til listen
                }
            }

            return kpiList; // Returnerer listen med KPI-data
        }
    }
}

/*
  Forventet funktionalitet
  Excel-fil: Sørg for, at KPI - Data.xlsx eksisterer i Assets-mappen, og at den har et ark kaldet "Data" med følgende kolonner:
  Kolonne 1: Antal Ordre på dagen (heltal)
  Kolonne 2: Præcision (procent som decimal)
  Kolonne 3: Uheld (heltal)
  Dataindlæsning: Alle rækker fra arket "Data" bliver læst ind i en liste af KPIData.
  Fejlhåndtering: Hvis filen eller arket ikke findes, vises en forståelig fejlbesked.
 */
