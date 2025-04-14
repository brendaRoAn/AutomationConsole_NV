using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace RunTeamConsole.Converters
{
    class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = value.ToString();
            //if (stringValue == "0001-01-01T00:00:00")
            if (stringValue == "1/1/0001 12:00:00 AM")
                return "-";
            else
                return stringValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateTime =  DateTime.Parse(value.ToString());
            return dateTime;
        }
    }
}
