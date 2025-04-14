using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RunTeamConsole.Models
{
    public class Favorites : ObservableObject
    {
        private string _name;
        private string _title;
        private bool _useBridge;
        private bool _saltConnectivity;
        private List<Process> _processes;
        private DateTime _lastexec;
        private Process _summaryProcessSelected;

        [JsonConstructor]
        public Favorites(string name, string title, List<Process> processes, DateTime lastexecution)
        {
            this._name = name;
            this._title = title;
            this._processes = processes;
            this._useBridge = processes[0].BrideServer;
            this._lastexec = lastexecution;
        }
        
        public Favorites(string name, string title, bool useBridge, List<Process> processes, bool saltConnectivity)
        {
            this._name = name;
            this._title = title;
            this._useBridge = useBridge;
            this._processes = processes;
            this._saltConnectivity = saltConnectivity;
        }
        
        public string Name {
            get { return this._name; }
            set { this._name = value; }
        }
        public string Title {
            get { return this._title; }
            set { this._title = value; }
        }
        public bool UseBridge {
            get { return this._useBridge; }
            set { this._useBridge = value; }
        }
        public List<Process> Processes 
        {
            get { return this._processes; }
            set { this._processes = value; }
        }

        [JsonIgnore]
        public Process SummarySelectedProcesses
        {
            get { return this._summaryProcessSelected; }
            set
            {
                this._summaryProcessSelected = value;
                this.OnPropertyChanged("SummarySelectedProcesses");
            }
        }

        public DateTime LastExecution
        {
            get { return this._lastexec; }
            set { this._lastexec = value; }
        }

        public bool SaltConnectivity
        {
            get { return this._saltConnectivity; }
            set { this._saltConnectivity = value; }
        }
    }
}
