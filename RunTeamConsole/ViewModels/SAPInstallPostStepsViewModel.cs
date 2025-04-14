using RunTeamConsole.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using RunTeamConsole.Models.Packages;
using RunTeamConsole.Models.SapInstallPostSteps;
using RunTeamConsole.Models.AddProcesses;
using RunTeamConsole.ViewModels.Commands;
using RunTeamConsole.Views.SapInstallPostSteps;
using System.Text.RegularExpressions;
using System.Windows;
using System.Net;

namespace RunTeamConsole.ViewModels
{
    public partial class AddProcessViewModel : ObservableObject
    {
        private bool _rz10Active = true, _sappfparActive = true, _se38Active = true, _stmsccmsActive = true, _stmstruckActive = true, _st03Active = true, _st04nActive = true, _st06Active = true, _sickActive = true, _sm12Active = true, _sm13Active = true, _sm65Active = true;
        private string _rz10FqicpName, _rz10FqicpValue, _rz10AddpName, _rz10AddpValue;
        private string _smlgCustomerName, _smlgInstanceGroup, _smlgRfcType, _smlgIpGroup;
        private bool _smlgRfcEnabled = true, checkIp = false;
        private string _sm36SapUser, _sm36SapPassword;
        private string _rz12GroupName, _rz12InstanceGroup;
        private int _rz12MaxQueue, _rz12MaxLogin, _rz12MaxSeparateLogons, _rz12Maxwp, _rz12Minfreewp, _rz12Maxcomm, _rz12MaxWaitTime, _rz12ActivatedNumber;
        bool _rz12Activated = true, checkMaxQ, checkMaxL, checkMaxSL, checkMaxWp, checkMinF, checkMaxcomm, checkMaxWT;
        private string _sm61GroupName, _sm61Instance;
        private string _rz04OperationName, _rz04Description;
        private int _rz04InTimeHour, _rz04InTimeMinute, _rz04EndTimeHour, _rz04EndTimeMinute;
        private string _rz70GatewayHost, _rz70GatewayService;
        private string _al11DirectoryPath, _al11DirectoryName, _al11ValidForServer;
        private string _strust02CertificateType, _strust02CertificatePath, _strust02CertificateName;
        private int? _scc4Client;
        private string _scc4ClientName, _scc4ClientCity, _scc4LogicalName, _scc4Currency, _scc4ClientRole, _scc4ChangesAndTransport, _scc4CrossClient, _scc4CopyComparisonTool, _scc4CattAndEcattRest;
        private int _sm21FromHour, _sm21FromMinute, _sm21ToHour, _sm21ToMinute;
        private DateTime _sm21FromDate, _sm21ToDate;
        private int _st22FromHour, _st22FromMinute, _st22ToHour, _st22ToMinute;
        private DateTime _st22FromDate, _st22ToDate;
        private string _st22User;
        private string _db13Job, _db13Recurrence, _db13Range, _db13RecurrenceDay, _db13RangeNoEnd, _db13RangeEndAfter;
        private int _db13StartDateHour, _db13StartDateMinute, _db13RecurrenceDayHour, _db13RecurrenceDayMinute, _db13RecurrenceHour, _db13RecurrenceOnceHour, _db13RecurrenceOnceMinute, _db13RangeEndByhour, _db13RangeEndByMinute;
        private TimeSpan _db13RecurrenceDayTimeSpan;
        private DateTime _db13StartDate, _db13RecurrenceOnceDate, _db13RangeEndByDate;
        private bool VarToCheckAl11Name = false;

        ObservableCollection<Rz10FqicpSettingsConfiguration> _fqicpList;
        ObservableCollection<Rz10AddpSettingsConfiguration> _addpList;
        ObservableCollection<SmlgSettingsConfiguration> _smlgList;
        ObservableCollection<Rz12SettingsConfiguration> _rz12List;
        ObservableCollection<Sm61SettingsConfiguration> _sm61List;
        ObservableCollection<Rz04SettingsConfiguration> _rz04List;
        ObservableCollection<Rz70SettingsConfiguration> _rz70List;
        ObservableCollection<Al11SettingsConfiguration> _al11List;
        ObservableCollection<Strust02SettingsConfiguration> _strust02List;
        ObservableCollection<Scc4SettingsConfiguration> _scc4List;
        ObservableCollection<Db13SettingsConfiguration> _db13List;

        public RelayCommand AddtoFqicpListCommand { get; private set; }
        public RelayCommand RemoveFromFqicpListCommand { get; private set; }
        public RelayCommand AddtoAddpListCommand { get; private set; }
        public RelayCommand RemoveFromAddpListCommand { get; private set; }
        public RelayCommand AddtoSmlgListCommand { get; private set; }
        public RelayCommand RemoveFromSmlgListCommand { get; private set; }
        public RelayCommand AddtoRz12ListCommand { get; private set; }
        public RelayCommand RemoveFromRz12ListCommand { get; private set; }
        public RelayCommand AddtoSm61ListCommand { get; private set; }
        public RelayCommand RemoveFromSm61ListCommand { get; private set; }
        public RelayCommand AddtoRz04ListCommand { get; private set; }
        public RelayCommand RemoveFromRz04ListCommand { get; private set; }
        public RelayCommand AddtoRz70ListCommand { get; private set; }
        public RelayCommand RemoveFromRz70ListCommand { get; private set; }
        public RelayCommand AddtoAl11ListCommand { get; private set; }
        public RelayCommand RemoveFromAl11ListCommand { get; private set; }
        public RelayCommand AddtoStrust02ListCommand { get; private set; }
        public RelayCommand RemoveFromStrust02ListCommand { get; private set; }
        public RelayCommand AddtoScc4ListCommand { get; private set; }
        public RelayCommand RemoveFromScc4ListCommand { get; private set; }
        public RelayCommand AddtoDb13ListCommand { get; private set; }
        public RelayCommand RemoveFromDb13ListCommand { get; private set; }

        #region Steps Configuration
        public bool Rz10Active
        {
            get { return _rz10Active; }
            set{ _rz10Active = value; }
        }
        public bool ShowRZ10Window
        {
            get
            {
                if(Rz10Active)
                    return true;
                else
                    return false;
            }
        }
        public bool SappfparActive
        {
            get { return _sappfparActive; }
            set { _sappfparActive = value; }
        }
        public bool Se38Active
        {
            get { return _se38Active; }
            set { _se38Active = value; }
        }
        public bool Stmsccms
        {
            get { return _stmsccmsActive; }
            set { _stmsccmsActive = value; }
        }
        public bool Stmstruck
        {
            get { return _stmstruckActive; }
            set { _stmstruckActive = value; }
        }
        public bool St03
        {
            get { return _st03Active; }
            set { _st03Active = value; }
        }
        public bool St04n
        {
            get { return _st04nActive; }
            set { _st04nActive = value; }
        }
        public bool St06
        {
            get { return _st06Active; }
            set { _st06Active = value; }
        }
        public bool Sick
        {
            get { return _sickActive; }
            set { _sickActive = value; }
        }
        public bool Sm12
        {
            get { return _sm12Active; }
            set { _sm12Active = value; }
        }
        public bool Sm65
        {
            get { return _sm65Active; }
            set { _sm65Active = value; }
        }
        public bool Sm13
        {
            get { return _sm13Active; }
            set { _sm13Active = value; }
        }
        #endregion

