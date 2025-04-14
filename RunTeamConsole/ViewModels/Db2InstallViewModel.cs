using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using RunTeamConsole.Models;
using RunTeamConsole.Models.DB2Install;
using RunTeamConsole.Models.Packages;
using RunTeamConsole.Views.DB2Install;
using static RunTeamConsole.Models.DB2Install.Db2Install;

namespace RunTeamConsole.ViewModels
{
    public partial class AddProcessViewModel : ObservableObject
    {
        //Variables to set OS
        private readonly string linux = "LINUX", aix = "AIX";
        //Variable to set DB
        private readonly string db2 = "DB2";
        //Variables for Linux
        private ObservableCollection<string> _db2InstallCatalogLinux, _availableDb2InstallCatalogOsDistributionForLinux, _availableDb2InstallCatalogOsArchitectureForLinux, _availableDb2InstallCatalogDbVersionForLinux, _availableDb2InstallCatalogDbPatchForLinux, _db2InstallOsDistributionForLinux, _db2InstallOsArchitectureForLinux, _db2InstallDbVersionForLinux, _db2InstallDbPatchForLinux;
        private string _selectedDb2InstallOsDistributionForLinux, _selectedDb2InstallOsArchitectureForLinux, _selectedDb2InstallDbVersionForLinux, _selectedDb2InstallCatalogDbPatchForLinux, _selectedDb2InstallCatalogFinalForLinux, _selectedDb2InstallCatalogOsDistributionForLinux, _selectedDb2InstallCatalogOsArchitectureForLinux, _selectedDb2InstallCatalogDbVersionForLinux;
        private ObservableCollection<Db2Install> _availableDb2InstallFilesForLinux;
        private Db2Install _selectedDb2InstallCatalogForLinux;
        //Variables for AIX
        private ObservableCollection<string> _db2InstallCatalogAIX, _availableDb2InstallCatalogOsDistributionForAIX, _availableDb2InstallCatalogOsArchitectureForAIX, _availableDb2InstallCatalogDbVersionForAIX, _availableDb2InstallCatalogDbPatchForAIX, _db2InstallOsDistributionForAIX, _db2InstallOsArchitectureForAIX, _db2InstallDbVersionForAIX, _db2InstallDbPatchForAIX;
        private string _selectedDb2InstallOsDistributionForAIX, _selectedDb2InstallOsArchitectureForAIX, _selectedDb2InstallDbVersionForAIX, _selectedDb2InstallCatalogDbPatchForAIX, _selectedDb2InstallCatalogFinalForAIX, _selectedDb2InstallCatalogOsDistributionForAIX, _selectedDb2InstallCatalogOsArchitectureForAIX, _selectedDb2InstallCatalogDbVersionForAIX;
        private ObservableCollection<Db2Install> _availableDb2InstallFilesForAIX;
        private Db2Install _selectedDb2InstallCatalogForAIX;
        private int db2InstallVal = 0;
        //Variables for pacemaker
        private string _pacemaker;


        //==============================Display Linux===============================================================
        public bool DisplayAvailableDb2InstallPackagesForLinux
        {
            get
            {
                return SelectedServersList.Any(x => x.DBType.ToUpper().Trim() == db2 && x.OS.ToUpper().Trim() == linux);
            }
        }
        public bool AtLeastOneSelectedDb2InstallPackageForLinux
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
        //==========================================================================================================
        //==============================Display AIX=================================================================
        public bool DisplayAvailableDb2InstallPackagesForAIX
        {
            get
            {
                return SelectedServersList.Any(x => x.DBType.ToUpper().Trim() == db2 && x.OS.ToUpper().Trim() == aix);
            }
        }
        public bool AtLeastOneSelectedDb2InstallPackageForAIX
        {
            get
            {
                if (SelectedDb2InstallCatalogForAIX != null)
                {
                    return true;
                }
                else
                    return false;
            }
        }
        //==========================================================================================================

