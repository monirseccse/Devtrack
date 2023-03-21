namespace DevTrack.Infrastructure.BusinessObjects
{
    public class MouseActivity
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public int TotalHits { get; set; }
    }
}
