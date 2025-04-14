using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RunTeamConsole.Models
{
    public class Authorization
    {
        public string ItUser { get; set; }
        public string Area { get; set; }
        public string ProcessName { get; set; }
        public string Status { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
