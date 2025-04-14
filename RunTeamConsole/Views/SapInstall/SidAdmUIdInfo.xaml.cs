using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static System.Net.Mime.MediaTypeNames;

namespace RunTeamConsole.Views
{
    /// <summary>
    /// Interaction logic for SidAdmUIdInfo.xaml
    /// </summary>
    public partial class SidAdmUIdInfo : Window
    {
        string wikiLink = "http://wiki.fit.freudenberg.de/doku.php?id=teamad:sap:checklists:sap_system_install";
        public SidAdmUIdInfo()
        {
            InitializeComponent();
        }

        private void CopySidAdmUIdLink_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(wikiLink);
            MessageBox.Show(("The hyperlink is already pasted to your clipboard."));
        }

        private void OkSidAdmUIdInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = wikiLink,
                UseShellExecute = true
            };

            Process.Start(psi);
        }
    }
}
