using Newtonsoft.Json;
using System.Collections.Generic;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class SAPErsServer : ObservableObject
    {
        private ServerSystem _sapErsServer;
        
        public ServerSystem SapErsServer
        {
            get { return _sapErsServer; }
            set { _sapErsServer = value; }
        }
    }
}