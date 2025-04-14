using Newtonsoft.Json;
using System.Collections.Generic;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class Sm36SettingsConfiguration : ObservableObject
    {
        private string _sapUser, _sapPassword;
        
        public string SapUser
        {
            get { return _sapUser; }
            set { _sapUser = value; }
        }
        public string SapPassword
        {
            get { return _sapPassword; }
            set { _sapPassword = value; }
        }
    }
}