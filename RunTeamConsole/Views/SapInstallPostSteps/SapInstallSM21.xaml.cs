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
    /// Interaction logic for SapInstallSM21.xaml
    /// </summary>
    public partial class SapInstallSM21 : UserControl
    {
        public SapInstallSM21()
        {
            InitializeComponent();
            int[] HourComboItems = new[]
            {
                0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23
            };
            int[] minuteComboItems = new int[12];
            for (int i = 0; i < 12; i++)
            {
                minuteComboItems[i] = i * 5;
            }
            Sm21FromHourCombobox.ItemsSource = HourComboItems;
            /*if(Sm21ToHourCombobox.Items.Count != 0)
                Sm21ToHourCombobox.Items.RemoveAt(0);
            Sm21ToHourCombobox.ItemsSource = HourComboItems;*/
            Sm21FromMinuteCombobox.ItemsSource = minuteComboItems;
            //Sm21ToMinuteCombobox.ItemsSource = minuteComboItems;
        }
    }
}
