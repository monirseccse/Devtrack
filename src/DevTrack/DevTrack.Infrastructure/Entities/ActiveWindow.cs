namespace DevTrack.Infrastructure.Entities
{
    public class ActiveWindow : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public string ProcessName { get; set; }
        public string MainWindowTitle { get; set; }
        public Activity Activity { get; set; }
    }
}
