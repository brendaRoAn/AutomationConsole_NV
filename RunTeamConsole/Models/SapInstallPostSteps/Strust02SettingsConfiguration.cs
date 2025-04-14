using Newtonsoft.Json;
using System.Collections.Generic;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class Strust02SettingsConfiguration : ObservableObject
    {
        private string _certificateType, _certificatePath, _certificateName;
        private bool _isSelected;
        public string CertificateType
        { 
            get { return _certificateType; }
            set {  _certificateType = value; }
        }
        public string CertificatePath
        {
            get { return _certificatePath; }
            set { _certificatePath = value; }
        }
        public string CertificateName
        {
            get { return _certificateName; }
            set { _certificateName = value; }
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