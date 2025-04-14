using RunTeamConsole.Models;
using RunTeamConsole.Views;
using RunTeamConsole.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Diagnostics;
using Process = RunTeamConsole.Models.Process;
using RunTeamConsole.Views.Windows;
using RunTeamConsole.Views.Principal.Windows;
using System.Net;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography.Xml;

namespace RunTeamConsole.ViewModels
{
    public class PrincipalViewModel : ObservableObject
    {
        private string _ituser;
        private string _ituserViewText;
        private string _osPass;
        private string _valItuser;
        private Process _selectedProcess;

        ituserWindow confirmationWindow;
        MaxTriesWindow maxTriesW;

        DataAccess da;
        private int _selectedProcessesCount;
        private string _selectedProcessesStatus;
        private bool _emergencyAbort;
        private bool _selectAllProcessesIsChecked;
        private int _displayedMessages;

        private string _selectedFilterType;
        private string _selectedFilterVariable;
        private ObservableCollection<string> _filterVariableList;
        private ObservableCollection<Filter> _filterList;

        private ObservableCollection<EvidenceItem> _evidence;
        private int _selectedTab = 0;

        public static bool processUpdFVal = false;
        public ObservableCollection<Process> Processes { get; private set; }
        public ObservableCollection<Models.Maintenance> Maintenances { get; private set; }
        public ObservableCollection<BroadcastMessages> BroadcastMessages { get; private set; }

        public RelayCommand ShowUserProfileCommand { get; private set; }
        public RelayCommand ShowMaintenanceCommand { get; private set; }
        public RelayCommand MessageOKCommand { get; private set; }

        public RelayCommand ChangeAddProcessViewCommand { get; private set; }
        public RelayCommand ChangeFavoritesViewCommand { get; private set; }
        public RelayCommand SelectAllProcessesCommand { get; private set; }

        public RelayCommand InfoViewCommand { get; private set; }
        public RelayCommand InfoEditCommand { get; private set; }
        public RelayCommand LogViewCommand { get; private set; }
        public RelayCommand ShowEvidenceCommand { get; private set; }

        public RelayCommand ShowStartConfirmationCommand { get; private set; }
        public RelayCommand StartProcessCommand { get; private set; }
        public RelayCommand ContinueProcessCommand { get; private set; }
        public RelayCommand ContinueFromOptionWindowCommand { get; private set; }
        public RelayCommand RepeatAlternCommand { get; private set; }
        public RelayCommand RepeatOrSolveManuallyCommand { get; private set; }
        public RelayCommand RepeatStepCommand { get; private set; }
        public RelayCommand RepeatFromOptionWindowCommand { get; private set; }
        public RelayCommand AlternFromOptionWindowCommand { get; private set; }
        public RelayCommand AbortProcessCommand { get; private set; }

        public RelayCommand ShowEmergencyAbortCommand { get; private set; }
        public RelayCommand EmergencyAbortCommand { get; private set; }

        public RelayCommand DiscardProcessCommand { get; private set; }

        public RelayCommand ShowFiltersCommand { get; private set; }
        public RelayCommand ClearAllFiltersCommand { get; private set; }
        public RelayCommand AddFilterCommand { get; private set; }
        public RelayCommand RemoveFiltersCommand { get; private set; }
        public RelayCommand ApplyFiltersCommand { get; private set; }
        public RelayCommand CancelFiltersCommand { get; private set; }

        public RelayCommand RefreshCommand { get; private set; }

        public string LogTitle { get; private set; }
        public string LogText { get; private set; }

        public int counterReplace = 0;
        public int counterProcesses = 80;
        public int processSelected = 0;

        private DateTime _calendarSelectedDate;
        private ObservableCollection<Models.Maintenance> _calendarMaintenanceList;

        public FilterSearch filterWindow;

        #region Wait for DataAccess Async setup

        public bool AyncSetupCompleted = false;
        /*public async void AsyncDataAccessVariables()
        {
            await Task.Run(async () => 
            { 

                while(!DataAccess.AsyncDataReady)
                {
                    await Task.Delay(500);
                }
            
            });

            Processes = new ObservableCollection<Process>(da.GetProcesses());
            Maintenances = new ObservableCollection<Models.Maintenance>(da.GetMaintenances());

            Debug.WriteLine("\n\n-- PrincipalViewModel Loaded Processes and Maintenances --\n\n");

            Debug.WriteLine("\n\n-- PrincipalViewModel Setup Completed --\n\n");

            SelectAllProcessesCommand = new RelayCommand(ChangeSelectAllProcesses, CanSelectAllProcesses);

            BroadcastMessages = new ObservableCollection<BroadcastMessages>();

            ChangeAddProcessViewCommand = new RelayCommand(ChangeToAddProcessVM, AlwwaysCanExecute);
            ChangeFavoritesViewCommand = new RelayCommand(ChangeToFavoritesVM, AlwwaysCanExecute);
            SelectAllProcessesCommand = new RelayCommand(ChangeSelectAllProcesses, CanSelectAllProcesses);

            InfoViewCommand = new RelayCommand(OpenInfoView, CanOpenInfoView);
            LogViewCommand = new RelayCommand(OpenLogView, CanOpenLogView);
            InfoEditCommand = new RelayCommand(EditInfo, CantExecute);
            ShowEvidenceCommand = new RelayCommand(OpenEvidence, CanOpenEvidence);

            ShowStartConfirmationCommand = new RelayCommand(ShowStartConfirmation, CanShowStartConfirmation);
            StartProcessCommand = new RelayCommand(StartProcess, CanStart);
            ContinueProcessCommand = new RelayCommand(ContinueSelectedProcesses, CanContinue);
            RepeatAlternCommand = new RelayCommand(RepeatAltern, CanRepeatAltern);
            AbortProcessCommand = new RelayCommand(AbortProcess, CanAbort);

            ShowEmergencyAbortCommand = new RelayCommand(ShowEmergencyAbort, CanShowEmergencyAbort);
            EmergencyAbortCommand = new RelayCommand(EmergencyAbort, CanAbortEmergency);

            DiscardProcessCommand = new RelayCommand(DiscardProcess, CanDiscard);

            ShowFiltersCommand = new RelayCommand(ShowFiltersWindow, CanShowFiltersWindow);
            ClearAllFiltersCommand = new RelayCommand(ClearAllFilters, CanClearAllFilters);
            AddFilterCommand = new RelayCommand(AddFilter, CanAddFilter);
            RemoveFiltersCommand = new RelayCommand(RemoveSelectedFilters, CanRemoveSelectedFilters);
            ApplyFiltersCommand = new RelayCommand(ApplyFilters, CanApplyFilters);
            CancelFiltersCommand = new RelayCommand(CancelFilters, AlwwaysCanExecute);

            RefreshCommand = new RelayCommand(RefreshProcess, AlwwaysCanExecute);

            _ituser = UserProfile.ItUser.ToLower();

            _osPass = "";
            _valItuser = UserProfile.ItUser.ToLower();
            this._evidence = new ObservableCollection<EvidenceItem>();
            _selectedProcessesCount = 0;
            _displayedMessages = 0;
            _selectedProcessesStatus = "";
            _calendarMaintenanceList = Maintenances;

            _filterVariableList = new ObservableCollection<string>();
            _filterList = new ObservableCollection<Filter>();

            DispatcherTimerSetup();
            AyncSetupCompleted = true;
        }*/
        #endregion
        
