using System;
using System.ComponentModel.DataAnnotations;

namespace RunTeamConsole.Models
{
    public class StatusExecution
    {
        public string Idx { get; set; }
        public int StepIndex { get; set; }
        public string State { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }
    }
}
