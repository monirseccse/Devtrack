using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Codes;

namespace DevTrack.Infrastructure.Services
{
    public interface IInvitationService
    {
        Task CreateInvitationsAsync(Invitation invitation);
        Task<IList<Invitation>> GetInvitationsAsync(int invitationStatus);
        Task UpdateInvitationStatusAsync(Guid invitationId, InvitationStatus status);
        Task<Invitation> GetInvitationAsync(Guid invitationId);
    }
}
