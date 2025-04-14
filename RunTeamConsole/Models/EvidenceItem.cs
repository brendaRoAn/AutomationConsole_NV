using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RunTeamConsole.Models
{
    public class EvidenceItem : ObservableObject
    {
        private string _title;
        private string _icon;
        private string _filepath;
        public EvidenceItem(string filepath)
        {
            this._filepath = filepath;
            FileInfo fi = new FileInfo(filepath);
            this._title = fi.Name;
            string icon = "";
            switch (fi.Extension.ToUpper())
            {
                case ".PNG":
                case ".JPG":
                    icon = "/img/windows/631.ico";
                    break;
                case ".OUT":
                case ".LOG":
                case ".TXT":
                    icon = "/img/windows/987.ico";
                    break;
                case ".CSV":
                case ".XLSM":
                    icon = "/img/windows/Excel-icon.png";
                    break;
                case ".HTML":
                case ".HTM":
                    icon = "/img/windows/2031.ico";
                    break;
            }
            this._icon = icon;
        }
        public string FilePath { get { return this._filepath; } }
        public string Title { get { return this._title; } set { this._title = value; } }
        public string Icon { get { return this._icon; } set { this._icon = value; } }
    }
}
