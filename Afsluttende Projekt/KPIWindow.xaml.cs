using System; // Indeholder grundlæggende typer som Exception og Action
using System.Collections.Generic; // Giver adgang til List<T>, som bruges til at holde KPI-data
using System.Linq; // Giver adgang til LINQ-metoder som OrderBy og Average
using System.Windows; // Indeholder WPF-klasser som Window og MessageBox

namespace Afsluttende_Projekt
{
    public partial class KPIWindow : Window
    {
        // Liste til at holde KPI-data hentet fra en ekstern kilde (Excel)
        private List<KPIData> kpiData;

        // Objekt, der håndterer læsning og behandling af KPI-data fra Excel-filen
        private KPIDataAccess dataAccess;

        public KPIWindow()
        {
            // Initialiserer alle UI-elementer defineret i XAML-filen
            InitializeComponent();

            // Tilføjer en eventhandler, der kører, når vinduet er færdigindlæst
            Loaded += KPIWindow_Loaded;
        }

        private void KPIWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Eventhandler der kaldes, når vinduet er færdigindlæst. Indlæser og viser KPI-data.
            try
            {
                // Opretter en instans af KPIDataAccess til at læse data fra Excel
                dataAccess = new KPIDataAccess();

                // Henter KPI-data og gemmer det i kpiData-listen
                kpiData = dataAccess.HentKPIData();

                // Opdaterer visningen i datagrid med de hentede data
                OpdaterKPIGrid();

                // Beregner og viser statistik som gennemsnit og median
                BeregnStatistik();
            }
            catch (Exception ex) // Håndterer fejl under dataindlæsning
            {
                // Viser en fejlmeddelelse, hvis der opstår en undtagelse
                MessageBox.Show($"Der opstod en fejl under indlæsning af KPI-data:\n{ex.Message}",
                                "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SortBedstTilVærst_Click(object sender, RoutedEventArgs e)
        {
            // Sorterer KPI-data i faldende rækkefølge baseret på præcision
            // Derefter sorteres efter "OrdrePåDagen" og "Uheld", hvis præcisionen er ens
            kpiData = kpiData.OrderByDescending(kpi => kpi.Præcision)
                             .ThenByDescending(kpi => kpi.OrdrePåDagen)
                             .ThenByDescending(kpi => kpi.Uheld)
                             .ToList();

            // Opdaterer visningen i datagrid med de sorterede data
            OpdaterKPIGrid();
        }

        private void SortVærstTilBedst_Click(object sender, RoutedEventArgs e)
        {
            // Sorterer KPI-data i stigende rækkefølge baseret på præcision
            // Derefter sorteres efter "OrdrePåDagen" og "Uheld", hvis præcisionen er ens
            kpiData = kpiData.OrderBy(kpi => kpi.Præcision)
                             .ThenBy(kpi => kpi.OrdrePåDagen)
                             .ThenBy(kpi => kpi.Uheld)
                             .ToList();

            // Opdaterer visningen i datagrid med de sorterede data
            OpdaterKPIGrid();
        }

        private void OpdaterKPIGrid()
        {
            // Fjerner den nuværende kilde for datagrid for at sikre korrekt opdatering
            KPIGrid.ItemsSource = null;

            // Tildeler den opdaterede liste som datakilde til datagrid
            KPIGrid.ItemsSource = kpiData;
        }

        private void BeregnStatistik()
        {
            // Hvis der ikke er nogen KPI-data, vis en besked og afslut metoden
            if (kpiData == null || kpiData.Count == 0)
            {
                txtStatistik.Text = "Ingen data tilgængelig.";
                return;
            }

            // Initialiserer en streng til at gemme statistik
            string statistik = "";

            // Beregner gennemsnit og median for Antal Ordre og tilføjer det til statistik-strengen
            statistik += $"Antal Ordre:\n";
            statistik += $"- Gennemsnit: {kpiData.Average(kpi => kpi.OrdrePåDagen):0.00}\n";
            statistik += $"- Median: {BeregnMedian(kpiData.Select(kpi => (double)kpi.OrdrePåDagen).ToList()):0.00}\n\n";

            // Beregner gennemsnit og median for Præcision og tilføjer det til statistik-strengen
            statistik += $"Præcision:\n";
            statistik += $"- Gennemsnit: {kpiData.Average(kpi => kpi.Præcision):0.00}%\n";
            statistik += $"- Median: {BeregnMedian(kpiData.Select(kpi => kpi.Præcision).ToList()):0.00}%\n\n";

            // Beregner gennemsnit og median for Uheld og tilføjer det til statistik-strengen
            statistik += $"Uheld:\n";
            statistik += $"- Gennemsnit: {kpiData.Average(kpi => kpi.Uheld):0.00}\n";
            statistik += $"- Median: {BeregnMedian(kpiData.Select(kpi => (double)kpi.Uheld).ToList()):0.00}\n";

            // Viser statistikken i tekstboksen
            txtStatistik.Text = statistik;
        }

        private double BeregnMedian(List<double> values)
        {
            // Returnerer 0, hvis listen er tom eller null
            if (values == null || values.Count == 0) return 0;

            // Sorterer listen for at finde medianen
            values.Sort();

            int count = values.Count;

            // Hvis der er et lige antal værdier, returnér gennemsnittet af de to midterste
            if (count % 2 == 0)
            {
                return (values[count / 2 - 1] + values[count / 2]) / 2.0;
            }
            else
            {
                // Hvis der er et ulige antal værdier, returnér den midterste værdi
                return values[count / 2];
            }
        }
    }
}

/*
 Forventet Funktionalitet
 Indlæsning af Data: Ved opstart indlæses KPI-data fra en Excel-fil, som viser antallet af ordrer, præcision (procent), og antal uheld.

 Visning i DataGrid: KPI-data vises i et DataGrid, som opdateres dynamisk ved ændringer.

 Sorteringsmuligheder:

 Sortér fra bedst til værst (faldende rækkefølge).
 Sortér fra værst til bedst (stigende rækkefølge).
 Statistikberegning: Beregner og viser gennemsnit og median for alle KPI-kategorier i tekstform.

 Fejlhåndtering: Informerer brugeren om fejl ved indlæsning af data.
 */
