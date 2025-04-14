using Newtonsoft.Json;
using System.Collections.Generic;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class Rz10SettingsConfiguration : ObservableObject
    {
        private List<Rz10FqicpSettings> _rz10FqicpSettings = new List<Rz10FqicpSettings>();
        private List<Rz10AddpSettings> _rz10AddpSettings;

        public List<Rz10FqicpSettings> Rz10FqicpSettingsList
        {
            get { return _rz10FqicpSettings; }
            set
            {
                if(_rz10FqicpSettings == null && Rz10FqicpSettingsList == null)
                    this._rz10FqicpSettings = value;
                //Rz10FqicpSettingsList.Add(_rz10FqicpSettings);
            }
        }
        public Rz10SettingsConfiguration(string? fqicpName, string? fqicpValue, string? addpName, string? addpValue)
        {
            if(fqicpName != null && fqicpValue != null)
            {
                if(Rz10FqicpSettingsList == null)
                    _rz10FqicpSettings = new List<Rz10FqicpSettings>();
                AddFqicpSettings(fqicpName, fqicpValue);
            }
        }
        public void AddFqicpSettings(string fqicpName, string fqicpValue)
        {
            Rz10FqicpSettings settings = new Rz10FqicpSettings(fqicpName, fqicpValue);
            _rz10FqicpSettings.Add(settings);
        }
        public void AddAddpSettings(string addpName, string addpValue)
        {
            Rz10AddpSettings settings = new Rz10AddpSettings(addpName, addpValue);
            _rz10AddpSettings.Add(settings);
        }
        public class Rz10FqicpSettings : ObservableObject
        {
            private bool _osMatch;
            public Rz10FqicpSettings(string fqicpName, string fqicpValue)
            {
                FqicpName = fqicpName;
                FqicpValue = fqicpValue;
            }
            public string FqicpName { get; set; }
            public string FqicpValue { get; set; }
            public bool OSMatch
            {
                get { return this._osMatch; }
                set
                {
                    this._osMatch = value;
                    this.OnPropertyChanged("OSMatch");
                }
            }
        }
        public class Rz10AddpSettings : ObservableObject
        {
            private bool _osMatch;
            public Rz10AddpSettings(string addpName, string addpValue)
            {
                AddpName = addpName;
                AddpValue = addpValue;
                OSMatch = true;
            }
            public string AddpName { get; set; }
            public string AddpValue { get; set; }
            public bool OSMatch
            {
                get { return this._osMatch; }
                set
                {
                    this._osMatch = value;
                    this.OnPropertyChanged("OSMatch");
                }
            }
        }
    }
}