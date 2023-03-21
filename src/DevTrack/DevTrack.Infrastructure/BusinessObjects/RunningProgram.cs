namespace DevTrack.Infrastructure.BusinessObjects
{
    public class RunningProgram
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public string MainWindowTitle { get; set; }
        public string ProcessName { get; set; }
        public DateTime Time { get; set; }
    }
}
