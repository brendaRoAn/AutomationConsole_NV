using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RunTeamConsole.Models
{
    public partial class Credentials : ObservableObject
    {
        private string _sapguiuser;
        private string _sapguipass;
        private string _osuser;
        private string _ospass;
        private string _sidadmuser;
        private string _sidadmpass;
        private string _dbuser;
        private string _dbpass;
        private string _webuser;
        private string _webpass;
        private string _dbschemapass;
        private string _targetITOPERPass;
        private string _targetDDICPass;
        private string _targetDDICCustomerPass;
        private bool _showWebCredentials;
        private bool _showSAPCredentials;
        private bool _showSAPClientListCredentials;
        private bool _showOSCredentials;
        private bool _showSIDADMCredentials;
        private bool _showDBCredentials;
        private bool _showSchemaPassword;
        private bool _showDBGroupBox;
        private bool _showSAPGroupBox;
        private List<ClientSet> _clientsList;

        [JsonConstructor]
        public Credentials(string webuser, string webpass, string sapguiuser, string sapguipass, string osuser, string ospass, string sidadmuser, string sidadmpass, string dbuser, string dbpass, string dbschemapass, List<ClientSet> ClientsList)
        {
            this._sapguiuser = sapguiuser;
            this._sapguipass = sapguipass;
            if (sapguiuser != "" && sapguipass != "")
                this._showSAPCredentials = true;
            this._osuser = osuser;
            this._ospass = ospass;
            if (osuser != "" || ospass != "")
                this._showOSCredentials = true;
            this._sidadmuser = sidadmuser;
            this._sidadmpass = sidadmpass;
            if (sidadmuser != "" && sidadmpass != "")
                this._showSIDADMCredentials = true;
            this._dbuser = dbuser;
            this._dbpass = dbpass;
            if (dbuser != "" && dbpass != "")
                this._showDBCredentials = true;
            this._webuser = webuser;
            this._webpass = webpass;
            if (webuser != "" && webpass != "")
                this._showWebCredentials = true;
            this._dbschemapass = dbschemapass;
            if (dbschemapass != "")
                this._showSchemaPassword = true;
            if (_showDBCredentials || _showSchemaPassword)
                this._showDBGroupBox = true;
            if (_showSAPCredentials || _showSIDADMCredentials)
                this._showSAPGroupBox = true;
            if (ClientsList != null)
            {
                this._clientsList = new List<ClientSet>(ClientsList);
                _showSAPClientListCredentials = true;
            }
            else
                this._clientsList = new List<ClientSet>();
        }

        public Credentials(bool webcred, bool sapguicred, bool oscred, bool sidadmcred, bool dbcred, bool schemapass)
        {
            this._showWebCredentials = webcred;
            this._showSAPCredentials = sapguicred;
            this._showOSCredentials = oscred;
            this._showSIDADMCredentials = sidadmcred;
            this._showDBCredentials = dbcred;
            this._showSchemaPassword = schemapass;
            if (_showDBCredentials || _showSchemaPassword)
                this._showDBGroupBox = true;
            if (_showSAPCredentials || _showSIDADMCredentials)
                this._showSAPGroupBox = true;
            this._webuser = "";
            this._webpass = "";
            this._sapguiuser = "";
            this._sapguipass = "";
            //this._osuser = "";
            this._osuser = UserProfile.ItUser.ToLower();
            this._ospass = "";
            this._sidadmuser = "";
            this._sidadmpass = "";
            this._dbuser = "";
            this._dbpass = "";
            this._dbschemapass = "";
            this._targetITOPERPass = "";
            this._targetDDICPass = "";
            this._targetDDICCustomerPass = "";
            this._clientsList = new List<ClientSet>();
        }

        public string WebUser
        {
            get { return this._webuser; }
            set 
            { 
                this._webuser = value;
                this.OnPropertyChanged("WebUser");
            }
        }
        public string WebPass
        {
            get { return this._webpass; }
            set 
            { 
                this._webpass = value;
                this.OnPropertyChanged("WebPass");
            }
        }
        public string SAPGuiUser
        {
            get { return this._sapguiuser; }
            set 
            {
                this._sapguiuser = value;
                this.OnPropertyChanged("SAPGuiUser");
            }
        }
        public string SAPGuiPass
        {
            get { return this._sapguipass; }
            set 
            { 
                this._sapguipass = value;
                this.OnPropertyChanged("SAPGuiPass");
            }
        }
        public string OSUser
        {
            get { return this._osuser; }
            set 
            {
                this._osuser = value;
                this.OnPropertyChanged("OSUser");
            }
        }
        public string OSPass
        {
            get { return this._ospass; }
            set 
            {
                this._ospass = value;
                this.OnPropertyChanged("OSPass");
            }
        }
        public string OSPassEncoded
        {
            get { return Auxiliar.Base64_Decode(this._ospass); }
            //set { this._ospass = value; }
        }
        public string SIDAdmUser
        {
            get { return this._sidadmuser; }
            set 
            {
                this._sidadmuser = value;
                this.OnPropertyChanged("SIDAdmUser");
            }
        }
        public string SIDAdmPass
        {
            get { return this._sidadmpass; }
            set 
            {
                this._sidadmpass = value;
                this.OnPropertyChanged("SIDAdmPass");
            }
        }
        public string DBUser
        {
            get { return this._dbuser; }
            set 
            {
                this._dbuser = value;
                this.OnPropertyChanged("DBUser");
            }
        }
        public string DBPass
        {
            get { return this._dbpass; }
            set 
            {
                this._dbpass = value;
                this.OnPropertyChanged("DBPass");
            }
        }
        public string DBSchemaPass
        {
            get { return this._dbschemapass; }
            set 
            {
                this._dbschemapass = value;
                this.OnPropertyChanged("DBSchemaPass");
            }
        }
        public string TargetITOPERPass
        {
            get { return this._targetITOPERPass; }
            set
            {
                this._targetITOPERPass = value;
                this.OnPropertyChanged("TargetITOPERPass");
            }
        }
        public string TargetDDICPass
        {
            get { return this._targetDDICPass; }
            set
            {
                this._targetDDICPass = value;
                this.OnPropertyChanged("TargetDDICPass");
            }
        }
        public string TargetDDICCustomerPass
        {
            get { return this._targetDDICCustomerPass; }
            set
            {
                this._targetDDICCustomerPass = value;
                this.OnPropertyChanged("TargetDDICCustomerPass");
            }
        }

        public bool ShowWebCredentials
        {
            get { return this._showWebCredentials; }
            //set { this._showWebCredentials = value; }
        }
        public bool ShowSAPCredentials
        {
            get { return this._showSAPCredentials; }
            //set { this._showWebCredentials = value; }
        }
        public bool ShowOSCredentials
        {
            get { return this._showOSCredentials; }
            //set { this._showWebCredentials = value; }
        }
        public bool ShowSIDADMCredentials
        {
            get { return this._showSIDADMCredentials; }
            //set { this._showWebCredentials = value; }
        }
        public bool ShowDBCredentials
        {
            get { return this._showDBCredentials; }
            //set { this._showWebCredentials = value; }
        }
        public bool ShowSchemaPassword
        {
            get { return this._showSchemaPassword; }
            //set { this._showWebCredentials = value; }
        }
        public bool ShowDBGroupBox
        {
            get { return this._showDBGroupBox; }
            //set { this._showWebCredentials = value; }
        }
        public bool ShowSAPGroupBox
        {
            get { return this._showSAPGroupBox; }
            //set { this._showWebCredentials = value; }
        }

        public bool ShowCredentialsSummary
        {
            get
            {
                if (ShowSAPCredentials || ShowDBCredentials || ShowSchemaPassword || ShowSIDADMCredentials || ShowWebCredentials)
                    return true;
                else
                    return false;

            }
            set { this.OnPropertyChanged("ShowCredentialsSummary"); }
        }

        public bool ShowSAPClientListCredentials
        {
            get { return this._showSAPClientListCredentials; }
            set 
            {
                this._showSAPClientListCredentials = value;
                this.OnPropertyChanged("ShowSAPClientListCredentials");
            }
        }

        public List<ClientSet> ClientsList
        {
            get { return this._clientsList; }
            set
            {
                this._clientsList = value;
                this.OnPropertyChanged("ClientsList");
            }
        }

        public class ClientSet : ObservableObject
        {
            private int _setNum;
            private bool _isEnabled;
            private bool _isSelected;
            private string _clientNum;
            private string _user;
            private string _userClient;
            private string _password;
            private string _description;

            [JsonConstructor]
            public ClientSet(int setNum, string client, string user, string password, string description)
            {
                this._setNum = setNum;
                if (client == "must")
                {
                    this._isEnabled = false;
                    this._isSelected = true;
                    this._clientNum = "000";
                }
                else if (client == "optional")
                {
                    this._isEnabled = true;
                    this._isSelected = true;
                    this._clientNum = "000";
                }
                else
                    this._clientNum = client;
                if (user == "must")
                {
                    this._isEnabled = false;
                    this._isSelected = true;
                    this._user = "";
                }
                else if(user == "optional")
                {
                    this._isEnabled = true;
                    this._isSelected = true;
                    this._user = "";
                }
                else
                    this._user = user;
                if (password == "must")
                {
                    this._isEnabled = false;
                    this._isSelected = true;
                    this._password = "";
                }
                else if (password == "optional")
                {
                    this._isEnabled = true;
                    this._isSelected = true;
                    this._password = "";
                }
                else
                    this._password = password;
                if (description == "must")
                {
                    this._isEnabled = false;
                    this._isSelected = true;
                    this._description = "";
                }
                else if (description == "optional")
                {
                    this._isEnabled = true;
                    this._isSelected = true;
                    this._description = "";
                }
                else
                    this._description = description;
            }

            public int SetNum
            {
                get { return _setNum; }
                set
                {
                    this._setNum = value;
                    this.OnPropertyChanged("SetNum");
                }
            }
            public bool IsEnabled
            {
                get { return _isEnabled; }
                set
                {
                    this._isEnabled = value;
                    this.OnPropertyChanged("IsEnabled");
                }
            }
            public bool IsSelected
            {
                get { return _isSelected; }
                set
                {
                    this._isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
            public string ClientNum
            {
                get { return _clientNum; }
                set
                { 
                    this._clientNum = value;
                    this.OnPropertyChanged("ClientNum");
                    UserClient = value + "-" + User;
                }
            }
            public string User
            {
                get { return this._user; }
                set
                { 
                    this._user = value;
                    this.OnPropertyChanged("User");
                    UserClient = ClientNum + "-" + value;
                }
            }
            public string UserClient
            {
                get { return this._userClient; }
                set
                { 
                    this._userClient = value;
                    this.OnPropertyChanged("UserClient");
                }
            }
            public string Password
            {
                get { return this._password; }
                set
                { 
                    this._password = value;
                    this.OnPropertyChanged("Password");
                }
            }
            public string Description
            {
                get { return this._description; }
                set
                { 
                    this._description = value;
                    this.OnPropertyChanged("Description");
                }
            }

        }
    }
}
