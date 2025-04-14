using Newtonsoft.Json;
using System.Collections.Generic;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class Scc4SettingsConfiguration : ObservableObject
    {
        private int? _client;
        private string _clientName, _clientCity, _logicalName, _currency, _clientRole, _changesAndTransport, _crossClient, _copyComparisonTool, _cattAndEcattRest;
        private bool _isSelected;

        public int? Client
        {
            get { return _client; }
            set { _client = value; }
        }
        public string ClientName
        {
            get { return _clientName; }
            set { _clientName = value; }
        }
        public string ClientCity
        {
            get { return _clientCity; }
            set { _clientCity = value; }
        }
        public string LogicalName
        {
            get { return _logicalName; }
            set { _logicalName = value; }
        }
        public string Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }
        public string ClientRole
        {
            get { return _clientRole; }
            set { _clientRole = value; }
        }
        public string ChangesAndTransport
        {
            get { return _changesAndTransport; }
            set { _changesAndTransport = value; }
        }
        public string CrossClient
        {
            get { return _crossClient; }
            set { _crossClient = value; }
        }
        public string CopyComparisonTool
        {
            get { return _copyComparisonTool; }
            set { _copyComparisonTool = value; }
        }
        public string CattAndEcattRest
        {
            get { return _cattAndEcattRest; }
            set { _cattAndEcattRest = value; }
        }
        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                this._isSelected = value;
                this.OnPropertyChanged("IsSelected");
            }
        }
    }
}