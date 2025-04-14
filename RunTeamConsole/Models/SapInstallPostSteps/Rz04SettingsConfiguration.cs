using Newtonsoft.Json;
using System;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class Rz04SettingsConfiguration : ObservableObject
    {
        private string _operationName, _description;
        private int _inTimeHour, _inTimeMinute, _endTimeHour, _endTimeMinute;
        private bool _isSelected;
        public string OperationName
        {
            get { return _operationName; }
            set { _operationName = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public int InTimeHour
        {
            get { return _inTimeHour; }
            set { _inTimeHour = value; }
        }
        public int InTimeMinute
        {
            get { return _inTimeMinute; }
            set { _inTimeMinute = value; }
        }
        public string InTime
        {
            get
            {
                string inHour, inMinute;

                if (InTimeHour < 10)
                    inHour = String.Concat("0" + InTimeHour);
                else
                    inHour = InTimeHour.ToString();
                
                if(InTimeMinute < 10)
                    inMinute = String.Concat("0" + InTimeMinute);
                else
                    inMinute = InTimeMinute.ToString();

                return string.Concat(inHour + ":" + inMinute);
            }
        }
        public int EndTimeHour
        {
            get { return _endTimeHour; }
            set { _endTimeHour = value; }
        }
        public int EndTimeMinute
        {
            get { return _endTimeMinute; }
            set { _endTimeMinute = value; }
        }
        public string EndTime
        {
            get
            {
                string endHour, endMinute;

                if (EndTimeHour < 10)
                    endHour = String.Concat("0" + EndTimeHour);
                else
                    endHour = EndTimeHour.ToString();

                if (EndTimeMinute < 10)
                    endMinute = String.Concat("0" + EndTimeMinute);
                else
                    endMinute = EndTimeMinute.ToString();

                return string.Concat(endHour + ":" + endMinute);
            }
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