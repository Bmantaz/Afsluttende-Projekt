using System;
using System;
using System.Collections.Generic;

namespace Afsluttende_Projekt
{
    public class BankoGenerator
    {
        private Random rand = new Random();

        // Intervaller for hver kolonne
        private (int start, int end)[] kolonneIntervaller = new (int, int)[]
        {
            (1,9),
            (10,19),
            (20,29),
            (30,39),
            (40,49),
            (50,59),
            (60,69),
            (70,79),
            (80,90)
        };

        public List<int[,]> Generate3Boards()
        {
            List<int[,]> boards = new List<int[,]>();
            for (int i = 0; i < 3; i++)
            {
                boards.Add(GenerateSingleBoard());
            }
            return boards;
        }

        private int[,] GenerateSingleBoard()
        {
            // Vi forsøger flere gange, indtil vi finder en gyldig plade
            for (int attempt = 0; attempt < 1000; attempt++)
            {
                int[,] board = TryGenerateBoard();
                if (board != null)
                {
                    return board;
                }
            }
            // Hvis vi ikke kan generere en plade på 1000 forsøg, returner null (usandsynligt)
            return null;
        }

        private int[,] TryGenerateBoard()
        {
            // Vi skal vælge 15 unikke tal i alt.
            // Minimum 1 tal fra hver kolonne.
            // 5 tal pr. række = 15 tal i alt.
            // Kolonner sorteres stigende lodret.

            // Start med at vælge mindst 1 tal fra hver kolonne
            // og derefter tilføje flere tal for at nå 15 i alt.

            List<List<int>> kolonneTal = new List<List<int>>();
            for (int i = 0; i < 9; i++)
            {
                kolonneTal.Add(new List<int>());
            }

            // Først vælg 9 tal (ét fra hver kolonne)
            for (int col = 0; col < 9; col++)
            {
                int val = PickRandomNumber(kolonneIntervaller[col].start, kolonneIntervaller[col].end, kolonneTal);
                if (val == 0) return null; // Fejl, kunne ikke finde ledigt tal
                kolonneTal[col].Add(val);
            }

            // Vi har nu 9 tal, skal bruge 6 mere for at få 15.
            // Vælg 6 ekstra tal i vilkårlige kolonner, men ingen kolonne må have mere end 3 tal (da vi kun har 3 rækker)
            for (int i = 0; i < 6; i++)
            {
                bool added = false;
                // Prøv op til 100 gange at finde en kolonne at tilføje et tal til
                for (int attempt = 0; attempt < 100 && !added; attempt++)
                {
                    int col = rand.Next(9);
                    if (kolonneTal[col].Count < 3)
                    {
                        int val = PickRandomNumber(kolonneIntervaller[col].start, kolonneIntervaller[col].end, kolonneTal);
                        if (val != 0)
                        {
                            kolonneTal[col].Add(val);
                            added = true;
                        }
                    }
                }
                if (!added) return null; // Kunne ikke placere flere tal
            }

            // Nu har vi 15 tal fordelt på 9 kolonner.
            // Sorter hver kolonne stigende
            for (int col = 0; col < 9; col++)
            {
                kolonneTal[col].Sort();
            }

            // Placer tal i et 3x9 board
            // Vi skal have 5 tal i hver række.
            // Vi har 15 tal i alt, så alle 3 rækker skal have præcist 5 tal.

            // Vi ved at hver kolonne max har 3 tal. Vi vil prøve at fordele tal på rækker, så der i alt bliver 5 i hver række.

            // Vi laver en rekursiv/backtracking-lignende løsningsmetode for at placere tal i 3 rækker.
            int[,] boardArrangement = new int[3, 9];

            // Lav en liste over kolonnens tal (hver kolonne har 1-3 tal):
            int[][] colNumbers = new int[9][];
            for (int c = 0; c < 9; c++)
            {
                colNumbers[c] = kolonneTal[c].ToArray();
            }

            // Vi skal fordele disse kolonnenumre i 3 rækker, sådan at hver række har i alt 5 tal.
            // Start med en backtracking:
            int[] rowCounts = new int[3]; // Antal tal i hver række pt.

            if (!AssignColumns(0, colNumbers, boardArrangement, rowCounts))
            {
                return null; // Kunne ikke fordele validt
            }

            return boardArrangement;
        }

        private bool AssignColumns(int colIndex, int[][] colNumbers, int[,] board, int[] rowCounts)
        {
            if (colIndex == 9)
            {
                // Alle kolonner er placeret. Tjek om hver række har 5 tal
                return (rowCounts[0] == 5 && rowCounts[1] == 5 && rowCounts[2] == 5);
            }

            int countNumbers = colNumbers[colIndex].Length;
            // Vi skal placere disse countNumbers i distincte rækker i stigende orden top-to-bottom
            // Da tallene allerede er sorteret, første tal skal i en højere række end næste, etc.
            // Vi skal vælge countNumbers rækker (ud af 3) i stigende rækkefølge (f.eks. hvis der er 2 tal, en mulig placering er row 0 og row 2).
            // Prøv alle kombinationer af rækker til disse tal:

            List<int[]> combinations = GetRowCombinations(countNumbers);

            foreach (var combo in combinations)
            {
                // Tjek om combo er mulig med hensyn til at have nok plads i rækkerne (max 5 tal per række)
                bool valid = true;
                int[] oldCounts = (int[])rowCounts.Clone();
                for (int i = 0; i < countNumbers; i++)
                {
                    int r = combo[i];
                    if (rowCounts[r] >= 5)
                    {
                        valid = false;
                        break;
                    }
                    rowCounts[r]++;
                }

                if (!valid)
                {
                    rowCounts = oldCounts;
                    continue;
                }

                // Hvis valid, placer tallene
                for (int i = 0; i < countNumbers; i++)
                {
                    int r = combo[i];
                    board[r, colIndex] = colNumbers[colIndex][i];
                }

                // Gå videre til næste kolonne
                if (AssignColumns(colIndex + 1, colNumbers, board, rowCounts))
                {
                    return true;
                }

                // Backtrack
                for (int i = 0; i < countNumbers; i++)
                {
                    int r = combo[i];
                    board[r, colIndex] = 0;
                }
                rowCounts = oldCounts;
            }

            return false;
        }

        private List<int[]> GetRowCombinations(int countNumbers)
        {
            // countNumbers er 1, 2 eller 3.
            // Hvis 1 tal: mulige kombinationer er {0}, {1}, {2} (men vi vil prøve alle 3)
            // Hvis 2 tal: mulige kombinationer: {0,1}, {0,2}, {1,2} (rækker i stigende orden)
            // Hvis 3 tal: eneste kombination der giver stigende order er {0,1,2}.

            List<int[]> combos = new List<int[]>();
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
            return combos;
        }

        private int PickRandomNumber(int start, int end, List<List<int>> kolonneTal)
        {
            // Vælg et tilfældigt tal i intervallet [start,end] som ikke allerede er valgt i kolonneTal
            // og ikke dubletter på tværs af pladen.
            HashSet<int> usedNumbers = new HashSet<int>();
            foreach (var colList in kolonneTal)
            {
                foreach (var num in colList)
                {
                    usedNumbers.Add(num);
                }
            }

            List<int> possible = new List<int>();
            for (int i = start; i <= end; i++)
            {
                if (!usedNumbers.Contains(i))
                    possible.Add(i);
            }

            if (possible.Count == 0) return 0; // Ikke muligt at finde et ledigt tal

            return possible[rand.Next(possible.Count)];
        }
    }
}