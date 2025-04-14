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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RunTeamConsole.ViewModels;

namespace RunTeamConsole.Views.SapInstallPostSteps
{
    /// <summary>
    /// Interaction logic for SapInstallSTRUST02.xaml
    /// </summary>
    public partial class SapInstallSTRUST02 : UserControl
    {
        public static string fullCertificateName, fileCertificateName;
        public SapInstallSTRUST02()
        {
            InitializeComponent();
        }

        private void UploadFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileToSave = new Microsoft.Win32.OpenFileDialog();

            bool? response;

            //This code says what kind of file we are going to look for ext and filter ir to the user, it is commented because we are waiting the needed ext type
            //ofd.DefaultExt = ".png";
            //In this code we define the text that will be showed in the ext bar for the user, it is commented because we are waiting the needed ext type
            //ofd.Filter = "Image file (*.png) | *.png";

            response = fileToSave.ShowDialog();

            if (response == true)
            {
                fullCertificateName = fileToSave.FileName;
                fileCertificateName = fileToSave.SafeFileName;
                CertificateNameTextBox.Text = fullCertificateName;
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Download completed!");
        }
    }
}
