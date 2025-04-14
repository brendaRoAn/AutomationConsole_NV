using Newtonsoft.Json;
using RunTeamConsole.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RunTeamConsole.Models
{
    public class ExtraInputsSet : ObservableObject
    {
        private string _process;
        private string _system;
        private string _step;
        private List<ExtraInput> _inputsSet;

        [JsonConstructor]
        public ExtraInputsSet(List<ExtraInput> inputsset)
        {
            this._inputsSet = inputsset;
        }
        
        public ExtraInputsSet(string process, string step)
        {
            this._process = process;
            this._step = step;
            this._inputsSet = new List<ExtraInput>();
        }
        
        public string Process
        {
            get { return this._process; }
            set { this._process = value; }
        }

        public string System
        {
            get { return this._system; }
            set { this._system = value; }
        }
        
        public string Step
        {
            get { return this._step; }
            set { this._step = value; }
        }

        public List<ExtraInput> InputsSet
        {
            get { return this._inputsSet; }
            set { this._inputsSet = value; }
        }

        public void AddInput(ExtraInput input)
        {
            this._inputsSet.Add(input);
        }
    }
}
