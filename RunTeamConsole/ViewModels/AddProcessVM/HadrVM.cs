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
        //Variables for servers
        //Search strings
        private string _searchStringPrimaryDB, _searchStringStandBy, _searchStringAscsScs, _searchStringErs, _searchStringAas;
        //Selected servers
        private ServerSystem _selectedPrimaryDBServer, _selectedStandbyDBServer, _selectedAcscScsServer, _selectedErsServer, _selectedAasServer;
        //List of servers
        private ObservableCollection<ServerSystem> _sapAasServerList = new ObservableCollection<ServerSystem>();
        private ObservableCollection<ServerSystem> _primaryDBServerList;// = new ObservableCollection<ServerSystem>();
        private ObservableCollection<ServerSystem> _standbyDBServerList;// = new ObservableCollection<ServerSystem>();
        private ObservableCollection<ServerSystem> _sapAcscScsServerList;// = new ObservableCollection<ServerSystem>();
        private ObservableCollection<ServerSystem> _sapErsServerList;// = new ObservableCollection<ServerSystem>();

        //Variables for data
        private string _sapUserSapsaPassword, _sapUserDisRecUser, _sapUserDisRecPass;

        #region ServerSystem Variables
        public ServerSystem SelectedPrimaryDBServer
        {
            get { return this._selectedPrimaryDBServer; }
            set
            {
                this._selectedPrimaryDBServer = value;
                this.OnPropertyChanged("SelectedPrimaryDBServer");
            }
        }
        public ServerSystem SelectedStandbyDBServer
        {
            get { return this._selectedStandbyDBServer; }
            set
            {
                this._selectedStandbyDBServer = value;
                this.OnPropertyChanged("SelectedStandbyDBServer");
            }
        }
        public ServerSystem SelectedAcscScsServer
        {
            get { return this._selectedAcscScsServer; }
            set
            {
                this._selectedAcscScsServer = value;
                this.OnPropertyChanged("SelectedAcscScsServer");
            }
        }
        public ServerSystem SelectedErsServer
        {
            get { return this._selectedErsServer; }
            set
            {
                this._selectedErsServer = value;
                this.OnPropertyChanged("SelectedErsServer");
            }
        }
        public ServerSystem SelectedAasServer
        {
            get
            { 
                return this._selectedAasServer; 
            }
            set
            {
                this._selectedAasServer = value;
                this.OnPropertyChanged("SelectedAasServer");
            }
        }
        #endregion

        #region string Variables
        public string SapUserSapsaPassword
        {
            get { return this._sapUserSapsaPassword; }
            set
            {
                this._sapUserSapsaPassword = value;
                this.OnPropertyChanged("SapUserSapsaPassword");
            }
        }
        public string SapUserDisasterRecoveryUser
        {
            get { return this._sapUserDisRecUser; }
            set
            {
                this._sapUserDisRecUser = value;
                this.OnPropertyChanged("SapUserDisasterRecoveryUser");
            }
        }
        public string SapUserDisasterRecoveryPassword
        {
            get { return this._sapUserDisRecPass; }
            set
            {
                this._sapUserDisRecPass = value;
                this.OnPropertyChanged("SapUserDisasterRecoveryPassword");
            }
        }
        #endregion

        #region ObservableCollection<ServerSystem> Variables
        public ObservableCollection<ServerSystem> PrimaryDBServerList
        {
            get { return this._primaryDBServerList; }
            set
            {
                this._primaryDBServerList = value;
                this.OnPropertyChanged("PrimaryDBServerList");
            }
        }
        public ObservableCollection<ServerSystem> StandbyDBServerList
        {
            get { return this._standbyDBServerList; }
            set
            {
                this._standbyDBServerList = value;
                this.OnPropertyChanged("StandbyDBServerList");
            }
        }
        public ObservableCollection<ServerSystem> SapAcscScsServerList
        {
            get { return this._sapAcscScsServerList; }
            set
            {
                this._sapAcscScsServerList = value;
                this.OnPropertyChanged("SapAcscScsServerList");
            }
        }
        public ObservableCollection<ServerSystem> SapErsServerList
        {
            get { return this._sapErsServerList; }
            set
            {
                this._sapErsServerList = value;
                this.OnPropertyChanged("SapErsServerList");
            }
        }
        public ObservableCollection<ServerSystem> SapAasServerList
        {
            get { return this._sapAasServerList; }
            set
            {
                this._sapAasServerList = value;
                this.OnPropertyChanged("SapAasServerList");
            }
        }
        #endregion
        
        //==============================SERVERS FILTERS=====================================
        #region Server Filters
        private bool FilteredServerForStandBy(object item)
        {
            ServerSystem server = item as ServerSystem;
            if (server != null)
                return server.Customer.Trim().ToUpper() == SelectedPrimaryDBServer.Customer.Trim().ToUpper()
                && server.SID.Trim().ToUpper() == SelectedPrimaryDBServer.SID.Trim().ToUpper() 
                && server.Hostname.Trim().ToUpper() != SelectedPrimaryDBServer.Hostname.Trim().ToUpper();
            else
                return true;
        }
        private bool FilteredServerForAscsScs(object item)
        {
            ServerSystem server = item as ServerSystem;
            if (server != null)
                return server.Customer.Trim().ToUpper() == SelectedPrimaryDBServer.Customer.Trim().ToUpper()
                && server.SID.Trim().ToUpper() == SelectedPrimaryDBServer.SID.Trim().ToUpper()
                && server.Hostname.Trim().ToUpper() != SelectedPrimaryDBServer.Hostname.Trim().ToUpper()
                && server.Hostname.Trim().ToUpper() != SelectedStandbyDBServer.Hostname.Trim().ToUpper();
            else
                return true;
        }
        private bool FilteredServerForErs(object item)
        {
            ServerSystem server = item as ServerSystem;
            if (server != null)
                return server.Customer.Trim().ToUpper() == SelectedPrimaryDBServer.Customer.Trim().ToUpper()
                && server.SID.Trim().ToUpper() == SelectedPrimaryDBServer.SID.Trim().ToUpper() 
                && server.Hostname.Trim().ToUpper() != SelectedPrimaryDBServer.Hostname.Trim().ToUpper() 
                && server.Hostname.Trim().ToUpper() != SelectedStandbyDBServer.Hostname.Trim().ToUpper()
                && server.Hostname.Trim().ToUpper() != SelectedAcscScsServer.Hostname.Trim().ToUpper();
            else
                return true;
        }
        private bool FilteredServerForAas(object item)
        {
            ServerSystem server = item as ServerSystem;
            if (server != null)
                return server.Customer.Trim().ToUpper() == SelectedPrimaryDBServer.Customer.Trim().ToUpper()
                && server.SID.Trim().ToUpper() == SelectedPrimaryDBServer.SID.Trim().ToUpper()
                && server.Hostname.Trim().ToUpper() != SelectedPrimaryDBServer.Hostname.Trim().ToUpper()
                && server.Hostname.Trim().ToUpper() != SelectedStandbyDBServer.Hostname.Trim().ToUpper()
                && server.Hostname.Trim().ToUpper() != SelectedAcscScsServer.Hostname.Trim().ToUpper()
                && server.Hostname.Trim().ToUpper() != SelectedErsServer.Hostname.Trim().ToUpper();
            else
                return true;
        }
        #endregion

        //==============================SEARCH STRINGS======================================
        #region Search Strings
        public string SearchStringPrimaryDB
        {
            get { return _searchStringPrimaryDB; }
            set
            {
                this._searchStringPrimaryDB = value.ToUpper();

                var filter = _filteredSystemCatalog;


                if (SelectedServersList.Count > 0)
                {
                    if (_searchStringPrimaryDB != "")
                    {
                        _searchCriteriaArray = _searchStringPrimaryDB.Split(" ").Where(x => !String.IsNullOrEmpty(x.Trim().ToUpper())).ToArray();
                        switch (_searchCriteriaArray.Length)
                        {
                            case 1:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && ServersSearchFirstWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && ServersSearchFirstWord(o));
                                break;
                            case 2:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                break;
                            case 3:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                break;
                            case 4:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                break;
                            case 5:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                break;
                        }
                    }
                    else
                    {
                        if (filter != null)
                            _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o));
                        else
                            _systemCatalog.Filter = o => FilterServersBySelectedSID(o);
                    }
                }
                else
                {
                    if (_searchStringPrimaryDB != "")
                    {
                        _searchCriteriaArray = _searchStringPrimaryDB.Split(" ").Where(x => !String.IsNullOrEmpty(x.Trim().ToUpper())).ToArray();
                        switch (_searchCriteriaArray.Length)
                        {
                            case 1:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && ServersSearchFirstWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && ServersSearchFirstWord(o));
                                break;
                            case 2:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                break;
                            case 3:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                break;
                            case 4:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                break;
                            case 5:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                break;
                        }
                    }
                    else
                    {
                        if (filter != null)
                            _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o));
                        else
                            _systemCatalog.Filter = o => FilterServersBySelectedSID(o);
                    }
                }
            }
        }
        public string SearchStringStandBy
        {
            get { return _searchStringStandBy; }
            set
            {
                this._searchStringStandBy = value.ToUpper();

                var filter = _filteredSystemCatalog;


                if (SelectedServersList.Count > 0)
                {
                    if (_searchStringStandBy != "")
                    {
                        _searchCriteriaArray = _searchStringStandBy.Split(" ").Where(x => !String.IsNullOrEmpty(x.Trim().ToUpper())).ToArray();
                        switch (_searchCriteriaArray.Length)
                        {
                            case 1:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForStandBy(o) && ServersSearchFirstWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForStandBy(o) && ServersSearchFirstWord(o));
                                break;
                            case 2:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForStandBy(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForStandBy(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                break;
                            case 3:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForStandBy(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForStandBy(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                break;
                            case 4:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForStandBy(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForStandBy(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                break;
                            case 5:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForStandBy(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForStandBy(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                break;
                        }
                    }
                    else
                    {
                        if (filter != null)
                            _systemCatalog.Filter = o => (filter(o) && FilteredServerForStandBy(o));
                        else
                            _systemCatalog.Filter = o => FilteredServerForStandBy(o);
                    }
                }
                else
                {
                    if (_searchStringStandBy != "")
                    {
                        _searchCriteriaArray = _searchStringStandBy.Split(" ").Where(x => !String.IsNullOrEmpty(x.Trim().ToUpper())).ToArray();
                        switch (_searchCriteriaArray.Length)
                        {
                            case 1:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForStandBy(o) && ServersSearchFirstWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForStandBy(o) && ServersSearchFirstWord(o));
                                break;
                            case 2:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForStandBy(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForStandBy(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                break;
                            case 3:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForStandBy(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForStandBy(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                break;
                            case 4:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForStandBy(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForStandBy(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                break;
                            case 5:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForStandBy(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForStandBy(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                break;
                        }
                    }
                    else
                    {
                        if (filter != null)
                            _systemCatalog.Filter = o => (filter(o) && FilteredServerForStandBy(o));
                        else
                            _systemCatalog.Filter = o => FilteredServerForStandBy(o);
                    }
                }
            }
        }
        public string SearchStringAscsScs
        {
            get { return _searchStringAscsScs; }
            set
            {
                this._searchStringAscsScs = value.ToUpper();

                var filter = _filteredSystemCatalog;


                if (SelectedServersList.Count > 0)
                {
                    if (_searchStringAscsScs != "")
                    {
                        _searchCriteriaArray = _searchStringAscsScs.Split(" ").Where(x => !String.IsNullOrEmpty(x.Trim().ToUpper())).ToArray();
                        switch (_searchCriteriaArray.Length)
                        {
                            case 1:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForAscsScs(o) && ServersSearchFirstWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForAscsScs(o) && ServersSearchFirstWord(o));
                                break;
                            case 2:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForAscsScs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForAscsScs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                break;
                            case 3:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForAscsScs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForAscsScs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                break;
                            case 4:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForAscsScs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForAscsScs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                break;
                            case 5:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForAscsScs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForAscsScs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                break;
                        }
                    }
                    else
                    {
                        if (filter != null)
                            _systemCatalog.Filter = o => (filter(o) && FilteredServerForAscsScs(o));
                        else
                            _systemCatalog.Filter = o => FilteredServerForAscsScs(o);
                    }
                }
                else
                {
                    if (_searchStringAscsScs != "")
                    {
                        _searchCriteriaArray = _searchStringAscsScs.Split(" ").Where(x => !String.IsNullOrEmpty(x.Trim().ToUpper())).ToArray();
                        switch (_searchCriteriaArray.Length)
                        {
                            case 1:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForAscsScs(o) && ServersSearchFirstWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForAscsScs(o) && ServersSearchFirstWord(o));
                                break;
                            case 2:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForAscsScs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForAscsScs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                break;
                            case 3:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForAscsScs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForAscsScs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                break;
                            case 4:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForAscsScs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForAscsScs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                break;
                            case 5:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForAscsScs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForAscsScs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                break;
                        }
                    }
                    else
                    {
                        if (filter != null)
                            _systemCatalog.Filter = o => (filter(o) && FilteredServerForAscsScs(o));
                        else
                            _systemCatalog.Filter = o => FilteredServerForAscsScs(o);
                    }
                }
            }
        }
        public string SearchStringErs
        {
            get { return _searchStringErs; }
            set
            {
                this._searchStringErs = value.ToUpper();

                var filter = _filteredSystemCatalog;


                if (SelectedServersList.Count > 0)
                {
                    if (_searchStringErs != "")
                    {
                        _searchCriteriaArray = _searchStringErs.Split(" ").Where(x => !String.IsNullOrEmpty(x.Trim().ToUpper())).ToArray();
                        switch (_searchCriteriaArray.Length)
                        {
                            case 1:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForErs(o) && ServersSearchFirstWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForErs(o) && ServersSearchFirstWord(o));
                                break;
                            case 2:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForErs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForErs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                break;
                            case 3:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForErs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForErs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                break;
                            case 4:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForErs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForErs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                break;
                            case 5:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForErs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForErs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                break;
                        }
                    }
                    else
                    {
                        if (filter != null)
                            _systemCatalog.Filter = o => (filter(o) && FilteredServerForErs(o));
                        else
                            _systemCatalog.Filter = o => FilteredServerForErs(o);
                    }
                }
                else
                {
                    if (_searchStringErs != "")
                    {
                        _searchCriteriaArray = _searchStringErs.Split(" ").Where(x => !String.IsNullOrEmpty(x.Trim().ToUpper())).ToArray();
                        switch (_searchCriteriaArray.Length)
                        {
                            case 1:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForErs(o) && ServersSearchFirstWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForErs(o) && ServersSearchFirstWord(o));
                                break;
                            case 2:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForErs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForErs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                break;
                            case 3:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForErs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForErs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                break;
                            case 4:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForErs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForErs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                break;
                            case 5:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForErs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForErs(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                break;
                        }
                    }
                    else
                    {
                        if (filter != null)
                            _systemCatalog.Filter = o => (filter(o) && FilteredServerForErs(o));
                        else
                            _systemCatalog.Filter = o => FilteredServerForErs(o);
                    }
                }
            }
        }
        public string SearchStringAas
        {
            get { return _searchStringAas; }
            set
            {
                this._searchStringAas = value.ToUpper();

                var filter = _filteredSystemCatalog;


                if (SelectedServersList.Count > 0)
                {
                    if (_searchStringAas != "")
                    {
                        _searchCriteriaArray = _searchStringAas.Split(" ").Where(x => !String.IsNullOrEmpty(x.Trim().ToUpper())).ToArray();
                        switch (_searchCriteriaArray.Length)
                        {
                            case 1:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForAas(o) && ServersSearchFirstWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForAas(o) && ServersSearchFirstWord(o));
                                break;
                            case 2:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForAas(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForAas(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                break;
                            case 3:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForAas(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForAas(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                break;
                            case 4:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForAas(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForAas(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                break;
                            case 5:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilterServersBySelectedSID(o) && FilteredServerForAas(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilterServersBySelectedSID(o) && FilteredServerForAas(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                break;
                        }
                    }
                    else
                    {
                        if (filter != null)
                            _systemCatalog.Filter = o => (filter(o) && FilteredServerForAas(o));
                        else
                            _systemCatalog.Filter = o => FilteredServerForAas(o);
                    }
                }
                else
                {
                    if (_searchStringAas != "")
                    {
                        _searchCriteriaArray = _searchStringAas.Split(" ").Where(x => !String.IsNullOrEmpty(x.Trim().ToUpper())).ToArray();
                        switch (_searchCriteriaArray.Length)
                        {
                            case 1:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForAas(o) && ServersSearchFirstWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForAas(o) && ServersSearchFirstWord(o));
                                break;
                            case 2:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForAas(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForAas(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o));
                                break;
                            case 3:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForAas(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForAas(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o));
                                break;
                            case 4:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForAas(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForAas(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o));
                                break;
                            case 5:
                                if (filter != null)
                                    _systemCatalog.Filter = o => (filter(o) && FilteredServerForAas(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                else
                                    _systemCatalog.Filter = o => (FilteredServerForAas(o) && ServersSearchFirstWord(o) && ServersSearchSecondWord(o) && ServersSearchThirdWord(o) && ServersSearchFourthWord(o) && ServersSearchFifthWord(o));
                                break;
                        }
                    }
                    else
                    {
                        if (filter != null)
                            _systemCatalog.Filter = o => (filter(o) && FilteredServerForAas(o));
                        else
                            _systemCatalog.Filter = o => FilteredServerForAas(o);
                    }
                }
            }
        }
        #endregion
    }
}
