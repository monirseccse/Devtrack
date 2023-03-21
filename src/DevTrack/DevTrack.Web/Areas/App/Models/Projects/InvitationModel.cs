using Autofac;
using DevTrack.Infrastructure.Codes;
using DevTrack.Infrastructure.Services;
using DevTrack.Infrastructure.Services.Encryption;

namespace DevTrack.Web.Areas.App.Models.Projects
{
    public class InvitationModel
    {
        public string InvitationId { get; set; }
        public string Email { get; set; }
        public InvitationStatus Status { get; set; }

        private IInvitationService _invitationService;
        private ILifetimeScope _scope;
        private ISymmetricEncryptionService _symmetricEncryptionService;

        public InvitationModel()
        {

        }

        public InvitationModel(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _invitationService = _scope.Resolve<IInvitationService>();
            _symmetricEncryptionService = _scope.Resolve<ISymmetricEncryptionService>();
        }

        internal async Task UpdateStatusToAccepted()
        {
            await _invitationService.UpdateInvitationStatusAsync(Guid.Parse(InvitationId), InvitationStatus.Accepted);
        }

        internal async Task LoadInvitation()
        {
            var decryptedinvitationId = await _symmetricEncryptionService.Decrypt(InvitationId);
            var invitation =  await _invitationService.GetInvitationAsync(Guid.Parse(decryptedinvitationId));
            InvitationId = invitation.Id.ToString();
            Email = invitation.Email;
            Status = invitation.Status;
        }
    }
}
