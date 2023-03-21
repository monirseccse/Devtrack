namespace DevTrack.Api.Model
{
    public class ProjectData
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public bool AllowWebcam { get; set; }
        public bool AllowScreenshot { get; set; }
        public bool AllowKeyboardHit { get; set; }
        public bool AllowMouseClick { get; set; }
        public bool AllowActiveWindow { get; set; }
        public bool AllowRunningProgram { get; set; }
        public bool AllowManualTimeEntry { get; set; }
        public int TimeInterval { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Role { get; set; }
    }
}
