namespace DevTrack.Infrastructure.BusinessObjects
{
    public class WebcamCapture
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public string Image { get; set; }
        public DateTime Time { get; set; }
    }
}
