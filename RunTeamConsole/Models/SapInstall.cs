using Newtonsoft.Json;

namespace RunTeamConsole.Models
{
    public class SapInstall : ObservableObject
    {
        //Classic SAP Install
        private string _sapSId, _hanaDbName, _dbScehmaName, _masterPass, _virtualHost, _virtHostInter, _domainName, _ascsInstNum, _pasInstNum, _hanaInstNum, _sapSysGId, _sapInsGId, _dbSIdAdmGId, _dbSIdAdmUId, _sidAdmUId, _sapAdmUId;
        private bool _setDomain;
        //SAP Install With ORACLE
        private string _sidSapUId, _sapHostname, _sapVirtualHostname, _databaseName, _oraSidGId, _oraSidUId, _oracleListenerPort, _databaseHn, _databaseVirtualHn;
        private string _virtualHostSap;
        private string _sapPasHnm, _sapAasHnm, _sapAasVHnm;

        #region Classic SAP Install Variables
        public string SapSId
        {
            get
            {
                return this._sapSId;
            }
            set
            {
                this._sapSId = value;
                this.OnPropertyChanged("SapSId");
            }
        }
        public string AscsInstNum
        {
            get
            {
                return this._ascsInstNum;
            }
            set
            {
                this._ascsInstNum = value;
                this.OnPropertyChanged("AscsInstNum");
            }
        }
        public string PasInstNum
        {
            get
            {
                return this._pasInstNum;
            }
            set
            {
                this._pasInstNum = value;
                this.OnPropertyChanged("PasInstNum");
            }
        }
        public string HanaDbName
        {
            get
            {
                return this._hanaDbName;
            }
            set
            {
                this._hanaDbName = value;
                this.OnPropertyChanged("HanaDbName");
            }
        }
        public string HanaInstNum
        {
            get
            {
                return this._hanaInstNum;
            }
            set
            {
                this._hanaInstNum = value;
                this.OnPropertyChanged("HanaInstNum");
            }
        }
        public string SapSysGId
        {
            get
            {
                return this._sapSysGId;
            }
            set
            {
                this._sapSysGId = value;
                this.OnPropertyChanged("SapSysGId");
            }
        }
        public string SapInsGId
        {
            get
            {
                return this._sapInsGId;
            }
            set
            {
                this._sapInsGId = value;
                this.OnPropertyChanged("SapInsGId");
            }
        }
        public string DbSIdAdmUId
        {
            get
            {
                return this._dbSIdAdmUId;
            }
            set
            {
                this._dbSIdAdmUId = value;
                this.OnPropertyChanged("DbSIdAdmUId");
            }
        }
        public string DbSIdAdmGId
        {
            get
            {
                return this._dbSIdAdmGId;
            }
            set
            {
                this._dbSIdAdmGId = value;
                this.OnPropertyChanged("DbSIdAdmGId");
            }
        }
        public string SidAdmUId
        {
            get
            {
                return this._sidAdmUId;
            }
            set
            {
                this._sidAdmUId = value;
                this.OnPropertyChanged("SidAdmUId");
            }
        }
        public string SapAdmUId
        {
            get
            {
                return this._sapAdmUId;
            }
            set
            {
                this._sapAdmUId = value;
                this.OnPropertyChanged("SapAdmUId");
            }
        }
        public string DbScehmaName
        {
            get
            {
                return this._dbScehmaName;
            }
            set
            {
                this._dbScehmaName = value;
                this.OnPropertyChanged("DbScehmaName");
            }
        }
        public string MasterPass
        {
            get
            {
                return this._masterPass;
            }
            set
            {
                this._masterPass = value;
                this.OnPropertyChanged("MasterPass");
            }
        }
        public string VirtualHost
        {
            get
            {
                return this._virtualHost;
            }
            set
            {
                this._virtualHost = value;
                this.OnPropertyChanged("VirtualHost");
            }
        }
        public string VirtHostInter
        {
            get
            {
                return this._virtHostInter;
            }
            set
            {
                this._virtHostInter = value;
                this.OnPropertyChanged("VirtHostInter");
            }
        }
        public bool SetDomain
        {
            get
            {
                return this._setDomain;
            }
            set
            {
                this._setDomain = value;
                this.OnPropertyChanged("SetDomain");
            }
        }
        public string DomainName
        {
            get
            {
                return this._domainName;
            }
            set
            {
                this._domainName = value;
                this.OnPropertyChanged("DomainName");
            }
        }
        #endregion

