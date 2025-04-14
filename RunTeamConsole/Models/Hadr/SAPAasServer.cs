using Newtonsoft.Json;
using System.Collections.Generic;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class SAPAasServer : ObservableObject
    {
        private List<ServerSystem> _sapAasServer;
        
        public List<ServerSystem> SapAasServer
        {
            get { return _sapAasServer; }
            set { _sapAasServer = value; }
        }
    }
}