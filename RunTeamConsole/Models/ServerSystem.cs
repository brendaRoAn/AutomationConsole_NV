using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RunTeamConsole.Views.Refresh;
using RunTeamConsole.Views.StartSapHadr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RunTeamConsole.Models
{
    public class ServerSystem : ObservableObject
    {
        private string _customer;
        private string _sid;
        private string _instance;
        private string _host;
        private string _type;
        private string _dqp;
        private string _version;
        private string _stack;
        private string _cidi;
        private string _os;
        private string _db;
        private string _region;
        private bool _isSelected;
        private bool _isEnabled;
        private bool _isSelectedOnSelectedList;
        //Refresh
        private bool _isSelectedOnSourceDB;
        private bool _isSelectedOnTargetSAP;
        private bool _isSelectedOnTargetDB;
        //HADR Variables Start
        private bool _isSelectedOnPrimaryDB, _isSelectedOnStandbyDB, _isSelectedOnAcscScs, _isSelectedOnErs, _isSelectedOnAas;
        //HADR Variables End

        public ServerSystem(string customer, string sidsys, string host, string type, string dqp, string version, string stack, string cidi, string os, string db, string region) 
        {
            string[] sidsysArray = sidsys.Split('_');
            this._customer = customer;
            this._sid = sidsysArray[0];
            this._instance= sidsysArray[1];
            this._host = host;
            this._type = type;
            this._dqp = dqp;
            this._version = version;
            this._stack = stack;
            this._cidi = cidi;
            this._os = os;
            this._db = db;
            this._region = region;
            this._isSelected = false;
            this._isEnabled = true;
        }

        [JsonConstructor]
        public ServerSystem(string customer, string sid, string instance, string hostname, string producttype, string environment, string version, string stack, string cidi, string os, string dbtype, string region)
        {
            this._customer = customer;
            this._sid = sid;
            this._instance = instance;
            this._host = hostname;
            this._type = producttype;
            this._dqp = environment;
            this._version = version;
            this._stack = stack;
            this._cidi = cidi.Trim();
            this._os = os;
            this._db = dbtype;
            this._region = region;
            this._isSelected = false;
            this._isEnabled = true;
        }

        public ServerSystem(JToken serverJson)
        {
            string[] sidsysArray;
            string customer;
            customer = (string)serverJson["customer"];
            
            string sidsys = GetAttributeFromHtmlString((string)serverJson["sid"]);
            sidsysArray = sidsys.Split('_');

            this._customer = GetCustomerFromHtmlString(customer);

            this._sid = sidsysArray[0];
            if(sidsysArray.Length>1)
                this._instance = sidsysArray[1];
            this._host = GetAttributeFromHtmlString((string)serverJson["hostname"]);
            this._type = (string)serverJson["type"];
            this._dqp = (string)serverJson["dqp"];
            this._version = (string)serverJson["version"];
            this._stack = (string)serverJson["stack"];
            this._cidi = GetAttributeFromHtmlString((string)serverJson["cidi"]);
            this._os = (string)serverJson["os"];
            this._db = (string)serverJson["db"];
            this._region = (string)serverJson["region"];
            this._isSelected = false;
            this._isEnabled = true;
        }

        [JsonIgnore]
        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                if (this.IsEnabled)
                {
                    this._isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                    if (MainWindow.AddPVMInstance != null)
                    {
                        if (value)
                        {
                            if (MainWindow.AddPVMInstance.SystemCatalog.Cast<ServerSystem>().ToList().Count == MainWindow.AddPVMInstance.SelectedServers.ToList().Count)
                                MainWindow.AddPVMInstance.SelectAllServersCheckboxIsChecked = true;
                            if(MainWindow.AddPVMInstance.SelectedProcess.SystemCopyModules != "NA" && MainWindow.AddPVMInstance.CurrentControl.UserControl is SelectTargetSystemView)
                            {
                                foreach(ServerSystem server in MainWindow.AddPVMInstance.SystemCatalog.Cast<ServerSystem>().ToList().Where(x => x.IsSelected).ToList())
                                {
                                    if (server.SID != this.SID)
                                        server.IsSelected = false;
                                }
                            }
                        }
                        else
                            MainWindow.AddPVMInstance.SelectAllServersCheckboxIsChecked = false;
                    }
                }
            }
        }

        [JsonIgnore]
        public bool IsEnabled
        {
            get { return this._isEnabled; }
            set
            {
                this._isEnabled = value;
                this.OnPropertyChanged("IsEnabled");
            }
        }

        [JsonIgnore]
        public bool IsSelectedOnSelectedList
        {
            get { return this._isSelectedOnSelectedList; }
            set
            {
                this._isSelectedOnSelectedList = value;
                this.OnPropertyChanged("IsSelectedOnSelectedList");
                if (value)
                {
                    if (MainWindow.AddPVMInstance.SelectedServersList.Count == MainWindow.AddPVMInstance.SelectedServersOnSelectedList.ToList().Count)
                        MainWindow.AddPVMInstance.SelectAllServersOnSelectedListCheckboxIsChecked = true;
                }
                else
                    MainWindow.AddPVMInstance.SelectAllServersOnSelectedListCheckboxIsChecked = false;
            }
        }

        [JsonIgnore]
        public bool IsSelectedOnSourceDB
        {
            get { return this._isSelectedOnSourceDB; }
            set
            {
                this._isSelectedOnSourceDB = value;
                this.OnPropertyChanged("IsSelectedOnSourceDB");
                if (value == true)
                {
                    MainWindow.AddPVMInstance.SelectedSourceDBServer = this;
                }
                else if (MainWindow.AddPVMInstance.SelectedSourceDBServer == this)
                    MainWindow.AddPVMInstance.SelectedSourceDBServer = null;
            }
        }
        [JsonIgnore]
        public bool IsSelectedOnPrimaryDB
        {
            get { return this._isSelectedOnPrimaryDB; }
            set
            {
                this._isSelectedOnPrimaryDB = value;
                this.OnPropertyChanged("IsSelectedOnPrimaryDB");
                if (value == true)
                {
                    MainWindow.AddPVMInstance.SelectedPrimaryDBServer = this;
                    this._isSelectedOnStandbyDB = false;
                }
                else if (MainWindow.AddPVMInstance.SelectedPrimaryDBServer == this)
                    MainWindow.AddPVMInstance.SelectedPrimaryDBServer = null;
            }
        }
        [JsonIgnore]
        public bool IsSelectedOnStandbyDB
        {
            get { return this._isSelectedOnStandbyDB; }
            set
            {

                this._isSelectedOnStandbyDB = value;
                this.OnPropertyChanged("IsSelectedOnStandbyDB");
                if (value == true)
                {
                    MainWindow.AddPVMInstance.SelectedStandbyDBServer = this;
                }
                else if (MainWindow.AddPVMInstance.SelectedStandbyDBServer == this)
                    MainWindow.AddPVMInstance.SelectedStandbyDBServer = null;
            }
        }
        [JsonIgnore]
        public bool IsSelectedOnAcscScs
        {
            get { return this._isSelectedOnAcscScs; }
            set
            {
                this._isSelectedOnAcscScs = value;
                this.OnPropertyChanged("IsSelectedOnAcscScs");
                if (value == true)
                {
                    MainWindow.AddPVMInstance.SelectedAcscScsServer = this;
                }
                else if (MainWindow.AddPVMInstance.SelectedAcscScsServer == this)
                    MainWindow.AddPVMInstance.SelectedAcscScsServer = null;
            }
        }
        [JsonIgnore]
        public bool IsSelectedOnErs
        {
            get { return this._isSelectedOnErs; }
            set
            {
                this._isSelectedOnErs = value;
                this.OnPropertyChanged("IsSelectedOnErs");
                if (value == true)
                {
                    MainWindow.AddPVMInstance.SelectedErsServer = this;
                }
                else if (MainWindow.AddPVMInstance.SelectedErsServer == this)
                    MainWindow.AddPVMInstance.SelectedErsServer = null;
            }
        }
        [JsonIgnore]
        public bool IsSelectedOnAas
        {
            get { return this._isSelectedOnAas; }
            set
            {
                this._isSelectedOnAas = value;
                this.OnPropertyChanged("IsSelectedOnAas");
                if (value == true)
                {
                    MainWindow.AddPVMInstance.SelectedAasServer = this;
                }
                else if (MainWindow.AddPVMInstance.SelectedAasServer == this)
                    MainWindow.AddPVMInstance.SelectedAasServer = null;
            }
        }
        [JsonIgnore]
        public bool IsSelectedOnTargetSAP
        {
            get { return this._isSelectedOnTargetSAP; }
            set
            {
                this._isSelectedOnTargetSAP = value;
                this.OnPropertyChanged("IsSelectedOnTargetSAP");
                if (value == true)
                    MainWindow.AddPVMInstance.SelectedTargetSAPServer = this;
                else if (MainWindow.AddPVMInstance.SelectedTargetSAPServer == this)
                    MainWindow.AddPVMInstance.SelectedTargetSAPServer = null;
            }
        }
        
        [JsonIgnore]
        public bool IsSelectedOnTargetDB
        {
            get { return this._isSelectedOnTargetDB; }
            set
            {
                this._isSelectedOnTargetDB = value;
                this.OnPropertyChanged("IsSelectedOnTargetDB");
                if (value == true)
                    MainWindow.AddPVMInstance.SelectedTargetDBServer = this;
                else if (MainWindow.AddPVMInstance.SelectedTargetDBServer == this)
                    MainWindow.AddPVMInstance.SelectedTargetDBServer = null;
            }
        }

        public string Customer
        {
            get { return this._customer; }
        }
        public string SID
        {
            get { return this._sid; }
        }
        public string Instance
        {
            get { return this._instance; }
        }
        public string Hostname
        {
            get { return this._host; }
        }
        public string ProductType
        {
            get { return this._type; }
        }
        public string Environment
        {
            get { return this._dqp; }
        }
        public string Version
        {
            get { return this._version; }
        }
        public string Stack
        {
            get { return this._stack; }
        }
        public string CIDI
        {
            get { return this._cidi; }
            set { this._cidi = value; }
        }
        public string OS
        {
            get { return this._os; }
            set { this._os = value; }

        }
        public string DBType
        {
            get { return this._db; }
            set { this._db = value; }
        }
        public string Region
        {
            get { return this._region; }
        }
        private string GetCustomerFromHtmlString(string htmlStr)
        {
            string[] customerArray;
            if (htmlStr.Contains("</a>"))
            {
                customerArray = htmlStr.Split("</a>");
                if (customerArray[1].Contains("\">"))
                    return customerArray[1].Split("\">")[1];
                else if(customerArray[0].Contains("\">"))
                    return customerArray[0].Split("\">")[1];
                else
                    return customerArray[0];
            }
            else
                return htmlStr;
        }
        private string GetAttributeFromHtmlString(string htmlStr)
        {
            string[] customerArray;
            if (htmlStr.Contains("</span>"))
            {
                customerArray = htmlStr.Split("</span>");
                if (customerArray[0].Contains("\">"))
                    return customerArray[0].Split("\">")[1];
                else
                    return customerArray[0];
            }
            else
                return htmlStr;
        }
    }
}