        //==============================Display Pacemaker===========================================================
        public bool DisplayPacemaker
        {
            get
            {
                if (SelectedProcess.ProjectName.ToUpper().Equals("DB2INSTALLCLOUD"))
                    return true;
                else
                    return false;
            }
        }

        public string Pacemaker
        {
            get
            {
                return this._pacemaker;
            }
            set 
            { 
                this._pacemaker = value;
                OnPropertyChanged("Pacemaker");
            }
        }
        //==========================================================================================================

        //==============================FUNCIÓN OBSERVABLECOLLECTION<STRING> FOR LINUX==============================
        #region ObservableCollection<string> For Linux
        public ObservableCollection<string> AvailableDb2InstallCatalogLinux
        {
            get { return this._db2InstallCatalogLinux; }
            set
            {
                this._db2InstallCatalogLinux = value;
                this.OnPropertyChanged("AvailableDb2InstallCatalogLinux");
            }
        }
        public ObservableCollection<string> AvailableDb2InstallCatalogOsDistributionForLinux
        {
            get { return this._availableDb2InstallCatalogOsDistributionForLinux; }
            set
            {
                this._availableDb2InstallCatalogOsDistributionForLinux = value;
                this.OnPropertyChanged("AvailableDb2InstallCatalogOsDistributionForLinux");
            }
        }
        public ObservableCollection<string> AvailableDb2InstallCatalogOsArchitectureForLinux
        {
            get { return this._availableDb2InstallCatalogOsArchitectureForLinux; }
            set
            {
                this._availableDb2InstallCatalogOsArchitectureForLinux = value;
                this.OnPropertyChanged("AvailableDb2InstallCatalogOsArchitectureForLinux");
            }
        }
        public ObservableCollection<string> AvailableDb2InstallCatalogDbVersionForLinux
        {
            get { return this._availableDb2InstallCatalogDbVersionForLinux; }
            set
            {
                this._availableDb2InstallCatalogDbVersionForLinux = value;
                this.OnPropertyChanged("AvailableDb2InstallCatalogDbVersionForLinux");
            }
        }
        public ObservableCollection<string> AvailableDb2InstallCatalogDbPatchForLinux
        {
            get { return this._availableDb2InstallCatalogDbPatchForLinux; }
            set
            {
                this._availableDb2InstallCatalogDbPatchForLinux = value;
                this.OnPropertyChanged("AvailableDb2InstallCatalogDbPatchForLinux");
            }
        }
        public ObservableCollection<string> Db2InstallOsDistributionForLinux
        {
            get { return this._db2InstallOsDistributionForLinux; }
            set
            {
                this._db2InstallOsDistributionForLinux = value;
                this.OnPropertyChanged("Db2InstallOsDistributionForLinux");
            }
        }
        public ObservableCollection<string> Db2InstallOsArchitectureForLinux
        {
            get { return this._db2InstallOsArchitectureForLinux; }
            set
            {
                this._db2InstallOsArchitectureForLinux = value;
                this.OnPropertyChanged("Db2InstallOsArchitectureForLinux");
            }
        }
        public ObservableCollection<string> Db2InstallDbVersionForLinux
        {
            get { return this._db2InstallDbVersionForLinux; }
            set
            {
                this._db2InstallDbVersionForLinux = value;
                this.OnPropertyChanged("Db2InstallDbVersionForLinux");
            }
        }
        public ObservableCollection<string> Db2InstallDbPatchForLinux
        {
            get { return this._db2InstallDbPatchForLinux; }
            set
            {
                this._db2InstallDbPatchForLinux = value;
                this.OnPropertyChanged("Db2InstallDbPatch");
            }
        }
        #endregion
        //==========================================================================================================