        public PrincipalViewModel()
        {
            /*this._emergencyAbort = false;

            ShowUserProfileCommand = new RelayCommand(ShowUserProfile, AlwwaysCanExecute);
            ShowMaintenanceCommand = new RelayCommand(ShowMaintenance, AlwwaysCanExecute);
            MessageOKCommand = new RelayCommand(OkClickPopup, AlwwaysCanExecute);
            RepeatOrSolveManuallyCommand = new RelayCommand(SolveManually, AlwwaysCanExecute);

            RepeatStepCommand = new RelayCommand(RepeatStep, AlwwaysCanExecute);
            RepeatFromOptionWindowCommand = new RelayCommand(OptionRepeatProcess, AlwwaysCanExecute);
            AlternFromOptionWindowCommand = new RelayCommand(OptionAlternProcess, AlwwaysCanExecute);
            ContinueFromOptionWindowCommand = new RelayCommand(OptionContinueProcess, AlwwaysCanExecute);

            da = new DataAccess();

            AsyncDataAccessVariables();*/
            this._emergencyAbort = false;

            ShowUserProfileCommand = new RelayCommand(ShowUserProfile, AlwwaysCanExecute);
            ShowMaintenanceCommand = new RelayCommand(ShowMaintenance, AlwwaysCanExecute);
            MessageOKCommand = new RelayCommand(OkClickPopup, AlwwaysCanExecute);
            RepeatOrSolveManuallyCommand = new RelayCommand(SolveManually, AlwwaysCanExecute);

            RepeatStepCommand = new RelayCommand(RepeatStep, AlwwaysCanExecute);
            RepeatFromOptionWindowCommand = new RelayCommand(OptionRepeatProcess, AlwwaysCanExecute);
            AlternFromOptionWindowCommand = new RelayCommand(OptionAlternProcess, AlwwaysCanExecute);
            ContinueFromOptionWindowCommand = new RelayCommand(OptionContinueProcess, AlwwaysCanExecute);

            da = new DataAccess();

            Processes = new ObservableCollection<Process>(da.GetProcesses());
            Maintenances = new ObservableCollection<Models.Maintenance>(da.GetMaintenances());
            BroadcastMessages = new ObservableCollection<BroadcastMessages>();

            ChangeAddProcessViewCommand = new RelayCommand(ChangeToAddProcessVM, AlwwaysCanExecute);
            ChangeFavoritesViewCommand = new RelayCommand(ChangeToFavoritesVM, AlwwaysCanExecute);
            //RefreshProcessCommand = new RelayCommand(ChangeToFavoritesVM, AlwwaysCanExecute);
            SelectAllProcessesCommand = new RelayCommand(ChangeSelectAllProcesses, CanSelectAllProcesses);

            InfoViewCommand = new RelayCommand(OpenInfoView, CanOpenInfoView);
            LogViewCommand = new RelayCommand(OpenLogView, CanOpenLogView);
            InfoEditCommand = new RelayCommand(EditInfo, CantExecute);
            ShowEvidenceCommand = new RelayCommand(OpenEvidence, CanOpenEvidence);

            ShowStartConfirmationCommand = new RelayCommand(ShowStartConfirmation, CanShowStartConfirmation);
            StartProcessCommand = new RelayCommand(StartProcess, CanStart);
            ContinueProcessCommand = new RelayCommand(ContinueSelectedProcesses, CanContinue);
            RepeatAlternCommand = new RelayCommand(RepeatAltern, CanRepeatAltern);
            AbortProcessCommand = new RelayCommand(AbortProcess, CanAbort);

            ShowEmergencyAbortCommand = new RelayCommand(ShowEmergencyAbort, CanShowEmergencyAbort);
            EmergencyAbortCommand = new RelayCommand(EmergencyAbort, CanAbortEmergency);

            DiscardProcessCommand = new RelayCommand(DiscardProcess, CanDiscard);

            ShowFiltersCommand = new RelayCommand(ShowFiltersWindow, CanShowFiltersWindow);
            ClearAllFiltersCommand = new RelayCommand(ClearAllFilters, CanClearAllFilters);
            AddFilterCommand = new RelayCommand(AddFilter, CanAddFilter);
            RemoveFiltersCommand = new RelayCommand(RemoveSelectedFilters, CanRemoveSelectedFilters);
            ApplyFiltersCommand = new RelayCommand(ApplyFilters, CanApplyFilters);
            CancelFiltersCommand = new RelayCommand(CancelFilters, AlwwaysCanExecute);

            RefreshCommand = new RelayCommand(RefreshProcess, AlwwaysCanExecute);

            _ituser = UserProfile.ItUser.ToLower();
            //_ituser = "";
            _osPass = "";
            _valItuser = UserProfile.ItUser.ToLower();
            //_valItuser = "";
            this._evidence = new ObservableCollection<EvidenceItem>();
            _selectedProcessesCount = 0;
            _displayedMessages = 0;
            _selectedProcessesStatus = "";
            _calendarMaintenanceList = Maintenances;

            _filterVariableList = new ObservableCollection<string>();
            _filterList = new ObservableCollection<Filter>();

            DispatcherTimerSetup();
        }
        public string ValItuser
        {
            get { return this._valItuser; }
            set
            {
                this._valItuser = value;
                this._ituser = value;
                this.OnPropertyChanged("ValItuser");
            }
        }
        public string ItuserViewText
        {
            get
            {
                string value = "Current " + this._ituser + " processes";
                return value; 
            }
        }
        public string OSPass
        {
            get { return this._osPass; }
            set
            {
                this._osPass = value;
                this.OnPropertyChanged("OSPass");
            }
        }
        public Process SelectedProcess
        {
            get { return this._selectedProcess; }
            set
            {
                this._selectedProcess = value;
                ObservableCollection<EvidenceItem> tempEvidence = new ObservableCollection<EvidenceItem>();
                if (value != null)
                {
                    foreach (Step step in _selectedProcess.StepList)
                    {
                        if (step.Evidence.Length > 0)
                        {
                            for (int i = 0; i < step.Evidence.Length; i++)
                            {
                                tempEvidence.Add(new EvidenceItem(step.Evidence[i]));
                            }
                        }
                    }
                }
                Evidences = tempEvidence;
                this.OnPropertyChanged("SelectedProcess");
            }
        }
        public IEnumerable<Process> SelectedProcesses
        {
            get { return Processes.Where(o => o.IsSelected); }
        }
        public int SelectedProcessesCount
        {
            get { return _selectedProcessesCount; }
            set
            {
                this._selectedProcessesCount = value;
                this.OnPropertyChanged("SelectedProcessesCount");
            }
        }
        public string SelectedProcessesStatus
        {
            get { return _selectedProcessesStatus; }
            set
            {
                this._selectedProcessesStatus = value;
                this.OnPropertyChanged("SelectedProcessesStatus");
            }
        }
        public bool SelectAllProcessesIsChecked
        {
            get { return this._selectAllProcessesIsChecked; }
            set
            {
                this._selectAllProcessesIsChecked = value;
                this.OnPropertyChanged("SelectAllProcessesIsChecked");
            }
        }
        public void ChangeSelectAllProcesses(object obj)
        {
            string processType, step, status, currentType, currentStep, currentStatus;
            
            if (this._selectAllProcessesIsChecked == true)
            {
                if (SelectedProcesses.ToList().Count == 0)
                {
                    if (Processes.Any(p => p.CurrentStatus.ToUpper() == "COMPLETED"))
                    {
                        if (Processes.Any(p => p.CurrentStatus.ToUpper() == "SCHEDULED"))
                        {
                            if (Processes.Any(p => p.CurrentStatus.ToUpper() == "ABORTED"))
                            { //COMPLETED, SCHEDULED && ABORTED
                                foreach (Process p in Processes)
                                {
                                    if (p.CurrentStatus.ToUpper() == "SCHEDULED" || p.CurrentStatus.ToUpper() == "ABORTED" || p.CurrentStatus.ToUpper() == "COMPLETED")
                                        p.IsSelected = true;
                                }
                            }
                            else// SCHEDULED && COMPLETED
                            {
                                foreach (Process p in Processes)
                                {
                                    if (p.CurrentStatus.ToUpper() == "SCHEDULED" || p.CurrentStatus.ToUpper() == "COMPLETED")
                                        p.IsSelected = true;
                                }
                            }
                        }//COMPLETED && ABORTED
                        else if (Processes.Any(p => p.CurrentStatus.ToUpper() == "ABORTED"))
                        {
                            foreach (Process p in Processes)
                            {
                                if (p.CurrentStatus.ToUpper() == "ABORTED" || p.CurrentStatus.ToUpper() == "COMPLETED")
                                    p.IsSelected = true;
                            }
                        }
                        else //COMPLETED
                        {
                            foreach (Process p in Processes)
                            {
                                if (p.CurrentStatus.ToUpper() == "COMPLETED")
                                    p.IsSelected = true;
                            }
                        }
                    }//If there is no COMPLETED
                    else if (Processes.Any(p => p.CurrentStatus.ToUpper() == "SCHEDULED"))
                    {// SCHEDULED && ABORTED
                        if (Processes.Any(p => p.CurrentStatus.ToUpper() == "ABORTED"))
                        {
                            foreach (Process p in Processes)
                            {
                                if (p.CurrentStatus.ToUpper() == "SCHEDULED" || p.CurrentStatus.ToUpper() == "ABORTED")
                                    p.IsSelected = true;
                            }
                        }
                        else
                        {//solo SCHEDULED
                            foreach (Process p in Processes)
                            {
                                if (p.CurrentStatus.ToUpper() == "SCHEDULED")
                                    p.IsSelected = true;
                            }
                        }
                    }//If there is no COMPLETED or SCHEDULED
                    else if (Processes.Any(p => p.CurrentStatus.ToUpper() == "ABORTED"))
                    {
                        foreach (Process p in Processes)
                        {
                            if (p.CurrentStatus.ToUpper() == "ABORTED")
                                p.IsSelected = true;
                        }
                    }
                }
                else
                {
                    processType = SelectedProcesses.First().ProjectName.ToUpper();
                    step = SelectedProcesses.First().CurrentStepName.ToUpper();
                    status = SelectedProcesses.First().CurrentStatus.ToUpper();

                    foreach (Process p in Processes)
                    {
                        currentStatus = p.CurrentStatus.ToUpper();
                        currentType = p.ProjectName.ToUpper(); ;
                        currentStep = p.CurrentStepName.ToUpper();
                        if (currentType == processType && currentStep == step && currentStatus == status)
                        {
                            p.IsSelected = true;
                            
                            processSelected++;
                        }
                    }
                }
            }
            else
            {
                foreach (Process p in Processes)
                {
                    p.IsSelected = false;
                }
            }
            int count = SelectedProcesses.Count();
            SelectedProcessesCount = count;
            if (count > 0 && count < 50)
            {
                SelectedProcessesStatus = SelectedProcesses.First().CurrentStatus;
            }
            else
            {
                SelectedProcessesStatus = "";
            }
        }
        public bool CanSelectAllProcesses(object obj)
        {
            if (Processes.Count > 0)
            {
                if (Processes.Any(p => p.CurrentStatus.ToUpper() == "SCHEDULED")
                    || Processes.Any(p => p.CurrentStatus.ToUpper() == "ABORTED")
                    || Processes.Any(p => p.CurrentStatus.ToUpper() == "COMPLETED"))
                    return true;
                else
                {
                    if (SelectedProcess != null)
                        return true;
                    else if( processSelected == 49)
                        return false;
                    else
                        return false;
                }

            }
            else
                return false;
        }
        public string SelectedFilterType
        {
            get { return _selectedFilterType; }
            set
            {
                this._selectedFilterType = value;
                this.OnPropertyChanged("SelectedFilterType");
                if (!String.IsNullOrEmpty(value))
                {
                    FilterVariableList = new ObservableCollection<string>();
                    switch (value)
                    {
                        case "Process Type":
                            foreach (Process process in Processes)
                            {
                                var tempType = FilterVariableList.Where(x => x == process.ProjectName).FirstOrDefault();
                                if (String.IsNullOrEmpty(tempType))
                                {
                                    FilterVariableList.Add(process.ProjectName);
                                }
                            }
                            break;
                        case "Customer":
                            foreach (Process process in Processes)
                            {
                                var tempType = FilterVariableList.Where(x => x == process.Customer).FirstOrDefault();
                                if (String.IsNullOrEmpty(tempType))
                                {
                                    FilterVariableList.Add(process.Customer);
                                }
                            }
                            break;
                        case "Environment":
                            foreach (Process process in Processes)
                            {
                                var tempType = FilterVariableList.Where(x => x == process.Environment).FirstOrDefault();
                                if (String.IsNullOrEmpty(tempType))
                                {
                                    FilterVariableList.Add(process.Environment);
                                }
                            }
                            break;
                        case "Status":
                            FilterVariableList.Add("Scheduled");
                            FilterVariableList.Add("Processing");
                            FilterVariableList.Add("DONE");
                            FilterVariableList.Add("WARNING");
                            FilterVariableList.Add("FAIL");
                            FilterVariableList.Add("ABORTED");
                            break;
                    }
                }
            }
        }
        public string SelectedFilterVariable
        {
            get { return this._selectedFilterVariable; }
            set
            {
                this._selectedFilterVariable = value;
                this.OnPropertyChanged("SelectedFilterVariable");
            }
        }
        public ObservableCollection<string> FilterVariableList
        {
            get { return _filterVariableList; }
            set
            {
                this._filterVariableList = value;
                this.OnPropertyChanged("FilterVariableList");
            }
        }
        public ObservableCollection<Filter> FilterList
        {
            get { return _filterList; }
            set
            {
                this._filterList = value;
                this.OnPropertyChanged("FilterList");
            }
        }
        public void ShowFiltersWindow(object obj)
        {
            filterWindow = new FilterSearch();
            filterWindow.Owner = System.Windows.Application.Current.MainWindow;
            filterWindow.DataContext = this;
            filterWindow.Show();
        }
        public bool CanShowFiltersWindow(object obj)
        {
            if (filterWindow == null)
                return true;
            else
                return false;
        }
        public void ClearAllFilters(object obj)
        {
            foreach (Process p in Processes)
            {
                p.IsHidden = false;
            }
            FilterList = new ObservableCollection<Filter>();
        }
        public bool CanClearAllFilters(object obj)
        {
            if (FilterList.Count > 0)
                return true;
            else
                return false;
        }
        public void AddFilter(object obj)
        {
            FilterList.Add(new Filter { Type = SelectedFilterType, Variable = SelectedFilterVariable, IsApplied = false, IsSelected = false });
        }
        public bool CanAddFilter(object obj)
        {
            if (!String.IsNullOrEmpty(SelectedFilterType) && !String.IsNullOrEmpty(SelectedFilterVariable))
                return true;
            else
                return false;
        }
        public void RemoveSelectedFilters(object obj)
        {
            foreach (Filter filter in FilterList.ToList())
            {
                if (filter.IsSelected == true)
                {
                    FilterList.Remove(filter);
                }
            }
            if (FilterList.Count == 0)
            {
                foreach (Process p in Processes)
                {
                    p.IsHidden = false;
                }
            }
            else
            {
                foreach (Process p in Processes)
                {
                    p.IsHidden = true;
                }
                foreach (Filter filter in FilterList)
                {
                    if (filter.IsApplied == true)
                    {
                        switch (filter.Type)
                        {
                            case "Process Type":
                                foreach (Process process in Processes)
                                {
                                    if (process.ProjectName == filter.Variable)
                                    {
                                        process.IsHidden = false;
                                    }
                                }
                                break;
                            case "Customer":
                                foreach (Process process in Processes)
                                {
                                    if (process.Customer == filter.Variable)
                                    {
                                        process.IsHidden = false;
                                    }
                                }
                                break;
                            case "Environment":
                                foreach (Process process in Processes)
                                {
                                    if (process.Environment == filter.Variable)
                                    {
                                        process.IsHidden = false;
                                    }
                                }
                                break;
                            case "Status":
                                foreach (Process process in Processes)
                                {
                                    if (process.CurrentStatus == filter.Variable)
                                    {
                                        process.IsHidden = false;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }
        public bool CanRemoveSelectedFilters(object obj)
        {
            if (FilterList.Any(x => x.IsSelected))
                return true;
            else
                return false;
        }
        public void ApplyFilters(object obj)
        {
            foreach (Process p in Processes)
            {
                p.IsHidden = true;
                p.IsSelected = false;
            }
            foreach (Filter filter in FilterList)
            {
                switch (filter.Type)
                {
                    case "Process Type":
                        foreach (Process process in Processes)
                        {
                            if (process.ProjectName == filter.Variable)
                            {
                                process.IsHidden = false;
                            }
                        }
                        break;
                    case "Customer":
                        foreach (Process process in Processes)
                        {
                            if (process.Customer == filter.Variable)
                            {
                                process.IsHidden = false;
                            }
                        }
                        break;
                    case "Environment":
                        foreach (Process process in Processes)
                        {
                            if (process.Environment == filter.Variable)
                            {
                                process.IsHidden = false;
                            }
                        }
                        break;
                    case "Status":
                        foreach (Process process in Processes)
                        {
                            if (process.CurrentStatus == filter.Variable)
                            {
                                process.IsHidden = false;
                            }
                        }
                        break;
                }
                filter.IsApplied = true;
            }
            filterWindow.Close();
        }
        public bool CanApplyFilters(object obj)
        {
            if (FilterList.Count > 0)
                return true;
            else
                return false;
        }
        public void CancelFilters(object obj)
        {
            filterWindow.Close();
            foreach (Filter filter in FilterList.ToList())
            {
                if (filter.IsApplied != true)
                {
                    FilterList.Remove(filter);
                }
            }
        }
        public static void SaveCountOfReplace(int replaceCount)
        {
            string cnfPath = "\\\\10.130.19.40\\ait\\TMP\\";
            string inputsFileName = "REPLACECOUNT.txt";
            string inputsContent = "Replace executions: " + replaceCount.ToString();
            if (File.Exists(inputsFileName))
            {
                File.Delete(inputsFileName);
            }
            File.WriteAllText(cnfPath + inputsFileName, inputsContent);
        }
        public int GetReplaceProcessInfoExec()
        {
            counterReplace++;
            SaveCountOfReplace(counterReplace);
            return counterReplace;
        }
        public async void ReplaceProcessInfo(string filechanged)
        {
            //Desde aquí comentado de manera correcta
            string tcode = "", description = "", prepost = "";
            Process NewInfo, p;
            ProcessExecution TempProcessExecution;
            StepExecution TempStepExecution;
            StatusExecution TempStatusExecution;
            string acStatus = "\\\\10.130.19.40\\ait\\RUNTEAMCONSOLE\\AC_STATUS\\";

            NewInfo = da.GetProcess(filechanged);
            
            string processingPath = Auxiliar.GetProcessingPath(NewInfo.Idx);

            if (NewInfo != null)
            {
                string acStatusFilePath = acStatus + UserProfile.ItUser.ToUpper() + "_" + NewInfo.Idx + "." + NewInfo.CurrentStatus;
                string acFileContent = NewInfo.Idx + "|" + NewInfo.CurrentStatus + "|" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string statusFileName = UserProfile.ItUser.ToUpper() + "_" + NewInfo.Idx;
                var dir = new DirectoryInfo(acStatus);

                foreach (var file in dir.EnumerateFiles(statusFileName + ".*"))
                    file.Delete();

                File.WriteAllText(acStatusFilePath, acFileContent);

                //Reviews all the idx of the user's processes and evaluates which one it should enter
                //and assigns the values ​​of the NewInfo process to the p process, repeating the data in a given way
                p = Processes.Where(x => x.Idx == NewInfo.Idx).FirstOrDefault();

                if (p != null)
                {
                    //Checks if the StepIndex of NewInfo is greater than or equal to that of p (even when p was generated equal to NewInfo)
                    //updates the green progress bar
                    if (NewInfo.CurrentStepIndex >= p.CurrentStepIndex && !String.IsNullOrEmpty(NewInfo.RealTimeLog))
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            //Assigns the TimeLog of NewInfo to p
                            //and refresh the datetime of the execution of the step as an update
                            p.RealTimeLog = NewInfo.RealTimeLog;
                        });
                    }
                    Thread.Sleep(1000);

                    //adds evidence if there is one
                    ObservableCollection<EvidenceItem> tempEvidence = new ObservableCollection<EvidenceItem>();
                    
                    for (int i = 0; i < NewInfo.StepList.Count; i++)
                    {
                        if (NewInfo.StepList[i].Evidence.Length > 0)
                        {
                            //Assigns the evidence of NewInfo to p
                            p.StepList[i].Evidence = NewInfo.StepList[i].Evidence;
                            for (int j = 0; j < NewInfo.StepList[i].Evidence.Length; j++)
                            {
                                if(!String.IsNullOrEmpty(NewInfo.StepList[i].Evidence[j]))
                                    tempEvidence.Add(new EvidenceItem(NewInfo.StepList[i].Evidence[j]));
                            }
                        }
                    }
                    if (_selectedProcess != null)
                    {
                        if (NewInfo.Idx == _selectedProcess.Idx)
                        {
                            Evidences = tempEvidence;
                            SelectedProcess = _selectedProcess;
                        }
                    }

                    //if ((p.IsSecuential && NewInfo.CurrentStepIndex == p.CurrentStepIndex) || (!p.IsSecuential && NewInfo.CurrentStatus != "Scheduled" && NewInfo.CurrentStepIndex != 0))
                    if ((NewInfo.CurrentStepIndex == p.CurrentStepIndex) || (NewInfo.CurrentStatus != "Scheduled" && NewInfo.CurrentStepIndex != 0))
                    {
                        DateTime processDateTimeWithoutMiliseconds = new DateTime(p.CurrentStatusDateTime.Year, p.CurrentStatusDateTime.Month, p.CurrentStatusDateTime.Day, 
                            p.CurrentStatusDateTime.Hour, p.CurrentStatusDateTime.Minute, p.CurrentStatusDateTime.Second);

                        //if (p.CurrentStatusDateTime < NewInfo.CurrentStatusDateTime || (processDateTimeWithoutMiliseconds == NewInfo.CurrentStatusDateTime && p.CurrentStatus != NewInfo.CurrentStatus))
                        if (p.CurrentStatus != NewInfo.CurrentStatus)
                        {
                            try
                            {
                                TempProcessExecution = Auxiliar.GetProcessExecution(NewInfo.Idx);
                                if (NewInfo.CurrentStepIndex != TempProcessExecution.CurrentStep)
                                {
                                    TempProcessExecution.CurrentStep = NewInfo.CurrentStepIndex;
                                    Auxiliar.PutProcessTraking(TempProcessExecution, "ProcessExecutions/" + TempProcessExecution.Id);
                                }
                            }
                            catch
                            {
                                TempProcessExecution = new ProcessExecution { Idx = p.Idx, ProcessName = p.ProjectName, CurrentStep = p.CurrentStepIndex, GroupName = p.Team, User = p.User, PAS = p.PAS, DBS = p.DBS, SID = p.SID, Title = p.Description, Customer = p.Customer, CreateDate = Auxiliar.DateTimeFromTimeStamp(p.TimeStamp) };
                                Auxiliar.PostProcessTraking(TempProcessExecution, "ProcessExecutions/");
                            }
                            
                            Step tempStep = NewInfo.StepList.Where(x => x.Name == NewInfo.CurrentStepName).FirstOrDefault();
                            
                            if (tempStep.Status != "" && tempStep.Status != "Processing")
                            {
                                if (tempStep.Email || tempStep.Status == "FAIL" || tempStep.Status == "WARNING")
                                    Auxiliar.SendStatusByEmail(NewInfo);

                                TempStepExecution = Auxiliar.GetStepExecution(NewInfo.Idx, tempStep.Index);
                                
                                if (TempStepExecution == null)
                                {
                                    TempStepExecution = new StepExecution { Idx = NewInfo.Idx, StepIndex = tempStep.Index, Name = tempStep.Name, Description = tempStep.Description, Log = tempStep.Log, Message = NewInfo.Message };
                                    Auxiliar.PostProcessTraking(TempStepExecution, "StepExecutions/");
                                }
                                else
                                {
                                    if (TempStepExecution.Log != tempStep.Log)
                                    {
                                        TempStepExecution.Log = tempStep.Log;
                                        Auxiliar.PutProcessTraking(TempStepExecution, "StepExecutions/" + TempStepExecution.Id);
                                    }
                                }

                                TempStatusExecution = Auxiliar.GetStatusExecution(NewInfo.Idx, tempStep.Index, tempStep.Status, tempStep.LastStatus.DateTime);
                                
                                if (TempStatusExecution == null)
                                {
                                    TempStatusExecution = new StatusExecution { Idx = NewInfo.Idx, StepIndex = tempStep.Index, State = tempStep.Status, DateTime = tempStep.LastStatus.DateTime };
                                    Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");
                                }
                            }
                            
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                p.CurrentStep = tempStep;
                                p.Progress = NewInfo.Progress;

                                for (int i = 0; i < NewInfo.StepList.Count; i++)
                                {
                                    if (p.StepList[i].Status != NewInfo.StepList[i].Status && !(NewInfo.StepList[i].Status == "" || NewInfo.StepList[i].Status == "Schedled"))//scheduled
                                        p.StepList[i].AddStatus(NewInfo.StepList[i].LastStatus);
                                    p.StepList[i].Log = NewInfo.StepList[i].Log;
                                }
                            });
                            processUpdFVal = true;
                            
                            if (NewInfo.OptionsMessage != null)
                            {
                                Auxiliar.ShowOptionsMessage(NewInfo.Idx, NewInfo.OptionsMessage);
                            }

                            Step step = p.StepList.Where(x => x.Name == NewInfo.CurrentStepName).FirstOrDefault();
                            
                            if (NewInfo.Message != "")
                            {
                                if (NewInfo.Message.Contains("NOTCOMPLETED"))
                                {
                                    if ((NewInfo.CurrentStatus == "FAIL" || NewInfo.CurrentStatus == "WARNING") && step.RepeatAuto)
                                    {
                                        RepeatAuto(p);
                                    }
                                }
                                else
                                {
                                    if (!NewInfo.Message.Contains("PASSWORD NOT VALID"))
                                    {
                                        if (p.IsSecuential)
                                        {
                                            if (p.CurrentStatus == "DONE")
                                            {
                                                if (step.ProcessAuto)
                                                {
                                                    if (Directory.Exists(processingPath))
                                                    {
                                                        ContinueProcess(p);
                                                    }
                                                    else
                                                    {
                                                        if (Directory.Exists(processingPath + NewInfo.Idx))
                                                        {
                                                            Directory.Move(processingPath + NewInfo.Idx, processingPath);
                                                            ContinueProcess(p);
                                                        }
                                                        else
                                                        {
                                                            Auxiliar.ShowMessage(NewInfo.Idx, "File not fond: " + processingPath + "\nPlease delete the process and schedule it again.", "Error");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Directory.Exists(processingPath))
                                            {
                                                StartProcess(p);
                                            }
                                            else
                                            {
                                                if (Directory.Exists(processingPath + NewInfo.Idx))
                                                {
                                                    Directory.Move(processingPath + NewInfo.Idx, processingPath);
                                                    StartProcess(p);
                                                }
                                                else
                                                {
                                                    Auxiliar.ShowMessage(NewInfo.Idx, "File not fond: " + processingPath + "\nPlease delete the process and schedule it again.", "Error");
                                                }
                                            }
                                        }
                                    }
                                    Auxiliar.ShowPopupMessage(NewInfo.Idx, NewInfo.Message, NewInfo.CurrentStatus);
                                    Auxiliar.SendLogRequest("Message shown|" + NewInfo.Idx + "|status " + NewInfo.CurrentStatus + "|message " + NewInfo.Message);
                                }
                            }
                            else
                            {
                                if (p.IsSecuential)
                                {
                                    if (p.CurrentStatus == "DONE")
                                    {
                                        if (step.ProcessAuto)
                                        {
                                            if (Directory.Exists(processingPath))
                                            {
                                                ContinueProcess(p);
                                            }
                                            else
                                            {
                                                if (Directory.Exists(processingPath + NewInfo.Idx))
                                                {
                                                    Directory.Move(processingPath + NewInfo.Idx, processingPath);
                                                    ContinueProcess(p);
                                                }
                                                else
                                                {
                                                    Auxiliar.ShowMessage(NewInfo.Idx, "File not fond: " + processingPath + "\nPlease delete the process and schedule it again.", "Error");
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (Directory.Exists(processingPath))
                                    {
                                        StartProcess(p);
                                    }
                                    else
                                    {
                                        if (Directory.Exists(processingPath + NewInfo.Idx))
                                        {
                                            Directory.Move(processingPath + NewInfo.Idx, processingPath);
                                            StartProcess(p);
                                        }
                                        else
                                        {
                                            Auxiliar.ShowMessage(NewInfo.Idx, "File not fond: " + processingPath + "\nPlease delete the process and schedule it again.", "Error");
                                        }
                                    }
                                }
                            }

                            if (p.StepList[p.CurrentStepIndex].LastStatus.State == "COMPLETED")
                            {
                                Auxiliar.MoveCompletedIdx(p);
                                Auxiliar.SendLogRequest("IDX moved to Completed|" + NewInfo.Idx);
                            }
                        }
                    }
                }
                else
                {
                    if (Auxiliar.ProcessExists(NewInfo))
                    {
                        ServerSystem server = Auxiliar.SalServerList.Where(x => x.Hostname == NewInfo.PAS && x.SID == NewInfo.SID).FirstOrDefault();
                        
                        if (server == null)
                        {
                            server = new ServerSystem("GLOBAL", "GLB", "00", "AliceServer", "BRIDGE", "PRD", "", "", "CI", "Linux", NewInfo.DBType, "");
                        }

                        NewInfo.Environment = server.Environment;
                        
                        if (NewInfo.DBS != NewInfo.PAS)
                        {
                            ServerSystem dbserver = Auxiliar.SalServerList.Where(x => x.Hostname == NewInfo.DBS && x.SID == NewInfo.SID).FirstOrDefault();
                            if (dbserver != null)
                            {
                                NewInfo.DBType = dbserver.DBType;
                            }
                        }
                        else
                        {
                            NewInfo.DBType = server.DBType;
                        }

                        var processTemplate = Auxiliar.ProcessInitConfig.Where(x => x.ProjectName == NewInfo.ProjectName).FirstOrDefault();
                        
                        if (processTemplate != null && processTemplate.IsSecuential)
                        {
                            for (int i = 0; i < processTemplate.StepList.Count; i++)
                            {
                                NewInfo.StepList[i].ProcessSelectedFlow = NewInfo.SelectedFlowMode;
                                NewInfo.StepList[i].Flow = processTemplate.StepList[i].Flow;
                                if (processTemplate.StepList[i].MaxTries != null)
                                {
                                    NewInfo.StepList[i].MaxTries = processTemplate.StepList[i].MaxTries;
                                }
                            }
                        }

                        TransactionsPackage transactionsPackage = new TransactionsPackage();
                        
                        if (NewInfo.TransactionsPackages != null)
                        {
                            foreach (TransactionsPackage package in NewInfo.TransactionsPackages)
                            {
                                foreach (TransactionsPackage.Transaction transaction in package.Transactions)
                                {
                                    if (processTemplate.TransactionsPackages != null)
                                    {
                                        TransactionsPackage tempPackage = processTemplate.TransactionsPackages.Where(x => x.Transactions.Any(t => t.TCode == transaction.TCode)).FirstOrDefault();
                                        
                                        if (tempPackage != null)
                                        {
                                            TransactionsPackage.Transaction tempTransaction = tempPackage.Transactions.Where(x => x.TCode == transaction.TCode).FirstOrDefault();
                                            
                                            if (tempTransaction != null)
                                            {
                                                int postActIndex = transaction.TCode.IndexOf("RPOST");
                                                
                                                if (postActIndex != -1)
                                                {
                                                    tcode = transaction.TCode.Substring(0, transaction.TCode.Length - postActIndex);
                                                    prepost = "POST";
                                                }
                                                else
                                                {
                                                    if (transaction.TCode.EndsWith("R"))
                                                    {
                                                        tcode = transaction.TCode.Substring(0, transaction.TCode.Length - 1);
                                                        prepost = "PRE";
                                                    }
                                                    else
                                                    {
                                                        tcode = transaction.TCode;
                                                    }
                                                }
                                                description = tempTransaction.Description;
                                            }
                                        }
                                    }

                                    TransactionsPackage.Transaction t = new TransactionsPackage.Transaction() { Description = description, TCode = tcode, Selected = "", PrePost = prepost };
                                    
                                    if (transactionsPackage.Transactions == null)
                                    {
                                        transactionsPackage.Transactions = new List<TransactionsPackage.Transaction>();
                                    }

                                    transactionsPackage.Transactions.Add(t);

                                    tcode = "";
                                    description = "";
                                    prepost = "";
                                }
                            }
                        }
                        //a partir de aquí falta
                        NewInfo.SelectedTransactions = transactionsPackage;
                        Auxiliar.ApplyFilterToProcess(NewInfo);
                        
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            Processes.Add(NewInfo);
                        });
                    }
                }
            }
        }
        public async Task UpdateStatus(Process acProcess, Process jsonProcess, Step tempStep)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                acProcess.CurrentStep = tempStep;
                acProcess.Progress = jsonProcess.Progress;

                for (int i = 0; i < jsonProcess.StepList.Count; i++)
                {
                    if (acProcess.StepList[i].Status != jsonProcess.StepList[i].Status && !(jsonProcess.StepList[i].Status == "" || jsonProcess.StepList[i].Status == "Schedled"))
                        acProcess.StepList[i].AddStatus(jsonProcess.StepList[i].LastStatus);
                    acProcess.StepList[i].Log = jsonProcess.StepList[i].Log;
                }
            });
            processUpdFVal = true;
        }
        public int SelectedTab
        {
            get { return this._selectedTab; }
            set
            {
                this._selectedTab = value;
                this.OnPropertyChanged("SelectedTab");
            }
        }
        public ObservableCollection<EvidenceItem> Evidences
        {
            get { return this._evidence; }
            set
            {
                this._evidence = value;
                this.OnPropertyChanged("Evidences");
            }
        }
        public EvidenceItem SelectedEvidence
        {
            set
            {
                if (value != null)
                    OpenEvidence(value.FilePath);

                this.OnPropertyChanged("SelectedEvidence");
            }
        }
        public void OpenEvidence(string pathfile)
        {
            string url = pathfile;
            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            try
            {
                System.Diagnostics.Process.Start(psi);
            }
            catch
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    MessagePopup messagePopup = new MessagePopup("Unable to open file","error", "Automation Console");
                    messagePopup.Owner = System.Windows.Application.Current.MainWindow;
                    messagePopup.Topmost = true;
                    messagePopup.DataContext = MainWindow.PVMInstance;
                    messagePopup.Show();
                });

            }
        }
        public void ChangeToAddProcessVM(object obj)
        {
            MainWindow myWindow = System.Windows.Application.Current.MainWindow as MainWindow;
            myWindow.SetAddProcessDataContext();
        }
        public void ChangeToFavoritesVM(object obj)
        {
            MainWindow myWindow = System.Windows.Application.Current.MainWindow as MainWindow;
            myWindow.SetFavoritesDataContext();
        }
        public void OpenInfoView(object obj)
        {
            ProcessInfo processInfoWindow = new ProcessInfo();
            processInfoWindow.Owner = System.Windows.Application.Current.MainWindow;
            processInfoWindow.Show();
        }
        public bool CanOpenInfoView(object obj)
        {
            if (SelectedProcesses.ToList().Count == 1)
            {
                return true;
            }
            else
                return false;
        }
        public void OpenLogView(object obj)
        {
            Process p = obj as Process;

            LogTitle = p.SID + " " + p.Customer + " " + p.ProjectName + " " + p.SelectedStep.Name + "Log";
            LogText = p.SelectedStep.Log;

            Log logWindow = new Log();
            logWindow.Owner = System.Windows.Application.Current.MainWindow;
            logWindow.ShowDialog();
        }
        public bool CanOpenLogView(object obj)
        {
            Process p = obj as Process;
            if (p != null)
                if (p.SelectedStep != null)
                {
                    if ( (p.SelectedStep.Name.Contains("CHECKIN") && !p.ProjectName.ToUpper().Contains("CLOUD") ) || (p.SelectedStep.Name.Contains("CHECKOUT") && !p.ProjectName.ToUpper().Contains("CLOUD")) )
                        return false;
                    else
                        return true;
                }
                else
                    return false;
            else
                return false;
        }
        public void EditInfo(object obj)
        {

        }
        private void OpenEvidence(object obj)
        {
            Process p = obj as Process;
            Step s = p.SelectedStep;
            if (s.Evidence.Length >= 1)
            {
                SelectedTab = 1;

                if (s.Evidence.Length == 1)
                {
                    OpenEvidence(s.Evidence[0]);
                }
            }
        }
        public bool CanOpenEvidence(object obj)
        {
            Process p = obj as Process;

            if (p != null)
                if (p.SelectedStep != null)
                {
                    Step s = p.SelectedStep;
                    if (s.Evidence.Length > 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            else
                return false;
        }
        public void RefreshProcess(object obj)
        {
            string line;

            string monitoringPath = "\\\\10.130.19.40\\ait\\RUNTEAMCONSOLE\\AIT_" + UserProfile.ItUser;
            string completedPath = "\\\\10.130.19.40\\ait\\RUNTEAMCONSOLE\\AIT_COMPLETED";
            string fileToCheck = "ProcessList.ToProcess";
            string fileProcessing = "ProcessList.Processing";
            string fileProcessed = "ProcessList.Processed_" + DateTime.Now.ToString("yyyyMMddHHmmss");

            fileToCheck = monitoringPath + "\\" + fileToCheck;
            fileProcessing = monitoringPath + "\\" + fileProcessing;
            fileProcessed = completedPath + "\\" + fileProcessed;
            try
            {
                if (File.Exists(fileToCheck))
                {
                    File.Move(fileToCheck, fileProcessing);
                    StreamReader sr = new StreamReader(fileProcessing);
                    line = sr.ReadLine();
                    while (line != null && line.Count() > 10)
                    {
                        if (File.Exists(line))
                        {
                            try
                            {
                                ReplaceProcessInfo(line);
                                line = sr.ReadLine();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error trying replace process info");
                            }
                        }
                        else
                            line = sr.ReadLine();
                    }
                    //close the file
                    sr.Close();
                    Console.ReadLine();

                    File.Move(fileProcessing, fileProcessed);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error trying replace process info");
            }
        }
        public void ShowStartConfirmation(object obj)
        {
            confirmationWindow = new ituserWindow();
            confirmationWindow.Owner = System.Windows.Application.Current.MainWindow;
            confirmationWindow.DataContext = this;
            confirmationWindow.ShowDialog();
        }
        public void RequestUserAndPasswordToOpen()
        {
            confirmationWindow = new ituserWindow();
            confirmationWindow.Owner = System.Windows.Application.Current.MainWindow;
            confirmationWindow.DataContext = this;
            confirmationWindow.ShowDialog();
        }
        public bool CanShowStartConfirmation(object obj)
        {
            var ret = true;
            string processType = "", step = "", status = "", currentType, currentStep, currentStatus;
            if (DisplayedMessages == 0)
            {
                if (SelectedProcesses.ToList().Count > 0)
                {
                    Process layout = Auxiliar.ProcessInitConfig.Find(x => x.ProjectName == SelectedProcesses.First().ProjectName);
                    if (layout != null)
                    {
                        if (SelectedProcesses.ToList().Count == 1)
                        {
                            currentStatus = SelectedProcesses.First().CurrentStatus.ToUpper();
                            if (!(currentStatus == "SCHEDULED" || currentStatus == "ABORTED"))
                            {
                                ret = false;
                            }
                            else
                            {
                                Step currSTAT = SelectedProcesses.First().SelectedStep;

                                if (currentStatus == "ABORTED" && (SelectedProcesses.First().SelectedStep == null || SelectedProcesses.First().SelectedStep.LastStatus.State.ToUpper() == "SCHEDULED" ))
                                    ret = false;
                            }
                        }
                        else
                        {
                            foreach (Process p in SelectedProcesses.ToList())
                            {
                                currentStatus = p.CurrentStatus.ToUpper();
                                if (!(currentStatus == "SCHEDULED" || currentStatus == "ABORTED"))
                                {
                                    ret = false;
                                    break;
                                }
                                currentType = p.ProjectName.ToUpper(); ;
                                currentStep = p.CurrentStepName.ToUpper();
                                if (processType == "" && step == "" && status == "")
                                {
                                    processType = currentType;
                                    step = currentStep;
                                    status = currentStatus;
                                }
                                else
                                {
                                    if (currentType != processType || currentStep != step || currentStatus != status)
                                    {
                                        ret = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                        ret = false;
                }
                else
                    ret = false;
            }
            else
                ret = false;
            return ret;
        }
        public void StartProcess(object obj)
        {
            List<Process> processList;
            StatusExecution TempStatusExecution;
            
            string acStatus = "\\\\10.130.19.40\\ait\\RUNTEAMCONSOLE\\AC_STATUS\\";
            var statusFileName = this._ituser;//Environment.UserName.ToUpper() + "_*";
            var dir = new DirectoryInfo(acStatus);
            var ext = new List<string> { ".PROCESSING", ".DONE", ".FAILED", ".WARNING" };

            var myFiles = Directory
            .EnumerateFiles(acStatus, statusFileName)
            .Where(s => ext.Contains(System.IO.Path.GetExtension(s).ToUpper()));

            if (myFiles.Count() < counterProcesses)
            {
                try
                {
                    if (confirmationWindow != null)
                        confirmationWindow.Close();
                }
                catch { Console.WriteLine("no se quiere cerrar la ventanita >.<"); }

                DateTime now = Auxiliar.ConvertToEST();

                Type pr = obj.GetType();
                if (pr.Equals(typeof(Process)))
                    processList = new List<Process>() { (obj as Process) };
                else
                {
                    processList = new List<Process>();
                    foreach (Process p in (obj as ObservableCollection<Process>))
                    {
                        if (p.IsSelected)
                            processList.Add(p);
                    }
                }
                int filesCount = myFiles.Count();

                if(processList.Count() + myFiles.Count() <= counterProcesses )
                {
                    string preFile, idxFileName, idxFileContent, inputFileName, inputContent, processedFolder, stepName, currentStatus, processingPath;
                    string[] abortedFiles, outputFiles, toProcessFiles;
                    int stepIndex, currentStepIndex;

                    foreach (Process p in processList)
                    {
                        string acStatusFilePath = acStatus + UserProfile.ItUser.ToUpper() + "_" + p.Idx + "." + p.CurrentStatus;
                        string acStatusProcessing = acStatus + UserProfile.ItUser.ToUpper() + "_" + p.Idx + ".Processing";
                        string acFileContent = p.Idx + "|" + p.CurrentStatus + "|" + DateTime.Now.ToString("yyyyMMddHHmmss");
                                            
                        if(p.CurrentStatus == "Scheduled" || p.CurrentStatus == "ABORTED")
                        { 
                            File.Delete(acStatusFilePath);
                            File.WriteAllText(acStatusProcessing, acFileContent);
                            Auxiliar.createDispatcheFile(p.ProjectName);
                        }
                        currentStatus = p.CurrentStatus;
                        currentStepIndex = p.CurrentStepIndex;
                        processingPath = Auxiliar.GetProcessingPath(p.Idx);
                        //create folder AIT_INTER required by Orq!
                        Auxiliar.CreateFolder(Auxiliar.GetAitInterPath(p.ProjectName));

                        p.Credentials.OSUser = ValItuser;


                        if (!String.IsNullOrEmpty(OSPass))
                        {
                            if (String.IsNullOrEmpty(UserProfile.CachedCredentials.OSPass))
                                UserProfile.CachedCredentials.OSPass = Auxiliar.Base64_Encode(OSPass);
                            if (String.IsNullOrEmpty(p.Credentials.OSPass))
                                p.Credentials.OSPass = Auxiliar.Base64_Encode(OSPass);
                        }

                        preFile = Auxiliar.GetPrePath(p.ProjectName) + p.Idx + ".PRE";
                        if (File.Exists(preFile))
                            File.Delete(preFile);

                        if (Directory.Exists(processingPath))
                        {
                            abortedFiles = Directory.GetFiles(processingPath, "*.ABORTED");
                            outputFiles = Directory.GetFiles(processingPath, "*.OUTPUT");

                            toProcessFiles = abortedFiles.Concat(outputFiles).ToArray();
                            if (toProcessFiles.Length > 0)
                            {
                                processedFolder = Auxiliar.GetProcessedAbortPath(p.Idx);
                                foreach (string file in toProcessFiles)
                                {
                                    FileInfo fi = new FileInfo(file);
                                    File.Move(file, Auxiliar.CreateFolder(processedFolder) + fi.Name);
                                }
                            }
                        }
                        if (!Auxiliar.SubOrqIsOn(p))//In this code the IDX is generated
                        {
                            idxFileName = p.Idx + ".IDX";
                            string processPath = Auxiliar.GetProcessPath(p.Idx);
                            processPath = processPath.Remove(processPath.Length - 1);

                            if (Directory.Exists(processPath))
                            {
                                Auxiliar.WriteOSPassOnCredentialsFile(p.Idx, p.Credentials.OSPass, p.Credentials.OSUser);
                                if (File.Exists(Auxiliar.GetAitDonePath(p.ProjectName) + idxFileName))
                                    File.Delete(Auxiliar.GetAitDonePath(p.ProjectName) + idxFileName);
                                else
                                    Console.WriteLine("File not fond: " + Auxiliar.GetAitDonePath(p.ProjectName) + idxFileName);

                                if (p.ApplReq)
                                    idxFileContent = p.Customer + "|" + p.SID + "|" + p.PASType + "|" + p.PAS + "|" + p.InstanceNum + "|" + p.Credentials.SIDAdmUser + "|" + 
                                        p.Credentials.SIDAdmPass + "|" + p.Credentials.OSUser + "|" + p.Credentials.OSPass + "|000|" + p.DBSType + "|" + 
                                        Auxiliar.GetConfingPath(p.Idx) + "|\\AIT_TMP\\LOG\\TRACE\\|" + p.StepList[0].Name + "|" + p.ProjectName;
                                else
                                    idxFileContent = p.Customer + "|" + p.SID + p.PAS.Substring(p.PAS.Length - 3).ToUpper() + "|" + p.PASType + "|" + p.PAS + "|" + 
                                        p.InstanceNum + "|" + p.Credentials.SIDAdmUser + "|" + p.Credentials.SIDAdmPass + "|" + p.Credentials.OSUser + "|" + 
                                        p.Credentials.OSPass + "|000|" + p.DBSType + "|" + Auxiliar.GetConfingPath(p.Idx) + "|\\AIT_TMP\\LOG\\TRACE\\|" + 
                                        p.StepList[0].Name + "|" + p.ProjectName;
                                Auxiliar.CreateFile(Auxiliar.GetAitFilesPath(p.ProjectName), idxFileName, idxFileContent);

                                Auxiliar.SendLogRequest("Writing IDX|" + p.Idx);
                            }
                            else
                            {
                                if (Directory.Exists(processPath + p.Idx))
                                {
                                    Directory.Move(processPath + p.Idx, processPath);
                                    Auxiliar.WriteOSPassOnCredentialsFile(p.Idx, p.Credentials.OSPass, p.Credentials.OSUser);
                                    if (File.Exists(Auxiliar.GetAitDonePath(p.ProjectName) + idxFileName))
                                        File.Delete(Auxiliar.GetAitDonePath(p.ProjectName) + idxFileName);
                                    else
                                        Console.WriteLine("File not fond: " + Auxiliar.GetAitDonePath(p.ProjectName) + idxFileName);

                                    if (p.ApplReq)
                                        idxFileContent = p.Customer + "|" + p.SID + "|" + p.PASType + "|" + p.PAS + "|" + p.InstanceNum + "|" + p.Credentials.SIDAdmUser + 
                                            "|" +  p.Credentials.SIDAdmPass + "|" + p.Credentials.OSUser + "|" + p.Credentials.OSPass + "|000|" + p.DBSType + "|" + 
                                            Auxiliar.GetConfingPath(p.Idx) + "|\\AIT_TMP\\LOG\\TRACE\\|" + p.StepList[0].Name + "|" + p.ProjectName;
                                    else
                                        idxFileContent = p.Customer + "|" + p.SID + p.PAS.Substring(p.PAS.Length - 3).ToUpper() + "|" + p.PASType + "|" + p.PAS + "|" + 
                                            p.InstanceNum + "|" + p.Credentials.SIDAdmUser + "|" + p.Credentials.SIDAdmPass + "|" + p.Credentials.OSUser + "|" + 
                                            p.Credentials.OSPass + "|000|" + p.DBSType + "|" + Auxiliar.GetConfingPath(p.Idx) + "|\\AIT_TMP\\LOG\\TRACE\\|" + 
                                            p.StepList[0].Name + "|" + p.ProjectName;
                                    Auxiliar.CreateFile(Auxiliar.GetAitFilesPath(p.ProjectName), idxFileName, idxFileContent);

                                    Auxiliar.SendLogRequest("Writing IDX|" + p.Idx);
                                }
                                else
                                {
                                    Auxiliar.ShowMessage(p.Idx, "File not fond: " + processPath + "\nPlease delete the process and schedule it again.", "Error");
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Auxiliar.SendLogRequest("SubOrq is on already|" + p.Idx);
                        }

                        if (p.IsSecuential)
                        {
                            if (currentStatus == "Scheduled")
                            {
                                stepName = p.StepList[0].Name;
                                stepIndex = p.StepList[0].Index;

                                inputFileName = p.Idx + "_" + stepName + ".INPUT";
                                inputContent = p.Idx + "|" + p.SID + "|" + p.PAS + "|" + p.PASType + "|" + p.PASOS + "|" + p.DBS + "|" + p.DBSType + "|" + p.DBSOS + 
                                    "|itDummy" + "|passDUMMY" + "|" + p.Credentials.OSUser + "|" + p.Credentials.OSPass + "|" + p.Credentials.DBSchemaPass;
                                Auxiliar.CreateFile(processingPath, inputFileName, inputContent);

                                App.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    p.StepList[stepIndex].Status = "Processing";
                                    p.CurrentStep = p.StepList[0];
                                });

                                Auxiliar.SendLogRequest("Process started|" + p.Idx);
                                Step tempStep = p.StepList[0];
                                StepExecution TempStepExecution = Auxiliar.GetStepExecution(p.Idx, tempStep.Index);
                                if (TempStepExecution == null)
                                {
                                    TempStepExecution = new StepExecution { Idx = p.Idx, StepIndex = tempStep.Index, Name = tempStep.Name, Description = tempStep.Description, 
                                        Log = "", Message = "" };
                                    Auxiliar.PostProcessTraking(TempStepExecution, "StepExecutions/");
                                }
                                TempStatusExecution = new StatusExecution { Idx = p.Idx, StepIndex = stepIndex, State = "Processing", DateTime = now };
                                Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");

                                p.StepList[0].TryNum++;
                                DataAccess.ChangeStepConfig(p.Idx, stepName, "Try", p.StepList[0].TryNum.ToString());
                            }
                            else if (currentStatus == "ABORTED")
                            {
                                if (SelectedProcesses.ToList().Count == 1)
                                {
                                    Step selectedStep;
                                    if (p.SelectedStep != null)
                                    {
                                        stepName = p.SelectedStep.Name;
                                        stepIndex = p.SelectedStep.Index;
                                        selectedStep = p.SelectedStep;
                                    }
                                    else
                                    {
                                        stepName = p.CurrentStepName;
                                        stepIndex = p.CurrentStepIndex;
                                        selectedStep = p.CurrentStep;
                                    }
                                    inputFileName = p.Idx + "_" + stepName + ".INPUT";
                                    inputContent = p.Idx + "|" + p.SID + "|" + p.PAS + "|" + p.PASType + "|" + p.PASOS + "|" + p.DBS + "|" + p.DBSType + "|" + p.DBSOS + "|" + 
                                        p.Credentials.SAPGuiUser + "|" + p.Credentials.SAPGuiPass + "|" + p.Credentials.OSUser + "|" + p.Credentials.OSPass + "|" + 
                                        p.Credentials.DBSchemaPass;
                                    Auxiliar.CreateFile(processingPath, inputFileName, inputContent);

                                    Auxiliar.DeleteRealTimeStepLog(stepName, p.Idx);

                                    App.Current.Dispatcher.Invoke((Action)delegate
                                    {
                                        p.StepList[stepIndex].Status = "Processing";
                                        p.CurrentStep = selectedStep;
                                    });

                                    Auxiliar.SendLogRequest("Process started|" + p.Idx + "|" + stepName);
                                    TempStatusExecution = new StatusExecution { Idx = p.Idx, StepIndex = stepIndex, State = "Processing", DateTime = now };
                                    Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");

                                    p.StepList[stepIndex].TryNum++;
                                    DataAccess.ChangeStepConfig(p.Idx, stepName, "Try", p.StepList[stepIndex].TryNum.ToString());
                                }
                                else
                                {
                                    stepName = p.CurrentStepName;
                                    stepIndex = p.CurrentStepIndex;

                                    Auxiliar.DeleteRealTimeStepLog(stepName, p.Idx);

                                    inputFileName = p.Idx + "_" + stepName + ".INPUT";
                                    inputContent = p.Idx + "|" + p.SID + "|" + p.PAS + "|" + p.PASType + "|" + p.PASOS + "|" + p.DBS + "|" + p.DBSType + "|" + p.DBSOS + "|" + 
                                        p.Credentials.SAPGuiUser + "|" + p.Credentials.SAPGuiPass + "|" + p.Credentials.OSUser + "|" + p.Credentials.OSPass + "|" + 
                                        p.Credentials.DBSchemaPass;
                                    Auxiliar.CreateFile(processingPath, inputFileName, inputContent);

                                    App.Current.Dispatcher.Invoke((Action)delegate
                                    {
                                        p.StepList[stepIndex].Status = "Processing";
                                        p.CurrentStep = p.CurrentStep;
                                    });

                                    Auxiliar.SendLogRequest("Process started|" + p.Idx + "|" + stepName);
                                    TempStatusExecution = new StatusExecution { Idx = p.Idx, StepIndex = stepIndex, State = "Processing", DateTime = now };
                                    Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");

                                    p.StepList[stepIndex].TryNum++;
                                    DataAccess.ChangeStepConfig(p.Idx, stepName, "Try", p.StepList[stepIndex].TryNum.ToString());
                                }

                            }

                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                p.CurrentStep.AddStatus(new Status("Processing", now));
                                p.RealTimeLog = "Processing...";
                                p.CurrentStep = p.CurrentStep;
                            });

                        }
                        else
                        {
                            foreach (Step step in p.StepList)
                            {
                                if (step.Index == 0)
                                {
                                    if (step.Status != "DONE")
                                    {
                                        inputFileName = Auxiliar.GetProcessingPath(p.Idx) + p.Idx + "_" + step.Name + ".INPUT";
                                        inputContent = p.Idx + "|" + p.SID + "|" + p.PAS + "|" + p.PASType + "|" + p.PASOS + "|" + p.DBS + "|" + p.DBSType + "|" + p.DBSOS + 
                                            "|" + p.Credentials.SAPGuiUser + "|" + p.Credentials.SAPGuiPass + "|" + p.Credentials.OSUser + "|" + p.Credentials.OSPass + "|" + 
                                            p.Credentials.DBSchemaPass;
                                        File.WriteAllText(inputFileName, inputContent);

                                        App.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            step.AddStatus(new Status("Processing", now));
                                            p.CurrentStep = step;
                                        });

                                        Auxiliar.SendLogRequest("Process started|" + p.Idx);
                                        TempStatusExecution = new StatusExecution { Idx = p.Idx, StepIndex = step.Index, State = "Processing", DateTime = now };
                                        Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");
                                    }
                                }
                                else if (step.Index == p.StepList.Last().Index)
                                {
                                    bool canFinish = true;
                                    DateTime lastStatus = new DateTime(0);
                                    for (int i = 0; i < p.StepList.Count - 1; i++)
                                    {
                                        if (p.StepList[i].Status == "" || p.StepList[i].Status == "Processing" || p.StepList[i].Status == "Scheduled")
                                        {
                                            canFinish = false;
                                            break;
                                        }
                                        if (p.StepList[i].LastStatus.DateTime > lastStatus)
                                        {
                                            lastStatus = p.StepList[i].LastStatus.DateTime;
                                        }
                                    }
                                    if (canFinish)
                                    {
                                        if (lastStatus > step.LastStatus.DateTime)
                                        {
                                            inputFileName = p.Idx + "_" + step.Name + ".INPUT";
                                            inputContent = p.Idx + "|" + p.SID + "|" + p.PAS + "|" + p.PASType + "|" + p.PASOS + "|" + p.DBS + "|" + p.DBSType + "|" +
                                                p.DBSOS + "|" + p.Credentials.SAPGuiUser + "|" + p.Credentials.SAPGuiPass + "|" + p.Credentials.OSUser + "|" + 
                                                p.Credentials.OSPass + "|" + p.Credentials.DBSchemaPass;
                                            Auxiliar.CreateFile(processingPath, inputFileName, inputContent);

                                            App.Current.Dispatcher.Invoke((Action)delegate
                                            {
                                                step.AddStatus(new Status("Processing", now));
                                                p.CurrentStep = step;
                                            });

                                            Auxiliar.SendLogRequest("Process no secuential continue|" + p.Idx + "|step index " + step.Index + "|step name " + step.Name);
                                            TempStatusExecution = new StatusExecution { Idx = p.Idx, StepIndex = step.Index, State = "Processing", DateTime = now };
                                            Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");
                                        }
                                    }
                                }
                                else
                                {
                                    if (step.PreviousStep != "")
                                    {
                                        bool canExec = true;
                                        Step prev;
                                        string[] prevIndexes = step.PreviousStep.Split("|");
                                        foreach (string i in prevIndexes)
                                        {
                                            int index = Int32.Parse(i);
                                            prev = p.StepList.Where(x => x.Index == index).FirstOrDefault();
                                            if (prev != null)
                                            {
                                                if (prev.Status != "DONE" || !(step.Status == "" || step.Status == "Scheduled"))
                                                {
                                                    canExec = false;
                                                }
                                            }
                                        }
                                        if (canExec)
                                        {
                                            inputFileName = p.Idx + "_" + step.Name + ".INPUT";
                                            inputContent = p.Idx + "|" + p.SID + "|" + p.PAS + "|" + p.PASType + "|" + p.PASOS + "|" + p.DBS + "|" + p.DBSType + "|" + 
                                                p.DBSOS + "|" + p.Credentials.SAPGuiUser + "|" + p.Credentials.SAPGuiPass + "|" + p.Credentials.OSUser + "|" + 
                                                p.Credentials.OSPass + "|" + p.Credentials.DBSchemaPass;
                                            Auxiliar.CreateFile(processingPath, inputFileName, inputContent);

                                            App.Current.Dispatcher.Invoke((Action)delegate
                                            {
                                                step.AddStatus(new Status("Processing", now));
                                                p.CurrentStep = step;
                                            });

                                            Auxiliar.SendLogRequest("Process no secuential continue|" + p.Idx + "|step index " + step.Index + "|step name " + step.Name);
                                            TempStatusExecution = new StatusExecution { Idx = p.Idx, StepIndex = step.Index, State = "Processing", DateTime = now };
                                            Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("prevstep null");
                                    }
                                }
                            }

                        }
                        if (SelectedProcesses.ToList().Count > 1)
                            p.IsSelected = false;
                    }
                }

                else
                    Auxiliar.ShowMessage("Currently are or will be 80 processes running, please wait til one of them finishes to execute any other process.", "Error");
            }
            else
                Auxiliar.ShowMessage("Currently are or will be 80 processes running, please wait til one of them finishes to execute any other process.", "Error");
            
        }
        public bool CanStart(object obj)
        {
            var ret = true;
            if (OSPass.Length > 0)
                ret = true;
            else
                ret = false;
            return ret;
        }
        public void ContinueSelectedProcesses(object obj)
        {
            foreach (Process p in SelectedProcesses.ToList())
            {
                if (p.OptionsMessage != null)
                {
                    Auxiliar.ShowOptionsMessage(p.Idx, p.OptionsMessage);
                }
                else
                {
                    string processPath = Auxiliar.GetProcessPath(p.Idx);
                    processPath = processPath.Remove(processPath.Length - 1);
                    if (Directory.Exists(processPath))
                    {
                        ContinueProcess(p);
                        if (SelectedProcesses.ToList().Count > 1)
                            p.IsSelected = false;
                    }
                    else
                    {
                        if (Directory.Exists(processPath + p.Idx))
                        {
                            Directory.Move(processPath + p.Idx, processPath);
                            ContinueProcess(p);
                        }
                        else
                        {
                            Auxiliar.ShowMessage(p.Idx, "File not fond: " + processPath + "\nPlease delete the process and schedule it again.", "Error");
                        }
                    }

                }
            }
        }
        public void OptionContinueProcess(object obj)
        {
            OptionsMessageWindow optWindow;
            string idx = "";
            Process p;
            if (obj != null)
            {
                optWindow = obj as OptionsMessageWindow;
                TextBox idxTexBox = optWindow.FindName("idxTextBox") as TextBox;
                idx = idxTexBox.Text;
                optWindow.allowClosing = true;
                optWindow.Close();
                if (!String.IsNullOrEmpty(idx))
                {
                    p = Processes.Where(x => x.Idx == idx).FirstOrDefault();
                    if (p != null)
                    {
                        string processingPath = Auxiliar.GetProcessingPath(p.Idx);
                        if (Directory.Exists(processingPath))
                            ContinueProcess(p);
                        else
                        {
                            if (Directory.Exists(processingPath + p.Idx))
                            {
                                Directory.Move(processingPath + p.Idx, processingPath);
                                ContinueProcess(p);
                            }
                            else
                            {
                                Auxiliar.ShowMessage(p.Idx, "File not fond: " + processingPath + "\nPlease delete the process and schedule it again.", "Error");
                            }
                        }
                    }
                }
            }

        }
        public void ContinueProcess(Process p)
        {
            string statusFileName, continueFileName, stopFileName, processingFolder, processedFolder;
            StatusExecution TempStatusExecution;
            int index;

            DateTime now = Auxiliar.ConvertToEST();
            processingFolder = Auxiliar.GetProcessingPath(p.Idx);

            if (p.IsSecuential)
            {
                index = p.StepList.IndexOf(p.StepList.Where(x => x.Name == p.CurrentStepName).FirstOrDefault());

                if (p.StepList[index + 1].FlowMatch)
                {
                    processedFolder = Auxiliar.GetProcessedPath(p.Idx);
                    statusFileName = p.Idx + "_" + p.CurrentStepName + "." + p.CurrentStatus;
                    continueFileName = p.Idx + "_" + p.CurrentStepName + ".CONTINUE";
                    stopFileName = p.Idx + "_" + p.CurrentStepName + ".STOP";

                    if (File.Exists(processingFolder + statusFileName))
                    {
                        if (p.CurrentStatus == "WARNING")
                        {
                            Auxiliar.CreateFolder(processedFolder);
                            if (File.Exists(processedFolder + statusFileName))
                            {
                                File.Delete(processedFolder + statusFileName);
                            }
                            File.Copy(processingFolder + statusFileName, processedFolder + statusFileName);
                        }

                        File.Move(processingFolder + statusFileName, processingFolder + continueFileName);
                        Auxiliar.SendLogRequest("Process secuential continue|" + p.Idx + "|step index " + index + "|step name " + p.CurrentStepName + "|status " + p.CurrentStatus);

                        Step tempStep = p.StepList[index + 1];
                        StepExecution TempStepExecution = Auxiliar.GetStepExecution(p.Idx, tempStep.Index);

                        if (TempStepExecution == null)
                        {
                            TempStepExecution = new StepExecution { Idx = p.Idx, StepIndex = tempStep.Index, Name = tempStep.Name, Description = tempStep.Description, Log = "", Message = "" };
                            Auxiliar.PostProcessTraking(TempStepExecution, "StepExecutions/");
                        }
                        
                        TempStatusExecution = new StatusExecution { Idx = p.Idx, StepIndex = index + 1, State = "Processing", DateTime = now };
                        Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");

                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            p.StepList[index + 1].AddStatus(new Status("Processing", now));
                            p.CurrentStep = p.StepList[index + 1];
                            p.RealTimeLog = "Processing...";
                        });

                        p.StepList[index + 1].TryNum++;
                        DataAccess.ChangeStepConfig(p.Idx, p.CurrentStepName, "Try", p.StepList[index + 1].TryNum.ToString());
                    
                    }
                    else if(File.Exists(processingFolder + stopFileName))
                    {
                        Step tempStep = p.StepList[index + 1];
                        StepExecution TempStepExecution = Auxiliar.GetStepExecution(p.Idx, tempStep.Index);

                        if (TempStepExecution == null)
                        {
                            TempStepExecution = new StepExecution { Idx = p.Idx, StepIndex = tempStep.Index, Name = tempStep.Name, Description = tempStep.Description, Log = "", Message = "" };
                            Auxiliar.PostProcessTraking(TempStepExecution, "StepExecutions/");
                        }

                        TempStatusExecution = new StatusExecution { Idx = p.Idx, StepIndex = index + 1, State = "Processing", DateTime = now };
                        Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");

                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            p.StepList[index + 1].AddStatus(new Status("Processing", now));
                            p.CurrentStep = p.StepList[index + 1];
                            p.RealTimeLog = "Processing...";
                        });

                        p.StepList[index + 1].TryNum++;
                        DataAccess.ChangeStepConfig(p.Idx, p.CurrentStepName, "Try", p.StepList[index + 1].TryNum.ToString());
                    }    
                    /*else //File Not Found 
                    {
                        Auxiliar.SendLogRequest("Process secuential try and fail to continue from AC side. Trying from server side|" + p.Idx + "|step index " + index + "|step name " + p.CurrentStepName + "|status " + p.CurrentStatus);
                        WebRequest request = WebRequest.Create(Auxiliar.serverURL + "ConsoleActions/Continue?idx=" + p.Idx + "&stepName=" + p.CurrentStepName + "&status=" + p.CurrentStatus);
                        request.Credentials = CredentialCache.DefaultCredentials;
                        try
                        {
                            WebResponse response = request.GetResponse();
                            if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                            {
                                Step tempStep = p.StepList[index + 1];
                                StepExecution TempStepExecution = Auxiliar.GetStepExecution(p.Idx, tempStep.Index);

                                if (TempStepExecution == null)
                                {
                                    TempStepExecution = new StepExecution { Idx = p.Idx, StepIndex = tempStep.Index, Name = tempStep.Name, Description = tempStep.Description, Log = "", Message = "" };
                                    Auxiliar.PostProcessTraking(TempStepExecution, "StepExecutions/");
                                }

                                TempStatusExecution = new StatusExecution { Idx = p.Idx, StepIndex = index + 1, State = "Processing", DateTime = now };
                                Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");

                                App.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    p.StepList[index + 1].AddStatus(new Status("Processing", now));
                                    p.CurrentStep = p.StepList[index + 1];
                                    p.RealTimeLog = "Processing...";
                                });

                                p.StepList[index + 1].TryNum++;
                                DataAccess.ChangeStepConfig(p.Idx, p.CurrentStepName, "Try", p.StepList[index + 1].TryNum.ToString());
                            }
                            else
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    MessagePopup messagePopup = new MessagePopup("Http Web Response FAILURE, Error Code: \n" + ((HttpWebResponse)response).StatusCode, "error", p.Idx);
                                    messagePopup.Owner = System.Windows.Application.Current.MainWindow;
                                    messagePopup.Topmost = true;
                                    messagePopup.DataContext = this;
                                    messagePopup.Show();
                                });
                            }
                        }
                        catch (Exception e)
                        {
                            request = WebRequest.Create(Auxiliar.serverBackupURL + "ConsoleActions/Continue?idx=" + p.Idx + "&stepName=" + p.CurrentStepName + "&status=" + p.CurrentStatus);
                            try
                            {
                                WebResponse response = request.GetResponse();
                                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                                {
                                    Step tempStep = p.StepList[index + 1];
                                    StepExecution TempStepExecution = Auxiliar.GetStepExecution(p.Idx, tempStep.Index);

                                    if (TempStepExecution == null)
                                    {
                                        TempStepExecution = new StepExecution { Idx = p.Idx, StepIndex = tempStep.Index, Name = tempStep.Name, Description = tempStep.Description, Log = "", Message = "" };
                                        Auxiliar.PostProcessTraking(TempStepExecution, "StepExecutions/");
                                    }

                                    TempStatusExecution = new StatusExecution { Idx = p.Idx, StepIndex = index + 1, State = "Processing", DateTime = now };
                                    Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");

                                    App.Current.Dispatcher.Invoke((Action)delegate
                                    {
                                        p.StepList[index + 1].AddStatus(new Status("Processing", now));
                                        p.CurrentStep = p.StepList[index + 1];
                                        p.RealTimeLog = "Processing...";
                                    });

                                    p.StepList[index + 1].TryNum++;
                                    DataAccess.ChangeStepConfig(p.Idx, p.CurrentStepName, "Try", p.StepList[index + 1].TryNum.ToString());
                                }
                                else
                                {
                                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                                    {
                                        MessagePopup messagePopup = new MessagePopup("Http Web Response FAILURE, Error Code: \n" + ((HttpWebResponse)response).StatusCode, "error", p.Idx);
                                        messagePopup.Owner = System.Windows.Application.Current.MainWindow;
                                        messagePopup.Topmost = true;
                                        messagePopup.DataContext = this;
                                        messagePopup.Show();
                                    });
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    MessagePopup messagePopup = new MessagePopup("Exception Occurred: \n" + ex, "error", p.Idx);
                                    messagePopup.Owner = System.Windows.Application.Current.MainWindow;
                                    messagePopup.Topmost = true;
                                    messagePopup.DataContext = this;
                                    messagePopup.Show();
                                });
                            }
                        }
                    }
                */
                }
                else
                {
                    AlternStep(p);
                }
            }
            else
            {
                if (Directory.Exists(processingFolder))
                { 
                    StartProcess(p);
                }
                else
                {
                    if (Directory.Exists(processingFolder + p.Idx))
                    {
                        Directory.Move(processingFolder + p.Idx, processingFolder);
                        StartProcess(p);
                    }
                    else
                    {
                        Auxiliar.ShowMessage(p.Idx, "File not fond: " + processingFolder + "\nPlease delete the process and schedule it again.", "Error");
                    }
                }
            }
        }
        public int NextFlowMatchingStepIndex(Process p)
        {
            int output = -1;
            string flow = p.StepList[p.CurrentStepIndex].Flow;
            for (int i = p.CurrentStepIndex +1; i < p.StepList.Count; i++)
            {
                if(p.StepList[i].Flow == flow)
                {
                    output = i;
                    break;
                }
            }

            return output;
        }
        public bool CanContinue(object obj)
        {
            var ret = true;
            int index;
            string processType = "", step = "", status = "", currentType, currentStep, currentStatus;

            if (DisplayedMessages == 0)
            {
                if (SelectedProcesses.ToList().Count > 0)
                {
                    Process layout = Auxiliar.ProcessInitConfig.Find(x => x.ProjectName == SelectedProcesses.First().ProjectName);
                    if (layout != null)
                    {
                        foreach (Process p in SelectedProcesses.ToList())
                        {
                            if (SelectedProcesses.First().IsSecuential == true)
                            {
                                index = p.CurrentStepIndex;
                                if (index < p.StepList.Count() - 1)
                                {
                                    currentStatus = p.CurrentStatus;
                                    if (!(currentStatus == "DONE" || currentStatus == "WARNING"))
                                    {
                                        ret = false;
                                        break;
                                    }
                                    currentType = p.ProjectName.ToUpper(); ;
                                    currentStep = p.CurrentStepName.ToUpper();
                                    if (processType == "" && step == "" && status == "")
                                    {
                                        processType = currentType;
                                        step = currentStep;
                                        status = currentStatus;
                                    }
                                    else
                                    {
                                        if (currentType != processType || currentStep != step || currentStatus != status)
                                        {
                                            ret = false;
                                            break;
                                        }
                                    }

                                }
                                else
                                    ret = false;
                            }
                            else
                            {
                                if (p.CurrentStatus == "Scheduled" || p.CurrentStatus.Contains("ABORT") || p.CurrentStatus == "Processing" || p.CurrentStatus == "FAIL")
                                    ret = false;
                                else
                                {
                                    if (!p.StepList.Any(x => x.LastStatus.State == "Scheduled"))
                                        ret = false;
                                }
                            }
                        }
                    }
                    else
                        ret = false;
                }
                else
                    ret = false;
            }
            else
                ret = false;
            return ret;
        }
        public void SolveManually(object obj)
        {
            Process p;
            try
            {
                if (maxTriesW != null)
                    maxTriesW.Close();
            }
            catch
            { Console.WriteLine("no se quiere cerrar la ventanita >.<"); }

            if (obj != null)
                p = obj as Process;
            else
                p = SelectedProcesses.ToList()[0];

            AlternStep(p);

            if (p.StepList[p.CurrentStepIndex].Flow == "AUTO") {
                p.SelectedFlowMode = "MANUAL";
                Auxiliar.SendLogRequest("Process proceeds to change restore mode to MANUAL|" + p.Idx);
                DataAccess.ChangeMAnualRestoreonfig(p.Idx);
            }
        }
        public void RepeatAltern(object obj)
        {
            Process p;
            if (obj != null)
                p = obj as Process;
            else
                p = SelectedProcesses.ToList()[0];

            if (p.OptionsMessage != null)
            {
                Auxiliar.ShowOptionsMessage(p.Idx, p.OptionsMessage);
                return;
            }

            Step step;
            DateTime now = Auxiliar.ConvertToEST();
            string statusFileName, processingFolder, processedFolder, idxFileName, idxFileContent, inputFileName, inputContent, outputFilename, completeFileName, exitFilename;

            processingFolder = Auxiliar.GetProcessingPath(p.Idx);
            processedFolder = Auxiliar.GetProcessedPath(p.Idx);

            if (p.IsSecuential)
            {
                statusFileName = p.Idx + "_" + p.CurrentStepName + "." + p.CurrentStatus;
                step = p.StepList.Where(x => x.Index == p.CurrentStepIndex).FirstOrDefault();

                if (step.AlternStep != -1)
                {
                    if (step.MaxTries == null)
                    {
                        AlternStep(p);
                    }
                    else
                    {
                        if (step.MaxTries > step.TryNum)
                        { 
                            RepeatStep(p);
                        }
                        else
                        {
                            maxTriesW = new MaxTriesWindow();
                            maxTriesW.Owner = System.Windows.Application.Current.MainWindow;
                            maxTriesW.DataContext = this;
                            maxTriesW.Title = step.Description;
                            maxTriesW.Show();
                        }
                    }
                }
                else
                {
                    RepeatStep(p);
                }
            }
            else
            {
                step = (obj as Step);

                int index = p.StepList.IndexOf(step);

                statusFileName = p.Idx + "_" + step.Name + "." + step.Status;
                outputFilename = p.Idx + "_" + step.Name + ".OUTPUT";

                if (File.Exists(processingFolder + statusFileName))
                {
                    if (File.Exists(processedFolder + statusFileName))
                    {
                        File.Move(processedFolder + statusFileName, processedFolder + statusFileName + Auxiliar.GenerateTimeStamp(Auxiliar.ConvertToEST()));
                    }

                    File.Move(processingFolder + statusFileName, processedFolder + statusFileName);
                }
                
                if (File.Exists(processingFolder + outputFilename))
                {
                    File.Delete(processingFolder + outputFilename);
                }

                Step lastStep = p.StepList.Last();

                completeFileName = p.Idx + "_" + lastStep.Name + ".COMPLETED";
                exitFilename = p.Idx + "_" + lastStep.Name + ".EXIT";

                if (File.Exists(processingFolder + completeFileName))
                {
                    File.Delete(processingFolder + completeFileName);
                }

                if (File.Exists(processingFolder + exitFilename))
                {
                    File.Delete(processingFolder + exitFilename);
                }

                if (!Auxiliar.SubOrqIsOn(p))
                {
                    idxFileName = p.Idx + ".IDX";

                    if (File.Exists(Auxiliar.GetAitDonePath(p.ProjectName) + idxFileName))
                    {
                        File.Delete(Auxiliar.GetAitDonePath(p.ProjectName) + idxFileName);
                    }
                    else
                    {
                        Console.WriteLine("File not fond: " + Auxiliar.GetAitDonePath(p.ProjectName) + idxFileName);
                    }

                    idxFileContent = p.Customer + "|" + p.SID + "|" + p.PASType + "|" + p.PAS + "|" + p.InstanceNum + "|" + p.Credentials.SIDAdmUser + "|" + p.Credentials.SIDAdmPass + "|" + 
                        p.Credentials.OSUser + "|" + p.Credentials.OSPass + "|000|" + p.DBSType + "|" + Auxiliar.GetConfingPath(p.Idx) + "|\\AIT_TMP\\LOG\\TRACE\\|" + p.StepList[0].Name + "|" + 
                        p.ProjectName;
                    File.WriteAllText(Auxiliar.GetAitFilesPath(p.ProjectName) + idxFileName, idxFileContent);

                }

                Auxiliar.DeleteRealTimeStepLog(step.Name, p.Idx);

                inputFileName = p.Idx + "_" + step.Name + ".INPUT";
                inputContent = p.Idx + "|" + p.SID + "|" + p.PAS + "|" + p.PASType + "|" + p.PASOS + "|" + p.DBS + "|" + p.DBSType + "|" + p.DBSOS + "|" + p.Credentials.SAPGuiUser + "|" + 
                    p.Credentials.SAPGuiPass + "|" + p.Credentials.OSUser + "|" + p.Credentials.OSPass + "|" + p.Credentials.DBSchemaPass;
                Auxiliar.CreateFile(processingFolder, inputFileName, inputContent);

                Auxiliar.SendLogRequest("Process proceeds to repeat step|" + p.Idx + "|step index" + p.CurrentStepIndex + "|step " + p.CurrentStepName + "|status " + p.CurrentStatus);

                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    step.AddStatus(new Status("Processing", now));
                    p.CurrentStep = step;
                });
            }
        }
        public void OptionRepeatProcess(object obj)
        {
            OptionsMessageWindow optWindow;
            string idx = "";
            Process p;
            if (obj != null)
            {
                optWindow = obj as OptionsMessageWindow;
                TextBox idxTexBox = optWindow.FindName("idxTextBox") as TextBox;
                idx = idxTexBox.Text;
                optWindow.allowClosing = true;
                optWindow.Close();
                if (!String.IsNullOrEmpty(idx))
                {
                    p = Processes.Where(x => x.Idx == idx).FirstOrDefault();
                    if (p != null)
                        RepeatStep(p);
                }
            }

        }
        public void RepeatStep(object obj)
        {
            Process p;
            try
            {
                if (maxTriesW != null)
                    maxTriesW.Close();
            }
            catch 
            { Console.WriteLine("no se quiere cerrar la ventanita >.<"); }

            if (obj != null)
                p = obj as Process;
            else
                p = SelectedProcesses.ToList()[0];

            Step step;
            StatusExecution TempStatusExecution;
            DateTime now = Auxiliar.ConvertToEST();
            string statusFileName, processingFolder, processedFolder, actionFilename;

            processingFolder = Auxiliar.GetProcessingPath(p.Idx);
            processedFolder = Auxiliar.GetProcessedPath(p.Idx);

            statusFileName = p.Idx + "_" + p.CurrentStepName + "." + p.CurrentStatus;
            actionFilename = p.Idx + "_" + p.CurrentStepName + ".REPEAT";

            step = p.StepList.Where(x => x.Index == p.CurrentStepIndex).FirstOrDefault();
            int index = p.StepList.IndexOf(step);

            if (File.Exists(processingFolder + statusFileName))
            {
                if (!File.Exists(processedFolder + statusFileName))
                {
                    Auxiliar.CreateFolder(processedFolder);
                    File.Copy(processingFolder + statusFileName, processedFolder + statusFileName);
                }

                Auxiliar.DeleteRealTimeStepLog(step.Name, p.Idx);
                File.Move(processingFolder + statusFileName, processingFolder + actionFilename);

                Auxiliar.SendLogRequest("Process proceeds to repeat step|" + p.Idx + "|step index" + p.CurrentStepIndex + "|step " + p.CurrentStepName + "|status " + p.CurrentStatus);

                TempStatusExecution = new StatusExecution { Idx = p.Idx, StepIndex = p.CurrentStepIndex, State = "Processing", DateTime = now };
                Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");

                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    p.StepList[index].AddStatus(new Status("Processing", now));
                    p.CurrentStep = p.StepList[index];
                });

