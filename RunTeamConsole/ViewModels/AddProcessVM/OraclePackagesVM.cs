
using RunTeamConsole.Models;
using System.Collections.ObjectModel;

namespace RunTeamConsole.ViewModels
{
    public partial class AddProcessViewModel : ObservableObject
    {
        private ObservableCollection<string> _oracleDBVersionsLinux;
        private ObservableCollection<string> _oracleDBVersionsAIX;
        private string _selectedOracleDBVersionForLinux;
        private string _selectedOracleDBVersionForAIX;
        private ObservableCollection<OraclePackage> _availableOraclePackagesForLinux;
        private OraclePackage _selectedOraclePackageForLinux;
        private ObservableCollection<OraclePackage> _availableOraclePackagesForAIX;
        private OraclePackage _selectedOraclePackageForAIX;

        public OraclePackage SelectedOraclePackageForLinux
        {
            get { return this._selectedOraclePackageForLinux; }
            set
            {
                this._selectedOraclePackageForLinux = value;
                this.OnPropertyChanged("SelectedOraclePackageForLinux");
            }
        }

        public OraclePackage SelectedOraclePackageForAIX
        {
            get { return this._selectedOraclePackageForAIX; }
            set
            {
                this._selectedOraclePackageForAIX = value;
                this.OnPropertyChanged("SelectedOraclePackageForAIX");
            }
        }

        public ObservableCollection<string> AvailableOracleDBVersionsForLinux
        {
            get { return this._oracleDBVersionsLinux; }
            set
            {
                this._oracleDBVersionsLinux = value;
                this.OnPropertyChanged("AvailableOracleDBVersionsLinux");
            }
        }

        public string SelectedOracleDBVersionForLinux
        {
            get { return this._selectedOracleDBVersionForLinux; }
            set
            {
                this._selectedOracleDBVersionForLinux = value;
                this.OnPropertyChanged("SelectedOracleDBVersionForLinux");
                foreach (OraclePackage pkg in AvailableOraclePackagesForLinux)
                {
                    if (pkg.DBVersion == value)
                        pkg.Active = true;
                    else
                        pkg.Active = false;
                }
                SelectedOraclePackageForLinux = null;
            }
        }

        public ObservableCollection<string> AvailableOracleDBVersionsForAIX
        {
            get { return this._oracleDBVersionsAIX; }
            set
            {
                this._oracleDBVersionsAIX = value;
                this.OnPropertyChanged("AvailableOracleDBVersionsForAIX");
            }
        }

        public string SelectedOracleDBVersionForAIX
        {
            get { return this._selectedOracleDBVersionForAIX; }
            set
            {
                this._selectedOracleDBVersionForAIX = value;
                this.OnPropertyChanged("SelectedOracleDBVersionForAIX");
                foreach (OraclePackage pkg in AvailableOraclePackagesForAIX)
                {
                    if (pkg.DBVersion == value)
                        pkg.Active = true;
                    else
                        pkg.Active = false;
                }
                SelectedOraclePackageForAIX = null;
            }
        }

        public ObservableCollection<OraclePackage> AvailableOraclePackagesForLinux
        {
            get { return this._availableOraclePackagesForLinux; }
            set
            {
                if (_availableOraclePackagesForLinux != value)
                {
                    _availableOraclePackagesForLinux = value;
                    OnPropertyChanged("AvailableOraclePackagesForLinux");
                }
            }
        }

        public bool DisplayAvailableOraclePackagesForLinux
        {
            get
            {
                if (AvailableOraclePackagesForLinux.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public ObservableCollection<OraclePackage> AvailableOraclePackagesForAIX
        {
            get { return this._availableOraclePackagesForAIX; }
            set
            {
                if (_availableOraclePackagesForAIX != value)
                {
                    _availableOraclePackagesForAIX = value;
                    OnPropertyChanged("AvailableOraclePackagesForAIX");
                }
            }
        }

        public bool DisplayAvailableOraclePackagesForAIX
        {
            get
            {
                if (AvailableOraclePackagesForAIX.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public bool AtLeastOneSelectedOraclePackageForLinux
        {
            get
            {
                if (SelectedOraclePackageForLinux != null)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public bool AtLeastOneSelectedOraclePackageForAIX
        {
            get
            {
                if (SelectedOraclePackageForAIX != null)
                {
                    return true;
                }
                else
                    return false;
            }
        }

    }
}