using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Linq;

namespace RunTeamConsole.Models.DB2Install
{
    public class Db2Install : ObservableObject
    {
        private string _db, _osType, _osDistribution, _osArchitecture, _sapProd, _sapStack, _sapKernel, _dbVersion, _dbPatch, _osPatch, _fileName, _controlData, _controlValue, _fileDescription, _control1, _control2, _control3, _control4, _control5;
        private List<Db2InstallFile> _file;
        public Db2Install(string osType, string osDistribution, string osArchitecture, string sapProd, string sapStack, string sapKernel, string database, string dbVersion, string dbPatch, string osPatch, string fileName, string controlData, string controlValue, string fileDescription, string control1, string control2, string control3, string control4, string control5)
        {
            this._osType = osType;
            this._osDistribution = osDistribution;
            this._osArchitecture = osArchitecture;
            this._sapProd = sapProd;
            this._sapStack = sapStack;
            this._sapKernel = sapKernel;
            this._db = database;
            this._dbVersion = dbVersion;
            this._dbPatch = dbPatch;
            this._osPatch = osPatch;
            this._fileName = fileName;
            this._controlData = controlData;
            this._controlValue = controlValue;
            this._fileDescription = fileDescription;
            this._control1 = control1;
            this._control2 = control2;
            this._control3 = control3;
            this._control4 = control4;
            this._control5 = control5;
            this._file = new List<Db2InstallFile>() { new Db2InstallFile(osType, osDistribution, osArchitecture, sapProd, sapStack, sapKernel, database, dbVersion, dbPatch, osPatch, fileName, controlData, controlValue, fileDescription, control1, control2, control3, control4, control5) };
        }
        
        public string OsType
        {
            get { return this._osType; }
            set { this._osType = value; }
        }
        public string OsDistribution
        {
            get { return this._osDistribution; }
            set { this._osDistribution = value; }
        }
        public string OsArchitecture
        {
            get { return this._osArchitecture; }
            set { this._osArchitecture = value; }
        }
        public string SapProduct
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
        public string Db
        {
            get { return this._db; }
            set { this._db = value; }
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
        public string ControlData
        {
            get { return this._controlData; }
            set { this._controlData = value; }
        }
        public string ControlValue
        {
            get { return this._controlValue; }
            set { this._controlValue = value; }
        }
        public string FileDescription
        {
            get { return this._fileDescription; }
            set { this._fileDescription = value; }
        }
        public string Control1
        {
            get { return this._control1; }
            set { this._control1 = value; }
        }
        public string Control2
        {
            get { return this._control2; }
            set { this._control2 = value; }
        }
        public string Control3
        {
            get { return this._control3; }
            set { this._control3 = value; }
        }
        public string Control4
        {
            get { return this._control4; }
            set { this._control4 = value; }
        }
        public string Control5
        {
            get { return this._control5; }
            set { this._control5 = value; }
        }
        public List<Db2InstallFile> File
        {
            get { return this._file; }
            set { this._file = value; }
        }

        public void AddFile(string osType, string osDistribution, string osArchitecture, string sapProd, string sapStack, string sapKernel, string database, string dbVersion, string dbPatch, string osPatch, string fileName, string controlData, string controlValue, string fileDescription, string control1, string control2, string control3, string control4, string control5)
        {
            Db2InstallFile file = new Db2InstallFile(osType, osDistribution, osArchitecture, sapProd, sapStack, sapKernel, database, dbVersion, dbPatch, osPatch, fileName, controlData, controlValue, fileDescription, control1, control2, control3, control4, control5);
            this._file.Add(file);
        }
        public class Db2InstallFile : ObservableObject
        {
            private bool _osMatch;
            public Db2InstallFile(string osType, string osDistribution, string osArchitecture, string sapProd, string sapStack, string sapKernel, string database, string dbVersion, string dbPatch, string osPatch, string fileName, string controlData, string controlValue, string fileDescription, string control1, string control2, string control3, string control4, string control5)
            {
                this.OSType = osType;
                this.OsDistribution = osDistribution;
                this.OsArchitecture = osArchitecture;
                this.SapProduct = sapProd;
                this.SapStack = sapStack;
                this.SapKernel = sapKernel;
                this.Database = database;
                this.DbVersion = dbVersion;
                this.DbPatch = dbPatch;
                this.OsPatch = osPatch;
                this.FileName = fileName;
                this.ControlData = controlData;
                this.ControlValue = controlValue;
                this.FileDescription = fileDescription;
                this.Control1 = control1;
                this.Control2 = control2;
                this.Control3 = control3;
                this.Control4 = control4;
                this.Control5 = control5;
            }
            
            public string OSType { get; set; }
            public string OsDistribution { get; set; }
            public string OsArchitecture { get; set; }
            public string DbVersion { get; set; }
            public string SapProduct { get; set; }
            public string SapStack { get; set; }
            public string SapKernel { get; set; }
            public string Database { get; set; }
            public string DbPatch { get; set; }
            public string OsPatch { get; set; }
            public string FileName { get; set; }
            public string ControlData { get; set; }
            public string ControlValue { get; set; }
            public string FileDescription { get; set; }
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