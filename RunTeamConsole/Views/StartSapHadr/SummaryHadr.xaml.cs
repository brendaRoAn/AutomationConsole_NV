using RunTeamConsole.Components;
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

namespace RunTeamConsole.Views.StartSapHadr
{
    /// <summary>
    /// Interaction logic for SummaryHadr.xaml
    /// </summary>
    public partial class SummaryHadr : UserControl
    {
        public SummaryHadr()
        {
            InitializeComponent();
        }
        private void Button_MouseDownSapsa(object sender, MouseButtonEventArgs e)
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
        private void Button_MouseUpSapsa(object sender, MouseButtonEventArgs e)
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

        private void Button_MouseDownUsrDisRec(object sender, MouseButtonEventArgs e)
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
        private void Button_MouseUpUsrDisRec(object sender, MouseButtonEventArgs e)
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
