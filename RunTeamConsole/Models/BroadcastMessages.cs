using System;

namespace RunTeamConsole.Models
{
    public class BroadcastMessages
    {
        public int Id { get; set; }
        public string Destination { get; set; }
        public string Message { get; set; }
        public string ACOptionLocks { get; set; }
        public string CustomerRestriction { get; set; }
        public string SystemTypeRestrictions { get; set; }
        public string RestartACAction { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string TimeStamp { get; set; }
        public string RequestedBy { get; set; }
        public bool Shown { get; set; }
    }
}
