using DevTrack.Infrastructure.Codes;

namespace DevTrack.Infrastructure.BusinessObjects
{
    public class Invitation
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Email { get; set; }
        public InvitationStatus Status { get; set; }
    }
}
