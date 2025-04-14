using RunTeamConsole.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using RunTeamConsole.Models.Packages;

namespace RunTeamConsole.ViewModels
{
    public partial class AddProcessViewModel : ObservableObject
    {
        private ObservableCollection<SapInstallCatalog> _availableSapInstallFilesForLinux;
        private ObservableCollection<string> _availableSapInstallCatalogOsVersion, _sapInstallCatalogVersionLinux, _sapInstallCatalogSapProduct, _sapInstallCatalogSapStack, _sapInstallCatalogSapKernel, _sapInstallCatalogSapDBType, _sapInstallCatalogSapDBTypeAAS, _sapInstallCatalogSapDBVersion, _sapInstallCatalogDbPatch;
        private ObservableCollection<string> _sapInstallOsType, _availableSapInstallCatalogOsPatch, _sapInstallDbPatch;
        private string _selectedSapInstallOsType, _selectedSapInstallOsPatch, _selectedSapInstallSapProduct, _selectedSapInstallSapStack, _selectedSapInstallSapKernel, _selectedSapInstallSapDBType, _selectedSapInstallSapDBTypeAAS, _selectedSapInstallSapDBVersion, _selectedSapInstallDBPatch;
        private string _selectedSapInstallCatalogOSVersion, _selectedSapInstallCatalogOSPatch, _selectedSapInstallCatalogSapProduct, _selectedSapInstallCatalogSapStack, _selectedSapInstallCatalogSapKernel, _selectedSapInstallCatalogSapDBType, _selectedSapInstallCatalogSapDBTypeAAS, _selectedSapInstallCatalogSapDBVersion, _selectedSapInstallCatalogSapDBPatch;
        private SapInstallCatalog _selectedSAPInstallCatalogForLinux, _selectedSapInstallFileForLinux, _selectedSAPInstallCatalogForLinuxHana;
        private int val = 0;
        private ObservableCollection<string> _sapInstallCatalogVersionOracle, _availableSapInstallCatalogOsVersionOracle, _sapInstallCatalogSapProductOracle, _sapInstallCatalogSapStackOracle, _sapInstallCatalogSapKernelOracle, _sapInstallCatalogSapDBTypeOracle, _sapInstallCatalogSapDBTypeOracleAAS, _sapInstallCatalogSapDBVersionOracle, _sapInstallOsTypeOracle, _availableSapInstallCatalogOsPatchOracle, _sapInstallCatalogDbPatchOracle, _sapInstallDbPatchOracle;
        private string _selectedSapInstallOsTypeOracle, _selectedSapInstallOsPatchOracle, _selectedSapInstallSapProductOracle, _selectedSapInstallSapStackOracle, _selectedSapInstallSapKernelOracle, _selectedSapInstallSapDBTypeOracle, _selectedSapInstallSapDBTypeOracleAAS, _selectedSapInstallSapDBVersionOracle, _selectedSapInstallDBPatchOracle;
        private string _selectedSapInstallCatalogOSVersionOracle, _selectedSapInstallCatalogOSPatchOracle, _selectedSapInstallCatalogSapProductOracle, _selectedSapInstallCatalogSapStackOracle, _selectedSapInstallCatalogSapKernelOracle, _selectedSapInstallCatalogSapDBTypeOracle, _selectedSapInstallCatalogSapDBTypeOracleAAS, _selectedSapInstallCatalogSapDBVersionOracle, _selectedSapInstallCatalogDBPatchOracle;
        private ObservableCollection<SapInstallCatalog> _availableSapInstallFilesForLinuxOracle;
        private SapInstallCatalog _selectedSAPInstallCatalogForLinuxOracle, _selectedSAPInstallCatalogForLinuxOracleAAS;

