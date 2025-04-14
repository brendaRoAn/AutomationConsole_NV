using Newtonsoft.Json;
using System.Collections.Generic;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class Sm61SettingsConfiguration : ObservableObject
    {
        private string _groupName, _instance;
        bool _isSelected;

        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }
        public string Instance
        {
            get { return _instance; }
            set {  _instance = value; }
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