        //==============================FUNCIÓN OBSERVABLECOLLECTION<STRING> FOR AIX================================
        #region ObservableCollection<string> For AIX
        public ObservableCollection<string> AvailableDb2InstallCatalogAIX
        {
            get { return this._db2InstallCatalogAIX; }
            set
            {
                this._db2InstallCatalogAIX = value;
                this.OnPropertyChanged("AvailableDb2InstallCatalogAIX");
            }
        }
        public ObservableCollection<string> AvailableDb2InstallCatalogOsDistributionForAIX
        {
            get { return this._availableDb2InstallCatalogOsDistributionForAIX; }
            set
            {
                this._availableDb2InstallCatalogOsDistributionForAIX = value;
                this.OnPropertyChanged("AvailableDb2InstallCatalogOsDistributionForAIX");
            }
        }
        public ObservableCollection<string> AvailableDb2InstallCatalogOsArchitectureForAIX
        {
            get { return this._availableDb2InstallCatalogOsArchitectureForAIX; }
            set
            {
                this._availableDb2InstallCatalogOsArchitectureForAIX = value;
                this.OnPropertyChanged("AvailableDb2InstallCatalogOsArchitectureForAIX");
            }
        }
        public ObservableCollection<string> AvailableDb2InstallCatalogDbVersionForAIX
        {
            get { return this._availableDb2InstallCatalogDbVersionForAIX; }
            set
            {
                this._availableDb2InstallCatalogDbVersionForAIX = value;
                this.OnPropertyChanged("AvailableDb2InstallCatalogDbVersionForAIX");
            }
        }
        public ObservableCollection<string> AvailableDb2InstallCatalogDbPatchForAIX
        {
            get { return this._availableDb2InstallCatalogDbPatchForAIX; }
            set
            {
                this._availableDb2InstallCatalogDbPatchForAIX = value;
                this.OnPropertyChanged("AvailableDb2InstallCatalogDbPatchForAIX");
            }
        }
        public ObservableCollection<string> Db2InstallOsDistributionForAIX
        {
            get { return this._db2InstallOsDistributionForAIX; }
            set
            {
                this._db2InstallOsDistributionForAIX = value;
                this.OnPropertyChanged("Db2InstallOsDistributionForAIX");
            }
        }
        public ObservableCollection<string> Db2InstallOsArchitectureForAIX
        {
            get { return this._db2InstallOsArchitectureForAIX; }
            set
            {
                this._db2InstallOsArchitectureForAIX = value;
                this.OnPropertyChanged("Db2InstallOsArchitectureForAIX");
            }
        }
        public ObservableCollection<string> Db2InstallDbVersionForAIX
        {
            get { return this._db2InstallDbVersionForAIX; }
            set
            {
                this._db2InstallDbVersionForAIX = value;
                this.OnPropertyChanged("Db2InstallDbVersionForAIX");
            }
        }
        public ObservableCollection<string> Db2InstallDbPatchForAIX
        {
            get { return this._db2InstallDbPatchForAIX; }
            set
            {
                this._db2InstallDbPatchForAIX = value;
                this.OnPropertyChanged("Db2InstallDbPatch");
            }
        }
        #endregion
        //==========================================================================================================

