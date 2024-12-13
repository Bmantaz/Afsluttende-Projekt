using System; // Indeholder grundlæggende typer og funktioner som Random
using System.Collections.Generic; // Giver adgang til datastrukturer som List<T> og HashSet<T>

namespace Afsluttende_Projekt
{
    public class BankoGenerator
    {
        // Tilfældighedsgenerator til at generere unikke tal
        private Random rand = new Random();

        // Definition af intervaller for hver kolonne i en bankoplade
        // Hver tuple (start, end) repræsenterer det numeriske interval for en kolonne
        private (int start, int end)[] kolonneIntervaller = new (int, int)[]
        {
            (1, 9),   // Kolonne 1: Tal fra 1 til 9
            (10, 19), // Kolonne 2: Tal fra 10 til 19
            (20, 29), // Kolonne 3: Tal fra 20 til 29
            (30, 39), // Kolonne 4: Tal fra 30 til 39
            (40, 49), // Kolonne 5: Tal fra 40 til 49
            (50, 59), // Kolonne 6: Tal fra 50 til 59
            (60, 69), // Kolonne 7: Tal fra 60 til 69
            (70, 79), // Kolonne 8: Tal fra 70 til 79
            (80, 90)  // Kolonne 9: Tal fra 80 til 90
        };

        
        // Genererer tre unikke bankoplader.
        
        // <returns>En liste med tre 3x9 matrixer (bankoplader).
        public List<int[,]> Generate3Boards()
        {
            List<int[,]> boards = new List<int[,]>(); // Opretter en liste til de tre plader
            for (int i = 0; i < 3; i++) // Laver tre iterationer for at generere tre plader
            {
                boards.Add(GenerateSingleBoard()); // Tilføjer en enkelt plade til listen
            }
            return boards; // Returnerer listen med alle tre plader
        }

        
        // Genererer en enkelt bankoplade.
        
        // <returns>En 3x9 matrix, der repræsenterer en valid bankoplade.
        private int[,] GenerateSingleBoard()
        {
            // Prøver op til 1000 gange at generere en valid bankoplade
            for (int attempt = 0; attempt < 1000; attempt++)
            {
                int[,] board = TryGenerateBoard(); // Forsøger at generere en plade
                if (board != null) // Hvis pladen er valid
                {
                    return board; // Returnerer den genererede plade
                }
            }
            // Hvis det ikke lykkedes at generere en valid plade, returner null
            return null;
        }
      
        // Forsøger at generere en valid bankoplade ved at placere 15 tal korrekt.
        // <returns>En valid 3x9 matrix eller null, hvis det mislykkes.
        private int[,] TryGenerateBoard()
        {
            // Liste til at holde tallene i hver kolonne
            List<List<int>> kolonneTal = new List<List<int>>();
            for (int i = 0; i < 9; i++) // Initialiserer listen med 9 kolonner
            {
                kolonneTal.Add(new List<int>());
            }

            // Vælger ét tal fra hvert interval for at sikre, at hver kolonne har mindst ét tal
            for (int col = 0; col < 9; col++)
            {
                int val = PickRandomNumber(kolonneIntervaller[col].start, kolonneIntervaller[col].end, kolonneTal);
                if (val == 0) return null; // Returnerer null, hvis der ikke kunne vælges et valid tal
                kolonneTal[col].Add(val); // Tilføjer det valgte tal til kolonnen
            }

            // Vælger 6 yderligere tal fordelt på kolonnerne for at nå 15 tal i alt
            for (int i = 0; i < 6; i++)
            {
                bool added = false;
                for (int attempt = 0; attempt < 100 && !added; attempt++) // Forsøger op til 100 gange at finde en kolonne
                {
                    int col = rand.Next(9); // Vælger en tilfældig kolonne
                    if (kolonneTal[col].Count < 3) // Sikrer, at der maksimalt er 3 tal pr. kolonne
                    {
                        int val = PickRandomNumber(kolonneIntervaller[col].start, kolonneIntervaller[col].end, kolonneTal);
                        if (val != 0) // Hvis der findes et valid tal
                        {
                            kolonneTal[col].Add(val); // Tilføjer tallet til kolonnen
                            added = true; // Marker som succesfuldt tilføjet
                        }
                    }
                }
                if (!added) return null; // Returnerer null, hvis et tal ikke kunne tilføjes
            }

            // Sorterer tallene i hver kolonne i stigende rækkefølge
            for (int col = 0; col < 9; col++)
            {
                kolonneTal[col].Sort();
            }

            // Initialiserer en 3x9 matrix til at holde tallene
            int[,] boardArrangement = new int[3, 9];
            int[][] colNumbers = new int[9][]; // Konverterer listen til arrays for nemmere håndtering
            for (int c = 0; c < 9; c++)
            {
                colNumbers[c] = kolonneTal[c].ToArray();
            }

            int[] rowCounts = new int[3]; // Tæller antallet af tal i hver række

            // Fordeler tallene på rækker ved hjælp af en rekursiv metode
            if (!AssignColumns(0, colNumbers, boardArrangement, rowCounts))
            {
                return null; // Returnerer null, hvis fordelingen ikke var mulig
            }

            return boardArrangement; // Returnerer den færdige bankoplade
        }


