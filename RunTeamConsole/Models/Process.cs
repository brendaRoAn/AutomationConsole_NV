using MassiveTestHelper.Models;
using Newtonsoft.Json;
using RunTeamConsole.Models.AddProcesses;
using RunTeamConsole.Models.DB2Install;
using RunTeamConsole.Models.SapInstallPostSteps;
using RunTeamConsole.Models.Packages;
using RunTeamConsole.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace RunTeamConsole.Models
{
    public class Process : ObservableObject
    {
        private string _projectname;
        private string _title;
        private string _description;
        private string _team;
        private string _subtype;
        private string _ostype;
        private string _subtypeImage;
        private bool _useBridgeServer;
        private bool _certificateRequired;
        private string _user;
        private string _idx;
        private string _creationtimeStamp;
        private string _srcSID;
        private string _trgSID;
        private string _customer;
        private string _srcPAS; 
        private string _sourceType; 
        private string _srcDBS;
        private string _srcDBSOS;
        private string _trgPAS;
        private string _trgDBS;
        private string _customerSAPClient;
        private Step _currentStep;
        private int _currentStepIndex;
        private string _realtimelog;
        private int _progress;
        private List<Step> _stepList;
        private List<Appl> _applList;
        private List<ServerSystem> _targetServerList;
        private List<ServerSystem> _sourceServers;
        private SaltMaster _masterServer;
        private string _instanceType;
        private string _pasType;
        private string _productType;
        private string _pasEnv;
        private string _pasOS;
        private string _trgtSystemDB;
        private string _dbType;
        private string _dbsType;
        private string _dbsOS;
        private string[] _emaildest;
        private Step _selectedStep;
        private Credentials _credentials;
        private string _srcInstanceNum;
        private string _instanceNum;
        private string _message;
        private OptionsMessage _optionMessage;
        private string _attachment;
        private bool _applReq;
        private bool _secuential;
        private string _selectedFlowMode;
        private string _systemCopyModules;
        
        private DateTime? _restoreDateTime;
        private int _streamsCV;
        private bool _isSelected;
        private bool _isHidden;
        private List<OraclePackage> _oraclePackages;
        private List<TransactionsPackage> _transactionsPackages;
        private TransactionsPackage _selectedTransactions;
        private List<SapInstallCatalog> _sapInstallCatalogs;
        private List<Db2Install> _db2InstallCatalogs;
        private List<Component> _exportTablesComponents;
        private List<ImportComponent> _importTablesComponents;
        private List<BDLS> _BDLSList;
        
        private List<Rz10FqicpSettingsConfiguration> _fqicpList;
        private List<Rz10AddpSettingsConfiguration> _addpList;
        private List<SmlgSettingsConfiguration> _smlgList;
        private string _sm36SapUser, _sm36SapPassword, _licenseFileFullName, _licenseFileName;
        private List<Rz12SettingsConfiguration> _rz12List;
        private List<Sm61SettingsConfiguration> _sm61List;
        private List<Rz04SettingsConfiguration> _rz04List;
        private List<Rz70SettingsConfiguration> _rz70List;
        private List<Al11SettingsConfiguration> _allList;
        private List<Strust02SettingsConfiguration> _strust02List;
        private List<Scc4SettingsConfiguration> _scc4List;
        private List<Db13SettingsConfiguration> _db13List;
        private DateTime _sm21From, _sm21To, _st22From, _st22To;
        private string _st22User;

        private ServerSystem _hadrPrimaryDbServer, _hadrStandbyDbServer, _hadrSapAcscScsServer, _hadrSapErsServer;
        private List<ServerSystem> _hadrSapAasServer;
        private string _hadrSapsaPass, _hadrDisRecUser, _hadrDisRecPass;

        private string _taskOracle;
        private List<JavaComponent> _javaComponents;
        private List<SAPKernelPackage> _SAPKernelPackages, _SAPKernelPackages2;
        private List<SAPHostAgentPackage> _SAPHostAgentPackages;
        private bool _selectAllStepsCheckbox;
        private bool _checkAllEmailCheckbox;
        private bool _checkAllAutoCheckbox;

        private string _db2Pacemaker;

        private SapInstall _sapInstall;

        //private string _hadrSapsapass, _hadrDisRecUser, _hadrDisRecPass;
        //private ServerSystem _hadrPrimaryDb, _hadrStandbyDb, _hadrSapAcscScs, _hadrSapErs;
        //private List<ServerSystem> _hadrSapAas;

        [JsonIgnore]
        public RelayCommand SelectAllStepsCommand { get; private set; }
        
        [JsonIgnore]
        public RelayCommand CheckAllEmailCommand { get; private set; }
        
        [JsonIgnore]
        public RelayCommand CheckAllAutoCommand { get; private set; }

        [JsonConstructor]
        public Process(string projectname, string subtype, bool bridgeServer, string user, string timestamp, int currentstep, string sid, string customer, string pas, string dbs, string instancenum, string realtimelog, int progress, List<Step> stepList, List<Appl> applList, string instancetype, string pastype, string pasos, string dbstype, string dbsos, string[] emaildest, Credentials credentials, string message, string attachment, string selectedFlowMode, string systemCopyModules, DateTime restoreDateTime, int CVStreams, SapInstall sapInstall)
        {
            //Not modify this line unless it's requested to change the Server Name
            string cloudServerName = "AliceServer";
            /* UC Attributes */
            this._projectname = projectname;
            this._subtype = subtype;
            this._useBridgeServer = bridgeServer;
            if (sid == "GLB" && customer == "GLOBAL" && pas == "syn99a08lp215" && bridgeServer == false)
            {
                dbs = cloudServerName;
                pas = cloudServerName;
                this._useBridgeServer = true;
            }
            this._subtypeImage = Auxiliar.GetCategoryImage(subtype);

            /* Particular Attributes */
            this._user = user;
            this._creationtimeStamp = timestamp;
            this._trgSID = sid;
            this._customer = customer;
            this._trgPAS = pas;
            this._trgDBS = dbs;
            this._instanceNum = instancenum;
            this._currentStepIndex = currentstep;
            this._realtimelog = realtimelog;
            this._progress = progress;
            this._stepList = new List<Step>(stepList);
            this._selectAllStepsCheckbox = true;
            this._checkAllEmailCheckbox = true;
            this._checkAllAutoCheckbox = true;
            foreach(Step s in this._stepList)
            {
                if (!s.IsSelected)
                {
                    this._selectAllStepsCheckbox = false;
                    break;
                }
            }
            foreach (Step s in this._stepList)
            {
                if (!s.EmailDefault)
                {
                    this._checkAllEmailCheckbox = false;
                    break;
                }
            }
            for(int i = 0; i < this._stepList.Count -1; i++)
            {
                if (!this._stepList[i].AutoDefault)
                {
                    this._checkAllAutoCheckbox = false;
                    break;
                }
            }
            if (currentstep > -1)
            {
                Step s = this.StepList.Where(x => x.Index == currentstep).FirstOrDefault();
                if (s != null)
                    this._currentStep = s;
            }
            else
                this._currentStep = this.StepList.First();
            if (applList != null)
            {
                this._applList = new List<Appl>(applList);
            }
            this._instanceType = instancetype;
            this._pasType = pastype;
            this._pasOS = pasos;
            this._dbsType = dbstype;
            this._dbsOS = dbsos;
            this._emaildest = emaildest;
            this._credentials = credentials;
            this._message = message;
            this._attachment = attachment;
            this._oraclePackages = new List<OraclePackage>();
            this._exportTablesComponents = new List<Component>();
            this._importTablesComponents = new List<ImportComponent>();
            this._javaComponents = new List<JavaComponent>();
            this._SAPKernelPackages = new List<SAPKernelPackage>();
            this._SAPKernelPackages2 = new List<SAPKernelPackage>();
            this._SAPHostAgentPackages = new List<SAPHostAgentPackage>();
            this._sapInstallCatalogs = new List<SapInstallCatalog>();
            this._db2InstallCatalogs = new List<Db2Install>();
            this._isSelected = false;
            this._isHidden = false;
            SelectAllStepsCommand = new RelayCommand(ChangeSelectAllSteps, CanChangeSelectAllSteps);
            CheckAllEmailCommand = new RelayCommand(CheckAllEmail, CanCheckAllEmail);
            CheckAllAutoCommand = new RelayCommand(CheckAllAuto, CanCheckAllAuto);
            if (selectedFlowMode != null)
                this._selectedFlowMode = selectedFlowMode;
            else
                this._selectedFlowMode = "";
            this._systemCopyModules = systemCopyModules;
            if (this._selectedFlowMode.ToUpper().Contains("AUTO"))
            {
                this._restoreDateTime = restoreDateTime;
                this._streamsCV = CVStreams;
            }
            else
            {
                this._streamsCV = 0;
            }
        }

        public Process(Process layout, string idx, string timestamp, int currentstep, string sid, string customer, string pas, string dbs, string instancenum, string realtimelog, int progress, List<Step> stepList, List<Appl> applList, string instancetype, string pastype, string pasos, string dbstype, string dbsos, string[] emaildest, Credentials credentials, string message, string attachment, string selectedFlowMode, string systemCopyModules, DateTime restoreDateTime, int CVStreams, SapInstall sapInstall)
        {
            this._projectname = layout.ProjectName;
            this._title = layout.Title;
            this._description = layout.Description;
            this._team = layout.Team;
            this._subtype = layout.Subtype;
            this._ostype = layout.OSType;
            this._applReq = layout.ApplReq;
            this._secuential = layout.IsSecuential;
            this._useBridgeServer = layout.BrideServer;
            this._subtypeImage = Auxiliar.GetCategoryImage(layout.Subtype);
            this._idx = idx;
            this._user = UserProfile.ItUser;
            this._creationtimeStamp = timestamp;
            this._trgSID = sid;
            this._customer = customer;
            this._trgPAS = pas;
            this._trgDBS = dbs;
            this._instanceNum = instancenum;
            this._currentStepIndex = currentstep;
            this._realtimelog = realtimelog;
            this._progress = progress;
            this._stepList = new List<Step>(stepList);
            this._selectAllStepsCheckbox = true;
            this._checkAllEmailCheckbox = true;
            this._checkAllAutoCheckbox = true;

            if (currentstep > -1)
            {
                Step s = this.StepList.Where(x => x.Index == currentstep).FirstOrDefault();
                if (s != null)
                    this._currentStep = s;
            }
            else
                this._currentStep = this.StepList.First();

            if (applList != null)
            {
                this._applList = new List<Appl>(applList);
            }

            this._instanceType = instancetype;
            this._pasType = pastype;
            this._pasOS = pasos;
            this._dbsType = dbstype;
            this._dbsOS = dbsos;
            this._emaildest = emaildest;
            this._credentials = credentials;
            this._message = message;
            this._attachment = attachment;
            this._oraclePackages = new List<OraclePackage>();
            this._exportTablesComponents = new List<Component>();
            this._importTablesComponents = new List<ImportComponent>();
            this._javaComponents = new List<JavaComponent>();
            this._SAPKernelPackages = new List<SAPKernelPackage>();
            this._SAPHostAgentPackages = new List<SAPHostAgentPackage>();
            this._sapInstallCatalogs = new List<SapInstallCatalog>();
            this._db2InstallCatalogs = new List<Db2Install>();
            this._isSelected = false;
            this._isHidden = false;
            SelectAllStepsCommand = new RelayCommand(ChangeSelectAllSteps, CanChangeSelectAllSteps);
            CheckAllEmailCommand = new RelayCommand(CheckAllEmail, CanCheckAllEmail);
            CheckAllAutoCommand = new RelayCommand(CheckAllAuto, CanCheckAllAuto);
            if (selectedFlowMode != null)
                this._selectedFlowMode = selectedFlowMode;
            else
                this._selectedFlowMode = "";
            this._systemCopyModules = systemCopyModules;
            if (this._selectedFlowMode.ToUpper().Contains("AUTO"))
            {
                this._restoreDateTime = restoreDateTime;
                this._streamsCV = CVStreams;
            }
            else
            {
                this._streamsCV = 0;
            }
        }

        public Process(Process layout, string idx, string timestamp, string sid, string customer) {
            this._projectname = layout.ProjectName;
            this._title = layout.Title;
            this._description = layout.Description;
            this._team = layout.Team;
            this._subtype = layout.Subtype;
            this._applReq = layout.ApplReq;
            this._secuential = layout.IsSecuential;
            this._systemCopyModules = layout.SystemCopyModules;
            this._idx = idx;
            this._user = UserProfile.ItUser;
            this._trgSID = sid;
            this._customer = customer;
            this._creationtimeStamp = timestamp;
            this._currentStepIndex = -1;
            this._applList = new List<Appl>();
            this._oraclePackages = new List<OraclePackage>();
            this._transactionsPackages = new List<TransactionsPackage>();
            this._exportTablesComponents = new List<Component>();
            this._importTablesComponents = new List<ImportComponent>();
            this._javaComponents = new List<JavaComponent>();
            this._SAPKernelPackages = new List<SAPKernelPackage>();
            this._SAPKernelPackages2 = new List<SAPKernelPackage>();
            this._SAPHostAgentPackages = new List<SAPHostAgentPackage>();
            this._sapInstallCatalogs = new List<SapInstallCatalog>();
            this._db2InstallCatalogs = new List<Db2Install>();
            this._selectedFlowMode = "";
            this._restoreDateTime = DateTime.Now;
            this._streamsCV = 0;
            this.Message = "";
        }

        public SapInstall SapInstall
        {
            get { return this._sapInstall; }
            set { this._sapInstall = value; }
        }

        /*public Hadr Hadr
        {
            get { return this._hadr; }
            set { this._hadr = value; }
        }*/
        
        /* UC Attributes */
        public string ProjectName
        {
            get { return this._projectname; }
        }
        public string Title
        {
            get { return this._title; }
            set { this._title = value; }
        }
        public string Description
        {
            get { return this._description; }
            set { this._description = value; }
        }
        public string Team
        {
            get { return this._team; }
            set { this._team = value; }
        }
        public string Subtype
        {
            get { return this._subtype; }
        }
        public string OSType
        {
            get { return this._ostype; }
            set { this._ostype = value; }
        }
        public string CategoryImage
        {
            get { return this._subtypeImage; }
        }
        public bool BrideServer
        {
            get { return this._useBridgeServer; }
            set { this._useBridgeServer = value; }
        }
        public bool CertificateRequired
        {
            get { return this._certificateRequired; }
            set { this._certificateRequired = value; }
        }
        public bool ApplReq
        {
            get { return this._applReq; }
            set { this._applReq = value; }
        }
        public bool IsSecuential
        {
            get { return this._secuential; }
            set { this._secuential = value; }
        }
        /* Particular Attributes */
        public string User
        {
            get { return this._user; }
            set { this._user = value; ; }
        }
        public string Idx
        {
            get { return this._idx; }
            set { this._idx = value; ; }
        }
        public string TimeStamp
        {
            get { return this._creationtimeStamp; }
            set { this._creationtimeStamp = value; ; }
        }
        public string SourceSID
        {
            get { return this._srcSID; }
            set { this._srcSID = value; }
        }
        public string SID
        {
            get { return this._trgSID; }
        }
        public string Customer
        {
            get { return this._customer; }
        }
        public string SourcePAS
        {
            get { return this._srcPAS; }
            set { this._srcPAS = value; }
        }
        public string SourceType
        {
            get { return this._sourceType; }
            set { this._sourceType = value; }
        }
        public string SourceDBS
        {
            get { return this._srcDBS; }
            set { this._srcDBS = value; }
        }
        public string SourceDBSOS
        {
            get { return this._srcDBSOS; }
            set { this._srcDBSOS = value; }
        }
        public string PAS
        {
            get { return this._trgPAS; }
            set { this._trgPAS = value; }
        }
        public string DBS
        {
            get { return this._trgDBS; }
            set { this._trgDBS = value; }
        }
        public string SourceInstanceNum
        {
            get { return this._srcInstanceNum; }
            set { this._srcInstanceNum = value; }
        }
        public string InstanceNum
        {
            get { return this._instanceNum; }
            set { this._instanceNum = value; }
        }
        public string CustomerSAPClient
        {
            get { return this._customerSAPClient; }
            set { this._customerSAPClient = value; }
        }

        [JsonIgnore]
        public Step CurrentStep
        {
            get { return this._currentStep; }
            set 
            {
                this._currentStep = value;
                this.OnPropertyChanged("CurrentStep");
                this.OnPropertyChanged("CurrentStepName");
                this.OnPropertyChanged("CurrentStepDescription");
                this.OnPropertyChanged("CurrentStatus");
            }
        }
        
        public int CurrentStepIndex
        {
            get 
            {
                if (this.CurrentStep != null)
                    return this.CurrentStep.Index;
                else
                    return this._currentStepIndex;

            }
        }

        [JsonIgnore]
        public string CurrentStepName
        {
            get
            {
                return this.CurrentStep.Name;
            }
        }

        [JsonIgnore]
        public string CurrentStepDescription
        {
            get
            {
                string stepDescription;
                int length;
                length = this.CurrentStep.Name.IndexOf("-");
                if(length >= 0)
                    stepDescription = this.CurrentStep.Name.Substring(0, length) + " " + this.CurrentStep.Description;
                else
                    stepDescription = this.CurrentStep.Name.Substring(0) + " " + this.CurrentStep.Description;
                return stepDescription;
            }
        }

        [JsonIgnore]
        public string CurrentStatus
        {
            get
            {
                this.OnPropertyChanged("CurrentStatusDateTime");
                this.OnPropertyChanged("CurrentStatusDate");
                this.OnPropertyChanged("CurrentStatusTime");
                this.OnPropertyChanged("CurrentStatusImage");
                return this.CurrentStep.LastStatus.State;
            }
        }

        [JsonIgnore]
        public string CurrentStatusImage
        {
            get { return Auxiliar.GetStatusImage(this.CurrentStep.LastStatus.State); }
        }

        [JsonIgnore]
        public DateTime CurrentStatusDateTime
        {
            get
            {
                return this.CurrentStep.LastStatus.DateTime;
            }
        }

        [JsonIgnore]
        public string CurrentStatusDate
        {
            get
            {
                DateTime date = this.CurrentStep.LastStatus.DateTime;
                string d = date.ToString("MM'/'dd'/'yyyy");
                return d;
            }
        }

        [JsonIgnore]
        public string CurrentStatusTime
        {
            get
            {
                DateTime date = this.CurrentStep.LastStatus.DateTime;
                string h = date.ToString("HH:mm:ss");
                return h;
            }
        }

        public string RealTimeLog
        {
            get { return this._realtimelog; }
            set
            {
                this._realtimelog = value;
                this.OnPropertyChanged("RealTimeLog");
            }
        }

        public int Progress
        {
            get { return this._progress; }
            set
            {
                this._progress = value;
                this.OnPropertyChanged("Progress");
            }
        }

        public List<Step> StepList
        {
            get { return this._stepList; }
            set
            {
                this._stepList = value;
                this.OnPropertyChanged("StepList");
            }
        }

        [JsonIgnore]
        public Step SelectedStep
        {
            get { return this._selectedStep; }
            set
            {
                this._selectedStep = value;
                this.OnPropertyChanged("SelectedStep");
            }
        }

        [JsonIgnore]
        public IEnumerable<Step> SelectedSteps
        {
            get { return this.StepList.Where(o => o.IsSelected); }
        }

        [JsonIgnore]
        public bool SelectAllCheckIsChecked
        {
            get { return this._selectAllStepsCheckbox; }
            set 
            {
                this._selectAllStepsCheckbox = value;
                this.OnPropertyChanged("SelectAllCheckIsChecked");
            }
        }

        public void ChangeSelectAllSteps(object obj)
        {
            CheckBox checkbok = (obj as CheckBox);
            foreach (Step step in this._stepList)
            {
                if (step.IsEnabled)
                {
                    if (checkbok.IsChecked.HasValue && checkbok.IsChecked.Value)
                    {
                        step.IsSelected = true;
                    }
                    else
                    {
                        step.IsSelected = false;
                    }
                }
            }
        }

        public bool CanChangeSelectAllSteps(object obj)
        {
            return !IsSecuential;
        }

        [JsonIgnore]
        public IEnumerable<Step> EmailDefaultSteps
        {
            get { return this.StepList.Where(o => o.EmailDefault); }
        }

        [JsonIgnore]
        public bool AllEmailIsChecked
        {
            get { return this._checkAllEmailCheckbox; }
            set 
            { 
                this._checkAllEmailCheckbox = value;
                this.OnPropertyChanged("AllEmailIsChecked");
            }
        }

        public void CheckAllEmail(object obj)
        {
            CheckBox checkbok = (obj as CheckBox);
            foreach (Step step in this._stepList)
            {
                if (step.EmailEnabled)
                {
                    if (checkbok.IsChecked.HasValue && checkbok.IsChecked.Value)
                    {
                        step.EmailDefault = true;
                    }
                    else
                    {
                        step.EmailDefault = false;
                    }
                }
            }
        }

        public bool CanCheckAllEmail(object obj)
        {
            return true;
        }

        [JsonIgnore]
        public IEnumerable<Step> AutoDefaultSteps
        {
            get { return this.StepList.Where(o => o.AutoDefault); }
        }

        [JsonIgnore]
        public bool AllAutoIsChecked
        {
            get { return this._checkAllAutoCheckbox; }
            set 
            { 
                this._checkAllAutoCheckbox = value;
                this.OnPropertyChanged("AllAutoIsChecked");
            }
        }

        public void CheckAllAuto(object obj)
        {
            CheckBox checkbok = (obj as CheckBox);
            foreach (Step step in this._stepList)
            {
                if (step.AutoEnabled)
                {
                    if (checkbok.IsChecked.HasValue && checkbok.IsChecked.Value)
                    {
                        step.AutoDefault = true;
                    }
                    else
                    {
                        step.AutoDefault = false;
                    }
                }
            }
        }

        public bool CanCheckAllAuto(object obj)
        {
            if (this.IsSecuential)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Appl> ApplList
        {
            get { return this._applList; }
            set { this._applList = value; }
        }

        public List<ServerSystem> ServerList
        {
            get { return this._targetServerList; }
            set { this._targetServerList = value; }
        }

        public List<ServerSystem> SourceServers
        {
            get { return this._sourceServers; }
            set { this._sourceServers = value; }
        }

        public SaltMaster MasterServer
        {
            get { return this._masterServer; }
            set { this._masterServer = value; }
        }

        public List<BDLS> BDLSList
        {
            get { return this._BDLSList; }
            set { this._BDLSList = value; }
        }
        #region SapInstallPostSteps
        public string LicenseFileFullName
        {
            get { return this._licenseFileFullName; }
            set { this._licenseFileFullName = value; }
        }
        public string LicenseFileName
        {
            get { return this._licenseFileName; }
            set { this._licenseFileName = value; }
        }
        public List<Rz10FqicpSettingsConfiguration> FqicpList
        {
            get { return this._fqicpList; }
            set { this._fqicpList = value; }
        }
        public List<Rz10AddpSettingsConfiguration> AddpList
        {
            get { return this._addpList; }
            set { this._addpList = value; }
        }
        public List<SmlgSettingsConfiguration> SmlgList
        {
            get { return this._smlgList; }
            set { this._smlgList = value; }
        }
        public string Sm36SapUser
        {
            get { return this._sm36SapUser; }
            set { this._sm36SapUser = value; }
        }
        public string Sm36SapPassword
        {
            get { return this._sm36SapPassword; }
            set { this._sm36SapPassword = value; }
        }
        public List<Rz12SettingsConfiguration> Rz12List
        {
            get { return this._rz12List; }
            set { this._rz12List = value; }
        }
        public List<Sm61SettingsConfiguration> Sm61List
        {
            get { return this._sm61List; }
            set { this._sm61List = value; }
        }
        public List<Rz04SettingsConfiguration> Rz04List
        {
            get { return this._rz04List; }
            set { this._rz04List = value; }
        }
        public List<Rz70SettingsConfiguration> Rz70List
        {
            get { return this._rz70List; }
            set { this._rz70List = value; }
        }
        public List<Al11SettingsConfiguration> Al11List
        {
            get { return this._allList; }
            set { this._allList = value; }
        }
        public List<Strust02SettingsConfiguration> Strust02List
        {
            get { return this._strust02List; }
            set { this._strust02List = value; }
        }
        public List<Scc4SettingsConfiguration> Scc4List
        {
            get { return this._scc4List; }
            set { this._scc4List = value; }
        }

        public List<Db13SettingsConfiguration> Db13List
        {
            get { return this._db13List; }
            set { this._db13List = value; }
        }
        public DateTime Sm21From
        {
            get { return this._sm21From; }
            set { this._sm21From = value; }
        }
        public DateTime Sm21To
        {
            get { return this._sm21To; }
            set { this._sm21To = value; }
        }
        public DateTime St22From
        {
            get { return this._st22From; }
            set { this._st22From = value; }
        }
        public DateTime St22To
        {
            get { return this._st22To; }
            set { this._st22To = value; }
        }
        public string St22User
        {
            get { return this._st22User; }
            set { this._st22User = value; }
        }
        #endregion
        #region Hadr
        public ServerSystem HadrPrimaryDbServer
        {
            get { return this._hadrPrimaryDbServer; }
            set { this._hadrPrimaryDbServer = value; }
        }
        public ServerSystem HadrStandbyDbServer
        {
            get { return this._hadrStandbyDbServer; }
            set { this._hadrStandbyDbServer = value; }
        }
        public ServerSystem HadrSapAcscScsServer
        {
            get { return this._hadrSapAcscScsServer; }
            set { this._hadrSapAcscScsServer = value; }
        }
        public ServerSystem HadrSapErsServer
        {
            get { return this._hadrSapErsServer; }
            set { this._hadrSapErsServer = value; }
        }
        public List<ServerSystem> HadrSapAasServer
        {
            get { return this._hadrSapAasServer; }
            set { this._hadrSapAasServer = value; }
        }
        public string HadrSapsaPassword
        {
            get { return this._hadrSapsaPass; }
            set { this._hadrSapsaPass = value; }
        }
        public string HadrDisRecUser
        {
            get { return this._hadrDisRecUser; }
            set { this._hadrDisRecUser = value; }
        }
        public string HadrDisRecPass
        {
            get { return this._hadrDisRecPass; }
            set { this._hadrDisRecPass = value; }
        }
        #endregion
        public string TaskOracle
        {
            get { return this._taskOracle; }
            set { this._taskOracle = value; }
        }
        public string IntanceType
        {
            get { return this._instanceType; }
            set { this._instanceType = value; }
        }

        public string ProductType
        {
            get { return this._productType; }
            set { this._productType = value; }
        }

        public string PASType
        {
            get { return this._pasType; }
            set { this._pasType = value; }
        }

        public string Environment
        {
            get { return this._pasEnv; }
            set { this._pasEnv = value; }
        }

        public string Db2Pacemaker
        {
            get { return this._db2Pacemaker; }
            set { this._db2Pacemaker = value; }
        }

        public bool IsPRD
        {
            get 
            {
                if (this._pasEnv == "PRD")
                    return true;
                else
                    return false;
            }
        }

        public string PASOS
        {
            get { return this._pasOS; }
            set { this._pasOS = value; }
        }

        public string TargetSystemDB
        {
            get { return this._trgtSystemDB; }
            set { this._trgtSystemDB = value; }
        }

        public string DBType
        {
            get { return this._dbType; }
            set { this._dbType = value; }
        }

        public string DBSType
        {
            get { return this._dbsType; }
            set { this._dbsType = value; }
        }

        public string DBSOS
        {
            get { return this._dbsOS; }
            set { this._dbsOS = value; }
        }

        public string[] EmailDest
        {
            get { return this._emaildest; }
            set { this._emaildest = value; }
        }

        public Credentials Credentials
        {
            get { return this._credentials; }
            set { this._credentials = value; }
        }

        public string Message
        {
            get { return this._message; }
            set { this._message = value; }
        }

        public OptionsMessage OptionsMessage
        {
            get { return this._optionMessage; }
            set { this._optionMessage = value; }
        }

        public string Attachment
        {
            get { return this._attachment; }
            set { this._attachment = value; }
        }

        public bool MultipleFlowMode
        {
            get
            {
                if (StepList.Where(x => !String.IsNullOrEmpty(x.Flow) && x.Flow != "ALL").Count() > 0)
                    return true;
                else
                    return false;
            }
        }
        
        public string SelectedFlowMode
        {
            get { return this._selectedFlowMode; }
            set 
            { 
                this._selectedFlowMode = value;
                this.OnPropertyChanged("SelectedFlowMode");
                foreach(Step s in this.StepList)
                {
                    s.ProcessSelectedFlow = value;
                }
            }
        }
        
        public string SystemCopyModules
        {
            get { return this._systemCopyModules; }
            set 
            { 
                this._systemCopyModules = value;
                this.OnPropertyChanged("SystemCopyModules");
            }
        }

        public bool ShowBDLSList
        {
            get
            {
                if (this.SystemCopyModules == "ALL" || this.SystemCopyModules.ToUpper().Contains("ACTIVITIES") || this.ProjectName.ToUpper().Contains("BDLS"))
                    return true;
                else
                    return false;
            }
        }

        public bool ShowAddSettingsInfo
        {
            get
            {
                if (this.ProjectName.ToUpper().Equals("SAPINSTALLPOSTACTIVITIES") || this.ProjectName.ToUpper().Equals("SAPINSTALLPOST"))
                    return false;
                else
                    return true;
            }
        }
        public bool ShowTransactionSetPostAct
        {
            get
            {
                if (this.ProjectName.ToUpper().Equals("SAPINSTALLPOSTACTIVITIES") || this.ProjectName.ToUpper().Equals("SAPINSTALLPOST"))
                    return true;
                else
                    return false;
            }
        }
        public bool ShowTransactionSetQC
        {
            get
            {
                if (this.ProjectName.ToUpper().Equals("SAPQC"))
                    return true;
                else
                    return false;
            }
        }
        public bool ShowTransactionSet
        {
            get
            {
                if (!this.ProjectName.ToUpper().Equals("SAPINSTALLPOSTACTIVITIES") || this.ProjectName.ToUpper().Equals("SAPINSTALLPOST") || !this.ProjectName.ToUpper().Equals("SAPQC"))
                    return true;
                else
                    return false;
            }
        }
        public bool RestoreDBIsAutomatic
        {
            get
            {
                if ((SystemCopyModules == "ALL" && SelectedFlowMode.ToUpper().Contains("AUTO")) || SystemCopyModules.ToUpper().Contains("DBRESTORE"))
                    return true;
                else
                    return false;
            }
        }

        public DateTime? RestoreDateTime
        {
            get {
                if (this._restoreDateTime == null)
                    return DateTime.Now;
                else
                    return this._restoreDateTime; 
            }
            set
            {
                if (value != null)
                    this._restoreDateTime = value; ;
            }
        }

        public int CVStreams
        {
            get { return this._streamsCV; }
            set
            {
                this._streamsCV = value;
                this.OnPropertyChanged("CVStreams");
            }
        }

        public bool AnyStepReplyAuto
        {
            get
            {
                if (StepList.Any(x => x.RepeatAuto == true))
                    return true;
                else
                    return false;
            }
        }

        public List<OraclePackage> OraclePackages
        {
            get { return this._oraclePackages; }
            set { this._oraclePackages = value; }
        }

        public TransactionsPackage SelectedTransactions
        {
            get { return this._selectedTransactions; }
            set { this._selectedTransactions = value; }
        }

        public List<TransactionsPackage> TransactionsPackages
        {
            get { return this._transactionsPackages; }
            set { this._transactionsPackages = value; }
        }

        public List<SapInstallCatalog> SapInstallCatalogs
        {
            get { return this._sapInstallCatalogs; }
            set { this._sapInstallCatalogs = value; }
        }

        public List<Db2Install> Db2InstallCatalogs
        {
            get { return this._db2InstallCatalogs; }
            set { this._db2InstallCatalogs = value; }
        }

        public List<Component> ExportTablesComponents
        {
            get { return this._exportTablesComponents; }
            set { this._exportTablesComponents = value; }
        }

        public List<ImportComponent> ImportTablesComponents
        {
            get { return this._importTablesComponents; }
            set { this._importTablesComponents = value; }
        }

        public List<JavaComponent> JavaComponents
        {
            get { return this._javaComponents; }
            set { this._javaComponents = value; }
        }

        public List<SAPKernelPackage> SAPKernelPackages
        {
            get { return this._SAPKernelPackages; }
            set { this._SAPKernelPackages = value; }
        }

        public List<SAPKernelPackage> SAPKernelPackages2
        {
            get { return this._SAPKernelPackages2; }
            set { this._SAPKernelPackages2 = value; }
        }

        public List<SAPHostAgentPackage> SAPHostAgentPackages
        {
            get { return this._SAPHostAgentPackages; }
            set { this._SAPHostAgentPackages = value; }
        }

        [JsonIgnore]
        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                string processType, stepName, status, currentType, currentStepName, currentStatus;

                if(value == false)
                    MainWindow.PVMInstance.SelectAllProcessesIsChecked = false;
                else
                {
                    if (MainWindow.PVMInstance.SelectedProcesses.ToList().Count > 0)
                    {
                        processType = MainWindow.PVMInstance.SelectedProcesses.First().ProjectName.ToUpper(); 
                        stepName = MainWindow.PVMInstance.SelectedProcesses.First().CurrentStepName.ToUpper();
                        status = MainWindow.PVMInstance.SelectedProcesses.First().CurrentStatus.ToUpper();
                        currentStatus = this.CurrentStatus.ToUpper();
                        currentType = this.ProjectName.ToUpper(); ;
                        currentStepName = this.CurrentStepName.ToUpper();
                        
                        if (currentType != processType || currentStepName != stepName || currentStatus != status || currentStatus == "PROCESSING")
                        {
                            foreach (Process p in MainWindow.PVMInstance.SelectedProcesses.ToList())
                            {
                                if (p != this)
                                    p.IsSelected = false;
                            }
                        }
                        else
                        {
                            if (MainWindow.PVMInstance.Processes.Count == MainWindow.PVMInstance.SelectedProcesses.ToList().Count)
                                MainWindow.PVMInstance.SelectAllProcessesIsChecked = true;
                        }

                    }
                }
                this._isSelected = value;
                this.OnPropertyChanged("IsSelected");
                int count = MainWindow.PVMInstance.SelectedProcesses.Count();
                MainWindow.PVMInstance.SelectedProcessesCount = count;
                
                if (count > 0)
                {
                    MainWindow.PVMInstance.SelectedProcessesStatus = MainWindow.PVMInstance.SelectedProcesses.First().CurrentStatus;
                }
                else
                {
                    MainWindow.PVMInstance.SelectedProcessesStatus = "";
                }
            }
        }

        [JsonIgnore]
        public bool IsHidden
        {
            get { return this._isHidden; }
            set 
            { 
                this._isHidden = value;
                this.OnPropertyChanged("IsHidden");
            }
        }
    }
}