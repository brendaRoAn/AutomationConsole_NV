using System;
using System.Collections.Generic;
using System.Text;

namespace RunTeamConsole.Models
{
    public class Filter
    {
        public string Type { get; set; }
        public string Variable { get; set; }
        public bool IsApplied { get; set; }
        public bool IsSelected { get; set; }
    }
}