        //=========================================FUNCIÓN STRING FOR LINUX=========================================
        #region string for Linux
        //Asigna el valor de OsDist
        public string SelectedDb2InstallOsDistributionForLinux
        {
            get { return this._selectedDb2InstallOsDistributionForLinux; }
            set
            {
                this._selectedDb2InstallOsDistributionForLinux = value;
                this._selectedDb2InstallCatalogOsDistributionForLinux = value;
                this.OnPropertyChanged("SelectedDb2InstallOsDistribution");
                Db2InstallOsDistributionForLinux = new ObservableCollection<string>();
                List<string> osArchList = new List<string>();
                AvailableDb2InstallCatalogOsArchitectureForLinux = new ObservableCollection<string>();
                foreach(Db2Install pkg in SelectedProcess.Db2InstallCatalogs.Where(x => x.OsType.Trim().ToUpper() == linux && x.Db == db2 && x.OsDistribution == _selectedDb2InstallOsDistributionForLinux).ToList())
                {
                    string tempOsDistribution = AvailableDb2InstallCatalogOsArchitectureForLinux.Where(x => x == pkg.OsDistribution).FirstOrDefault();
                    if(tempOsDistribution == null)
                    { 
                        if(osArchList.Count == 0)
                        {
                            AvailableDb2InstallCatalogOsArchitectureForLinux.Add(pkg.OsArchitecture);
                            osArchList.Add(pkg.OsArchitecture);
                        }
                        else
                        {
                            for (int i = 0; i < osArchList.Count(); i++)
                            {
                                if (osArchList[i] == pkg.OsArchitecture)
                                    this.db2InstallVal++;
                            }
                            if (this.db2InstallVal == 0)
                            {
                                AvailableDb2InstallCatalogOsArchitectureForLinux.Add(pkg.OsArchitecture);
                                osArchList.Add(pkg.OsArchitecture);
                            }
                            this.db2InstallVal = 0;
                        }
                    }
                }
            }
        }
        public string SelectedDb2InstallOsArchitectureForLinux
        {
            get { return this._selectedDb2InstallOsArchitectureForLinux; }
            set
            {
                this._selectedDb2InstallOsArchitectureForLinux = value;
                this._selectedDb2InstallCatalogOsArchitectureForLinux = value;
                this.OnPropertyChanged("SelectedDb2InstallOsArchitecture");
                Db2InstallOsArchitectureForLinux = new ObservableCollection<string>();
                List<string> dbVersionList = new List<string>();
                AvailableDb2InstallCatalogDbVersionForLinux = new ObservableCollection<string>();
                foreach(Db2Install pkg in SelectedProcess.Db2InstallCatalogs.Where(x => x.OsType.Trim().ToUpper() == linux && x.Db == db2 && x.OsDistribution == _selectedDb2InstallOsDistributionForLinux && x.OsArchitecture == this._selectedDb2InstallOsArchitectureForLinux).ToList())
                {
                    string tempOsArchitecture = AvailableDb2InstallCatalogDbVersionForLinux.Where(x => x == pkg.OsArchitecture).FirstOrDefault();
                    if(this._selectedDb2InstallOsDistributionForLinux == pkg.OsDistribution && this._selectedDb2InstallOsArchitectureForLinux == pkg.OsArchitecture && tempOsArchitecture == null)
                    {
                        if(dbVersionList.Count == 0)
                        {
                            AvailableDb2InstallCatalogDbVersionForLinux.Add(pkg.DbVersion);
                            dbVersionList.Add(pkg.DbVersion);
                        }
                        else
                        {
                            for (int i = 0; i < dbVersionList.Count(); i++)
                            {
                                if (dbVersionList[i] == pkg.DbVersion)
                                    this.db2InstallVal++;
                            }
                            if (this.db2InstallVal == 0)
                            {
                                AvailableDb2InstallCatalogDbVersionForLinux.Add(pkg.DbVersion);
                                dbVersionList.Add(pkg.DbVersion);
                            }
                            this.db2InstallVal = 0;
                        }
                    }
                }
            }
        }
        public string SelectedDb2InstallDbVersionForLinux
        {
            get {  return this._selectedDb2InstallDbVersionForLinux; }
            set
            {
                this._selectedDb2InstallDbVersionForLinux = value;
                this._selectedDb2InstallCatalogDbVersionForLinux = value;
                this.OnPropertyChanged("SelectedDb2InstallDbVersion");
                Db2InstallDbVersionForLinux = new ObservableCollection<string>();
                List<string> dbPatchList = new List<string>();
                AvailableDb2InstallCatalogDbPatchForLinux = new ObservableCollection<string>();
                foreach (Db2Install pkg in SelectedProcess.Db2InstallCatalogs.Where(x => x.OsType.Trim().ToUpper() == linux && x.Db == db2 && x.OsDistribution == _selectedDb2InstallOsDistributionForLinux && x.OsArchitecture == this._selectedDb2InstallOsArchitectureForLinux && x.DbVersion == this._selectedDb2InstallDbVersionForLinux).ToList())
                {
                    string tempDbVersion = AvailableDb2InstallCatalogDbPatchForLinux.Where(x => x == pkg.DbVersion).FirstOrDefault();
                    if (this._selectedDb2InstallOsDistributionForLinux == pkg.OsDistribution && this._selectedDb2InstallOsArchitectureForLinux == pkg.OsArchitecture && this._selectedDb2InstallDbVersionForLinux == pkg.DbVersion && tempDbVersion == null)
                    {
                        AvailableDb2InstallCatalogDbPatchForLinux.Add(pkg.DbPatch);
                        dbPatchList.Add(pkg.DbPatch);
                    }
                    else
                    {
                        for (int i = 0; i < dbPatchList.Count(); i++)
                        {
                            if (dbPatchList[i] == pkg.DbPatch)
                                this.db2InstallVal++;
                        }
                        if (this.db2InstallVal == 0)
                        {
                            AvailableDb2InstallCatalogDbPatchForLinux.Add(pkg.DbPatch);
                            dbPatchList.Add(pkg.DbPatch);
                        }
                        this.db2InstallVal = 0;
                    }
                }
            }
        }
        public string SelectedDb2InstallCatalogFinalForLinux
        {
            get { return this._selectedDb2InstallCatalogFinalForLinux; }
            set
            {
                this._selectedDb2InstallCatalogFinalForLinux = value;
                this.OnPropertyChanged("SelectedDb2InstallCatalogForLinux");
            }
        }
        public string SelectedDb2InstallCatalogDbPatchForLinux
        {
            get { return this._selectedDb2InstallCatalogDbPatchForLinux; }
            set
            {
                this._selectedDb2InstallCatalogDbPatchForLinux = value;
                this.OnPropertyChanged("SelectedDb2InstallCatalogDbPatchForLinux");
                Db2InstallCatalogForLinux();
            }
        }
        public string SelectedDb2InstallCatalogOsDistributionForLinux
        {
            get { return this._selectedDb2InstallCatalogOsDistributionForLinux; }
            set
            {
                this._selectedDb2InstallCatalogOsDistributionForLinux = value;
                this.OnPropertyChanged("SelectedDb2InstallCatalogOsDistributionForLinux");
            }
        }
        public string SelectedDb2InstallCatalogOsArchitectureForLinux
        {
            get { return this._selectedDb2InstallCatalogOsArchitectureForLinux; }
            set
            {
                this._selectedDb2InstallCatalogOsArchitectureForLinux = value;
                this.OnPropertyChanged("SelectedDb2InstallCatalogOsArchitectureForLinux");
            }
        }
        public string SelectedDb2InstallCatalogDbVersionForLinux
        {
            get { return this._selectedDb2InstallCatalogDbVersionForLinux; }
            set
            {
                this._selectedDb2InstallCatalogDbVersionForLinux = value;
                this.OnPropertyChanged("SelectedDb2InstallCatalogDbVersionForLinux");
            }
        }
        #endregion
        //==========================================================================================================

