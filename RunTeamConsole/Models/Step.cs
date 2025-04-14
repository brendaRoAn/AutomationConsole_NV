using Newtonsoft.Json;
using RunTeamConsole.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
namespace RunTeamConsole.Models
{
    public class Step : ObservableObject
    {
        int _index;
        private string _processName;
        private string _processTimeStamp;
        string _moduleName;
        string _description;
        string _instance;
        private string _defaultprocessauto;
        private string _flowMode;
        private int _tryNum;
        private int? _maxTries;
        private bool _processauto;
        private bool _autoinitialconf;
        private string _defaultemail;
        private bool _email;
        private bool _emailinitialconf;
        string _selectable;
        string _previousstep;
        int? _alternstep;
        private string _defaultRepeatAuto;
        private bool _repeatAutoConf;
        private bool _repeatAuto;
        private string _dateAuto;
        private string _timeAuto;
        List<Status> _statuslist;
        ExtraInputsSet _extrainputs;
        [JsonIgnore]
        public RelayCommand ChangeEmailConfigCommand { get; private set; }
        [JsonIgnore]
        public RelayCommand ChangeAutoConfigCommand { get; private set; }
        [JsonIgnore]
        public RelayCommand ChangeReplyConfigCommand { get; private set; }
        private string _transactions;
        List<TransactionsPackage.Transaction> _transactionsList;
        private string[] _evidence;
        private string _log;
        private bool _isSelected;
        private string _processSelectedFlow;

        [JsonConstructor]
        public Step(int index, string name, string description, string instance, string flow, int? maxtries, string defaultprocessauto, string defaultemail, bool processauto, bool email, string selectable, string previousstep, int? alternstep, string log, string[] evidence, string transactions, string defaultrepeatauto, string dateauto, string timeauto, int tryNum, string process, string processtimestamp, List<Status> statuslist, ExtraInputsSet extrainputs)
        {
            this._index = index;
            this._moduleName = name;
            this._description = description;
            if(flow == null)
                this._flowMode = "";
            else
                this._flowMode = flow;
            this._maxTries = maxtries;
            if (statuslist != null)
                this._statuslist = statuslist;
            else
                this._statuslist = new List<Status>();
            if(defaultprocessauto != null)
            {
                this._defaultprocessauto = defaultprocessauto;
                if (this._defaultprocessauto.ToUpper().Contains("Y"))
                   _autoinitialconf = true;
                else
                    _autoinitialconf = false;
            }
            if(defaultemail != null)
            {
                this._defaultemail = defaultemail;
                if (this._defaultemail.ToUpper().Contains("Y"))
                    _emailinitialconf = true;
                else
                    _emailinitialconf = false;
            }
            this._processauto = processauto;
            this._email = email;
            this._selectable = selectable;
            if(selectable == "must")
                this._isSelected = true;
            else
                this._isSelected = false;
            if (previousstep != null)
                this._previousstep = previousstep;
            else
                this._previousstep = "";
            if (alternstep != null)
                this._alternstep = alternstep;
            else
                this._alternstep = -1;
            this._log = log;
            if (evidence != null) 
            {
                List<string> evidenceList = new List<string>();
                foreach(string e in evidence)
                {
                    if(!String.IsNullOrEmpty(e))
                        evidenceList.Add(e);
                }
                this._evidence = evidenceList.ToArray();
            }
            else
                this._evidence = new string[] { };
            this._transactions = transactions;

            if (defaultrepeatauto != null)
            {
                this._defaultRepeatAuto = defaultrepeatauto;
                if (this._defaultRepeatAuto.ToUpper().Contains("Y"))
                {
                    _repeatAutoConf = true;
                    _repeatAuto = true;
                }
                else
                {
                    _repeatAutoConf = false;
                    _repeatAuto = false;
                }
            }
            this._dateAuto = dateauto;
            this._timeAuto = timeauto;
            
            this._tryNum = tryNum;

            this._processName = process;
            this._processSelectedFlow = "";
            if (processtimestamp != null)
                this._processTimeStamp = processtimestamp;
            else
                this._processTimeStamp = Auxiliar.GenerateTimeStamp(Auxiliar.ConvertToEST());
            ChangeAutoConfigCommand = new RelayCommand(ChangeAutoConfig, CanChangeAuto);
            ChangeEmailConfigCommand = new RelayCommand(ChangeEmailConfig, CanChangeEmail);
            ChangeReplyConfigCommand = new RelayCommand(ChangeReplyConfig, CanChangeReply);
            this._extrainputs = extrainputs;
            this._transactionsList = new List<TransactionsPackage.Transaction>();
        }
        public int Index
        {
            get { return this._index; }
        }
        public string Name
        {
            get { return this._moduleName; }
        }
        public string Description
        {
            get { return this._description; }
        }

        public string Instance
        {
            get { return this._instance; }
        }

