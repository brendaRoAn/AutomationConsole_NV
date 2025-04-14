using Newtonsoft.Json;
using System.Collections.Generic;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class Rz70SettingsConfiguration : ObservableObject
    {
        private string _gatewayHost, _gatewayService;
        private bool _isSelected;
        public string GatewayHost
        {
            get { return _gatewayHost; } 
            set { _gatewayHost = value;}
        }
        public string GatewayService
        {
            get { return _gatewayService; }
            set { _gatewayService = value;}
        }
        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                this._isSelected = value;
                this.OnPropertyChanged("IsSelected");
            }
        }

    }
}