using RunTeamConsole.Models;
using RunTeamConsole.Models.AddProcesses;
using RunTeamConsole.Models.Packages;
using RunTeamConsole.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using static RunTeamConsole.Models.TransactionsPackage;

namespace RunTeamConsole.ViewModels
{
    public partial class AddProcessViewModel : ObservableObject
    {
        private bool _showBackupWarningMessage;
        private string _selectedFlowMode, _searchStringSourceServer, _searchStringTargetSAPServer, _sourceBDLSSID, _targetBDLSSID, _sourceClient, _targetClient;
        private string _customerSAPClient;
        private int _sourceBackupHourTime;
        private int _sourceBackupMinuteTime;
        private int _SourceBackupCV;
        private DateTime _sourceBackupDateTime;
        private DateTime _sourceBackupDate;
        private ServerSystem _selectedDBSourceServer;
        private ServerSystem _selectedSAPTargetServer;
        private ServerSystem _selecteDBTargetServer;
        private ObservableCollection<ServerSystem> _selectedDBSourceServerList;
        private ObservableCollection<TransactionsPackage> _availablePreactTransactionsPackages;
        private ObservableCollection<TransactionsPackage> _availablePostactTransactionsPackages;
        private ObservableCollection<Transaction> _selectedTransactionsPreActList;
        private ObservableCollection<Transaction> _selectedTransactionsPostActList;
        private ObservableCollection<BDLS> _BDLSList;

        private bool _selectAllExportTablesIsChecked, _selectAllImportTablesIsChecked;
        private Component _selectedComponent;
        private ObservableCollection<Component> _exportTablesComponentsList;
        private ObservableCollection<ImportComponent> _importTablesComponentsList;
        public RelayCommand SelectAllExportTablesCommand { get; private set; }
        public RelayCommand SelectAllImportTablesCommand { get; private set; }

        public RelayCommand MoveToSelectedPreActTransactions { get; private set; }
        public RelayCommand RemoveFromSelectedPreActTransactions { get; private set; }
        public RelayCommand MoveToSelectedPostActTransactions { get; private set; }
        public RelayCommand RemoveFromSelectedPostActTransactions { get; private set; }
        public RelayCommand SelectAllTransactionsOnSelectedPreActListCommand { get; private set; }
        public RelayCommand AddtoBDLSListCommand { get; private set; }
        public RelayCommand RemoveFromBDLSListCommand { get; private set; }


        public RelayCommand SelectPreactTransactionsSet { get; private set; }

        private ObservableCollection<string> _flowModes;

        public ObservableCollection<string> FlowModes
        {
            get { return this._flowModes; }
            set
            {
                this._flowModes = value;
                this.OnPropertyChanged("FlowModes");
            }
        }

        public string SelectedFlowMode
        {
            get
            {
                if (SelectedProcess != null)
                    return _selectedFlowMode;
                else
                    return "";
            }
            set
            {
                this._selectedFlowMode = value;
                this.OnPropertyChanged("SelectedFlowMode");
            }
        }

