using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class Rz12SettingsConfiguration : ObservableObject
    {
        private string _groupName, _instanceGroup;
        int _maxQueue, _maxLogin, _maxSeparateLogons, _maxwp, _minfreewp, _maxcomm, _maxWaitTime;
        bool _activated, _isSelected;

        public string GroupName
        { 
            get { return _groupName; } 
            set {  _groupName = value; }
        }
        public string InstanceGroup
        {
            get { return _instanceGroup; }
            set { _instanceGroup = value; }
        }
        public bool Activated
        {
            get { return _activated; }
            set
            { 
                _activated = value;
                if (value)
                    ActivatedNumber = 1;
                else
                    ActivatedNumber = 0;
            }
        }
        public int ActivatedNumber
        {
            get ; set ;
        }
        public int MaxQueue
        {
            get { return _maxQueue; }
            set { _maxQueue = value; }
        }
        public int MaxLogin
        {
            get { return _maxLogin; }
            set { _maxLogin = value; }
        }
        public int MaxSeparateLogons
        {
            get { return _maxSeparateLogons; }
            set { _maxSeparateLogons = value; }
        }
        public int Maxwp
        {
            get { return _maxwp; }
            set { _maxwp = value; }
        }
        public int Minfreewp
        {
            get { return _minfreewp; }
            set { _minfreewp = value; }
        }
        public int Maxcomm
        {
            get { return _maxcomm; }
            set { _maxcomm = value; }
        }
        public int MaxWaitTime
        {
            get { return _maxWaitTime; }
            set { _maxWaitTime = value; }
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
    }
}