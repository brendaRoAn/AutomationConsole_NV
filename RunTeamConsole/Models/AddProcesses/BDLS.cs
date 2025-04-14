namespace RunTeamConsole.Models.AddProcesses
{
    public class BDLS : ObservableObject
    {
        private string _sourceClient;
        private string _sourceSID;
        private string _targetSID;
        private string _targetClient;
        private bool _isSelected;

        public string SourceClient
        {
            get { return _sourceClient; }
            set { _sourceClient = value; }
        }
        public string SourceSID
        {
            get { return _sourceSID; }
            set { _sourceSID = value; }
        }

        public string TargetClient
        {
            get { return _targetClient; }
            set { _targetClient = value; }
        }
        public string TargetSID
        {
            get { return _targetSID; }
            set { _targetSID = value; }
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