        //=========================================FUNCIÓN STRING FOR AIX===========================================
        #region string for AIX
        //Asigna el valor de OsDist
        public string SelectedDb2InstallOsDistributionForAIX
        {
            get { return this._selectedDb2InstallOsDistributionForAIX; }
            set
            {
                this._selectedDb2InstallOsDistributionForAIX = value;
                this._selectedDb2InstallCatalogOsDistributionForAIX = value;
                this.OnPropertyChanged("SelectedDb2InstallOsDistribution");
                Db2InstallOsDistributionForAIX = new ObservableCollection<string>();
                List<string> osArchList = new List<string>();
                AvailableDb2InstallCatalogOsArchitectureForAIX = new ObservableCollection<string>();
                foreach (Db2Install pkg in SelectedProcess.Db2InstallCatalogs.Where(x => x.OsType.Trim().ToUpper() == aix && x.Db == db2 && x.OsDistribution == _selectedDb2InstallOsDistributionForAIX).ToList())
                {
                    string tempOsDistribution = AvailableDb2InstallCatalogOsArchitectureForAIX.Where(x => x == pkg.OsDistribution).FirstOrDefault();
                    if (tempOsDistribution == null)
                    {
                        if (osArchList.Count == 0)
                        {
                            AvailableDb2InstallCatalogOsArchitectureForAIX.Add(pkg.OsArchitecture);
                            osArchList.Add(pkg.OsArchitecture);
                        }
                        else
                        {
                            for (int i = 0; i < osArchList.Count(); i++)
                            {
                                if (osArchList[i] == pkg.OsArchitecture)
                                    this.db2InstallVal++;
                            }
                            if (this.db2InstallVal == 0)
                            {
                                AvailableDb2InstallCatalogOsArchitectureForAIX.Add(pkg.OsArchitecture);
                                osArchList.Add(pkg.OsArchitecture);
                            }
                            this.db2InstallVal = 0;
                        }
                    }
                }
            }
        }
        public string SelectedDb2InstallOsArchitectureForAIX
        {
            get { return this._selectedDb2InstallOsArchitectureForAIX; }
            set
            {
                this._selectedDb2InstallOsArchitectureForAIX = value;
                this._selectedDb2InstallCatalogOsArchitectureForAIX = value;
                this.OnPropertyChanged("SelectedDb2InstallOsArchitecture");
                Db2InstallOsArchitectureForAIX = new ObservableCollection<string>();
                List<string> dbVersionList = new List<string>();
                AvailableDb2InstallCatalogDbVersionForAIX = new ObservableCollection<string>();
                foreach (Db2Install pkg in SelectedProcess.Db2InstallCatalogs.Where(x => x.OsType.Trim().ToUpper() == aix && x.Db == db2 && x.OsDistribution == _selectedDb2InstallOsDistributionForAIX && x.OsArchitecture == this._selectedDb2InstallOsArchitectureForAIX).ToList())
                {
                    string tempOsArchitecture = AvailableDb2InstallCatalogDbVersionForAIX.Where(x => x == pkg.OsArchitecture).FirstOrDefault();
                    if (this._selectedDb2InstallOsDistributionForAIX == pkg.OsDistribution && this._selectedDb2InstallOsArchitectureForAIX == pkg.OsArchitecture && tempOsArchitecture == null)
                    {
                        if (dbVersionList.Count == 0)
                        {
                            AvailableDb2InstallCatalogDbVersionForAIX.Add(pkg.DbVersion);
                            dbVersionList.Add(pkg.DbVersion);
                        }
                        else
                        {
                            for (int i = 0; i < dbVersionList.Count(); i++)
                            {
                                if (dbVersionList[i] == pkg.DbVersion)
                                    this.db2InstallVal++;
                            }
                            if (this.db2InstallVal == 0)
                            {
                                AvailableDb2InstallCatalogDbVersionForAIX.Add(pkg.DbVersion);
                                dbVersionList.Add(pkg.DbVersion);
                            }
                            this.db2InstallVal = 0;
                        }
                    }
                }
            }
        }
        public string SelectedDb2InstallDbVersionForAIX
        {
            get { return this._selectedDb2InstallDbVersionForAIX; }
            set
            {
                this._selectedDb2InstallDbVersionForAIX = value;
                this._selectedDb2InstallCatalogDbVersionForAIX = value;
                this.OnPropertyChanged("SelectedDb2InstallDbVersion");
                Db2InstallDbVersionForAIX = new ObservableCollection<string>();
                List<string> dbPatchList = new List<string>();
                AvailableDb2InstallCatalogDbPatchForAIX = new ObservableCollection<string>();
                foreach (Db2Install pkg in SelectedProcess.Db2InstallCatalogs.Where(x => x.OsType.Trim().ToUpper() == aix && x.Db == db2 && x.OsDistribution == _selectedDb2InstallOsDistributionForAIX && x.OsArchitecture == this._selectedDb2InstallOsArchitectureForAIX && x.DbVersion == this._selectedDb2InstallDbVersionForAIX).ToList())
                {
                    string tempDbVersion = AvailableDb2InstallCatalogDbPatchForAIX.Where(x => x == pkg.DbVersion).FirstOrDefault();
                    if (this._selectedDb2InstallOsDistributionForAIX == pkg.OsDistribution && this._selectedDb2InstallOsArchitectureForAIX == pkg.OsArchitecture && this._selectedDb2InstallDbVersionForAIX == pkg.DbVersion && tempDbVersion == null)
                    {
                        AvailableDb2InstallCatalogDbPatchForAIX.Add(pkg.DbPatch);
                        dbPatchList.Add(pkg.DbPatch);
                    }
                    else
                    {
                        for (int i = 0; i < dbPatchList.Count(); i++)
                        {
                            if (dbPatchList[i] == pkg.DbPatch)
                                this.db2InstallVal++;
                        }
                        if (this.db2InstallVal == 0)
                        {
                            AvailableDb2InstallCatalogDbPatchForAIX.Add(pkg.DbPatch);
                            dbPatchList.Add(pkg.DbPatch);
                        }
                        this.db2InstallVal = 0;
                    }
                }
            }
        }
        public string SelectedDb2InstallCatalogFinalForAIX
        {
            get { return this._selectedDb2InstallCatalogFinalForAIX; }
            set
            {
                this._selectedDb2InstallCatalogFinalForAIX = value;
                this.OnPropertyChanged("SelectedDb2InstallCatalogForAIX");
            }
        }
        public string SelectedDb2InstallCatalogDbPatchForAIX
        {
            get { return this._selectedDb2InstallCatalogDbPatchForAIX; }
            set
            {
                this._selectedDb2InstallCatalogDbPatchForAIX = value;
                this.OnPropertyChanged("SelectedDb2InstallCatalogDbPatchForAIX");
                Db2InstallCatalogForAIX();
            }
        }
        public string SelectedDb2InstallCatalogOsDistributionForAIX
        {
            get { return this._selectedDb2InstallCatalogOsDistributionForAIX; }
            set
            {
                this._selectedDb2InstallCatalogOsDistributionForAIX = value;
                this.OnPropertyChanged("SelectedDb2InstallCatalogOsDistributionForAIX");
            }
        }
        public string SelectedDb2InstallCatalogOsArchitectureForAIX
        {
            get { return this._selectedDb2InstallCatalogOsArchitectureForAIX; }
            set
            {
                this._selectedDb2InstallCatalogOsArchitectureForAIX = value;
                this.OnPropertyChanged("SelectedDb2InstallCatalogOsArchitectureForAIX");
            }
        }
        public string SelectedDb2InstallCatalogDbVersionForAIX
        {
            get { return this._selectedDb2InstallCatalogDbVersionForAIX; }
            set
            {
                this._selectedDb2InstallCatalogDbVersionForAIX = value;
                this.OnPropertyChanged("SelectedDb2InstallCatalogDbVersionForAIX");
            }
        }
        #endregion
        //==========================================================================================================

