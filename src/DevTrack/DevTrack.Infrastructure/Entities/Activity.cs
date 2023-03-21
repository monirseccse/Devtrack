namespace DevTrack.Infrastructure.Entities
{
    public class Activity : IEntity<Guid>
    {
        public Guid Id {get; set; }
        public Guid ApplicationUserId { get; set; }
        public Guid ProjectId { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsOnline { get; set; }
        public MouseActivity MouseActivity { get; set; }
        public ScreenCapture ScreenCapture { get; set; }
        public List<ActiveWindow> ActiveWindows { get; set; }
        public WebcamCapture WebcamCapture { get; set; }
        public List<RunningProgram> RunningPrograms { get; set; }
        public KeyboardActivity KeyboardActivity { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Project Project { get; set; }
    }
}
