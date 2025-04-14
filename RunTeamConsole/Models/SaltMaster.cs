using System;
using System.Collections.Generic;
using System.Text;

namespace RunTeamConsole.Models
{
    public class SaltMaster : ObservableObject
    {
        private string _customer;
        private string _environment;
        private string _hostname;
        private string _user;
        private string _key;
        private string _type;
        private bool _isSelected;
        private bool _isEnabled;

        public SaltMaster(string customer, string environment, string hostname, string user, string key, string type)
        {
            this._customer = customer;
            this._environment = environment;
            this._hostname = hostname;
            this._user = user;
            this._key = key;
            this._type = type;
            this._isSelected = false;
            this._isEnabled = true;
        }

        public string Customer
        {
            get { return this._customer; }
        }
        public string Environment
        {
            get { return this._environment; }
        }
        public string Hostname
        {
            get { return this._hostname; }
        }
        public string User
        {
            get { return this._user; }
        }
        public string Key
        {
            get { return this._key; }
        }
        public string Type
        {
            get { return this._type; }
        }

        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                this._isSelected = value;
                this.OnPropertyChanged("IsSelected");
            }
        }

        public bool IsEnabled
        {
            get { return this._isEnabled; }
            set
            {
                this._isEnabled = value;
                this.OnPropertyChanged("IsEnabled");
            }
        }

    }
}
