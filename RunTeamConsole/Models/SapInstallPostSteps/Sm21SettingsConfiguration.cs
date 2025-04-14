using Newtonsoft.Json;
using System;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class Sm21SettingsConfiguration : ObservableObject
    {
        private int _fromHour, _fromMinute, _toHour, _toMinute;
        private DateTime _fromDate, _toDate;

        public int FromHour
        {
            get { return _fromHour; }
            set { _fromHour = value; }
        }
        public int FromMinute
        {
            get { return _fromMinute; }
            set { _fromMinute = value; }
        }
        public DateTime FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; }
        }
        public int ToHour
        {
            get { return _toHour; }
            set { _toHour = value; }
        }
        public int ToMinute
        {
            get { return _toMinute; }
            set { _toMinute = value; }
        }
        public DateTime ToDate
        {
            get { return _toDate; }
            set { _toDate = value; }
        }
    }
}