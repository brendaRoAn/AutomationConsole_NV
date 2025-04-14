using RunTeamConsole.Components;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RunTeamConsole.Views
{
    /// <summary>
    /// Interaction logic for FavoritesSummary.xaml
    /// </summary>
    public partial class FavoritesSummary : UserControl
    {
        public FavoritesSummary()
        {
            InitializeComponent();
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
                password.Visibility = Visibility.Collapsed;
                textbox.Visibility = Visibility.Visible;
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
                password.Visibility = Visibility.Visible;
                textbox.Visibility = Visibility.Collapsed;
            }
        }
    }
}