        #region RZ10 configuration
        public string Rz10FqicpName
        {
            get { return _rz10FqicpName; }
            set
            {
                if (value == null)
                    this._rz10FqicpName = null;
                else if(value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[&+,:;=?#|'<>.^*!\\[\\]{}`°¬/¨´~¿¡])"))
                        {
                            if (!Regex.IsMatch(c.ToString(), " "))
                                valueToPut = valueToPut += c.ToString();
                        }
                    }
                    _rz10FqicpName = valueToPut;
                }
                else
                    _rz10FqicpName = null;
                this.OnPropertyChanged("Rz10FqicpName");
            }
        }
        public string Rz10FqicpValue
        {
            get { return _rz10FqicpValue; }
            set
            {
                if (value == null)
                    this._rz10FqicpValue = value;
                else if (value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[&+,:;=?#|'<>.^*!\\[\\]{}`°¬/¨´~¿¡])"))
                            valueToPut = valueToPut += c.ToString();
                    }
                    _rz10FqicpValue = valueToPut;
                }
                else
                    _rz10FqicpValue = null;
                this.OnPropertyChanged("Rz10FqicpValue");
            }
        }
        public string Rz10AddpName
        {
            get { return _rz10AddpName; }
            set
            {
                if (value == null)
                    this._rz10AddpName = value;
                else if (value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[&+,:;=?#|'<>.^*!\\[\\]{}`°¬/¨´~¿¡])"))
                        {
                            if (!Regex.IsMatch(c.ToString(), " "))
                                valueToPut = valueToPut += c.ToString();
                        }
                    }
                    _rz10AddpName = valueToPut;
                }
                else
                    _rz10AddpName = null;

                this.OnPropertyChanged("Rz10AddpName");
            }
        }
        public string Rz10AddpValue
        {
            get { return _rz10AddpValue; }
            set
            {
                if (value == null)
                    this._rz10AddpValue = value;
                else if (value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[&+,:;=?#|'<>.^*!\\[\\]{}`°¬/¨´~¿¡])"))
                            valueToPut = valueToPut += c.ToString();
                    }
                    _rz10AddpValue = valueToPut;
                }
                else
                    _rz10AddpValue = null;
                this.OnPropertyChanged("Rz10AddpValue");
            }
        }
        public ObservableCollection<Rz10FqicpSettingsConfiguration> FqicpList
        {
            get { return this._fqicpList; }
            set
            {
                this._fqicpList = value;
                this.OnPropertyChanged("FqicpList");
            }
        }
        public ObservableCollection<Rz10AddpSettingsConfiguration> AddpList
        {
            get { return this._addpList; }
            set
            {
                this._addpList = value;
                this.OnPropertyChanged("AddpList");
            }
        }
        /*public bool ShowFqicpList
        {
            get
            {
                if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLPOSTACTIVITIES"))
                    return true;
                else
                    return false;
            }
        }
        public bool ShowAddpList
        {
            get
            {
                if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLPOSTACTIVITIES"))
                    return true;
                else
                    return false;
            }
        }*/
        public void AddRz10Fqicp(object obj)
        {
            FqicpList.Add(new Rz10FqicpSettingsConfiguration { FqicpName = Rz10FqicpName, FqicpValue = Rz10FqicpValue, IsSelected = false });
            //_rz10FqicpName = null;
            Rz10FqicpName = null;
            //_rz10FqicpValue = null;
            Rz10FqicpValue = null;
        }
        public bool CanAddRz10Fqicp(object obj)
        {
            if (Rz10FqicpName != null && Rz10FqicpValue != null)
                return true;
            else
                return false;
        }
        public void AddRz10Addp(object obj)
        {
            AddpList.Add(new Rz10AddpSettingsConfiguration { AddpName = Rz10AddpName, AddpValue = Rz10AddpValue, IsSelected = false });
            _rz10AddpName = null;
            Rz10AddpName = null;
            _rz10AddpValue = null;
            Rz10AddpValue = null;
        }
        public bool CanAddRz10Addp(object obj)
        {
            if (Rz10AddpName != null && Rz10AddpValue != null)
                return true;
            else
                return false;
        }
        public void RemoveRz10Fqicp(object obj)
        {
            foreach (Rz10FqicpSettingsConfiguration item in FqicpList.Where(o => o.IsSelected).ToList())
            {
                FqicpList.Remove(item);
            }
        }
        public bool CanRemoveRz10Fqicp(object obj)
        {
            if (FqicpList.Where(o => o.IsSelected).ToList().Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void RemoveRz10Addp(object obj)
        {
            foreach (Rz10AddpSettingsConfiguration item in AddpList.Where(o => o.IsSelected).ToList())
            {
                AddpList.Remove(item);
            }
        }
        public bool CanRemoveRz10Addp(object obj)
        {
            if (AddpList.Where(o => o.IsSelected).ToList().Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region SMLG configuration
        public string SmlgCustomerName
        {
            get { return _smlgCustomerName; }
            set
            {
                if (value == null)
                    _smlgCustomerName = value;
                else if (value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                            valueToPut = valueToPut += c.ToString();
                    }
                    _smlgCustomerName = valueToPut;
                }
                else
                    _smlgCustomerName = null;

                this.OnPropertyChanged("SmlgCustomerName");
            }
        }
        public string SmlgInstanceGroup
        {
            get { return _smlgInstanceGroup; }
            set
            {
                if (value == null)
                    _smlgInstanceGroup = value;
                else if (value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                            valueToPut = valueToPut += c.ToString();
                    }
                    _smlgInstanceGroup = valueToPut;
                }
                else
                    _smlgInstanceGroup = null;

                this.OnPropertyChanged("SmlgInstanceGroup");
            }
        }
        public string SmlgIpGroup
        {
            get { return _smlgIpGroup; }
            set
            {
                _smlgIpGroup = value;
                this.OnPropertyChanged("SmlgIpGroup");

                if (value != null)
                {
                    string[] ipArray;
                    ipArray = value.Split('.');

                    if (ipArray.Count() == 4 && ipArray[3] != "")
                    {
                        IPAddress ip;
                        bool validateIP = IPAddress.TryParse(value, out ip);
                        if (!validateIP)
                        {
                            MessageBox.Show("This is not a valid ip address, please try again.");
                            checkIp = false;
                        }
                        else
                            checkIp = true;
                    }
                }
            }
        }
        public bool SmlgRfcEnabled
        {
            get { return _smlgRfcEnabled; }
            set
            {
                _smlgRfcEnabled = value;
                this.OnPropertyChanged("SmlgRfcEnabled");
            }
        }
        public string SmlgRfcType
        {
            get { return _smlgRfcType; }
            set
            {
                if( value == null)
                    _smlgRfcType = "R - Round Robin";
                else
                    _smlgRfcType = value;

                this.OnPropertyChanged("SmlgRfcType");
            }
        }
        public ObservableCollection<SmlgSettingsConfiguration> SmlgList
        {
            get { return this._smlgList; }
            set
            {
                this._smlgList = value;
                this.OnPropertyChanged("SmlgList");
            }
        }
        public bool ShowSmlgList
        {
            get
            {
                if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLPOSTACTIVITIES") || SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLPOST"))
                    return true;
                else
                    return false;
            }
        }
        public void AddSmlg(object obj)
        {
            SmlgList.Add(new SmlgSettingsConfiguration { CustomerName = _smlgCustomerName, InstanceGroup = _smlgInstanceGroup, IpGroup = _smlgIpGroup, RfcEnabled = _smlgRfcEnabled, RfcType = _smlgRfcType, IsSelected = false });
            SmlgCustomerName = null;
            SmlgInstanceGroup = null;
            SmlgIpGroup = null;
            SmlgRfcEnabled = true;
            SmlgRfcType = null;
        }
        public bool CanAddSmlg(object obj)
        {
            if (_smlgCustomerName != null && _smlgInstanceGroup != null && checkIp && _smlgRfcType != null)
                return true;
            else
                return false;
        }
        public void RemoveSmlg(object obj)
        {
            foreach (SmlgSettingsConfiguration item in SmlgList.Where(o => o.IsSelected).ToList())
            {
                SmlgList.Remove(item);
            }
        }
        public bool CanRemoveSmlg(object obj)
        {
            if (SmlgList.Where(o => o.IsSelected).ToList().Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region SM36 configuration
        public string Sm36SapUser
        {
            get { return _sm36SapUser; }
            set
            {
                if (value == null)
                    _sm36SapUser = value;
                else if (value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                            valueToPut = valueToPut += c.ToString();
                    }
                    _sm36SapUser = valueToPut;
                }
                else
                    _sm36SapUser = null;

                this.OnPropertyChanged("Sm36SapUser");
            }
        }
        public string Sm36SapPassword
        {
            get { return _sm36SapPassword; }
            set
            {
                _sm36SapPassword = value;
                this.OnPropertyChanged("Sm36SapPassword");
            }
        }
        #endregion

        #region RZ12 configuration
        public string Rz12GroupName
        {
            get { return _rz12GroupName; }
            set
            {
                if(value == null)
                    _rz12GroupName = value;
                else if(value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                        {
                            if (!Regex.IsMatch(c.ToString(), " "))
                                valueToPut = valueToPut += c.ToString();
                        }
                    }
                    _rz12GroupName = valueToPut;
                }
                else
                    _rz12GroupName = null;
                this.OnPropertyChanged("Rz12GroupName");
            }
        }
        public string Rz12InstanceGroup
        {
            get { return _rz12InstanceGroup; }
            set
            {
                if(value == null)
                    _rz12InstanceGroup = value;
                else if(value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!\\[\\]{}`°¬/¨´~¿¡-])"))
                            valueToPut = valueToPut += c.ToString();
                    }
                    _rz12InstanceGroup = valueToPut;
                }
                else
                    _rz12InstanceGroup = null;
                this.OnPropertyChanged("Rz12InstanceGroup");
            }
        }
        public bool Rz12Activated
        {
            get { return _rz12Activated; }
            set
            {
                _rz12Activated = value;
                this.OnPropertyChanged("Rz12Activated");
            }
        }
        public int Rz12MaxQueue
        {
            get { return _rz12MaxQueue; }
            set
            {
                if(value.ToString() != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value.ToString());
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                        {
                            if (!Regex.IsMatch(c.ToString(), " "))
                                valueToPut = valueToPut += c.ToString();
                        }
                    }
                    _rz12MaxQueue = Int32.Parse(valueToPut);
                    this.OnPropertyChanged("Rz12MaxQueue");
                    checkMaxQ = true;
                }
                else
                    checkMaxQ = false;
            }
        }
        public int Rz12MaxLogin
        {
            get { return _rz12MaxLogin; }
            set
            {
                if (value.ToString() != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value.ToString());
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                        {
                            if (!Regex.IsMatch(c.ToString(), " "))
                                valueToPut = valueToPut += c.ToString();
                        }
                    }
                    _rz12MaxLogin = Int32.Parse(valueToPut);
                    this.OnPropertyChanged("Rz12MaxLogin");
                    checkMaxL = true;
                }
                else
                    checkMaxL = false;
            }
        }
        public int Rz12MaxSeparateLogons
        {
            get { return _rz12MaxSeparateLogons; }
            set
            {
                if (value.ToString() != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value.ToString());
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                        {
                            if (!Regex.IsMatch(c.ToString(), " "))
                                valueToPut = valueToPut += c.ToString();
                        }
                    }
                    _rz12MaxSeparateLogons = Int32.Parse(valueToPut);
                    this.OnPropertyChanged("Rz12MaxSeparateLogons");
                    checkMaxSL = true;
                }
                else
                    checkMaxSL = false;
            }
        }
        public int Rz12Maxwp
        {
            get { return _rz12Maxwp; }
            set
            {
                if (value.ToString() != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value.ToString());
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                        {
                            if (!Regex.IsMatch(c.ToString(), " "))
                                valueToPut = valueToPut += c.ToString();
                        }
                    }
                    _rz12Maxwp = Int32.Parse(valueToPut);
                    this.OnPropertyChanged("Rz12Maxwp");
                    checkMaxWp = true;
                }
                else
                    checkMaxWp = false;
            }
        }
        public int Rz12Minfreewp
        {
            get { return _rz12Minfreewp; }
            set
            {
                if (value.ToString() != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value.ToString());
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                        {
                            if (!Regex.IsMatch(c.ToString(), " "))
                                valueToPut = valueToPut += c.ToString();
                        }
                    }
                    _rz12Minfreewp = Int32.Parse(valueToPut);
                    this.OnPropertyChanged("Rz12Minfreewp");
                    checkMinF = true;
                }
                else
                    checkMinF = false;
            }
        }
        public int Rz12Maxcomm
        {
            get { return _rz12Maxcomm; }
            set
            {
                if (value.ToString() != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value.ToString());
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                        {
                            if (!Regex.IsMatch(c.ToString(), " "))
                                valueToPut = valueToPut += c.ToString();
                        }
                    }
                    _rz12Maxcomm = Int32.Parse(valueToPut);
                    this.OnPropertyChanged("Rz12Maxcomm");
                    checkMaxcomm = true;
                }
                else
                    checkMaxcomm = false;
            }
        }
        public int Rz12MaxWaitTime
        {
            get { return _rz12MaxWaitTime; }
            set
            {
                if (value.ToString() != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value.ToString());
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                        {
                            if (!Regex.IsMatch(c.ToString(), " "))
                                valueToPut = valueToPut += c.ToString();
                        }
                    }
                    _rz12MaxWaitTime = Int32.Parse(valueToPut);
                    this.OnPropertyChanged("Rz12MaxWaitTime");
                    checkMaxWT = true;
                }
                else
                    checkMaxWT = false;
            }
        }
        public ObservableCollection<Rz12SettingsConfiguration> Rz12List
        {
            get { return this._rz12List; }
            set
            {
                this._rz12List = value;
                this.OnPropertyChanged("Rz12List");
            }
        }
        public void AddRz12(object obj)
        {
            Rz12List.Add(new Rz12SettingsConfiguration { GroupName = _rz12GroupName, InstanceGroup = _rz12InstanceGroup, Activated = _rz12Activated, MaxQueue = _rz12MaxQueue, MaxLogin = _rz12MaxLogin, MaxSeparateLogons = _rz12MaxSeparateLogons, Maxwp = _rz12Maxwp, Minfreewp = _rz12Minfreewp, Maxcomm = _rz12Maxcomm, MaxWaitTime = _rz12MaxWaitTime, IsSelected = false });
            Rz12GroupName = null;
            Rz12InstanceGroup = null;
            Rz12Activated = true;
            Rz12MaxQueue = 5;
            Rz12MaxLogin = 90;
            Rz12MaxSeparateLogons = 25;
            Rz12Maxwp = 75;
            Rz12Minfreewp = 1;
            Rz12Maxcomm = 90;
            Rz12MaxWaitTime = 15;
        }
        public bool CanAddRz12(object obj)
        {
            if (_rz12GroupName != null && _rz12InstanceGroup != null && checkMaxQ && checkMaxL && checkMaxSL && checkMaxWp && checkMinF && checkMaxcomm && checkMaxWT)
                return true;
            else
                return false;
        }
        public void RemoveRz12(object obj)
        {
            foreach (Rz12SettingsConfiguration item in Rz12List.Where(o => o.IsSelected).ToList())
            {
                Rz12List.Remove(item);
            }
        }
        public bool CanRemoveRz12(object obj)
        {
            if (Rz12List.Where(o => o.IsSelected).ToList().Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region SM61 configuration
        public string Sm61GroupName
        {
            get { return _sm61GroupName; }
            set
            {
                if (value == null)
                    _sm61GroupName = value;
                else if (value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                            valueToPut = valueToPut += c.ToString();
                    }
                    _sm61GroupName = valueToPut;
                }
                else
                    _sm61GroupName = null;

                this.OnPropertyChanged("Sm61GroupName");
            }
        }
        public string Sm61Instance
        {
            get { return _sm61Instance; }
            set
            {
                if (value == null)
                    _sm61Instance = value;
                else if (value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                            valueToPut = valueToPut += c.ToString();
                    }
                    _sm61Instance = valueToPut;
                }
                else
                    _sm61Instance = null;

                this.OnPropertyChanged("Sm61Instance");
            }
        }
        public ObservableCollection<Sm61SettingsConfiguration> Sm61List
        {
            get { return this._sm61List; }
            set
            {
                this._sm61List = value;
                this.OnPropertyChanged("Sm61List");
            }
        }
        public void AddSm61(object obj)
        {
            Sm61List.Add(new Sm61SettingsConfiguration { GroupName = _sm61GroupName, Instance = _sm61Instance, IsSelected = false });
            Sm61GroupName = null;
            Sm61Instance = null;
        }
        public bool CanAddSm61(object obj)
        {
            if (_sm61GroupName != null && _sm61Instance != null)
                return true;
            else
                return false;
        }
        public void RemoveSm61(object obj)
        {
            foreach (Sm61SettingsConfiguration item in Sm61List.Where(o => o.IsSelected).ToList())
            {
                Sm61List.Remove(item);
            }
        }
        public bool CanRemoveSm61(object obj)
        {
            if (Sm61List.Where(o => o.IsSelected).ToList().Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        
        #region RZ04 configuration
        public string Rz04OperationName
        {
            get { return _rz04OperationName; }
            set
            {
                if (value == null)
                    _rz04OperationName = value;
                else if (value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!\\[\\]{}`°¬/¨´~¿¡])"))
                        {
                            if (!Regex.IsMatch(c.ToString(), " "))
                                valueToPut = valueToPut += c.ToString();
                        }
                    }

