namespace DevTrack.Infrastructure.Entities
{
    public class Project : EntityBase, IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool AllowScreenshot { get; set; }
        public bool AllowWebcam { get; set; }
        public bool AllowKeyboardHit { get; set; }
        public bool AllowMouseClick { get; set; }
        public bool AllowActiveWindow { get; set; }
        public bool AllowManualTimeEntry { get; set; }
        public bool AllowRunningProgram { get; set; }
        public int TimeInterval { get; set; }
        public bool IsArchived { get; set; }        
        public List<ProjectUser> ProjectApplicationUsers { get; set; }
        public List<Invitation> Invitations { get; set; }
        public List<Activity> Activities { get; set; }
    }
}
