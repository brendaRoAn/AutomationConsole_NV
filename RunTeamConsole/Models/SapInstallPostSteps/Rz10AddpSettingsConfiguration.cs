using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Controls;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class Rz10AddpSettingsConfiguration : ObservableObject
    {
        private string _addpName, _addpValue;
        private bool _isSelected;

        public string AddpName
        {
            get { return _addpName; }
            set { _addpName = value; }
        }

        public string AddpValue
        {
            get { return _addpValue; }
            set { _addpValue = value; }
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