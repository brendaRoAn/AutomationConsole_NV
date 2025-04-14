using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RunTeamConsole.Models
{
    public class OptionsMessage : ObservableObject
    {
        private string _title;
        private string _body;
        private string _tooltip;
        private string _continueCommandText;
        private string _repeatCommandText;
        private string _alternCommandText;

        [JsonConstructor]
        public OptionsMessage(string title, string body, string tooltip, string continueCommandText, string repeatCommandText, string alternCommandText)
        {
            this._title = title;
            this._body = body;
            this._tooltip = tooltip;
            this._continueCommandText = continueCommandText;
            this._repeatCommandText = repeatCommandText;
            this._alternCommandText = alternCommandText;
        }

        public string Title 
        {
            get { return this._title; }
            set
            {
                this._title = value;
                this.OnPropertyChanged("Title");
            }
        }
        public string Body 
        {
            get { return this._body; }
            set
            {
                this._body = value;
                this.OnPropertyChanged("Body");
            }
        }
        public string Tooltip 
        {
            get { return this._tooltip; }
            set
            {
                this._tooltip = value;
                this.OnPropertyChanged("Tooltip");
            }
        }
        public string ContinueCommandText
        {
            get { return this._continueCommandText; }
            set
            {
                this._continueCommandText = value;
                this.OnPropertyChanged("ContinueCommandText");
            }
        }
        public string RepeatCommandText
        {
            get { return this._repeatCommandText; }
            set
            {
                this._repeatCommandText = value;
                this.OnPropertyChanged("RepeatCommandText");
            }
        }
        public string AlternCommandText
        {
            get { return this._alternCommandText; }
            set
            {
                this._alternCommandText = value;
                this.OnPropertyChanged("AlternCommandText");
            }
        }
    }
}
