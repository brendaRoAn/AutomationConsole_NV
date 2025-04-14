using Newtonsoft.Json;
using System.Collections.Generic;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class SapInstallPostSteps : ObservableObject
    {
        private string _fileName, _fileSourcePath;
        private LicenseFileConfiguration _licenseFile;
        public string FileName
        {
            get { return this._fileName; }
            set { this._fileName = value; }
        }
        public string FileSourcePath
        {
            get { return this._fileSourcePath; }
            set { this._fileSourcePath = value; }
        }
        public SapInstallPostSteps(string licenseFileName, string licenseFilePath)
        {
            this._fileName = licenseFileName;
            this._fileSourcePath = licenseFilePath;
            this._licenseFile = new LicenseFileConfiguration(licenseFileName, licenseFilePath);
        }
        public class LicenseFileConfiguration : ObservableObject
        {
            private string _fileName, _fileSourcePath, _fileFullName;
            public string FileName
            {
                get { return this._fileName; }
                set { this._fileName = value; }
            }
            public string FileSourcePath
            {
                get { return this._fileSourcePath; }
                set { this._fileSourcePath = value; }
            }
            public string FileFullName
            {
                get { return _fileFullName; }
                set { this._fileFullName = value; }
            }
            public LicenseFileConfiguration(string fileName, string fileSourcePath)
            {
                this._fileName = fileName;
                this._fileSourcePath = fileSourcePath;
                this._fileFullName = string.Concat(fileName, fileSourcePath);
            }


        }
        
    }
}