        //========================FUNCIÓN OBSERVABLECOLLECTION<DB2INSTALL> FOR LINUX================================
        public ObservableCollection<Db2Install> AvailableDb2InstallFilesForLinux
        {
            get { return this._availableDb2InstallFilesForLinux; }
            set
            {
                if (_availableDb2InstallFilesForLinux != value)
                {
                    this._availableDb2InstallFilesForLinux = value;
                    this.OnPropertyChanged("AvailabledB2InstallFilesForLinux");
                }
            }
        }
        //==========================================================================================================

        //========================FUNCIÓN OBSERVABLECOLLECTION<DB2INSTALL> FOR AIX==================================
        public ObservableCollection<Db2Install> AvailableDb2InstallFilesForAIX
        {
            get { return this._availableDb2InstallFilesForAIX; }
            set
            {
                if (_availableDb2InstallFilesForAIX != value)
                {
                    this._availableDb2InstallFilesForAIX = value;
                    this.OnPropertyChanged("AvailabledB2InstallFilesForAIX");
                }
            }
        }
        //==========================================================================================================

        //===================================FUNCIÓN DB2INSTALL FOR LINUX===========================================
        public Db2Install SelectedDb2InstallCatalogForLinux
        {
            get { return this._selectedDb2InstallCatalogForLinux; }
            set
            {
                this._selectedDb2InstallCatalogForLinux = value;
                this.OnPropertyChanged("SelectedDb2InstallCatalogForLinux");
            }
        }
        //==========================================================================================================

