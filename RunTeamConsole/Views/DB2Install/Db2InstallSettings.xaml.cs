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

namespace RunTeamConsole.Views.DB2Install
{
    /// <summary>
    /// Interaction logic for Db2InstallSettings.xaml
    /// </summary>
    public partial class Db2InstallSettings : UserControl
    {
        public Db2InstallSettings()
        {
            InitializeComponent();

            string[] Pacemaker = new[]
            {
                "Yes",
                "No"
            };
            PacemakerCombobox.ItemsSource = Pacemaker;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
