using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace RunTeamConsole.Models
{
    public class ExtraInput : ObservableObject
    {
        private string _name;
        private string _description;
        private string _hints;
        private string _type;
        private string _format;
        private string _minvalue;
        private string _maxvalue;
        private string _options;
        private ObservableCollection<string> _optionsArray;
        private string _defaultvalue;
        private bool _clearvalues;
        private string _value;

        [JsonConstructor]
        public ExtraInput(string name, string description, string hints, string type, string format, string defaultvalue, string options, string[] optionsArray, string minvalue, string maxvalue, bool clearvalues)
        {
            this._name = name;
            this._description = description;
            this._hints = hints;
            this._type = type;
            this._format = format;
            this._defaultvalue = defaultvalue;
            if (optionsArray == null)
            {
                if (!String.IsNullOrEmpty(options)) {
                    this._options = options;
                    var optionsArr = options.Split("|");
                    this._optionsArray = new ObservableCollection<string>(optionsArr);
                } 
            }
            else
            {
                this._optionsArray = new ObservableCollection<string>(optionsArray); 
            }
            this._minvalue = minvalue;
            this._maxvalue = maxvalue;
            this._clearvalues = clearvalues;
        }
        public ExtraInput(string name, string description, string hints, string type, string format, string defaultvalue, ObservableCollection<string> optionsArray, string minvalue, string maxvalue, bool clearvalues)
        {
            this._name = name;
            this._description = description;
            this._hints = hints;
            this._type = type;
            this._format = format;
            this._defaultvalue = defaultvalue;
            this._optionsArray = optionsArray;
            this._minvalue = minvalue;
            this._maxvalue = maxvalue;
            this._clearvalues = clearvalues;
        }

        public string Name
        {
            get { return this._name; }
        }
        public string Description
        {
            get { return this._description; }
        }
        public string Hint
        {
            get { return this._hints; }
        }
        public string Type //List -> 1 a N
        {
            get { return this._type; }
        }
        public string Format
        {
            get { return this._format; }
        }
        public string DefaultValue
        {
            get { return this._defaultvalue; }
            set { this._defaultvalue = value; }
        }
        public string MaxValue
        {
            get { return this._maxvalue; }
        }
        public string MinValue
        {
            get { return this._minvalue; }
        }
        public bool ClearValues
        {
            get { return this._clearvalues; }
            set { this._clearvalues = value; }
        }
        public string Value
        {
            get { return this._value; }
            set { this._value = value; this.OnPropertyChanged("Value"); }
        }

        public string Options
        {
            get { return this._options; }
            set { this._options = value; }
        }
        public ObservableCollection<string> OptionsArray
        {
            get { return this._optionsArray; }
            set { this._optionsArray = value; }
        }


    }
}
