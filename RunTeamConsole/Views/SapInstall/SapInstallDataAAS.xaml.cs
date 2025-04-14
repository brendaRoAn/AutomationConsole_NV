using Newtonsoft.Json.Linq;
using RunTeamConsole.Components;
using RunTeamConsole.Models;
using RunTeamConsole.Models.Packages;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using static System.Net.WebRequestMethods;

namespace RunTeamConsole.Views.SapInstall
{
    /// <summary>
    /// Interaction logic for SAPData.xaml
    /// </summary>
    public partial class SapInstallDataAAS : UserControl
    {
        string ascsInstNum;

        public SapInstallDataAAS()
        {
            InitializeComponent();

            bool[] setDomainComboItems = new[]
            {
                true, false
            };
            SourceSetDomainCombobox.ItemsSource = setDomainComboItems;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text.Length < 5)
            {
                MessageBox.Show("You need to write at least 10 characters");
            }
        }

        private void PasswordValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9][a-z][A-Z]+");
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

        private void SidAdmUIdInfo_Click(object sender, RoutedEventArgs e)
        {
            SidAdmUIdInfo sidAdmUIdInfo = new SidAdmUIdInfo();
            sidAdmUIdInfo.Show();
        }
    }
}
