using RunTeamConsole.Models.Packages;
using System.Collections.ObjectModel;

namespace RunTeamConsole.ViewModels
{
    public partial class AddProcessViewModel : ObservableObject
    {
        private ObservableCollection<SAPHostAgentPackage> _availableSAPHostAgentPackagesForLinux, _selectedSAPHostAgentPackagesForLinux, _availableSAPHostAgentPackagesForAIX, _selectedSAPHostAgentPackagesForAIX;
        private SAPHostAgentPackage _selectedSAPHostAgentPackageForLinux, _selectedSAPHostAgentPackageForAIX;
        private ObservableCollection<string> _SAPHostAgentVersionsForLinux,_SAPHostAgentVersionsForAIX;
        private string _selectedSAPHostAgentVersionForLinux, _selectedSAPHostAgentVersionForAIX;

        public ObservableCollection<SAPHostAgentPackage> AvailableSAPHostAgentPackagesForLinux
        {
            get { return this._availableSAPHostAgentPackagesForLinux; }
            set
            {
                this._availableSAPHostAgentPackagesForLinux = value;
                this.OnPropertyChanged("AvailableSAPHostAgentPackagesForLinux");
            }
        }

        public SAPHostAgentPackage SelectedSAPHostAgentPackageForLinux
        {
            get { return this._selectedSAPHostAgentPackageForLinux; }
            set
            {
                this._selectedSAPHostAgentPackageForLinux = value;
                this.OnPropertyChanged("SelectedSAPHostAgentPackageForLinux");
                SelectedSAPHostAgentPackagesForLinux = new ObservableCollection<SAPHostAgentPackage>() { this._selectedSAPHostAgentPackageForLinux };
            }
        }
        public ObservableCollection<SAPHostAgentPackage> SelectedSAPHostAgentPackagesForLinux
        {
            get { return this._selectedSAPHostAgentPackagesForLinux; }
            set
            {
                this._selectedSAPHostAgentPackagesForLinux = value;
                this.OnPropertyChanged("SelectedSAPHostAgentPackagesForLinux");
            }
        }


        public ObservableCollection<string> AvailableSAPHostAgentVersionsForLinux
        {
            get { return this._SAPHostAgentVersionsForLinux; }
            set
            {
                this._SAPHostAgentVersionsForLinux = value;
                this.OnPropertyChanged("AvailableSAPHostAgentVersionsForLinux");
            }
        }

        public string SelectedSAPHostAgentVersionForLinux
        {
            get { return this._selectedSAPHostAgentVersionForLinux; }
            set
            {
                this._selectedSAPHostAgentVersionForLinux = value;
                this.OnPropertyChanged("SelectedSAPHostAgentVersionForLinux");
                foreach (SAPHostAgentPackage pkg in AvailableSAPHostAgentPackagesForLinux)
                {
                    if (pkg.Version == value)
                        pkg.Active = true;
                    else
                        pkg.Active = false;
                }
                SelectedSAPHostAgentPackageForLinux = null;
            }
        }
        
        public bool DisplayAvailableSAPHostAgentPackagesForLinux
        {
            get
            {
                if (AvailableSAPHostAgentPackagesForLinux.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public ObservableCollection<SAPHostAgentPackage> AvailableSAPHostAgentPackagesForAIX
        {
            get { return this._availableSAPHostAgentPackagesForAIX; }
            set
            {
                this._availableSAPHostAgentPackagesForAIX = value;
                this.OnPropertyChanged("AvailableSAPHostAgentPackagesForAIX");
            }
        }

        public SAPHostAgentPackage SelectedSAPHostAgentPackageForAIX
        {
            get { return this._selectedSAPHostAgentPackageForAIX; }
            set
            {
                this._selectedSAPHostAgentPackageForAIX = value;
                this.OnPropertyChanged("SelectedSAPHostAgentPackageForAIX");
                SelectedSAPHostAgentPackagesForAIX = new ObservableCollection<SAPHostAgentPackage>() { this._selectedSAPHostAgentPackageForAIX };
            }
        }

        public ObservableCollection<SAPHostAgentPackage> SelectedSAPHostAgentPackagesForAIX
        {
            get { return this._selectedSAPHostAgentPackagesForAIX; }
            set
            {
                this._selectedSAPHostAgentPackagesForAIX = value;
                this.OnPropertyChanged("SelectedSAPHostAgentPackagesForAIX");
            }
        }


        public ObservableCollection<string> AvailableSAPHostAgentVersionsForAIX
        {
            get { return this._SAPHostAgentVersionsForAIX; }
            set
            {
                this._SAPHostAgentVersionsForAIX = value;
                this.OnPropertyChanged("AvailableSAPHostAgentVersionsForAIX");
            }
        }

        public string SelectedSAPHostAgentVersionForAIX
        {
            get { return this._selectedSAPHostAgentVersionForAIX; }
            set
            {
                this._selectedSAPHostAgentVersionForAIX = value;
                this.OnPropertyChanged("SelectedSAPHostAgentVersionForAIX");
                foreach (SAPHostAgentPackage pkg in AvailableSAPHostAgentPackagesForAIX)
                {
                    if (pkg.Version == value)
                        pkg.Active = true;
                    else
                        pkg.Active = false;
                }
                SelectedSAPHostAgentPackageForAIX = null;
            }
        }

        public bool DisplayAvailableSAPHostAgentPackagesForAIX
        {
            get
            {
                if (AvailableSAPHostAgentPackagesForAIX.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public bool AtLeastOneSelecteSAPHostAgentPackageForLinux
        {
            get
            {
                if (SelectedSAPHostAgentPackageForLinux != null)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public bool AtLeastOneSelectedSAPHostAgentPackageForAIX
        {
            get
            {
                if (SelectedSAPHostAgentPackageForAIX != null)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public bool AtLeastOneDB2InstallPackageSelectedForLinux
        {
            get
            {
                if (SelectedDb2InstallCatalogForLinux != null)
                {
                    return true;
                }
                else
                    return false;
            }
        }

    }
}