        // Rekursiv metode til at fordele tallene fra kolonner til rækker.

        private bool AssignColumns(int colIndex, int[][] colNumbers, int[,] board, int[] rowCounts)
        {
            if (colIndex == 9) // Hvis alle kolonner er placeret
            {
                // Sikrer, at hver række har præcis 5 tal
                return (rowCounts[0] == 5 && rowCounts[1] == 5 && rowCounts[2] == 5);
            }

            List<int[]> combinations = GetRowCombinations(colNumbers[colIndex].Length); // Henter valide kombinationer af rækker
            foreach (var combo in combinations)
            {
                bool valid = true;
                int[] oldCounts = (int[])rowCounts.Clone(); // Gemmer den oprindelige tilstand af rækker
                for (int i = 0; i < combo.Length; i++)
                {
                    int r = combo[i];
                    if (rowCounts[r] >= 5) // Tjekker, om rækken allerede har 5 tal
                    {
                        valid = false;
                        break;
                    }
                    rowCounts[r]++;
                }

                if (!valid) // Hvis kombinationen ikke er valid
                {
                    rowCounts = oldCounts; // Tilbagefører ændringer
                    continue;
                }

                // Placerer tallene i pladen
                for (int i = 0; i < combo.Length; i++)
                {
                    board[combo[i], colIndex] = colNumbers[colIndex][i];
                }

                // Fortsætter til næste kolonne
                if (AssignColumns(colIndex + 1, colNumbers, board, rowCounts))
                {
                    return true; // Returnerer true, hvis en valid løsning findes
                }

                // Backtracking: Fjerner tallene og genopretter rækkerne
                for (int i = 0; i < combo.Length; i++)
                {
                    board[combo[i], colIndex] = 0;
                }
                rowCounts = oldCounts;
            }

            return false; // Returnerer false, hvis ingen valid løsning findes
        }

        // Returnerer alle mulige valide kombinationer af rækker, hvor tal kan placeres.
        private List<int[]> GetRowCombinations(int countNumbers)
        {
            List<int[]> combos = new List<int[]>(); // Liste til kombinationer
            if (countNumbers == 1)
            {
                combos.Add(new int[] { 0 });
                combos.Add(new int[] { 1 });
                combos.Add(new int[] { 2 });
            }
            else if (countNumbers == 2)
            {
                combos.Add(new int[] { 0, 1 });
                combos.Add(new int[] { 0, 2 });
                combos.Add(new int[] { 1, 2 });
            }
            else if (countNumbers == 3)
            {
                combos.Add(new int[] { 0, 1, 2 });
            }
            return combos; // Returnerer alle kombinationer
        }

        // Vælger et tilfældigt tal fra et interval, som ikke allerede er brugt.
        private int PickRandomNumber(int start, int end, List<List<int>> kolonneTal)
        {
            HashSet<int> usedNumbers = new HashSet<int>(); // Gemmer allerede brugte tal
            foreach (var colList in kolonneTal)
            {
                foreach (var num in colList)
                {
                    usedNumbers.Add(num); // Tilføjer alle brugte tal
                }
            }

            List<int> possible = new List<int>(); // Liste til mulige valide tal
            for (int i = start; i <= end; i++)
            {
                if (!usedNumbers.Contains(i)) // Tjekker, om tallet ikke er brugt
                {
                    possible.Add(i);
                }
            }

            if (possible.Count == 0) return 0; // Returnerer 0, hvis ingen valide tal findes
            return possible[rand.Next(possible.Count)]; // Returnerer et tilfældigt valid tal
        }
    }
}


/*
 Forventet Funktionalitet
 Generering af Bankoplader:
 Hver plade er en valid 3x9 matrix med 15 unikke tal.
 Tallene opfylder reglen om max. 3 tal pr. kolonne og præcis 5 tal pr. række.
 Fordeling af Tal:
 Tallene er korrekt fordelt i rækkerne ved hjælp af rekursiv backtracking.
 */