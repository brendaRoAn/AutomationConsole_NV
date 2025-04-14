using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace RunTeamConsole.Views.Principal.Windows
{
    /// <summary>
    /// Interaction logic for OptionsMessageWindow.xaml
    /// </summary>
    public partial class OptionsMessageWindow : Window
    {
        public bool allowClosing = false;

        public OptionsMessageWindow(string idx, string title, string body, string tooltip, string continueButtonText, string repeatButtonText, string alternButtonText, string windowTitle = "Automation Console")
        {
            string _title = Application.Current.FindResource("strTitle").ToString() + "-" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            MainWindow.PVMInstance.DisplayedMessages++;

            Closing += OnWindowClosing;

            InitializeComponent();
            this.Title = windowTitle;

            TextBox idxTexBox = this.FindName("idxTextBox") as TextBox;
            idxTexBox.Text = idx;

            Label msgTitle = this.FindName("title") as Label;
            msgTitle.Content = title.Replace("\\n", "\n");
            Label msgContent = this.FindName("body") as Label;
            msgContent.Content = body.Replace("\\n", "\n");
            Label msgTooltip = this.FindName("tooltip") as Label;
            msgTooltip.Content = tooltip.Replace("\\n", "\n");

            Button continueButton = this.FindName("continueButton") as Button;
            if (String.IsNullOrEmpty(continueButtonText))
                continueButton.Visibility = Visibility.Collapsed;
            else
                continueButton.Content = continueButtonText.Replace("\\n", "\n");
            
            Button repeatButton = this.FindName("repeatButton") as Button;
            if (String.IsNullOrEmpty(repeatButtonText))
                repeatButton.Visibility = Visibility.Collapsed;
            else
                repeatButton.Content = repeatButtonText.Replace("\\n", "\n");
            
            Button alternButton = this.FindName("alternButton") as Button;
            if (String.IsNullOrEmpty(alternButtonText))
                alternButton.Visibility = Visibility.Collapsed;
            else
                alternButton.Content = alternButtonText.Replace("\\n", "\n");
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (allowClosing)
                MainWindow.PVMInstance.DisplayedMessages--;
            else
                e.Cancel = true;
        }

    }
}
