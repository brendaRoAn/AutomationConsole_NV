using System.Windows.Controls;

namespace RunTeamConsole.Models.AddProcesses
{
    public class AddProcessControl : ObservableObject
    {
        private UserControl _userControl;

        public UserControl UserControl
        {
            get { return _userControl; }
            set { _userControl = value; }
        }

        public MenuItem MenuItem { get; set; }

        private bool isActive;

        public bool IsActive
        {
            get { return this.isActive; }
            set 
            {
                this.isActive = value;
                this.OnPropertyChanged("IsActive");
                this.MenuItem.IsSelected = true;
                this.MenuItem.Icon = "\\img\\icons\\processing.png";
            }
        }

    }
}
