using RunTeamConsole.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using RunTeamConsole.Models.Packages;
using RunTeamConsole.ViewModels.Commands;
using System.Windows.Input;

namespace RunTeamConsole.ViewModels
{
    public partial class AddProcessViewModel : ObservableObject
    {
        public void RequestUser()
        {
            this.OkCommand = new RelayCommand(this.OkCommandExecuted);
            this.OkCommand = new RelayCommand(this.CancelCommandExecuted);
        }

        public bool? DialogResultValue { get; set; }

        private void CancelCommandExecuted(object obj)
        {
            this.DialogResultValue = false;
            this.CloseAction();
        }

        private void OkCommandExecuted(object obj)
        {
            this.DialogResultValue = true;
            this.CloseAction();
        }

        private string userText;

        public string UserText
        {
            get => userText;
            set
            {
                //userText = value;
                // Prism
                /*SetProperty(ref this.userText, value);*/
            }
        }

        public Action CloseAction { get; set; }

        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }


    }
}