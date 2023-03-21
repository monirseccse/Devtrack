namespace DevTrack.Infrastructure.BusinessObjects
{
    public class KeyboardActivity
    { 
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public string KeyCounts { get; set; }
        public int TotalHits { get; set; }
    }
}
