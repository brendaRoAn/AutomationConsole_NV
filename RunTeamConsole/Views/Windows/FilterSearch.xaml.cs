using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RunTeamConsole.Views.Windows
{
    /// <summary>
    /// Interaction logic for FilterSearch.xaml
    /// </summary>
    public partial class FilterSearch : Window
    {
        public FilterSearch()
        {
            InitializeComponent();
            Closing += OnWindowClosing;

        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            MainWindow.PVMInstance.filterWindow = null;
        }

    }
}
