using Autofac;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Codes;
using DevTrack.Infrastructure.Services;

namespace DevTrack.Worker.Models
{
    public class InvitationModel
    {
        protected ILifetimeScope? _scope;
        private IInvitationService? _invitationService;

        public void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _invitationService = _scope.Resolve<IInvitationService>();
        }

        internal async Task<IList<Invitation>> GetNewInvitations()
        {
            return await _invitationService.GetInvitationsAsync(0);
        }

        internal async Task UpdateStatusToPending(Guid invitationId)
        {
            await _invitationService.UpdateInvitationStatusAsync(invitationId, InvitationStatus.Pending);
        }
    }
}
