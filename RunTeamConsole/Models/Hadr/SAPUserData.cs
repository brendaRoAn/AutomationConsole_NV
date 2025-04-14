using Newtonsoft.Json;
using System.Collections.Generic;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class SAPUserData : ObservableObject
    {
        private string _sapsaPassword, _disRecUser, _disRecovPassword;
        
        public string SapsaPassword
        {
            get { return _sapsaPassword; }
            set { _sapsaPassword = value; }
        }
        public string DisRecUser
        {
            get { return _disRecUser; }
            set { _disRecUser = value; }
        }

        public string DisRecPassword
        {
            get { return _disRecovPassword; }
            set { _disRecovPassword = value; }
        }
    }
}