                step.TryNum++;
                DataAccess.ChangeStepConfig(p.Idx, step.Name, "Try", step.TryNum.ToString());
            }
            else //File Not Found 
            {
                Auxiliar.SendLogRequest("Process secuential try and fail to repeat from AC side. Trying from server side|" + p.Idx + "|step index " + index + "|step name " + 
                    p.CurrentStepName + "|status " + p.CurrentStatus);
                WebRequest request = WebRequest.Create(Auxiliar.serverURL + "ConsoleActions/Repeat?idx=" + p.Idx + "&stepName=" + p.CurrentStepName + "&status=" + p.CurrentStatus);
                request.Credentials = CredentialCache.DefaultCredentials;
                try
                {
                    WebResponse response = request.GetResponse();
                    if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                    {
                        TempStatusExecution = new StatusExecution { Idx = p.Idx, StepIndex = p.CurrentStepIndex, State = "Processing", DateTime = now };
                        Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");

                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            p.StepList[index].AddStatus(new Status("Processing", now));
                            p.CurrentStep = p.StepList[index];
                        });

                        step.TryNum++;
                        DataAccess.ChangeStepConfig(p.Idx, step.Name, "Try", step.TryNum.ToString());
                    }
                    else
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                            MessagePopup messagePopup = new MessagePopup("Http Web Response FAILURE, Error Code: \n" + ((HttpWebResponse)response).StatusCode, "error", p.Idx);
                            messagePopup.Owner = System.Windows.Application.Current.MainWindow;
                            messagePopup.Topmost = true;
                            messagePopup.DataContext = this;
                            messagePopup.Show();
                        });
                    }
                }
                catch (Exception e)
                {
                    request = WebRequest.Create(Auxiliar.serverBackupURL + "ConsoleActions/Repeat?idx=" + p.Idx + "&stepName=" + p.CurrentStepName + "&status=" + p.CurrentStatus);
                    try
                    {
                        WebResponse response = request.GetResponse();
                        if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                        {
                            TempStatusExecution = new StatusExecution { Idx = p.Idx, StepIndex = p.CurrentStepIndex, State = "Processing", DateTime = now };
                            Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");

                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                p.StepList[index].AddStatus(new Status("Processing", now));
                                p.CurrentStep = p.StepList[index];
                            });

                            step.TryNum++;
                            DataAccess.ChangeStepConfig(p.Idx, step.Name, "Try", step.TryNum.ToString());
                        }
                        else
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                                MessagePopup messagePopup = new MessagePopup("Http Web Response FAILURE, Error Code: \n" + ((HttpWebResponse)response).StatusCode, "error", p.Idx);
                                messagePopup.Owner = System.Windows.Application.Current.MainWindow;
                                messagePopup.Topmost = true;
                                messagePopup.DataContext = this;
                                messagePopup.Show();
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                            MessagePopup messagePopup = new MessagePopup("An Exception has been found: \n" + ex.Message, "error", p.Idx);
                            messagePopup.Owner = System.Windows.Application.Current.MainWindow;
                            messagePopup.Topmost = true;
                            messagePopup.DataContext = this;
                            messagePopup.Show();
                        });
                    }
                }
            }
        }
        public void OptionAlternProcess(object obj)
        {
            OptionsMessageWindow optWindow;
            string idx = "";
            Process p;
            if (obj != null)
            {
                optWindow = obj as OptionsMessageWindow;
                TextBox idxTexBox = optWindow.FindName("idxTextBox") as TextBox;
                idx = idxTexBox.Text;
                optWindow.allowClosing = true;
                optWindow.Close();
                if (!String.IsNullOrEmpty(idx))
                {
                    p = Processes.Where(x => x.Idx == idx).FirstOrDefault();
                    if (p != null)
                        AlternStep(p);
                }
            }

        }
        public void AlternStep(object obj)
        {
            Process p;
            if (obj != null)
                p = obj as Process;
            else
                p = SelectedProcesses.ToList()[0];

            StatusExecution TempStatusExecution;
            Step step = p.StepList.Where(x => x.Index == p.CurrentStepIndex).FirstOrDefault();
            Step alternStep = p.StepList.Where(x => x.Index == step.AlternStep).FirstOrDefault();

            ///////////////////////////
            StepExecution TempStepExecution = Auxiliar.GetStepExecution(p.Idx, alternStep.Index);
            if (TempStepExecution == null)
            {
                TempStepExecution = new StepExecution { Idx = p.Idx, StepIndex = alternStep.Index, Name = alternStep.Name, Description = alternStep.Description, Log = "", Message = "" }; 
                Auxiliar.PostProcessTraking(TempStepExecution, "StepExecutions/");
            }
            ///////////////////////////
            
            DateTime now = Auxiliar.ConvertToEST();

            string processingFolder = Auxiliar.GetProcessingPath(p.Idx);
            string processedFolder = Auxiliar.GetProcessedPath(p.Idx);

            string statusFileName = p.Idx + "_" + p.CurrentStepName + "." + p.CurrentStatus;
            string alternFilename = p.Idx + "_" + p.CurrentStepName + ".ALTERN";

            int alternIndex = (int)p.StepList[step.Index].AlternStep;

            Auxiliar.DeleteRealTimeStepLog(alternStep.Name, p.Idx);

            if (File.Exists(processingFolder + statusFileName))
            {
                File.Delete(processingFolder + statusFileName);
                Auxiliar.CreateFile(processingFolder, alternFilename, alternStep.Index.ToString());

                Auxiliar.SendLogRequest("Process proceeds to altern step|" + p.Idx + "|step index" + p.CurrentStepIndex + "|step " + p.CurrentStepName + "|status " + p.CurrentStatus + 
                    "|altern step index" + alternIndex + "|altern step " + alternStep.Name);

                TempStatusExecution = new StatusExecution { Idx = p.Idx, StepIndex = alternIndex, State = "Processing", DateTime = now };
                Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");

                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    alternStep.AddStatus(new Status("Processing", now));
                    p.CurrentStep = alternStep;
                });

                step.TryNum++;
                DataAccess.ChangeStepConfig(p.Idx, step.Name, "Try", step.TryNum.ToString());
            }
            else //File Not Found 
            {
                Auxiliar.SendLogRequest("Process secuential try and fail to go Altern step from AC side. Trying from server side|" + p.Idx + "|step index " + p.CurrentStepIndex + 
                    "|step name " + p.CurrentStepName + "|status " + p.CurrentStatus + "|altern step index" + alternIndex + "|altern step " + alternStep.Name);
                WebRequest request = WebRequest.Create(Auxiliar.serverURL + "ConsoleActions/Altern?idx=" + p.Idx + "&stepName=" + p.CurrentStepName + "&status=" + p.CurrentStatus);
                request.Credentials = CredentialCache.DefaultCredentials;
                try
                {
                    WebResponse response = request.GetResponse();
                    if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                    {
                        TempStatusExecution = new StatusExecution { Idx = p.Idx, StepIndex = alternIndex, State = "Processing", DateTime = now };
                        Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");

                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            alternStep.AddStatus(new Status("Processing", now));
                            p.CurrentStep = alternStep;
                        });

                        step.TryNum++;
                        DataAccess.ChangeStepConfig(p.Idx, step.Name, "Try", step.TryNum.ToString());
                    }
                    else
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                            MessagePopup messagePopup = new MessagePopup("Http Web Response FAILURE, Error Code: \n" + ((HttpWebResponse)response).StatusCode, "error", p.Idx);
                            messagePopup.Owner = System.Windows.Application.Current.MainWindow;
                            messagePopup.Topmost = true;
                            messagePopup.DataContext = this;
                            messagePopup.Show();
                        });
                    }
                }
                catch (Exception e)
                {
                    request = WebRequest.Create(Auxiliar.serverBackupURL + "ConsoleActions/Repeat?idx=" + p.Idx + "&stepName=" + p.CurrentStepName + "&status=" + p.CurrentStatus);
                    try
                    {
                        WebResponse response = request.GetResponse();
                        if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                        {
                            TempStatusExecution = new StatusExecution { Idx = p.Idx, StepIndex = alternIndex, State = "Processing", DateTime = now };
                            Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");

                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                alternStep.AddStatus(new Status("Processing", now));
                                p.CurrentStep = alternStep;
                            });

                            step.TryNum++;
                            DataAccess.ChangeStepConfig(p.Idx, step.Name, "Try", step.TryNum.ToString());
                        }
                        else
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                                MessagePopup messagePopup = new MessagePopup("Http Web Response FAILURE, Error Code: \n" + ((HttpWebResponse)response).StatusCode, "error", p.Idx);
                                messagePopup.Owner = System.Windows.Application.Current.MainWindow;
                                messagePopup.Topmost = true;
                                messagePopup.DataContext = this;
                                messagePopup.Show();
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                            MessagePopup messagePopup = new MessagePopup("An Exception has been found \n" + ex.Message, "error", p.Idx);
                            messagePopup.Owner = System.Windows.Application.Current.MainWindow;
                            messagePopup.Topmost = true;
                            messagePopup.DataContext = this;
                            messagePopup.Show();
                        });
                    }
                }
            } 
        }
        public bool CanRepeatAltern(object obj)
        {
            Process p;
            Step s;
            string currentStatus;
            if (DisplayedMessages == 0)
            {
                if (SelectedProcesses.ToList().Count == 1)
                {
                    Process layout = Auxiliar.ProcessInitConfig.Find(x => x.ProjectName == SelectedProcesses.First().ProjectName);
                    if (layout != null)
                    {
                        p = SelectedProcesses.ToList()[0];
                        if (p.IsSecuential)
                        {
                            currentStatus = p.CurrentStatus;
                            if (currentStatus.Contains("FAIL") || currentStatus == "WARNING")
                            {
                                return true;
                            }
                            else
                                return false;
                        }
                        else
                        {
                            s = (obj as Step);
                            if (s == null)
                                return false;
                            else
                            {
                                currentStatus = s.Status;
                                if (currentStatus.Contains("FAIL") || currentStatus == "WARNING" || currentStatus == "DONE")
                                {
                                    return true;
                                }
                                else
                                    return false;
                            }
                        }
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
        public void RepeatAuto(Process p)
        {
            Step step;
            StatusExecution TempStatusExecution;
            string statusFileName, actionFilename, processingFolder, processedFolder;

            processingFolder = Auxiliar.GetProcessingPath(p.Idx);
            processedFolder = Auxiliar.GetProcessedPath(p.Idx);

            int index;

            step = p.StepList.Where(x => x.Index == p.CurrentStepIndex).FirstOrDefault();
            index = p.StepList.IndexOf(step);

            DateTime now = Auxiliar.ConvertToEST(), nextExec = Auxiliar.nextRepeatDateTime(step.RepeatDate, step.RepeatTime);

            if (p.IsSecuential)
            {
                statusFileName = p.Idx + "_" + p.CurrentStepName + "." + p.CurrentStatus;
                actionFilename = p.Idx + "_" + p.CurrentStepName + ".SPOST" + Auxiliar.GenerateTimeStamp(nextExec);

                if (File.Exists(processingFolder + statusFileName))
                {
                    if (!File.Exists(processedFolder + statusFileName))
                    {
                        Auxiliar.CreateFolder(processedFolder);
                        File.Copy(processingFolder + statusFileName, processedFolder + statusFileName);
                    }
                    Auxiliar.DeleteRealTimeStepLog(step.Name, p.Idx);
                    File.Move(processingFolder + statusFileName, processingFolder + actionFilename);

                    Auxiliar.SendLogRequest("Process schedule repeat step|" + p.Idx + "|step index" + p.CurrentStepIndex + "|step " + p.CurrentStepName + "|status " + p.CurrentStatus);

                    TempStatusExecution = new StatusExecution { Idx = p.Idx, StepIndex = p.CurrentStepIndex, State = "Processing", DateTime = nextExec };
                    Auxiliar.PostProcessTraking(TempStatusExecution, "StatusExecutions/");

                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        p.StepList[index].AddStatus(new Status("Processing", now));
                        p.CurrentStep = p.StepList[index];
                    });
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                        MessagePopup messagePopup = new MessagePopup(p.Idx, "An error has ocurred\nStatus File not found:\n" + processingFolder + statusFileName + "\nPlease contact Innovation Team", "error");
                        messagePopup.Owner = System.Windows.Application.Current.MainWindow;
                        messagePopup.Topmost = true;
                        messagePopup.DataContext = this;
                        messagePopup.Show();
                    });
                }
            }
        }
        public void AbortProcess(object obj)
        {
            string statusFileName, abortFileName, processingFolder, statusSavedFileName;
            int index;
            DateTime now = Auxiliar.ConvertToEST();

            string acStatus = "\\\\10.130.19.40\\ait\\RUNTEAMCONSOLE\\AC_STATUS\\";
                        
            foreach (Process p in SelectedProcesses.ToList())
            {
                index = p.StepList.IndexOf(p.StepList.Where(x => x.Name == p.CurrentStepName).FirstOrDefault());

                processingFolder = Auxiliar.GetProcessingPath(p.Idx);
                statusFileName = processingFolder + p.Idx + "_" + p.CurrentStepName + "." + p.CurrentStatus;
                abortFileName = p.Idx + "_" + p.CurrentStepName + ".ABORT";
                statusSavedFileName = Environment.UserName.ToUpper() + "_" + p.Idx + ".*";
                var dir = new DirectoryInfo(acStatus);

                if (File.Exists(statusFileName))
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        p.StepList[index].AddStatus(new Status("ABORT", now));
                        p.CurrentStep = p.StepList[index];
                    });

                    foreach (var file in dir.EnumerateFiles(statusSavedFileName))
                        file.Delete();

                    File.Move(statusFileName, processingFolder + abortFileName);
                    Auxiliar.SendLogRequest("Process aborted|" + p.Idx + "|step " + p.CurrentStepName + "|status " + p.CurrentStatus);

                }
                else //File Not Found 
                {
                    Auxiliar.SendLogRequest("Process secuential try and fail to abort from AC side. Trying from server side|" + p.Idx + "|step index " + index + "|step name " + p.CurrentStepName + "|status " + p.CurrentStatus);
                    WebRequest request = WebRequest.Create(Auxiliar.serverURL + "ConsoleActions/Abort?idx=" + p.Idx + "&stepName=" + p.CurrentStepName + "&status=" + p.CurrentStatus);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    try
                    {
                        WebResponse response = request.GetResponse();
                        if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                p.StepList[index].AddStatus(new Status("ABORT", now));
                                p.CurrentStep = p.StepList[index];
                            });
                        }
                        else
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                                MessagePopup messagePopup = new MessagePopup("Http Web Response FAILURE, Error Code: \n" + ((HttpWebResponse)response).StatusCode, "error", p.Idx);
                                messagePopup.Owner = System.Windows.Application.Current.MainWindow;
                                messagePopup.Topmost = true;
                                messagePopup.DataContext = this;
                                messagePopup.Show();
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        request = WebRequest.Create(Auxiliar.serverBackupURL + "ConsoleActions/Abort?idx=" + p.Idx + "&stepName=" + p.CurrentStepName + "&status=" + p.CurrentStatus);
                        try
                        {
                            WebResponse response = request.GetResponse();
                            if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                            {
                                App.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    p.StepList[index].AddStatus(new Status("ABORT", now));
                                    p.CurrentStep = p.StepList[index];
                                });
                            }
                            else
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                                    MessagePopup messagePopup = new MessagePopup("Http Web Response FAILURE, Error Code: \n" + ((HttpWebResponse)response).StatusCode, "error", p.Idx);
                                    messagePopup.Owner = System.Windows.Application.Current.MainWindow;
                                    messagePopup.Topmost = true;
                                    messagePopup.DataContext = this;
                                    messagePopup.Show();
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                                MessagePopup messagePopup = new MessagePopup("An Exception has been found \n" + ex.Message, "error", p.Idx);
                                messagePopup.Owner = System.Windows.Application.Current.MainWindow;
                                messagePopup.Topmost = true;
                                messagePopup.DataContext = this;
                                messagePopup.Show();
                            });
                        }
                    }
                }

            }
        }
        public bool CanAbort(object obj)
        {
            var ret = true;
            if (SelectedProcesses.ToList().Count > 0)
            {
                foreach (Process p in SelectedProcesses.ToList())
                {
                    string currentStatus = p.CurrentStatus.ToUpper();
                    if (!(currentStatus == "DONE" || currentStatus == "WARNING" || currentStatus.Contains("FAIL")))
                        ret = false;
                }
            }
            else
                ret = false;
            return ret;
        }
        public void ShowEmergencyAbort(object obj)
        {
            this._emergencyAbort = true;
        }
        public bool CanShowEmergencyAbort(object obj)
        {
            if (obj != null)
            {
                Process p = obj as Process;
                if (p.CurrentStatus == "Processing")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public void EmergencyAbort(object obj)
        {
            Process p = obj as Process;
            string contentStr, evidencePath;
            evidencePath = Auxiliar.GetEvidencePath(p.Idx);
            contentStr = p.Idx + "|" + p.SID + "|" + p.PAS + "|" + p.PASType + "|" + p.PASOS + "|" + p.DBS + "|" + p.DBSType + "|" + p.DBSOS + "|" + p.Credentials.SAPGuiUser + "|" + p.Credentials.SAPGuiPass + "|" + p.Credentials.OSUser + "|" + p.Credentials.OSPass + "|" + p.Credentials.DBSchemaPass + "|STATUS:FAIL|MSG:PROCESS ABORTED BY THE USER!|";
            try
            {
                Auxiliar.CreateFile(evidencePath, p.Idx + "_" + p.CurrentStepName + ".OUTPUTSCRT", contentStr);
                this._emergencyAbort = false;

                Auxiliar.SendLogRequest("Emegency abort|" + p.Idx + "|step " + p.CurrentStepName + "|status " + p.CurrentStatus);
            }
            catch(Exception e)
            {
                Auxiliar.SendLogRequest("Emegency abort fail|" + p.Idx + "|step " + p.CurrentStepName + "|status " + p.CurrentStatus);
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                    MessagePopup messagePopup = new MessagePopup(e.Message, "error", p.Idx);
                    messagePopup.Owner = System.Windows.Application.Current.MainWindow;
                    messagePopup.Topmost = true;
                    messagePopup.DataContext = this;
                    messagePopup.Show();
                });
            }
        }
        public bool CanAbortEmergency(object obj)
        {
            if (obj != null)
            {
                Process p = obj as Process;
                if (this._emergencyAbort)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public void DiscardProcess(object obj)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Are you sure you want to remove those processes?", Auxiliar.appTitle, MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.OK)
            {
                foreach (Process p in SelectedProcesses.ToList())
                {
                    Auxiliar.RenamePreviousProcess(p);
                }
                SelectAllProcessesIsChecked = false;
            }
        }
        public bool CanDiscard(object obj)
        {
            var ret = true;
            if (SelectedProcesses.ToList().Count > 0)
            {
                foreach (Process p in SelectedProcesses.ToList())
                {
                    string currentStatus = p.CurrentStatus.ToUpper();
                    if (!(currentStatus == "SCHEDULED" || currentStatus == "ABORTED" || currentStatus == "COMPLETED"))
                        ret = false;
                }
            }
            else
                ret = false;
            return ret;
        }
        public void ShowUserProfile(object obj)
        {
            ProfileInfo profileWindow = new ProfileInfo();
            profileWindow.Owner = System.Windows.Application.Current.MainWindow;
            profileWindow.Show();
        }
        public void ShowMaintenance(object obj)
        {
            Views.Maintenance maintenanceWindow = new Views.Maintenance();
            maintenanceWindow.Owner = System.Windows.Application.Current.MainWindow;
            maintenanceWindow.DataContext = this;
            maintenanceWindow.Show();
        }
        public DateTime CalendarSelectedDate
        {
            get { return this._calendarSelectedDate; }
            set
            {
                this._calendarSelectedDate = value;
                this.OnPropertyChanged("CalendarSelectedDate");
                if (value != null)
                    CalendarMaintenanceList = new ObservableCollection<Models.Maintenance>(GetMaintenancesPerDay(value));
                else
                    CalendarMaintenanceList = Maintenances;
            }
        }
        public ObservableCollection<Models.Maintenance> CalendarMaintenanceList
        {
            get { return this._calendarMaintenanceList; }
            set
            {
                this._calendarMaintenanceList = value;
                this.OnPropertyChanged("CalendarMaintenanceList");
            }
        }
        public List<Models.Maintenance> GetMaintenancesPerDay(DateTime date)
        {
            List<Models.Maintenance> output = new List<Models.Maintenance>();
            foreach (Models.Maintenance maintenance in Maintenances)
            {
                if (maintenance.StartDate < date.AddDays(1) && maintenance.EndDate > date)
                    output.Add(maintenance);
            }
            return output;
        }
        public int DisplayedMessages
        {
            get { return this._displayedMessages; }
            set
            {
                this._displayedMessages = value;
                this.OnPropertyChanged("DisplayedMessages");
            }
        }
        public void OkClickPopup(object obj)
        {
            MessagePopup messagePopup = obj as MessagePopup;
            messagePopup.allowClosing = true;
            messagePopup.Close();
        }
        public bool AlwwaysCanExecute(object obj) { return true; }
        public bool CantExecute(object obj) { return false; }
        private void DispatcherTimerSetup()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMinutes(3);
            dispatcherTimer.Tick += new EventHandler(CheckBroadcastMessages);
            dispatcherTimer.Start();
        }
        private void CheckBroadcastMessages(object sender, EventArgs e)
        {
            DateTime now = Auxiliar.ConvertToEST();
            ObservableCollection<BroadcastMessages> serverMessages;
            BroadcastMessages tempMessage;
            serverMessages = new ObservableCollection<BroadcastMessages>(da.GetBroadcastMessages());

            foreach (BroadcastMessages broadcastMessage in serverMessages)
            {
                tempMessage = BroadcastMessages.Where(x => x.TimeStamp == broadcastMessage.TimeStamp).FirstOrDefault();
                if (tempMessage != null)
                {
                    if (tempMessage.Shown == false)
                    {
                        if (tempMessage.StartTime <= now)
                        {
                            ShowBroadcastMessage(tempMessage);
                        }
                    }
                }
                else
                {
                    if (broadcastMessage.StartTime <= now)
                    {
                        ShowBroadcastMessage(broadcastMessage);
                    }
                    BroadcastMessages.Add(broadcastMessage);
                }
            }
        }
        private void ShowBroadcastMessage(BroadcastMessages message)
        {

            message.Shown = true;

            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
            {
                MessagePopup messagePopup = new MessagePopup(message.Message, "WARNING");
                messagePopup.Owner = System.Windows.Application.Current.MainWindow;
                messagePopup.Topmost = true;
                messagePopup.DataContext = this;
                messagePopup.Show();
            });

            Auxiliar.SendLogRequest("Broadcast message shown|" + message.TimeStamp + "|destination: " + message.Destination + "|message " + message.Message);
        }
    }
}