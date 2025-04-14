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

namespace RunTeamConsole.Views.SapInstallPostSteps
{
    /// <summary>
    /// Interaction logic for SapInstallStepsToExecute.xaml
    /// </summary>
    public partial class SapInstallStepsToExecute : UserControl
    {
        public static bool? rz10Checked = false;
        public SapInstallStepsToExecute()
        {
            InitializeComponent();
        }
    }
}
