using DevTrack.Infrastructure.Codes;

namespace DevTrack.Infrastructure.Entities
{
    public class Invitation : EntityBase, IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Email { get; set; }
        public InvitationStatus Status{ get; set; }
        public Project Project { get; set; }
    }
}
