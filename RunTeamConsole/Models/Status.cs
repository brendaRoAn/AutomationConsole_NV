using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RunTeamConsole.Models
{
    public class Status
    {
        private string _status;
        private DateTime _datetime;
        
        [JsonConstructor]
        public Status(string state, DateTime datetime)
        {
            this._datetime = datetime;
            this._status = state;
        }

        public string State
        {
            get { return this._status; }
            set { this._status = value; }
        }
        public DateTime DateTime
        {
            get { return this._datetime; }
            set { this._datetime= value; }
        }
    }
}
