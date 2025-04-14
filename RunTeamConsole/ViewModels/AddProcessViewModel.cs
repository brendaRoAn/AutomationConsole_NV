using RunTeamConsole.Models;
using RunTeamConsole.ViewModels.Commands;
using RunTeamConsole.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.Specialized;
using MassiveTestHelper.Models;
using System.Windows.Media;
using System.Windows.Data;
using Newtonsoft.Json;
using static RunTeamConsole.Models.TransactionsPackage;
using RunTeamConsole.Models.AddProcesses;
using RunTeamConsole.Views.Refresh;
using RunTeamConsole.Models.Packages;
using RunTeamConsole.Views.AddProcessCommon.ExtraInputs;
using RunTeamConsole.Views.AddProcessCommon;
using UserControl1 = RunTeamConsole.Views.AddProcessCommon.UserControl1;
using RunTeamConsole.Views.SapInstall;
using Process = RunTeamConsole.Models.Process;
using System.Text.RegularExpressions;
using RunTeamConsole.Views.SapInstallPostSteps;
using RunTeamConsole.Views.DB2Install;
using RunTeamConsole.Models.DB2Install;
using RunTeamConsole.Views.StartSapHadr;

namespace RunTeamConsole.ViewModels
{
    public partial class AddProcessViewModel : ObservableObject
    {
        private AddProcessControl _currentControl;
        private ObservableCollection<AddProcessControl> _addProcessFlow;
        private int _controlIndex;
        private bool _addedSapPAViews;
        private ObservableCollection<Models.MenuItem> _teamsItems;
        private Models.MenuItem _selectedItem;
        private Process _selectedProcess;
        private bool _userCanExcecuteSelectedProcess;
        private Credentials _credentials;
        public string NewEmail { get; set; }
        private ObservableCollection<string> _emailDest;
        private SapInstall _sapInstall;
        private Hadr _hadr;
        private UserControl _packageView;
        private string _searchString;
        private string _input1, _input2;
        private int _sapSysGid;
        private string[] _searchCriteriaArray;

        //Variable to check count of type tenan on servers
        int tenanCount = 0;

        //Start of private variables for SAP Install with SALT
        private string _sapSId, _hanaDbName, _dbScehmaName, _masterPass, _virtualHost, _virtHostInter, _domainName, _ascsInstNum, _pasInstNum, _hanaInstNum, _sapSysGId, _sapInsGId, _dbSIdAdmGId, _dbSIdAdmUId, _sidAdmUId, _sapAdmUId;
        private string currentSID = "", currentSidLinux = "", currentSidAIX = "";
        private bool _setDomain, varToCheck, varToCheckAscs, varToCheckPas, varToCheckInN, varToCheckDbn, varToCheckSnm, varToCheckVir, varToCheckVhi, varToCheckDmn, varToCheckSysG, varToCheckInsG, varToCheckDsiU, varToCheckDsiG, varToCheckSidU, varToCheckAdmU;
        private bool _dbOracle = false, _dbHana = false;
        //End of private variables for SAP Install with SALT

        //Start of private variables for SAP Install with SALT with ORACLE and S4HANA 2 NODES
        private string _sidSapUId, _sapHostname, _sapVirtualHostname, _databaseName, _oraSidGId, _oraSidUId, _oracleListenerPort, _databaseHn, _databaseVirtualHn, _virtualHostSap;
        private bool varToCheckSVHN, varToCheckOSG, varToCheckOSU, varToCheckOLP, varToCheckDVH, varToCheckShn, varToCheckVhs;
        //End of private variables for SAP Install with SALT with ORACLE and S4HANA 2 NODES

        //Start of private variables for SAP Install with SALT with AAS
        private string _sapPasHnm, _sapAasHnm, _sapAasVHnm;
        private bool varToCheckSph, varToCheckSah, varToCheckSavh;
        //End of private variables for SAP Install with SALT with AAS

        //Start of private variables for DB2 Install
        private string _db2InstallDb, _db2InstallOsType, _db2InstallOsDist, _db2InstallOsArch, _db2InstallDbVer, _db2InstallDbPat, _db2InstallFileN, _db2InstallFileD;
        //End of private variables for DB2 Install

        //Start of private variables for SAP Install Post Activities with ORACLE
        private bool _taskOracleFix, _taskOracleCheck;
        private string _taskOracleFixText, _taskOracleCheckText, _taskOracle;
        //End of private variables for SAP Install Post Activities with ORACLE

        //Start of private variables for START SAP HADR SYBASE
        private string _hadrSapsaPass, _hadrDisastRecoUsr, _hadrDisastRecoPass;

        //End of private variables for START SAP HADR SYBASE

        private ObservableCollection<ServerSystem> _serverList;
        private ICollectionView _systemCatalog;
        private Predicate<object> _filteredSystemCatalog;
        private ServerSystem _selectedServer;
        private ObservableCollection<ServerSystem> _selectedServerList;
        
        public ObservableCollection<ServerSystem> StandbyServersToSelect;

        public ObservableCollection<SaltMaster> MastersList { get; private set; }

        private List<ExtraInputsSet> _inptutsextralayout;
        private ObservableCollection<ExtraInputsSet> _inptutsextra;
        private ObservableCollection<ExtraInputsSet> _summaryinptutsextra;

        private bool _selectAllServersCheckboxIsChecked, _selectAllServersOnSelectedListCheckboxIsChecked;

        public RelayCommand ChangePrincipalViewCommand { get; private set; }
        public RelayCommand ChangeNextViewCommand { get; private set; }
        public RelayCommand ChangePrevViewCommand { get; private set; }
        public RelayCommand MoveToSelectedServers { get; private set; }
        public RelayCommand RemoveFromSelectedServers { get; private set; }
        public RelayCommand SelectAllServersCommand { get; private set; }
        public RelayCommand SelectAllServersOnSelectedListCommand { get; private set; }
        public RelayCommand AddNewEmailDest { get; private set; }
        public RelayCommand ScheduleProcessesCommand { get; private set; }
        public RelayCommand ShowAddToFavoritesPromtCommand { get; private set; }
        public RelayCommand AddToFavoritesCommand { get; private set; }
        public RelayCommand ShowSidAdmUIdCommand { get; private set; }

        public AddProcessViewModel()
        {
            _teamsItems = new ObservableCollection<Models.MenuItem>();
            SolidColorBrush blackBrush = Brushes.Black, grayBrush = Brushes.Gray, foregroundBrush;
          
            foreach (Process pl in Auxiliar.ProcessInitConfig)
            {
                Certificate TempCertificate = Auxiliar.Certificates.Where(x => x.Area == pl.Team && x.ProcessName == pl.ProjectName).FirstOrDefault();
                Authorization TempAuthorizaton = Auxiliar.Authorizations.Where(x => x.Area == pl.Team && x.ProcessName == pl.ProjectName).FirstOrDefault();

                if ((pl.CertificateRequired && TempCertificate != null && TempAuthorizaton != null) || !pl.CertificateRequired)
                {
                    foregroundBrush = blackBrush;
                }
                else
                {
                    foregroundBrush = grayBrush;
                }

                Models.MenuItem teamItem = _teamsItems.Where(x => x.Title == pl.Team).FirstOrDefault();

                //This code is to hide all cloud processes for not Innovation Team users
                if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG" || UserProfile.ItUser.ToUpper() == "ITROBLB")
                //if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG" || UserProfile.ItUser.ToUpper() == "ITROBLB" || UserProfile.ItUser.ToUpper() == "VDC-ROBEB")
                {
                    if (teamItem == null)
                    {
                        teamItem = new Models.MenuItem() { Title = pl.Team, Icon = "", Foreground = blackBrush };
                        _teamsItems.Add(teamItem);
                        teamItem.IsExpanded = true;
                    }
                    
                    Models.MenuItem categoryItem = teamItem.Items.Where(x => x.Title == pl.Subtype).FirstOrDefault();
                    
                    if (categoryItem == null)
                    {
                        categoryItem = new Models.MenuItem() { Title = pl.Subtype, Icon = Auxiliar.GetCategoryImage(pl.Subtype), Foreground = blackBrush };
                        categoryItem.Items.CollectionChanged += MenuItemsCollectionChanged;
                        categoryItem.Items.Add(new Models.MenuItem() { Title = pl.Title, Icon = "", Foreground = foregroundBrush });
                        teamItem.Items.Add(categoryItem);
                        categoryItem.IsExpanded = true;
                    }
                    else
                    {
                        categoryItem.Items.Add(new Models.MenuItem() { Title = pl.Title, Icon = "", Foreground = foregroundBrush });
                    }

                    if (pl.TransactionsPackages != null)
                    {
                        foreach (TransactionsPackage tPkg in pl.TransactionsPackages)
                        {
                            foreach (Transaction t in tPkg.Transactions)
                            {
                                t.IsEnabled = true;
                                t.SetDefaultSelected();
                            }
                        }
                    }
                }
                else if(!(pl.Subtype.ToUpper().Equals("INTERNAL_TEST")))
                {
                    if (teamItem == null)
                    {
                        teamItem = new Models.MenuItem() { Title = pl.Team, Icon = "", Foreground = blackBrush };
                        _teamsItems.Add(teamItem);
                        teamItem.IsExpanded = true;
                    }

                    Models.MenuItem categoryItem = teamItem.Items.Where(x => x.Title == pl.Subtype).FirstOrDefault();
                    
                    if (categoryItem == null)
                    {
                        categoryItem = new Models.MenuItem() { Title = pl.Subtype, Icon = Auxiliar.GetCategoryImage(pl.Subtype), Foreground = blackBrush };
                        categoryItem.Items.CollectionChanged += MenuItemsCollectionChanged;
                        categoryItem.Items.Add(new Models.MenuItem() { Title = pl.Title, Icon = "", Foreground = foregroundBrush });
                        teamItem.Items.Add(categoryItem);
                        categoryItem.IsExpanded = true;
                    }
                    else
                    {
                        categoryItem.Items.Add(new Models.MenuItem() { Title = pl.Title, Icon = "", Foreground = foregroundBrush });
                    }

                    if (pl.TransactionsPackages != null)
                    {
                        foreach (TransactionsPackage tPkg in pl.TransactionsPackages)
                        {
                            foreach (Transaction t in tPkg.Transactions)
                            {
                                t.IsEnabled = true;
                                t.SetDefaultSelected();
                            }
                        }
                    }
                }

            } 

            _flowModes = new ObservableCollection<string> { };
            _selectedFlowMode = "";
            _sourceBackupDate = DateTime.Now;

            _serverList = new ObservableCollection<ServerSystem>(Auxiliar.SalServerList);
            
            foreach (ServerSystem server in _serverList)
            {
                server.IsSelected = false;
                server.IsEnabled = true;
            }
            
            _selectedServerList = new ObservableCollection<ServerSystem>();
            _systemCatalog = new ListCollectionView(_serverList);
            _systemCatalog.SortDescriptions.Add(new SortDescription("Customer", ListSortDirection.Ascending));

            _filteredSystemCatalog = null;
            _selectedServer = null;

            MastersList = new ObservableCollection<SaltMaster>(Auxiliar.SaltMastersList);
            _selectedMasterServer = null;

            ChangePrincipalViewCommand = new RelayCommand(ChangeToPrincipalVM, CanChangeView);
            ChangeNextViewCommand = new RelayCommand(ChangeToNextView, CanChangeNextView);
            ChangePrevViewCommand = new RelayCommand(ChangeToPrevView, CanChangePrevView);

            MoveToSelectedServers = new RelayCommand(AddSelectedServers, CanAddSelectedServers);
            RemoveFromSelectedServers = new RelayCommand(RemoveSelectedServers, CanRemoveSelectedServers);
            SelectAllServersCommand = new RelayCommand(ChangeSelectAllServers, CanChangeSelectAllServers);
            SelectAllServersOnSelectedListCommand = new RelayCommand(ChangeSelectAllServersOnSelectedList, CanChangeSelectAllServersOnSelectedList);

            MoveToSelectedTransactions = new RelayCommand(AddSelectedTransactions, CanAddSelectedTransactions);
            RemoveFromSelectedTransactions = new RelayCommand(RemoveSelectedTransactions, CanRemoveSelectedTransactions);
            SelectAllTransactionsCommand = new RelayCommand(ChangeSelectAllTransactions, CanChangeSelectAllTransactions);
            SelectAllTransactionsOnSelectedListCommand = new RelayCommand(ChangeSelectAllTransactionsOnSelectedList, CanChangeSelectAllTransactionsOnSelectedList);


            MoveToSelectedJavaComponents = new RelayCommand(AddSelectedJavaComponents, CanAddSelectedJavaComponents);
            RemoveFromSelectedJavaComponents = new RelayCommand(RemoveSelectedJavaComponents, CanRemoveSelectedJavaComponents);
            SelectAllJavaComponentsCommand = new RelayCommand(ChangeSelectAllJavaComponents, CanChangeSelectAllJavaComponents);
            SelectAllJavaComponentsOnSelectedListCommand = new RelayCommand(ChangeSelectAllJavaComponentsOnSelectedList, CanChangeSelectAllJavaComponentsOnSelectedList);

            //Refresh
            MoveToSelectedPreActTransactions = new RelayCommand(AddSelectedPreActTransactions, CanAddSelectedTransactions);
            RemoveFromSelectedPreActTransactions = new RelayCommand(RemoveSelectedTransactionsPreAct, CanRemoveSelectedTransactionsPreAct);
            SelectAllTransactionsOnSelectedPreActListCommand = new RelayCommand(SelectAllTransactionsOnSelectedPreActList, CanSelectAllTransactionsOnSelectedPreActList);
            MoveToSelectedPostActTransactions = new RelayCommand(AddSelectedPostActTransactions, CanAddSelectedTransactions);
            RemoveFromSelectedPostActTransactions = new RelayCommand(RemoveSelectedTransactionsPostAct, CanRemoveSelectedTransactionsPostAct);
            SelectAllExportTablesCommand = new RelayCommand(ChangeSelectAllExportTables, CanSelectAllExportTables);
            SelectAllImportTablesCommand = new RelayCommand(ChangeSelectAllImportTables, CanSelectAllImportTables);
            AddtoBDLSListCommand = new RelayCommand(AddBDLS, CanAddBDLS);
            RemoveFromBDLSListCommand = new RelayCommand(RemoveBDLS, CanRemoveBDLS);
            this._BDLSList = new ObservableCollection<BDLS>();
            this._SourceBackupCV = 16;

            //SAP Install Post Activities
            AddtoFqicpListCommand = new RelayCommand(AddRz10Fqicp, CanAddRz10Fqicp);
            RemoveFromFqicpListCommand = new RelayCommand(RemoveRz10Fqicp, CanRemoveRz10Fqicp);
            this._fqicpList = new ObservableCollection<Models.SapInstallPostSteps.Rz10FqicpSettingsConfiguration>();
            AddtoAddpListCommand = new RelayCommand(AddRz10Addp, CanAddRz10Addp);
            RemoveFromAddpListCommand = new RelayCommand(RemoveRz10Addp, CanRemoveRz10Addp);
            this._addpList = new ObservableCollection<Models.SapInstallPostSteps.Rz10AddpSettingsConfiguration>();
            AddtoSmlgListCommand = new RelayCommand(AddSmlg, CanAddSmlg);
            RemoveFromSmlgListCommand = new RelayCommand(RemoveSmlg, CanRemoveSmlg);
            this._smlgList = new ObservableCollection<Models.SapInstallPostSteps.SmlgSettingsConfiguration>();
            AddtoRz12ListCommand = new RelayCommand(AddRz12, CanAddRz12);
            RemoveFromRz12ListCommand = new RelayCommand(RemoveRz12, CanRemoveRz12);
            this._rz12List = new ObservableCollection<Models.SapInstallPostSteps.Rz12SettingsConfiguration>();
            AddtoSm61ListCommand = new RelayCommand(AddSm61, CanAddSm61);
            RemoveFromSm61ListCommand = new RelayCommand(RemoveSm61, CanRemoveSm61);
            this._sm61List = new ObservableCollection<Models.SapInstallPostSteps.Sm61SettingsConfiguration>();
            AddtoRz04ListCommand = new RelayCommand(AddRz04, CanAddRz04);
            RemoveFromRz04ListCommand = new RelayCommand(RemoveRz04, CanRemoveRz04);
            this._rz04List = new ObservableCollection<Models.SapInstallPostSteps.Rz04SettingsConfiguration>();
            AddtoRz70ListCommand = new RelayCommand(AddRz70, CanAddRz70);
            RemoveFromRz70ListCommand = new RelayCommand(RemoveRz70, CanRemoveRz70);
            this._rz70List = new ObservableCollection<Models.SapInstallPostSteps.Rz70SettingsConfiguration>();
            AddtoAl11ListCommand = new RelayCommand(AddAl11, CanAddAl11);
            RemoveFromAl11ListCommand = new RelayCommand(RemoveAl11, CanRemoveAl11);
            this._al11List = new ObservableCollection<Models.SapInstallPostSteps.Al11SettingsConfiguration>();
            AddtoStrust02ListCommand = new RelayCommand(AddStrust02, CanAddStrust02);
            RemoveFromStrust02ListCommand = new RelayCommand(RemoveStrust02, CanRemoveStrust02);
            this._strust02List = new ObservableCollection<Models.SapInstallPostSteps.Strust02SettingsConfiguration>();
            AddtoScc4ListCommand = new RelayCommand(AddScc4, CanAddScc4);
            RemoveFromScc4ListCommand = new RelayCommand(RemoveScc4, CanRemoveScc4);
            this._scc4List = new ObservableCollection<Models.SapInstallPostSteps.Scc4SettingsConfiguration>();
            AddtoDb13ListCommand = new RelayCommand(AddDb13, CanAddDb13);
            RemoveFromDb13ListCommand = new RelayCommand(RemoveDb13, CanRemoveDb13);
            this._db13List = new ObservableCollection<Models.SapInstallPostSteps.Db13SettingsConfiguration>();

            SelectPreactTransactionsSet = new RelayCommand(ChangeSelectPreActTransactionsSet, CanChangeSelectPreActTransactionsSet);

            _customerSAPClient = "000";
            _sourceClient = "000";
            _targetClient = "000";

            AddNewEmailDest = new RelayCommand(AddEmailDest, CanAddEmailDest);
            ScheduleProcessesCommand = new RelayCommand(ScheduleProcesses, CanCreateNewProcess);
            ShowAddToFavoritesPromtCommand = new RelayCommand(ShowAddToFavoritesPromt, CanCreateNewProcess);
            AddToFavoritesCommand = new RelayCommand(AddToFavorites, CanCreateNewProcess);

            NewEmail = "@syntax.com";
            _emailDest = new ObservableCollection<string>();
            _availableOraclePackagesForLinux = new ObservableCollection<OraclePackage>();
            _availableOraclePackagesForAIX = new ObservableCollection<OraclePackage>();
            _availableSAPHostAgentPackagesForLinux = new ObservableCollection<SAPHostAgentPackage>();
            _availableSAPHostAgentPackagesForAIX = new ObservableCollection<SAPHostAgentPackage>();
            _availableTransactionsPackages = new ObservableCollection<TransactionsPackage>();
            _javaComponentsCatalog = new ObservableCollection<JavaComponent>();

            //Refresh
            _availablePreactTransactionsPackages = new ObservableCollection<TransactionsPackage>();
            _availablePostactTransactionsPackages = new ObservableCollection<TransactionsPackage>();
            _exportTablesComponentsList = new ObservableCollection<Models.Packages.Component>();
            _importTablesComponentsList = new ObservableCollection<Models.Packages.ImportComponent>();
            _selectedTransactionsList = new ObservableCollection<Transaction>();
            _controlIndex = 0;
            _addProcessFlow = new ObservableCollection<AddProcessControl>();
            _addProcessFlow.Add(new AddProcessControl { UserControl = new SelectProcessView(), MenuItem = new Models.MenuItem { Title = "Select your process", Icon = "\\img\\icons\\processing.png" } });

            //For SAP Install
            _sapInstall = new SapInstall();

            _currentControl = _addProcessFlow[0];

        }

        //Initialize the set of SAP Install public variables with SALT
        #region SAP Install public variables with SALT
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
                string valueToPut = "";

                if (value == null)
                    this._ascsInstNum = value;
                else if (!SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD") && (value == this.PasInstNum || value == this.HanaInstNum))
                {
                    MessageBox.Show("ASCS Instance Number, PAS Instance Number and HANA Instance Number can't have the same number, please try again with a new one.");
                    valueToPut = null;
                    this._ascsInstNum = valueToPut;
                    this.OnPropertyChanged("AscsInstNum");
                }
                else
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);

                    foreach (char c in chars)
                    {
                        if (!c.ToString().Equals(" "))
                            valueToPut = valueToPut += c.ToString();
                    }

                    if (value.Length == 2)
                        this.varToCheckAscs = true;
                    else
                        this.varToCheckAscs = false;

                    this._ascsInstNum = valueToPut;
                    this.OnPropertyChanged("AscsInstNum");
                }
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
                string valueToPut = "";


