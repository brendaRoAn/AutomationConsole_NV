using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace RunTeamConsole.Views.Refresh
{
    /// <summary>
    /// Interaction logic for BDLSView.xaml
    /// </summary>
    public partial class BDLSView : UserControl
    {
        public BDLSView()
        {
            InitializeComponent();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
