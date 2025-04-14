using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RunTeamConsole.Views.Windows
{
    /// <summary>
    /// Interaction logic for MessagePopup.xaml
    /// </summary>
    public partial class MessagePopup : Window
    {

        public bool allowClosing = false;
        public MessagePopup(string message, string status, string title = "Automation Console")
        {
            string selectedFileName;
            string _title = Application.Current.FindResource("strTitle").ToString() + "-" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            MainWindow.PVMInstance.DisplayedMessages++;

            Closing += OnWindowClosing;

            InitializeComponent();
            this.Title = title;

            TextBlock msgContent = this.FindName("msgContent") as TextBlock;
            msgContent.Text = message.Replace("\\n", "\n");

            Image imgStatus = this.FindName("imgStatus") as Image; 
            switch (status) { 
                case "DONE":
                case "COMPLETED":
                    selectedFileName = "\\img\\icons\\info.png";
                    break;
                case "WARNING":
                    selectedFileName = "\\img\\icons\\warning.png";
                    break;
                case "empty":
                case "":
                    selectedFileName = "\\img\\icons\\info-gray.png";
                    break;
                default:
                    selectedFileName = "\\img\\icons\\delete.png";
                    break;
            }
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(selectedFileName, UriKind.Relative);
            bitmap.EndInit();
            imgStatus.Source = bitmap;
            this.SizeToContent = SizeToContent.WidthAndHeight;
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if(allowClosing)
                MainWindow.PVMInstance.DisplayedMessages--;
            else
                e.Cancel = true;
        }

    }
}
