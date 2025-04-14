using RunTeamConsole.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace RunTeamConsole.ViewModels
{
    public partial class AddProcessViewModel : ObservableObject
    {
        private SAPKernelPackage _selectedSAPKernelPackageForOracle, _selectedSAPKernelPackageForSybase, _selectedSAPKernelPackageForDB2, _selectedSAPKernelPackageForHana, _selectedSAPKernelPackageForSAPDB, _selectedSAPKernelPackageForWebD;
        private ObservableCollection<SAPKernelPackage> _availableSAPKernelPatchesForOracle, _availableSAPKernelPatchesForSybase, _availableSAPKernelPatchesForDB2, _availableSAPKernelPatchesForHana, _availableSAPKernelPatchesForSAPDB, _availableSAPKernelPatchesForWebD;
        private ObservableCollection<string> _sapKernelUnicodeListForOracle, _sapKernelVersionListForOracle, _sapKernelPatchListForOracle;
        private ObservableCollection<string> _sapKernelUnicodeListForSybase, _sapKernelVersionListForSybase, _sapKernelPatchListForSybase;
        private ObservableCollection<string> _sapKernelUnicodeListForDB2, _sapKernelVersionListForDB2, _sapKernelPatchListForDB2;
        private ObservableCollection<string> _sapKernelUnicodeListForHana, _sapKernelVersionListForHana, _sapKernelPatchListForHana;
        private ObservableCollection<string> _sapKernelUnicodeListForSAPDB, _sapKernelVersionListForSAPDB, _sapKernelPatchListForSAPDB;
        private ObservableCollection<string> _sapKernelUnicodeListForWebD, _sapKernelVersionListForWebD, _sapKernelPatchListForWebD;
        private string _selectedSAPKernelUnicodeForOracle, _selectedSAPKernelVersionForOracle, _selectedSAPKernelPatchForOracle;
        private string _selectedSAPKernelUnicodeForSybase, _selectedSAPKernelVersionForSybase, _selectedSAPKernelPatchForSybase;
        private string _selectedSAPKernelUnicodeForDB2, _selectedSAPKernelVersionForDB2, _selectedSAPKernelPatchForDB2;
        private string _selectedSAPKernelUnicodeForHana, _selectedSAPKernelVersionForHana, _selectedSAPKernelPatchForHana;
        private string _selectedSAPKernelUnicodeForSAPDB, _selectedSAPKernelVersionForSAPDB, _selectedSAPKernelPatchForSAPDB;
        private string _selectedSAPKernelUnicodeForWebD, _selectedSAPKernelVersionForWebD, _selectedSAPKernelPatchForWebD;

        public bool DisplayAvailableSAPKernelPackagesForOracle
        {
            get
            {
                return SelectedServersList.Any(x => x.DBType.ToUpper().Trim() == "ORACLE" && x.ProductType.ToUpper().Trim() != "WEBD");
            }
        }
        public SAPKernelPackage SelectedSAPKernelPackageForOracle
        {
            get { return this._selectedSAPKernelPackageForOracle; }
            set
            {
                this._selectedSAPKernelPackageForOracle = value;
                this.OnPropertyChanged("SelectedSAPKernelPackageForOracle");
            }
        }
        public ObservableCollection<SAPKernelPackage> AvailableSAPKernelPatchesForOracle
        {
            get { return this._availableSAPKernelPatchesForOracle; }
            set
            {
                this._availableSAPKernelPatchesForOracle = value;
                this.OnPropertyChanged("AvailableSAPKernelPatchesForOracle");
            }
        }
        public ObservableCollection<string> SAPKernelUnicodeListForOracle
        {
            get { return this._sapKernelUnicodeListForOracle; }
            set
            {
                this._sapKernelUnicodeListForOracle = value;
                this.OnPropertyChanged("SAPKernelUnicodeListForOracle");
            }
        }
        public ObservableCollection<string> SAPKernelVersionListForOracle
        {
            get { return this._sapKernelVersionListForOracle; }
            set
            {
                this._sapKernelVersionListForOracle = value;
                this.OnPropertyChanged("SAPKernelVersionListForOracle");
            }
        }
        public ObservableCollection<string> SAPKernelPatchListForOracle
        {
            get { return this._sapKernelPatchListForOracle; }
            set
            {
                this._sapKernelPatchListForOracle = value;
                this.OnPropertyChanged("SAPKernelPatchListForOracle");
            }
        }
        public string SelectedSAPKernelUnicodeForOracle
        {
            get { return this._selectedSAPKernelUnicodeForOracle; }
            set
            {
                this._selectedSAPKernelUnicodeForOracle = value;
                this.OnPropertyChanged("SelectedSAPKernelUnicodeForOracle");
                SAPKernelVersionListForOracle = new ObservableCollection<string>();
                foreach (SAPKernelPackage pkg in SelectedProcess.SAPKernelPackages.Where(x => x.DB.Trim().ToUpper() == "ORACLE" && x.Unicode == value).ToList())
                {
                    string tempSAPVersion = SAPKernelVersionListForOracle.Where(x => x == pkg.Version).FirstOrDefault();
                    if (tempSAPVersion == null)
                        SAPKernelVersionListForOracle.Add(pkg.Version);
                }
            }
        }
        public string SelectedSAPKernelVersionForOracle
        {
            get { return this._selectedSAPKernelVersionForOracle; }
            set
            {
                this._selectedSAPKernelVersionForOracle = value;
                this.OnPropertyChanged("SelectedSAPKernelVersionForOracle");
                SAPKernelPatchListForOracle = new ObservableCollection<string>();
                foreach (SAPKernelPackage pkg in SelectedProcess.SAPKernelPackages.Where(x => x.DB.Trim().ToUpper() == "ORACLE" && x.Unicode == SelectedSAPKernelUnicodeForOracle && x.Version == value).ToList())
                {
                    string tempPatch = SAPKernelPatchListForOracle.Where(x => x == pkg.Patch).FirstOrDefault();
                    if (tempPatch == null)
                        SAPKernelPatchListForOracle.Add(pkg.Patch);
                }
            }
        }
        public string SelectedSAPKernelPatchForOracle
        {
            get { return this._selectedSAPKernelPatchForOracle; }
            set
            {
                this._selectedSAPKernelPatchForOracle = value;
                this.OnPropertyChanged("SelectedSAPKernelPatchForOracle");
                SAPKernelPackageInputsChangeForOracle();
            }
        }
        public void SAPKernelPackageInputsChangeForOracle()
        {
            if(!String.IsNullOrEmpty(SelectedSAPKernelUnicodeForOracle) && !String.IsNullOrEmpty(SelectedSAPKernelVersionForOracle) && !String.IsNullOrEmpty(SelectedSAPKernelPatchForOracle))
            {
                SAPKernelPackage tempPackage = AvailableSAPKernelPatchesForOracle.Where(x => x.Unicode == SelectedSAPKernelUnicodeForOracle && x.Version == SelectedSAPKernelVersionForOracle && x.Patch == SelectedSAPKernelPatchForOracle).FirstOrDefault();
                SelectedSAPKernelPackageForOracle = tempPackage;
                if (tempPackage != null)
                {
                    foreach (SAPKernelPackage.SAPKernelFile file in SelectedSAPKernelPackageForOracle.PackageFiles)
                    {
                        if (SelectedServersList.Where(x => x.DBType.Trim().ToUpper() == "ORACLE").Any(x => x.OS.Trim().ToUpper() == file.OS.Trim().ToUpper()))
                            file.OSMatch = true;
                        else
                            file.OSMatch = false;
                    }
                }
            }
        }

        public bool DisplayAvailableSAPKernelPackagesForSybase
        {
            get
            {
                return SelectedServersList.Any(x => x.DBType.Trim().ToUpper() == "SYBASE");
            }
        }
        public SAPKernelPackage SelectedSAPKernelPackageForSybase
        {
            get { return this._selectedSAPKernelPackageForSybase; }
            set
            {
                this._selectedSAPKernelPackageForSybase = value;
                this.OnPropertyChanged("SelectedSAPKernelPackageForSybase");
            }
        }
        public ObservableCollection<SAPKernelPackage> AvailableSAPKernelPatchesForSybase
        {
            get { return this._availableSAPKernelPatchesForSybase; }
            set
            {
                this._availableSAPKernelPatchesForSybase = value;
                this.OnPropertyChanged("AvailableSAPKernelPatchesForSybase");
            }
        }
        public ObservableCollection<string> SAPKernelUnicodeListForSybase
        {
            get { return this._sapKernelUnicodeListForSybase; }
            set
            {
                this._sapKernelUnicodeListForSybase = value;
                this.OnPropertyChanged("SAPKernelUnicodeListForSybase");
            }
        }
        public ObservableCollection<string> SAPKernelVersionListForSybase
        {
            get { return this._sapKernelVersionListForSybase; }
            set
            {
                this._sapKernelVersionListForSybase = value;
                this.OnPropertyChanged("SAPKernelVersionListForSybase");
            }
        }
        public ObservableCollection<string> SAPKernelPatchListForSybase
        {
            get { return this._sapKernelPatchListForSybase; }
            set
            {
                this._sapKernelPatchListForSybase = value;
                this.OnPropertyChanged("SAPKernelPatchListForSybase");
            }
        }
        public string SelectedSAPKernelUnicodeForSybase
        {
            get { return this._selectedSAPKernelUnicodeForSybase; }
            set
            {
                this._selectedSAPKernelUnicodeForSybase = value;
                this.OnPropertyChanged("SelectedSAPKernelUnicodeForSybase");
                SAPKernelVersionListForSybase = new ObservableCollection<string>();
                foreach (SAPKernelPackage pkg in SelectedProcess.SAPKernelPackages.Where(x => x.DB.Trim().ToUpper() == "SYBASE" && x.Unicode == value).ToList())
                {
                    string tempSAPVersion = SAPKernelVersionListForSybase.Where(x => x == pkg.Version).FirstOrDefault();
                    if (tempSAPVersion == null)
                        SAPKernelVersionListForSybase.Add(pkg.Version);
                }
            }
        }
        public string SelectedSAPKernelVersionForSybase
        {
            get { return this._selectedSAPKernelVersionForSybase; }
            set
            {
                this._selectedSAPKernelVersionForSybase = value;
                this.OnPropertyChanged("SelectedSAPKernelVersionForSybase");
                SAPKernelPatchListForSybase = new ObservableCollection<string>();
                foreach (SAPKernelPackage pkg in SelectedProcess.SAPKernelPackages.Where(x => x.DB.Trim().ToUpper() == "SYBASE" && x.Unicode == SelectedSAPKernelUnicodeForSybase && x.Version == value).ToList())
                {
                    string tempPatch = SAPKernelPatchListForSybase.Where(x => x == pkg.Patch).FirstOrDefault();
                    if (tempPatch == null)
                        SAPKernelPatchListForSybase.Add(pkg.Patch);
                }
            }
        }
        public string SelectedSAPKernelPatchForSybase
        {
            get { return this._selectedSAPKernelPatchForSybase; }
            set
            {
                this._selectedSAPKernelPatchForSybase = value;
                this.OnPropertyChanged("SelectedSAPKernelPatchForSybase");
                SAPKernelPackageInputsChangeForSybase();
            }
        }
        public void SAPKernelPackageInputsChangeForSybase()
        {
            if(!String.IsNullOrEmpty(SelectedSAPKernelUnicodeForSybase) && !String.IsNullOrEmpty(SelectedSAPKernelVersionForSybase) && !String.IsNullOrEmpty(SelectedSAPKernelPatchForSybase))
            {
                SAPKernelPackage tempPackage = AvailableSAPKernelPatchesForSybase.Where(x => x.Unicode == SelectedSAPKernelUnicodeForSybase && x.Version == SelectedSAPKernelVersionForSybase && x.Patch == SelectedSAPKernelPatchForSybase).FirstOrDefault();
                SelectedSAPKernelPackageForSybase = tempPackage;
                if (tempPackage != null)
                {
                    foreach (SAPKernelPackage.SAPKernelFile file in SelectedSAPKernelPackageForSybase.PackageFiles)
                    {
                        if (SelectedServersList.Where(x => x.DBType.Trim().ToUpper() == "SYBASE").Any(x => x.OS.Trim().ToUpper() == file.OS.Trim().ToUpper()))
                            file.OSMatch = true;
                        else
                            file.OSMatch = false;
                    }
                }
            }
        }

        public bool DisplayAvailableSAPKernelPackagesForDB2
        {
            get
            {
                return SelectedServersList.Any(x => x.DBType.Trim().ToUpper() == "DB2");
            }
        }
        public SAPKernelPackage SelectedSAPKernelPackageForDB2
        {
            get { return this._selectedSAPKernelPackageForDB2; }
            set
            {
                this._selectedSAPKernelPackageForDB2 = value;
                this.OnPropertyChanged("SelectedSAPKernelPackageForDB2");
            }
        }
        public ObservableCollection<SAPKernelPackage> AvailableSAPKernelPatchesForDB2
        {
            get { return this._availableSAPKernelPatchesForDB2; }
            set
            {
                this._availableSAPKernelPatchesForDB2 = value;
                this.OnPropertyChanged("AvailableSAPKernelPatchesForDB2");
            }
        }
        public ObservableCollection<string> SAPKernelUnicodeListForDB2
        {
            get { return this._sapKernelUnicodeListForDB2; }
            set
            {
                this._sapKernelUnicodeListForDB2 = value;
                this.OnPropertyChanged("SAPKernelUnicodeListForDB2");
            }
        }
        public ObservableCollection<string> SAPKernelVersionListForDB2
        {
            get { return this._sapKernelVersionListForDB2; }
            set
            {
                this._sapKernelVersionListForDB2 = value;
                this.OnPropertyChanged("SAPKernelVersionListForDB2");
            }
        }
        public ObservableCollection<string> SAPKernelPatchListForDB2
        {
            get { return this._sapKernelPatchListForDB2; }
            set
            {
                this._sapKernelPatchListForDB2 = value;
                this.OnPropertyChanged("SAPKernelPatchListForDB2");
            }
        }
        public string SelectedSAPKernelUnicodeForDB2
        {
            get { return this._selectedSAPKernelUnicodeForDB2; }
            set
            {
                this._selectedSAPKernelUnicodeForDB2 = value;
                this.OnPropertyChanged("SelectedSAPKernelUnicodeForDB2");
                SAPKernelVersionListForDB2 = new ObservableCollection<string>();
                foreach (SAPKernelPackage pkg in SelectedProcess.SAPKernelPackages.Where(x => x.DB.Trim().ToUpper() == "DB2" && x.Unicode == value).ToList())
                {
                    string tempSAPVersion = SAPKernelVersionListForDB2.Where(x => x == pkg.Version).FirstOrDefault();
                    if (tempSAPVersion == null)
                        SAPKernelVersionListForDB2.Add(pkg.Version);
                }
            }
        }
        public string SelectedSAPKernelVersionForDB2
        {
            get { return this._selectedSAPKernelVersionForDB2; }
            set
            {
                this._selectedSAPKernelVersionForDB2 = value;
                this.OnPropertyChanged("SelectedSAPKernelVersionForDB2");
                SAPKernelPatchListForDB2 = new ObservableCollection<string>();
                foreach (SAPKernelPackage pkg in SelectedProcess.SAPKernelPackages.Where(x => x.DB.Trim().ToUpper() == "DB2" && x.Unicode == SelectedSAPKernelUnicodeForDB2 && x.Version == value).ToList())
                {
                    string tempPatch = SAPKernelPatchListForDB2.Where(x => x == pkg.Patch).FirstOrDefault();
                    if (tempPatch == null)
                        SAPKernelPatchListForDB2.Add(pkg.Patch);
                }
            }
        }
        public string SelectedSAPKernelPatchForDB2
        {
            get { return this._selectedSAPKernelPatchForDB2; }
            set
            {
                this._selectedSAPKernelPatchForDB2 = value;
                this.OnPropertyChanged("SelectedSAPKernelPatchForDB2");
                SAPKernelPackageInputsChangeForDB2();
            }
        }
        public void SAPKernelPackageInputsChangeForDB2()
        {
            if(!String.IsNullOrEmpty(SelectedSAPKernelUnicodeForDB2) && !String.IsNullOrEmpty(SelectedSAPKernelVersionForDB2) && !String.IsNullOrEmpty(SelectedSAPKernelPatchForDB2))
            {
                SAPKernelPackage tempPackage = AvailableSAPKernelPatchesForDB2.Where(x => x.Unicode == SelectedSAPKernelUnicodeForDB2 && x.Version == SelectedSAPKernelVersionForDB2 && x.Patch == SelectedSAPKernelPatchForDB2).FirstOrDefault();
                SelectedSAPKernelPackageForDB2 = tempPackage;
                if (tempPackage != null)
                {
                    foreach (SAPKernelPackage.SAPKernelFile file in SelectedSAPKernelPackageForDB2.PackageFiles)
                    {
                        if (SelectedServersList.Where(x => x.DBType.Trim().ToUpper() == "DB2").Any(x => x.OS.Trim().ToUpper() == file.OS.Trim().ToUpper()))
                            file.OSMatch = true;
                        else
                            file.OSMatch = false;
                    }
                }
            }
        }

        public bool DisplayAvailableSAPKernelPackagesForHana
        {
            get
            {
                return SelectedServersList.Any(x => x.DBType.Trim().ToUpper() == "HANA");
            }
        }
        public SAPKernelPackage SelectedSAPKernelPackageForHana
        {
            get { return this._selectedSAPKernelPackageForHana; }
            set
            {
                this._selectedSAPKernelPackageForHana = value;
                this.OnPropertyChanged("SelectedSAPKernelPackageForHana");
            }
        }
        public ObservableCollection<SAPKernelPackage> AvailableSAPKernelPatchesForHana
        {
            get { return this._availableSAPKernelPatchesForHana; }
            set
            {
                this._availableSAPKernelPatchesForHana = value;
                this.OnPropertyChanged("AvailableSAPKernelPatchesForHana");
            }
        }
        public ObservableCollection<string> SAPKernelUnicodeListForHana
        {
            get { return this._sapKernelUnicodeListForHana; }
            set
            {
                this._sapKernelUnicodeListForHana = value;
                this.OnPropertyChanged("SAPKernelUnicodeListForHana");
            }
        }
        public ObservableCollection<string> SAPKernelVersionListForHana
        {
            get { return this._sapKernelVersionListForHana; }
            set
            {
                this._sapKernelVersionListForHana = value;
                this.OnPropertyChanged("SAPKernelVersionListForHana");
            }
        }
        public ObservableCollection<string> SAPKernelPatchListForHana
        {
            get { return this._sapKernelPatchListForHana; }
            set
            {
                this._sapKernelPatchListForHana = value;
                this.OnPropertyChanged("SAPKernelPatchListForHana");
            }
        }
        public string SelectedSAPKernelUnicodeForHana
        {
            get { return this._selectedSAPKernelUnicodeForHana; }
            set
            {
                this._selectedSAPKernelUnicodeForHana = value;
                this.OnPropertyChanged("SelectedSAPKernelUnicodeForHana");
                SAPKernelVersionListForHana = new ObservableCollection<string>();
                foreach (SAPKernelPackage pkg in SelectedProcess.SAPKernelPackages.Where(x => x.DB.Trim().ToUpper() == "HANA" && x.Unicode == value).ToList())
                {
                    string tempSAPVersion = SAPKernelVersionListForHana.Where(x => x == pkg.Version).FirstOrDefault();
                    if (tempSAPVersion == null)
                        SAPKernelVersionListForHana.Add(pkg.Version);
                }
            }
        }
        public string SelectedSAPKernelVersionForHana
        {
            get { return this._selectedSAPKernelVersionForHana; }
            set
            {
                this._selectedSAPKernelVersionForHana = value;
                this.OnPropertyChanged("SelectedSAPKernelVersionForHana");
                SAPKernelPatchListForHana = new ObservableCollection<string>();
                foreach (SAPKernelPackage pkg in SelectedProcess.SAPKernelPackages.Where(x => x.DB.Trim().ToUpper() == "HANA" && x.Unicode == SelectedSAPKernelUnicodeForHana && x.Version == value).ToList())
                {
                    string tempPatch = SAPKernelPatchListForHana.Where(x => x == pkg.Patch).FirstOrDefault();
                    if (tempPatch == null)
                        SAPKernelPatchListForHana.Add(pkg.Patch);
                }
            }
        }
        public string SelectedSAPKernelPatchForHana
        {
            get { return this._selectedSAPKernelPatchForHana; }
            set
            {
                this._selectedSAPKernelPatchForHana = value;
                this.OnPropertyChanged("SelectedSAPKernelPatchForHana");
                SAPKernelPackageInputsChangeForHana();
            }
        }
        public void SAPKernelPackageInputsChangeForHana()
        {
            if (!String.IsNullOrEmpty(SelectedSAPKernelUnicodeForHana) && !String.IsNullOrEmpty(SelectedSAPKernelVersionForHana) && !String.IsNullOrEmpty(SelectedSAPKernelPatchForHana))
            {
                SAPKernelPackage tempPackage = AvailableSAPKernelPatchesForHana.Where(x => x.Unicode == SelectedSAPKernelUnicodeForHana && x.Version == SelectedSAPKernelVersionForHana && x.Patch == SelectedSAPKernelPatchForHana).FirstOrDefault();
                SelectedSAPKernelPackageForHana = tempPackage;
                if (tempPackage != null)
                {
                    foreach (SAPKernelPackage.SAPKernelFile file in SelectedSAPKernelPackageForHana.PackageFiles)
                    {
                        if (SelectedServersList.Where(x => x.DBType.Trim().ToUpper() == "HANA").Any(x => x.OS.Trim().ToUpper() == file.OS.Trim().ToUpper()))
                            file.OSMatch = true;
                        else
                            file.OSMatch = false;
                    }
                }
            }
        }

        public bool DisplayAvailableSAPKernelPackagesForSAPDB
        {
            get
            {
                return SelectedServersList.Any(x => x.DBType.Trim().ToUpper() == "SAPDB");
            }
        }
        public SAPKernelPackage SelectedSAPKernelPackageForSAPDB
        {
            get { return this._selectedSAPKernelPackageForSAPDB; }
            set
            {
                this._selectedSAPKernelPackageForSAPDB = value;
                this.OnPropertyChanged("SelectedSAPKernelPackageForSAPDB");
            }
        }
        public ObservableCollection<SAPKernelPackage> AvailableSAPKernelPatchesForSAPDB
        {
            get { return this._availableSAPKernelPatchesForSAPDB; }
            set
            {
                this._availableSAPKernelPatchesForSAPDB = value;
                this.OnPropertyChanged("AvailableSAPKernelPatchesForSAPDB");
            }
        }
        public ObservableCollection<string> SAPKernelUnicodeListForSAPDB
        {
            get { return this._sapKernelUnicodeListForSAPDB; }
            set
            {
                this._sapKernelUnicodeListForSAPDB = value;
                this.OnPropertyChanged("SAPKernelUnicodeListForSAPDB");
            }
        }
        public ObservableCollection<string> SAPKernelVersionListForSAPDB
        {
            get { return this._sapKernelVersionListForSAPDB; }
            set
            {
                this._sapKernelVersionListForSAPDB = value;
                this.OnPropertyChanged("SAPKernelVersionListForSAPDB");
            }
        }
        public ObservableCollection<string> SAPKernelPatchListForSAPDB
        {
            get { return this._sapKernelPatchListForSAPDB; }
            set
            {
                this._sapKernelPatchListForSAPDB = value;
                this.OnPropertyChanged("SAPKernelPatchListForSAPDB");
            }
        }
        public string SelectedSAPKernelUnicodeForSAPDB
        {
            get { return this._selectedSAPKernelUnicodeForSAPDB; }
            set
            {
                this._selectedSAPKernelUnicodeForSAPDB = value;
                this.OnPropertyChanged("SelectedSAPKernelUnicodeForSAPDB");
                SAPKernelVersionListForSAPDB = new ObservableCollection<string>();
                foreach (SAPKernelPackage pkg in SelectedProcess.SAPKernelPackages.Where(x => x.DB.Trim().ToUpper() == "SAPDB" && x.Unicode == value).ToList())
                {
                    string tempSAPVersion = SAPKernelVersionListForSAPDB.Where(x => x == pkg.Version).FirstOrDefault();
                    if (tempSAPVersion == null)
                        SAPKernelVersionListForSAPDB.Add(pkg.Version);
                }
            }
        }
        public string SelectedSAPKernelVersionForSAPDB
        {
            get { return this._selectedSAPKernelVersionForSAPDB; }
            set
            {
                this._selectedSAPKernelVersionForSAPDB = value;
                this.OnPropertyChanged("SelectedSAPKernelVersionForSAPDB");
                SAPKernelPatchListForSAPDB = new ObservableCollection<string>();
                foreach (SAPKernelPackage pkg in SelectedProcess.SAPKernelPackages.Where(x => x.DB.Trim().ToUpper() == "SAPDB" && x.Unicode == SelectedSAPKernelUnicodeForSAPDB && x.Version == value).ToList())
                {
                    string tempPatch = SAPKernelPatchListForSAPDB.Where(x => x == pkg.Patch).FirstOrDefault();
                    if (tempPatch == null)
                        SAPKernelPatchListForSAPDB.Add(pkg.Patch);
                }
            }
        }
        public string SelectedSAPKernelPatchForSAPDB
        {
            get { return this._selectedSAPKernelPatchForSAPDB; }
            set
            {
                this._selectedSAPKernelPatchForSAPDB = value;
                this.OnPropertyChanged("SelectedSAPKernelPatchForSAPDB");
                SAPKernelPackageInputsChangeForSAPDB();
            }
        }
        public void SAPKernelPackageInputsChangeForSAPDB()
        {
            if(!String.IsNullOrEmpty(SelectedSAPKernelUnicodeForSAPDB) && !String.IsNullOrEmpty(SelectedSAPKernelVersionForSAPDB) && !String.IsNullOrEmpty(SelectedSAPKernelPatchForSAPDB))
            {
                SAPKernelPackage tempPackage = AvailableSAPKernelPatchesForSAPDB.Where(x => x.Unicode == SelectedSAPKernelUnicodeForSAPDB && x.Version == SelectedSAPKernelVersionForSAPDB && x.Patch == SelectedSAPKernelPatchForSAPDB).FirstOrDefault();
                SelectedSAPKernelPackageForSAPDB = tempPackage;
                if (tempPackage != null)
                {
                    foreach (SAPKernelPackage.SAPKernelFile file in SelectedSAPKernelPackageForSAPDB.PackageFiles)
                    {
                        if (SelectedServersList.Where(x => x.DBType.Trim().ToUpper() == "SAPDB").Any(x => x.OS.Trim().ToUpper() == file.OS.Trim().ToUpper()))
                            file.OSMatch = true;
                        else
                            file.OSMatch = false;
                    }
                }
            }
        }

        public bool DisplayAvailableSAPKernelPackagesForWebD
        {
            get
            {
                return SelectedServersList.Any(x => x.ProductType.Trim().ToUpper() == "WEBD");
            }
        }
        public SAPKernelPackage SelectedSAPKernelPackageForWebD
        {
            get { return this._selectedSAPKernelPackageForWebD; }
            set
            {
                this._selectedSAPKernelPackageForWebD = value;
                this.OnPropertyChanged("SelectedSAPKernelPackageForWebD");
            }
        }
        public ObservableCollection<SAPKernelPackage> AvailableSAPKernelPatchesForWebD
        {
            get { return this._availableSAPKernelPatchesForWebD; }
            set
            {
                this._availableSAPKernelPatchesForWebD = value;
                this.OnPropertyChanged("AvailableSAPKernelPatchesForWebD");
            }
        }
        public ObservableCollection<string> SAPKernelUnicodeListForWebD
        {
            get { return this._sapKernelUnicodeListForWebD; }
            set
            {
                this._sapKernelUnicodeListForWebD = value;
                this.OnPropertyChanged("SAPKernelUnicodeListForWebD");
            }
        }
        public ObservableCollection<string> SAPKernelVersionListForWebD
        {
            get { return this._sapKernelVersionListForWebD; }
            set
            {
                this._sapKernelVersionListForWebD = value;
                this.OnPropertyChanged("SAPKernelVersionListForWebD");
            }
        }
        public ObservableCollection<string> SAPKernelPatchListForWebD
        {
            get { return this._sapKernelPatchListForWebD; }
            set
            {
                this._sapKernelPatchListForWebD = value;
                this.OnPropertyChanged("SAPKernelPatchListForWebD");
            }
        }
        public string SelectedSAPKernelUnicodeForWebD
        {
            get { return this._selectedSAPKernelUnicodeForWebD; }
            set
            {
                this._selectedSAPKernelUnicodeForWebD = value;
                this.OnPropertyChanged("SelectedSAPKernelUnicodeForWebD");
                SAPKernelVersionListForWebD = new ObservableCollection<string>();
                foreach (SAPKernelPackage pkg in SelectedProcess.SAPKernelPackages.Where(x => x.DB.Trim().ToUpper() == "WEBDISP" && x.Unicode == value).ToList())
                {
                    string tempSAPVersion = SAPKernelVersionListForWebD.Where(x => x == pkg.Version).FirstOrDefault();
                    if (tempSAPVersion == null)
                        SAPKernelVersionListForWebD.Add(pkg.Version);
                }
            }
        }
        public string SelectedSAPKernelVersionForWebD
        {
            get { return this._selectedSAPKernelVersionForWebD; }
            set
            {
                this._selectedSAPKernelVersionForWebD = value;
                this.OnPropertyChanged("SelectedSAPKernelVersionForWebD");
                SAPKernelPatchListForWebD = new ObservableCollection<string>();
                foreach (SAPKernelPackage pkg in SelectedProcess.SAPKernelPackages.Where(x => x.DB.Trim().ToUpper() == "WEBDISP" && x.Unicode == SelectedSAPKernelUnicodeForWebD && x.Version == value).ToList())
                {
                    string tempPatch = SAPKernelPatchListForWebD.Where(x => x == pkg.Patch).FirstOrDefault();
                    if (tempPatch == null)
                        SAPKernelPatchListForWebD.Add(pkg.Patch);
                }
            }
        }
        public string SelectedSAPKernelPatchForWebD
        {
            get { return this._selectedSAPKernelPatchForWebD; }
            set
            {
                this._selectedSAPKernelPatchForWebD = value;
                this.OnPropertyChanged("SelectedSAPKernelPatchForWebD");
                SAPKernelPackageInputsChangeForWebD();
            }
        }
        public void SAPKernelPackageInputsChangeForWebD()
        {
            if (!String.IsNullOrEmpty(SelectedSAPKernelUnicodeForWebD) && !String.IsNullOrEmpty(SelectedSAPKernelVersionForWebD) && !String.IsNullOrEmpty(SelectedSAPKernelPatchForWebD))
            {
                SAPKernelPackage tempPackage = AvailableSAPKernelPatchesForWebD.Where(x => x.Unicode == SelectedSAPKernelUnicodeForWebD && x.Version == SelectedSAPKernelVersionForWebD && x.Patch == SelectedSAPKernelPatchForWebD).FirstOrDefault();
                SelectedSAPKernelPackageForWebD = tempPackage;
                if (tempPackage != null)
                {
                    foreach (SAPKernelPackage.SAPKernelFile file in SelectedSAPKernelPackageForWebD.PackageFiles)
                    {
                        if (SelectedServersList.Where(x => x.ProductType.Trim().ToUpper() == "WEBD").Any(x => x.OS.Trim().ToUpper() == file.OS.Trim().ToUpper()))
                            file.OSMatch = true;
                        else
                            file.OSMatch = false;
                    }
                }
            }
        }

        public bool AtLeastOneSAPKernelPackageSelectedForOracle
        {
            get
            {
                if (SelectedSAPKernelPackageForOracle != null)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public bool AtLeastOneSAPKernelPackageSelectedForSybase
        {
            get
            {
                if (SelectedSAPKernelPackageForSybase != null)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public bool AtLeastOneSAPKernelPackageSelectedForDB2
        {
            get
            {
                if (SelectedSAPKernelPackageForDB2 != null)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public bool AtLeastOneSAPKernelPackageSelectedForHana
        {
            get
            {
                if (SelectedSAPKernelPackageForHana != null)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public bool AtLeastOneSAPKernelPackageSelectedForSAPDB
        {
            get
            {
                if (SelectedSAPKernelPackageForSAPDB != null)
                {
                    return true;
                }
                else
                    return false;
            }
        }
        public bool AtLeastOneSAPKernelPackageSelectedForWebD
        {
            get
            {
                if (SelectedSAPKernelPackageForWebD != null)
                {
                    return true;
                }
                else
                    return false;
            }
        }
    }
}