        public string PreviousStep
        {
            get { return this._previousstep; }
        }

        public int? AlternStep
        {
            get { return this._alternstep; }
        }

        public string Flow
        {
            get { return this._flowMode; }
            set { this._flowMode = value; }
        }

        public int TryNum
        {
            get { return this._tryNum; }
            set
            { 
                this._tryNum = value; 
                this.OnPropertyChanged("TryNum"); 
            }
        }
        
        public int? MaxTries
        {
            get { return this._maxTries; }
            set { this._maxTries = value; }
        }

        public Status LastStatus
        {
            get
            {
                if (this._statuslist.Count() > 0)
                    return this._statuslist.Last();
                else
                    return new Status("Scheduled", Auxiliar.DateTimeFromTimeStamp(_processTimeStamp));
            }
        }
        public void AddStatus(Status status)
        {
            this._statuslist.Add(status);
            StatusImage = Auxiliar.GetStatusImage(status.State);
            this.OnPropertyChanged("Status");
        }

        public ExtraInputsSet ExtraInputs
        {
            get { return this._extrainputs; }
            set { this._extrainputs = value; }
        }

        public string Status
        {
            get 
            {
                if (this._statuslist.Count() > 0)
                    return this._statuslist.Last().State;
                else
                    return "";
            }
            set
            {
                //this._status = value;
                this.OnPropertyChanged("Status");
                StatusImage = Auxiliar.GetStatusImage(value);
            }
        }
        public string StatusImage
        {
            get { return Auxiliar.GetStatusImage(Status); }
            set
            {
                //this._statusImage = value;
                this.OnPropertyChanged("StatusImage");
            }
        }
        public List<Status> StatusList
        {
            get { return this._statuslist; }
        }
        public bool Email
        {
            get
            {
                return this._email;
            }
            set
            {
                this._email = value;
                this.OnPropertyChanged("Email");
            }
        }
        public string EmailConf
        {
            get { return this._defaultemail; }
        }
        public bool EmailDefault
        {
            get { return this._emailinitialconf; }
            set
            {
                this._emailinitialconf = value;
                this.OnPropertyChanged("EmailDefault");
                var p = Auxiliar.ProcessInitConfig.Where(x => x.ProjectName == this.Process).FirstOrDefault();
                if (p != null)
                {
                    if (value == false)
                    {
                        p.AllEmailIsChecked = false;
                    }
                    else
                    {
                        if (p.StepList.Count == p.EmailDefaultSteps.ToList().Count)
                            p.AllEmailIsChecked = true;
                    }
                }
            }
        }
        public bool EmailEnabled
        {
            get
            {
                if (this._defaultemail == "yes" || this._defaultemail == "no")
                    return false;
                else
                    return true;
            }
        }
        public bool ProcessAuto
        {
            get { return this._processauto; }
            set
            {
                this._processauto = value;
                this.OnPropertyChanged("ProcessAuto");
            }
        }
        public string AutoConf
        {
            get { return this._defaultprocessauto; }
        }
        public bool AutoDefault
        {
            get{ return this._autoinitialconf; }
            set
            {
                this._autoinitialconf = value;
                this.OnPropertyChanged("AutoDefault");
                var p = Auxiliar.ProcessInitConfig.Where(x => x.ProjectName == this.Process).FirstOrDefault();
                if (p != null)
                {
                    if (value == false)
                    {
                        p.AllAutoIsChecked = false;
                    }
                    else
                    {
                        if (p.StepList.Count -1 == p.AutoDefaultSteps.ToList().Count)
                            p.AllAutoIsChecked = true;
                    }
                }
            }
        }
        public bool AutoEnabled
        {
            get
            {
                bool isSecuential;
                isSecuential = Auxiliar.ProcessInitConfig.Where(x => x.ProjectName == this._processName).FirstOrDefault().IsSecuential;
                if (isSecuential)
                {
                    if (this._defaultprocessauto == "yes" || this._defaultprocessauto == "no")
                        return false;
                    else
                        return true;
                }
                else
                    return false;
            }
        }
        public void ChangeEmailConfig(object obj)
        {
            ListView lista;
            Process process;
            string idx;
            if (obj != null)
            {
                lista = obj as ListView;
                process = lista.DataContext as Process;
                idx = process.Idx;
                //Lanzar subproceso (paralelo) 
                DataAccess.ChangeStepConfig(idx, this.Name, "Email", Email.ToString().ToUpper());
           
            }
        }
        public bool CanChangeEmail(object obj)
        {
            string step = this.Name;
            int stepIndex = this.Index;
            Process p = Auxiliar.ProcessInitConfig.Find(x => x.ProjectName == this.Process);
            if (p != null)
            {
                List<Step> stepsInitConfig = p.StepList;
                //int index = stepsInitConfig.FindIndex(x => x.Name == step);
                Step stepLayout = stepsInitConfig.Where(x => x.Index == stepIndex).FirstOrDefault();
                if (stepLayout != null)
                {
                    string emailDefault = stepLayout.EmailConf;
                    if (emailDefault == "yes" || emailDefault == "no")
                        return false;
                    else
                        return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        public void ChangeAutoConfig(object obj)
        {
            ListView lista;
            Process process;
            string idx;
            if (obj != null)
            {
                lista = obj as ListView;
                process = lista.DataContext as Process;
                idx = process.Idx;
                //Lanzar subproceso (paralelo)
                DataAccess.ChangeStepConfig(idx, this.Name, "Auto", ProcessAuto.ToString().ToUpper());
            }
        }

        public bool CanChangeAuto(object obj)
        {
            string step = this.Name;
            int stepIndex = this.Index;
            Process p = Auxiliar.ProcessInitConfig.Find(x => x.ProjectName == this.Process);
            if (p != null)
            {
                List<Step> stepsInitConfig = p.StepList;
                //int index = stepsInitConfig.FindIndex(x => x.Name == step);
                Step stepLayout = stepsInitConfig.Where(x => x.Index == stepIndex).FirstOrDefault();
                if (stepLayout != null)
                {
                    bool isSecuential;
                    isSecuential = Auxiliar.ProcessInitConfig.Where(x => x.ProjectName == this._processName).FirstOrDefault().IsSecuential;
                    if (isSecuential)
                    {
                        string autoDefault = stepLayout.AutoConf;
                        if (autoDefault == "yes" || autoDefault == "no")
                            return false;
                        else
                            return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }
        public void ChangeReplyConfig(object obj)
        {
            ListView lista;
            Process process;
            string idx;
            if (obj != null)
            {
                lista = obj as ListView;
                process = lista.DataContext as Process;
                idx = process.Idx;

                //Lanzar subproceso (paralelo)
                //private readonly BackgroundWorker worker = new BackgroundWorker();

                DataAccess.ChangeStepConfig(idx, this.Name, "Reply", RepeatAuto.ToString().ToUpper());
            }
        }

        public bool CanChangeReply(object obj)
        {
            if (this.RepeatAutoConf)
            {
                return true;
            }
            else
                return false;
        }
        public string Process
        {
            get { return this._processName; }
            set { this._processName = value; }
        }
        public string Log
        {
            get { return this._log; }
            set { this._log = value; }
        }
        public string[] Evidence
        {
            get { return this._evidence; }
            set { this._evidence = value; }
        }
        public string Transactions
        {
            get { return this._transactions; }
            set { this._transactions = value; }
        }

        public List<TransactionsPackage.Transaction> TransactionsList
        {
            get { return this._transactionsList; }
            set { this._transactionsList = value; }
        }

        public bool RepeatAutoConf
        {
            get { return this._repeatAutoConf; }
        }
        
        public bool RepeatAuto
        {
            get { return this._repeatAuto; }
            set 
            {
                this._repeatAuto = value;
                this.OnPropertyChanged("RepeatAuto");
            }
        }
        
        public string RepeatDate
        {
            get { return this._dateAuto; }
            set 
            {
                this._dateAuto = value;
                this.OnPropertyChanged("RepeatDate");
            }
        }
        
        public string RepeatTime
        {
            get { return this._timeAuto; }
            set 
            {
                this._timeAuto = value;
                this.OnPropertyChanged("RepeatTime");
            }
        }
        
        [JsonIgnore]
        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                if (this._selectable == "must")
                    this._isSelected = true;
                else 
                    this._isSelected = value;
                this.OnPropertyChanged("IsSelected");
                var p = Auxiliar.ProcessInitConfig.Where(x => x.ProjectName == this.Process).FirstOrDefault();
                if (p != null)
                {
                    if (value == false)
                    {
                        p.SelectAllCheckIsChecked = false;
                    }
                    else
                    {
                        if (p.StepList.Count == p.SelectedSteps.ToList().Count)
                            p.SelectAllCheckIsChecked = true;
                    }
                }
            }
        }
        [JsonIgnore]
        public string Selectable
        {
            get { return this._selectable; }
        }
        [JsonIgnore]
        public bool IsEnabled
        {
            get { 
                if (this._selectable == "must")
                    return false;
                else
                    return true;
            }
        }
        
        [JsonIgnore]
        public string ProcessSelectedFlow
        {
            get { return this._processSelectedFlow; }
            set 
            {
                this._processSelectedFlow = value; 
                this.OnPropertyChanged("ProcessSelectedFlow"); 
                this.OnPropertyChanged("FlowMatch"); 
            }
        }
        
        [JsonIgnore]
        public bool FlowMatch
        {
            get {
                if (this._processSelectedFlow == "")
                    return true; 
                else if (this._flowMode == "" || this._flowMode == "ALL" || this._flowMode == this._processSelectedFlow)
                    return true;
                else 
                    return false;
            }
        }

    }
}
