using DevTrack.Infrastructure.Codes;

namespace DevTrack.Infrastructure.BusinessObjects
{
    public class ProjectUserList
    {
        public Guid ApplicationUserId { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectRole Role { get; set; }
    }
}