                if (value == null)
                    this._pasInstNum = value;
                else if (SelectedProcess.ProjectName.Trim().ToUpper() == "SAPINSTALLHANACLOUD")
                {
                    if (value == this.AscsInstNum)
                    {
                        MessageBox.Show("ASCS Instance Number and PAS Instance Number can't have the same number, please try again with a new one.");
                        valueToPut = null;
                        this._pasInstNum = valueToPut;
                        this.OnPropertyChanged("PasInstNum");
                    }
                    else
                    {
                        List<char> chars = new List<char>();
                        chars.AddRange(value);

                        foreach (char c in chars)
                        {
                            if (!c.ToString().Equals(" "))
                                valueToPut = valueToPut += c.ToString();
                        }

                        if (value.Length == 2)
                            this.varToCheckPas = true;
                        else
                            this.varToCheckPas = false;

                        this._pasInstNum = valueToPut;
                        this.OnPropertyChanged("PasInstNum");
                    }
                }
                else if (value == this.AscsInstNum || value == this.HanaInstNum)
                {
                    MessageBox.Show("ASCS Instance Number, PAS Instance Number and HANA Instance Number can't have the same number, please try again with a new one.");
                    valueToPut = null;
                    this._pasInstNum = valueToPut;
                    this.OnPropertyChanged("PasInstNum");
                }
                else
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);

                    foreach (char c in chars)
                    {
                        if (!c.ToString().Equals(" "))
                            valueToPut = valueToPut += c.ToString();
                    }

                    if (value.Length == 2)
                        this.varToCheckPas = true;
                    else
                        this.varToCheckPas = false;

                    this._pasInstNum = valueToPut;
                    this.OnPropertyChanged("PasInstNum");
                }
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
                if (value == null)
                {
                    this.varToCheckDbn = false;
                    this._hanaDbName = value;
                }
                else
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    if (SelectedServersList.Count() == 1)
                    {
                        if (value.ToUpper() == SapSId)
                        {
                            MessageBox.Show("SAP SID and HANA DB Name can't have the same value, please try again with a new one.");
                            valueToPut = null;
                            this._hanaDbName = valueToPut;
                            this.OnPropertyChanged("HanaDbName");
                        }
                        else
                        {
                            foreach (char c in chars)
                            {
                                if (!c.ToString().Equals(" "))
                                {
                                    if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                                        valueToPut = valueToPut += c.ToString();
                                }
                            }

                            if (value.Length == 3)
                                this.varToCheckDbn = true;
                            else
                                this.varToCheckDbn = false;

                            this._hanaDbName = valueToPut.ToUpper();
                            this.OnPropertyChanged("HanaDbName");
                        }
                    }
                    else
                    {
                        foreach (char c in chars)
                        {
                            if (!c.ToString().Equals(" "))
                            {
                                if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                                    valueToPut = valueToPut += c.ToString();
                            }
                        }

                        if (value.Length == 3)
                            this.varToCheckDbn = true;
                        else
                            this.varToCheckDbn = false;

                        this._hanaDbName = valueToPut.ToUpper();
                        this.OnPropertyChanged("HanaDbName");
                    }
                }
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
                string valueToPut = "";

                if (value == null)
                    this._hanaInstNum = value;
                else if (SelectedProcess.ProjectName.Trim().ToUpper() == "SAPINSTALLHANACLOUD")
                {
                    if (value == this.AscsInstNum)
                    {
                        MessageBox.Show("ASCS Instance Number and HANA Instance Number can't have the same number, please try again with a new one.");
                        valueToPut = null;
                        this._hanaInstNum = valueToPut;
                        this.OnPropertyChanged("HanaInstNum");
                    }
                    else
                    {
                        List<char> chars = new List<char>();
                        chars.AddRange(value);

                        foreach (char c in chars)
                        {
                            if (!c.ToString().Equals(" "))
                                valueToPut = valueToPut += c.ToString();
                        }

                        if (value.Length == 2)
                            this.varToCheckInN = true;
                        else
                            this.varToCheckInN = false;

                        this._hanaInstNum = valueToPut;
                        this.OnPropertyChanged("HanaInstNum");
                    }
                }
                else if (value == this.AscsInstNum || value == this.PasInstNum)
                {
                    MessageBox.Show("ASCS Instance Number, PAS Instance Number and HANA Instance Number can't have the same number, please try again with a new one.");
                    valueToPut = null;
                    this._hanaInstNum = valueToPut;
                    this.OnPropertyChanged("HanaInstNum");
                }
                else
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);

                    foreach (char c in chars)
                    {
                        if (!c.ToString().Equals(" "))
                            valueToPut = valueToPut += c.ToString();
                    }

                    if (value.Length == 2)
                        this.varToCheckInN = true;
                    else
                        this.varToCheckInN = false;

                    this._hanaInstNum = valueToPut;
                    this.OnPropertyChanged("HanaInstNum");
                }
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
                if (value == null)
                    this._sapSysGId = value;
                else if (ShowHANA)
                {
                    if (value == this.SapInsGId || value == this.DbSIdAdmGId)
                    {
                        MessageBox.Show("sapsys gID, sapinst gID  and DB SID Adm gID can't have the same number, please try again with a new one.");
                        this._sapSysGId = null;
                    }
                    else
                    {
                        List<char> chars = new List<char>();
                        chars.AddRange(value);
                        string valueToPut = "";

                        foreach (char c in chars)
                        {
                            if (!c.ToString().Equals(" "))
                                valueToPut = valueToPut += c.ToString();
                        }

                        if (value == "")
                            this.varToCheckSysG = false;
                        else
                            this.varToCheckSysG = true;

                        this._sapSysGId = valueToPut;
                        this.OnPropertyChanged("SapSysGId");
                    }
                }
                else if (ShowORACLE)
                {
                    if (SelectedServersList.Count == 1)
                    {
                        if (value == this.SapInsGId || value == this.OraSidGId)
                        {
                            MessageBox.Show("sapsys gID, sapinst gID and OraSID gID can't have the same number, please try again with a new one.");
                            this._sapSysGId = null;
                        }
                        else
                        {
                            List<char> chars = new List<char>();
                            chars.AddRange(value);
                            string valueToPut = "";

                            foreach (char c in chars)
                            {
                                if (!c.ToString().Equals(" "))
                                    valueToPut = valueToPut += c.ToString();
                            }

                            if (value == "")
                                this.varToCheckSysG = false;
                            else
                                this.varToCheckSysG = true;

                            this._sapSysGId = valueToPut;
                            this.OnPropertyChanged("SapSysGId");
                        }
                    }
                    else
                    {
                        if (value == this.SapInsGId)
                        {
                            MessageBox.Show("sapsys gID and sapinst gID can't have the same number, please try again with a new one.");
                            this._sapSysGId = null;
                        }
                        else
                        {
                            List<char> chars = new List<char>();
                            chars.AddRange(value);
                            string valueToPut = "";

                            foreach (char c in chars)
                            {
                                if (!c.ToString().Equals(" "))
                                    valueToPut = valueToPut += c.ToString();
                            }

                            if (value == "")
                                this.varToCheckSysG = false;
                            else
                                this.varToCheckSysG = true;

                            this._sapSysGId = valueToPut;
                            this.OnPropertyChanged("SapSysGId");
                        }
                    }
                }
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
                if (value == null)
                    this._sapInsGId = value;
                else if (ShowHANA)
                {
                    if (value == this.SapSysGId || value == this.DbSIdAdmGId)
                    {
                        MessageBox.Show("sapsys gID, sapinst gID  and DB SID Adm gID can't have the same number, please try again with a new one.");
                        this._sapInsGId = null;
                    }
                    else
                    {
                        List<char> chars = new List<char>();
                        chars.AddRange(value);
                        string valueToPut = "";

                        foreach (char c in chars)
                        {
                            if (!c.ToString().Equals(" "))
                                valueToPut = valueToPut += c.ToString();
                        }

                        if (value == "")
                            this.varToCheckInsG = false;
                        else
                            this.varToCheckInsG = true;

                        this._sapInsGId = valueToPut;
                        this.OnPropertyChanged("SapInsGId");
                    }
                }
                else if (ShowORACLE)
                {
                    if (SelectedServersList.Count == 1)
                    {
                        if (value == this.SapSysGId || value == this.OraSidGId)
                        {
                            MessageBox.Show("sapsys gID, sapinst gID and OraSID gID can't have the same number, please try again with a new one.");
                            this._sapInsGId = null;
                        }
                        else
                        {
                            List<char> chars = new List<char>();
                            chars.AddRange(value);
                            string valueToPut = "";

                            foreach (char c in chars)
                            {
                                if (!c.ToString().Equals(" "))
                                    valueToPut = valueToPut += c.ToString();
                            }

                            if (value == "")
                                this.varToCheckInsG = false;
                            else
                                this.varToCheckInsG = true;

                            this._sapInsGId = valueToPut;
                            this.OnPropertyChanged("SapInsGId");
                        }
                    }
                    else
                    {
                        if (value == this.SapSysGId)
                        {
                            MessageBox.Show("sapsys gID and sapinst gID can't have the same number, please try again with a new one.");
                            this._sapInsGId = null;
                        }
                        else
                        {
                            List<char> chars = new List<char>();
                            chars.AddRange(value);
                            string valueToPut = "";

                            foreach (char c in chars)
                            {
                                if (!c.ToString().Equals(" "))
                                    valueToPut = valueToPut += c.ToString();
                            }

                            if (value == "")
                                this.varToCheckInsG = false;
                            else
                                this.varToCheckInsG = true;

                            this._sapInsGId = valueToPut;
                            this.OnPropertyChanged("SapInsGId");
                        }
                    }
                }
                else
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!c.ToString().Equals(" "))
                            valueToPut = valueToPut += c.ToString();
                    }

                    if (value == "")
                        this.varToCheckInsG = false;
                    else
                        this.varToCheckInsG = true;

                    this._sapInsGId = valueToPut;
                    this.OnPropertyChanged("SapInsGId");
                }
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
                if (value == null)
                    this._dbSIdAdmUId = value;
                else if (value == this.SidAdmUId || value == this.SapAdmUId)
                {
                    MessageBox.Show("SID adm uID, sap adm uID and DB SID Adm uID can't have the same number, please try again with a new one.");
                    this._dbSIdAdmUId = null;
                }
                else
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!c.ToString().Equals(" "))
                            valueToPut = valueToPut += c.ToString();
                    }

                    if (value == "")
                        this.varToCheckDsiU = false;
                    else
                        this.varToCheckDsiU = true;

                    this._dbSIdAdmUId = valueToPut;
                    this.OnPropertyChanged("DbSIdAdmUId");
                }
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
                if (value == null)
                    this._dbSIdAdmGId = value;
                else if (value == this.SapSysGId || value == this.SapInsGId)
                {
                    MessageBox.Show("sapsys gID, sapinst gID and DB SID Adm gID can't have the same number, please try again with a new one.");
                    this._dbSIdAdmGId = null;
                }
                else
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!c.ToString().Equals(" "))
                            valueToPut = valueToPut += c.ToString();
                    }

                    if (value == "")
                        this.varToCheckDsiG = false;
                    else
                        this.varToCheckDsiG = true;

                    this._dbSIdAdmGId = valueToPut;
                    this.OnPropertyChanged("DbSIdAdmGId");
                }
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
                if (value == null)
                    this._sidAdmUId = value;
                else if (ShowHANA)
                {
                    if (value == this.SapAdmUId || value == this.DbSIdAdmUId)
                    {
                        MessageBox.Show("SID adm uID, sap adm uID and DB SID Adm uID can't have the same number, please try again with a new one.");
                        this._sidAdmUId = null;
                    }
                    else
                    {
                        List<char> chars = new List<char>();
                        chars.AddRange(value);
                        string valueToPut = "";

                        foreach (char c in chars)
                        {
                            if (!c.ToString().Equals(" "))
                                valueToPut = valueToPut += c.ToString();
                        }

                        if (value == "")
                            this.varToCheckSidU = false;
                        else
                            this.varToCheckSidU = true;

                        this._sidAdmUId = valueToPut;
                        this.OnPropertyChanged("SidAdmUId");
                    }
                }
                else if (ShowORACLE)
                {
                    if (SelectedServersList.Count == 1)
                    {
                        if (value == this.SapAdmUId || value == this.OraSidUId)
                        {
                            MessageBox.Show("SID adm uID, sap adm uID and OraSID uID can't have the same number, please try again with a new one.");
                            this._sidAdmUId = null;
                        }
                        else
                        {
                            List<char> chars = new List<char>();
                            chars.AddRange(value);
                            string valueToPut = "";

                            foreach (char c in chars)
                            {
                                if (!c.ToString().Equals(" "))
                                    valueToPut = valueToPut += c.ToString();
                            }

                            if (value == "")
                                this.varToCheckSidU = false;
                            else
                                this.varToCheckSidU = true;

                            this._sidAdmUId = valueToPut;
                            this.OnPropertyChanged("SidAdmUId");
                        }
                    }
                    else
                    {
                        if (value == this.SapAdmUId)
                        {
                            MessageBox.Show("SID adm uID and sap adm uID can't have the same number, please try again with a new one.");
                            this._sidAdmUId = null;
                        }
                        else
                        {
                            List<char> chars = new List<char>();
                            chars.AddRange(value);
                            string valueToPut = "";

                            foreach (char c in chars)
                            {
                                if (!c.ToString().Equals(" "))
                                    valueToPut = valueToPut += c.ToString();
                            }

                            if (value == "")
                                this.varToCheckSidU = false;
                            else
                                this.varToCheckSidU = true;

                            this._sidAdmUId = valueToPut;
                            this.OnPropertyChanged("SidAdmUId");
                        }
                    }
                }
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
                if (value == null)
                    this._sapAdmUId = value;
                else if (ShowHANA)
                {
                    if (value == this.SidAdmUId || value == this.DbSIdAdmUId)
                    {
                        MessageBox.Show("SID adm uID, sap adm uID and DB SID Adm uID can't have the same number, please try again with a new one.");
                        this._sapAdmUId = null;
                    }
                    else
                    {
                        List<char> chars = new List<char>();
                        chars.AddRange(value);
                        string valueToPut = "";

                        foreach (char c in chars)
                        {
                            if (!c.ToString().Equals(" "))
                                valueToPut = valueToPut += c.ToString();
                        }

                        if (value == "")
                            this.varToCheckAdmU = false;
                        else
                            this.varToCheckAdmU = true;

                        this._sapAdmUId = valueToPut;
                        this.OnPropertyChanged("SapAdmUId");
                    }
                }
                else if (ShowORACLE)
                {
                    if (SelectedServersList.Count == 1)
                    {
                        if (value == this.SidAdmUId || value == this.OraSidUId)
                        {
                            MessageBox.Show("SID adm uID, sap adm uID and OraSID uID can't have the same number, please try again with a new one.");
                            this._sapAdmUId = null;
                        }
                        else
                        {
                            List<char> chars = new List<char>();
                            chars.AddRange(value);
                            string valueToPut = "";

                            foreach (char c in chars)
                            {
                                if (!c.ToString().Equals(" "))
                                    valueToPut = valueToPut += c.ToString();
                            }

                            if (value == "")
                                this.varToCheckAdmU = false;
                            else
                                this.varToCheckAdmU = true;

                            this._sapAdmUId = valueToPut;
                            this.OnPropertyChanged("SapAdmUId");
                        }
                    }
                    else
                    {
                        if (value == this.SidAdmUId)
                        {
                            MessageBox.Show("SID adm uID and sap adm uID can't have the same number, please try again with a new one.");
                            this._sapAdmUId = null;
                        }
                        else
                        {
                            List<char> chars = new List<char>();
                            chars.AddRange(value);
                            string valueToPut = "";

                            foreach (char c in chars)
                            {
                                if (!c.ToString().Equals(" "))
                                    valueToPut = valueToPut += c.ToString();
                            }

                            if (value == "")
                                this.varToCheckAdmU = false;
                            else
                                this.varToCheckAdmU = true;

                            this._sapAdmUId = valueToPut;
                            this.OnPropertyChanged("SapAdmUId");
                        }
                    }
                }
                else
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!c.ToString().Equals(" "))
                            valueToPut = valueToPut += c.ToString();
                    }

                    if (value == "")
                        this.varToCheckAdmU = false;
                    else
                        this.varToCheckAdmU = true;

                    this._sapAdmUId = valueToPut;
                    this.OnPropertyChanged("SapAdmUId");
                }
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
                List<char> chars = new List<char>();
                chars.AddRange(value);
                string valueToPut = "";

                foreach (char c in chars)
                {
                    if (!c.ToString().Equals(" "))
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                            valueToPut = valueToPut += c.ToString();
                    }
                }

                if (value == "")
                    this.varToCheckSnm = false;
                else
                    this.varToCheckSnm = true;

                this._dbScehmaName = valueToPut.ToUpper();
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
                if (value.Length > 9)
                {
                    if (Regex.IsMatch(value, "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)"))
                        this.varToCheck = true;
                }
                else
                    this.varToCheck = false;

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
                if (value == null)
                {
                    this._virtualHost = value;
                    this.varToCheckVir = false;
                    this.OnPropertyChanged("VirtualHost");
                }
                else
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!c.ToString().Equals(" "))
                        {
                            if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                                valueToPut = valueToPut += c.ToString();
                        }
                    }

                    if (value == "")
                        this.varToCheckVir = false;
                    else
                        this.varToCheckVir = true;

                    this._virtualHost = valueToPut;
                    this.OnPropertyChanged("VirtualHost");
                }
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
                if (value == null)
                {
                    this._virtHostInter = value;
                    this.varToCheckVhi = false;
                    this.OnPropertyChanged("VirtHostInter");
                }
                else
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!c.ToString().Equals(" "))
                        {
                            if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                                valueToPut = valueToPut += c.ToString();
                        }
                    }

                    if (value == "")
                        this.varToCheckVhi = false;
                    else
                        this.varToCheckVhi = true;

                    this._virtHostInter = valueToPut;
                    this.OnPropertyChanged("VirtHostInter");
                }
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
                if (value == true)
                    DomainName = null;
                else
                    DomainName = "NA";
            }
        }
        public string DomainName
        {
            get
            {
                if (SetDomain == false)
                    return "NA";
                else
                    return this._domainName;
            }
            set
            {
                if (value == null)
                {
                    this._domainName = value;
                    this.OnPropertyChanged("DomainName");
                }
                else
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!c.ToString().Equals(" "))
                        {
                            if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                                valueToPut = valueToPut += c.ToString();
                        }
                    }
                    if (value == "")
                        this.varToCheckDmn = false;
                    else
                        this.varToCheckDmn = true;
                    this._domainName = valueToPut;
                    this.OnPropertyChanged("DomainName");
                }
            }
        }
        public bool DbOracle
        {
            get
            {
                return this._dbOracle;
            }
            set
            {
                this._dbOracle = value;
                this.OnPropertyChanged("DbOracle");
            }
        }
        public bool DbHana
        {
            get
            {
                return this._dbHana;
            }
            set
            {
                this._dbHana = value;
                this.OnPropertyChanged("DbHana");
            }
        }
        #endregion
        //End of the set of SAP Install public variables with SALT

        //Initialize the set of SAP Install public variables with SALT with ORACLE
        #region SAP Install public variables with SALT with ORACLE
        public string SapHostname
        {
            get
            {
                return this._sapHostname;
            }
            set
            {
                if (value == "")
                {
                    this._sapHostname = null;
                    this.varToCheckShn = false;
                    this.OnPropertyChanged("SapHostname");
                }
                else
                {
                    this._sapHostname = value;
                    this.varToCheckShn = true;
                    this.OnPropertyChanged("SapHostname");
                }
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
                if (value == null)
                {
                    this.varToCheckSVHN = false;
                    this._sapVirtualHostname = value;
                    this.OnPropertyChanged("SapVirtualHostname");
                }
                else
                {
                    if (SelectedServersList.Count == 2)
                    {
                        if (value == DatabaseVirtualHn)
                        {
                            MessageBox.Show("Sap Virtual Hostname and Database Virtual Hostname can't have the same number if there are two servers selected, please try again with a new one.");
                            string valueToPut = null;
                            this._sapVirtualHostname = valueToPut;
                            this.OnPropertyChanged("OraSidGId");
                        }
                        else
                        {
                            List<char> chars = new List<char>();
                            chars.AddRange(value);
                            string valueToPut = "";

                            foreach (char c in chars)
                            {
                                if (!c.ToString().Equals(" "))
                                {
                                    if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                                        valueToPut = valueToPut += c.ToString();
                                }
                            }
                            if (value == "")
                                this.varToCheckSVHN = false;
                            else
                                this.varToCheckSVHN = true;
                            this._sapVirtualHostname = valueToPut;
                            this.OnPropertyChanged("SapVirtualHostname");
                        }
                    }
                    else
                    {
                        List<char> chars = new List<char>();
                        chars.AddRange(value);
                        string valueToPut = "";

                        foreach (char c in chars)
                        {
                            if (!c.ToString().Equals(" "))
                            {
                                if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                                    valueToPut = valueToPut += c.ToString();
                            }
                        }
                        if (value == "")
                            this.varToCheckSVHN = false;
                        else
                            this.varToCheckSVHN = true;
                        this._sapVirtualHostname = valueToPut;
                        this.OnPropertyChanged("SapVirtualHostname");
                    }
                }
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
                if (value == null)
                    this._oraSidGId = value;
                else if (SelectedServersList.Count == 1)
                {
                    if (value == this.SapSysGId || value == this.SapInsGId)
                    {
                        MessageBox.Show("sapsys gID, sapinst gID and OraSID gID can't have the same number, please try again with a new one.");
                        this._oraSidGId = null;
                    }
                    else
                    {
                        List<char> chars = new List<char>();
                        chars.AddRange(value);
                        string valueToPut = "";

                        foreach (char c in chars)
                        {
                            if (!c.ToString().Equals(" "))
                                valueToPut = valueToPut += c.ToString();
                        }

                        if (value == "")
                            this.varToCheckOSG = false;
                        else
                            this.varToCheckOSG = true;

                        this._oraSidGId = valueToPut;
                        this.OnPropertyChanged("OraSidGId");
                    }
                }
                else
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!c.ToString().Equals(" "))
                            valueToPut = valueToPut += c.ToString();
                    }

                    if (value == "")
                        this.varToCheckOSG = false;
                    else
                        this.varToCheckOSG = true;

                    this._oraSidGId = valueToPut;
                    this.OnPropertyChanged("OraSidGId");
                }
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
                if (value == null)
                    this._oraSidUId = value;
                else if (SelectedServersList.Count == 1)
                {
                    if (value == this.SidAdmUId || value == this.SapAdmUId)
                    {
                        MessageBox.Show("SID adm uID, sap adm uID and OraSID uID can't have the same number, please try again with a new one.");
                        this._oraSidUId = null;
                    }
                    else
                    {
                        List<char> chars = new List<char>();
                        chars.AddRange(value);
                        string valueToPut = "";

                        foreach (char c in chars)
                        {
                            if (!c.ToString().Equals(" "))
                                valueToPut = valueToPut += c.ToString();
                        }

                        if (value == "")
                            this.varToCheckOSU = false;
                        else
                            this.varToCheckOSU = true;

                        this._oraSidUId = valueToPut;
                        this.OnPropertyChanged("OraSidUId");
                    }
                }
                else
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!c.ToString().Equals(" "))
                            valueToPut = valueToPut += c.ToString();
                    }

                    if (value == "")
                        this.varToCheckOSU = false;
                    else
                        this.varToCheckOSU = true;

                    this._oraSidUId = valueToPut;
                    this.OnPropertyChanged("OraSidUId");
                }
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
                if (value == null)
                {
                    this._oracleListenerPort = value;
                    this.varToCheckOLP = false;
                    this.OnPropertyChanged("OracleListenerPort");
                }
                else
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!c.ToString().Equals(" "))
                            valueToPut = valueToPut += c.ToString();
                    }

                    if (value == "")
                        this.varToCheckOLP = false;
                    else
                        this.varToCheckOLP = true;

                    this._oracleListenerPort = valueToPut;
                    this.OnPropertyChanged("OracleListenerPort");
                }
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
                if (value == null)
                {
                    this._databaseVirtualHn = value;
                    this.varToCheckDVH = false;
                    this.OnPropertyChanged("DatabaseVirtualHn");
                }
                else
                {
                    if (SelectedServersList.Count == 2)
                    {
                        if (value == SapVirtualHostname)
                        {
                            MessageBox.Show("Sap Virtual Hostname and Database Virtual Hostname can't have the same number if there are two servers selected, please try again with a new one.");
                            string valueToPut = null;
                            this._databaseVirtualHn = valueToPut;
                            this.OnPropertyChanged("OraSidGId");
                        }
                        else
                        {
                            List<char> chars = new List<char>();
                            chars.AddRange(value);
                            string valueToPut = "";

                            foreach (char c in chars)
                            {
                                if (!c.ToString().Equals(" "))
                                {
                                    if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                                        valueToPut = valueToPut += c.ToString();
                                }
                            }
                            if (value == "")
                                this.varToCheckDVH = false;
                            else
                                this.varToCheckDVH = true;
                            this._databaseVirtualHn = valueToPut;
                            this.OnPropertyChanged("DatabaseVirtualHn");
                        }
                    }
                    else
                    {
                        List<char> chars = new List<char>();
                        chars.AddRange(value);
                        string valueToPut = "";

                        foreach (char c in chars)
                        {
                            if (!c.ToString().Equals(" "))
                            {
                                if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                                    valueToPut = valueToPut += c.ToString();
                            }
                        }
                        if (value == "")
                            this.varToCheckDVH = false;
                        else
                            this.varToCheckDVH = true;
                        this._databaseVirtualHn = valueToPut;
                        this.OnPropertyChanged("DatabaseVirtualHn");
                    }
                }
            }
        }
        public string VirtualHostSap
        {
            get
            {
                return this._virtualHostSap;
            }
            set
            {
                if (value == null)
                {
                    this.varToCheckDVH = false;
                    this._virtualHostSap = value;
                    this.OnPropertyChanged("VirtualHostSap");
                }
                else
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!c.ToString().Equals(" "))
                        {
                            if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                                valueToPut = valueToPut += c.ToString();
                        }
                    }
                    if (value == "")
                        this.varToCheckDVH = false;
                    else
                        this.varToCheckDVH = true;
                    this._virtualHostSap = valueToPut;
                    this.OnPropertyChanged("VirtualHostSap");
                }
            }
        }
        #endregion
        //End of the set of SAP Install public variables with SALT with ORACLE

        //Initialize the set of SAP Install public variables with SALT with Additional Application Server
        #region SAP Install public variables with SALT with Additional Application Server
        public string SapPasHnm
        {
            get
            {
                return this._sapPasHnm;
            }
            set
            {
                if (value == "")
                {
                    this._sapPasHnm = null;
                    this.varToCheckSph = false;
                    this.OnPropertyChanged("SapPasHnm");
                }
                else
                {
                    this._sapPasHnm = value;
                    this.varToCheckSph = true;
                    this.OnPropertyChanged("SapPasHnm");
                }
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
                if (value == "")
                {
                    this._sapAasHnm = null;
                    this.varToCheckSah = false;
                    this.OnPropertyChanged("SapAasHnm");
                }
                else
                {
                    this._sapAasHnm = value;
                    this.varToCheckSah = true;
                    this.OnPropertyChanged("SapAasHnm");
                }
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
                if (value == "")
                {
                    this._sapAasVHnm = null;
                    this.varToCheckSavh = false;
                    this.OnPropertyChanged("SapAasVHnm");
                }
                else
                {
                    this._sapAasVHnm = value;
                    this.varToCheckSavh = true;
                    this.OnPropertyChanged("SapAasVHnm");
                }
            }
        }
        #endregion
        //End of the set of SAP Install public variables with SALT with Additional Application Server

        //Initialize the set of SAP Install Post Activities public variables
        #region SAP Install Post Activities public variables
        public string LicenseFullName
        {
            get { return SapInstallLicenseFile.fullName; }
            set
            {
                //this._licenseFullName = SapInstallLicenseFile.fullNamePath;
                this.OnPropertyChanged("LicenseFullName");
            }

        }

        public string CertificateFullName
        {
            get { return SapInstallSTRUST02.fullCertificateName; }
            set
            {
                this.OnPropertyChanged("CertificateFullName");
            }
        }
        #endregion
        //End of the set of SAP Install Post Activities public variables

        //Start of the set of DB2 Install public variables
        #region DB2 Install public variables
        public string Db2InstallDb
        {
            get
            {
                return this._db2InstallDb;
            }
            set
            {
                this._db2InstallDb = value;
                this.OnPropertyChanged("Db2InstallDb");
            }
        }
        public string Db2InstallOsType
        {
            get
            {
                return this._db2InstallOsType;
            }
            set
            {
                this._db2InstallOsType = value;
                this.OnPropertyChanged("Db2InstallOsType");
            }
        }
        public string Db2InstallOsDist
        {
            get
            {
                return this._db2InstallOsDist;
            }
            set
            {
                this._db2InstallOsDist = value;
                this.OnPropertyChanged("Db2InstallOsDist");
            }
        }
        public string Db2InstallOsArch
        {
            get
            {
                return this._db2InstallOsArch;
            }
            set
            {
                this._db2InstallOsArch = value;
                this.OnPropertyChanged("Db2InstallOsArchitecture");
            }
        }
        public string Db2InstallDbVer
        {
            get
            {
                return this._db2InstallDbVer;
            }
            set
            {
                this._db2InstallDbVer = value;
                this.OnPropertyChanged("Db2InstallDbVersion");
            }
        }
        public string Db2InstallDbPat
        {
            get
            {
                return this._db2InstallDbPat;
            }
            set
            {
                this._db2InstallDbPat = value;
                this.OnPropertyChanged("Db2InstallDbPatch");
            }
        }
        public string Db2InstallFileN
        {
            get
            {
                return this._db2InstallFileN;
            }
            set
            {
                this._db2InstallFileN = value;
                this.OnPropertyChanged("Db2InstallFileName");
            }
        }
        public string Db2InstallFileD
        {
            get
            {
                return this._db2InstallFileD;
            }
            set
            {
                this._db2InstallFileD = value;
                this.OnPropertyChanged("Db2InstallFileDescription");
            }
        }
        #endregion
        //End of the set of DB2 Install public variables

        //Initialize the set of SAP Install Post Activities with ORACLE
        #region SAP Install public variables with SALT
        public bool TaskOracleCheck
        {
            get
            {
                return  this._taskOracleCheck;
            }
            set
            {
                this._taskOracleCheck = value;
                if (value)
                {
                    this._taskOracleCheckText = "Check";
                    this._taskOracleFixText = null;
                }
                this.OnPropertyChanged("TaskOracleCheck");
            }
        }
        public bool TaskOracleFix
        {
            get
            {
                return this._taskOracleFix;
            }
            set 
            {
                this._taskOracleFix = value;
                if (value)
                {
                    this._taskOracleFixText = "Fix";
                    this._taskOracleCheckText = null;
                }
                this.OnPropertyChanged("TaskOracleFix");
            }
        }
        public string TaskOracle
        {
            get
            {
                if(this._taskOracleFixText != null)
                    return this._taskOracle = this._taskOracleFixText;
                else
                    return this._taskOracle = this._taskOracleCheckText;
            }
            set
            {
                this._taskOracle = value;
                this.OnPropertyChanged("TaskOracle");
            }
        }
        #endregion
        //End the set of SAP Install Post Activities with ORACLE

        //Initialize the set of START SAP HADR SYBASE
        #region START SAP HADR SYBASE public variables
        public String HadrSapsaPassword
        {
            get { return this._sapUserSapsaPassword; }
            set
            {
                if (value == null)
                {
                    //_hadrSapsaPass = null;
                    this._sapUserSapsaPassword = null;
                }
                else
                {
                    //_hadrSapsaPass = value;
                    this._sapUserSapsaPassword = value;
                }
            }
        }
        public String HadrDisasterRecoveryUser
        {
            get { return _sapUserDisRecUser; }
            set
            {
                if (value == null)
                {
                    _hadrDisastRecoUsr = null;
                    _sapUserDisRecUser = null;
                }
                else
                {
                    _hadrDisastRecoUsr = value;
                    _sapUserDisRecUser = value;
                }
            }
        }
        public String HadrDisasterRecoveryPassword
        {
            get { return _sapUserDisRecPass; }
            set
            {
                if (value == null)
                {
                    _hadrDisastRecoPass = null;
                    _sapUserDisRecPass = null;
                }
                else
                {
                    _hadrDisastRecoPass = value;
                    _sapUserDisRecPass = value;
                }
            }
        }
        #endregion
        //End the set of START SAP HADR SYBASE

        public ObservableCollection<AddProcessControl> AddProcessFlow
        {
            get { return this._addProcessFlow; }
            set
            {
                this._addProcessFlow = value;
                this.OnPropertyChanged("AddProcessFlow");
            }
        }
        public ObservableCollection<Models.MenuItem> Categories
        {
            get { return _teamsItems; }
        }
        public Models.MenuItem SelectedItem
        {
            get { return this._selectedItem; }
            set
            {
                this._selectedItem = value;
                SelectedProcess = Auxiliar.ProcessInitConfig.Where(x => x.Title == _selectedItem.Title).FirstOrDefault();
                SelectedDescription = this.SelectedProcess.Description;
                if (SelectedProcess.MultipleFlowMode)
                {
                    FlowModes = new ObservableCollection<string>(
                        SelectedProcess.StepList
                       .Where(x => !String.IsNullOrEmpty(x.Flow) && x.Flow != "ALL")
                       .Select(s => s.Flow)
                       .Distinct()
                       .ToList()
                    );
                }
                else
                    FlowModes = new ObservableCollection<string>();
                SelectedFlowMode = "";

                _systemCatalog.Filter = null;

                if (_selectedProcess.ProjectName.Trim().ToUpper() == "SAPINSTALLHANACLOUD" || _selectedProcess.ProjectName.Trim().ToUpper() == "SAPPOSTHANACLOUD")
                {
                    _filteredSystemCatalog = FilterServersByProcessDB;
                }
                else if (_selectedProcess.DBSType != "UNIVERSAL" && _selectedProcess.DBSType != null)
                {
                    if (_selectedProcess.OSType != "UNIVERSAL" && _selectedProcess.OSType != null)
                        _filteredSystemCatalog = o => (FilterServersByProcessDB(o) && FilterServersByProcessOS(o));
                    else
                        _filteredSystemCatalog = FilterServersByProcessDB;
                }
                else if (_selectedProcess.OSType != "UNIVERSAL" && _selectedProcess.OSType != null)
                {
                    if (_selectedProcess.Subtype.ToUpper().Contains("JAVA"))
                        _filteredSystemCatalog = o => (FilterServersByProcessOS(o) && FilterServersByJavaStack(o));
                    else if (_selectedProcess.Subtype.ToUpper().Contains("BOBJ"))
                    {
                        _filteredSystemCatalog = o => (FilterServersByProcessOS(o) && FilterServersBobJ(o));
                        this.Credentials.WebUser = "FITMON_BOBJ";
                        this.Credentials.WebPass = "F1tm0nB0bj";
                    }
                    else
                        _filteredSystemCatalog = FilterServersByProcessOS;
                }
                else _filteredSystemCatalog = null;

                _systemCatalog.Filter = _filteredSystemCatalog;
                _systemCatalog.MoveCurrentTo(null);

                this.OnPropertyChanged("SelectedItem");
            }
        }
        public Process SelectedProcess
        {
            get { return this._selectedProcess; }
            set
            {
                this._selectedProcess = value;
                this.OnPropertyChanged("SelectedProcess");

                Certificate TempCertificate = Auxiliar.Certificates.Where(x => x.Area == SelectedProcess.Team && x.ProcessName == SelectedProcess.ProjectName).FirstOrDefault();
                Models.Authorization TempAuthorizaton = Auxiliar.Authorizations.Where(x => x.Area == SelectedProcess.Team && x.ProcessName == SelectedProcess.ProjectName).FirstOrDefault();

                if ((SelectedProcess.CertificateRequired && TempCertificate != null && TempAuthorizaton != null) || !SelectedProcess.CertificateRequired)// || UserProfile.Department.ToUpper() == "SAP MS AUTOMATION")
                {
                    UserCanExcecuteSelectedProcess = true;
                }
                else
                {
                    UserCanExcecuteSelectedProcess = false;
                }

                this.OnPropertyChanged("ShowsTrainingHiperLink");

                this.OnPropertyChanged("IsMultipleFlowMode");

                bool webCred = false, sapguiCred = false, osCred = false, sidadmCred = false, dbCred = false, dbschemaPass = false;
                
                if (value.Credentials.WebUser != "" && value.Credentials.WebPass != "")
                    webCred = true;
                
                if (value.Credentials.SAPGuiUser != "" && value.Credentials.SAPGuiPass != "")
                    sapguiCred = true;
                
                if (value.Credentials.OSUser != "" && value.Credentials.OSPass != "")
                    osCred = true;
                
                if (value.Credentials.SIDAdmUser != "" && value.Credentials.SIDAdmPass != "")
                    sidadmCred = true;
               
                if (value.Credentials.DBUser != "" && value.Credentials.DBPass != "")
                    dbCred = true;
                
                if (value.Credentials.DBSchemaPass != "")
                    dbschemaPass = true;
                
                Credentials = new Credentials(webCred, sapguiCred, osCred, sidadmCred, dbCred, dbschemaPass);
                
                if (this._selectedProcess.Credentials.ClientsList.Count > 0)
                {
                    Credentials.ClientsList = new List<Credentials.ClientSet>();
                
                    foreach (Credentials.ClientSet clientSet in this._selectedProcess.Credentials.ClientsList)
                    {
                        if (clientSet.IsSelected)
                        {
                            Credentials.ClientsList.Add(clientSet);
                        }
                    }
                    
                    if (Credentials.ClientsList.Any(x => x.IsSelected == true))
                        Credentials.ShowSAPClientListCredentials = true;
                }
                
                if (!String.IsNullOrEmpty(UserProfile.CachedCredentials.SAPGuiUser) && String.IsNullOrEmpty(Credentials.SAPGuiUser))
                {
                    Credentials.SAPGuiUser = UserProfile.CachedCredentials.SAPGuiUser;
                }
                
                if (!String.IsNullOrEmpty(UserProfile.CachedCredentials.SAPGuiPass) && String.IsNullOrEmpty(Credentials.SAPGuiPass))
                {
                    Credentials.SAPGuiPass = UserProfile.CachedCredentials.SAPGuiPass;
                }
                
                if (!String.IsNullOrEmpty(UserProfile.CachedCredentials.SIDAdmUser) && String.IsNullOrEmpty(Credentials.SIDAdmUser))
                {
                    Credentials.SIDAdmUser = UserProfile.CachedCredentials.SIDAdmUser;
                }
                
                if (!String.IsNullOrEmpty(UserProfile.CachedCredentials.SIDAdmPass) && String.IsNullOrEmpty(Credentials.SIDAdmPass))
                {
                    Credentials.SIDAdmPass = UserProfile.CachedCredentials.SIDAdmPass;
                }
                
                if (!String.IsNullOrEmpty(UserProfile.CachedCredentials.DBUser) && String.IsNullOrEmpty(Credentials.DBUser))
                {
                    Credentials.DBUser = UserProfile.CachedCredentials.DBUser;
                }
                
                if (!String.IsNullOrEmpty(UserProfile.CachedCredentials.DBPass) && String.IsNullOrEmpty(Credentials.DBPass))
                {
                    Credentials.DBPass = UserProfile.CachedCredentials.DBPass;
                }
                
                if (!String.IsNullOrEmpty(UserProfile.CachedCredentials.DBSchemaPass) && String.IsNullOrEmpty(Credentials.DBSchemaPass))
                {
                    Credentials.DBSchemaPass = UserProfile.CachedCredentials.DBSchemaPass;
                }

                SourceBackupCV = 8;
            }
        }
        public bool UserCanExcecuteSelectedProcess
        {
            get
            {
                return this._userCanExcecuteSelectedProcess;
            }
            set
            {
                this._userCanExcecuteSelectedProcess = value;
                this.OnPropertyChanged("UserCanExcecuteSelectedProcess");
            }
        }
        public string SelectedDescription
        {
            get
            {
                if (SelectedProcess != null)
                    return SelectedProcess.Description;
                else
                    return "Select a process";
            }
            set
            {
                this.OnPropertyChanged("SelectedDescription");
            }
        }
        public bool ShowsTrainingHiperLink
        {
            get
            {
                if (SelectedProcess == null)
                    return false;
                else
                    return !UserCanExcecuteSelectedProcess;
            }
            set
            {
                this.OnPropertyChanged("ShowsTrainingHiperLink");
            }
        }
        public ICollectionView SystemCatalog
        {
            get
            {
                return this._systemCatalog;
            }
            set
            {
                this._systemCatalog = value;
                this.OnPropertyChanged("SystemCatalog");
            }
        }
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                this._searchString = value.ToUpper();

                var filter = _filteredSystemCatalog;

                if (_searchString != "")
                {
                    _searchCriteriaArray = _searchString.Split(" ").Where(x => !String.IsNullOrEmpty(x.Trim().ToUpper())).ToArray();
                    switch (_searchCriteriaArray.Length)
                    {
                        case 1:
                            if (filter != null)
                                _systemCatalog.Filter = o => (filter(o) && ServersSearchFirstWord(o));
                            else
                                _systemCatalog.Filter = ServersSearchFirstWord;
                            break;
                        case 2:
                            if (filter != null)
                                _systemCatalog.Filter = o => (filter(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                            else
                                _systemCatalog.Filter = o => (ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                            break;
                        case 3:
                            if (filter != null)
                                _systemCatalog.Filter = o => (filter(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                            else
                                _systemCatalog.Filter = o => (ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                            break;
                        case 4:
                            if (filter != null)
                                _systemCatalog.Filter = o => (filter(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                            else
                                _systemCatalog.Filter = o => (ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                            break;
                        case 5:
                            if (filter != null)
                                _systemCatalog.Filter = o => (filter(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                            else
                                _systemCatalog.Filter = o => (ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                            break;
                    }
                }
                else
                {
                    if (filter != null)
                        _systemCatalog.Filter = filter;
                    else
                        _systemCatalog.Filter = null;
                }

                if (SystemCatalog.Cast<ServerSystem>().ToList().Any(x => x.IsSelected == false))
                    SelectAllServersCheckboxIsChecked = false;
                else if (SystemCatalog.Cast<ServerSystem>().ToList().All(x => x.IsSelected == true))
                    SelectAllServersCheckboxIsChecked = true;

            }
        }
        public ServerSystem SelectedServer
        {
            get { return _systemCatalog.CurrentItem as ServerSystem ; }
            set
            {
                if (_controlIndex > 0)
                {
                    this._selectedServer = value;
                    if (_selectedServer != null)
                    {
                        //If the process requires applications
                        if (_selectedProcess.ApplReq == true)
                        {
                            var filter = _filteredSystemCatalog;
                            if (filter != null)
                                _systemCatalog.Filter = o => (filter(o) && FilterServersByServerSID(o));
                            else
                                _systemCatalog.Filter = o => (FilterServersByServerSID(o));
                            for (int i = 0; i < _serverList.Count; i++)
                            {
                                if (_serverList[i].SID == value.SID)
                                {
                                    _serverList[i].IsSelected = true;
                                }
                            }

                        }
                    }
                    this.OnPropertyChanged("SelectedServer");
                    this.OnPropertyChanged("SelectedServers");
                }
            }
        }
        public IEnumerable<ServerSystem> SelectedServers
        {
            get { return _systemCatalog.Cast<ServerSystem>().ToList().Where(o => o.IsSelected); }
        }
        public IEnumerable<ServerSystem> SelectedServersOnSelectedList
        {
            get { return _selectedServerList.Where(o => o.IsSelectedOnSelectedList); }
        }
        public ObservableCollection<ServerSystem> SelectedServersList
        {
            get { return this._selectedServerList; }
            set 
            { 
                this._selectedServerList = value;
                this.OnPropertyChanged("SelectedServersList");
            }
        }
        public void AddSelectedServers(object obj)
        {
            foreach (ServerSystem s in SelectedServers.ToList())
            {
                SelectedServersList.Add(s);
                s.IsSelected = false;
                s.IsEnabled = false;
            }
            SelectAllServersCheckboxIsChecked = false;
            if (SelectedServersList.Count > SelectedServersOnSelectedList.ToList().Count)
                SelectAllServersOnSelectedListCheckboxIsChecked = false;

            if (SelectedProcess.SystemCopyModules != "NA" && CurrentControl.UserControl is SelectTargetSystemView)
            {
                var filter = _filteredSystemCatalog;
                if (filter != null)
                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilterServersTarget(o));
                else
                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilterServersTarget(o));
            }
        }
        public bool CanAddSelectedServers(object obj)
        {
            if (SelectedServers.ToList().Count > 0)
                return true;
            else
                return false;
        }
        public void RemoveSelectedServers(object obj)
        {
            foreach (ServerSystem server in SelectedServersOnSelectedList.ToList())
            {
                SelectedServersList.Remove(server);
                server.IsSelectedOnSelectedList = false;
                server.IsEnabled = true;
            }

            if (SelectedProcess.SystemCopyModules != "NA" && CurrentControl.UserControl is SelectTargetSystemView)
            {
                if (String.IsNullOrEmpty(_searchStringTargetSAPServer))
                    SearchStringTargetSAPServer = "";
                else
                    SearchStringTargetSAPServer = SearchStringTargetSAPServer;
            }
        }
        public bool CanRemoveSelectedServers(object obj)
        {
            if (SelectedServersOnSelectedList.ToList().Count > 0)
                return true;
            else
                return false;
        }
        public bool SelectAllServersCheckboxIsChecked
        {
            get { return this._selectAllServersCheckboxIsChecked; }
            set
            {
                this._selectAllServersCheckboxIsChecked = value;
                this.OnPropertyChanged("SelectAllServersCheckboxIsChecked");
            }
        }
        public void ChangeSelectAllServers(object obj)
        {
            CheckBox checkbok = (obj as CheckBox);
            foreach (ServerSystem server in SystemCatalog)
            {
                if (server.IsEnabled == true)
                {
                    if (checkbok.IsChecked.HasValue && checkbok.IsChecked.Value)
                    {
                        server.IsSelected = true;
                    }
                    else
                    {
                        server.IsSelected = false;
                    }
                }
            }
        }
        public bool CanChangeSelectAllServers(object obj)
        {
            if (!SystemCatalog.IsEmpty)
                return true;
            else
                return false;
        }
        public bool SelectAllServersOnSelectedListCheckboxIsChecked
        {
            get { return this._selectAllServersOnSelectedListCheckboxIsChecked; }
            set
            {
                this._selectAllServersOnSelectedListCheckboxIsChecked = value;
                this.OnPropertyChanged("SelectAllServersOnSelectedListCheckboxIsChecked");
            }
        }
        public void ChangeSelectAllServersOnSelectedList(object obj)
        {
            CheckBox checkbok = (obj as CheckBox);
            foreach (ServerSystem server in SelectedServersList)
            {
                if (checkbok.IsChecked.HasValue && checkbok.IsChecked.Value)
                {
                    server.IsSelectedOnSelectedList = true;
                }
                else
                {
                    server.IsSelectedOnSelectedList = false;
                }
            }
        }
        public bool CanChangeSelectAllServersOnSelectedList(object obj)
        {
            if (SelectedServersList.Count > 0)
                return true;
            else
                return false;
        }
        public ServerSystem RemoveSelectedServer
        {
            get { return null; }
            set
            {
                ServerSystem server = (value as ServerSystem);
                server.IsSelected = false;
            }
        }
        public ServerSystem SummarySelectedServer
        {
            get { return null; }
            set
            {
                if (ExtraInputs != null)
                {
                    IEnumerable<ExtraInputsSet> inputs = ExtraInputs.Where(x => x.System == value.SID);
                    if (inputs != null)
                    {
                        SummaryExtraIputSet = new ObservableCollection<ExtraInputsSet>();
                        foreach (ExtraInputsSet set in inputs)
                        {
                            SummaryExtraIputSet.Add(set);
                        }
                    }
                }
            }
        }
        public IEnumerable<Step> SelectedSteps
        {
            get { return SelectedProcess.StepList.Where(o => o.IsSelected); }
        }
        public List<ExtraInputsSet> ExtraInputsLayout
        {
            get { return this._inptutsextralayout; }
            set
            {
                this._inptutsextralayout = value;
                this.OnPropertyChanged("ExtraInputsLayout");
            }
        }
        public ObservableCollection<ExtraInputsSet> ExtraInputs
        {
            get { return this._inptutsextra; }
            set
            {
                this._inptutsextra = value;
                this.OnPropertyChanged("ExtraInputs");
            }
        }
        public ObservableCollection<ExtraInputsSet> SummaryExtraIputSet
        {
            get { return this._summaryinptutsextra; }
            set 
            {
                this._summaryinptutsextra = value;
                this.OnPropertyChanged("SummaryExtraIputSet");
            }
        }
        public UserControl PackageView
        {
            get { return this._packageView; }
            set
            {
                this._packageView = value;
                this.OnPropertyChanged("PackageView");
            }
        }
        public Credentials Credentials
        {
            get { return this._credentials; }
            set { this._credentials = value; }
        }
        public ObservableCollection<string> EmailDest
        {
            get { return this._emailDest; }
            set
            {
                this._emailDest = value;
                this.OnPropertyChanged("EmailDest");
            }
        }
        public SapInstall SapInstall
        {
            get { return this._sapInstall; }
            set { this._sapInstall = value; }
        }
        public AddProcessControl CurrentControl
        {
            get { return this._currentControl; }
            set
            {
                this._currentControl = value;
                this.OnPropertyChanged("CurrentControl");
                this._currentControl.IsActive = true;
            }
        }
        public string FavoritesName
        {
            get; set;
        }
        public void ChangeToPrincipalVM(object obj)
        {
            MainWindow myWindow = Application.Current.MainWindow as MainWindow;
            myWindow.SetPrincipalDataContext();
        }
        public bool CanChangeView(object obj) { return true; }
        public static ImportTable ExportToImport(ExportTable exportTables)
        {
            return new ImportTable((exportTables.Name), (exportTables.Description));
        }
        public void ChangeToNextView(object obj) 
        {

            string tempVersion;

            _controlIndex++;
            if (CurrentControl.UserControl is SapInstallStepsToExecute && _addedSapPAViews == false)
            {
                AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallLicenseFile(), MenuItem = new Models.MenuItem { Title = "License File Path", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                if (ShowRZ10Window)
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallRZ10(), MenuItem = new Models.MenuItem { Title = "RZ10 Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallDB13(), MenuItem = new Models.MenuItem { Title = "DB13 Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallSM36(), MenuItem = new Models.MenuItem { Title = "SM36 Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallSMLG(), MenuItem = new Models.MenuItem { Title = "SMLG Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallRZ12(), MenuItem = new Models.MenuItem { Title = "RZ12 Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallSM61(), MenuItem = new Models.MenuItem { Title = "SM61 Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallRZ04(), MenuItem = new Models.MenuItem { Title = "RZ04 Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallRZ70(), MenuItem = new Models.MenuItem { Title = "RZ70 Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallAL11(), MenuItem = new Models.MenuItem { Title = "AL11 Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallSTRUST02(), MenuItem = new Models.MenuItem { Title = "STRUST02 Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallSCC4(), MenuItem = new Models.MenuItem { Title = "SCC4 Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallSM21(), MenuItem = new Models.MenuItem { Title = "SM21 Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallST22(), MenuItem = new Models.MenuItem { Title = "ST22 Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                AddProcessFlow.Add(new AddProcessControl { UserControl = new EmailDestinationsView(), MenuItem = new Models.MenuItem { Title = "Email Destinations", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                AddProcessFlow.Add(new AddProcessControl { UserControl = new Summary(), MenuItem = new Models.MenuItem { Title = "Summary", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                _addedSapPAViews = true;
            }
            if (CurrentControl.UserControl is SelectProcessView)
            {
                if (SelectedProcess.ProjectName.ToUpper().Equals("STARTSAPCRMSYBASEHACLOUD") || SelectedProcess.ProjectName.ToUpper().Equals("STOPSAPCRMSYBASEHACLOUD"))
                {
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new PrimaryDBServer(), MenuItem = new Models.MenuItem { Title = "Select Primary DB Server", Icon = "\\img\\icons\\processing.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new StandbyDBServer(), MenuItem = new Models.MenuItem { Title = "Select Standby DB Server", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SapAcscScsServer(), MenuItem = new Models.MenuItem { Title = "Select SAP ASCS/SCS Server", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SapErsServer(), MenuItem = new Models.MenuItem { Title = "Select SAP ERS Server", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SapAasServer(), MenuItem = new Models.MenuItem { Title = "Select Additional Application Servers", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SAPUserData(), MenuItem = new Models.MenuItem { Title = "Additional Sybase Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new EmailDestinationsView(), MenuItem = new Models.MenuItem { Title = "Email Destinations", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SummaryHadr(), MenuItem = new Models.MenuItem { Title = "Summary", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                }
                else if (SelectedProcess.ProjectName.ToUpper().Equals("SAPQC") || SelectedProcess.ProjectName.ToUpper().Equals("SAPQCNGZTCLOUD"))
                {
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SelectServersView(), MenuItem = new Models.MenuItem { Title = "Select System(s)", Icon = "\\img\\icons\\processing.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new AdditionalSettings(), MenuItem = new Models.MenuItem { Title = "Additional Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new TransactionsPostactivities(), MenuItem = new Models.MenuItem { Title = "SAP Transactions Post activities", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallSM21(), MenuItem = new Models.MenuItem { Title = "SM21 Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallST22(), MenuItem = new Models.MenuItem { Title = "ST22 Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new EmailDestinationsView(), MenuItem = new Models.MenuItem { Title = "Email Destinations", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new Summary(), MenuItem = new Models.MenuItem { Title = "Summary", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                }
                else if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLPOSTACTIVITIES") || SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLPOST"))
                {
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SelectServersView(), MenuItem = new Models.MenuItem { Title = "Select System(s)", Icon = "\\img\\icons\\processing.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new AdditionalSettings(), MenuItem = new Models.MenuItem { Title = "Additional Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new TransactionsPostactivities(), MenuItem = new Models.MenuItem { Title = "SAP Transactions Post activities", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallStepsToExecute(), MenuItem = new Models.MenuItem { Title = "Steps to Execute", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                }
                else if (SelectedProcess.ProjectName.ToUpper().Equals("SAPPOSTHANACLOUD"))
                {
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SelectServersView(), MenuItem = new Models.MenuItem { Title = "Select System(s)", Icon = "\\img\\icons\\processing.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SapHanaData(), MenuItem = new Models.MenuItem { Title = "Insert Additional Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new EmailDestinationsView(), MenuItem = new Models.MenuItem { Title = "Email Destinations", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SummarySAPInstallPostHANA(), MenuItem = new Models.MenuItem { Title = "Summary", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                }
                else if (SelectedProcess.ProjectName.ToUpper().Equals("SAPPOSTORACLOUD"))
                {
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SelectServersView(), MenuItem = new Models.MenuItem { Title = "Select System(s)", Icon = "\\img\\icons\\processing.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SapOracleData(), MenuItem = new Models.MenuItem { Title = "Insert Additional Settings", Icon = "\\img\\icons\\processing.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new EmailDestinationsView(), MenuItem = new Models.MenuItem { Title = "Email Destinations", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SummarySAPInstallPostOracle(), MenuItem = new Models.MenuItem { Title = "Summary", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                }
                else if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLCLOUD"))
                {
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SelectServersView(), MenuItem = new Models.MenuItem { Title = "Select System(s)", Icon = "\\img\\icons\\processing.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallData(), MenuItem = new Models.MenuItem { Title = "Input SAP Data", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    if (SelectedProcess.SapInstallCatalogs.Count() > 0)
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new ServerInfo(), MenuItem = new Models.MenuItem { Title = "SAP Install Catalog", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new EmailDestinationsView(), MenuItem = new Models.MenuItem { Title = "Email Destinations", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SummarySAPInstall(), MenuItem = new Models.MenuItem { Title = "Summary", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                }
                else if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLORACLECLOUD"))
                {
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SelectServersView(), MenuItem = new Models.MenuItem { Title = "Select System(s)", Icon = "\\img\\icons\\processing.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallDataOracle(), MenuItem = new Models.MenuItem { Title = "Input SAP Data", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    if (SelectedProcess.SapInstallCatalogs.Count() > 0)
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new ServerInfo(), MenuItem = new Models.MenuItem { Title = "SAP Install Catalog", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new EmailDestinationsView(), MenuItem = new Models.MenuItem { Title = "Email Destinations", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SummarySAPInstallOracle(), MenuItem = new Models.MenuItem { Title = "Summary", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                }
                else if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLHANACLOUD"))
                {
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SelectServersView(), MenuItem = new Models.MenuItem { Title = "Select System(s)", Icon = "\\img\\icons\\processing.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallDataHana2N(), MenuItem = new Models.MenuItem { Title = "Input SAP Data", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    if (SelectedProcess.SapInstallCatalogs.Count() > 0)
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new ServerInfo(), MenuItem = new Models.MenuItem { Title = "SAP Install Catalog", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new EmailDestinationsView(), MenuItem = new Models.MenuItem { Title = "Email Destinations", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SummarySAPInstallHana2N(), MenuItem = new Models.MenuItem { Title = "Summary", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                }
                else if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                {
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SelectServersView(), MenuItem = new Models.MenuItem { Title = "Select System(s)", Icon = "\\img\\icons\\processing.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SapInstallDataAAS(), MenuItem = new Models.MenuItem { Title = "Input SAP Data", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    if (SelectedProcess.SapInstallCatalogs.Count() > 0)
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new ServerInfo(), MenuItem = new Models.MenuItem { Title = "SAP Install Catalog", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new EmailDestinationsView(), MenuItem = new Models.MenuItem { Title = "Email Destinations", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SummarySAPInstallAAS(), MenuItem = new Models.MenuItem { Title = "Summary", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                }
                else if (SelectedProcess.ProjectName.ToUpper().Contains("DB2"))
                {
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SelectServersView(), MenuItem = new Models.MenuItem { Title = "Select System(s)", Icon = "\\img\\icons\\processing.png" } });
                    if (SelectedProcess.Db2InstallCatalogs.Count() > 0)
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new Db2InstallSettings(), MenuItem = new Models.MenuItem { Title = "Additional Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new EmailDestinationsView(), MenuItem = new Models.MenuItem { Title = "Email Destinations", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new Summary(), MenuItem = new Models.MenuItem { Title = "Summary", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                }
                else if (SelectedProcess.SystemCopyModules != "NA" && SelectedProcess.SystemCopyModules != null)
                {
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SourceDB(), MenuItem = new Models.MenuItem { Title = "Select Source DB Server", Icon = "\\img\\icons\\processing.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SelectTargetSystemView(), MenuItem = new Models.MenuItem { Title = "Select Target System", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    if (Credentials.ShowSAPClientListCredentials == true || RestoreDBIsAutomatic || Credentials.ShowDBGroupBox)
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new AdditionalSettings(), MenuItem = new Models.MenuItem { Title = "Additional Settings", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    if (SelectedProcess.SystemCopyModules == "ALL" || SelectedProcess.SystemCopyModules.ToUpper().Contains("ACTIVITIES"))
                    {
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new TransactionsPreactivities(), MenuItem = new Models.MenuItem { Title = "SAP Transactions Pre activities", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new ExportTables(), MenuItem = new Models.MenuItem { Title = "SAP Export Catalog Tables", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new TransactionsPostactivities(), MenuItem = new Models.MenuItem { Title = "SAP Transactions Post activities", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new ImportTables(), MenuItem = new Models.MenuItem { Title = "SAP Import Catalog Tables", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    }
                    if (ShowBDLSList)
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new BDLSView(), MenuItem = new Models.MenuItem { Title = "SAP Logical System Conversion", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new EmailDestinationsView(), MenuItem = new Models.MenuItem { Title = "Email Destinations", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SummaryRefresh(), MenuItem = new Models.MenuItem { Title = "Summary", Icon = "\\img\\icons\\stepsarrow-b.png" } });

                    foreach (ServerSystem server in _serverList)
                    {
                        server.IsSelectedOnSourceDB = false;
                        server.IsSelectedOnTargetSAP = false;
                        server.IsSelectedOnTargetDB = false;
                    }
                }
                else
                {
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new SelectServersView(), MenuItem = new Models.MenuItem { Title = "Select System(s)", Icon = "\\img\\icons\\processing.png" } });

                    var selectedStepsWithExtraInputs = SelectedSteps.Where(s => s.ExtraInputs.InputsSet != null).ToList();

                    if (selectedStepsWithExtraInputs.Count() > 0)
                    {
                        if (SelectedProcess.TransactionsPackages.Count() > 0)
                        {
                        }
                        _inptutsextralayout = new List<ExtraInputsSet>();
                        foreach (Step s in selectedStepsWithExtraInputs)
                        {
                            ExtraInputsSet set = new ExtraInputsSet(SelectedProcess.ProjectName, s.Name);
                            foreach (ExtraInput i in s.ExtraInputs.InputsSet)
                            {
                                set.AddInput(i);
                            }
                            _inptutsextralayout.Add(set);
                        }

                        Models.MenuItem detailsMenuItem = new Models.MenuItem { Title = "Additional Settings", Icon = "\\img\\icons\\stepsarrow-b.png", IsExpanded = true };
                        detailsMenuItem.Items.CollectionChanged += MenuItemsCollectionChanged;
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new ProcessDetailsView(), MenuItem = detailsMenuItem });

                    }
                    else
                    {
                        ExtraInputsLayout = null;
                        ExtraInputs = null;
                        SummaryExtraIputSet = null;
                    }

                    if (SelectedProcess.OraclePackages.Count() > 0)
                    {
                        Models.MenuItem detailsMenuItem = new Models.MenuItem { Title = "Select Oracle Packages", Icon = "\\img\\icons\\stepsarrow-b.png", IsExpanded = true };
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new PackageView(), MenuItem = detailsMenuItem });
                    }
                    else if (SelectedProcess.SAPHostAgentPackages.Count() > 0)
                    {
                        Models.MenuItem detailsMenuItem = new Models.MenuItem { Title = "Additional Settings", Icon = "\\img\\icons\\stepsarrow-b.png", IsExpanded = true };
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new PackageView(), MenuItem = detailsMenuItem });
                    }
                    else if (SelectedProcess.JavaComponents.Count() > 0)
                    {
                        Models.MenuItem detailsMenuItem = new Models.MenuItem { Title = "Select Java Components", Icon = "\\img\\icons\\stepsarrow-b.png", IsExpanded = true };
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new SelectJavaComponents(), MenuItem = detailsMenuItem });
                    }
                    else if (SelectedProcess.SAPKernelPackages.Count() > 0)
                    {
                        Models.MenuItem detailsMenuItem = new Models.MenuItem { Title = "Additional Settings", Icon = "\\img\\icons\\stepsarrow-b.png", IsExpanded = true };
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new SelectSAPKernelPatches(), MenuItem = detailsMenuItem });
                    }
                    if (SelectedProcess.SAPKernelPackages2.Count() > 0)
                    {
                        Models.MenuItem detailsMenuItem = new Models.MenuItem { Title = "Additional Settings 2", Icon = "\\img\\icons\\stepsarrow-b.png", IsExpanded = true };
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new SelectSAPKernelPatches2(), MenuItem = detailsMenuItem });
                        AddProcessFlow.Add(new AddProcessControl { UserControl = new UserControl1(), MenuItem = new Models.MenuItem { Title = "Set Inputs", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    }
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new EmailDestinationsView(), MenuItem = new Models.MenuItem { Title = "Email Destinations", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                    AddProcessFlow.Add(new AddProcessControl { UserControl = new Summary(), MenuItem = new Models.MenuItem { Title = "Summary", Icon = "\\img\\icons\\stepsarrow-b.png" } });
                }
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallData)
            {
                DbScehmaName = "SAPHANADB";
                VirtualHost = SelectedServersList[0].Hostname;
                VirtHostInter = "eth0";
                DomainName = "NA";
                SapSId = SelectedServersList[0].SID;
                this.varToCheck = false;
                this.varToCheckAscs = false;
                this.varToCheckPas = false;
                this.varToCheckInN = false;
                this.varToCheckDbn = false;
                this.varToCheckSnm = true;
                this.varToCheckVir = true;
                this.varToCheckVhi = true;
                this.varToCheckDmn = true;
                this.varToCheckSysG = false;
                this.varToCheckInsG = false;
                this.varToCheckDsiU = false;
                this.varToCheckDsiG = false;
                this.varToCheckSidU = false;
                this.varToCheckAdmU = false;
            }
            else if (_addProcessFlow[_controlIndex].UserControl is AdditionalSettings && (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLPOSTACTIVITIES") || SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLPOST") || SelectedProcess.ProjectName.ToUpper().Equals("SAPQC") || SelectedProcess.ProjectName.ToUpper().Equals("SAPQCNGZTCLOUD")))
            {
                SelectedTargetSAPServer = SelectedServersList.Where(s => s.CIDI.ToUpper().Trim().Contains("CI")).FirstOrDefault();
            }
            else if (_addProcessFlow[_controlIndex].UserControl is Db2InstallSettings)
            {
                string sapSidLinux = "", sapSidAIX = "";

                foreach (ServerSystem server in SelectedServersList)
                {
                    if (server.OS.ToUpper() == "LINUX")
                        sapSidLinux = server.SID;

                    else if (server.OS.ToUpper() == "AIX")
                        sapSidAIX = server.SID;
                }

                Db2InstallDb = SelectedServersList[0].DBType.ToUpper();
                Db2InstallOsType = SelectedServersList[0].OS.ToUpper();
                //SelectedTargetSAPServer = SelectedServersList.Where(s => s.DBType.ToUpper().Trim().Contains("DB2")).FirstOrDefault();

                if (sapSidLinux != currentSidLinux)
                {
                    SelectedDb2InstallCatalogFinalForLinux = null;
                    currentSidLinux = sapSidLinux;
                    Pacemaker = "";
                }

                if (sapSidAIX != currentSidAIX)
                {
                    SelectedDb2InstallCatalogFinalForAIX = null;
                    currentSidAIX = sapSidAIX;
                }

                if (SelectedDb2InstallCatalogFinalForLinux == null)
                {
                    //For Linux
                    AvailableDb2InstallCatalogLinux = new ObservableCollection<string>();
                    AvailableDb2InstallFilesForLinux = new ObservableCollection<Db2Install>();
                    AvailableDb2InstallCatalogOsDistributionForLinux = new ObservableCollection<string>();
                    AvailableDb2InstallCatalogOsArchitectureForLinux = new ObservableCollection<string>();
                    AvailableDb2InstallCatalogDbVersionForLinux = new ObservableCollection<string>();
                    AvailableDb2InstallCatalogDbPatchForLinux = new ObservableCollection<string>();

                    SelectedDb2InstallOsDistributionForLinux = null;
                    SelectedDb2InstallOsArchitectureForLinux = null;
                    SelectedDb2InstallDbVersionForLinux = null;
                    SelectedDb2InstallCatalogDbPatchForLinux = null;

                    SelectedDb2InstallCatalogForLinux = null;
                }
                if (SelectedDb2InstallCatalogFinalForAIX == null)
                {
                    //For AIX
                    AvailableDb2InstallFilesForAIX = new ObservableCollection<Models.DB2Install.Db2Install>();
                    AvailableDb2InstallCatalogOsDistributionForAIX = new ObservableCollection<string>();
                    AvailableDb2InstallCatalogOsArchitectureForAIX = new ObservableCollection<string>();
                    AvailableDb2InstallCatalogDbVersionForAIX = new ObservableCollection<string>();
                    AvailableDb2InstallCatalogDbPatchForAIX = new ObservableCollection<string>();

                    SelectedDb2InstallOsDistributionForAIX = null;
                    SelectedDb2InstallOsArchitectureForAIX = null;
                    SelectedDb2InstallDbVersionForAIX = null;
                    SelectedDb2InstallCatalogDbPatchForAIX = null;

                    SelectedDb2InstallCatalogForAIX = null;
                }

                string tempOsDistLinux, tempOsArchLinux, tempDbVerLinux, tempDbPatLinux, tempOsDistAIX, tempOsArchAIX, tempDbVerAIX, tempDbPatAIX;

                foreach (Db2Install pkg in SelectedProcess.Db2InstallCatalogs)
                {
                    switch (pkg.OsType.Trim().ToUpper())
                    {
                        case "LINUX":
                            AvailableDb2InstallFilesForLinux.Add(pkg);
                            SelectedDb2InstallCatalogFinalForLinux = "LINUX";
                            tempOsDistLinux = AvailableDb2InstallCatalogOsDistributionForLinux.Where(x => x == pkg.OsDistribution).FirstOrDefault();
                            tempOsArchLinux = AvailableDb2InstallCatalogOsArchitectureForLinux.Where(x => x == pkg.OsArchitecture).FirstOrDefault();
                            tempDbVerLinux = AvailableDb2InstallCatalogDbVersionForLinux.Where(x => x == pkg.DbVersion).FirstOrDefault();
                            tempDbPatLinux = AvailableDb2InstallCatalogDbPatchForLinux.Where(x => x == pkg.DbPatch).FirstOrDefault();

                            if (tempOsDistLinux == null)
                                AvailableDb2InstallCatalogOsDistributionForLinux.Add(pkg.OsDistribution);
                            break;

                        case "AIX":
                            AvailableDb2InstallFilesForAIX.Add(pkg);
                            SelectedDb2InstallCatalogFinalForAIX = "AIX";
                            tempOsDistAIX = AvailableDb2InstallCatalogOsDistributionForAIX.Where(x => x == pkg.OsDistribution).FirstOrDefault();
                            tempOsArchAIX = AvailableDb2InstallCatalogOsArchitectureForAIX.Where(x => x == pkg.OsArchitecture).FirstOrDefault();
                            tempDbVerAIX = AvailableDb2InstallCatalogDbVersionForAIX.Where(x => x == pkg.DbVersion).FirstOrDefault();
                            tempDbPatAIX = AvailableDb2InstallCatalogDbPatchForAIX.Where(x => x == pkg.DbPatch).FirstOrDefault();

                            if (tempOsDistAIX == null)
                                AvailableDb2InstallCatalogOsDistributionForAIX.Add(pkg.OsDistribution);
                            break;
                    }
                }
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallData)
            {
                if (SelectedServersList[0].DBType == "ORACLE")
                {
                    DbOracle = true;
                    DbHana = false;
                }
                else if (SelectedServersList[0].DBType == "HANA")
                {
                    DbHana = true;
                    DbOracle = false;
                }

                DbScehmaName = "SAPHANADB";
                VirtualHost = SelectedServersList[0].Hostname;
                VirtHostInter = "eth0";
                DomainName = "NA";
                SapSId = SelectedServersList[0].SID;
                MasterPass = "Mast4adm123";

                if (SapSId != currentSID)
                {
                    SidAdmUId = null;
                    HanaDbName = null;
                    DbSIdAdmUId = null;
                    DbSIdAdmGId = null;
                    AscsInstNum = null;
                    PasInstNum = null;
                    SapSysGId = null;
                    SapInsGId = null;
                    SapAdmUId = null;
                    HanaInstNum = null;
                    currentSID = SapSId;
                }

                if (SelectedServersList[0].Environment.Trim().ToUpper() == "DEV")
                {
                    AscsInstNum = "01";
                    PasInstNum = "00";
                    SapSysGId = "200";
                    SapInsGId = "202";
                    SapAdmUId = "1500";
                    HanaInstNum = "03";
                }
                else if (SelectedServersList[0].Environment.Trim().ToUpper() == "QAS")
                {
                    AscsInstNum = "11";
                    PasInstNum = "10";
                    SapSysGId = "200";
                    SapInsGId = "202";
                    SapAdmUId = "1500";
                    HanaInstNum = "13";
                }
                else if (SelectedServersList[0].Environment.Trim().ToUpper() == "PRD")
                {
                    AscsInstNum = "21";
                    PasInstNum = "20";
                    SapSysGId = "200";
                    SapInsGId = "202";
                    SapAdmUId = "1500";
                    HanaInstNum = "23";
                }
                else
                {
                    AscsInstNum = "31";
                    PasInstNum = "30";
                    SapSysGId = "200";
                    SapInsGId = "202";
                    SapAdmUId = "1500";
                    HanaInstNum = "33";
                }

            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallDataOracle)
            {
                int sid = 0;

                DbOracle = true;
                DbHana = false;
                for (int i = 0; i < SelectedServersList.Count(); i++)
                {
                    if (SelectedServersList[i].CIDI.Trim().ToUpper() == "CI")
                    {
                        SapSId = SelectedServersList[i].SID;
                        sid = 0;
                        break;
                    }
                    else
                        sid++;
                }
                if (sid != 0)
                    SapSId = null;
                DomainName = "NA";
                DatabaseName = SapSId;
                MasterPass = "Mast4adm123";
                if (OraSidGId == null || (OraSidGId != "201" && SapSId != currentSID))
                    OraSidGId = "201";
                if (OraSidUId == null || (OraSidUId != "1600" && SapSId != currentSID))
                    OraSidUId = "1600";
                OracleListenerPort = "1521";

                DatabaseHn = null;
                DatabaseVirtualHn = null;
                SapHostname = null;
                SapVirtualHostname = null;

                if (SapSId != currentSID || SapSId == null)
                {
                    SidAdmUId = null;
                    AscsInstNum = null;
                    PasInstNum = null;
                    SapSysGId = null;
                    SapInsGId = null;
                    SapAdmUId = null;
                    currentSID = SapSId;
                }

                if (SelectedServersList.Count() == 1)
                {
                    if (SelectedServersList[0].CIDI.Trim().ToUpper() == "CI")
                    {
                        DatabaseHn = SelectedServersList[0].Hostname;
                        DatabaseVirtualHn = SelectedServersList[0].Hostname;
                    }
                }

                for (int i = 0; i < SelectedServersList.Count(); i++)
                {
                    if (SelectedServersList[i].CIDI.Trim().ToUpper() == "CI")
                    {
                        SapHostname = SelectedServersList[i].Hostname;
                        SapVirtualHostname = SelectedServersList[i].Hostname;
                        if (SelectedServersList[i].Environment.Trim().ToUpper() == "DEV")
                        {
                            AscsInstNum = "01";
                            PasInstNum = "00";
                            SapSysGId = "200";
                            SapInsGId = "202";
                            SapAdmUId = "1500";
                        }
                        else if (SelectedServersList[i].Environment.Trim().ToUpper() == "QAS")
                        {
                            AscsInstNum = "11";
                            PasInstNum = "10";
                            SapSysGId = "200";
                            SapInsGId = "202";
                            SapAdmUId = "1500";
                        }
                        else if (SelectedServersList[i].Environment.Trim().ToUpper() == "PRD")
                        {
                            AscsInstNum = "21";
                            PasInstNum = "20";
                            SapSysGId = "200";
                            SapInsGId = "202";
                            SapAdmUId = "1500";
                        }
                        else
                        {
                            AscsInstNum = "31";
                            PasInstNum = "30";
                            SapSysGId = "200";
                            SapInsGId = "202";
                            SapAdmUId = "1500";
                        }
                    }
                    else if (SelectedServersList[i].CIDI.ToUpper() == "DO")
                    {
                        DatabaseHn = SelectedServersList[i].Hostname;
                        DatabaseVirtualHn = SelectedServersList[i].Hostname;
                    }
                }
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallDataHana2N)
            {
                DbOracle = false;
                DbHana = true;
                int sid = 0;
                for (int i = 0; i < SelectedServersList.Count(); i++)
                {
                    if (SelectedServersList[i].CIDI.Trim().ToUpper() == "CI")
                    {
                        SapSId = SelectedServersList[0].SID;
                        sid = 0;
                        break;
                    }
                    else
                        sid++;
                }
                if (sid != 0)
                    SapSId = null;
                VirtHostInter = "eth0";
                DomainName = "NA";
                DbScehmaName = "SAPHANADB";
                MasterPass = "Mast4adm123";
                SapVirtualHostname = null;
                DatabaseVirtualHn = null;

                if (SapSId != currentSID || SapSId == null)
                {
                    SidAdmUId = null;
                    VirtualHostSap = null;
                    SapHostname = null;
                    VirtualHost = null;
                    DatabaseHn = null;
                    AscsInstNum = null;
                    PasInstNum = null;
                    SapSysGId = null;
                    SapInsGId = null;
                    SapAdmUId = null;
                    HanaInstNum = null;
                    HanaDbName = null;
                    currentSID = SapSId;
                }

                if (SelectedServersList.Count() == 2)
                {
                    HanaDbName = SapSId;
                    for (int i = 0; i < SelectedServersList.Count(); i++)
                    {
                        if (SelectedServersList[i].CIDI.Trim().ToUpper() == "CI")
                        {
                            SapHostname = SelectedServersList[i].Hostname;
                            SapVirtualHostname = SapHostname;
                            if (SelectedServersList[0].Environment.Trim().ToUpper() == "DEV")
                            {
                                AscsInstNum = "01";
                                PasInstNum = "00";
                                SapSysGId = "200";
                                SapInsGId = "202";
                                SapAdmUId = "1500";
                                HanaInstNum = "00";
                            }
                            else if (SelectedServersList[0].Environment.Trim().ToUpper() == "QAS")
                            {
                                AscsInstNum = "11";
                                PasInstNum = "10";
                                SapSysGId = "200";
                                SapInsGId = "202";
                                SapAdmUId = "1500";
                                HanaInstNum = "10";
                            }
                            else if (SelectedServersList[0].Environment.Trim().ToUpper() == "PRD")
                            {
                                AscsInstNum = "21";
                                PasInstNum = "20";
                                SapSysGId = "200";
                                SapInsGId = "202";
                                SapAdmUId = "1500";
                                HanaInstNum = "20";
                            }
                            else
                            {
                                AscsInstNum = "31";
                                PasInstNum = "30";
                                SapSysGId = "200";
                                SapInsGId = "202";
                                SapAdmUId = "1500";
                                HanaInstNum = "30";
                            }
                        }
                        else if (SelectedServersList[i].CIDI.Trim().ToUpper() == "DB" || SelectedServersList[i].CIDI.Trim().ToUpper() == "")
                        {
                            DatabaseHn = SelectedServersList[i].Hostname;
                            DatabaseVirtualHn = DatabaseHn;
                        }
                    }
                }
                else if (SelectedServersList.Count() == 1)
                {
                    HanaDbName = null;
                    for (int i = 0; i < SelectedServersList.Count(); i++)
                    {
                        if (SelectedServersList[i].CIDI.Trim().ToUpper() == "CI")
                        {
                            SapHostname = SelectedServersList[i].Hostname;
                            DatabaseHn = SelectedServersList[i].Hostname;
                            SapVirtualHostname = SapHostname;
                            DatabaseVirtualHn = DatabaseHn;

                            if (SelectedServersList[0].Environment.Trim().ToUpper() == "DEV")
                            {
                                AscsInstNum = "01";
                                PasInstNum = "00";
                                SapSysGId = "200";
                                SapInsGId = "202";
                                SapAdmUId = "1500";
                                HanaInstNum = "03";
                            }
                            else if (SelectedServersList[0].Environment.Trim().ToUpper() == "QAS")
                            {
                                AscsInstNum = "11";
                                PasInstNum = "10";
                                SapSysGId = "200";
                                SapInsGId = "202";
                                SapAdmUId = "1500";
                                HanaInstNum = "13";
                            }
                            else if (SelectedServersList[0].Environment.Trim().ToUpper() == "PRD")
                            {
                                AscsInstNum = "21";
                                PasInstNum = "20";
                                SapSysGId = "200";
                                SapInsGId = "202";
                                SapAdmUId = "1500";
                                HanaInstNum = "23";
                            }
                            else
                            {
                                AscsInstNum = "31";
                                PasInstNum = "30";
                                SapSysGId = "200";
                                SapInsGId = "202";
                                SapAdmUId = "1500";
                                HanaInstNum = "33";
                            }
                        }
                        else if (SelectedServersList[i].CIDI.Trim().ToUpper() == "DB" || SelectedServersList[i].CIDI.Trim().ToUpper() == "")
                        {
                            DatabaseHn = SelectedServersList[i].Hostname;
                            DatabaseVirtualHn = DatabaseHn;
                        }
                    }
                }

            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapHanaData)
            {
                DbOracle = false;
                DbHana = true;

                HanaInstNum = SelectedServersList[0].Instance;
                HanaDbName = SelectedServersList[0].SID;
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapOracleData)
            {
                DbOracle = true;
                DbHana = false;

                TaskOracleCheck = false;
                TaskOracleFix = false;

            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallDataAAS)
            {
                int sid = 0;
                for (int i = 0; i < SelectedServersList.Count(); i++)
                {
                    if (SelectedServersList[i].CIDI.Trim().ToUpper() == "CI")
                    {
                        SapSId = SelectedServersList[0].SID;
                        sid = 0;
                        break;
                    }
                    else
                        sid++;
                }
                if (sid != 0)
                    SapSId = null;
                DomainName = "NA";
                MasterPass = "Mast4adm123";

                if (SapSId != currentSID || SapSId == null)
                {
                    SidAdmUId = null;
                    AscsInstNum = null;
                    SapInsGId = null;
                    SapAdmUId = null;
                    SapPasHnm = null;
                    SapAasHnm = null;
                    SapAasVHnm = null;
                    currentSID = SapSId;
                }

                if (SelectedServersList.Count() == 2)
                {

                    for (int i = 0; i < SelectedServersList.Count(); i++)
                    {
                        if (SelectedServersList[i].CIDI.Trim().ToUpper() == "CI")
                        {
                            SapPasHnm = SelectedServersList[i].Hostname;
                            if (SelectedServersList[0].Environment.Trim().ToUpper() == "DEV")
                            {
                                SapInsGId = "202";
                                SapAdmUId = "1500";
                            }
                            else if (SelectedServersList[0].Environment.Trim().ToUpper() == "QAS")
                            {
                                SapInsGId = "202";
                                SapAdmUId = "1500";
                            }
                            else if (SelectedServersList[0].Environment.Trim().ToUpper() == "PRD")
                            {
                                SapInsGId = "202";
                                SapAdmUId = "1500";
                            }
                            else
                            {
                                SapInsGId = "202";
                                SapAdmUId = "1500";
                            }
                        }
                        else if (SelectedServersList[i].CIDI.Trim().ToUpper() == "DI")
                        {
                            SapAasHnm = SelectedServersList[i].Hostname;
                            SapAasVHnm = SelectedServersList[i].Hostname;
                        }
                    }
                }
            }
            else if (_addProcessFlow[_controlIndex].UserControl is ServerInfo)
            {
                if (SelectedServersList[0].DBType == "ORACLE")
                {
                    DbOracle = true;
                    DbHana = false;
                }
                else if (SelectedServersList[0].DBType == "HANA")
                {
                    DbHana = true;
                    DbOracle = false;
                }

                if (SelectedSapInstallCatalogOSVersion == null)
                {
                    AvailableSapInstallFilesForLinux = new ObservableCollection<SapInstallCatalog>();
                    AvailableSapInstallCatalogVersionLinux = new ObservableCollection<string>();
                    AvailableSapInstallCatalogOsVersion = new ObservableCollection<string>();
                    AvailableSapInstallCatalogOsPatch = new ObservableCollection<string>();
                    AvailableSapInstallCatalogSapProduct = new ObservableCollection<string>();
                    AvailableSapInstallCatalogSapStack = new ObservableCollection<string>();
                    AvailableSapInstallCatalogSapKernel = new ObservableCollection<string>();

                    SelectedSapInstallCatalogOSVersion = null;
                    SelectedSapInstallCatalogOSPatch = null;
                    SelectedSapInstallCatalogSapProduct = null;
                    SelectedSapInstallCatalogSapStack = null;
                    SelectedSapInstallCatalogSapKernel = null;

                    SelectedSAPInstallCatalogForLinux = null;

                    currentSID = SapSId;

                    if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                        AvailableSapInstallCatalogSapDBTypeAAS = new ObservableCollection<string>();
                    else
                    {
                        AvailableSapInstallCatalogSapDBType = new ObservableCollection<string>();
                        AvailableSapInstallCatalogSapDBVersion = new ObservableCollection<string>();
                        AvailableSapInstallCatalogDbPatch = new ObservableCollection<string>();

                        SelectedSapInstallCatalogSapDBType = null;
                        SelectedSapInstallCatalogSapDBVersion = null;
                    }
                }

                if (SelectedSapInstallCatalogOSVersionOracle == null)
                {
                    AvailableSapInstallFilesForLinuxOracle = new ObservableCollection<SapInstallCatalog>();
                    AvailableSapInstallCatalogVersionOracle = new ObservableCollection<string>();
                    AvailableSapInstallCatalogOsVersionOracle = new ObservableCollection<string>();
                    AvailableSapInstallCatalogOsPatchOracle = new ObservableCollection<string>();
                    AvailableSapInstallCatalogSapProductOracle = new ObservableCollection<string>();
                    AvailableSapInstallCatalogSapStackOracle = new ObservableCollection<string>();
                    AvailableSapInstallCatalogSapKernelOracle = new ObservableCollection<string>();
                    AvailableSapInstallCatalogSapDBTypeOracle = new ObservableCollection<string>();
                    AvailableSapInstallCatalogSapDBVersionOracle = new ObservableCollection<string>();
                    AvailableSapInstallCatalogDbPatchOracle = new ObservableCollection<string>();

                    SelectedSapInstallCatalogOSVersionOracle = null;
                    SelectedSapInstallCatalogOSPatchOracle = null;
                    SelectedSapInstallCatalogSapProductOracle = null;
                    SelectedSapInstallCatalogSapStackOracle = null;
                    SelectedSapInstallCatalogSapKernelOracle = null;
                    SelectedSapInstallCatalogSapDBTypeOracle = null;
                    SelectedSapInstallCatalogSapDBVersionOracle = null;

                    SelectedSAPInstallCatalogForLinuxOracle = null;
                    currentSID = SapSId;
                }

                string tempUnicode;
                string tempLinOSVer, tempLinSAPKernelPatch;
                string tempSapProduct, tempSapStack, tempSapKernel, tempSapDBType, tempSapDBTypeAAS, tempSapDBVersion, tempDbPatch;

                foreach (SapInstallCatalog pkg in SelectedProcess.SapInstallCatalogs)
                {
                    switch (pkg.DbName.Trim().ToUpper())
                    {
                        case "HANA":
                            AvailableSapInstallFilesForLinux.Add(pkg);
                            SelectedSapInstallCatalogOSVersion = "LINUX";
                            tempUnicode = AvailableSapInstallCatalogVersionLinux.Where(x => x == pkg.OpSysType).FirstOrDefault();
                            tempLinOSVer = AvailableSapInstallCatalogOsVersion.Where(x => x == pkg.OsDist).FirstOrDefault();
                            tempLinSAPKernelPatch = AvailableSapInstallCatalogOsPatch.Where(x => x == pkg.OsArch).FirstOrDefault();
                            tempSapProduct = AvailableSapInstallCatalogSapProduct.Where(x => x == pkg.SapProd).FirstOrDefault();
                            tempSapStack = AvailableSapInstallCatalogSapStack.Where(x => x == pkg.SapStack).FirstOrDefault();
                            tempSapKernel = AvailableSapInstallCatalogSapKernel.Where(x => x == pkg.SapKernel).FirstOrDefault();
                            if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                            {
                                tempSapDBTypeAAS = AvailableSapInstallCatalogSapDBTypeAAS.Where(x => x == pkg.DbName).FirstOrDefault();
                            }
                            else
                            {
                                tempSapDBType = AvailableSapInstallCatalogSapDBType.Where(x => x == pkg.DbName).FirstOrDefault();
                                tempSapDBVersion = AvailableSapInstallCatalogSapDBVersion.Where(x => x == pkg.DbVersion).FirstOrDefault();
                                tempDbPatch = AvailableSapInstallCatalogDbPatch.Where(x => x == pkg.DbPatch).FirstOrDefault();
                            }

                            if (tempUnicode == null)  //Lee si tempUnicode es nulo en caso de ser asi añade el tipo de OS (Linux en este caso)
                                AvailableSapInstallCatalogVersionLinux.Add(pkg.OpSysType);
                            if (tempLinOSVer == null) //Lee si tempLinOsVer es nulo en caso de ser asi añade la version de OS (SLES) evita que se añada multiples veces
                                AvailableSapInstallCatalogOsVersion.Add(pkg.OsDist);
                            //if (tempLinSAPKernelPatch == null) //Lee si tempLinSAPKernelPatch es nulo en caso de ser asi añade la version de Patch y evita que se añada multiples veces
                            //AvailableSapInstallCatalogOsPatch.Add(pkg.OsArch);
                            break;

                        case "ORACLE":
                            AvailableSapInstallFilesForLinuxOracle.Add(pkg);
                            SelectedSapInstallCatalogOSVersionOracle = "LINUX";
                            tempUnicode = AvailableSapInstallCatalogVersionOracle.Where(x => x == pkg.OpSysType).FirstOrDefault();
                            tempLinOSVer = AvailableSapInstallCatalogOsVersionOracle.Where(x => x == pkg.OsDist).FirstOrDefault();
                            tempLinSAPKernelPatch = AvailableSapInstallCatalogOsPatchOracle.Where(x => x == pkg.OsArch).FirstOrDefault();
                            tempSapProduct = AvailableSapInstallCatalogSapProductOracle.Where(x => x == pkg.SapProd).FirstOrDefault();
                            tempSapStack = AvailableSapInstallCatalogSapStackOracle.Where(x => x == pkg.SapStack).FirstOrDefault();
                            tempSapKernel = AvailableSapInstallCatalogSapKernelOracle.Where(x => x == pkg.SapKernel).FirstOrDefault();
                            tempSapDBType = AvailableSapInstallCatalogSapDBTypeOracle.Where(x => x == pkg.DbName).FirstOrDefault();
                            tempSapDBVersion = AvailableSapInstallCatalogSapDBVersionOracle.Where(x => x == pkg.DbVersion).FirstOrDefault();
                            tempDbPatch = AvailableSapInstallCatalogDbPatchOracle.Where(x => x == pkg.DbPatch).FirstOrDefault(); ;

                            if (tempUnicode == null)  //Lee si tempUnicode es nulo en caso de ser asi añade el tipo de OS (Linux en este caso)
                                AvailableSapInstallCatalogVersionOracle.Add(pkg.OpSysType);
                            if (tempLinOSVer == null) //Lee si tempLinOsVer es nulo en caso de ser asi añade la version de OS (SLES) evita que se añada multiples veces
                                AvailableSapInstallCatalogOsVersionOracle.Add(pkg.OsDist);
                            break;
                    }
                }
            }
            else if (_addProcessFlow[_controlIndex].UserControl is PackageView)
            {
                List<OraclePackage> tempOraclePackages = new List<OraclePackage>();
                AvailableOraclePackagesForLinux.Clear();
                AvailableOraclePackagesForAIX.Clear();
                SelectedOraclePackageForLinux = null;
                SelectedOraclePackageForAIX = null;

                AvailableSAPHostAgentPackagesForAIX.Clear();
                AvailableSAPHostAgentPackagesForLinux.Clear();
                SelectedSAPHostAgentPackageForLinux = null;
                SelectedSAPHostAgentPackageForAIX = null;

                //Adds the available packages to the package selection depending on the characteristics of the selected server
                if (SelectedProcess.OraclePackages.Count() > 0)
                {
                    AvailableOraclePackagesForLinux = new ObservableCollection<OraclePackage>();
                    AvailableOraclePackagesForAIX = new ObservableCollection<OraclePackage>();

                    AvailableOracleDBVersionsForLinux = new ObservableCollection<string>();
                    AvailableOracleDBVersionsForAIX = new ObservableCollection<string>();

                    SelectedOracleDBVersionForLinux = null;
                    SelectedOracleDBVersionForAIX = null;
                    foreach (OraclePackage pkg in SelectedProcess.OraclePackages)
                    {
                        if (SelectedServersList.Any(server => server.OS.Trim().ToUpper() == pkg.OS.Trim().ToUpper() && server.CIDI.Trim().ToUpper() != "DI"))
                        {
                            if (pkg.OS.Trim().ToUpper() == "LINUX" && !AvailableOraclePackagesForLinux.Contains(pkg))
                            {
                                tempVersion = AvailableOracleDBVersionsForLinux.Where(x => x == pkg.DBVersion).FirstOrDefault();
                                if (tempVersion == null)
                                    AvailableOracleDBVersionsForLinux.Add(pkg.DBVersion);
                                AvailableOraclePackagesForLinux.Add(pkg);
                            }
                            else if (pkg.OS.Trim().ToUpper() == "AIX" && !AvailableOraclePackagesForAIX.Contains(pkg))
                            {
                                tempVersion = AvailableOracleDBVersionsForAIX.Where(x => x == pkg.DBVersion).FirstOrDefault();
                                if (tempVersion == null)
                                    AvailableOracleDBVersionsForAIX.Add(pkg.DBVersion);
                                AvailableOraclePackagesForAIX.Add(pkg);
                            }
                        }
                    }
                }
                else if (SelectedProcess.SAPHostAgentPackages.Count() > 0)
                {
                    AvailableSAPHostAgentPackagesForLinux = new ObservableCollection<SAPHostAgentPackage>();
                    AvailableSAPHostAgentPackagesForAIX = new ObservableCollection<SAPHostAgentPackage>();

                    AvailableSAPHostAgentVersionsForLinux = new ObservableCollection<string>();
                    AvailableSAPHostAgentVersionsForAIX = new ObservableCollection<string>();

                    SelectedSAPHostAgentVersionForLinux = null;
                    SelectedSAPHostAgentVersionForAIX = null;

                    foreach (SAPHostAgentPackage pkg in SelectedProcess.SAPHostAgentPackages)
                    {
                        if (SelectedServersList.Any(server => server.OS.Trim().ToUpper() == pkg.OS.Trim().ToUpper()))
                        {
                            if (pkg.OS.Trim().ToUpper() == "LINUX")
                            {
                                tempVersion = AvailableSAPHostAgentVersionsForLinux.Where(x => x == pkg.Version).FirstOrDefault();
                                if (tempVersion == null)
                                    AvailableSAPHostAgentVersionsForLinux.Add(pkg.Version);
                                AvailableSAPHostAgentPackagesForLinux.Add(pkg);
                            }
                            else if (pkg.OS.Trim().ToUpper() == "AIX")
                            {
                                tempVersion = AvailableSAPHostAgentVersionsForAIX.Where(x => x == pkg.Version).FirstOrDefault();
                                if (tempVersion == null)
                                    AvailableSAPHostAgentVersionsForAIX.Add(pkg.Version);
                                AvailableSAPHostAgentPackagesForAIX.Add(pkg);
                            }
                        }
                    }

                }
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SelectSAPKernelPatches && SelectedProcess.SAPKernelPackages.Count() > 0)
            {
                string tempUnicode;
                SelectedSAPKernelPackageForOracle = null; SelectedSAPKernelPackageForSybase = null; SelectedSAPKernelPackageForDB2 = null; SelectedSAPKernelPackageForHana = null; SelectedSAPKernelPackageForSAPDB = null; SelectedSAPKernelPackageForWebD = null;
                AvailableSAPKernelPatchesForOracle = new ObservableCollection<SAPKernelPackage>();
                AvailableSAPKernelPatchesForSybase = new ObservableCollection<SAPKernelPackage>();
                AvailableSAPKernelPatchesForDB2 = new ObservableCollection<SAPKernelPackage>();
                AvailableSAPKernelPatchesForHana = new ObservableCollection<SAPKernelPackage>();
                AvailableSAPKernelPatchesForSAPDB = new ObservableCollection<SAPKernelPackage>();
                AvailableSAPKernelPatchesForWebD = new ObservableCollection<SAPKernelPackage>();
                SAPKernelUnicodeListForOracle = new ObservableCollection<string>();
                SAPKernelUnicodeListForSybase = new ObservableCollection<string>();
                SAPKernelUnicodeListForDB2 = new ObservableCollection<string>();
                SAPKernelUnicodeListForHana = new ObservableCollection<string>();
                SAPKernelUnicodeListForSAPDB = new ObservableCollection<string>();
                SAPKernelUnicodeListForWebD = new ObservableCollection<string>();
                SelectedSAPKernelUnicodeForOracle = null; SelectedSAPKernelVersionForOracle = null; SelectedSAPKernelPatchForOracle = null;
                SelectedSAPKernelUnicodeForSybase = null; SelectedSAPKernelVersionForSybase = null; SelectedSAPKernelPatchForSybase = null;
                SelectedSAPKernelUnicodeForDB2 = null; SelectedSAPKernelVersionForDB2 = null; SelectedSAPKernelPatchForDB2 = null;
                SelectedSAPKernelUnicodeForHana = null; SelectedSAPKernelVersionForHana = null; SelectedSAPKernelPatchForHana = null;
                SelectedSAPKernelUnicodeForSAPDB = null; SelectedSAPKernelVersionForSAPDB = null; SelectedSAPKernelPatchForSAPDB = null;
                SelectedSAPKernelUnicodeForWebD = null; SelectedSAPKernelVersionForWebD = null; SelectedSAPKernelPatchForWebD = null;
                foreach (SAPKernelPackage pkg in SelectedProcess.SAPKernelPackages)
                {
                    switch (pkg.DB.Trim().ToUpper())
                    {
                        case "ORACLE":
                            AvailableSAPKernelPatchesForOracle.Add(pkg);
                            tempUnicode = SAPKernelUnicodeListForOracle.Where(x => x == pkg.Unicode).FirstOrDefault();
                            if (tempUnicode == null)
                                SAPKernelUnicodeListForOracle.Add(pkg.Unicode);
                            break;
                        case "SYBASE":
                            AvailableSAPKernelPatchesForSybase.Add(pkg);
                            tempUnicode = SAPKernelUnicodeListForSybase.Where(x => x == pkg.Unicode).FirstOrDefault();
                            if (tempUnicode == null)
                                SAPKernelUnicodeListForSybase.Add(pkg.Unicode);
                            break;
                        case "DB2":
                            AvailableSAPKernelPatchesForDB2.Add(pkg);
                            tempUnicode = SAPKernelUnicodeListForDB2.Where(x => x == pkg.Unicode).FirstOrDefault();
                            if (tempUnicode == null)
                                SAPKernelUnicodeListForDB2.Add(pkg.Unicode);
                            break;
                        case "HANA":
                            AvailableSAPKernelPatchesForHana.Add(pkg);
                            tempUnicode = SAPKernelUnicodeListForHana.Where(x => x == pkg.Unicode).FirstOrDefault();
                            if (tempUnicode == null)
                                SAPKernelUnicodeListForHana.Add(pkg.Unicode);
                            break;
                        case "SAPDB":
                            AvailableSAPKernelPatchesForSAPDB.Add(pkg);
                            tempUnicode = SAPKernelUnicodeListForSAPDB.Where(x => x == pkg.Unicode).FirstOrDefault();
                            if (tempUnicode == null)
                                SAPKernelUnicodeListForSAPDB.Add(pkg.Unicode);
                            break;
                        case "WEBDISP":
                            AvailableSAPKernelPatchesForWebD.Add(pkg);
                            tempUnicode = SAPKernelUnicodeListForWebD.Where(x => x == pkg.Unicode).FirstOrDefault();
                            if (tempUnicode == null)
                                SAPKernelUnicodeListForWebD.Add(pkg.Unicode);
                            break;
                    }
                }
            }
            else if (CurrentControl.UserControl is SourceDB)
            {
                var filter = _filteredSystemCatalog;
                _systemCatalog.Filter = o => (filter(o) && FilterServersTarget(o));
            }
            else if (CurrentControl.UserControl is SelectTargetSystemView)
            {
                var targetDBServer = SelectedServersList.Where(s => s.CIDI.ToUpper().Trim().Contains("DO")).FirstOrDefault();
                var targetSAPServer = SelectedServersList.Where(s => s.CIDI.ToUpper().Trim().Contains("CI")).FirstOrDefault();
                if (targetDBServer != null)
                    SelectedTargetDBServer = targetDBServer;
                else
                    SelectedTargetDBServer = targetSAPServer;
                SelectedTargetSAPServer = targetSAPServer;
            }
            else if (CurrentControl.UserControl is PrimaryDBServer)
            {
                var filter = _filteredSystemCatalog;
                _systemCatalog.Filter = o => (filter(o) && FilteredServerForStandBy(o));
                _selectedStandbyDBServer = null;
            }
            else if (CurrentControl.UserControl is StandbyDBServer)
            {
                var filter = _filteredSystemCatalog;
                _systemCatalog.Filter = o => (filter(o) && FilteredServerForAscsScs(o));
                SelectedAcscScsServer = null;
            }
            else if (CurrentControl.UserControl is SapAcscScsServer)
            {
                var filter = _filteredSystemCatalog;
                _systemCatalog.Filter = o => (filter(o) && FilteredServerForErs(o));
                SelectedErsServer = null;
            }
            else if (CurrentControl.UserControl is SapErsServer)
            {
                var filter = _filteredSystemCatalog;
                _systemCatalog.Filter = o => (filter(o) && FilteredServerForAas(o));
            }
            else if(CurrentControl.UserControl is SapAasServer)
            {
                var filter = _filteredSystemCatalog;
                _systemCatalog.Filter = o => (filter(o) && FilteredServerForAas(o));
                SapAasServerList.Clear();
                foreach (ServerSystem server in SelectedServersList)
                        SapAasServerList.Add(server);

                HadrSapsaPassword = "chesPutz5";
                HadrDisasterRecoveryUser = "DR_admin";
                HadrDisasterRecoveryPassword = "chesPutz5";
                
            }
            /*else if (_addProcessFlow[_controlIndex].UserControl is SAPUserData)
            {
                HadrSapsaPassword = "chesPutz5";
                HadrDisasterRecoveryUser = "DR_admin";
                HadrDisasterRecoveryPassword = "chesPutz5";
            }*/
            else if (_addProcessFlow[_controlIndex].UserControl is SelectJavaComponents && SelectedProcess.JavaComponents.Count() > 0)
            {
                JavaComponentsCatalog = new ObservableCollection<JavaComponent>();
                foreach (JavaComponent component in SelectedProcess.JavaComponents)
                {
                    JavaComponentsCatalog.Add(new JavaComponent(component.Name));
                }
                SelectedJavaComponentsList = new ObservableCollection<JavaComponent>();
            }
            else if (_addProcessFlow[_controlIndex].UserControl is TransactionsPreactivities && SelectedProcess.TransactionsPackages.Count() > 0)
            {
                SelectedTransactionsPreActList = new ObservableCollection<Transaction>();
            }
            else if (_addProcessFlow[_controlIndex].UserControl is ExportTables)
            {
                //The export loading for catalogs begins
                DataAccess.SetupTables(SelectedTargetDBServer.Customer, SelectedProcess.ProjectName, SelectedProcess);

                ExportTablesComponentsList = new ObservableCollection<Models.Packages.Component>(SelectedProcess.ExportTablesComponents.Select(x => new Models.Packages.Component(x)));

                foreach (Models.Packages.Component component in ExportTablesComponentsList)
                {
                    component.IsSelected = true;
                    component.ExportOrImport = "export";
                }
                SelectAllExportTablesIsChecked = true;
                SelectedComponent = null;
            }
            else if (_addProcessFlow[_controlIndex].UserControl is TransactionsPostactivities && SelectedProcess.TransactionsPackages.Count() > 0)
            {
                SelectedTransactionsPostActList = new ObservableCollection<Transaction>();
            }
            else if (_addProcessFlow[_controlIndex].UserControl is ImportTables && SelectedProcess.ExportTablesComponents.Count() > 0)
            {
                int selectedExportTables = ExportTablesComponentsList.Where(x => x.IsSelected).Count();
                if (selectedExportTables == ExportTablesComponentsList.Count())
                {
                    SelectAllImportTablesIsChecked = true;
                }

                ImportTablesComponentsList = new ObservableCollection<Models.Packages.ImportComponent>(SelectedProcess.ImportTablesComponents.Select(x => new Models.Packages.ImportComponent(x)));

                foreach (ImportComponent importComponent in ImportTablesComponentsList)
                {
                    Models.Packages.ImportComponent tempComponent;

                    tempComponent = new Models.Packages.ImportComponent(importComponent.Name, importComponent.Description, importComponent.Default, importComponent.IsSelected, importComponent.IsEnabled, importComponent.ImportTables);
                }

                SelectedComponent = null;
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallStepsToExecute)
            {
                _rz10Active = true;
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallRZ10)
            {
                _rz10FqicpName = null;
                _rz10FqicpValue = null;
                _rz10AddpName = null;
                _rz10AddpValue = null;
                _fqicpList = new ObservableCollection<Models.SapInstallPostSteps.Rz10FqicpSettingsConfiguration>();
                _addpList = new ObservableCollection<Models.SapInstallPostSteps.Rz10AddpSettingsConfiguration>();
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallSMLG)
            {
                _smlgCustomerName = null;
                _smlgInstanceGroup = null;
                _smlgIpGroup = null;
                _smlgRfcEnabled = true;
                _smlgRfcType = "R - Round Robin";
                _smlgList = new ObservableCollection<Models.SapInstallPostSteps.SmlgSettingsConfiguration>();
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallSM36)
            {
                _sm36SapUser = null;
                _sm36SapPassword = null;
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallRZ12)
            {
                _rz12GroupName = null;
                _rz12InstanceGroup = null;
                _rz12Activated = true;
                _rz12MaxQueue = 5;
                checkMaxQ = true;
                _rz12MaxLogin = 90;
                checkMaxL = true;
                _rz12MaxSeparateLogons = 25;
                checkMaxSL = true;
                _rz12Maxwp = 75;
                checkMaxWp = true;
                _rz12Minfreewp = 1;
                checkMinF = true;
                _rz12Maxcomm = 90;
                checkMaxcomm = true;
                _rz12MaxWaitTime = 15;
                checkMaxWT = true;
                _rz12List = new ObservableCollection<Models.SapInstallPostSteps.Rz12SettingsConfiguration>();
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallSM61)
            {
                _sm61GroupName = null;
                _sm61Instance = null;
                _sm61List = new ObservableCollection<Models.SapInstallPostSteps.Sm61SettingsConfiguration>();
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallRZ04)
            {
                _rz04OperationName = null;
                _rz04Description = null;
                _rz04List = new ObservableCollection<Models.SapInstallPostSteps.Rz04SettingsConfiguration>();
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallRZ70)
            {
                _rz70GatewayHost = null;
                _rz70GatewayService = "sapgw";
                _rz70List = new ObservableCollection<Models.SapInstallPostSteps.Rz70SettingsConfiguration>();
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallAL11)
            {
                _al11DirectoryPath = null;
                _al11DirectoryName = null;
                _al11ValidForServer = "all";
                _al11List = new ObservableCollection<Models.SapInstallPostSteps.Al11SettingsConfiguration>();
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallSTRUST02)
            {
                _strust02CertificateType = null;
                _strust02CertificatePath = null;
                _strust02List = new ObservableCollection<Models.SapInstallPostSteps.Strust02SettingsConfiguration>();
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallSCC4)
            {
                _scc4ClientName = null;
                _scc4ClientCity = null;
                _scc4LogicalName = SelectedTargetSAPServer.SID + "CLNT";
                _scc4Currency = null;
                _scc4ClientRole = null;
                _scc4ChangesAndTransport = null;
                _scc4CrossClient = null;
                _scc4CopyComparisonTool = null;
                _scc4CattAndEcattRest = null;
                _scc4List = new ObservableCollection<Models.SapInstallPostSteps.Scc4SettingsConfiguration>();
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallSM21)
            {
                _sm21FromDate = DateTime.Today.AddMonths(-1);
                _sm21ToDate = DateTime.Today;
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallST22)
            {
                _st22FromDate = DateTime.Today.AddMonths(-1);
                _st22ToDate = DateTime.Today;
                _st22User = "*";
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapInstallDB13)
            {
                _db13Job = null;
                _db13StartDate = DateTime.Today;
                _db13StartDateHour = 0;
                _db13StartDateMinute = 0;
                _db13Recurrence = null;
                _db13RecurrenceDayHour = 0;
                _db13RecurrenceDayMinute = 0;
                _db13List = new ObservableCollection<Models.SapInstallPostSteps.Db13SettingsConfiguration>();
            }
            else if (_addProcessFlow[_controlIndex].UserControl is BDLSView)
            {
                _sourceBDLSSID = "";
                _targetBDLSSID = "";
                _BDLSList = new ObservableCollection<BDLS>();
                SelectedTransactionsPostActList = new ObservableCollection<Transaction>();
                if (SelectedProcess.ProjectName.ToUpper().Contains("BDLS") && SelectedProcess.TransactionsPackages.Count() > 0)
                {
                    AvailablePostactTransactionsPackages = new ObservableCollection<TransactionsPackage>(SelectedProcess.TransactionsPackages.Where(t => t.Subgroup.Trim().ToUpper().Contains("POST")));
                    if (AvailablePostactTransactionsPackages.Count == 0)
                        AvailablePostactTransactionsPackages = new ObservableCollection<TransactionsPackage>(SelectedProcess.TransactionsPackages.Where(t => t.Transactions.Any(t => t.Step.Trim().ToUpper().Contains("POST"))));

                    foreach (TransactionsPackage pkg in AvailablePostactTransactionsPackages)
                    {
                        if (pkg.IsSelected)
                        {
                            foreach (Transaction transaction in pkg.Transactions)
                            {
                                if (transaction.Step.Trim().ToUpper().Contains("POST"))
                                {
                                    Credentials.ClientSet tempClientSet = Credentials.ClientsList.First();
                                    if (tempClientSet != null)
                                        transaction.ClientSet = tempClientSet;
                                    SelectedTransactionsPostActList.Add(transaction);
                                }
                            }
                        }
                    }
                }
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SummaryRefresh)
            {
                SelectedSourceDBServerList = new ObservableCollection<ServerSystem>() { SelectedSourceDBServer };
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SummaryHadr)
            {
                PrimaryDBServerList = new ObservableCollection<ServerSystem>() { SelectedPrimaryDBServer };
                StandbyDBServerList = new ObservableCollection<ServerSystem>() { SelectedStandbyDBServer };
                SapAcscScsServerList = new ObservableCollection<ServerSystem>() { SelectedAcscScsServer };
                SapErsServerList = new ObservableCollection<ServerSystem>() { SelectedErsServer };
            }

                CurrentControl.MenuItem.Icon = "\\img\\icons\\done.png";
            CurrentControl = _addProcessFlow[_controlIndex];

            if (CurrentControl.UserControl is ProcessDetailsView && SelectedProcess.TransactionsPackages.Count() > 0)
            {
                AvailableTransactionsPackages.Clear();
                SelectedTransactionsPackage = null;
                AvailableTransactionsPackages = new ObservableCollection<TransactionsPackage>(SelectedProcess.TransactionsPackages);
                SelectedTransactionsList = new ObservableCollection<Transaction>();
                if (ExtraInputsLayout != null)
                {
                    ExtraInputs = new ObservableCollection<ExtraInputsSet>();
                    List<ServerSystem> serversToExecute = SelectedServersList.ToList();
                    for (int i = 0; i < serversToExecute.Count; i++)
                    {
                        foreach (ExtraInputsSet iS in ExtraInputsLayout)
                        {
                            ExtraInputsSet tempSet = new ExtraInputsSet(iS.Process, iS.Step);
                            tempSet.System = serversToExecute[i].SID;
                            tempSet.InputsSet = new List<ExtraInput>();
                            foreach (ExtraInput ei in iS.InputsSet)
                            {
                                ExtraInput tempInput = new ExtraInput(ei.Name, ei.Description, ei.Hint, ei.Type, ei.Format, ei.DefaultValue, ei.OptionsArray, ei.MinValue, ei.MaxValue, ei.ClearValues);
                                tempInput.Value = "";
                                tempSet.InputsSet.Add(tempInput);
                            }
                            ExtraInputs.Add(tempSet);
                        }
                    }
                }
                PackageView = null;
                PackageView = new Transactions();
            }
            else if (CurrentControl.UserControl is TransactionsPreactivities && SelectedProcess.TransactionsPackages.Count() > 0)
            {
                AvailablePreactTransactionsPackages.Clear();
                SelectedTransactionsPackage = null;
                AvailablePreactTransactionsPackages = new ObservableCollection<TransactionsPackage>(SelectedProcess.TransactionsPackages.Where(t => t.Subgroup.Trim().ToUpper().Contains("PRE")));
                if (AvailablePreactTransactionsPackages.Count == 0)
                    AvailablePreactTransactionsPackages = new ObservableCollection<TransactionsPackage>(SelectedProcess.TransactionsPackages.Where(t => t.Transactions.Any(t => t.Step.Trim().ToUpper().Contains("PRE"))));

                foreach (TransactionsPackage pkg in AvailablePreactTransactionsPackages)
                {
                    pkg.AvailableClientUsers = new List<string>();
                    foreach (Credentials.ClientSet set in Credentials.ClientsList)
                    {
                        if (set.IsSelected)
                            pkg.AvailableClientUsers.Add(set.UserClient);
                    }
                    if (pkg.IsSelected)
                    {
                        foreach (Transaction t in pkg.Transactions)
                        {
                            if (t.Step.Trim().ToUpper().Contains("PRE"))
                            {
                                SelectedTransactionsPreActList.Add(t);
                            }
                        }
                    }
                }
            }
            else if (CurrentControl.UserControl is TransactionsPostactivities && SelectedProcess.TransactionsPackages.Count() > 0)
            {
                AvailablePostactTransactionsPackages.Clear();
                SelectedTransactionsPackage = null;
                if (SelectedProcess.ProjectName.ToUpper().Equals("SAPQC") || SelectedProcess.ProjectName.ToUpper().Equals("SAPQCNGZTCLOUD"))
                    AvailablePostactTransactionsPackages = new ObservableCollection<TransactionsPackage>(SelectedProcess.TransactionsPackages.Where(t => t.Subgroup.Trim().ToUpper().Contains("QC")));
                else
                    AvailablePostactTransactionsPackages = new ObservableCollection<TransactionsPackage>(SelectedProcess.TransactionsPackages.Where(t => t.Subgroup.Trim().ToUpper().Contains("POST")));
                if (AvailablePostactTransactionsPackages.Count == 0)
                {
                    if (SelectedProcess.ProjectName.ToUpper().Equals("SAPQC") || SelectedProcess.ProjectName.ToUpper().Equals("SAPQCNGZTCLOUD"))
                        AvailablePostactTransactionsPackages = new ObservableCollection<TransactionsPackage>(SelectedProcess.TransactionsPackages.Where(t => t.Transactions.Any(t => t.Step.Trim().ToUpper().Contains("QC"))));
                    else
                        AvailablePostactTransactionsPackages = new ObservableCollection<TransactionsPackage>(SelectedProcess.TransactionsPackages.Where(t => t.Transactions.Any(t => t.Step.Trim().ToUpper().Contains("POST"))));
                }

                foreach (TransactionsPackage pkg in AvailablePostactTransactionsPackages)
                {
                    pkg.AvailableClientUsers = new List<string>();
                    foreach (Credentials.ClientSet set in Credentials.ClientsList)
                    {
                        if (set.IsSelected)
                            pkg.AvailableClientUsers.Add(set.UserClient);
                    }
                    if (pkg.IsSelected)
                    {
                        foreach (Transaction transaction in pkg.Transactions)
                        {
                            if (SelectedProcess.ProjectName.ToUpper().Equals("SAPQC") || SelectedProcess.ProjectName.ToUpper().Equals("SAPQCNGZTCLOUD"))
                            {
                                if (transaction.Step.Trim().ToUpper().Contains("QC"))
                                {
                                    SelectedTransactionsPostActList.Add(transaction);
                                }
                            }
                            else
                            {
                                if (transaction.Step.Trim().ToUpper().Contains("POST"))
                                    SelectedTransactionsPostActList.Add(transaction);
                            }
                        }
                    }
                }
            }
            /*else if (CurrentControl.UserControl is SAPUserData)
            {
                HadrSapsaPassword = "chesPutz5";
                HadrDisasterRecoveryUser = "DR_admin";
                HadrDisasterRecoveryPassword = "chesPutz5";
            }*/
        }
        public bool CanChangeNextView(object obj) 
        {
            bool ret = true;
            if (CurrentControl.UserControl is SelectProcessView)
            {
                if (SelectedItem != null)
                {
                    if (SelectedProcess.AnyStepReplyAuto)
                    {
                        foreach (Step tempStep in SelectedProcess.StepList.Where(x => x.RepeatAuto == true).ToList())
                        {
                            if (tempStep.RepeatDate == "" || tempStep.RepeatTime == "")
                                ret = false;
                        }
                    }
                    if (SelectedProcess.MultipleFlowMode)
                    {
                        if (SelectedFlowMode == "")
                            ret = false;
                    }

                    Certificate TempCertificate = Auxiliar.Certificates.Where(x => x.Area == SelectedProcess.Team && x.ProcessName == SelectedProcess.ProjectName).FirstOrDefault();
                    Models.Authorization TempAuthorizaton = Auxiliar.Authorizations.Where(x => x.Area == SelectedProcess.Team && x.ProcessName == SelectedProcess.ProjectName).FirstOrDefault();

                    if (SelectedProcess.CertificateRequired && (TempCertificate == null || TempAuthorizaton == null) && (UserProfile.Department.ToUpper() != "SAP MS AUTOMATION" && UserProfile.Department.ToUpper() != "SAP MS MANAGEMENT"))
                    {
                        ret = false;
                    }
                }
                else
                    ret = false;
            }
            else if (CurrentControl.UserControl is SelectServersView)
            {
                int i = 0, j = 0, k = 0, l = 0;
                string ciSidName = "", dbSidName = "";

                if (SelectedServersList.Count() == 0)
                    ret = false;
                else if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLHANACLOUD"))
                {
                    foreach (ServerSystem serverSystem in SelectedServersList)
                    {
                        if (serverSystem.CIDI.Trim().ToUpper() == "CI")
                        {
                            i++;
                            ciSidName = serverSystem.SID;
                        }
                        else if (serverSystem.ProductType.Trim().ToUpper() == "TENANT" || serverSystem.ProductType.Trim().ToUpper() == "SYSTEM")
                        {
                            j++;
                            dbSidName = serverSystem.SID;
                        }
                        else
                            k++;
                    }

                    if (j == 1)
                    {
                        if (ciSidName != dbSidName || k > 0)
                            ret = false;
                    }
                    else if (i == 0 || i > 1 || j > 1 || k > 0)
                        ret = false;
                }
                else if (SelectedProcess.ProjectName.ToUpper().Equals("SAPPOSTHANACLOUD"))
                {
                    string sid, oldSid="";

                    foreach (ServerSystem serverSystem in SelectedServersList)
                    {
                        sid = serverSystem.SID;

                        if (sid != oldSid)
                        {
                            i++;
                            oldSid = serverSystem.SID;
                        }
                        if (serverSystem.CIDI.ToUpper().Equals("CI"))
                            j++;
                    }

                    if (i > 1)
                        ret = false;
                    else if (j == 0)
                        ret = false;

                }
                else if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLORACLECLOUD") || SelectedProcess.ProjectName.ToUpper().Equals("SAPPOSTORACLOUD"))
                {
                    foreach (ServerSystem serverSystem in SelectedServersList)
                    {
                        if (serverSystem.CIDI.Trim().ToUpper() == "CI")
                        {
                            i++;
                            ciSidName = serverSystem.SID;
                        }
                        else if (serverSystem.CIDI.Trim().ToUpper() == "DO" || serverSystem.CIDI.Trim().ToUpper() == "DB")
                        {
                            j++;
                            dbSidName = serverSystem.SID;
                        }
                        else
                            k++;
                    }
                    
                    if (k != 0)
                        ret = false;
                    else if (j == 1)
                    {
                        if (ciSidName != dbSidName)
                            ret = false;
                        else if (i == 0)
                            ret = false;
                    }
                    else if (i == 1)
                    {
                        foreach(ServerSystem server in SystemCatalog)
                        {
                            if (server.SID.ToUpper().Equals(ciSidName))
                            {
                                if (server.CIDI.Trim().ToUpper() == "DO")
                                    ret = false;
                            }
                        }
                    }
                    else if (i == 0 || i > 1 || j > 1)
                        ret = false;
                    else if (i != 1 && j != 1)
                        ret = false;
                }
                else if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                {
                    foreach (ServerSystem serverSystem in SelectedServersList)
                    {
                        if (serverSystem.CIDI.Trim().ToUpper() == "CI")
                        {
                            i++;
                            ciSidName = serverSystem.SID;
                        }
                        else if (serverSystem.CIDI.Trim().ToUpper() == "DI")
                        {
                            j++;
                            dbSidName = serverSystem.SID;
                        }
                        else
                            k++;
                    }

                    if (ciSidName != dbSidName)
                        ret = false;
                    else if (i != 1 || j != 1 || k != 0)
                        ret = false;
                }
                else if (SelectedProcess.ProjectName.ToUpper().Equals("SAPPOSTORACLOUD"))
                {
                    foreach (ServerSystem serverSystem in SelectedServersList)
                    {
                        if (serverSystem.CIDI.Trim().ToUpper() == "DO")
                        {
                            i++;
                            ciSidName = serverSystem.SID;
                        }
                        else if (serverSystem.CIDI.Trim().ToUpper() == "CI")
                        {
                            j++;
                            dbSidName = serverSystem.SID;
                        }
                    }

                    if (i != 0 && j != 0)
                        ret = false;
                    else if (i > 1 || j > 1)
                        ret = false;
                }
                else if (SelectedProcess.ProjectName.ToUpper().Equals("DB2INSTALLCLOUD"))
                {
                    foreach (ServerSystem serverSystem in SelectedServersList)
                    {

                        if (dbSidName == "")
                            dbSidName = serverSystem.SID;
                        else if (dbSidName != serverSystem.SID)
                            i++;

                        if (serverSystem.CIDI.Trim().ToUpper() == "CI")
                            j++;
                        else if (serverSystem.CIDI.Trim().ToUpper() == "DO")
                            k++;
                        else if (serverSystem.CIDI.Trim().ToUpper() == "DI")
                            l++;
                    }

                    if (i != 0)
                        ret = false;
                    else if (l!=0)
                        ret = false;
                    else if (j != 0 && k != 0)
                        ret = false;
                    else if (j > 1)
                        ret = false;
                    else if (k > 1)
                        ret = false;
                }
            }
            else if (CurrentControl.UserControl is Connectivity)
            {
                if (SaltConnectivityCheckboxIsChecked && SelectedMasterServer == null)
                    ret = false;
            }
            else if (CurrentControl.UserControl is PackageView)
            {
                if (SelectedProcess.OraclePackages.Count() > 0)
                {
                    if (SelectedServersList.Any(server => server.OS.Trim().ToUpper() == "LINUX"))
                    {
                        if (SelectedOraclePackageForLinux == null)
                            ret = false;
                    }
                    else if (SelectedServersList.Any(x => x.OS.Trim().ToUpper() == "AIX"))
                    {
                        if (SelectedOraclePackageForAIX == null)
                            ret = false;
                    }
                }
            }
            else if (CurrentControl.UserControl is ProcessDetailsView)
            {
                if (SelectedProcess.TransactionsPackages.Count() > 0)
                {
                    if (SelectedTransactionsList.Count() > 0)
                        ret = true;
                    else
                        ret = false;
                }
                else if (ExtraInputsLayout != null)
                {
                    if (ret && ExtraInputs.All(x => x.InputsSet.All(i => i.Value.Length > 0)))
                        ret = true;
                    else ret = false;
                }
            }
            else if (CurrentControl.UserControl is SelectSAPKernelPatches)
            {
                if (SelectedProcess.SAPKernelPackages.Count() > 0)
                {
                    if (DisplayAvailableSAPKernelPackagesForOracle)
                    {
                        if (SelectedSAPKernelPackageForOracle == null)
                            ret = false;
                    }
                    if (DisplayAvailableSAPKernelPackagesForSybase)
                    {
                        if (SelectedSAPKernelPackageForSybase == null)
                            ret = false;
                    }
                    if (DisplayAvailableSAPKernelPackagesForDB2)
                    {
                        if (SelectedSAPKernelPackageForDB2 == null)
                            ret = false;
                    }
                    if (DisplayAvailableSAPKernelPackagesForHana)
                    {
                        if (SelectedSAPKernelPackageForHana == null)
                            ret = false;
                    }
                    if (DisplayAvailableSAPKernelPackagesForSAPDB)
                    {
                        if (SelectedSAPKernelPackageForSAPDB == null)
                            ret = false;
                    }
                    if (DisplayAvailableSAPKernelPackagesForWebD)
                    {
                        if (SelectedSAPKernelPackageForWebD == null)
                            ret = false;
                    }
                    if(SelectedSAPKernelPackageForOracle == null && SelectedSAPKernelPackageForSybase == null && SelectedSAPKernelPackageForDB2 == null && SelectedSAPKernelPackageForHana == null && SelectedSAPKernelPackageForSAPDB == null && SelectedSAPKernelPackageForWebD == null)
                        ret = false;
                }
            }
            else if (CurrentControl.UserControl is PrimaryDBServer)
            {
                if (SelectedPrimaryDBServer == null)
                    ret = false;
            }
            else if (CurrentControl.UserControl is StandbyDBServer)
            {
                if (SelectedStandbyDBServer == null)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SapAcscScsServer)
            {
                if (SelectedAcscScsServer == null)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SapErsServer)
            {
                if (SelectedErsServer == null)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SapAasServer)
            {
                if (SelectedServersList.Count() == 0)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SAPUserData)
            {
                if (_sapUserSapsaPassword == null)
                    ret = false;
                if (_hadrDisastRecoUsr == null)
                    ret = false;
                if (_sapUserDisRecUser == null)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SapInstallData)
            {
                if (_sapSId == null)
                    ret = false;
                if (_ascsInstNum == null)
                    ret = false;
                if (_pasInstNum == null)
                    ret = false;
                if (_hanaDbName == null)
                    ret = false;
                if (_hanaInstNum == null)
                    ret = false;
                if (_sapSysGid == null)
                    ret = false;
                if (_sapInsGId == null)
                    ret = false;
                if (_dbSIdAdmUId == null)
                    ret = false;
                if (_dbSIdAdmGId == null)
                    ret = false;
                if (_sidAdmUId == null)
                    ret = false;
                if (_sapAdmUId == null)
                    ret = false;
                if (_dbScehmaName == null)
                    ret = false;
                if (_masterPass == null)
                    ret = false;
                if (_virtualHost == null)
                    ret = false;
                if (_virtHostInter == null)
                    ret = false;
                if (_domainName == null)
                    ret = false;
                if (varToCheck == false)
                    ret = false;
                if (varToCheckAscs == false)
                    ret = false;
                if (varToCheckPas == false)
                    ret = false;
                if (varToCheckInN == false)
                    ret = false;
                if (varToCheckDbn == false)
                    ret = false;
                if (varToCheckSnm == false)
                    ret = false;
                if (varToCheckVir == false)
                    ret = false;
                if (varToCheckVhi == false)
                    ret = false;
                if (varToCheckDmn == false)
                    ret = false;
                if (varToCheckSysG == false)
                    ret = false;
                if (varToCheckInsG == false)
                    ret = false;
                if (varToCheckDsiU == false)
                    ret = false;
                if (varToCheckDsiG == false)
                    ret = false;
                if (varToCheckSidU == false)
                    ret = false;
                if (varToCheckAdmU == false)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SapInstallDataOracle)
            {
                if (_sapSId == null)
                    ret = false;
                if (_ascsInstNum == null)
                    ret = false;
                if (_pasInstNum == null)
                    ret = false;
                if (_sapSysGId == null)
                    ret = false;
                if (_sapInsGId == null)
                    ret = false;
                if (_sapAdmUId == null)
                    ret = false;
                if (_sidAdmUId == null)
                    ret = false;
                if (_sapHostname == null)
                    ret = false;
                if (_sapVirtualHostname == null)
                    ret = false;
                if (_databaseName == null)
                    ret = false;
                if (_oraSidGId == null)
                    ret = false;
                if (_oraSidUId == null)
                    ret = false;
                if (_oracleListenerPort == null)
                    ret = false;
                if (_databaseHn == null)
                    ret = false;
                if (_databaseVirtualHn == null)
                    ret = false;
                if (_domainName == null)
                    ret = false;
                if (_masterPass == null)
                    ret = false;
                if (!(varToCheckAscs && varToCheckPas && varToCheckSysG && varToCheckInsG && varToCheckAdmU && varToCheckSidU && varToCheckSVHN && varToCheckOSG && varToCheckOSU && varToCheckOLP && varToCheckDVH && varToCheckDmn && varToCheck))
                    ret = false;
            }
            else if (CurrentControl.UserControl is SapInstallDataHana2N)
            {
                if (_sapSId == null)
                    ret = false;
                if (_ascsInstNum == null)
                    ret = false;
                if (_pasInstNum == null)
                    ret = false;
                if (_sapSysGId == null)
                    ret = false;
                if (_sapInsGId == null)
                    ret = false;
                if (_sapAdmUId == null)
                    ret = false;
                if (_sidAdmUId == null)
                    ret = false;
                if (_sapHostname == null)
                    ret = false;
                if (_sapVirtualHostname == null)
                    ret = false;
                if (_hanaDbName == null)
                    ret = false;
                if (_hanaInstNum == null)
                    ret = false;
                if (_dbSIdAdmUId == null)
                    ret = false;
                if (_dbScehmaName == null)
                    ret = false;
                if (_databaseHn == null)
                    ret = false;
                if (_databaseVirtualHn == null)
                    ret = false;
                if (_domainName == null)
                    ret = false;
                if (_masterPass == null)
                    ret = false;
                if (varToCheck == false)
                    ret = false;
                if (varToCheckAscs == false)
                    ret = false;
                if (varToCheckPas == false)
                    ret = false;
                if (varToCheckInN == false)
                    ret = false;
                if (varToCheckDbn == false)
                    ret = false;
                if (varToCheckSnm == false)
                    ret = false;
                if (varToCheckVhi == false)
                    ret = false;
                if (varToCheckDmn == false)
                    ret = false;
                if (varToCheckSysG == false)
                    ret = false;
                if (varToCheckInsG == false)
                    ret = false;
                if (varToCheckDsiU == false)
                    ret = false;
                if (varToCheckSidU == false)
                    ret = false;
                if (varToCheckAdmU == false)
                    ret = false;
                if (varToCheckDVH == false)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SapHanaData)
            {
                if (_hanaDbName == null)
                    ret = false;
                if (_hanaInstNum == null)
                    ret = false;
                if (_masterPass == null)
                    ret = false;
                if (varToCheck == false)
                    ret = false;
                if (varToCheckDbn == false)
                    ret = false;
                if (varToCheckInN == false)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SapOracleData)
            {
                if (_taskOracleFix == false && _taskOracleCheck == false)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SapInstallDataAAS)
            {
                if (_sapSId == null)
                    ret = false;
                if (_ascsInstNum == null)
                    ret = false;
                if (_sapInsGId == null)
                    ret = false;
                if (_sapAdmUId == null)
                    ret = false;
                if (_sapPasHnm == null)
                    ret = false;
                if (_sapAasHnm == null)
                    ret = false;
                if (_sapAasVHnm == null)
                    ret = false;
                if (_domainName == null)
                    ret = false;
                if (_masterPass == null)
                    ret = false;
                if (varToCheck == false)
                    ret = false;
                if (varToCheckAscs == false)
                    ret = false;
                if (varToCheckInsG == false)
                    ret = false;
                if (varToCheckAdmU == false)
                    ret = false;
                if (varToCheckSph == false)
                    ret = false;
                if (varToCheckSah == false)
                    ret = false;
                if (varToCheckSavh == false)
                    ret = false;
                if (varToCheckDmn == false)
                    ret = false;
            }
            else if (CurrentControl.UserControl is ServerInfo)
            {
                if (ShowHANA)
                {
                    if (SelectedSapInstallOsType == null)
                        ret = false;
                    if (SelectedSapInstallOsPatch == null)
                        ret = false;
                    if (SelectedSapInstallSapProduct == null)
                        ret = false;
                    if (SelectedSapInstallSapStack == null)
                        ret = false;
                    if (SelectedSapInstallSapKernel == null)
                        ret = false;
                    if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                    {
                        if (SelectedSapInstallSapDBTypeAAS == null)
                            ret = false;
                    }
                    else
                    {
                        if (SelectedSapInstallSapDBType == null)
                            ret = false;
                        if (SelectedSapInstallSapDBVersion == null)
                            ret = false;
                        if (SelectedSapInstallDBPatch == null)
                            ret = false;
                    }
                }
                if (ShowORACLE)
                {
                    if (SelectedSapInstallOsTypeOracle == null)
                        ret = false;
                    if (SelectedSapInstallOsPatchOracle == null)
                        ret = false;
                    if (SelectedSapInstallSapProductOracle == null)
                        ret = false;
                    if (SelectedSapInstallSapStackOracle == null)
                        ret = false;
                    if (SelectedSapInstallSapKernelOracle == null)
                        ret = false;
                    if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                    {
                        if (SelectedSapInstallSapDBTypeOracleAAS == null)
                            ret = false;
                    }
                    else
                    {
                        if (SelectedSapInstallSapDBTypeOracle == null)
                            ret = false;
                        if (SelectedSapInstallSapDBVersionOracle == null)
                            ret = false;
                        if (SelectedSapInstallDBPatchOracle == null)
                            ret = false;
                    }
                }
            }
            else if (CurrentControl.UserControl is SapInstallLicenseFile)
            {
                if (LicenseFullName == null)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SapInstallRZ10)
            {
                if (ShowRZ10Window)
                {
                    if (_addpList.Count == 0 && _fqicpList.Count == 0)
                        ret = false;
                    else if (_addpList.Count != 0 && _fqicpList.Count == 0)
                        ret = true;
                    else if (_addpList.Count == 0 && _fqicpList.Count != 0)
                        ret = true;
                    else if (_addpList.Count != 0 && _fqicpList.Count != 0)
                        ret = true;
                }
                else
                    ret = true;
            }
            else if (CurrentControl.UserControl is SapInstallSMLG)
            {
                if (_smlgList.Count == 0)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SapInstallRZ12)
            {
                if (_rz12List.Count == 0)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SapInstallSM36)
            {
                if (_sm36SapUser == null || _sm36SapPassword == null)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SapInstallRZ12)
            {
                if (_sm61List.Count == 0)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SapInstallAL11)
            {
                if (_al11List.Count == 0)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SapInstallSTRUST02)
            {
                if (_strust02List.Count == 0)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SapInstallSCC4)
            {
                if (_scc4List.Count == 0)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SapInstallSM21)
            {
                if (_sm21FromDate == null || _sm21ToDate == null)
                    ret = false;
            }
            else if (CurrentControl.UserControl is Db2InstallSettings)
            {
                if (DisplayAvailableDb2InstallPackagesForAIX)
                {
                    if (SelectedDb2InstallCatalogForAIX == null)
                        ret = false;
                }

                if (DisplayAvailableDb2InstallPackagesForLinux)
                {
                    if (SelectedDb2InstallCatalogForLinux == null)
                        ret = false;
                    if(SelectedProcess.ProjectName.Equals("DB2INSTALLCLOUD") && String.IsNullOrEmpty(Pacemaker))
                        ret = false;
                }
            }
            else if (CurrentControl.UserControl is EmailDestinationsView)
            {
                if (EmailDest.Count < 1)
                    ret = false;
            }
            else if (CurrentControl.UserControl is Summary || CurrentControl.UserControl is SummaryRefresh || CurrentControl.UserControl is SummarySAPInstall || CurrentControl.UserControl is SummarySAPInstallOracle || CurrentControl.UserControl is SummarySAPInstallHana2N || CurrentControl.UserControl is SummarySAPInstallAAS || CurrentControl.UserControl is SummarySAPInstallPostHANA || CurrentControl.UserControl is SummarySAPInstallPostOracle || CurrentControl.UserControl is SummaryDB2Install || CurrentControl.UserControl is SummaryHadr)
            {
                ret = false;
            }
            else if (CurrentControl.UserControl is SourceDB)
            {
                if (SelectedSourceDBServer == null)
                    ret = false;
            }
            else if (CurrentControl.UserControl is SelectTargetSystemView)
            {
                if (SelectedServersList.Count == 0)
                    ret = false;
                else
                {
                    if(!SelectedServersList.Any(server => server.CIDI.ToUpper().Trim().Contains("CI")))
                        ret = false;
                }
            }
            else if (CurrentControl.UserControl is AdditionalSettings)
            {
                foreach(Credentials.ClientSet clientSet in Credentials.ClientsList)
                {
                    if (clientSet.IsSelected)
                    {
                        if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLPOSTACTIVITIES") || SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLPOST") || SelectedProcess.ProjectName.ToUpper().Equals("SAPQC") || SelectedProcess.ProjectName.ToUpper().Equals("SAPQCNGZTCLOUD"))
                        {
                            if (String.IsNullOrEmpty(clientSet.User) || String.IsNullOrEmpty(clientSet.Password) || String.IsNullOrEmpty(clientSet.Description))
                                ret = false;
                            break;
                        }
                        else if (String.IsNullOrEmpty(clientSet.User) || clientSet.ClientNum.Length < 3 || String.IsNullOrEmpty(clientSet.Password) || String.IsNullOrEmpty(clientSet.Description))
                            ret = false;
                    }
                }
                int selectedClientsLists = Credentials.ClientsList.Where(x => x.IsSelected).ToList().Count;
                var istinctUserClient = Credentials.ClientsList.Where(x => x.IsSelected).GroupBy(x => x.UserClient).ToList().Select(y => y.Key).ToList();

                if (selectedClientsLists != istinctUserClient.Count())
                {
                    // Duplicates exist
                    ret = false;
                }
                if (Credentials.ShowDBGroupBox && (Credentials.DBUser.Length == 0 || Credentials.DBPass.Length == 0))
                    ret = false;
                if (RestoreDBIsAutomatic && DateTime.Compare(SourceBackupDateTime,new DateTime(1900,1,1)) < 0 )
                    ret = false;
                if (CustomerSAPClient.Length != 3)
                    ret = false;
            }
            else if (CurrentControl.UserControl is TransactionsPreactivities)
            {
                if (SelectedTransactionsPreActList.Count == 0)
                    ret = false;
                foreach (TransactionsPackage.Transaction transaction in SelectedTransactionsPreActList)
                {
                    if (transaction.ClientSet==null)
                    {
                        ret = false;
                        break;
                    }
                }
            }
            else if (CurrentControl.UserControl is TransactionsPostactivities)
            {
                if (SelectedTransactionsPostActList.Count == 0)
                    ret = false;
                foreach (TransactionsPackage.Transaction transaction in SelectedTransactionsPostActList)
                {
                    if (transaction.ClientSet == null)
                    {
                        ret = false;
                        break;
                    }
                }
            }
            else if (CurrentControl.UserControl is SelectJavaComponents)
            {
                if (SelectedJavaComponentsList.Count == 0)
                    ret = false;
            }
            else if (CurrentControl.UserControl is BDLSView)
            {
                if (_BDLSList.Count == 0)
                    ret = false;
            }
            return ret;
        }
        public void ChangeToPrevView(object obj)
        {
            _controlIndex--;
            if (_controlIndex == 0)
            {
                _searchString = "";
                SelectedServersList = new ObservableCollection<ServerSystem>();
                foreach (ServerSystem server in _serverList)
                {
                    server.IsSelected = false;
                    server.IsEnabled = true;
                }
                ExtraInputs = null;
                _systemCatalog.Filter = _filteredSystemCatalog;

                SelectedOraclePackageForLinux = null;
                SelectedOraclePackageForAIX = null;

                SelectedSAPKernelPackageForOracle = null;
                SelectedSAPKernelPackageForSybase = null;
                SelectedSAPKernelPackageForDB2 = null;
                SelectedSAPKernelPackageForHana = null;
                SelectedSAPKernelPackageForSAPDB = null;
                SelectedSAPKernelPackageForWebD = null;

                AddProcessFlow = new ObservableCollection<AddProcessControl>();
                AddProcessFlow.Add(new AddProcessControl { UserControl = new SelectProcessView(), MenuItem = new Models.MenuItem { Title = "Select your process", Icon = "\\img\\icons\\processing.png" } });
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SourceDB)
            {
                var filter = _filteredSystemCatalog;
                if (!String.IsNullOrEmpty(_searchStringSourceServer))
                    _searchStringSourceServer = "";
                _systemCatalog.Filter = o => (filter(o) && FilterServersOnlyPRD(o));

                foreach (ServerSystem s in SelectedServersList.ToList())
                {
                    SelectedServersList.Remove(s);
                    s.IsSelectedOnSelectedList = false;
                    s.IsEnabled = true;
                }

            }
            
            else if (_addProcessFlow[_controlIndex].UserControl is SelectTargetSystemView)
            {
                var filter = _filteredSystemCatalog;
                if (!String.IsNullOrEmpty(_searchStringTargetSAPServer))
                    _searchStringTargetSAPServer = "";
                _systemCatalog.Filter = o => (filter(o) && FilterServersTarget(o));
            }
            else if (_addProcessFlow[_controlIndex].UserControl is StandbyDBServer)
            {
                var filter = _filteredSystemCatalog;
                if (!String.IsNullOrEmpty(_searchStringStandBy))
                    _searchStringStandBy = "";
                _systemCatalog.Filter = o => (filter(o) && FilteredServerForStandBy(o));
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapAcscScsServer)
            {
                var filter = _filteredSystemCatalog;
                if (!String.IsNullOrEmpty(_searchStringAscsScs))
                    _searchStringAscsScs = "";
                _systemCatalog.Filter = o => (filter(o) && FilteredServerForAscsScs(o));
            }
            else if (_addProcessFlow[_controlIndex].UserControl is SapErsServer)
            {
                var filter = _filteredSystemCatalog;
                if (!String.IsNullOrEmpty(_searchStringErs))
                    _searchStringErs = "";
                _systemCatalog.Filter = o => (filter(o) && FilteredServerForErs(o));
            }
            else if (CurrentControl.UserControl is SelectJavaComponents)
            {
                foreach (JavaComponent javaComponent in SelectedJavaComponentsList)
                {
                    javaComponent.IsSelected = false;
                    javaComponent.IsEnabled = true;
                }
            }
            CurrentControl.MenuItem.Icon = "\\img\\icons\\stepsarrow-b.png";
            CurrentControl = _addProcessFlow[_controlIndex];
        }
        public bool CanChangePrevView(object obj)
        {
            if (_controlIndex > 0)
                return true;
            else
                return false;
        }
        public void AddEmailDest(object obj)
        {
            TextBox textBox = (obj as TextBox);
            string emailDest = textBox.Text.ToLower();
            EmailDest.Add(emailDest);
            textBox.Text = "@syntax.com";
        }
        public bool CanAddEmailDest(object obj)
        {
            string emailDest = (obj as TextBox).Text.ToLower();
            if (emailDest.ToUpper().Contains("@SYNTAX.COM"))
            {
                if (EmailDest.IndexOf(emailDest) >= 0)
                    return false;
                else if (emailDest.Length > 14 && !emailDest.Contains(",") && !emailDest.Contains(";"))
                    return true;
                else return false;
            }
            else
                return false;
        }
        public void ScheduleProcesses(object obj)
        {
            List<Process> processList = CreateNewProcessList();

            if (processList != null)
            {
                foreach (Process p in processList)
                {
                    Auxiliar.CreateNewProcessFiles(p, SelectedProcess.ProjectName.ToUpper().Contains("CLOUD"));
                }

                MainWindow myWindow = Application.Current.MainWindow as MainWindow;
                myWindow.SetPrincipalDataContext();

                foreach (ServerSystem server in _serverList)
                {
                    server.IsSelected = false;
                    server.IsEnabled = true;
                }
            }

            if (SelectedProcess.TransactionsPackages!=null)
            {
                foreach (TransactionsPackage pkg in SelectedProcess.TransactionsPackages)
                {
                    foreach (Transaction t in pkg.Transactions)
                    {
                        t.IsEnabled = true;
                        t.SetDefaultSelected();
                    }
                }
            }

            if (SelectedProcess.JavaComponents.Count() > 0)
            {
                foreach (JavaComponent component in SelectedProcess.JavaComponents)
                {
                    component.IsEnabled = true;
                }
            }
        }
        public bool CanCreateNewProcess(object obj)
        {
            bool ret = true;
            if (Credentials == null)
            {
                ret = false;
            }
            else
            {
                bool webCredValidation = true;
                bool dbCredValidation = true;
                bool sapCredValidation = true;
                bool sidadmCredValidation = true;
                bool schemaPassValidation = true;
                if (Credentials.ShowWebCredentials)
                {
                    if (Credentials.WebUser.Length == 0 || Credentials.WebPass.Length == 0)
                        webCredValidation = false;
                }
                if (Credentials.ShowSAPCredentials)
                {
                    if (Credentials.SAPGuiUser.Length == 0 || Credentials.SAPGuiPass.Length == 0)
                        sapCredValidation = false;
                }
                if (Credentials.ShowSIDADMCredentials)
                {
                    if (Credentials.SIDAdmUser.Length == 0 || Credentials.SIDAdmPass.Length == 0)
                        sidadmCredValidation = false;
                }
                if (Credentials.ShowDBCredentials)
                {
                    if (Credentials.DBUser.Length == 0 || Credentials.DBPass.Length == 0)
                        dbCredValidation = false;
                }
                if (Credentials.ShowSchemaPassword)
                {
                    if (Credentials.DBSchemaPass.Length == 0 && !((CurrentControl.UserControl is SummarySAPInstallPostHANA) || (CurrentControl.UserControl is SummarySAPInstallPostOracle)))
                        schemaPassValidation = false;
                }
                if (webCredValidation && sapCredValidation && sidadmCredValidation && dbCredValidation && schemaPassValidation)
                    ret = true;
                else
                    ret = false;
            }
            if (ret == true && (CurrentControl.UserControl is Summary || CurrentControl.UserControl is SummaryRefresh || CurrentControl.UserControl is SummarySAPInstall || CurrentControl.UserControl is SummarySAPInstallOracle || CurrentControl.UserControl is SummarySAPInstallHana2N || CurrentControl.UserControl is SummarySAPInstallAAS || CurrentControl.UserControl is SummarySAPInstallPostHANA || CurrentControl.UserControl is SummarySAPInstallPostOracle || CurrentControl.UserControl is SummaryDB2Install || CurrentControl.UserControl is SummaryHadr))
            {
                if (!(String.IsNullOrEmpty(MasterPass)) && (CurrentControl.UserControl is SummarySAPInstallPostHANA || CurrentControl.UserControl is SummarySAPInstallPostOracle))
                    return true;
                else if (CurrentControl.UserControl is SummaryDB2Install && !(String.IsNullOrEmpty(Pacemaker)))
                    return true;
                else
                    return true;
            }
            else
                return false;
        }
        public void ShowAddToFavoritesPromt(object obj)
        {
            InputDialog inputWindow = new InputDialog();
            inputWindow.Owner = Application.Current.MainWindow;
            inputWindow.DataContext = this;
            inputWindow.ShowDialog();
        }
        public void AddToFavorites(object obj)
        {
            InputDialog inputWindow = (obj as InputDialog);

            List<Process> processList = CreateNewProcessList();
            Process layout;

            if (processList != null)
            {
                layout = Auxiliar.ProcessInitConfig.Where(x => x.ProjectName == processList.First().ProjectName).FirstOrDefault();
                foreach (Process p in processList)
                {
                    if (!layout.Credentials.ShowOSCredentials)
                    {
                        p.Credentials.OSUser = "";
                        p.Credentials.OSPass = "";
                    }
                    if (!layout.Credentials.ShowSAPCredentials)
                    {
                        p.Credentials.SAPGuiUser = "";
                        p.Credentials.SAPGuiPass = "";
                    }
                    if (!layout.Credentials.ShowSIDADMCredentials)
                    {
                        p.Credentials.SIDAdmUser = "";
                        p.Credentials.SIDAdmPass = "";
                    }
                    if (!layout.Credentials.ShowWebCredentials)
                    {
                        p.Credentials.WebUser = "";
                        p.Credentials.WebPass = "";
                    }
                    if (!layout.Credentials.ShowDBCredentials)
                    {
                        p.Credentials.DBUser = "";
                        p.Credentials.DBPass = "";
                    }
                    if (!layout.Credentials.ShowSchemaPassword)
                    {
                        p.Credentials.DBSchemaPass = "";
                    }
                }

                Favorites newFav = new Favorites(FavoritesName, SelectedProcess.Title, SelectedProcess.BrideServer, processList, SelectedProcess.ProjectName.ToUpper().Contains("CLOUD"));

                MainWindow.FVMInstance.FavoriteProfileList.Add(newFav);

                try
                {
                    string jsonString = JsonConvert.SerializeObject(newFav);
                    string userProyectPath = Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser + "\\" + SelectedProcess.ProjectName + "\\FAVORITES\\";
                    string processFileName = FavoritesName + ".JSON";
                    Auxiliar.CreateFile(userProyectPath, processFileName, jsonString);
                    inputWindow.Close();

                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow.SetFavoritesDataContext();

                    foreach (ServerSystem server in _serverList)
                    {
                        server.IsSelected = false;
                        server.IsEnabled = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error trying replace process info");
                }
            }
        }
        // Listen or unlisten to employees as they're added or removed
        private void MenuItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        var item = e.NewItems[i] as Models.MenuItem;
                        item.PropertyChanged += MenuItemPropertyChanged;

                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        var item = e.NewItems[i] as Models.MenuItem;
                        item.PropertyChanged -= MenuItemPropertyChanged;
                    }
                    break;
            }
        }
        // Only listen for the employee being selected
        private void MenuItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Models.MenuItem.IsSelected))
            {
                SelectedItem = sender as Models.MenuItem;
            }
        }
        private bool FilterServersBobJ(object item)
        {
            ServerSystem server = item as ServerSystem;
            if (server != null)
                return server.ProductType.Trim().ToUpper() == "BOBJ" || server.ProductType.Trim().ToUpper() == "BOBJA";
            else
                return true;
        }
        /* private bool FilterServerHadr(object item)
         {
             ServerSystem server = item as ServerSystem;
             //foreach (ServerSystem server in serversToFilter)
             //{
             if (SelectedStandbyDBServer == null)
                 {
                     return _selectedProcess.DBSType.Trim().ToUpper().Equals(_serverList.DBType.Trim().ToUpper()) && _selectedProcess.Customer.Trim().ToUpper().Equals(server.Customer.Trim().ToUpper()) && _selectedProcess.SID.Trim().ToUpper().Equals(server.SID.Trim().ToUpper());
                 }
                 else
                     return _selectedProcess.DBSType.Trim().ToUpper().Equals(server.DBType.Trim().ToUpper());
             //}
         }*/
        private bool FilterServersByProcessDB(object item)
        {
            ServerSystem server = item as ServerSystem;

            if (server != null)
            {
                if (_selectedProcess.ProjectName.Trim().ToUpper() == "SAPINSTALLHANACLOUD" || _selectedProcess.ProjectName.Trim().ToUpper() == "SAPPOSTHANACLOUD")
                {
                    if (server.DBType.Trim().ToUpper() == "HANA")
                    {
                        _selectedProcess.DBSType = "HANA";
                        return _selectedProcess.DBSType.Trim().ToUpper().Equals(server.DBType.Trim().ToUpper());

                    }
                    else if (server.DBType.Trim().ToUpper() == "DB")
                    {
                        _selectedProcess.DBSType = "DB";
                        return _selectedProcess.DBSType.Trim().ToUpper().Equals(server.DBType.Trim().ToUpper());
                    }
                    else
                        return false;
                }
                /*if (_selectedProcess.ProjectName.Trim().ToUpper() == "SAPPOSTHANACLOUD")
                {
                    if (server.ProductType.Trim().ToUpper() == "TENANT" || server.ProductType.Trim().ToUpper() == "SYSTEM")
                    {
                        _selectedProcess.DBSType = server.DBType.Trim().ToUpper();
                        return _selectedProcess.DBSType.Trim().ToUpper().Equals(server.DBType.Trim().ToUpper());
                    }
                    else
                        return false;
                }*/
                else
                    return _selectedProcess.DBSType.Trim().ToUpper().Equals(server.DBType.Trim().ToUpper());
            }
            else
                return true;
        }
        private bool FilterServersByProcessOS(object item)
        {
            ServerSystem server = item as ServerSystem;
            if (server != null)
                return _selectedProcess.OSType.Trim().ToUpper().Contains(server.OS.Trim().ToUpper());
            else
                return true;
        }
        private bool ServersSearchFirstWord(object item)
        {
            ServerSystem server = item as ServerSystem;
            string cs = _searchCriteriaArray[0];
            return  server.SID.Trim().ToUpper().Contains(cs) ||
                    server.Customer.Trim().ToUpper().Contains(cs) ||
                    server.Hostname.Trim().ToUpper().Contains(cs) ||
                    server.ProductType.Trim().ToUpper().Contains(cs) ||
                    server.Stack.Trim().ToUpper().Contains(cs) ||
                    server.Environment.Trim().ToUpper().Contains(cs) ||
                    server.CIDI.Trim().ToUpper().Contains(cs) ||
                    server.Version.Trim().ToUpper().Contains(cs) ||
                    server.OS.Trim().ToUpper().Contains(cs) ||
                    server.DBType.Trim().ToUpper().Contains(cs)
                ;
        }
        private bool ServersSearchSecondWord(object item)
        {
            ServerSystem server = item as ServerSystem;
            string cs = _searchCriteriaArray[1];
            return  server.SID.Trim().ToUpper().Contains(cs) ||
                    server.Customer.Trim().ToUpper().Contains(cs) ||
                    server.Hostname.Trim().ToUpper().Contains(cs) ||
                    server.ProductType.Trim().ToUpper().Contains(cs) ||
                    server.Stack.Trim().ToUpper().Contains(cs) ||
                    server.Environment.Trim().ToUpper().Contains(cs) ||
                    server.CIDI.Trim().ToUpper().Contains(cs) ||
                    server.Version.Trim().ToUpper().Contains(cs) ||
                    server.OS.Trim().ToUpper().Contains(cs) ||
                    server.DBType.Trim().ToUpper().Contains(cs)
                ;
        }
        private bool ServersSearchThirdWord(object item)
        {
            ServerSystem server = item as ServerSystem;
            string cs = _searchCriteriaArray[2];
            return  server.SID.Trim().ToUpper().Contains(cs) ||
                    server.Customer.Trim().ToUpper().Contains(cs) ||
                    server.Hostname.Trim().ToUpper().Contains(cs) ||
                    server.ProductType.Trim().ToUpper().Contains(cs) ||
                    server.Stack.Trim().ToUpper().Contains(cs) ||
                    server.Environment.Trim().ToUpper().Contains(cs) ||
                    server.CIDI.Trim().ToUpper().Contains(cs) ||
                    server.Version.Trim().ToUpper().Contains(cs) ||
                    server.OS.Trim().ToUpper().Contains(cs) ||
                    server.DBType.Trim().ToUpper().Contains(cs)
                ;
        }
        private bool ServersSearchFourthWord(object item)
        {
            ServerSystem server = item as ServerSystem;
            string cs = _searchCriteriaArray[3];
            return  server.SID.Trim().ToUpper().Contains(cs) ||
                    server.Customer.Trim().ToUpper().Contains(cs) ||
                    server.Hostname.Trim().ToUpper().Contains(cs) ||
                    server.ProductType.Trim().ToUpper().Contains(cs) ||
                    server.Stack.Trim().ToUpper().Contains(cs) ||
                    server.Environment.Trim().ToUpper().Contains(cs) ||
                    server.CIDI.Trim().ToUpper().Contains(cs) ||
                    server.Version.Trim().ToUpper().Contains(cs) ||
                    server.OS.Trim().ToUpper().Contains(cs) ||
                    server.DBType.Trim().ToUpper().Contains(cs)
                ;
        }
        private bool ServersSearchFifthWord(object item)
        {
            ServerSystem server = item as ServerSystem;
            string cs = _searchCriteriaArray[4];
            return  server.SID.Trim().ToUpper().Contains(cs) ||
                    server.Customer.Trim().ToUpper().Contains(cs) ||
                    server.Hostname.Trim().ToUpper().Contains(cs) ||
                    server.ProductType.Trim().ToUpper().Contains(cs) ||
                    server.Stack.Trim().ToUpper().Contains(cs) ||
                    server.Environment.Trim().ToUpper().Contains(cs) ||
                    server.CIDI.Trim().ToUpper().Contains(cs) ||
                    server.Version.Trim().ToUpper().Contains(cs) ||
                    server.OS.Trim().ToUpper().Contains(cs) ||
                    server.DBType.Trim().ToUpper().Contains(cs)
                ;
        }
        private bool FilterServersByServerOS(object item)
        {
            ServerSystem server = item as ServerSystem;
            if (server != null)
                return SelectedServer.OS.Contains(server.OS.Trim().ToUpper());
            else
                return true;
        }
        private bool FilterServersByServerSID(object item)
        {
            ServerSystem server = item as ServerSystem;
            if (server != null && SelectedServer != null)
                return SelectedServer.SID == server.SID;
            else
                return true;
        }
        private bool FilterServersBySelectedSID(object item)
        {
            ServerSystem server = item as ServerSystem;

            ServerSystem selectedServer;
            if (server != null && SelectedServersList.Count > 0)
            {
                selectedServer = SelectedServersList.First();
                return selectedServer.SID == server.SID;
            }
            else
                return true;
        }
        private List<Process> CreateNewProcessList()
        {
            string pTitle, ituser, creationTime, idx, customer, sid, pas, dbs, instance, instanceType, pasType, pasOS, dbType, dbOS;
            bool issecuential, applReq;
            Process newProcess, layout;
            
            pTitle = SelectedItem.Title;
            ituser = UserProfile.ItUser;
            layout = Auxiliar.ProcessInitConfig.Where(x => x.Title == pTitle).FirstOrDefault();
            applReq = layout.ApplReq;
            issecuential = layout.IsSecuential;
            List<Process> newProcesses = new List<Process>();
            List<Step> stepList = new List<Step>(SelectedSteps.ToList());
            List<Appl> applList = new List<Appl>();
            List<ServerSystem> serversToExecute = SelectedServersList.ToList();

            foreach (Step step in stepList)
            {
                step.ProcessAuto = step.AutoDefault;
                step.Email = step.EmailDefault;
            }

            if (layout.BrideServer)
            {
                customer = "GLOBAL";
                sid = "GLB";
                pas = "syn99a08lp215";
                pasType = "bridge";
                instanceType = "CI";
                instance = "00";

                creationTime = Auxiliar.GenerateTimeStamp(Auxiliar.ConvertToEST());
                idx = creationTime + "_" + sid + customer + "_" + layout.ProjectName + "_" + ituser;

                string[] emailDest = EmailDest.ToArray<string>();
                dbType = layout.DBSType;

                dbs = pas;
                pasOS = layout.OSType;
                dbOS = layout.OSType;
                newProcess = new Process(layout, idx, creationTime, -1, sid, customer, pas, dbs, instance, "", 0, stepList, applList, instanceType, pasType, pasOS, dbType, dbOS, emailDest, Credentials, "", "", "", "NA", DateTime.Now, 0, SapInstall);
                newProcess.ServerList = SelectedServersList.ToList();

                if (layout.Subtype.Contains("JAVA") && this.SelectedJavaComponentsList != null)
                {
                    newProcess.JavaComponents = new List<JavaComponent>(this.SelectedJavaComponentsList);
                }

                newProcesses.Add(newProcess);
            }
            else
            {
                if (SelectedProcess.ProjectName.ToUpper() == "SAPINSTALLPOSTACTIVITIES" || SelectedProcess.ProjectName.ToUpper() == "SAPINSTALLPOST" ||
                    SelectedProcess.ProjectName.ToUpper() == "SAPQC" || SelectedProcess.ProjectName.ToUpper() == "SAPQCNGZTCLOUD")
                {
                    for (int i = 0; i < serversToExecute.Count; i++)
                    {
                        ServerSystem tempServer = serversToExecute[i];
                        customer = tempServer.Customer.Replace(" ", "").Trim().ToUpper();
                        sid = tempServer.SID.Trim();

                        creationTime = Auxiliar.GenerateTimeStamp(Auxiliar.ConvertToEST());
                        idx = creationTime + "_" + sid + customer + "_" + layout.ProjectName + "_" + ituser;
                        newProcess = new Process(layout, idx, creationTime, sid, customer);

                        newProcess.Db13List = new List<Models.SapInstallPostSteps.Db13SettingsConfiguration>(_db13List);
                        newProcess.FqicpList = new List<Models.SapInstallPostSteps.Rz10FqicpSettingsConfiguration>(_fqicpList);
                        newProcess.AddpList = new List<Models.SapInstallPostSteps.Rz10AddpSettingsConfiguration>(_addpList);

                        newProcess.SmlgList = new List<Models.SapInstallPostSteps.SmlgSettingsConfiguration>(_smlgList);
                        newProcess.Rz12List = new List<Models.SapInstallPostSteps.Rz12SettingsConfiguration>(_rz12List);
                        newProcess.Sm61List = new List<Models.SapInstallPostSteps.Sm61SettingsConfiguration>(_sm61List);
                        newProcess.Rz04List = new List<Models.SapInstallPostSteps.Rz04SettingsConfiguration>(_rz04List);
                        newProcess.Rz70List = new List<Models.SapInstallPostSteps.Rz70SettingsConfiguration>(_rz70List);
                        newProcess.Al11List = new List<Models.SapInstallPostSteps.Al11SettingsConfiguration>(_al11List);
                        newProcess.Strust02List = new List<Models.SapInstallPostSteps.Strust02SettingsConfiguration>(_strust02List);
                        newProcess.Scc4List = new List<Models.SapInstallPostSteps.Scc4SettingsConfiguration>(_scc4List);
                        newProcess.Sm36SapUser = this._sm36SapUser;
                        newProcess.Sm36SapPassword = this._sm36SapPassword;
                        newProcess.Sm21From = this.Sm21FromDateComplete;
                        newProcess.Sm21To = this.Sm21ToDateComplete;
                        newProcess.St22From = this.St22FromDateComplete;
                        newProcess.St22To = this.St22ToDateComplete;
                        newProcess.St22User = this.St22User;

                        if (!(SelectedProcess.ProjectName.ToUpper() == "SAPPOSTHANACLOUD" || SelectedProcess.ProjectName.ToUpper() == "SAPPOSTORACLOUD"))
                            newProcess.InstanceNum = SelectedTargetSAPServer.Instance;

                        if (this._taskOracleFixText != null)
                            newProcess.TaskOracle = this._taskOracleFixText;
                        else if (this._taskOracleCheckText != null)
                            newProcess.TaskOracle = this._taskOracleCheckText;

                        newProcesses.Add(newProcess);
                        newProcess.ServerList = new List<ServerSystem>();

                        newProcess.ServerList.Add(tempServer);

                        switch (tempServer.CIDI.Trim())
                        {
                            case "CI":
                                newProcess.PAS = tempServer.Hostname.Trim();
                                newProcess.PASType = tempServer.Stack.Trim();
                                newProcess.ProductType = tempServer.ProductType.Trim();
                                newProcess.PASOS = tempServer.OS.Trim();
                                newProcess.OSType = tempServer.OS.Trim();
                                newProcess.InstanceNum = tempServer.Instance;
                                newProcess.TargetSystemDB = tempServer.DBType.Trim();
                                break;
                            case "DI":
                                newProcess.ApplList.Add(
                                    new Appl(
                                        tempServer.Hostname.Trim(),
                                        tempServer.Stack.Trim(),
                                        tempServer.OS.Trim()
                                        ));
                                break;
                            case "DO":
                                newProcess.DBS = tempServer.Hostname.Trim();
                                newProcess.DBSType = SelectedProcess.DBSType;
                                newProcess.DBSOS = tempServer.OS.Trim();
                                newProcess.TargetSystemDB = tempServer.DBType.Trim();
                                break;
                        }

                        foreach (Process p in newProcesses)
                        {
                            if (SelectedProcess.ProjectName.ToUpper() != "SAPPOSTORACLOUD" && SelectedProcess.ProjectName.ToUpper() != "SAPPOSTHANACLOUD")
                            {
                                if (String.IsNullOrEmpty(p.PAS))
                                {
                                    MessageBox.Show("No CI selected for " + p.SID, layout.ProjectName, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return null;
                                }
                            }
                            //else
                            //{
                            if (String.IsNullOrEmpty(p.DBS))
                            {
                                p.DBS = p.PAS;
                                p.DBSType = SelectedProcess.DBSType;
                                p.DBSOS = p.PASOS;
                            }

                            p.IntanceType = (p.ApplList.Count > 0) ? "ALL" : "CI";

                            p.StepList = stepList;

                            if (SelectedProcess.TransactionsPackages == null)
                            {
                                p.StepList = stepList;
                            }
                            else
                            {
                                p.TransactionsPackages = new List<TransactionsPackage>();

                                foreach (TransactionsPackage package in SelectedProcess.TransactionsPackages)
                                {
                                    p.TransactionsPackages.Add(package);
                                }

                                p.StepList = stepList;
                                foreach (Step step in p.StepList)
                                {
                                    step.TransactionsList = new List<TransactionsPackage.Transaction>();
                                    foreach (TransactionsPackage package in SelectedProcess.TransactionsPackages)
                                    {
                                        if (package.IsSelected)
                                        {
                                            foreach (Transaction t in package.Transactions)
                                            {
                                                string subgrp = package.Subgroup.Replace(" ", "").Trim().ToUpper();
                                                if (subgrp.Contains("SET"))
                                                {
                                                    if (t.Step != null)
                                                    {
                                                        string stepT = t.Step.Replace(" ", "").Trim().ToUpper();
                                                        if (step.Name.Contains(stepT))
                                                        {
                                                            step.TransactionsList.Add(t);
                                                        }
                                                    }
                                                }
                                                else if (step.Name.Contains(subgrp))
                                                {
                                                    if (step.Transactions == "")
                                                        step.Transactions = t.TCode;
                                                    else
                                                        step.Transactions = step.Transactions + "," + t.TCode;
                                                }
                                            }
                                        }
                                    }

                                }
                            }

                            p.EmailDest = EmailDest.ToArray<string>();

                            p.Credentials = Credentials;

                            if (p.ProjectName.ToUpper().Equals("SAPINSTALLCLOUD"))
                                p.SapInstall = new SapInstall(this.SapSId, this.AscsInstNum, this.PasInstNum, this.HanaDbName, this.HanaInstNum,
                                    this.SapSysGId, this.SapInsGId, this.DbSIdAdmGId, this.DbSIdAdmUId, this.SidAdmUId, this.SapAdmUId,
                                    this.DbScehmaName, this.MasterPass, this.VirtualHost, this.VirtHostInter, this.SetDomain, this.DomainName);
                            else if (p.ProjectName.ToUpper().Equals("SAPINSTALLORACLECLOUD"))
                                p.SapInstall = new SapInstall(this.SapSId, this.AscsInstNum, this.PasInstNum, this.SapSysGId, this.SapInsGId,
                                    this.SapAdmUId, this.SidAdmUId, this.SapHostname, this.SapVirtualHostname, this.DatabaseName, this.OraSidGId,
                                    this.OraSidUId, this.OracleListenerPort, this.DatabaseHn, this.DatabaseVirtualHn, this.SetDomain,
                                    this.DomainName, this.MasterPass);
                            else if (p.ProjectName.ToUpper().Equals("SAPINSTALLHANACLOUD"))
                                p.SapInstall = new SapInstall(this.SapSId, this.AscsInstNum, this.PasInstNum, this.HanaDbName, this.HanaInstNum,
                                    this.SapSysGId, this.SapInsGId, this.DbSIdAdmGId, this.DbSIdAdmUId, this.SidAdmUId, this.SapAdmUId,
                                    this.DbScehmaName, this.MasterPass, this.DatabaseVirtualHn, this.VirtHostInter, this.SetDomain, this.DomainName,
                                    this.DatabaseHn, this.SapVirtualHostname, this.SapHostname);
                            else if (p.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                                p.SapInstall = new SapInstall(this.SapSId, this.AscsInstNum, this.SapInsGId, this.SapAdmUId, this.SapPasHnm,
                                    this.SapAasHnm, this.SapAasVHnm, this.MasterPass, this.SetDomain, this.DomainName);
                            else if ((p.ProjectName.ToUpper().Equals("SAPPOSTHANACLOUD")))
                                p.SapInstall = new SapInstall(this.HanaInstNum, this.HanaDbName, this.MasterPass);

                            if (p.PASOS == "Linux" && this.SelectedOraclePackageForLinux != null)
                            {
                                p.OraclePackages.Add(this.SelectedOraclePackageForLinux);
                            }
                            else if (p.PASOS == "AIX" && this.SelectedOraclePackageForAIX != null)
                            {
                                p.OraclePackages.Add(this.SelectedOraclePackageForAIX);
                            }

                            if (this.SelectedSAPInstallCatalogForLinux != null)
                            {
                                p.SapInstallCatalogs.Add(this.SelectedSAPInstallCatalogForLinux);
                            }

                            if (this.SelectedSAPInstallCatalogForLinuxOracle != null)
                            {
                                p.SapInstallCatalogs.Add(this.SelectedSAPInstallCatalogForLinuxOracle);
                            }

                            if (layout.ProjectName.Contains("SAPKERNELUPGRADE"))
                            {
                                if (p.ProductType == "WEBD")
                                {
                                    if (SelectedSAPKernelPackageForWebD != null)
                                        p.SAPKernelPackages.Add(SelectedSAPKernelPackageForWebD);
                                }
                                else
                                {
                                    switch (p.TargetSystemDB.Trim().ToUpper())
                                    {
                                        case "ORACLE":
                                            if (SelectedSAPKernelPackageForOracle != null)
                                                p.SAPKernelPackages.Add(SelectedSAPKernelPackageForOracle);
                                            break;
                                        case "SYBASE":
                                            if (SelectedSAPKernelPackageForSybase != null)
                                                p.SAPKernelPackages.Add(SelectedSAPKernelPackageForSybase);
                                            break;
                                        case "DB2":
                                            if (SelectedSAPKernelPackageForDB2 != null)
                                                p.SAPKernelPackages.Add(SelectedSAPKernelPackageForDB2);
                                            break;
                                        case "HANA":
                                            if (SelectedSAPKernelPackageForHana != null)
                                                p.SAPKernelPackages.Add(SelectedSAPKernelPackageForHana);
                                            break;
                                        case "SAPDB":
                                            if (SelectedSAPKernelPackageForSAPDB != null)
                                                p.SAPKernelPackages.Add(SelectedSAPKernelPackageForSAPDB);
                                            break;
                                        default:
                                            MessageBox.Show("There are not available patches for this DB type: " + p.TargetSystemDB, layout.ProjectName,
                                                MessageBoxButton.OK, MessageBoxImage.Error);
                                            return null;
                                            break;
                                    }
                                }
                            }
                            //}
                        }
                    }
                }
                else if (SelectedProcess.ProjectName.ToUpper() == "SAPPOSTHANACLOUD" || SelectedProcess.ProjectName.ToUpper() == "SAPPOSTORACLOUD")
                {
                    //for (int i = 0; i < serversToExecute.Count; i++)
                    //{
                    //ServerSystem tempServer = serversToExecute[i];
                    ServerSystem tempServer = serversToExecute[0];
                    //customer = tempServer.Customer.Replace(" ", "").Trim().ToUpper();
                    customer = serversToExecute[0].Customer.Replace(" ", "").Trim().ToUpper();
                    //sid = tempServer.SID.Trim();
                    sid = serversToExecute[0].SID.Trim();

                    creationTime = Auxiliar.GenerateTimeStamp(Auxiliar.ConvertToEST());
                    idx = creationTime + "_" + sid + customer + "_" + layout.ProjectName + "_" + ituser;
                    newProcess = new Process(layout, idx, creationTime, sid, customer);

                    /*newProcess.Db13List = new List<Models.SapInstallPostSteps.Db13SettingsConfiguration>(_db13List);
                    newProcess.FqicpList = new List<Models.SapInstallPostSteps.Rz10FqicpSettingsConfiguration>(_fqicpList);
                    newProcess.AddpList = new List<Models.SapInstallPostSteps.Rz10AddpSettingsConfiguration>(_addpList);

                    newProcess.SmlgList = new List<Models.SapInstallPostSteps.SmlgSettingsConfiguration>(_smlgList);
                    newProcess.Rz12List = new List<Models.SapInstallPostSteps.Rz12SettingsConfiguration>(_rz12List);
                    newProcess.Sm61List = new List<Models.SapInstallPostSteps.Sm61SettingsConfiguration>(_sm61List);
                    newProcess.Rz04List = new List<Models.SapInstallPostSteps.Rz04SettingsConfiguration>(_rz04List);
                    newProcess.Rz70List = new List<Models.SapInstallPostSteps.Rz70SettingsConfiguration>(_rz70List);
                    newProcess.Al11List = new List<Models.SapInstallPostSteps.Al11SettingsConfiguration>(_al11List);
                    newProcess.Strust02List = new List<Models.SapInstallPostSteps.Strust02SettingsConfiguration>(_strust02List);
                    newProcess.Scc4List = new List<Models.SapInstallPostSteps.Scc4SettingsConfiguration>(_scc4List);
                    newProcess.Sm36SapUser = this._sm36SapUser;
                    newProcess.Sm36SapPassword = this._sm36SapPassword;
                    newProcess.Sm21From = this.Sm21FromDateComplete;
                    newProcess.Sm21To = this.Sm21ToDateComplete;
                    newProcess.St22From = this.St22FromDateComplete;
                    newProcess.St22To = this.St22ToDateComplete;
                    newProcess.St22User = this.St22User;*/

                    //if (!(SelectedProcess.ProjectName.ToUpper() == "SAPPOSTHANACLOUD" || SelectedProcess.ProjectName.ToUpper() == "SAPPOSTORACLOUD"))
                    //if (!(SelectedProcess.ProjectName.ToUpper() == "SAPPOSTHANACLOUD" || SelectedProcess.ProjectName.ToUpper() == "SAPPOSTORACLOUD"))
                    if (SelectedProcess.ProjectName.ToUpper() == "SAPPOSTHANACLOUD")
                        tempServer = serversToExecute.Where(x => x.CIDI == "DB").FirstOrDefault();
                    else if (SelectedProcess.ProjectName.ToUpper() == "SAPPOSTORACLOUD")
                    {
                        if (serversToExecute.Count > 1)
                            tempServer = serversToExecute.Where(x => x.CIDI == "DO").FirstOrDefault();
                        else if (serversToExecute.Count == 1)
                            tempServer = serversToExecute.Where(x => x.CIDI == "CI").FirstOrDefault();
                    }

                    newProcess.InstanceNum = tempServer.Instance;

                    if (this._taskOracleFixText != null)
                        newProcess.TaskOracle = this._taskOracleFixText;
                    else if (this._taskOracleCheckText != null)
                        newProcess.TaskOracle = this._taskOracleCheckText;

                    newProcesses.Add(newProcess);
                    newProcess.ServerList = new List<ServerSystem>();

                    for (int i = 0; i < serversToExecute.Count; i++)
                    {
                        newProcess.ServerList.Add(serversToExecute[i]);

                        switch (serversToExecute[i].CIDI.Trim())
                        {
                            case "CI":
                                newProcess.PAS = serversToExecute[i].Hostname.Trim();
                                newProcess.PASType = serversToExecute[i].Stack.Trim();
                                newProcess.ProductType = serversToExecute[i].ProductType.Trim();
                                newProcess.PASOS = serversToExecute[i].OS.Trim();
                                newProcess.OSType = serversToExecute[i].OS.Trim();
                                newProcess.InstanceNum = serversToExecute[i].Instance;
                                newProcess.TargetSystemDB = serversToExecute[i].DBType.Trim();
                                break;
                            case "DI":
                                newProcess.ApplList.Add(
                                    new Appl(
                                        serversToExecute[i].Hostname.Trim(),
                                        serversToExecute[i].Stack.Trim(),
                                        serversToExecute[i].OS.Trim()
                                        ));
                                break;
                            case "DO":
                                newProcess.DBS = serversToExecute[i].Hostname.Trim();
                                newProcess.DBSType = SelectedProcess.DBSType;
                                newProcess.DBSOS = serversToExecute[i].OS.Trim();
                                newProcess.TargetSystemDB = serversToExecute[i].DBType.Trim();
                                break;
                        }
                    }

                    //foreach (Process p in newProcesses)
                    //{
                    /*if (SelectedProcess.ProjectName.ToUpper() != "SAPPOSTORACLOUD" && SelectedProcess.ProjectName.ToUpper() != "SAPPOSTHANACLOUD")
                    {
                        if (String.IsNullOrEmpty(p.PAS))
                        {
                            MessageBox.Show("No CI selected for " + p.SID, layout.ProjectName, MessageBoxButton.OK, MessageBoxImage.Error);
                            return null;
                        }
                    }*/
                    //else
                    //{
                    //if (String.IsNullOrEmpty(p.DBS))
                    if (String.IsNullOrEmpty(newProcess.DBS))
                    {
                        newProcess.DBS = newProcess.PAS;
                        newProcess.DBSType = SelectedProcess.DBSType;
                        newProcess.DBSOS = newProcess.PASOS;
                    }

                    newProcess.IntanceType = (newProcess.ApplList.Count > 0) ? "ALL" : "CI";

                    newProcess.StepList = stepList;

                    /*if (SelectedProcess.TransactionsPackages == null)
                    {
                        newProcess.StepList = stepList;
                    }
                    else
                    {
                        newProcess.TransactionsPackages = new List<TransactionsPackage>();

                        foreach (TransactionsPackage package in SelectedProcess.TransactionsPackages)
                        {
                            p.TransactionsPackages.Add(package);
                        }

                        p.StepList = stepList;
                        foreach (Step step in p.StepList)
                        {
                            step.TransactionsList = new List<TransactionsPackage.Transaction>();
                            foreach (TransactionsPackage package in SelectedProcess.TransactionsPackages)
                            {
                                if (package.IsSelected)
                                {
                                    foreach (Transaction t in package.Transactions)
                                    {
                                        string subgrp = package.Subgroup.Replace(" ", "").Trim().ToUpper();
                                        if (subgrp.Contains("SET"))
                                        {
                                            if (t.Step != null)
                                            {
                                                string stepT = t.Step.Replace(" ", "").Trim().ToUpper();
                                                if (step.Name.Contains(stepT))
                                                {
                                                    step.TransactionsList.Add(t);
                                                }
                                            }
                                        }
                                        else if (step.Name.Contains(subgrp))
                                        {
                                            if (step.Transactions == "")
                                                step.Transactions = t.TCode;
                                            else
                                                step.Transactions = step.Transactions + "," + t.TCode;
                                        }
                                    }
                                }
                            }

                        }
                    }*/

                    newProcess.EmailDest = EmailDest.ToArray<string>();

                    newProcess.Credentials = Credentials;

                    /*if (p.ProjectName.ToUpper().Equals("SAPINSTALLCLOUD"))
                        p.SapInstall = new SapInstall(this.SapSId, this.AscsInstNum, this.PasInstNum, this.HanaDbName, this.HanaInstNum,
                            this.SapSysGId, this.SapInsGId, this.DbSIdAdmGId, this.DbSIdAdmUId, this.SidAdmUId, this.SapAdmUId,
                            this.DbScehmaName, this.MasterPass, this.VirtualHost, this.VirtHostInter, this.SetDomain, this.DomainName);
                    else if (p.ProjectName.ToUpper().Equals("SAPINSTALLORACLECLOUD"))
                        p.SapInstall = new SapInstall(this.SapSId, this.AscsInstNum, this.PasInstNum, this.SapSysGId, this.SapInsGId,
                            this.SapAdmUId, this.SidAdmUId, this.SapHostname, this.SapVirtualHostname, this.DatabaseName, this.OraSidGId,
                            this.OraSidUId, this.OracleListenerPort, this.DatabaseHn, this.DatabaseVirtualHn, this.SetDomain,
                            this.DomainName, this.MasterPass);
                    else if (p.ProjectName.ToUpper().Equals("SAPINSTALLHANACLOUD"))
                        p.SapInstall = new SapInstall(this.SapSId, this.AscsInstNum, this.PasInstNum, this.HanaDbName, this.HanaInstNum,
                            this.SapSysGId, this.SapInsGId, this.DbSIdAdmGId, this.DbSIdAdmUId, this.SidAdmUId, this.SapAdmUId,
                            this.DbScehmaName, this.MasterPass, this.DatabaseVirtualHn, this.VirtHostInter, this.SetDomain, this.DomainName,
                            this.DatabaseHn, this.SapVirtualHostname, this.SapHostname);
                    else if (p.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                        p.SapInstall = new SapInstall(this.SapSId, this.AscsInstNum, this.SapInsGId, this.SapAdmUId, this.SapPasHnm,
                            this.SapAasHnm, this.SapAasVHnm, this.MasterPass, this.SetDomain, this.DomainName);*/
                    //else if ((p.ProjectName.ToUpper().Equals("SAPPOSTHANACLOUD")))
                    if ((newProcess.ProjectName.ToUpper().Equals("SAPPOSTHANACLOUD")))
                        newProcess.SapInstall = new SapInstall(this.HanaInstNum, this.HanaDbName, this.MasterPass);

                    if (newProcess.PASOS == "Linux" && this.SelectedOraclePackageForLinux != null)
                    {
                        newProcess.OraclePackages.Add(this.SelectedOraclePackageForLinux);
                    }
                    else if (newProcess.PASOS == "AIX" && this.SelectedOraclePackageForAIX != null)
                    {
                        newProcess.OraclePackages.Add(this.SelectedOraclePackageForAIX);
                    }

                    if (this.SelectedSAPInstallCatalogForLinux != null)
                    {
                        newProcess.SapInstallCatalogs.Add(this.SelectedSAPInstallCatalogForLinux);
                    }

                    if (this.SelectedSAPInstallCatalogForLinuxOracle != null)
                    {
                        newProcess.SapInstallCatalogs.Add(this.SelectedSAPInstallCatalogForLinuxOracle);
                    }

                    /*if (layout.ProjectName.Contains("SAPKERNELUPGRADE"))
                    {
                        if (newProcess.ProductType == "WEBD")
                        {
                            if (SelectedSAPKernelPackageForWebD != null)
                                p.SAPKernelPackages.Add(SelectedSAPKernelPackageForWebD);
                        }
                        else
                        {
                            switch (p.TargetSystemDB.Trim().ToUpper())
                            {
                                case "ORACLE":
                                    if (SelectedSAPKernelPackageForOracle != null)
                                        p.SAPKernelPackages.Add(SelectedSAPKernelPackageForOracle);
                                    break;
                                case "SYBASE":
                                    if (SelectedSAPKernelPackageForSybase != null)
                                        p.SAPKernelPackages.Add(SelectedSAPKernelPackageForSybase);
                                    break;
                                case "DB2":
                                    if (SelectedSAPKernelPackageForDB2 != null)
                                        p.SAPKernelPackages.Add(SelectedSAPKernelPackageForDB2);
                                    break;
                                case "HANA":
                                    if (SelectedSAPKernelPackageForHana != null)
                                        p.SAPKernelPackages.Add(SelectedSAPKernelPackageForHana);
                                    break;
                                case "SAPDB":
                                    if (SelectedSAPKernelPackageForSAPDB != null)
                                        p.SAPKernelPackages.Add(SelectedSAPKernelPackageForSAPDB);
                                    break;
                                default:
                                    MessageBox.Show("There are not available patches for this DB type: " + p.TargetSystemDB, layout.ProjectName,
                                        MessageBoxButton.OK, MessageBoxImage.Error);
                                    return null;
                                    break;
                            }
                        }
                    }*/
                    //}
                    //}
                    //}
                }
                else if (SelectedProcess.SystemCopyModules != "NA" && SelectedProcess.SystemCopyModules != null && 
                    !(SelectedProcess.ProjectName.ToUpper().Contains("DB2")))    
                {
                    sid = SelectedTargetSAPServer.SID.Trim();
                    customer = SelectedTargetSAPServer.Customer.Replace(" ", "").Trim().ToUpper();
                    creationTime = Auxiliar.GenerateTimeStamp(Auxiliar.ConvertToEST());
                    idx = creationTime + "_" + sid + customer + "_" + layout.ProjectName + "_" + ituser;
                    newProcess = new Process(layout, idx, creationTime, sid, customer);
                    newProcess.SourceDBS = SelectedSourceDBServer.Hostname.Trim();
                    newProcess.SourceSID = SelectedSourceDBServer.SID.Trim();
                    newProcess.SourceInstanceNum = SelectedSourceDBServer.Instance;
                    newProcess.SourceType = SelectedSourceDBServer.ProductType.Trim();
                    newProcess.SourceDBSOS = SelectedSourceDBServer.OS.Trim();
                    newProcess.SourceServers = new List<ServerSystem>() { };
                    newProcess.SourceServers.Add(SelectedSourceDBServer);

                    newProcess.PAS = SelectedTargetSAPServer.Hostname.Trim();
                    newProcess.PASType = SelectedTargetSAPServer.Stack.Trim();
                    newProcess.PASOS = SelectedTargetSAPServer.OS.Trim();
                    newProcess.OSType = SelectedTargetSAPServer.OS.Trim();
                    newProcess.InstanceNum = SelectedTargetSAPServer.Instance;
                    newProcess.DBS = SelectedTargetDBServer.Hostname.Trim();
                    newProcess.DBSType = SelectedProcess.DBSType;
                    newProcess.DBSOS = SelectedTargetDBServer.OS.Trim();
                    newProcess.CustomerSAPClient = CustomerSAPClient.Trim();

                    newProcess.ServerList = new List<ServerSystem>() { };
                    newProcess.ServerList.Add(SelectedTargetSAPServer);

                    for (int i = 0; i < serversToExecute.Count; i++)
                    {
                        ServerSystem tempServer = serversToExecute[i];
                        if (serversToExecute[i].CIDI.Trim().ToUpper().Contains("DI"))
                        {
                            newProcess.ApplList.Add(new Appl(tempServer.Hostname.Trim(), tempServer.Stack.Trim(), tempServer.OS.Trim()));
                            newProcess.ServerList.Add(tempServer);
                        }
                    }

                    newProcess.IntanceType = (newProcess.ApplList.Count > 0) ? "ALL" : "CI";

                    if (SelectedProcess.TransactionsPackages == null)
                    {
                        newProcess.StepList = stepList;
                    }
                    else
                    {
                        newProcess.TransactionsPackages = new List<TransactionsPackage>();

                        foreach (TransactionsPackage package in SelectedProcess.TransactionsPackages)
                        {
                            newProcess.TransactionsPackages.Add(package);
                        }   

                        newProcess.StepList = stepList;
                        foreach (Step step in newProcess.StepList)
                        {
                            step.TransactionsList = new List<TransactionsPackage.Transaction>();
                            foreach (TransactionsPackage package in SelectedProcess.TransactionsPackages)
                            {
                                if (package.IsSelected)
                                {
                                    foreach (Transaction t in package.Transactions)
                                    {
                                        string subgrp = package.Subgroup.Replace(" ", "").Trim().ToUpper();
                                        if (subgrp.Contains("SET"))
                                        {
                                            if (t.Step != null)
                                            {
                                                string stepT = t.Step.Replace(" ", "").Trim().ToUpper();
                                                if (step.Name.Contains(stepT))
                                                {
                                                    step.TransactionsList.Add(t);
                                                }
                                            }
                                        }
                                        else if (step.Name.Contains(subgrp))
                                        {
                                            if (step.Transactions == "")
                                                step.Transactions = t.TCode;
                                            else
                                                step.Transactions = step.Transactions + "," + t.TCode;
                                        }
                                    }
                                }
                            }

                        }
                    }
                    
                    newProcess.SelectedFlowMode = SelectedFlowMode;
                    
                    if (RestoreDBIsAutomatic)
                    {
                        newProcess.RestoreDateTime = SourceBackupDateTime;
                        newProcess.CVStreams = SourceBackupCV;
                    }

                    newProcess.ExportTablesComponents = new List<Models.Packages.Component>();
                    
                    foreach(Models.Packages.Component component in ExportTablesComponentsList)
                    {
                        if (component.IsSelected == true)
                            newProcess.ExportTablesComponents.Add(component);
                    }
                    
                    newProcess.ImportTablesComponents = new List<Models.Packages.ImportComponent>();
                    
                    foreach(Models.Packages.ImportComponent component in ImportTablesComponentsList)
                    {
                        if (component.IsSelected == true)
                            newProcess.ImportTablesComponents.Add(component);
                    }
                    
                    newProcess.BDLSList = new List<BDLS>(BDLSList);

                    newProcess.EmailDest = EmailDest.ToArray<string>();

                    newProcess.Credentials = Credentials;

                    newProcesses.Add(newProcess);
                }
                else
                {
                    if (_selectedProcess.ApplReq == true)
                    {
                        for (int i = 0; i < serversToExecute.Count; i++)
                        {
                            ServerSystem tempServer = serversToExecute[i];
                            customer = tempServer.Customer.Replace(" ", "").Trim().ToUpper();
                            sid = tempServer.SID.Trim();

                            if (newProcesses.Count > 0)
                            {
                                newProcess = newProcesses.Where(x => x.SID == sid && x.Customer == customer).FirstOrDefault();
                                if (newProcess == null)
                                {
                                    creationTime = Auxiliar.GenerateTimeStamp(Auxiliar.ConvertToEST());
                                    if (sid.Length > 3)
                                    {
                                        idx = creationTime + "_" + sid + '@' + customer + "_" + layout.ProjectName + "_" + ituser;
                                    }
                                    else
                                    {
                                        idx = creationTime + "_" + sid + customer + "_" + layout.ProjectName + "_" + ituser;
                                    }
                                    newProcess = new Process(layout, idx, creationTime, sid, customer);
                                    newProcesses.Add(newProcess);
                                    newProcess.ServerList = new List<ServerSystem>();
                                }
                            }
                            else
                            {
                                creationTime = Auxiliar.GenerateTimeStamp(Auxiliar.ConvertToEST());
                                idx = creationTime + "_" + sid + customer + "_" + layout.ProjectName + "_" + ituser;
                                newProcess = new Process(layout, idx, creationTime, sid, customer);
                                newProcesses.Add(newProcess);
                                newProcess.ServerList = new List<ServerSystem>();
                            }
                            newProcess.ServerList.Add(tempServer);

                            switch (tempServer.CIDI.Trim())
                            {
                                case "CI":
                                    newProcess.PAS = tempServer.Hostname.Trim();
                                    newProcess.PASType = tempServer.Stack.Trim();
                                    newProcess.ProductType = tempServer.ProductType.Trim();
                                    newProcess.PASOS = tempServer.OS.Trim();
                                    newProcess.OSType = tempServer.OS.Trim();
                                    newProcess.InstanceNum = tempServer.Instance;
                                    newProcess.TargetSystemDB = tempServer.DBType.Trim();
                                    break;
                                case "DI":
                                    newProcess.ApplList.Add(
                                        new Appl(
                                            tempServer.Hostname.Trim(),
                                            tempServer.Stack.Trim(),
                                            tempServer.OS.Trim()
                                            ));
                                    break;
                                case "DO":
                                    newProcess.DBS = tempServer.Hostname.Trim();
                                    newProcess.DBSType = SelectedProcess.DBSType;
                                    newProcess.DBSOS = tempServer.OS.Trim();
                                    newProcess.TargetSystemDB = tempServer.DBType.Trim();
                                    break;
                            }
                        }

                        foreach (Process p in newProcesses)
                        {
                            if (String.IsNullOrEmpty(p.PAS) && !(p.ProjectName.Equals("STARTSAPCRMSYBASEHACLOUD") || p.ProjectName.Equals("STOPSAPCRMSYBASEHACLOUD")))
                            {
                                MessageBox.Show("No CI selected for " + p.SID, layout.ProjectName, MessageBoxButton.OK, MessageBoxImage.Error);
                                return null;
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(p.DBS))
                                {
                                    p.DBS = p.PAS;
                                    p.DBSType = SelectedProcess.DBSType;
                                    p.DBSOS = p.PASOS;
                                }

                                p.IntanceType = (p.ApplList.Count > 0) ? "ALL" : "CI";

                                p.StepList = stepList;

                                p.EmailDest = EmailDest.ToArray<string>();

                                p.Credentials = Credentials;

                                if(p.ProjectName.ToUpper().Equals("STARTSAPCRMSYBASEHACLOUD") || p.ProjectName.ToUpper().Equals("STOPSAPCRMSYBASEHACLOUD"))
                                {
                                    p.PAS = SelectedPrimaryDBServer.Hostname.Trim();
                                    p.PASType = SelectedPrimaryDBServer.Stack.Trim();
                                    p.ProductType = SelectedPrimaryDBServer.ProductType.Trim();
                                    p.PASOS = SelectedPrimaryDBServer.OS.Trim();
                                    p.OSType = SelectedPrimaryDBServer.OS.Trim();
                                    p.InstanceNum = SelectedPrimaryDBServer.Instance;
                                    p.TargetSystemDB = SelectedPrimaryDBServer.DBType.Trim();
                                    //p.Hadr = new Hadr(SelectedPrimaryDBServer,SelectedStandbyDBServer,SelectedAcscScsServer, SelectedErsServer, SapAasServerList, SapUserSapsaPassword,SapUserDisasterRecoveryUser,SapUserDisasterRecoveryPassword);
                                    p.HadrPrimaryDbServer = SelectedPrimaryDBServer;
                                    p.HadrStandbyDbServer = SelectedStandbyDBServer;
                                    p.HadrSapAcscScsServer = SelectedAcscScsServer;
                                    p.HadrSapErsServer = SelectedErsServer;
                                    p.HadrSapAasServer = SapAasServerList.ToList();
                                    p.HadrSapsaPassword = _sapUserSapsaPassword;
                                    //p.HadrSapsaPassword = SapUserSapsaPassword;
                                    p.HadrDisRecUser = SapUserDisasterRecoveryUser;
                                    p.HadrDisRecPass = SapUserDisasterRecoveryPassword;
                                }

                                if (p.ProjectName.ToUpper().Equals("SAPINSTALLCLOUD"))
                                    p.SapInstall = new SapInstall(this.SapSId, this.AscsInstNum, this.PasInstNum, this.HanaDbName, this.HanaInstNum, this.SapSysGId, 
                                        this.SapInsGId, this.DbSIdAdmGId, this.DbSIdAdmUId, this.SidAdmUId, this.SapAdmUId, this.DbScehmaName, this.MasterPass, 
                                        this.VirtualHost, this.VirtHostInter, this.SetDomain, this.DomainName);
                                else if (p.ProjectName.ToUpper().Equals("SAPINSTALLORACLECLOUD"))
                                    p.SapInstall = new SapInstall(this.SapSId, this.AscsInstNum, this.PasInstNum, this.SapSysGId, this.SapInsGId, this.SapAdmUId, 
                                        this.SidAdmUId, this.SapHostname, this.SapVirtualHostname, this.DatabaseName, this.OraSidGId, this.OraSidUId, 
                                        this.OracleListenerPort, this.DatabaseHn, this.DatabaseVirtualHn, this.SetDomain, this.DomainName, this.MasterPass);
                                else if (p.ProjectName.ToUpper().Equals("SAPINSTALLHANACLOUD"))
                                    p.SapInstall = new SapInstall(this.SapSId, this.AscsInstNum, this.PasInstNum, this.HanaDbName, this.HanaInstNum, this.SapSysGId, 
                                        this.SapInsGId, this.DbSIdAdmGId, this.DbSIdAdmUId, this.SidAdmUId, this.SapAdmUId, this.DbScehmaName, this.MasterPass, 
                                        this.DatabaseVirtualHn, this.VirtHostInter, this.SetDomain, this.DomainName, this.DatabaseHn, this.SapVirtualHostname, 
                                        this.SapHostname);
                                else if (p.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                                    p.SapInstall = new SapInstall(this.SapSId, this.AscsInstNum, this.SapInsGId, this.SapAdmUId, this.SapPasHnm, this.SapAasHnm, 
                                        this.SapAasVHnm, this.MasterPass, this.SetDomain, this.DomainName);
                                else if (p.ProjectName.ToUpper().Equals("SAPPOSTHANACLOUD"))
                                    p.SapInstall = new SapInstall(this.HanaInstNum, this.HanaDbName, this.MasterPass);

                                if (p.PASOS == "Linux" && this.SelectedOraclePackageForLinux != null)
                                {
                                    p.OraclePackages.Add(this.SelectedOraclePackageForLinux);
                                }
                                else if (p.PASOS == "AIX" && this.SelectedOraclePackageForAIX != null)
                                {
                                    p.OraclePackages.Add(this.SelectedOraclePackageForAIX);
                                }

                                if (this.SelectedDb2InstallCatalogForLinux != null)
                                    p.Db2InstallCatalogs.Add(this.SelectedDb2InstallCatalogForLinux);

                                if (this.SelectedDb2InstallCatalogForAIX != null)
                                    p.Db2InstallCatalogs.Add(this.SelectedDb2InstallCatalogForAIX);

                                if (this.SelectedSAPInstallCatalogForLinux != null)
                                    p.SapInstallCatalogs.Add(this.SelectedSAPInstallCatalogForLinux);

                                if (this.SelectedSAPInstallCatalogForLinuxOracle != null)
                                    p.SapInstallCatalogs.Add(this.SelectedSAPInstallCatalogForLinuxOracle);

                                if (layout.ProjectName.Contains("SAPKERNELUPGRADE"))
                                {
                                    if(p.ProductType == "WEBD")
                                    {
                                        if (SelectedSAPKernelPackageForWebD != null)
                                            p.SAPKernelPackages.Add(SelectedSAPKernelPackageForWebD);
                                    }
                                    else
                                    {
                                        switch (p.TargetSystemDB.Trim().ToUpper())
                                        {
                                            case "ORACLE":
                                                if (SelectedSAPKernelPackageForOracle != null)
                                                    p.SAPKernelPackages.Add(SelectedSAPKernelPackageForOracle);
                                                break;
                                            case "SYBASE":
                                                if (SelectedSAPKernelPackageForSybase != null)
                                                    p.SAPKernelPackages.Add(SelectedSAPKernelPackageForSybase);
                                                break;
                                            case "DB2":
                                                if (SelectedSAPKernelPackageForDB2 != null)
                                                    p.SAPKernelPackages.Add(SelectedSAPKernelPackageForDB2);
                                                break;
                                            case "HANA":
                                                if (SelectedSAPKernelPackageForHana != null)
                                                    p.SAPKernelPackages.Add(SelectedSAPKernelPackageForHana);
                                                break;
                                            case "SAPDB":
                                                if (SelectedSAPKernelPackageForSAPDB != null)
                                                    p.SAPKernelPackages.Add(SelectedSAPKernelPackageForSAPDB);
                                                break;
                                            default:
                                                MessageBox.Show("There are not available patches for this DB type: " + p.TargetSystemDB, layout.ProjectName, 
                                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                                return null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < serversToExecute.Count; i++)
                        {
                            if (ExtraInputs != null)
                            {
                                foreach (ExtraInputsSet eis in ExtraInputs)
                                {
                                    if (serversToExecute[i].SID == eis.System)
                                    {
                                        stepList.Where(x => x.Name == eis.Step).FirstOrDefault().ExtraInputs = eis;
                                    }
                                }
                            }

                            ServerSystem tempServer = serversToExecute[i];
                            pas = tempServer.Hostname.Trim();
                            pasType = tempServer.Stack.Trim();
                            pasOS = tempServer.OS.Trim();

                            dbs = tempServer.Hostname.Trim();
                            dbType = SelectedProcess.DBSType;
                            dbOS = tempServer.OS.Trim();

                            customer = tempServer.Customer.Replace(" ", "").Trim().ToUpper();
                            sid = tempServer.SID.Trim();
                            instance = tempServer.Instance;
                            instanceType = "CI";

                            string[] emailDest = EmailDest.ToArray<string>();

                            creationTime = Auxiliar.GenerateTimeStamp(Auxiliar.ConvertToEST());
                            idx = creationTime + "_" + sid + pas.Substring(pas.Length - 3).Trim().ToUpper() + '@' + customer + "_" + layout.ProjectName + "_" + ituser;
                            newProcess = new Process(layout, idx, creationTime, -1, sid, customer, pas, dbs, instance, "", 0, stepList, applList, instanceType, pasType, 
                                pasOS, dbType, dbOS, emailDest, Credentials, "", "", SelectedFlowMode, "NA", DateTime.Now, 0, SapInstall);

                            newProcess.ServerList = new List<ServerSystem>();
                            newProcess.ServerList.Add(tempServer);

                            newProcess.TargetSystemDB = tempServer.DBType.Trim();
                            
                            if(!(String.IsNullOrEmpty(Pacemaker)))
                                newProcess.Db2Pacemaker = Pacemaker;

                            if (newProcess.PASOS == "Linux")
                            {
                                if (this.SelectedOraclePackageForLinux != null)
                                    newProcess.OraclePackages.Add(this.SelectedOraclePackageForLinux);
                                if (this.SelectedSAPHostAgentPackageForLinux != null)
                                    newProcess.SAPHostAgentPackages.Add(this.SelectedSAPHostAgentPackageForLinux);
                                if (this.SelectedDb2InstallCatalogForLinux != null)
                                    newProcess.Db2InstallCatalogs.Add(this.SelectedDb2InstallCatalogForLinux);
                            }
                            else if (newProcess.PASOS == "AIX")
                            {
                                if (this.SelectedOraclePackageForAIX != null)
                                    newProcess.OraclePackages.Add(this.SelectedOraclePackageForAIX);
                                if (this.SelectedSAPHostAgentPackageForAIX != null)
                                    newProcess.SAPHostAgentPackages.Add(this.SelectedSAPHostAgentPackageForAIX);
                                if (this.SelectedDb2InstallCatalogForAIX != null)
                                    newProcess.Db2InstallCatalogs.Add(this.SelectedDb2InstallCatalogForAIX);
                            }

                            if (layout.ProjectName.Contains("SAPKERNELUPGRADE"))
                            {
                                if (newProcess.PASType == "WEBD")
                                {
                                    if (SelectedSAPKernelPackageForWebD != null)
                                        newProcess.SAPKernelPackages.Add(SelectedSAPKernelPackageForWebD);
                                }
                                else
                                {
                                    switch (newProcess.TargetSystemDB.Trim().ToUpper())
                                    {
                                        case "ORACLE":
                                            if (SelectedSAPKernelPackageForOracle != null)
                                                newProcess.SAPKernelPackages.Add(SelectedSAPKernelPackageForOracle);
                                            break;
                                        case "SYBASE":
                                            if (SelectedSAPKernelPackageForSybase != null)
                                                newProcess.SAPKernelPackages.Add(SelectedSAPKernelPackageForSybase);
                                            break;
                                        case "DB2":
                                            if (SelectedSAPKernelPackageForDB2 != null)
                                                newProcess.SAPKernelPackages.Add(SelectedSAPKernelPackageForDB2);
                                            break;
                                        case "HANA":
                                            if (SelectedSAPKernelPackageForHana != null)
                                                newProcess.SAPKernelPackages.Add(SelectedSAPKernelPackageForHana);
                                            break;
                                        case "SAPDB":
                                            if (SelectedSAPKernelPackageForSAPDB != null)
                                                newProcess.SAPKernelPackages.Add(SelectedSAPKernelPackageForSAPDB);
                                            break;
                                        default:
                                            MessageBox.Show("There are not available patches for this DB type: " + newProcess.TargetSystemDB, layout.ProjectName, 
                                                MessageBoxButton.OK, MessageBoxImage.Error);
                                            return null;
                                    }
                                }
                            }

                            newProcesses.Add(newProcess);

                        }
                    }
                }
            }
            
            if (SelectedProcess.ProjectName.ToUpper().Contains("CLOUD"))
            {
                foreach (Process process in newProcesses)
                {
                    process.MasterServer = SelectedMasterServer;
                }
            }

            foreach (Process process in newProcesses)
            {
                if (process.ProjectName.ToUpper().Equals("SAPSYSTEMCOPYORADBCOPYCLOUD"))
                    Auxiliar.CreateServerListFile(process);
                if ((process.ProjectName.ToUpper() == "SAPINSTALLPOSTACTIVITIES") || (process.ProjectName.ToUpper() == "SAPINSTALLPOST"))
                    Auxiliar.UploadSapPAFile(process);
                if (process.ProjectName.ToUpper() == "SAPQC" || process.ProjectName.ToUpper() == "SAPQCNGZTCLOUD")
                    Auxiliar.UploadQualityCheckFile(process);
                if (process.ProjectName.ToUpper().Contains("NGZT"))
                    Auxiliar.GenerateNGZTFolders(process);
                
            }

            return newProcesses;
        }    
    }
}
