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
    /// Interaction logic for SapInstallST22.xaml
    /// </summary>
    public partial class SapInstallST22 : UserControl
    {
        public SapInstallST22()
        {
            InitializeComponent();
            int[] hourComboItems = new[]
            {
                0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23
            };
            int[] minuteComboItems = new int[12];
            for (int i = 0; i < 12; i++)
            {
                minuteComboItems[i] = i * 5;
            }
            St22FromHourComboBox.ItemsSource = hourComboItems;
            St22FromMinuteComboBox.ItemsSource = minuteComboItems;
            St22ToHourComboBox.ItemsSource= hourComboItems;
            St22ToMinuteComboBox.ItemsSource = minuteComboItems;
        }
    }
}
