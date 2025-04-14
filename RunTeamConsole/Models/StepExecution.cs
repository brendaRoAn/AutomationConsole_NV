namespace RunTeamConsole.Models
{
    public class StepExecution
    {
        public int Id { get; set; }
        public int StepIndex { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Log { get; set; }
        public string Message { get; set; }
        public string Idx { get; set; }
    }
}
