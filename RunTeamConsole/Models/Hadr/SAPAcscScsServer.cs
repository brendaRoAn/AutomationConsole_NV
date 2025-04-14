using Newtonsoft.Json;
using System.Collections.Generic;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class SAPAcscScsServer : ObservableObject
    {
        private ServerSystem _sapAcscScsServerServer;
        
        public ServerSystem SapAcscScsServerServer
        {
            get { return _sapAcscScsServerServer; }
            set { _sapAcscScsServerServer = value; }
        }
    }
}