        //===================================FUNCIÓN DB2INSTALL FOR AIX=============================================
        public Db2Install SelectedDb2InstallCatalogForAIX
        {
            get { return this._selectedDb2InstallCatalogForAIX; }
            set
            {
                this._selectedDb2InstallCatalogForAIX = value;
                this.OnPropertyChanged("SelectedDb2InstallCatalogForAIX");
            }
        }
        //==========================================================================================================

        //=========================================FUNCIÓN VOID FOR LINUX===========================================
        public void Db2InstallCatalogForLinux()
        {
            if(!String.IsNullOrEmpty(SelectedDb2InstallOsDistributionForLinux) && !String.IsNullOrEmpty(SelectedDb2InstallOsArchitectureForLinux) && !String.IsNullOrEmpty(SelectedDb2InstallDbVersionForLinux) && !String.IsNullOrEmpty(SelectedDb2InstallCatalogDbPatchForLinux))
            {
                Db2Install tempCatalog = AvailableDb2InstallFilesForLinux.Where(x => x.OsType == linux && x.Db == db2 && x.OsDistribution == SelectedDb2InstallCatalogOsDistributionForLinux && x.OsArchitecture == SelectedDb2InstallCatalogOsArchitectureForLinux && x.DbVersion == SelectedDb2InstallCatalogDbVersionForLinux && x.DbPatch == SelectedDb2InstallCatalogDbPatchForLinux).FirstOrDefault();
                SelectedDb2InstallCatalogForLinux = tempCatalog;
                if (tempCatalog != null)
                {
                    foreach (Db2Install.Db2InstallFile file in SelectedDb2InstallCatalogForLinux.File)
                    {
                        if (SelectedServersList.Where(x => x.OS.Trim().ToUpper() == linux && x.DBType == db2).Any(x => x.OS.Trim().ToUpper() == file.OSType.Trim().ToUpper()))
                            file.OSMatch = true;
                        else
                            file.OSMatch = false;
                    }
                }
            }
        }
        //==========================================================================================================

