using System.Collections.ObjectModel;

namespace RunTeamConsole.Models
{
    public class MenuItem : TreeViewItemBase
    {
        private string icon;

        public string Icon
        {
            get { return icon; }
            set 
            { 
                icon = value;
                OnPropertyChanged("Icon");
            }
        }

        public MenuItem()
        {
            this.Items = new ObservableCollection<MenuItem>();
        }

        public string Title { get; set; }
        public System.Windows.Media.Brush Foreground { get; set; }

        public ObservableCollection<MenuItem> Items { get; set; }
    }
}