        public bool IsMultipleFlowMode
        {
            get
            {
                if (SelectedProcess != null)
                {
                    if (SelectedProcess.MultipleFlowMode)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        public bool RestoreDBIsAutomatic
        {
            get
            {
                if ((SelectedProcess.SystemCopyModules == "ALL" && SelectedFlowMode.ToUpper().Contains("AUTO")) || SelectedProcess.SystemCopyModules.ToUpper().Contains("DBRESTORE"))
                    return true;
                else
                    return false;
            }
        }
        
        public bool ShowBDLSList
        {
            get
            {
                if (SelectedProcess.SystemCopyModules == "ALL" || SelectedProcess.SystemCopyModules.ToUpper().Contains("ACTIVITIES") || SelectedProcess.ProjectName.ToUpper().Contains("BDLS"))
                    return true;
                else
                    return false;
            }
        }

        public bool ShowAddSettingsInfo
        {
            get
            {
                if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLPOSTACTIVITIES") || SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLPOST"))
                    return false;
                else
                    return true;
            }
        }
        public string CustomerSAPClient
        {
            get
            {
                return this._customerSAPClient;
            }
            set
            {
                this._customerSAPClient = value;
                this.OnPropertyChanged("CustomerSAPClient");
            }
        }

        public bool ShowBackupWarningMessage
        {
            get { return this._showBackupWarningMessage; }
            set 
            {
                this._showBackupWarningMessage = value; 
                this.OnPropertyChanged("ShowBackupWarningMessage"); 
            }
        }

        public string BackupWarningMessage
        {
            get
            {
                return "***********************W A R N I N G*************************\n ***By Backups policies the retention time is just 15 days***\n**************************************************************\nDear " + UserProfile.ItUser + " please keep in mind that the backup date could\nnot be available unless you have been required explicitly to\nbackups teams, otherwise it wont be available.\nBefore continue we strongly recommend confirm the backup\navailability.\nPlease type again the backup date to confirm. Thank you.";
            }
        }

        public ServerSystem SelectedSourceDBServer
        {
            get { return this._selectedDBSourceServer; }
            set
            {
                this._selectedDBSourceServer = value;
                this.OnPropertyChanged("SelectedSourceDBServer");
            }
        }
        
        public ObservableCollection<ServerSystem> SelectedSourceDBServerList
        {
            get { return this._selectedDBSourceServerList; }
            set
            {
                this._selectedDBSourceServerList = value;
                this.OnPropertyChanged("SelectedSourceDBServerList");
            }
        }
        
        public ServerSystem SelectedTargetSAPServer
        {
            get { return this._selectedSAPTargetServer; }
            set
            {
                this._selectedSAPTargetServer = value;
                this.OnPropertyChanged("SelectedTargetSAPServer");
            }
        }

        public ServerSystem SelectedTargetDBServer
        {
            get { return this._selecteDBTargetServer; }
            set
            {
                this._selecteDBTargetServer = value;
                this.OnPropertyChanged("SelectedTargetDBServer");
            }
        }

        public DateTime SourceBackupDateTime
        {
            get { return this._sourceBackupDateTime; }
            set
            {
                if (value.AddDays(15) < DateTime.Now && ShowBackupWarningMessage == false)
                {
                    ShowBackupWarningMessage = true;
                    SourceBackupDate = DateTime.Today;
                }
                else
                {
                    this._sourceBackupDateTime = value;
                    this.OnPropertyChanged("SourceBackupDateTime");
                }
            }
        }

        public DateTime SourceBackupDate
        {
            get { return this._sourceBackupDate; }
            set
            {
                this._sourceBackupDate = value;
                this.OnPropertyChanged("SourceBackupDate");
                updateSourceBackupDateTime();
            }
        }
        public int SourceBackupHourTime
        {
            get { return this._sourceBackupHourTime; }
            set
            {
                this._sourceBackupHourTime = value;
                this.OnPropertyChanged("SourceBackupHourTime");
                updateSourceBackupDateTime();
            }
        }
        public int SourceBackupMinuteTime
        {
            get { return this._sourceBackupMinuteTime; }
            set
            {
                this._sourceBackupMinuteTime = value;
                this.OnPropertyChanged("SourceBackupMinuteTime");
                updateSourceBackupDateTime();
            }
        }
        
        public int SourceBackupCV
        {
            get { return this._SourceBackupCV; }
            set
            {
                this._SourceBackupCV = value;
                this.OnPropertyChanged("SourceBackupCV");
            }
        }

        public Component SelectedComponent
        {
            get { return this._selectedComponent; }
            set
            {
                _selectedComponent = value;
                this.OnPropertyChanged("SelectedComponent");
            }
        }
        
        public ObservableCollection<Component> ExportTablesComponentsList
        {
            get { return this._exportTablesComponentsList; }
            set
            {
                _exportTablesComponentsList = value;
                this.OnPropertyChanged("ExportTablesComponentsList");
            }
        }

        public ObservableCollection<ImportComponent> ImportTablesComponentsList
        {
            get { return this._importTablesComponentsList; }
            set
            {
                _importTablesComponentsList = value;
                this.OnPropertyChanged("ImportTablesComponentsList");
            }
        }

        public bool SelectAllExportTablesIsChecked
        {
            get { return this._selectAllExportTablesIsChecked; }
            set
            {
                this._selectAllExportTablesIsChecked = value;
                this.OnPropertyChanged("SelectAllExportTablesIsChecked");
            }
        }
        public bool SelectAllImportTablesIsChecked
        {
            get { return this._selectAllImportTablesIsChecked; }
            set
            {
                this._selectAllImportTablesIsChecked = value;
                this.OnPropertyChanged("SelectAllImportTablesIsChecked");
            }
        }
        public ObservableCollection<TransactionsPackage> AvailablePreactTransactionsPackages
        {
            get { return this._availablePreactTransactionsPackages; }
            set
            {
                if (_availablePreactTransactionsPackages != value)
                {
                    _availablePreactTransactionsPackages = value;
                    this.OnPropertyChanged("AvailablePreactTransactionsPackages");
                }
            }
        }
        
        public ObservableCollection<TransactionsPackage> AvailablePostactTransactionsPackages
        {
            get { return this._availablePostactTransactionsPackages; }
            set
            {
                if (_availablePostactTransactionsPackages != value)
                {
                    _availablePostactTransactionsPackages = value;
                    this.OnPropertyChanged("AvailablePostactTransactionsPackages");
                }
            }
        }

        public ObservableCollection<Transaction> SelectedTransactionsPreActList
        {
            get { return this._selectedTransactionsPreActList; }
            set
            {
                _selectedTransactionsPreActList = value;
                this.OnPropertyChanged("SelectedTransactionsPreActList");
            }
        }

        public IEnumerable<Transaction> SelectedTransactionsOnSelectedPreActList
        {
            get { return SelectedTransactionsPreActList.Where(o => o.IsSelectedOnSelectedList); }
        }

        public void AddSelectedPreActTransactions(object obj)
        {
            foreach (Transaction t in SelectedTransactions.ToList())
            {
                SelectedTransactionsPreActList.Add(t);
                t.IsSelected = false;
                t.IsEnabled = false;
            }
            /*SelectAllTransactionsCheckboxIsChecked = false;
            if (SelectedTransactionsList.Count > SelectedTransactionsOnSelectedList.ToList().Count)
                SelectAllTransactionsOnSelectedListCheckboxIsChecked = false;*/
        }

        public void RemoveSelectedTransactionsPreAct(object obj)
        {
            foreach (Transaction t in SelectedTransactionsOnSelectedPreActList.ToList())
            {
                SelectedTransactionsPreActList.Remove(t);
                t.IsSelectedOnSelectedList = false;
                t.IsEnabled = true;
            }
            /*
            if (SelectedTransactionsList.Count > SelectedTransactionsPreActList.ToList().Count)
                SelectAllTransactionsOnSelectedListCheckboxIsChecked = false;*/
        }
        public bool CanRemoveSelectedTransactionsPreAct(object obj)
        {
            if (SelectedTransactionsOnSelectedPreActList.ToList().Count > 0)
                return true;
            else
                return false;
        }

        public ObservableCollection<Transaction> SelectedTransactionsPostActList
        {
            get { return this._selectedTransactionsPostActList; }
            set
            {
                _selectedTransactionsPostActList = value;
                this.OnPropertyChanged("SelectedTransactionsPostActList");
            }
        }

        public IEnumerable<Transaction> SelectedTransactionsOnSelectedPostActList
        {
            get { return SelectedTransactionsPostActList.Where(o => o.IsSelectedOnSelectedList); }
        }

        public void AddSelectedPostActTransactions(object obj)
        {
            foreach (Transaction t in SelectedTransactions.ToList())
            {
                SelectedTransactionsPostActList.Add(t);
                t.IsSelected = false;
                t.IsEnabled = false;
            }
        }

        public void RemoveSelectedTransactionsPostAct(object obj)
        {
            foreach (Transaction t in SelectedTransactionsOnSelectedPostActList.ToList())
            {
                SelectedTransactionsPostActList.Remove(t);
                t.IsSelectedOnSelectedList = false;
                t.IsEnabled = true;
            }
        }
        public bool CanRemoveSelectedTransactionsPostAct(object obj)
        {
            if (SelectedTransactionsOnSelectedPostActList.ToList().Count > 0)
                return true;
            else
                return false;
        }

        public void SelectAllTransactionsOnSelectedPreActList(object obj)
        {
            CheckBox checkbok = (obj as CheckBox);
            foreach (Transaction trns in SelectedTransactionsPreActList)
            {
                if (checkbok.IsChecked.HasValue && checkbok.IsChecked.Value)
                {
                    trns.IsSelectedOnSelectedList = true;
                }
                else
                {
                    trns.IsSelectedOnSelectedList = false;
                }
            }
        }

        public bool CanSelectAllTransactionsOnSelectedPreActList(object obj)
        {
            if (SelectedTransactionsPreActList.Count > 0)
                return true;
            else
                return false;
        }

        public string SearchStringSourceServer
        {
            get { return _searchStringSourceServer; }
            set
            {
                this._searchStringSourceServer = value.ToUpper();

                var filter = _filteredSystemCatalog;

                if (_searchStringSourceServer != "")
                {
                    _searchCriteriaArray = _searchStringSourceServer.Split(" ").Where(x => !String.IsNullOrEmpty(x.Trim().ToUpper())).ToArray();
                    switch (_searchCriteriaArray.Length)
                    {
                        case 1:
                            if (filter != null)
                                _systemCatalog.Filter = o => (filter(o) && FilterServersOnlyPRD(o) && ServersSearchFirstWord(o));
                            else
                                _systemCatalog.Filter = o => (FilterServersOnlyPRD(o) && ServersSearchFirstWord(o));
                            break;
                        case 2:
                            if (filter != null)
                                _systemCatalog.Filter = o => (filter(o) && FilterServersOnlyPRD(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                            else
                                _systemCatalog.Filter = o => (FilterServersOnlyPRD(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                            break;
                        case 3:
                            if (filter != null)
                                _systemCatalog.Filter = o => (filter(o) && FilterServersOnlyPRD(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                            else
                                _systemCatalog.Filter = o => (FilterServersOnlyPRD(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                            break;
                        case 4:
                            if (filter != null)
                                _systemCatalog.Filter = o => (filter(o) && FilterServersOnlyPRD(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                            else
                                _systemCatalog.Filter = o => (FilterServersOnlyPRD(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                            break;
                        case 5:
                            if (filter != null)
                                _systemCatalog.Filter = o => (filter(o) && FilterServersOnlyPRD(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                            else
                                _systemCatalog.Filter = o => (FilterServersOnlyPRD(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                            break;
                    }
                }
                else
                {
                    if (filter != null)
                        _systemCatalog.Filter = o => (filter(o) && FilterServersOnlyPRD(o));
                    else
                        _systemCatalog.Filter = o => FilterServersOnlyPRD(o);
                }
            }
        }

        public string SearchStringTargetSAPServer
        {
            get { return _searchStringTargetSAPServer; }
            set
            {
                this._searchStringTargetSAPServer = value.ToUpper();

                var filter = _filteredSystemCatalog;


                if (SelectedServersList.Count > 0)
                {
                    if (_searchStringTargetSAPServer != "")
                    {
                        _searchCriteriaArray = _searchStringTargetSAPServer.Split(" ").Where(x => !String.IsNullOrEmpty(x.Trim().ToUpper())).ToArray();
                        switch (_searchCriteriaArray.Length)
                        {
                            case 1:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilterServersTarget(o) && ServersSearchFirstWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilterServersTarget(o) && ServersSearchFirstWord(o));
                                break;
                            case 2:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilterServersTarget(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilterServersTarget(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                break;
                            case 3:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilterServersTarget(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilterServersTarget(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                break;
                            case 4:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilterServersTarget(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilterServersTarget(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                break;
                            case 5:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilterServersTarget(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilterServersTarget(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                break;
                        }
                    }
                    else
                    {
                        if (filter != null)
                            _systemCatalog.Filter = o => (filter(o) && FilterServersTarget(o));
                        else
                            _systemCatalog.Filter = o => FilterServersTarget(o);
                    }
                }
                else
                {
                    if (_searchStringTargetSAPServer != "")
                    {
                        _searchCriteriaArray = _searchStringTargetSAPServer.Split(" ").Where(x => !String.IsNullOrEmpty(x.Trim().ToUpper())).ToArray();
                        switch (_searchCriteriaArray.Length)
                        {
                            case 1:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersTarget(o) && ServersSearchFirstWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersTarget(o) && ServersSearchFirstWord(o));
                                break;
                            case 2:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersTarget(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersTarget(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                break;
                            case 3:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersTarget(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersTarget(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                break;
                            case 4:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersTarget(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersTarget(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                break;
                            case 5:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersTarget(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersTarget(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                break;
                        }
                    }
                    else
                    {
                        if (filter != null)
                            _systemCatalog.Filter = o => (filter(o) && FilterServersTarget(o));
                        else
                            _systemCatalog.Filter = o => FilterServersTarget(o);
                    }
                }
            }
        }

        private bool FilterServersOnlyPRD(object item)
        {
            ServerSystem server = item as ServerSystem;
            if (server != null)
                return server.Environment.Trim().ToUpper() == "PRD"
                && server.CIDI.Trim().ToUpper() != "DI";
            else
                return true;
        }
        private bool FilterServersTarget(object item)
        {
            ServerSystem server = item as ServerSystem;
            if (server != null)
                return server.Customer.Trim().ToUpper() == SelectedSourceDBServer.Customer.Trim().ToUpper()
                && server.SID.Trim().ToUpper() != SelectedSourceDBServer.SID.Trim().ToUpper()
                //&& server.Environment.Trim().ToUpper() != "DEV"
                && server.Environment.Trim().ToUpper() != "PRD";
            else
                return true;
        }

        public void updateSourceBackupDateTime()
        {
            SourceBackupDateTime = DateTime.Parse(SourceBackupDate.ToString().Split(" ")[0] + " " + SourceBackupHourTime + ":" + SourceBackupMinuteTime);
        }

        public void ChangeSelectAllExportTables(object obj)
        {
            if (this._selectAllExportTablesIsChecked)
            {
                foreach (Component component in ExportTablesComponentsList)
                {
                    component.IsSelected = true;
                }
            }
            else
            {
                foreach (Component component in ExportTablesComponentsList)
                {
                    if(component.IsEnabled)
                        component.IsSelected = false;
                }
            }
        }
        public bool CanSelectAllExportTables(object obj)
        {
            if (ExportTablesComponentsList.Count > 0)
                return true;
            else
                return false;
        }

        public void ChangeSelectAllImportTables(object obj)
        {
            if (this._selectAllImportTablesIsChecked == true)
            {
                foreach (ImportComponent component in ImportTablesComponentsList)
                {
                    component.IsSelected = true;
                }
            }
            else
            {
                foreach (ImportComponent component in ImportTablesComponentsList)
                {
                    if (component.IsEnabled)
                        component.IsSelected = false;
                }
            }
        }
        public bool CanSelectAllImportTables(object obj)
        {
            if (ImportTablesComponentsList.Count > 0)
                return true;
            else
                return false;
        }

        public string SourceBDLSSID
        {
            get { return _sourceBDLSSID; }
            set
            {
                this._sourceBDLSSID = value.ToUpper();
                this.OnPropertyChanged("SourceBDLSSID");
            }
        }

        public string SourceClient
        {
            get { return _sourceClient; }
            set
            {
                this._sourceClient = value;
                this.OnPropertyChanged("SourceClient");
            }
        }

        public string TargetBDLSSID
        {
            get { return _targetBDLSSID; }
            set
            {
                this._targetBDLSSID = value.ToUpper();
                this.OnPropertyChanged("TargetBDLSSID");
            }
        }
        public string TargetClient
        {
            get { return _targetClient; }
            set
            {
                this._targetClient = value;
                this.OnPropertyChanged("TargetClient");
            }
        }

        public ObservableCollection<BDLS> BDLSList {
            get { return this._BDLSList; }
            set 
            { 
                this._BDLSList = value;
                this.OnPropertyChanged("BDLSList");
            }
        }

        public void AddBDLS(object obj)
        {
            BDLSList.Add(new BDLS { SourceSID = SourceBDLSSID, SourceClient = SourceClient, TargetSID = TargetBDLSSID, TargetClient = TargetClient, IsSelected = false });
        }

        public bool CanAddBDLS(object obj)
        {
            if (SourceBDLSSID.Length != 3 || TargetBDLSSID.Length != 3|| SourceClient.Length != 3|| TargetClient.Length != 3 )
                return false;
            else
                return true;
        }
        
        public void RemoveBDLS(object obj)
        {
            foreach (BDLS item in BDLSList.Where(o => o.IsSelected).ToList())
            {
                BDLSList.Remove(item);
            }
        }

        public bool CanRemoveBDLS(object obj)
        {
            if(BDLSList.Where(o => o.IsSelected).ToList().Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ChangeSelectPreActTransactionsSet(object obj)
        {
            TransactionsPackage pkg = (obj as TransactionsPackage);
            foreach (Transaction t in pkg.TransactionsPreact)
            {
                if (pkg.IsSelected)
                    SelectedTransactionsPreActList.Add(t);
                else
                    SelectedTransactionsPreActList.Remove(t);
            }
        }

        public bool CanChangeSelectPreActTransactionsSet(object obj)
        {
            TransactionsPackage pkg = (obj as TransactionsPackage);
            if (pkg.Subgroup == "SET 01" || pkg.Subgroup == "SET 02" || pkg.Subgroup == "SET 03")
            {
                return false;
            }
            else
                return true;
        }

    }
}
