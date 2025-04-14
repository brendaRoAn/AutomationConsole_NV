using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RunTeamConsole.Views.SapInstallPostSteps
{
    /// <summary>
    /// Interaction logic for SapInstallSMLG.xaml
    /// </summary>
    public partial class SapInstallSMLG : UserControl
    {
        public SapInstallSMLG()
        {
            InitializeComponent();

            string[] rfcTypeOptions = new string[]
            {
                "R - Round Robin",
                "B - Best Performance",
                "W - Weighted Round Robin"
            };
            SmlgRfcTypeCombobox.ItemsSource = rfcTypeOptions;
            SmlgRfcTypeCombobox.SelectedItem = rfcTypeOptions[0];
        }
    }
}
