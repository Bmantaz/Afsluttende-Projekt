using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Afsluttende_Projekt
{
    public partial class CEOLoginWindow : Window
    {
        public bool AccessGranted { get; private set; } = false;
        private const string CorrectCode = "1234"; // Kan ændres efter behov

        public CEOLoginWindow()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (txtCEOCode.Password.Trim() == CorrectCode)
            {
                AccessGranted = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Forkert kode. Prøv igen.", "Adgang nægtet", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}