using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RunTeamConsole.Views.SapInstallPostSteps
{
    /// <summary>
    /// Interaction logic for SapInstallDB13.xaml
    /// </summary>
    public partial class SapInstallDB13 : UserControl
    {
        //private bool handle = true;
        public SapInstallDB13()
        {
            InitializeComponent();
            int[] hourComboItems = new[]
            {
                0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23
            };
            int[] minuteComboItems = new int[12];
            for (int i = 0; i < 12; i++)
            {
                minuteComboItems[i] = i * 5;
            }
            string[] everyItems = new[]
            {
                "Day",
                "Hour",
                "Week",
                "Once"
            };
            string[] rangeItems = new[]
            {
                "No End Date",
                "End After",
                "End By"
            };
            string[] jobName = new[]
            {
                "Whole database online backup",
                "Whole database offline backup",
                "Whole database offline + redo log backup",
                "Whole database online backup",
                "Whole database offline backup",
                "Whole database online + redo log backup",
                "Check database",
                "Cleanup logs",
                "Compress database",
                "Full database offline + redo log backup",
                "Full database online backup",
                "Full database offline backup",
                "Full database online + redo log backup",
                "Incremental offline + redo log backup",
                "Incremental database online backup",
                "Incremental database offline backup",
                "Incremental online + redo log backup",
                "Redo log backup",
                "Adapt next extents",
                "Prepare for RMAN backup",
                "Partial database offline backup",
                "Partial database online backup",
                "Check and update optimizer statistics",
                "Initialize tape",
                "Validate structure",
                "Verify database",
                "Central Calendar Log Collector"
            };

            Db13JobCombobox.ItemsSource = jobName;
            
            Db13StartDateHourCombobox.ItemsSource = hourComboItems;
            Db13StartDateMinuteCombobox.ItemsSource = minuteComboItems;

            Db13RecurrenceCombobox.ItemsSource = everyItems;
            
            Db13RangeCombobox.ItemsSource = rangeItems;
        }

        private void Db13RecurrenceCombobox_DropDownClosed(object sender, EventArgs e)
        {
            string[] day;
            
            if (Db13RecurrenceCombobox.SelectedItem.ToString().ToUpper().Equals("DAY"))
            {
                Db13RecurrenceAtDayStackPanel.Visibility = Visibility.Visible;

                int[] hourComboItems = new[]
                {
                    0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23
                };

                int[] minuteComboItems = new int[12];
                for (int i = 0; i < 12; i++)
                {
                    minuteComboItems[i] = i * 5;
                }

                Db13RecurrenceDayHourCombobox.ItemsSource = hourComboItems;
                Db13RecurrenceDayMinuteCombobox.ItemsSource= minuteComboItems;
            }
            else if(Db13RecurrenceCombobox.SelectedItem.ToString().ToUpper().Equals("WEEK"))
            {
                Db13RecurrenceAtWeekStackPanel.Visibility = Visibility.Visible;

                day = new[]
                {
                    "Monday",
                    "Tuesday",
                    "Wednesday",
                    "Thursday",
                    "Friday",
                    "Saturday",
                    "Sunday"
                };

                Db13RecurrenceAtDayCombobox.ItemsSource = day;
            }
            else if (Db13RecurrenceCombobox.SelectedItem.ToString().ToUpper().Equals("HOUR") || Db13RecurrenceCombobox.SelectedItem.ToString().ToUpper().Equals("ONCE"))
            {
                Db13RecurrenceAtDayStackPanel.Visibility = Visibility.Collapsed;
                Db13RecurrenceAtWeekStackPanel.Visibility = Visibility.Collapsed;
            }
            else if(Db13RecurrenceCombobox.SelectedItem == null)
            {
                //do nothing
            }
        }

        private void Db13RangeCombobox_DropDownClosed(object sender, EventArgs e)
        {
            if(Db13RecurrenceCombobox.SelectedItem == null)
            {
                Db13RecurrenceEndByStackPanel.Visibility = Visibility.Collapsed;
                Db13RangeEndAfterStackPanel.Visibility = Visibility.Collapsed;
            }
            if (Db13RangeCombobox.SelectedItem.ToString().ToUpper().Equals("END AFTER"))
            {
                Db13RecurrenceEndByStackPanel.Visibility = Visibility.Collapsed;
                Db13RangeEndAfterStackPanel.Visibility = Visibility.Visible;
            }
            else if (Db13RangeCombobox.SelectedItem.ToString().ToUpper().Equals("END BY"))
            {
                Db13RangeEndAfterStackPanel.Visibility = Visibility.Collapsed;
                Db13RecurrenceEndByStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                Db13RangeEndAfterStackPanel.Visibility = Visibility.Collapsed;
                Db13RecurrenceEndByStackPanel.Visibility = Visibility.Collapsed;
            }
        }
    }
}
