using RunTeamConsole.Models;
using RunTeamConsole.Views;
using RunTeamConsole.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Diagnostics;
using Process = RunTeamConsole.Models.Process;
using RunTeamConsole.Views.Windows;
using RunTeamConsole.Views.Principal.Windows;
using System.Net;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography.Xml;
using System.IO.Packaging;

namespace RunTeamConsole.ViewModels
{
    public class AppViewModel : ObservableObject
    {
        private string _itUser, _password;

        public string ItUSer
        {
            get { return this._itUser; }
            set
            {
                this._itUser = value;
                this.OnPropertyChanged("ItUser");
            }
        }
        public string Password
        {
            get { return this._password; }
            set
            {
                this._password = value;
                this.OnPropertyChanged("Password");
            }
        }
        public RelayCommand RunAutomationConsoleCommand { get; private set; }

        public AppViewModel()
        {
            RunAutomationConsoleCommand = new RelayCommand(OkClickPopup, AlwwaysCanExecute);
        }
        public void OkClickPopup(object obj)
        {
            MessagePopup messagePopup = obj as MessagePopup;
            messagePopup.allowClosing = true;
            messagePopup.Close();
        }
        public bool AlwwaysCanExecute(object obj) { return true; }
    }
}