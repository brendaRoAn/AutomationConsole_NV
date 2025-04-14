using Newtonsoft.Json;
using System.Collections.Generic;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class PrimaryDBServer : ObservableObject
    {
        private ServerSystem _primaryDbServer;
        
        public ServerSystem PrimaryDbServer
        {
            get { return _primaryDbServer; }
            set { _primaryDbServer = value; }
        }
    }
}