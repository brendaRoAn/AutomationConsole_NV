using System;

namespace RunTeamConsole.Models
{
    public class ProcessExecution
    {
        public int Id { get; set; }
        public string Idx { get; set; }
        public string ProcessName { get; set; }
        public string Title { get; set; }
        public int CurrentStep { get; set; }
        public string GroupName { get; set; }
        public string User { get; set; }
        public string PAS { get; set; }
        public string DBS { get; set; }
        public string SID { get; set; }
        public string Customer { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