                    _rz04OperationName = valueToPut;
                }
                else
                    _rz04OperationName = null;
                
                this.OnPropertyChanged("Rz04OperationName");
            }
        }
        public string Rz04Description
        {
            get { return _rz04Description; }
            set
            {
                if (value == null)
                    _rz04Description = value;
                else if (value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                            valueToPut = valueToPut += c.ToString();
                    }

                    _rz04Description = valueToPut;
                }
                else
                    _rz04Description = null;

                this.OnPropertyChanged("Rz04Description");
            }
        }
        public int Rz04InTimeHour
        {
            get { return _rz04InTimeHour; }
            set
            {
                _rz04InTimeHour = value;
                this.OnPropertyChanged("Rz04InTimeHour");
            }
        }
        public int Rz04InTimeMinute
        {
            get { return _rz04InTimeMinute; }
            set
            {
                _rz04InTimeMinute = value;
                this.OnPropertyChanged("Rz04InTimeMinute");
            }
        }
        public int Rz04EndTimeHour
        {
            get { return _rz04EndTimeHour; }
            set
            {
                _rz04EndTimeHour = value;
                this.OnPropertyChanged("Rz04EndTimeHour");
            }
        }
        public int Rz04EndTimeMinute
        {
            get { return _rz04EndTimeMinute; }
            set
            {
                _rz04EndTimeMinute = value;
                this.OnPropertyChanged("Rz04EndTimeMinute");
            }
        }
        public ObservableCollection<Rz04SettingsConfiguration> Rz04List
        {
            get { return this._rz04List; }
            set
            {
                this._rz04List = value;
                this.OnPropertyChanged("Rz04List");
            }
        }
        public void AddRz04(object obj)
        {
            Rz04List.Add(new Rz04SettingsConfiguration { OperationName = _rz04OperationName, Description = _rz04Description, InTimeHour = _rz04InTimeHour, InTimeMinute = _rz04InTimeMinute, EndTimeHour = _rz04EndTimeHour, EndTimeMinute = _rz04EndTimeMinute, IsSelected = false });
            Rz04OperationName = null;
            Rz04Description = null;
            Rz04InTimeHour = 00;
            Rz04InTimeMinute = 00;
            Rz04EndTimeHour = 00;
            Rz04EndTimeMinute = 00;
        }
        public bool CanAddRz04(object obj)
        {
            if (_rz04OperationName != null && _rz04Description != null)
                return true;
            else
                return false;
        }
        public void RemoveRz04(object obj)
        {
            foreach (Rz04SettingsConfiguration item in Rz04List.Where(o => o.IsSelected).ToList())
            {
                Rz04List.Remove(item);
            }
        }
        public bool CanRemoveRz04(object obj)
        {
            if (Rz04List.Where(o => o.IsSelected).ToList().Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region RZ70 configuration
        public string Rz70GatewayHost
        {
            get { return _rz70GatewayHost; }
            set
            {
                if (value == null)
                    _rz70GatewayHost = value;
                else if (value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                        {
                            if (!Regex.IsMatch(c.ToString(), " "))
                                valueToPut = valueToPut += c.ToString();
                        }
                    }
                    _rz70GatewayHost = valueToPut;
                }
                else
                    _rz70GatewayHost = null;
                this.OnPropertyChanged("Rz70GatewayHost");
            }
        }
        public string Rz70GatewayService
        {
            get { return _rz70GatewayService; }
            set
            {
                if (value == null)
                    _rz70GatewayService = "sapgw";
                else if (value != "")
                {
                    if (value.Length >= 5)
                    {
                        if (value.Contains("sapgw"))
                        {
                            List<char> chars = new List<char>();

                            value = value.Remove(0, 5);
                            chars.AddRange(value);
                            string valueToPut = "";

                            foreach (char c in chars)
                            {
                                if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                                {
                                    if (!Regex.IsMatch(c.ToString(), " "))
                                        valueToPut = valueToPut += c.ToString();
                                }
                            }
                            _rz70GatewayService = "sapgw" + valueToPut;
                        }

                        else
                        {
                            MessageBox.Show("The gateway service needs to start with sapgw, please try again.");
                            _rz70GatewayService = "sapgw";
                        }
                    }
                    else
                    {
                        MessageBox.Show("The gateway service needs to start with sapgw, please try again.");
                        _rz70GatewayService = "sapgw";
                    }
                }
                else
                    _rz70GatewayService = "sapgw";
                this.OnPropertyChanged("Rz70GatewayService");
            }
        }
        public ObservableCollection<Rz70SettingsConfiguration> Rz70List
        {
            get { return this._rz70List; }
            set
            {
                this._rz70List = value;
                this.OnPropertyChanged("Rz70List");
            }
        }
        public void AddRz70(object obj)
        {
            Rz70List.Add(new Rz70SettingsConfiguration { GatewayHost = _rz70GatewayHost, GatewayService = _rz70GatewayService, IsSelected = false });
            Rz70GatewayHost = null;
            Rz70GatewayService = null;
        }
        public bool CanAddRz70(object obj)
        {
            if (_rz70GatewayHost != null && _rz70GatewayService != null && _rz70GatewayService.Length == 7)
                return true;
            else
                return false;
        }
        public void RemoveRz70(object obj)
        {
            foreach (Rz70SettingsConfiguration item in Rz70List.Where(o => o.IsSelected).ToList())
            {
                Rz70List.Remove(item);
            }
        }
        public bool CanRemoveRz70(object obj)
        {
            if (Rz70List.Where(o => o.IsSelected).ToList().Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region AL11 configuration
        public string Al11DirectoryPath
        {
            get { return _al11DirectoryPath; }
            set
            {
                if(value == null)
                    _al11DirectoryPath = value;
                if (value != "")
                    _al11DirectoryPath = value;
                else
                    _al11DirectoryPath = null;
                this.OnPropertyChanged("Al11DirectoryPath");
            }
        }
        public string Al11DirectoryName
        {
            get { return _al11DirectoryName; }
            set
            {
                if (value == null)
                    _al11DirectoryName = value;
                else if (value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+:;=?@#|'<>,^*()%!\\[\\]{}`°¬/¨´~¿¡])"))
                            valueToPut = valueToPut += c.ToString();
                    }

                    _al11DirectoryName = valueToPut;
                }
                else
                    _al11DirectoryName = null;
                this.OnPropertyChanged("Al11DirectoryName");
            }
        }
        public string Al11ValidForServer
        {
            get { return _al11ValidForServer; }
            set
            {
                if (value == null)
                    _al11ValidForServer = value;
                if (value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+:;=?@#|'<>,^*()%!\\[\\]{}`°¬/¨´~¿¡])"))
                            valueToPut = valueToPut += c.ToString();
                    }
                    _al11ValidForServer = valueToPut;
                }
                else
                    _al11ValidForServer = null;
                this.OnPropertyChanged("Al11ValidForServer");
            }
        }
        public ObservableCollection<Al11SettingsConfiguration> Al11List
        {
            get { return this._al11List; }
            set
            {
                this._al11List = value;
                this.OnPropertyChanged("Al11List");
            }
        }
        public void AddAl11(object obj)
        {
            Al11List.Add(new Al11SettingsConfiguration { DirectoryPath = _al11DirectoryPath, DirectoryName = _al11DirectoryName, ValidForServer = _al11ValidForServer, IsSelected = false });
            Al11DirectoryPath = null;
            Al11DirectoryName = null;
            Al11ValidForServer = "all";
        }
        public bool CanAddAl11(object obj)
        {
            if (_al11DirectoryPath != null && _al11DirectoryName != null && _al11ValidForServer != null)
            {
                if (_al11DirectoryName.Length > 2 && _al11DirectoryPath.Length > 1)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public void RemoveAl11(object obj)
        {
            foreach (Al11SettingsConfiguration item in Al11List.Where(o => o.IsSelected).ToList())
            {
                Al11List.Remove(item);
            }
        }
        public bool CanRemoveAl11(object obj)
        {
            if (Al11List.Where(o => o.IsSelected).ToList().Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region STRUST02 configuration
        public string Strust02CertificateType
        {
            get { return _strust02CertificateType; }
            set
            {
                _strust02CertificateType = value;
                this.OnPropertyChanged("Strust02CertificateType");
            }
        }
        public string Strust02CertificatePath
        {
            get { return SapInstallSTRUST02.fullCertificateName; }
            set
            {
                _strust02CertificatePath = SapInstallSTRUST02.fullCertificateName;
                this.OnPropertyChanged("Strust02CertificatePath");
            }
        }
        public string Strust02CertificateName
        {
            get { return SapInstallSTRUST02.fileCertificateName; }
            set
            {
                _strust02CertificateName = SapInstallSTRUST02.fileCertificateName;
                this.OnPropertyChanged("Strust02CertificateName");
            }
        }
        public ObservableCollection<Strust02SettingsConfiguration> Strust02List
        {
            get { return this._strust02List; }
            set
            {
                this._strust02List = value;
                this.OnPropertyChanged("Strust02List");
            }
        }
        public void AddStrust02(object obj)
        {
            Strust02List.Add(new Strust02SettingsConfiguration { CertificateType = _strust02CertificateType, CertificatePath = Strust02CertificatePath, CertificateName = Strust02CertificateName, IsSelected = false });
            Strust02CertificateType = null;
            Strust02CertificatePath = null;
            //SapInstallSTRUST02.fullCertificateName = null;
        }
        public bool CanAddStrust02(object obj)
        {
            if (_strust02CertificateType != null && Strust02CertificatePath != null)
                return true;
            else
                return false;
        }
        public void RemoveStrust02(object obj)
        {
            foreach (Strust02SettingsConfiguration item in Strust02List.Where(o => o.IsSelected).ToList())
            {
                Strust02List.Remove(item);
            }
        }
        public bool CanRemoveStrust02(object obj)
        {
            if (Strust02List.Where(o => o.IsSelected).ToList().Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region SCC4 configuration
        public int? Scc4Client
        {
            get { return _scc4Client; }
            set
            {
                _scc4Client = value;
                this.OnPropertyChanged("Scc4Client");
            }
        }
        public string Scc4ClientName
        {
            get { return _scc4ClientName; }
            set
            {
                if (value == null)
                    _scc4ClientName = value;
                else if (value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                            valueToPut = valueToPut += c.ToString();
                    }
                    _scc4ClientName = valueToPut;
                }
                else
                    _scc4ClientName = null;
                this.OnPropertyChanged("Scc4ClientName");
            }
        }
        public string Scc4ClientCity
        {
            get { return _scc4ClientCity; }
            set
            {
                if (value == null)
                    _scc4ClientCity = value;
                else if (value != "")
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(value);
                    string valueToPut = "";

                    foreach (char c in chars)
                    {
                        if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                        {
                            if (!Regex.IsMatch(c.ToString(), " "))
                                valueToPut = valueToPut += c.ToString();
                        }
                    }
                    _scc4ClientCity = valueToPut;
                }
                else
                    _scc4ClientCity = null;
                this.OnPropertyChanged("Scc4ClientCity");
            }
        }
        public string Scc4LogicalName
        {
            get { return _scc4LogicalName; }
            set
            {
                string requiredText = SelectedTargetSAPServer.SID + "CLNT";
                
                if (value == null || value == "")
                    _scc4LogicalName = requiredText;
                else
                {
                    if (value.Length >= 7)
                    {
                        if (value.Contains(requiredText))
                        {
                            List<char> chars = new List<char>();

                            value = value.Remove(0, 7);
                            chars.AddRange(value);
                            string valueToPut = "";

                            foreach (char c in chars)
                            {
                                if (!Regex.IsMatch(c.ToString(), "^(?=.*[$&+,:;=?@#|'<>.^*()%!_\\[\\]{}`°¬/¨´~¿¡-])"))
                                {
                                    if (!Regex.IsMatch(c.ToString(), " "))
                                    {
                                        if (!Regex.IsMatch(c.ToString(), "^[a-zA-Z]"))
                                            valueToPut = valueToPut += c.ToString();
                                    }
                                }
                            }
                            _scc4LogicalName = requiredText + valueToPut;
                        }

                        else
                        {
                            MessageBox.Show("The gateway service needs to start with {0}, please try again.", requiredText);
                            _scc4LogicalName = requiredText;
                        }
                    }
                    else
                    {
                        MessageBox.Show("The gateway service needs to start with {0}, please try again.", requiredText);
                        _scc4LogicalName = requiredText;
                    }
                }
                this.OnPropertyChanged("Scc4LogicalName");
            }
        }
        public string Scc4Currency
        {
            get { return _scc4Currency; }
            set
            {
                _scc4Currency = value;
                this.OnPropertyChanged("Scc4Currency");
            }
        }
        public string Scc4ClientRole
        {
            get { return _scc4ClientRole; }
            set
            {
                _scc4ClientRole = value;
                this.OnPropertyChanged("Scc4ClientRole");
            }
        }
        public string Scc4ChangesAndTransport
        {
            get { return _scc4ChangesAndTransport; }
            set
            {
                _scc4ChangesAndTransport = value;
                this.OnPropertyChanged("Scc4ChangesAndTransport");
            }
        }
        public string Scc4CrossClient
        {
            get { return _scc4CrossClient; }
            set
            {
                _scc4CrossClient = value;
                this.OnPropertyChanged("Scc4CrossClient");
            }
        }
        public string Scc4CopyComparisonTool
        {
            get { return _scc4CopyComparisonTool; }
            set
            {
                _scc4CopyComparisonTool = value;
                this.OnPropertyChanged("Scc4CopyComparisonTool");
            }
        }
        public string Scc4CattAndEcattRest
        {
            get { return _scc4CattAndEcattRest; }
            set
            {
                _scc4CattAndEcattRest = value;
                this.OnPropertyChanged("Scc4CattAndEcattRest");
            }
        }
        public ObservableCollection<Scc4SettingsConfiguration> Scc4List
        {
            get { return this._scc4List; }
            set
            {
                this._scc4List = value;
                this.OnPropertyChanged("Scc4List");
            }
        }
        public void AddScc4(object obj)
        {
            Scc4List.Add(new Scc4SettingsConfiguration { Client = _scc4Client, ClientName = _scc4ClientName, ClientCity = _scc4ClientCity, LogicalName = _scc4LogicalName, Currency = _scc4Currency, ClientRole = _scc4ClientRole, ChangesAndTransport = _scc4ChangesAndTransport, CrossClient = _scc4CrossClient, CopyComparisonTool = _scc4CopyComparisonTool, CattAndEcattRest = _scc4CattAndEcattRest, IsSelected = false });
            Scc4Client = null;
            Scc4ClientName = null;
            Scc4ClientCity = null;
            Scc4LogicalName = null;
            Scc4Currency = null;
            Scc4ClientRole = null;
            Scc4ChangesAndTransport = null;
            Scc4CrossClient = null;
            Scc4CopyComparisonTool = null;
            Scc4CattAndEcattRest = null;
        }
        public bool CanAddScc4(object obj)
        {
            if (_scc4Client != null && _scc4ClientName != null && _scc4ClientCity != null && _scc4LogicalName != null && _scc4LogicalName.Length == 10  && _scc4Currency != null && _scc4ClientRole != null && _scc4ChangesAndTransport != null && _scc4CrossClient != null && _scc4CopyComparisonTool != null && Scc4CattAndEcattRest != null)
                return true;
            else
                return false;
        }
        public void RemoveScc4(object obj)
        {
            foreach (Scc4SettingsConfiguration item in Scc4List.Where(o => o.IsSelected).ToList())
            {
                Scc4List.Remove(item);
            }
        }
        public bool CanRemoveScc4(object obj)
        {
            if (Scc4List.Where(o => o.IsSelected).ToList().Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region SM21 configuration
        public int Sm21FromHour
        {
            get { return _sm21FromHour; }
            set
            {
                _sm21FromHour = value;
                this.OnPropertyChanged("Sm21FromHour");
            }
        }
        public int Sm21FromMinute
        {
            get { return _sm21FromMinute; }
            set
            {
                _sm21FromMinute = value;
                this.OnPropertyChanged("Sm21FromMinute");
            }
        }
        public DateTime Sm21FromDate
        {
            get { return _sm21FromDate; }
            set
            {
                _sm21FromDate = value;
                this.OnPropertyChanged("Sm21FromDate");
            }
        }
        public DateTime Sm21FromDateComplete
        {
            get { return new DateTime(_sm21FromDate.Year, _sm21FromDate.Month, _sm21FromDate.Day, Sm21FromHour, Sm21FromMinute, 0); }
            
        }
        public int Sm21ToHour
        {
            get { return _sm21ToHour; }
            set
            {
                _sm21ToHour = value;
                this.OnPropertyChanged("Sm21ToHour");
            }
        }
        public int Sm21ToMinute
        {
            get { return _sm21ToMinute; }
            set
            {
                _sm21ToMinute = value;
                this.OnPropertyChanged("Sm21ToMinute");
            }
        }
        public DateTime Sm21ToDate
        {
            get { return _sm21ToDate; }
            set
            {
                _sm21ToDate = value;
                this.OnPropertyChanged("Sm21ToDate");
            }
        }
        public DateTime Sm21ToDateComplete
        {
            get { return new DateTime(_sm21ToDate.Year, _sm21ToDate.Month, _sm21ToDate.Day, Sm21ToHour, Sm21ToMinute, 0); }

        }
        #endregion

        #region ST22 configuration
        public int St22FromHour
        {
            get { return _st22FromHour; }
            set
            {
                _st22FromHour = value;
                this.OnPropertyChanged("St22FromHour");
            }
        }
        public int St22FromMinute
        {
            get { return _st22FromMinute; }
            set
            {
                _st22FromMinute = value;
                this.OnPropertyChanged("St22FromMinute");
            }
        }
        public DateTime St22FromDate
        {
            get { return _st22FromDate; }
            set
            {
                _st22FromDate = value;
                this.OnPropertyChanged("St22FromDate");
            }
        }
        public DateTime St22FromDateComplete
        {
            get { return new DateTime(_st22FromDate.Year, _st22FromDate.Month, _st22FromDate.Day, St22FromHour, St22FromMinute, 0); }

        }
        public int St22ToHour
        {
            get { return _st22ToHour; }
            set
            {
                _st22ToHour = value;
                this.OnPropertyChanged("St22ToHour");
            }
        }
        public int St22ToMinute
        {
            get { return _st22ToMinute; }
            set
            {
                _st22ToMinute = value;
                this.OnPropertyChanged("St22ToMinute");
            }
        }
        public DateTime St22ToDate
        {
            get { return _st22ToDate; }
            set
            {
                _st22ToDate = value;
                this.OnPropertyChanged("St22ToDate");
            }
        }
        public DateTime St22ToDateComplete
        {
            get { return new DateTime(_st22ToDate.Year, _st22ToDate.Month, _st22ToDate.Day, St22ToHour, St22ToMinute, 0); }

        }
        public string St22User
        {
            get { return _st22User; }
            set
            {
                _st22User = value;
                this.OnPropertyChanged("St22User");
            }
        }
        #endregion

        #region DB13 configuration
        public string Db13Job
        {
            get { return _db13Job; }
            set
            {
                _db13Job = value;
                this.OnPropertyChanged("Db13Job");
            }
        }
        public string Db13Recurrence
        {
            get { return _db13Recurrence; }
            set
            {
                _db13Recurrence = value;
                this.OnPropertyChanged("Db13Recurrence");
            }
        }
        public string Db13Range
        {
            get { return _db13Range; }
            set
            {
                _db13Range = value;
                this.OnPropertyChanged("Db13Range");
            }
        }
        public string Db13RecurrenceDay
        {
            get { return _db13RecurrenceDay; }
            set
            {
                _db13RecurrenceDay = value;
                this.OnPropertyChanged("Db13RecurrenceDay");
            }
        }
        public string Db13RangeNoEnd
        {
            get { return _db13RangeNoEnd; }
            set
            {
                _db13RangeNoEnd = value;
                this.OnPropertyChanged("Db13RangeNoEnd");
            }
        }
        public string Db13RangeEndAfter
        {
            get { return _db13RangeEndAfter; }
            set
            {
                _db13RangeEndAfter = value;
                this.OnPropertyChanged("Db13RangeEndAfter");
            }
        }
        public int Db13StartDateHour
        {
            get { return _db13StartDateHour; }
            set
            {
                _db13StartDateHour = value;
                TimeSpan ts = new TimeSpan(_db13StartDateHour, 0, 0);
                TimeSpan tsHours = new TimeSpan(Db13StartDate.Hour, 0, 0);
                Db13StartDate = Db13StartDate - tsHours + ts;
                this.OnPropertyChanged("Db13StartDateHour");
            }
        }
        public int Db13StartDateMinute
        {
            get { return _db13StartDateMinute; }
            set
            {
                _db13StartDateMinute = value;
                TimeSpan ts = new TimeSpan(0, _db13StartDateMinute, 0);
                TimeSpan tsMinutes = new TimeSpan(0, Db13StartDate.Minute, 0);
                Db13StartDate = Db13StartDate - tsMinutes + ts;
                this.OnPropertyChanged("Db13StartDateMinute");
            }
        }
        public int Db13RecurrenceDayHour
        {
            get { return _db13RecurrenceDayHour; }
            set
            {
                _db13RecurrenceDayHour = value;
                TimeSpan ts = new TimeSpan(_db13RecurrenceDayHour, 0, 0);
                TimeSpan tsHours = new TimeSpan(Db13RecurrenceDayTimeSpan.Hours, 0, 0);
                Db13RecurrenceDayTimeSpan = Db13RecurrenceDayTimeSpan - tsHours + ts;
                this.OnPropertyChanged("Db13RecurrenceDayHour");
            }
        }
        public int Db13RecurrenceDayMinute
        {
            get { return _db13RecurrenceDayMinute; }
            set
            {
                _db13RecurrenceDayMinute = value;
                TimeSpan ts = new TimeSpan(0, _db13RecurrenceDayMinute, 0);
                TimeSpan tsMinutes = new TimeSpan(0, Db13RecurrenceDayTimeSpan.Minutes, 0);
                Db13RecurrenceDayTimeSpan = Db13RecurrenceDayTimeSpan - tsMinutes + ts;
                this.OnPropertyChanged("Db13RecurrenceDayMinute");
            }
        }
        public TimeSpan Db13RecurrenceDayTimeSpan
        {
            get { return _db13RecurrenceDayTimeSpan; }
            set
            {
                _db13RecurrenceDayTimeSpan = value;
                this.OnPropertyChanged("Db13RecurrenceDayTimeSpan");
            }
        }
        public String Db13RecurrenceDayTimeSpanString
        {
            get { return string.Format("{0}:{1}", Db13RecurrenceDayTimeSpan.Hours, Db13RecurrenceDayTimeSpan.Minutes); }
        }
        public int Db13RecurrenceHour
        {
            get { return _db13RecurrenceHour; }
            set
            {
                _db13RecurrenceHour = value;
                this.OnPropertyChanged("Db13RecurrenceHour");
            }
        }
        public int Db13RecurrenceOnceHour
        {
            get { return _db13RecurrenceOnceHour; }
            set
            {
                _db13RecurrenceOnceHour = value;
                this.OnPropertyChanged("Db13RecurrenceOnceHour");
            }
        }
        public int Db13RecurrenceOnceMinute
        {
            get { return _db13RecurrenceOnceMinute; }
            set
            {
                _db13RecurrenceOnceMinute = value;
                this.OnPropertyChanged("Db13RecurrenceOnceMinute");
            }
        }
        public int Db13RangeEndByhour
        {
            get { return _db13RangeEndByhour; }
            set
            {
                _db13RangeEndByhour = value;
                this.OnPropertyChanged("Db13RangeEndByhour");
            }
        }
        public int Db13RangeEndByMinute
        {
            get { return _db13RangeEndByMinute; }
            set
            {
                _db13RangeEndByMinute = value;
                this.OnPropertyChanged("Db13RangeEndByMinute");
            }
        }
        public DateTime Db13StartDate
        {
            get { return _db13StartDate; }
            set
            {
                _db13StartDate = value;
                this.OnPropertyChanged("Db13StartDate");
            }
        }
        public DateTime Db13RecurrenceOnceDate
        {
            get { return _db13RecurrenceOnceDate; }
            set
            {
                _db13RecurrenceOnceDate = value;
                this.OnPropertyChanged("Db13RecurrenceOnceDate");
            }
        }
        public DateTime Db13RangeEndByDate
        {
            get { return _db13RangeEndByDate; }
            set
            {
                _db13RangeEndByDate = value;
                this.OnPropertyChanged("Db13RangeEndByDate");
            }
        }
        public ObservableCollection<Db13SettingsConfiguration> Db13List
        {
            get { return this._db13List; }
            set
            {
                this._db13List = value;
                this.OnPropertyChanged("Db13List");
            }
        }
        public void AddDb13(object obj)
        {
            Db13List.Add(new Db13SettingsConfiguration { Job = _db13Job, Recurrence = _db13Recurrence, Range = _db13Range, RecurrenceDay = _db13RecurrenceDay, RangeNoEnd = _db13RangeNoEnd, RangeEndAfter = _db13RangeEndAfter, StartDateHour = _db13StartDateHour, StartDateMinute = _db13StartDateMinute, RecurrenceDayHour = _db13RecurrenceDayHour, RecurrenceDayMinute = _db13RecurrenceDayMinute, RecurrenceDayTimeSpan = _db13RecurrenceDayTimeSpan, RecurrenceHour = _db13RecurrenceHour, RecurrenceOnceHour = _db13RecurrenceOnceHour, RecurrenceOnceMinute = _db13RecurrenceOnceMinute, RangeEndByhour = _db13RangeEndByhour, RangeEndByMinute = _db13RangeEndByMinute, StartDate = _db13StartDate, RecurrenceOnceDate = _db13RecurrenceOnceDate, RangeEndByDate = _db13RangeEndByDate, IsSelected = false });
            Db13Job = null;
            Db13Recurrence = null;
            Db13Range = null;
            Db13RecurrenceDay = null;
            Db13RecurrenceDayHour = 0;
            Db13RecurrenceDayMinute = 0;
            Db13RecurrenceDayTimeSpan = TimeSpan.Zero;
            Db13RangeNoEnd = null;
            Db13RangeEndAfter = null;
            Db13StartDateHour = 0;
            Db13StartDateMinute = 0;
            Db13RecurrenceDayHour = 0;
            Db13RecurrenceHour = 0;
            Db13RecurrenceOnceHour = 0;
            Db13RecurrenceOnceMinute = 0;
            Db13RangeEndByhour = 0;
            Db13RangeEndByMinute = 0;
            Db13StartDate = DateTime.Today;
            Db13RecurrenceOnceDate = DateTime.Today;
            Db13RangeEndByDate = DateTime.Today;
        }
        public bool CanAddDb13(object obj)
        {
            if (_db13Job != null && _db13StartDate != null)
            {
                if (_db13Recurrence == null)
                    return false;
                if (_db13Recurrence.ToUpper() == "DAY")
                {
                    if (Db13Range == null)
                        return false;
                    else if (Db13Range.ToUpper() == "NO END DATE")
                        return true;
                    else if (Db13Range.ToUpper() == "END AFTER")
                        return true;
                    else if (Db13Range.ToUpper() == "END BY")
                        return true;
                    else
                        return false;
                }
                else if (_db13Recurrence.ToUpper() == "HOUR")
                {
                    if (Db13Range == null)
                        return false;
                    else if (Db13Range.ToUpper() == "NO END DATE")
                        return true;
                    else if (Db13Range.ToUpper() == "END AFTER")
                        return true;
                    else if (Db13Range.ToUpper() == "END BY")
                        return true;
                    else
                        return false;
                }
                else if (_db13Recurrence.ToUpper() == "WEEK")
                {
                    if (Db13Range == null)
                        return false;
                    else if (Db13Range.ToUpper() == "NO END DATE")
                        return true;
                    else if (Db13Range.ToUpper() == "END AFTER")
                    {
                        if (Db13RangeEndAfter != null)
                            return true;
                        else
                            return false;
                    }
                    else if (Db13Range.ToUpper() == "END BY")
                        return true;
                    else
                        return false;
                }
                else if (_db13Recurrence.ToUpper() == "ONCE")
                {
                    if (Db13Range == null)
                        return false;
                    else if (Db13Range.ToUpper() == "NO END DATE")
                        return true;
                    else if (Db13Range.ToUpper() == "END AFTER")
                        return true;
                    else if (Db13Range.ToUpper() == "END BY")
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public void RemoveDb13(object obj)
        {
            foreach (Db13SettingsConfiguration item in Db13List.Where(o => o.IsSelected).ToList())
            {
                Db13List.Remove(item);
            }
        }
        public bool CanRemoveDb13(object obj)
        {
            if (Db13List.Where(o => o.IsSelected).ToList().Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ShowRecurrenceAtDayList
        {
            get
            {
                if (Db13Recurrence.ToUpper() != "DAY" || Db13Recurrence == null)
                    return false;
                else
                    return true;
            }
        }
        public bool ShowRecurrenceAtWeekList
        {
            get
            {
                if (_db13Recurrence.ToUpper() == "WEEK")
                    return true;
                else
                    return false;
            }
        }
        #endregion
    }
}