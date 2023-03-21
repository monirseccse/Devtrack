using DevTrack.Infrastructure.Codes;

namespace DevTrack.Infrastructure.Entities
{
    public class ProjectUser
    {
        public Guid ApplicationUserId { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectRole Role { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Project Project { get; set; }
    }
}
