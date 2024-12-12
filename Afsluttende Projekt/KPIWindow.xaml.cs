using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Afsluttende_Projekt
{
    public partial class KPIWindow : Window
    {
        private List<KPIData> kpiData;
        private KPIDataAccess dataAccess;

        public KPIWindow()
        {
            InitializeComponent();


            if (KPIChart == null)
            {
                MessageBox.Show("KPIChart er ikke initialiseret korrekt.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            dataAccess = new KPIDataAccess();
            kpiData = dataAccess.HentKPIData();
            KPIGrid.ItemsSource = kpiData;
            VisualiserGrafer("Søjlediagram");
        }

        private void SortBedstTilVærst_Click(object sender, RoutedEventArgs e)
        {
            kpiData = CustomSort(kpiData, true);
            KPIGrid.ItemsSource = null;
            KPIGrid.ItemsSource = kpiData;
        }

        private void SortVærstTilBedst_Click(object sender, RoutedEventArgs e)
        {
            kpiData = CustomSort(kpiData, false);
            KPIGrid.ItemsSource = null;
            KPIGrid.ItemsSource = kpiData;
        }

        private List<KPIData> CustomSort(List<KPIData> data, bool descending)
        {
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = i + 1; j < data.Count; j++)
                {
                    bool condition = descending
                        ? data[i].Præcision < data[j].Præcision
                        : data[i].Præcision > data[j].Præcision;

                    if (condition)
                    {
                        var temp = data[i];
                        data[i] = data[j];
                        data[j] = temp;
                    }
                }
            }
            return data;
        }

        private void VisualiserGrafer(string type)
        {
            // Null-check for at sikre, at KPIChart er korrekt initialiseret
            if (KPIChart == null)
            {
                MessageBox.Show("KPIChart er ikke initialiseret korrekt. Kontroller XAML-definitionen.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Rydder eksisterende graf
            KPIChart.Children.Clear();

            if (type == "Søjlediagram")
            {
                int width = 40;
                int spacing = 10;
                for (int i = 0; i < kpiData.Count; i++)
                {
                    var bar = new Rectangle
                    {
                        Width = width,
                        Height = kpiData[i].Præcision * 3, // Skaleringsfaktor for visning
                        Fill = Brushes.Blue
                    };
                    Canvas.SetLeft(bar, i * (width + spacing));
                    Canvas.SetBottom(bar, 0);
                    KPIChart.Children.Add(bar);
                }
            }
            else if (type == "Linjediagram")
            {
                for (int i = 0; i < kpiData.Count - 1; i++)
                {
                    var line = new Line
                    {
                        X1 = i * 60,
                        Y1 = 300 - kpiData[i].Præcision * 3, // Skaleringsfaktor for linjediagram
                        X2 = (i + 1) * 60,
                        Y2 = 300 - kpiData[i + 1].Præcision * 3,
                        Stroke = Brushes.Green,
                        StrokeThickness = 2
                    };
                    KPIChart.Children.Add(line);
                }
            }
            else if (type == "Cirkeldiagram")
            {
                // Placeholder for cirkeldiagram - implementér, hvis nødvendigt
                MessageBox.Show("Cirkeldiagram ikke implementeret endnu.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void cmbGrafter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (cmbGrafter.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selected != null)
            {
                VisualiserGrafer(selected);
            }
            else
            {
                MessageBox.Show("Kunne ikke hente den valgte graftype.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
