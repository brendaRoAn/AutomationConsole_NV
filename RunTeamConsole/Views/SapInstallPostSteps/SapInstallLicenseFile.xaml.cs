
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using RunTeamConsole.Models.SapInstallPostSteps;
using RunTeamConsole.Models;
using System.Diagnostics;
using RunTeamConsole.ViewModels;

namespace RunTeamConsole.Views.SapInstallPostSteps
{
    /// <summary>
    /// Interaction logic for SapInstallLicenseFile.xaml
    /// </summary>
    public partial class SapInstallLicenseFile : UserControl
    {
        public static string fullName, fileName;
        public SapInstallLicenseFile()
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
                fullName = fileToSave.FileName;
                fileName = fileToSave.SafeFileName;
                FileNameTextBox.Text = fullName;
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Download completed!");
        }
    }
}
