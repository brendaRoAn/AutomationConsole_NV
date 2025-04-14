using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class SmlgSettingsConfiguration : ObservableObject
    {
        private string _customerName, _instanceGroup, _rfcType, _ipGroup;
        private bool _rfcEnabled, _isSelected;
        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; }
        }
        public string InstanceGroup
        {
            get { return _instanceGroup; }
            set { _instanceGroup = value; }
        }
        public string IpGroup
        {
            get { return _ipGroup; }
            set { _ipGroup = value; }
        }
        public bool RfcEnabled
        {
            get { return _rfcEnabled; }
            set { _rfcEnabled = value;}
        }
        public string RfcType
        {
            get { return _rfcType; }
            set { _rfcType = value;}
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