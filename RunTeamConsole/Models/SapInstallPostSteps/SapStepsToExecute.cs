using Newtonsoft.Json;
using System.Collections.Generic;

namespace RunTeamConsole.Models.SapInstallPostSteps
{
    public class SapStepsToExecute : ObservableObject
    {
        public class Rz10Active : ObservableObject
        {
            private bool _rz10;
            public bool Rz10
            {
                get { return _rz10; }
                set { _rz10 = value; }
            }
        }
        private bool _rz10;
        #region SapStepsToExecute bool variables
        public bool Rz10 { get; set; }
        public bool ExecSappfpar { get; set; }
        public bool ExecSe38 { get; set; }
        public bool ExecStmsccms { get; set; }
        public bool ExecStmstruck { get; set; }
        public bool ExecSt03n { get; set; }
        public bool ExecSt04n { get; set; }
        public bool ExecSt06 { get; set; }
        public bool ExecSick { get; set; }
        public bool ExecSm12 { get; set; }
        public bool ExecSm13 { get; set; }
        public bool ExecSm65 { get; set; }
        #endregion

        public SapStepsToExecute(bool execRz10, bool execSappfpar, bool execSe38, bool execStmsccms, bool execStmstruck, bool execSt03n, bool execSt04n, bool execSt06, bool execSick, bool execSm12, bool execSm13, bool execSm65)
        {
            //ExecRz10 = execRz10;
            ExecSappfpar = execSappfpar;
            ExecSe38 = execSe38;
            ExecStmsccms = execStmsccms;
            ExecStmstruck = execStmstruck;
            ExecSt03n = execSt03n;
            ExecSt04n = execSt04n;
            ExecSt06 = execSt06;
            ExecSick = execSick;
            ExecSm12 = execSm12;
            ExecSm13 = execSm13;
            ExecSm65 = execSm65;
        }
    }
}