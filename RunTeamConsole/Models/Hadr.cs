using Newtonsoft.Json;
using RunTeamConsole.Views.StartSapHadr;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;

namespace RunTeamConsole.Models
{
    public class Hadr : ObservableObject
    {
        #region HADR START SAP Configuration
        private string _sapsaPassword, _disRecUser, _disRecPass;
        private ServerSystem _primaryDBServer, _standbyDBServer, _acscScsServer, _ersServer;
        ObservableCollection<ServerSystem> _aasServer;

        public string SapsaPassword
        {
            get
            {
                return this._sapsaPassword;
            }
            set
            {
                this._sapsaPassword = value;
                this.OnPropertyChanged("SapsaPassword");
            }
        }
        public string DisasterRecoveryUser
        {
            get
            {
                return this._disRecUser;
            }
            set
            {
                this._disRecUser = value;
                this.OnPropertyChanged("DisasterRecoveryUser");
            }
        }
        public string DisasterRecoveryPassword
        {
            get
            {
                return this._disRecPass;
            }
            set
            {
                this._disRecPass = value;
                this.OnPropertyChanged("DisasterRecoveryPassword");
            }
        }
        public ServerSystem PrimaryDBServer
        {
            get 
            { 
                return this._primaryDBServer;
            }
            set 
            {
                this._primaryDBServer = value;
                this.OnPropertyChanged("PrimaryDBServer");
            }
        }
        public ServerSystem StandbyDBServer
        {
            get
            {
                return this._standbyDBServer;
            }
            set
            {
                this._standbyDBServer = value;
                this.OnPropertyChanged("StandbyDBServer");
            }
        }
        public ServerSystem AcscScsServer
        {
            get
            {
                return this._acscScsServer;
            }
            set
            {
                this._acscScsServer = value;
                this.OnPropertyChanged("AcscScsServer");
            }
        }
        public ServerSystem ErsServer
        {
            get
            {
                return this._ersServer;
            }
            set
            {
                this._ersServer = value;
                this.OnPropertyChanged("ErsServer");
            }
        }
        public ObservableCollection<ServerSystem> AasServer
        {
            get
            {
                return this._aasServer;
            }
            set
            {
                this._aasServer = value;
                this.OnPropertyChanged("AasServer");
            }
        }
        #endregion
    }
}