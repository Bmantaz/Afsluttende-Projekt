using System.Collections.Generic; // Giver adgang til List<T>, som bruges til at håndtere en liste af bankoplader
using System.Data; // Giver adgang til DataTable, som bruges til at repræsentere data i DataGrid
using System.Windows; // Giver adgang til WPF-funktionalitet som vinduer og kontroller
using System.Windows.Controls; // Giver adgang til DataGrid-kontrol

namespace Afsluttende_Projekt
{
    public partial class BankoWindow : Window
    {
        public BankoWindow()
        {
            InitializeComponent(); // Initialiserer UI-komponenter defineret i XAML-filen
            Loaded += BankoWindow_Loaded; // Tildeler en event-handler, der køres, når vinduet er færdigindlæst
        }

        // Event-handler der kører, når vinduet er færdigindlæst
        private void BankoWindow_Loaded(object sender, RoutedEventArgs e) // Bruges til at generere bankoplader og vise dem, når vinduet åbnes.
        {
            BankoGenerator gen = new BankoGenerator(); // Opretter en instans af BankoGenerator
            List<int[,]> boards = gen.Generate3Boards(); // Genererer tre bankoplader (3x9 arrays)

            // Vis hver plade i en DataGrid
            VisPlade(dgPlade1, boards[0]); // Viser første plade i dgPlade1
            VisPlade(dgPlade2, boards[1]); // Viser anden plade i dgPlade2
            VisPlade(dgPlade3, boards[2]); // Viser tredje plade i dgPlade3
        }

        // Metode til at vise en plade i en DataGrid
        private void VisPlade(DataGrid dg, int[,] plade)
        {
            // Opretter en DataTable for at formatere data til DataGrid
            DataTable dt = new DataTable();

            // Tilføjer 9 kolonner (en for hver kolonne i bankopladen)
            for (int c = 0; c < 9; c++)
            {
                dt.Columns.Add($"Col{c + 1}", typeof(string)); // Kolonner navngives "Col1", "Col2", osv.
            }

            // Tilføjer 3 rækker (en for hver række i bankopladen)
            for (int r = 0; r < 3; r++)
            {
                DataRow row = dt.NewRow(); // Opretter en ny række i DataTable
                for (int c = 0; c < 9; c++) // Itererer gennem hver kolonne
                {
                    int val = plade[r, c]; // Henter værdien fra bankopladen
                    row[c] = val == 0 ? "" : val.ToString(); // Sætter værdien som tom, hvis den er 0, ellers konverteres den til en streng
                }
                dt.Rows.Add(row); // Tilføjer rækken til DataTable
            }

            dg.ItemsSource = dt.DefaultView; // Binder DataTable til DataGrid for visning
        }
    }
}

/*
 Forventet funktionalitet
 Når vinduet åbnes, genereres tre bankoplader.
 Hver plade vises i sin egen DataGrid (dgPlade1, dgPlade2, dgPlade3).
 Tomme felter i pladerne (værdier på 0) vises som tomme celler i DataGrid.
 */