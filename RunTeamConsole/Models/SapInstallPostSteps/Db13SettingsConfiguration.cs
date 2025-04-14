using System;
using System.Windows.Documents;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class Db13SettingsConfiguration : ObservableObject
    {
        private string _job, _recurrence, _range, _recurrenceDay, _rangeNoEnd, _rangeEndAfter;
        private int? _startDateHour, _startDateMinute, _recurrenceDayHour, _recurrenceDayMinute, _recurrenceHour, _recurrenceOnceHour, _recurrenceOnceMinute, _rangeEndByhour, _rangeEndByMinute;
        private DateTime? _startDate, _recurrenceOnceDate, _rangeEndByDate;
        private TimeSpan _recurrenceDayTimeSpan;
        private bool _isSelected, handle = true;

        public string Job
        {
            get { return _job; }
            set { _job = value; }
        }
        public string Recurrence
        {
            get { return _recurrence; }
            set { _recurrence = value; }
        }
        public string Range
        {
            get { return _range; }
            set { _range = value; }
        }
        public string RecurrenceDay
        {
            get { return _recurrenceDay; }
            set { _recurrenceDay = value; }
        }
        public string RangeNoEnd
        {
            get { return _rangeNoEnd; }
            set { _rangeNoEnd = value; }
        }
        public string RangeEndAfter
        {
            get { return _rangeEndAfter; }
            set { _rangeEndAfter = value; }
        }
        public int? StartDateHour
        {
            get { return _startDateHour; }
            set { _startDateHour = value; }
        }
        public int? StartDateMinute
        {
            get { return _startDateMinute; }
            set { _startDateMinute = value; }
        }
        public int? RecurrenceDayHour
        {
            get { return _recurrenceDayHour; }
            set { _recurrenceDayHour = value;}
        }
        public int? RecurrenceDayMinute
        {
            get { return _recurrenceDayMinute; }
            set { _recurrenceDayMinute = value; }
        }
        public TimeSpan RecurrenceDayTimeSpan
        {
            get { return _recurrenceDayTimeSpan; }
            set { _recurrenceDayTimeSpan = value; }
        }
        public String RecurrenceDayTimeSpanString
        {
            get { return string.Format("{0}:{1}", RecurrenceDayTimeSpan.Hours, RecurrenceDayTimeSpan.Minutes);  }

        }
        public int? RecurrenceHour
        {
            get { return _recurrenceHour; }
            set { _recurrenceHour = value; }
        }
        public int? RecurrenceOnceHour
        {
            get { return _recurrenceOnceHour; }
            set { _recurrenceOnceHour = value;}
        }
        public int? RecurrenceOnceMinute
        {
            get { return _recurrenceOnceMinute; }
            set { _recurrenceOnceMinute = value;}
        }
        public int? RangeEndByhour
        {
            get { return _rangeEndByhour; }
            set { _rangeEndByhour = value;}
        }
        public int? RangeEndByMinute
        {
            get { return _rangeEndByMinute; }
            set { _rangeEndByMinute = value; }
        }
        public DateTime? StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }
        public DateTime? RecurrenceOnceDate
        {
            get { return _recurrenceOnceDate; }
            set { _recurrenceOnceDate = value; }
        }
        public DateTime? RangeEndByDate
        {
            get { return _rangeEndByDate; }
            set { _rangeEndByDate = value; }
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