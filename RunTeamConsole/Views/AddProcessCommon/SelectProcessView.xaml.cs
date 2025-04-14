using System.Windows.Controls;
using System.Diagnostics;
using System.Windows;

namespace RunTeamConsole.Views
{
    public partial class SelectProcessView : UserControl
    {
        public SelectProcessView()
        {
            InitializeComponent();
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {//Training/Indications/ORACLE%20Patching%20(v1.0)
            var psi = new ProcessStartInfo
            {
                FileName = Auxiliar.serverURL + "Training/Certifications",
                UseShellExecute = true
            };

            System.Diagnostics.Process.Start(psi);
        }

    }
}