        #region HANA Catalog
        //==============================FUNCIÓN OBSERVABLECOLLECTION<STRING>==============================
        #region OBSERVABLECOLLECTION<STRING> for HANA
        public ObservableCollection<string> AvailableSapInstallCatalogVersionLinux
        {
            get { return this._sapInstallCatalogVersionLinux; }
            set
            {
                this._sapInstallCatalogVersionLinux = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogVersionLinux");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogOsVersion
        {
            get { return this._availableSapInstallCatalogOsVersion; }
            set
            {
                this._availableSapInstallCatalogOsVersion = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogOsVersion");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogSapProduct
        {
            get { return this._sapInstallCatalogSapProduct; }
            set
            {
                this._sapInstallCatalogSapProduct = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogSapProduct");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogSapStack
        {
            get { return this._sapInstallCatalogSapStack; }
            set
            {
                this._sapInstallCatalogSapStack = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogSapStack");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogSapKernel
        {
            get { return this._sapInstallCatalogSapKernel; }
            set
            {
                this._sapInstallCatalogSapKernel = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogSapKernel");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogSapDBType
        {
            get { return this._sapInstallCatalogSapDBType; }
            set
            {
                this._sapInstallCatalogSapDBType = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogSapDBType");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogSapDBTypeAAS
        {
            get { return this._sapInstallCatalogSapDBTypeAAS; }
            set
            {
                this._sapInstallCatalogSapDBTypeAAS = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogSapDBTypeAAS");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogSapDBVersion
        {
            get { return this._sapInstallCatalogSapDBVersion; }
            set
            {
                this._sapInstallCatalogSapDBVersion = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogSapDBVersion");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogDbPatch
        {
            get { return this._sapInstallCatalogDbPatch; }
            set
            {
                this._sapInstallCatalogDbPatch = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogDbPatch");
            }
        }
        public ObservableCollection<string> SapInstallOsType
        {
            get { return this._sapInstallOsType; }
            set
            {
                this._sapInstallOsType = value;
                this.OnPropertyChanged("SapInstallOsType");
            }
        }
        public ObservableCollection<string> SapInstallDbPatch
        {
            get { return this._sapInstallDbPatch; }
            set
            {
                this._sapInstallDbPatch = value;
                this.OnPropertyChanged("SapInstallDbPatch");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogOsPatch
        {
            get { return this._availableSapInstallCatalogOsPatch; }
            set
            {
                this._availableSapInstallCatalogOsPatch = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogOsPatch");
            }
        }
        #endregion
        //================================================================================================

        //=========================================FUNCIÓN STRING=========================================
        #region STRING for HANA
        //Asigna el valor de OsDist
        public string SelectedSapInstallOsType
        {
            get { return this._selectedSapInstallOsType; }
            set
            {
                this._selectedSapInstallOsType = value;
                this.OnPropertyChanged("SelectedSapInstallOsType");
                SapInstallOsType = new ObservableCollection<string>();
                List<string> osArchList = new List<string>();
                AvailableSapInstallCatalogOsPatch = new ObservableCollection<string>();
                foreach (SapInstallCatalog pkg in SelectedProcess.SapInstallCatalogs.Where(x => x.OpSysType.Trim().ToUpper() == "LINUX" && x.DbName.Trim().ToUpper() == "HANA" && x.OsDist == this._selectedSapInstallOsType).ToList())
                {
                    string tempSapInstallOsType = AvailableSapInstallCatalogOsPatch.Where(x => x == pkg.OsDist).FirstOrDefault();
                    if (tempSapInstallOsType == null)
                    {
                        if (osArchList.Count() == 0)
                        {
                            AvailableSapInstallCatalogOsPatch.Add(pkg.OsArch);
                            osArchList.Add(pkg.OsArch);
                        }
                        else
                        {
                            for(int i = 0; i < osArchList.Count(); i++)
                            {
                                if (osArchList[i]  == pkg.OsArch)
                                    this.val++;
                            }
                            if(this.val == 0)
                            {
                                AvailableSapInstallCatalogOsPatch.Add(pkg.OsArch);
                                osArchList.Add(pkg.OsArch);
                            }
                            this.val = 0;
                        }
                    }
                }
            }
        }
        //Asigna el valor de OsArch
        public string SelectedSapInstallOsPatch
        {
            get { return this._selectedSapInstallOsPatch; }
            set
            {
                this._selectedSapInstallOsPatch = value;
                this._selectedSapInstallCatalogOSPatch = value;
                this.OnPropertyChanged("SelectedSapInstallOsPatch");
                SapInstallOsType = new ObservableCollection<string>();
                List<string> sapProdList = new List<string>();
                AvailableSapInstallCatalogSapProduct = new ObservableCollection<string>();
                foreach (SapInstallCatalog pkg in SelectedProcess.SapInstallCatalogs.Where(x => x.OpSysType.Trim().ToUpper() == "LINUX" && x.DbName == "HANA" && x.OsArch == this._selectedSapInstallOsPatch).ToList())
                {
                    string tempSapInstallOsPatch = AvailableSapInstallCatalogSapProduct.Where(x => x == pkg.OsArch).FirstOrDefault();
                    if (this._selectedSapInstallOsType == pkg.OsDist && this._selectedSapInstallOsPatch == pkg.OsArch && tempSapInstallOsPatch == null)
                    {
                        if(sapProdList.Count() == 0)
                        {
                            AvailableSapInstallCatalogSapProduct.Add(pkg.SapProd);
                            sapProdList.Add(pkg.SapProd);
                        }
                        else
                        {
                            for(int i = 0; i < sapProdList.Count(); i++)
                            {
                                if (sapProdList[i] == pkg.SapProd)
                                    this.val++;
                            }
                            if(this.val == 0)
                            {
                                AvailableSapInstallCatalogSapProduct.Add(pkg.SapProd);
                                sapProdList.Add(pkg.SapProd);
                            }
                            this.val = 0;
                        }
                        
                    }
                }
            }
        }
        //Asigna el valor de SapProd
        public string SelectedSapInstallSapProduct
        {
            get { return this._selectedSapInstallSapProduct; }
            set
            {
                this._selectedSapInstallSapProduct = value;
                this.OnPropertyChanged("SelectedSapInstallSapProduct");
                SapInstallOsType = new ObservableCollection<string>();
                List<string> sapStackList = new List<string>();
                AvailableSapInstallCatalogSapStack = new ObservableCollection<string>();
                foreach (SapInstallCatalog pkg in SelectedProcess.SapInstallCatalogs.Where(x => x.OpSysType.Trim().ToUpper() == "LINUX" && x.DbName == "HANA" && x.OsArch == this._selectedSapInstallOsPatch && x.SapProd == this._selectedSapInstallSapProduct).ToList())
                {
                    string tempSapInstallCatalogSapProduct = AvailableSapInstallCatalogSapStack.Where(x => x == pkg.SapProd).FirstOrDefault();
                    if (this._selectedSapInstallOsType == pkg.OsDist && this._selectedSapInstallOsPatch == pkg.OsArch && this._selectedSapInstallSapProduct == pkg.SapProd && tempSapInstallCatalogSapProduct == null)
                    {
                        if (sapStackList.Count() == 0)
                        {
                            AvailableSapInstallCatalogSapStack.Add(pkg.SapStack);
                            sapStackList.Add(pkg.SapStack);
                        }
                        else
                        {
                            for (int i = 0; i < sapStackList.Count(); i++)
                            {
                                if (sapStackList[i] == pkg.SapStack)
                                    this.val++;
                            }
                            if (this.val == 0)
                            {
                                AvailableSapInstallCatalogSapStack.Add(pkg.SapStack);
                                sapStackList.Add(pkg.SapStack);
                            }
                            this.val = 0;
                        }
                    }
                }
            }
        }
        //Asigna el valor de SapStack
        public string SelectedSapInstallSapStack
        {
            get { return this._selectedSapInstallSapStack; }
            set
            {
                this._selectedSapInstallSapStack = value;
                this.OnPropertyChanged("SelectedSapInstallSapStack");
                SapInstallOsType = new ObservableCollection<string>();
                List<string> sapInstallCatalogSapKernelList = new List<string>();
                AvailableSapInstallCatalogSapKernel = new ObservableCollection<string>();
                foreach (SapInstallCatalog pkg in SelectedProcess.SapInstallCatalogs.Where(x => x.OpSysType.Trim().ToUpper() == "LINUX" && x.DbName == "HANA" && x.OsArch == this._selectedSapInstallOsPatch && x.SapProd == this._selectedSapInstallSapProduct && x.SapStack == this._selectedSapInstallSapStack).ToList())
                {
                    string tmpSapInstallCatalogSapStack = AvailableSapInstallCatalogSapKernel.Where(x => x == pkg.SapStack).FirstOrDefault();
                    if (this._selectedSapInstallOsType == pkg.OsDist && this._selectedSapInstallOsPatch == pkg.OsArch && this._selectedSapInstallSapProduct == pkg.SapProd && this._selectedSapInstallSapStack ==  pkg.SapStack && tmpSapInstallCatalogSapStack == null)
                    {
                        if(sapInstallCatalogSapKernelList.Count() == 0)
                        {
                            AvailableSapInstallCatalogSapKernel.Add(pkg.SapKernel);
                            sapInstallCatalogSapKernelList.Add(pkg.SapKernel);
                        }
                        else
                        {
                            for (int i = 0;i <  sapInstallCatalogSapKernelList.Count();i++)
                            {
                                if (sapInstallCatalogSapKernelList[i] == pkg.SapKernel)
                                    this.val++;
                            }
                            if(this.val == 0)
                            {
                                AvailableSapInstallCatalogSapKernel.Add(pkg.SapKernel);
                                sapInstallCatalogSapKernelList.Add(pkg.SapKernel);
                            }
                            this.val = 0;
                        }
                    }
                }
            }
        }
        //Asigna el valor SapKernel
        public string SelectedSapInstallSapKernel
        {
            get { return this._selectedSapInstallSapKernel; }
            set
            {
                this._selectedSapInstallSapKernel = value;
                this.OnPropertyChanged("SelectedSapInstallSapKernel");
                //SapInstallOsType = new ObservableCollection<string>();
                List<string> sapInstallCatalogSapDBTypeList = new List<string>();
                if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                    AvailableSapInstallCatalogSapDBTypeAAS = new ObservableCollection<string>();
                else
                    AvailableSapInstallCatalogSapDBType = new ObservableCollection<string>();
                foreach (SapInstallCatalog pkg in SelectedProcess.SapInstallCatalogs.Where(x => x.OpSysType.Trim().ToUpper() == "LINUX" && x.DbName == "HANA" && x.OsArch == this._selectedSapInstallOsPatch && x.SapProd == this._selectedSapInstallSapProduct && x.SapStack == this._selectedSapInstallSapStack && x.SapKernel == this._selectedSapInstallSapKernel).ToList())
                {
                    string tmpSapInstallCatalogSapKernel;
                    if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                        tmpSapInstallCatalogSapKernel = AvailableSapInstallCatalogSapDBTypeAAS.Where(x => x == pkg.SapKernel).FirstOrDefault();
                    else
                        tmpSapInstallCatalogSapKernel = AvailableSapInstallCatalogSapDBType.Where(x => x == pkg.SapKernel).FirstOrDefault();

                    if (this._selectedSapInstallOsType == pkg.OsDist && this._selectedSapInstallOsPatch == pkg.OsArch && this._selectedSapInstallSapProduct == pkg.SapProd && this._selectedSapInstallSapStack == pkg.SapStack && this._selectedSapInstallSapKernel == pkg.SapKernel && tmpSapInstallCatalogSapKernel == null)
                    {
                        if (sapInstallCatalogSapDBTypeList.Count() == 0)
                        {
                            if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                                AvailableSapInstallCatalogSapDBTypeAAS.Add(pkg.DbName);
                            else
                                AvailableSapInstallCatalogSapDBType.Add(pkg.DbName);
                            sapInstallCatalogSapDBTypeList.Add(pkg.DbName);
                        }
                        else
                        {
                            for (int i = 0; i < sapInstallCatalogSapDBTypeList.Count(); i++)
                            {
                                if (sapInstallCatalogSapDBTypeList[i] == pkg.DbName)
                                    this.val++;
                            }
                            if (this.val == 0)
                            {
                                if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                                    AvailableSapInstallCatalogSapDBTypeAAS.Add(pkg.DbName);
                                else
                                    AvailableSapInstallCatalogSapDBType.Add(pkg.DbName);
                                sapInstallCatalogSapDBTypeList.Add(pkg.DbName);
                            }
                            this.val = 0;
                        }
                    }
                }
            }
        }
        //Asigna el valor DbName para todos los procesos menos SAP Install AAS
        public string SelectedSapInstallSapDBType
        {
            get { return this._selectedSapInstallSapDBType; }
            set
            {
                this._selectedSapInstallSapDBType = value;
                this.OnPropertyChanged("SelectedSapInstallSapDBType");
                SapInstallOsType = new ObservableCollection<string>();
                List<string> sapInstallCatalogSapDBVersionList = new List<string>();
                AvailableSapInstallCatalogSapDBVersion = new ObservableCollection<string>();
                foreach (SapInstallCatalog pkg in SelectedProcess.SapInstallCatalogs.Where(x => x.OpSysType.Trim().ToUpper() == "LINUX" && x.DbName == "HANA" && x.OsArch == this._selectedSapInstallOsPatch && x.SapProd == this._selectedSapInstallSapProduct && x.SapStack == this._selectedSapInstallSapStack && x.SapKernel == this._selectedSapInstallSapKernel).ToList())
                {
                    string tmpSapInstallCatalogSapDBType = AvailableSapInstallCatalogSapDBVersion.Where(x => x == pkg.DbName).FirstOrDefault();
                    if(this._selectedSapInstallOsType == pkg.OsDist && this._selectedSapInstallOsPatch == pkg.OsArch && this._selectedSapInstallSapProduct == pkg.SapProd && this._selectedSapInstallSapStack == pkg.SapStack && this._selectedSapInstallSapKernel == pkg.SapKernel && this._selectedSapInstallSapDBType == pkg.DbName && tmpSapInstallCatalogSapDBType == null)
                    {
                        if(sapInstallCatalogSapDBVersionList.Count == 0)
                        {
                            AvailableSapInstallCatalogSapDBVersion.Add(pkg.DbVersion);
                            sapInstallCatalogSapDBVersionList.Add(pkg.DbVersion);
                        }
                        else
                        {
                            for (int i = 0; i < sapInstallCatalogSapDBVersionList.Count(); i++)
                            {
                                if (sapInstallCatalogSapDBVersionList[i] == pkg.DbVersion)
                                    this.val++;
                            }
                            if(this.val == 0)
                            {
                                AvailableSapInstallCatalogSapDBVersion.Add(pkg.DbVersion);
                                sapInstallCatalogSapDBVersionList.Add(pkg.DbVersion);
                            }
                            this.val=0;
                        }
                    }
                }
            }
        }
        //Asigna el valor DbName para el proceso SAP Install AAS
        public string SelectedSapInstallSapDBTypeAAS
        {
            get { return this._selectedSapInstallSapDBTypeAAS; }
            set
            {
                this._selectedSapInstallSapDBTypeAAS = value;
                this.OnPropertyChanged("SelectedSapInstallSapDBTypeAAS");
                SapInstallCatalogForLinuxAAS();
            }
        }
        //Asigna el valor DbVersion
        public string SelectedSapInstallSapDBVersion
        {
            get { return this._selectedSapInstallSapDBVersion; }
            set
            {
                this._selectedSapInstallSapDBVersion = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogSapDBVersion");
                SapInstallDbPatch = new ObservableCollection<string>();
                List<string> sapInstallCatalogDBPatchList = new List<string>();
                AvailableSapInstallCatalogDbPatch = new ObservableCollection<string>();
                foreach (SapInstallCatalog pkg in SelectedProcess.SapInstallCatalogs.Where(x => x.OpSysType.Trim().ToUpper() == "LINUX" && x.DbName == "HANA" && x.OsArch == this._selectedSapInstallOsPatch && x.SapProd == this._selectedSapInstallSapProduct && x.SapStack == this._selectedSapInstallSapStack && x.SapKernel == this._selectedSapInstallSapKernel && x.DbName == this._selectedSapInstallSapDBType).ToList())
                {
                    string tmpSapInstallCatalogSapDBVersion = AvailableSapInstallCatalogDbPatch.Where(x => x == pkg.DbName).FirstOrDefault();
                    if(this._selectedSapInstallOsType == pkg.OsDist && this._selectedSapInstallOsPatch == pkg.OsArch && this._selectedSapInstallSapProduct == pkg.SapProd && this._selectedSapInstallSapStack == pkg.SapStack && this._selectedSapInstallSapKernel == pkg.SapKernel && this._selectedSapInstallSapDBType == pkg.DbName && this._selectedSapInstallSapDBVersion == pkg.DbVersion && tmpSapInstallCatalogSapDBVersion == null)
                    {
                        if(sapInstallCatalogDBPatchList.Count == 0)
                        {
                            AvailableSapInstallCatalogDbPatch.Add(pkg.DbPatch);
                            sapInstallCatalogDBPatchList.Add(pkg.DbPatch);
                        }
                        else
                        {
                            for (int i = 0; i < sapInstallCatalogDBPatchList.Count(); i++)
                            {
                                if (sapInstallCatalogDBPatchList[i] == pkg.DbPatch)
                                    this.val++;
                            }
                            if(this.val == 0)
                            {
                                AvailableSapInstallCatalogDbPatch.Add(pkg.DbPatch);
                                sapInstallCatalogDBPatchList.Add(pkg.DbPatch);
                            }
                            this.val = 0;
                        }
                    }
                }
            }
        }
        public string SelectedSapInstallDBPatch
        {
            get { return this._selectedSapInstallDBPatch; }
            set
            {
                this._selectedSapInstallDBPatch = value;
                this.OnPropertyChanged("SelectedSapInstallDBPatch");
                SapInstallCatalogForLinux();
            }
        }
        public string SelectedSapInstallCatalogOSVersion
        {
            get { return this._selectedSapInstallCatalogOSVersion; }
            set
            {
                this._selectedSapInstallCatalogOSVersion = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogOSVersion");
            }
        }
        public string SelectedSapInstallCatalogOSPatch
        {
            get { return this._selectedSapInstallCatalogOSPatch; }
            set
            {
                this._selectedSapInstallCatalogOSPatch = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogOSPatch");
            }
        }
        public string SelectedSapInstallCatalogSapProduct
        {
            get { return this._selectedSapInstallCatalogSapProduct; }
            set
            {
                this._selectedSapInstallCatalogSapProduct = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogSapProduct");
            }
        }
        public string SelectedSapInstallCatalogSapStack
        {
            get { return this._selectedSapInstallCatalogSapStack; }
            set
            {
                this._selectedSapInstallCatalogSapStack = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogSapStack");
            }
        }
        public string SelectedSapInstallCatalogSapKernel
        {
            get { return this._selectedSapInstallCatalogSapKernel; }
            set
            {
                this._selectedSapInstallCatalogSapKernel = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogSapKernel");
            }
        }
        public string SelectedSapInstallCatalogSapDBType
        {
            get { return this._selectedSapInstallCatalogSapDBType; }
            set
            {
                this._selectedSapInstallCatalogSapDBType = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogSapDBType");
            }
        }
        public string SelectedSapInstallCatalogSapDBTypeAAS
        {
            get { return this._selectedSapInstallCatalogSapDBTypeAAS; }
            set
            {
                _selectedSapInstallCatalogSapDBTypeAAS = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogSapDBTypeAAS");
            }
        }
        public string SelectedSapInstallCatalogSapDBVersion
        {
            get { return this._selectedSapInstallCatalogSapDBVersion; }
            set
            {
                _selectedSapInstallCatalogSapDBVersion = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogSapDBVersion");
            }
        }
        public string SelectedSapInstallCatalogSapDBPatch
        {
            get { return this._selectedSapInstallCatalogSapDBPatch; }
            set
            {
                _selectedSapInstallCatalogSapDBPatch = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogSapDBPatch");
            }
        }
        #endregion
        //================================================================================================

        //========================FUNCIÓN OBSERVABLECOLLECTION<SAPINSTALLCATALOG>=========================
        public ObservableCollection<SapInstallCatalog> AvailableSapInstallFilesForLinux
        {
            get { return this._availableSapInstallFilesForLinux; }
            set
            {
                if (_availableSapInstallFilesForLinux != value)
                {
                    this._availableSapInstallFilesForLinux = value;
                    this.OnPropertyChanged("AvailableSapInstallFilesForLinux");
                }
            }
        }
        //================================================================================================

        //===================================FUNCIÓN SAPINSTALLCATALOG====================================
        #region SapInstallCatalog for HANA
        public SapInstallCatalog SelectedSAPInstallCatalogForLinux
        {
            get { return this._selectedSAPInstallCatalogForLinux; }
            set
            {
                this._selectedSAPInstallCatalogForLinux = value;
                this.OnPropertyChanged("SelectedSAPInstallCatalogForLinux");
            }
        }
        public SapInstallCatalog SelectedSAPInstallCatalogForLinuxAAS
        {
            get { return this._selectedSAPInstallCatalogForLinux; }
            set
            {
                this._selectedSAPInstallCatalogForLinux = value;
                this.OnPropertyChanged("SelectedSAPInstallCatalogForLinux");
            }
        }
        #endregion
        //================================================================================================

        //=========================================FUNCIÓN VOID===========================================
        #region VOID for HANA
        public void SapInstallCatalogForLinux()
        {
            if (!String.IsNullOrEmpty(SelectedSapInstallCatalogOSVersion) )//&& !String.IsNullOrEmpty(SelectedSapInstallOsPatch) && !String.IsNullOrEmpty(SelectedSapInstallSapProduct) && !String.IsNullOrEmpty(SelectedSapInstallSapDBType) && !String.IsNullOrEmpty(SelectedSapInstallCatalogSapDBVersion))
            {
                SapInstallCatalog tempCatalog = AvailableSapInstallFilesForLinux.Where(x => x.OpSysType.ToUpper() == SelectedSapInstallCatalogOSVersion && x.OsDist == SelectedSapInstallOsType && x.OsArch == SelectedSapInstallOsPatch && x.SapProd == SelectedSapInstallSapProduct && x.SapStack == SelectedSapInstallSapStack && x.SapKernel == SelectedSapInstallSapKernel && x.DbName == "HANA" && x.DbVersion == SelectedSapInstallSapDBVersion && x.DbPatch == SelectedSapInstallDBPatch).FirstOrDefault();
                SelectedSAPInstallCatalogForLinux = tempCatalog;
                if (tempCatalog != null)
                {
                    foreach (SapInstallCatalog.SapCatalogFile file in SelectedSAPInstallCatalogForLinux.CatalogFiles)
                    {
                        if (SelectedServersList.Where(x => x.OS.Trim().ToUpper() == "LINUX").Any(x => x.OS.Trim().ToUpper() == file.OpSysType.Trim().ToUpper()))
                            file.OSMatch = true;
                        else
                            file.OSMatch = false;
                    }
                }
            }
        }
        public void SapInstallCatalogForLinuxAAS()
        {
            if (!String.IsNullOrEmpty(SelectedSapInstallCatalogOSVersion))//&& !String.IsNullOrEmpty(SelectedSapInstallOsPatch) && !String.IsNullOrEmpty(SelectedSapInstallSapProduct) && !String.IsNullOrEmpty(SelectedSapInstallSapDBType) && !String.IsNullOrEmpty(SelectedSapInstallCatalogSapDBVersion))
            {
                SapInstallCatalog tempCatalog = AvailableSapInstallFilesForLinux.Where(x => x.OpSysType.ToUpper() == SelectedSapInstallCatalogOSVersion && x.OsDist == SelectedSapInstallOsType && x.OsArch == SelectedSapInstallOsPatch && x.SapProd == SelectedSapInstallSapProduct && x.SapStack == SelectedSapInstallSapStack && x.SapKernel == SelectedSapInstallSapKernel && x.DbName == "HANA").FirstOrDefault();
                SelectedSAPInstallCatalogForLinux = tempCatalog;
                if (tempCatalog != null)
                {
                    foreach (SapInstallCatalog.SapCatalogFile file in SelectedSAPInstallCatalogForLinux.CatalogFiles)
                    {
                        if (SelectedServersList.Where(x => x.OS.Trim().ToUpper() == "LINUX").Any(x => x.OS.Trim().ToUpper() == file.OpSysType.Trim().ToUpper()))
                            file.OSMatch = true;
                        else
                            file.OSMatch = false;
                    }
                }
            }
        }
        #endregion
        //================================================================================================
        //=========================================FUNCIÓN SHOW===========================================
        public bool ShowHANA
        {
            get
            {
                //if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLCLOUD") || SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLHANACLOUD") || SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                if(DbHana)
                    return true;
                else
                    return false;
            }
        }
        //================================================================================================
        #endregion

        #region ORACLE Catalog
        //==============================FUNCIÓN OBSERVABLECOLLECTION<STRING>==============================
        #region OBSERVABLECOLLECTION<STRING> for ORACLE
        public ObservableCollection<string> AvailableSapInstallCatalogVersionOracle
        {
            get { return this._sapInstallCatalogVersionOracle; }
            set
            {
                this._sapInstallCatalogVersionOracle = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogVersionOracle");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogOsVersionOracle
        {
            get { return this._availableSapInstallCatalogOsVersionOracle; }
            set
            {
                this._availableSapInstallCatalogOsVersionOracle = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogOsVersionOracle");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogSapProductOracle
        {
            get { return this._sapInstallCatalogSapProductOracle; }
            set
            {
                this._sapInstallCatalogSapProductOracle = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogSapProductOracle");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogSapStackOracle
        {
            get { return this._sapInstallCatalogSapStackOracle; }
            set
            {
                this._sapInstallCatalogSapStackOracle = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogSapStackOracle");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogSapKernelOracle
        {
            get { return this._sapInstallCatalogSapKernelOracle; }
            set
            {
                this._sapInstallCatalogSapKernelOracle = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogSapKernelOracle");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogSapDBTypeOracle
        {
            get { return this._sapInstallCatalogSapDBTypeOracle; }
            set
            {
                this._sapInstallCatalogSapDBTypeOracle = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogSapDBTypeOracle");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogSapDBTypeOracleAAS
        {
            get { return this._sapInstallCatalogSapDBTypeOracleAAS; }
            set
            {
                this._sapInstallCatalogSapDBTypeOracleAAS = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogSapDBTypeOracleAAS");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogSapDBVersionOracle
        {
            get { return this._sapInstallCatalogSapDBVersionOracle; }
            set
            {
                this._sapInstallCatalogSapDBVersionOracle = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogSapDBVersionOracle");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogDbPatchOracle
        {
            get { return this._sapInstallCatalogDbPatchOracle; }
            set
            {
                this._sapInstallCatalogDbPatchOracle = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogDbPatchOracle");
            }
        }
        public ObservableCollection<string> SapInstallOsTypeOracle
        {
            get { return this._sapInstallOsTypeOracle; }
            set
            {
                this._sapInstallOsTypeOracle = value;
                this.OnPropertyChanged("SapInstallOsTypeOracle");
            }
        }
        public ObservableCollection<string> AvailableSapInstallCatalogOsPatchOracle
        {
            get { return this._availableSapInstallCatalogOsPatchOracle; }
            set
            {
                this._availableSapInstallCatalogOsPatchOracle = value;
                this.OnPropertyChanged("AvailableSapInstallCatalogOsPatchOracle");
            }
        }
        public ObservableCollection<string> SapInstallDbPatchOracle
        {
            get { return this._sapInstallDbPatchOracle; }
            set
            {
                this._sapInstallDbPatchOracle = value;
                this.OnPropertyChanged("SapInstallDbPatchOracle");
            }
        }
        #endregion
        //================================================================================================
        //=========================================FUNCIÓN STRING=========================================
        #region STRING for ORACLE
        //Asigna el valor de OsDist
        public string SelectedSapInstallOsTypeOracle
        {
            get { return this._selectedSapInstallOsTypeOracle; }
            set
            {
                this._selectedSapInstallOsTypeOracle = value;
                this.OnPropertyChanged("SelectedSapInstallOsTypeOracle");
                SapInstallOsTypeOracle = new ObservableCollection<string>();
                List<string> osArchList = new List<string>();
                AvailableSapInstallCatalogOsPatchOracle = new ObservableCollection<string>();
                foreach (SapInstallCatalog pkg in SelectedProcess.SapInstallCatalogs.Where(x => x.OpSysType.Trim().ToUpper() == "LINUX" && x.DbName.Trim().ToUpper() == "ORACLE" && x.OsDist == this._selectedSapInstallOsTypeOracle).ToList())
                {
                    string tempSapInstallOsType = AvailableSapInstallCatalogOsPatchOracle.Where(x => x == pkg.OsDist).FirstOrDefault();
                    if (tempSapInstallOsType == null)
                    {
                        if (osArchList.Count() == 0)
                        {
                            AvailableSapInstallCatalogOsPatchOracle.Add(pkg.OsArch);
                            osArchList.Add(pkg.OsArch);
                        }
                        else
                        {
                            for (int i = 0; i < osArchList.Count(); i++)
                            {
                                if (osArchList[i] == pkg.OsArch)
                                    this.val++;
                            }
                            if (this.val == 0)
                            {
                                AvailableSapInstallCatalogOsPatchOracle.Add(pkg.OsArch);
                                osArchList.Add(pkg.OsArch);
                            }
                            this.val = 0;
                        }
                    }
                }
            }
        }
        //Asigna el valor de OsArch
        public string SelectedSapInstallOsPatchOracle
        {
            get { return this._selectedSapInstallOsPatchOracle; }
            set
            {
                this._selectedSapInstallOsPatchOracle = value;
                //this._selectedSapInstallCatalogOSPatchOracle = value;
                this.OnPropertyChanged("SelectedSapInstallOsPatchOracle");
                SapInstallOsTypeOracle = new ObservableCollection<string>();
                List<string> sapProdList = new List<string>();
                AvailableSapInstallCatalogSapProductOracle = new ObservableCollection<string>();
                foreach (SapInstallCatalog pkg in SelectedProcess.SapInstallCatalogs.Where(x => x.OpSysType.Trim().ToUpper() == "LINUX" && x.DbName == "ORACLE" && x.OsArch == this._selectedSapInstallOsPatchOracle).ToList())
                {
                    string tempSapInstallOsPatch = AvailableSapInstallCatalogSapProductOracle.Where(x => x == pkg.OsArch).FirstOrDefault();
                    if (this._selectedSapInstallOsTypeOracle == pkg.OsDist && this._selectedSapInstallOsPatchOracle == pkg.OsArch && tempSapInstallOsPatch == null)
                    {
                        if (sapProdList.Count() == 0)
                        {
                            AvailableSapInstallCatalogSapProductOracle.Add(pkg.SapProd);
                            sapProdList.Add(pkg.SapProd);
                        }
                        else
                        {
                            for (int i = 0; i < sapProdList.Count(); i++)
                            {
                                if (sapProdList[i] == pkg.SapProd)
                                    this.val++;
                            }
                            if (this.val == 0)
                            {
                                AvailableSapInstallCatalogSapProductOracle.Add(pkg.SapProd);
                                sapProdList.Add(pkg.SapProd);
                            }
                            this.val = 0;
                        }

                    }
                }
            }
        }
        //Asigna el valor de SapProd
        public string SelectedSapInstallSapProductOracle
        {
            get { return this._selectedSapInstallSapProductOracle; }
            set
            {
                this._selectedSapInstallSapProductOracle = value;
                this.OnPropertyChanged("SelectedSapInstallSapProduct");
                SapInstallOsTypeOracle = new ObservableCollection<string>();
                List<string> sapStackList = new List<string>();
                AvailableSapInstallCatalogSapStackOracle = new ObservableCollection<string>();
                foreach (SapInstallCatalog pkg in SelectedProcess.SapInstallCatalogs.Where(x => x.OpSysType.Trim().ToUpper() == "LINUX" && x.DbName == "ORACLE" && x.OsArch == this._selectedSapInstallOsPatchOracle && x.SapProd == this._selectedSapInstallSapProductOracle).ToList())
                {
                    string tempSapInstallCatalogSapProduct = AvailableSapInstallCatalogSapStack.Where(x => x == pkg.SapProd).FirstOrDefault();
                    if (this._selectedSapInstallOsTypeOracle == pkg.OsDist && this._selectedSapInstallOsPatchOracle == pkg.OsArch && this._selectedSapInstallSapProductOracle == pkg.SapProd && tempSapInstallCatalogSapProduct == null)
                    {
                        if (sapStackList.Count() == 0)
                        {
                            AvailableSapInstallCatalogSapStackOracle.Add(pkg.SapStack);
                            sapStackList.Add(pkg.SapStack);
                        }
                        else
                        {
                            for (int i = 0; i < sapStackList.Count(); i++)
                            {
                                if (sapStackList[i] == pkg.SapStack)
                                    this.val++;
                            }
                            if (this.val == 0)
                            {
                                AvailableSapInstallCatalogSapStackOracle.Add(pkg.SapStack);
                                sapStackList.Add(pkg.SapStack);
                            }
                            this.val = 0;
                        }
                    }
                }
            }
        }
        //Asigna el valor de SapStack
        public string SelectedSapInstallSapStackOracle
        {
            get { return this._selectedSapInstallSapStackOracle; }
            set
            {
                this._selectedSapInstallSapStackOracle = value;
                this.OnPropertyChanged("SelectedSapInstallSapStackOracle");
                SapInstallOsTypeOracle = new ObservableCollection<string>();
                List<string> sapInstallCatalogSapKernelList = new List<string>();
                AvailableSapInstallCatalogSapKernelOracle = new ObservableCollection<string>();
                foreach (SapInstallCatalog pkg in SelectedProcess.SapInstallCatalogs.Where(x => x.OpSysType.Trim().ToUpper() == "LINUX" && x.DbName == "ORACLE" && x.OsArch == this._selectedSapInstallOsPatchOracle && x.SapProd == this._selectedSapInstallSapProductOracle && x.SapStack == this._selectedSapInstallSapStackOracle).ToList())
                {
                    string tmpSapInstallCatalogSapStack = AvailableSapInstallCatalogSapKernelOracle.Where(x => x == pkg.SapStack).FirstOrDefault();
                    if (this._selectedSapInstallOsTypeOracle == pkg.OsDist && this._selectedSapInstallOsPatchOracle == pkg.OsArch && this._selectedSapInstallSapProductOracle == pkg.SapProd && this._selectedSapInstallSapStackOracle == pkg.SapStack && tmpSapInstallCatalogSapStack == null)
                    {
                        if (sapInstallCatalogSapKernelList.Count() == 0)
                        {
                            AvailableSapInstallCatalogSapKernelOracle.Add(pkg.SapKernel);
                            sapInstallCatalogSapKernelList.Add(pkg.SapKernel);
                        }
                        else
                        {
                            for (int i = 0; i < sapInstallCatalogSapKernelList.Count(); i++)
                            {
                                if (sapInstallCatalogSapKernelList[i] == pkg.SapKernel)
                                    this.val++;
                            }
                            if (this.val == 0)
                            {
                                AvailableSapInstallCatalogSapKernelOracle.Add(pkg.SapKernel);
                                sapInstallCatalogSapKernelList.Add(pkg.SapKernel);
                            }
                            this.val = 0;
                        }
                    }
                }
            }
        }
        //Asigna el valor SapKernel
        public string SelectedSapInstallSapKernelOracle
        {
            get { return this._selectedSapInstallSapKernelOracle; }
            set
            {
                this._selectedSapInstallSapKernelOracle = value;
                this.OnPropertyChanged("SelectedSapInstallSapKernelOracle");
                //SapInstallOsType = new ObservableCollection<string>();
                List<string> sapInstallCatalogSapDBTypeList = new List<string>();
                if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                    AvailableSapInstallCatalogSapDBTypeOracleAAS = new ObservableCollection<string>();
                else
                    AvailableSapInstallCatalogSapDBTypeOracle = new ObservableCollection<string>();
                foreach (SapInstallCatalog pkg in SelectedProcess.SapInstallCatalogs.Where(x => x.OpSysType.Trim().ToUpper() == "LINUX" && x.DbName == "ORACLE" && x.OsArch == this._selectedSapInstallOsPatchOracle && x.SapProd == this._selectedSapInstallSapProductOracle && x.SapStack == this._selectedSapInstallSapStackOracle && x.SapKernel == this._selectedSapInstallSapKernelOracle).ToList())
                {
                    string tmpSapInstallCatalogSapKernel;
                    if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                        tmpSapInstallCatalogSapKernel = AvailableSapInstallCatalogSapDBTypeOracleAAS.Where(x => x == pkg.SapKernel).FirstOrDefault();
                    else
                        tmpSapInstallCatalogSapKernel = AvailableSapInstallCatalogSapDBTypeOracle.Where(x => x == pkg.SapKernel).FirstOrDefault();

                    if (this._selectedSapInstallOsTypeOracle == pkg.OsDist && this._selectedSapInstallOsPatchOracle == pkg.OsArch && this._selectedSapInstallSapProductOracle == pkg.SapProd && this._selectedSapInstallSapStackOracle == pkg.SapStack && this._selectedSapInstallSapKernelOracle == pkg.SapKernel && tmpSapInstallCatalogSapKernel == null)
                    {
                        if (sapInstallCatalogSapDBTypeList.Count() == 0)
                        {
                            if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                                AvailableSapInstallCatalogSapDBTypeOracleAAS.Add(pkg.DbName);
                            else
                                AvailableSapInstallCatalogSapDBTypeOracle.Add(pkg.DbName);
                            sapInstallCatalogSapDBTypeList.Add(pkg.DbName);
                        }
                        else
                        {
                            for (int i = 0; i < sapInstallCatalogSapDBTypeList.Count(); i++)
                            {
                                if (sapInstallCatalogSapDBTypeList[i] == pkg.DbName)
                                    this.val++;
                            }
                            if (this.val == 0)
                            {
                                if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                                    AvailableSapInstallCatalogSapDBTypeOracleAAS.Add(pkg.DbName);
                                else
                                    AvailableSapInstallCatalogSapDBTypeOracle.Add(pkg.DbName);
                                sapInstallCatalogSapDBTypeList.Add(pkg.DbName);
                            }
                            this.val = 0;
                        }
                    }
                }
            }
        }
        //Asigna el valor DbName para todos los procesos menos SAP Install AAS
        public string SelectedSapInstallSapDBTypeOracle
        {
            get { return this._selectedSapInstallSapDBTypeOracle; }
            set
            {
                this._selectedSapInstallSapDBTypeOracle = value;
                this.OnPropertyChanged("SelectedSapInstallSapDBTypeOracle");
                SapInstallOsTypeOracle = new ObservableCollection<string>();
                List<string> sapInstallCatalogSapDBVersionList = new List<string>();
                AvailableSapInstallCatalogSapDBVersionOracle = new ObservableCollection<string>();
                foreach (SapInstallCatalog pkg in SelectedProcess.SapInstallCatalogs.Where(x => x.OpSysType.Trim().ToUpper() == "LINUX" && x.DbName == "ORACLE" && x.OsArch == this._selectedSapInstallOsPatchOracle && x.SapProd == this._selectedSapInstallSapProductOracle && x.SapStack == this._selectedSapInstallSapStackOracle && x.SapKernel == this._selectedSapInstallSapKernelOracle).ToList())
                {
                    string tmpSapInstallCatalogSapDBType = AvailableSapInstallCatalogSapDBVersion.Where(x => x == pkg.DbName).FirstOrDefault();
                    if (this._selectedSapInstallOsTypeOracle == pkg.OsDist && this._selectedSapInstallOsPatchOracle == pkg.OsArch && this._selectedSapInstallSapProductOracle == pkg.SapProd && this._selectedSapInstallSapStackOracle == pkg.SapStack && this._selectedSapInstallSapKernelOracle == pkg.SapKernel && this._selectedSapInstallSapDBTypeOracle == pkg.DbName && tmpSapInstallCatalogSapDBType == null)
                    {
                        if (sapInstallCatalogSapDBVersionList.Count == 0)
                        {
                            AvailableSapInstallCatalogSapDBVersionOracle.Add(pkg.DbVersion);
                            sapInstallCatalogSapDBVersionList.Add(pkg.DbVersion);
                        }
                        else
                        {
                            for (int i = 0; i < sapInstallCatalogSapDBVersionList.Count(); i++)
                            {
                                if (sapInstallCatalogSapDBVersionList[i] == pkg.DbVersion)
                                    this.val++;
                            }
                            if (this.val == 0)
                            {
                                AvailableSapInstallCatalogSapDBVersionOracle.Add(pkg.DbVersion);
                                sapInstallCatalogSapDBVersionList.Add(pkg.DbVersion);
                            }
                            this.val = 0;
                        }
                    }
                }
            }
        }
        //Asigna el valor DbName para el proceso SAP Install AAS
        public string SelectedSapInstallSapDBTypeOracleAAS
        {
            get { return this._selectedSapInstallSapDBTypeOracleAAS; }
            set
            {
                this._selectedSapInstallSapDBTypeOracleAAS = value;
                this.OnPropertyChanged("SelectedSapInstallSapDBTypeOracleAAS");
                SapInstallCatalogForLinuxOracleAAS();
            }
        }
        //Asigna el valor DbVersion
        public string SelectedSapInstallSapDBVersionOracle
        {
            get { return this._selectedSapInstallSapDBVersionOracle; }
            set
            {
                this._selectedSapInstallSapDBVersionOracle = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogSapDBVersionOracle");
                SapInstallDbPatchOracle = new ObservableCollection<string>();
                List<string> sapInstallCatalogDBPatchListOracle = new List<string>();
                AvailableSapInstallCatalogDbPatchOracle = new ObservableCollection<string>();
                foreach (SapInstallCatalog pkg in SelectedProcess.SapInstallCatalogs.Where(x => x.OpSysType.Trim().ToUpper() == "LINUX" && x.DbName == "ORACLE" && x.OsArch == this._selectedSapInstallOsPatchOracle && x.SapProd == this._selectedSapInstallSapProductOracle && x.SapStack == this._selectedSapInstallSapStackOracle && x.SapKernel == this._selectedSapInstallSapKernelOracle && x.DbName == this._selectedSapInstallSapDBTypeOracle).ToList())
                {
                    string tmpSapInstallCatalogSapDBVersionOracle = AvailableSapInstallCatalogDbPatchOracle.Where(x => x == pkg.DbName).FirstOrDefault();
                    if (this._selectedSapInstallOsTypeOracle == pkg.OsDist && this._selectedSapInstallOsPatchOracle == pkg.OsArch && this._selectedSapInstallSapProductOracle == pkg.SapProd && this._selectedSapInstallSapStackOracle == pkg.SapStack && this._selectedSapInstallSapKernelOracle == pkg.SapKernel && this._selectedSapInstallSapDBTypeOracle == pkg.DbName && this._selectedSapInstallSapDBVersionOracle == pkg.DbVersion && tmpSapInstallCatalogSapDBVersionOracle == null)
                    {
                        if (sapInstallCatalogDBPatchListOracle.Count == 0)
                        {
                            AvailableSapInstallCatalogDbPatchOracle.Add(pkg.DbPatch);
                            sapInstallCatalogDBPatchListOracle.Add(pkg.DbPatch);
                        }
                        else
                        {
                            for (int i = 0; i < sapInstallCatalogDBPatchListOracle.Count(); i++)
                            {
                                if (sapInstallCatalogDBPatchListOracle[i] == pkg.DbPatch)
                                    this.val++;
                            }
                            if (this.val == 0)
                            {
                                AvailableSapInstallCatalogDbPatchOracle.Add(pkg.DbPatch);
                                sapInstallCatalogDBPatchListOracle.Add(pkg.DbPatch);
                            }
                            this.val = 0;
                        }
                    }
                }
            }
        }
        public string SelectedSapInstallDBPatchOracle
        {
            get { return this._selectedSapInstallDBPatchOracle; }
            set
            {
                this._selectedSapInstallDBPatchOracle = value;
                this.OnPropertyChanged("SelectedSapInstallDBPatchOracle");
                SapInstallCatalogForLinuxOracle();
            }
        }
        public string SelectedSapInstallCatalogOSVersionOracle
        {
            get { return this._selectedSapInstallCatalogOSVersionOracle; }
            set
            {
                this._selectedSapInstallCatalogOSVersionOracle = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogOSVersionOracle");
            }
        }
        public string SelectedSapInstallCatalogOSPatchOracle
        {
            get { return this._selectedSapInstallCatalogOSPatchOracle; }
            set
            {
                this._selectedSapInstallCatalogOSPatchOracle = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogOSPatchOracle");
            }
        }
        public string SelectedSapInstallCatalogSapProductOracle
        {
            get { return this._selectedSapInstallCatalogSapProductOracle; }
            set
            {
                this._selectedSapInstallCatalogSapProductOracle = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogSapProductOracle");
            }
        }
        public string SelectedSapInstallCatalogSapStackOracle
        {
            get { return this._selectedSapInstallCatalogSapStackOracle; }
            set
            {
                this._selectedSapInstallCatalogSapStackOracle = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogSapStackOracle");
            }
        }
        public string SelectedSapInstallCatalogSapKernelOracle
        {
            get { return this._selectedSapInstallCatalogSapKernelOracle; }
            set
            {
                this._selectedSapInstallCatalogSapKernelOracle = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogSapKernelOracle");
            }
        }
        public string SelectedSapInstallCatalogSapDBTypeOracle
        {
            get { return this._selectedSapInstallCatalogSapDBTypeOracle; }
            set
            {
                this._selectedSapInstallCatalogSapDBTypeOracle = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogSapDBTypeOracle");
            }
        }
        public string SelectedSapInstallCatalogSapDBTypeOracleAAS
        {
            get { return this._selectedSapInstallCatalogSapDBTypeOracleAAS; }
            set
            {
                this._selectedSapInstallCatalogSapDBTypeOracleAAS = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogSapDBTypeOracleAAS");
            }
        }
        public string SelectedSapInstallCatalogSapDBVersionOracle
        {
            get { return this._selectedSapInstallCatalogSapDBVersionOracle; }
            set
            {
                _selectedSapInstallCatalogSapDBVersionOracle = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogSapDBVersionOracle");
            }
        }
        public string SelectedSapInstallCatalogDBPatchOracle
        {
            get { return this._selectedSapInstallCatalogDBPatchOracle; }
            set
            {
                _selectedSapInstallCatalogDBPatchOracle = value;
                this.OnPropertyChanged("SelectedSapInstallCatalogDBPatchOracle");
            }
        }
        #endregion
        //================================================================================================
        //========================FUNCIÓN OBSERVABLECOLLECTION<SAPINSTALLCATALOG>=========================
        public ObservableCollection<SapInstallCatalog> AvailableSapInstallFilesForLinuxOracle
        {
            get { return this._availableSapInstallFilesForLinuxOracle; }
            set
            {
                if (_availableSapInstallFilesForLinuxOracle != value)
                {
                    this._availableSapInstallFilesForLinuxOracle = value;
                    this.OnPropertyChanged("AvailableSapInstallFilesForLinuxOracle");
                }
            }
        }
        //================================================================================================
        //===================================FUNCIÓN SAPINSTALLCATALOG====================================
        #region SapInstallCatalog For ORACLE
        public SapInstallCatalog SelectedSAPInstallCatalogForLinuxOracle
        {
            get { return this._selectedSAPInstallCatalogForLinuxOracle; }
            set
            {
                this._selectedSAPInstallCatalogForLinuxOracle = value;
                this.OnPropertyChanged("SelectedSAPInstallCatalogForLinuxOracle");
            }
        }
        public SapInstallCatalog SelectedSAPInstallCatalogForLinuxOracleAAS
        {
            get { return this._selectedSAPInstallCatalogForLinuxOracleAAS; }
            set
            {
                this._selectedSAPInstallCatalogForLinuxOracleAAS = value;
                this.OnPropertyChanged("SelectedSAPInstallCatalogForLinuxOracleAAS");
            }
        }
        #endregion
        //================================================================================================
        //=========================================FUNCIÓN VOID===========================================
        #region VOID for ORACLE
        public void SapInstallCatalogForLinuxOracle()
        {
            if (!String.IsNullOrEmpty(SelectedSapInstallCatalogOSVersionOracle))
            {
                SapInstallCatalog tempCatalog = AvailableSapInstallFilesForLinuxOracle.Where(x => x.OpSysType.ToUpper() == SelectedSapInstallCatalogOSVersionOracle && x.OsDist == SelectedSapInstallOsTypeOracle && x.OsArch == SelectedSapInstallOsPatchOracle && x.SapProd == SelectedSapInstallSapProductOracle && x.SapStack == SelectedSapInstallSapStackOracle && x.SapKernel == SelectedSapInstallSapKernelOracle && x.DbName == "ORACLE" && x.DbVersion == SelectedSapInstallSapDBVersionOracle && x.DbPatch == SelectedSapInstallDBPatchOracle).FirstOrDefault();
                SelectedSAPInstallCatalogForLinuxOracle = tempCatalog;
                if (tempCatalog != null)
                {
                    foreach (SapInstallCatalog.SapCatalogFile file in SelectedSAPInstallCatalogForLinuxOracle.CatalogFiles)
                    {
                        if (SelectedServersList.Where(x => x.DBType.Trim().ToUpper() == "ORACLE").Any(x => x.OS.Trim().ToUpper() == file.OpSysType.Trim().ToUpper()))
                            file.OSMatch = true;
                        else
                            file.OSMatch = false;
                    }
                }
            }
        }
        public void SapInstallCatalogForLinuxOracleAAS()
        {
            if (!String.IsNullOrEmpty(SelectedSapInstallCatalogOSVersionOracle))
            {
                SapInstallCatalog tempCatalog = AvailableSapInstallFilesForLinuxOracle.Where(x => x.OpSysType.ToUpper() == SelectedSapInstallCatalogOSVersionOracle && x.OsDist == SelectedSapInstallOsTypeOracle && x.OsArch == SelectedSapInstallOsPatchOracle && x.SapProd == SelectedSapInstallSapProductOracle && x.SapStack == SelectedSapInstallSapStackOracle && x.SapKernel == SelectedSapInstallSapKernelOracle && x.DbName == "ORACLE").FirstOrDefault();
                SelectedSAPInstallCatalogForLinuxOracle = tempCatalog;
                if (tempCatalog != null)
                {
                    foreach (SapInstallCatalog.SapCatalogFile file in SelectedSAPInstallCatalogForLinuxOracle.CatalogFiles)
                    {
                        if (SelectedServersList.Where(x => x.DBType.Trim().ToUpper() == "ORACLE").Any(x => x.OS.Trim().ToUpper() == file.OpSysType.Trim().ToUpper()))
                            file.OSMatch = true;
                        else
                            file.OSMatch = false;
                    }
                }
            }
        }
        #endregion
        //================================================================================================
        //=========================================FUNCIÓN SHOW===========================================
        public bool ShowORACLE
        {
            get
            {
                //if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLORACLECLOUD") || SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                if(DbOracle)
                    return true;
                else
                    return false;
            }
        }
        //================================================================================================
        #endregion

        #region Edit SAP Virtual Hostname
        //=========================================FUNCIÓN SHOW===========================================
        public bool CantEditSapVHN
        {
            get
            {
                if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLHANACLOUD") && SelectedServersList.Count() == 1)
                    return false;
                else
                    return true;
            }
        }
        public bool CanEditSapVHN
        {
            get
            {
                if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLHANACLOUD") && SelectedServersList.Count() == 2)
                    return false;
                else
                    return true;
            }
        }
        public bool CanEditHanaDbN
        {
            get
            {
                if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLHANACLOUD") && SelectedServersList.Count() == 1)
                    return true;
                else
                    return false;
            }
        }
        public bool CantEditHanaDbN
        {
            get
            {
                if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLHANACLOUD") && SelectedServersList.Count() == 2)
                    return true;
                else
                    return false;
            }
        }
        //================================================================================================
        #endregion

        #region Show Settings
        public bool ShowAASSettings
        {
            get
            {
                if (SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                    return true;
                else
                    return false;
            }
        }
        public bool ShowAllSettings
        {
            get
            {
                if (!SelectedProcess.ProjectName.ToUpper().Equals("SAPINSTALLAASCLOUD"))
                    return true;
                else
                    return false;
            }
        }
        #endregion
    }
}