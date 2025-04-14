using Newtonsoft.Json.Linq;
using RunTeamConsole.Components;
using RunTeamConsole.Models;
using RunTeamConsole.Models.Packages;
using RunTeamConsole.ViewModels;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using static System.Net.WebRequestMethods;

namespace RunTeamConsole.Views.SapInstallPostSteps
{
    /// <summary>
    /// Interaction logic for SAPData.xaml
    /// </summary>
    public partial class SapOracleData : UserControl
    {
        string selction;
        public SapOracleData()
        {
            InitializeComponent();
        }
        /*
        private void RButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton li = (sender as RadioButton);

            li.Content.ToString();
        }*/
    }
}