        //=========================================FUNCIÓN VOID FOR AIX=============================================
        public void Db2InstallCatalogForAIX()
        {
            if (!String.IsNullOrEmpty(SelectedDb2InstallOsDistributionForAIX) && !String.IsNullOrEmpty(SelectedDb2InstallOsArchitectureForAIX) && !String.IsNullOrEmpty(SelectedDb2InstallDbVersionForAIX) && !String.IsNullOrEmpty(SelectedDb2InstallCatalogDbPatchForAIX))
            {
                Db2Install tempCatalog = AvailableDb2InstallFilesForAIX.Where(x => x.OsType == aix && x.Db == db2 && x.OsDistribution == SelectedDb2InstallCatalogOsDistributionForAIX && x.OsArchitecture == SelectedDb2InstallCatalogOsArchitectureForAIX && x.DbVersion == SelectedDb2InstallCatalogDbVersionForAIX && x.DbPatch == SelectedDb2InstallCatalogDbPatchForAIX).FirstOrDefault();
                SelectedDb2InstallCatalogForAIX = tempCatalog;
                if (tempCatalog != null)
                {
                    foreach (Db2Install.Db2InstallFile file in SelectedDb2InstallCatalogForAIX.File)
                    {
                        if (SelectedServersList.Where(x => x.OS.Trim().ToUpper() == aix && x.DBType == db2).Any(x => x.OS.Trim().ToUpper() == file.OSType.Trim().ToUpper()))
                            file.OSMatch = true;
                        else
                            file.OSMatch = false;
                    }
                }
            }
        }
        //==========================================================================================================
    }
}