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

namespace RunTeamConsole.Views
{
    /// <summary>
    /// Interaction logic for EmailDestinationsView.xaml
    /// </summary>
    public partial class EmailDestinationsView : UserControl
    {
        public ListBoxItem itm = new ListBoxItem();
        public EmailDestinationsView()
        {
            InitializeComponent();

            /*THIS WORKS
            ListBoxItem itm = new ListBoxItem();
            itm.Content = "some text";
            EmailList.Items.Add(itm);*/
        }

        private void AddEmails(object sender, RoutedEventArgs e)
        {
            EmailList.Items.Add(NewEmail.Text);
            NewEmail.Text = "";
        }
}
}