        #region SAP Install with ORACLE Variables
        public string SidSapUId
        {
            get
            {
                return this._sidSapUId;
            }
            set
            {
                this._sidSapUId = value;
                this.OnPropertyChanged("SidSapUId");
            }
        }
        public string SapHostname
        {
            get
            {
                return this._sapHostname;
            }
            set
            {
                this._sapHostname = value;
                this.OnPropertyChanged("SapHostname");
            }
        }
        public string SapVirtualHostname
        {
            get
            {
                return this._sapVirtualHostname;
            }
            set
            {
                this._sapVirtualHostname = value;
                this.OnPropertyChanged("SapVirtualHostname");
            }
        }
        public string DatabaseName
        {
            get
            {
                return this._databaseName;
            }
            set
            {
                this._databaseName = value;
                this.OnPropertyChanged("DatabaseName");
            }
        }
        public string OraSidGId
        {
            get
            {
                return this._oraSidGId;
            }
            set
            {
                this._oraSidGId = value;
                this.OnPropertyChanged("OraSidGId");
            }
        }
        public string OraSidUId
        {
            get
            {
                return this._oraSidUId;
            }
            set
            {
                this._oraSidUId = value;
                this.OnPropertyChanged("OraSidUId");
            }
        }
        public string OracleListenerPort
        {
            get
            {
                return this._oracleListenerPort;
            }
            set
            {
                this._oracleListenerPort = value;
                this.OnPropertyChanged("OracleListenerPort");
            }
        }
        public string DatabaseHn
        {
            get
            {
                return this._databaseHn;
            }
            set
            {
                this._databaseHn = value;
                this.OnPropertyChanged("DatabaseHn");
            }
        }
        public string DatabaseVirtualHn
        {
            get
            {
                return this._databaseVirtualHn;
            }
            set
            {
                this._databaseVirtualHn = value;
                this.OnPropertyChanged("DatabaseVirtualHn");
            }
        }
        #endregion

        #region SAP Install HANA 2 Nodes variables
        public string VirtualHostSap
        {
            get
            {
                return this._virtualHostSap;
            }
            set
            {
                this._virtualHostSap = value;
                this.OnPropertyChanged("VirtualHostSap");
            }
        }
        #endregion

        #region SAP Install Additional Application Server
        public string SapPasHnm
        {
            get
            {
                return this._sapPasHnm;
            }
            set
            {
                this._sapPasHnm = value;
                this.OnPropertyChanged("SapPasHnm");
            }
        }
        public string SapAasHnm
        {
            get
            {
                return this._sapAasHnm;
            }
            set
            {
                this._sapAasHnm = value;
                this.OnPropertyChanged("SapAasHnm");
            }
        }
        public string SapAasVHnm
        {
            get
            {
                return this._sapAasVHnm;
            }
            set
            {
                this._sapAasVHnm = value;
                this.OnPropertyChanged("SapAasVHnm");
            }
        }
        #endregion

        //This is for Classic SAP Install
        [JsonConstructor]
        public SapInstall(string SapSId, string AscsInstNum, string PasInstNum, string HanaDbName, string HanaInstNum, string SapSysGId, string SapInsGId, string DbSIdAdmGId, string DbSIdAdmUId, string SidAdmUId, string SapAdmUId, string DbScehmaName, string MasterPass, string VirtualHost, string VirtHostInter, bool SetDomain, string DomainName)
        {
            this._sapSId = SapSId;
            this._ascsInstNum = AscsInstNum;
            this._pasInstNum = PasInstNum;
            this._hanaDbName = HanaDbName;
            this._hanaInstNum = HanaInstNum;
            this._sapSysGId = SapSysGId;
            this._sapInsGId = SapInsGId;
            this._dbSIdAdmGId = DbSIdAdmGId;
            this._dbSIdAdmUId = DbSIdAdmUId;
            this._sidAdmUId = SidAdmUId;
            this._sapAdmUId = SapAdmUId;
            this._dbScehmaName = DbScehmaName;
            this._masterPass = MasterPass;
            this._virtualHost = VirtualHost;
            this._virtHostInter = VirtHostInter;
            this._setDomain = SetDomain;
            this._domainName = DomainName;

        }

