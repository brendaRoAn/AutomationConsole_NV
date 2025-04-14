using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MassiveTestHelper.Models
{
    public class Appl
    {
        private string _hostname;
        private string _type;
        private string _os;
        public string Hostname
        {
            get { return _hostname; }
        }
        public string Type
        {
            get { return _type; }
        }

        public string OS
        {
            get { return _os; }
        }
        public Appl(string hostname, string type, string os)
        {
            this._hostname = hostname;
            this._type = type;
            this._os = os;
        }
    }
}
