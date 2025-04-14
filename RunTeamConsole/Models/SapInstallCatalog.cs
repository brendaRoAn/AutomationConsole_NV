using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace RunTeamConsole.Models
{
    public class SapInstallCatalog : ObservableObject
    {
        private string _opSysType, _osDist, _osArch, _sapProd, _sapStack, _sapKernel, _dbName, _dbVersion, _dbPatch, _osPatch, _fileName, _controlData, _controlValue, _fileDesc, _control1, _control2, _control3, _control4, _control5;
        private bool _active;
        private List<SapCatalogFile> _files;

        public SapInstallCatalog(string opSysType, string osDist, string osArch, string sapProd, string sapStack, string sapKernel, string dbName, string dbVersion, string dbPatch, string osPatch, string fileName, string controlData, string controlValue, string fileDescription, string control1, string control2, string control3, string control4, string control5)
        {
            this._opSysType = opSysType;
            this._osDist = osDist;
            this._osArch = osArch;
            this._sapProd = sapProd;
            this._sapStack = sapStack;
            this._sapKernel = sapKernel;
            this._dbName = dbName;
            this._dbVersion = dbVersion;
            this._dbPatch = dbPatch;
            this._osPatch = osPatch;
            this._fileName = fileName;
            this._fileDesc = fileDescription;
            this._control1 = control1;
            this._control2 = control2;
            this._control3 = control3;
            this._control4 = control4;
            this._control5 = control5;
            this._files = new List<SapCatalogFile>() { new SapCatalogFile(opSysType, osDist, osArch, sapProd, sapStack, sapKernel, dbName, dbVersion, dbPatch, osPatch, fileName, controlData, controlValue, fileDescription, control1, control2, control3, control4, control5) };
        }

        public string OpSysType
        {
            get { return this._opSysType; }
            set { this._opSysType = value; }
        }

        public string OsDist
        {
            get { return this._osDist; }
            set { this._osDist = value; }
        }

        public string OsArch
        {
            get { return this._osArch; }
            set { this._osArch = value; }
        }

        public string SapProd
        {
            get { return this._sapProd; }
            set { this._sapProd = value; }
        }

        public string SapStack
        {
            get { return this._sapStack; }
            set { this._sapStack = value; }
        }

        public string SapKernel
        {
            get { return this._sapKernel; }
            set { this._sapKernel = value; }
        }

        public string DbName
        {
            get { return this._dbName; }
            set { this._dbName = value; }
        }

        public string DbVersion
        {
            get { return this._dbVersion; }
            set { this._dbVersion = value; }
        }

        public string DbPatch
        {
            get { return this._dbPatch; }
            set { this._dbPatch = value; }
        }

        public string OsPatch
        {
            get { return this._osPatch; }
            set { this._osPatch = value; }
        }

        public string FileName
        {
            get { return this._fileName; }
            set { this._fileName = value; }
        }

        public string FileDesc
        {
            get { return this._fileDesc; }
            set { this._fileDesc = value; }
        }

        public bool Active
        {
            get { return this._active; }
            set
            {
                this._active = value;
                this.OnPropertyChanged("Active");
            }
        }

        public List<SapCatalogFile> CatalogFiles
        {
            get { return this._files; }
            set { this._files = value; }
        }

        public void AddFile(string opSysType, string osDist, string osArch, string sapProd, string sapStack, string sapKernel, string dbName, string dbVersion, string dbPatch, string osPatch, string fileName, string controlData, string controlValue, string fileDescription, string control1, string control2, string control3, string control4, string control5)
        {
            SapCatalogFile pkg = new SapCatalogFile(opSysType, osDist, osArch, sapProd, sapStack, sapKernel, dbName, dbVersion, dbPatch, osPatch, fileName, controlData, controlValue, fileDescription, control1, control2, control3, control4, control5);
            this._files.Add(pkg);
        }

        public class SapCatalogFile : ObservableObject
        {
            private bool _osMatch;

            public SapCatalogFile(string opSysType, string osDist, string osArch, string sapProd, string sapStack, string sapKernel, string dbName, string dbVersion, string dbPatch, string osPatch, string fileName, string controlData, string controlValue, string fileDescription, string control1, string control2, string control3, string control4, string control5)
            {
                this.OpSysType = opSysType;
                this.OsDist = osDist;
                this.OsArch = osArch;
                this.SapProd = sapProd;
                this.SapStack = sapStack;
                this.SapKernel = sapKernel;
                this.DbName = dbName;
                this.DbVersion = dbVersion;
                this.DbPatch = dbPatch;
                this.OsPatch = osPatch;
                this.Name = fileName;
                this.ControlData = controlData;
                this.ControlValue = controlValue;
                this.Description = fileDescription;
                this.Control1 = control1;
                this.Control2 = control2;
                this.Control3 = control3;
                this.Control4 = control4;
                this.Control5 = control5;

            }
            public string OpSysType { get; set; }
            public string OsDist { get; set; }
            public string OsArch { get; set; }
            public string SapProd { get; set; }
            public string SapStack { get; set; }
            public string SapKernel { get; set; }
            public string DbName { get; set; }
            public string DbVersion { get; set; }
            public string DbPatch { get; set; }
            public string OsPatch { get; set; }
            public string Name { get; set; }
            public string ControlData { get; set; }
            public string ControlValue { get; set; }
            public string Description { get; set; }
            public string Control1 { get; set; }
            public string Control2 { get; set; }
            public string Control3 { get; set; }
            public string Control4 { get; set; }
            public string Control5 { get; set; }

            [JsonIgnore]
            public bool OSMatch
            {
                get { return this._osMatch; }
                set
                {
                    this._osMatch = value;
                    this.OnPropertyChanged("OSMatch");
                }
            }
        }
    }
}