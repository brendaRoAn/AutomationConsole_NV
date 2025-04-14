using RunTeamConsole.Components;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace RunTeamConsole.Views.Refresh
{
    /// <summary>
    /// Interaction logic for AdditionalSettings.xaml
    /// </summary>
    public partial class AdditionalSettings : UserControl
    {
        public AdditionalSettings()
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
            SourceBackupTimeHourCombobox.ItemsSource = hourComboItems;
            SourceBackupTimeMinuteCombobox.ItemsSource = minuteComboItems;

            int[] cvStreamsComboItems = new[]
            {
                4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40
            };
            SourceBackupCVCombobox.ItemsSource = cvStreamsComboItems;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid parent = (Grid)(sender as Button).Parent;
            int index = parent.Children.IndexOf(sender as Button);
            TextBox textbox = null;
            BindablePasswordBox password = null;
            foreach (var el in parent.Children)
            {
                if (el.GetType() == typeof(TextBox))
                {
                    textbox = (TextBox)el;
                }
                else if (el.GetType() == typeof(BindablePasswordBox))
                {
                    password = (BindablePasswordBox)el;
                }
            }
            if (textbox != null && password != null)
            {
                password.Visibility = System.Windows.Visibility.Collapsed;
                textbox.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Grid parent = (Grid)(sender as Button).Parent;
            int index = parent.Children.IndexOf(sender as Button);
            TextBox textbox = null;
            BindablePasswordBox password = null;
            foreach (var el in parent.Children)
            {
                if (el.GetType() == typeof(TextBox))
                {
                    textbox = (TextBox)el;
                }
                else if (el.GetType() == typeof(BindablePasswordBox))
                {
                    password = (BindablePasswordBox)el;
                }
            }
            if (textbox != null && password != null)
            {
                password.Visibility = System.Windows.Visibility.Visible;
                textbox.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
