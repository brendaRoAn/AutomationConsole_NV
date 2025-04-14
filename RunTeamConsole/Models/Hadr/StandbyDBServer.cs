using Newtonsoft.Json;
using System.Collections.Generic;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class StandbyDBServer : ObservableObject
    {
        private ServerSystem _standbyDbServer;
        
        public ServerSystem StandbyDbServer
        {
            get { return _standbyDbServer; }
            set { _standbyDbServer = value; }
        }
    }
}