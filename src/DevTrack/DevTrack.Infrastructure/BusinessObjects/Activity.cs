namespace DevTrack.Infrastructure.BusinessObjects
{
    public class Activity
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public Guid ProjectId { get; set; }
        public string Description { get; set; }
        public string DateRange { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsOnline { get; set; }
        public MouseActivity MouseActivity { get; set; }
        public ScreenCapture ScreenCapture { get; set; }
        public List<ActiveWindow> ActiveWindows { get; set; }
        public WebcamCapture WebcamCapture { get; set; }
        public List<RunningProgram> RunningPrograms { get; set; }
        public KeyboardActivity KeyboardActivity { get; set; }
    }
}