        //This is for SAP Install with ORACLE
        public SapInstall(string SapSId, string AscsInstNum, string PasInstNum, string SapSysGId, string SapInsGId, string SapAdmUId, string SidAdmUId, string SapHn, string SapVHn, string DBName, string OraSidGId, string OraSidUId, string OracleLPort, string DBHn, string DBVirHn, bool SetDomain, string DomainName, string MasterPass)
        {
            this._sapSId = SapSId;
            this._ascsInstNum = AscsInstNum;
            this._pasInstNum = PasInstNum;
            this._sapSysGId = SapSysGId;
            this._sapInsGId = SapInsGId;
            this._sapAdmUId = SapAdmUId;
            this._sidAdmUId = SidAdmUId;
            this._sidSapUId = SidSapUId;
            this._sapHostname = SapHn;
            this._sapVirtualHostname = SapVHn;
            this._databaseName = DBName;
            this._oraSidGId = OraSidGId;
            this._oraSidUId = OraSidUId;
            this._oracleListenerPort = OracleLPort;
            this._databaseHn = DBHn;
            this._databaseVirtualHn = DBVirHn;
            this._setDomain = SetDomain;
            this._domainName = DomainName;
            this._masterPass = MasterPass;
        }

        //This is for SAP Install with HANA 2Nodes
        public SapInstall(string SapSId, string AscsInstNum, string PasInstNum, string HanaDbName, string HanaInstNum, string SapSysGId, string SapInsGId, string DbSIdAdmGId, string DbSIdAdmUId, string SidAdmUId, string SapAdmUId, string DbScehmaName, string MasterPass, string VirtualHost, string VirtHostInter, bool SetDomain, string DomainName, string DBHn, string SapVH, string SapHn)
        {
            this._sapSId = SapSId;
            this._ascsInstNum = AscsInstNum;
            this._pasInstNum = PasInstNum;
            this._hanaDbName = HanaDbName;
            this._hanaInstNum = HanaInstNum;
            this._sapSysGId = SapSysGId;
            this._sapInsGId = SapInsGId;
            this._dbSIdAdmGId = DbSIdAdmGId;
            this._dbSIdAdmUId = DbSIdAdmUId;
            this._sidAdmUId = SidAdmUId;
            this._sapAdmUId = SapAdmUId;
            this._dbScehmaName = DbScehmaName;
            this._masterPass = MasterPass;
            this._virtualHost = VirtualHost;
            this._virtHostInter = VirtHostInter;
            this._setDomain = SetDomain;
            this._domainName = DomainName;
            this._databaseHn = DBHn;
            this._virtualHostSap = SapVH;
            this._sapHostname = SapHn;
        }

        //This is for SAP Install with Additional Application Server
        public SapInstall(string SapSId, string AscsInstNum, string SapInsGId, string SapAdmUId, string SapPasHnm, string SapAasHnm, string SapAasVHnm, string MasterPass, bool SetDomain, string DomainName)
        {
            this._sapSId = SapSId;
            this._ascsInstNum = AscsInstNum;
            this._sapInsGId = SapInsGId;
            this._sapAdmUId = SapAdmUId;
            this._sapPasHnm = SapPasHnm;
            this._sapAasHnm = SapAasHnm;
            this._sapAasVHnm = SapAasVHnm;
            this._masterPass = MasterPass;
            this._setDomain = SetDomain;
            this._domainName = DomainName;
        }

        //This is for SAP Install PostActivities with HANA
        public SapInstall(string HanaInstNum, string HanaDbName, string MasterPass)
        {
            this._hanaInstNum = HanaInstNum;
            this._hanaDbName = HanaDbName;
            this._masterPass = MasterPass;
        }

        //this is the function where we declare SapInstall variables as null
        public SapInstall()
        {
           //Please do not fill this space, this method is to process that are not SAP Install
        }
    }
}