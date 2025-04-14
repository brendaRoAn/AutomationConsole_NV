using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Controls;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class Rz10FqicpSettingsConfiguration : ObservableObject
    {
        private string _fqicpName, _fqicpValue;
        private bool _isSelected;

        public string FqicpName
        {
            get { return _fqicpName; }
            set { _fqicpName = value; }
        }

        public string FqicpValue
        {
            get { return _fqicpValue; }
            set { _fqicpValue = value; }
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