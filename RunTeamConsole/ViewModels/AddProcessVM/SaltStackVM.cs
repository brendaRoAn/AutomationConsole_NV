using RunTeamConsole.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RunTeamConsole.ViewModels
{
    public partial class AddProcessViewModel : ObservableObject
    {
        private bool _saltConnectivityCheckboxIsChecked;
        private SaltMaster _selectedMasterServer;
        public SaltMaster SelectedMasterServer
        {
            get
            {
                return this._selectedMasterServer;
            }
            set
            {
                this._selectedMasterServer = value;
                this.OnPropertyChanged("SelectedMasterServer");
            }
        }

        public bool SaltConnectivityCheckboxIsChecked 
        { 
            get
            {
                return this._saltConnectivityCheckboxIsChecked;
            }
            set
            {
                this._saltConnectivityCheckboxIsChecked = value;
                this.OnPropertyChanged("SaltConnectivityCheckboxIsChecked");
            }
        }
    }
}
