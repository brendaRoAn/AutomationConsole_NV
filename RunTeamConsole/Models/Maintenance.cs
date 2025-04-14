using System;
using System.Collections.Generic;
using System.Text;

namespace RunTeamConsole.Models
{
    public class Maintenance : ObservableObject
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }
}
