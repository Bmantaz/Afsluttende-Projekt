using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Afsluttende_Projekt
{
    public partial class BankoWindow : Window
    {
        public BankoWindow()
        {
            InitializeComponent();
            Loaded += BankoWindow_Loaded;
        }

        private void BankoWindow_Loaded(object sender, RoutedEventArgs e)
        {
            BankoGenerator gen = new BankoGenerator();
            List<int[,]> boards = gen.Generate3Boards();

            // boards er en liste med 3 plader, hver 3x9
            // Vi viser hver plade i et DataGrid
            VisPlade(dgPlade1, boards[0]);
            VisPlade(dgPlade2, boards[1]);
            VisPlade(dgPlade3, boards[2]);
        }

        private void VisPlade(DataGrid dg, int[,] plade)
        {
            // Omdan pladen til en DataTable for nem visning i DataGrid
            DataTable dt = new DataTable();
            for (int c = 0; c < 9; c++)
            {
                dt.Columns.Add($"Col{c + 1}", typeof(string));
            }

            for (int r = 0; r < 3; r++)
            {
                DataRow row = dt.NewRow();
                for (int c = 0; c < 9; c++)
                {
                    int val = plade[r, c];
                    row[c] = val == 0 ? "" : val.ToString();
                }
                dt.Rows.Add(row);
            }

            dg.ItemsSource = dt.DefaultView;
        }
    }
}