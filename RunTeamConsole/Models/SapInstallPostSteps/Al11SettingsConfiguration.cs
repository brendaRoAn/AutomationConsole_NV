using Newtonsoft.Json;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class Al11SettingsConfiguration : ObservableObject
    {
        private string _directoryPath, _directoryName, _validForServer;
        private bool _isSelected;
        public string DirectoryPath
        {
            get { return _directoryPath;}
            set { _directoryPath = value; }
        }
        public string DirectoryName
        {
            get { return _directoryName;} 
            set {  _directoryName = value; }
        }
        public string ValidForServer
        {
            get { return _validForServer;}
            set { _validForServer = value; }
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