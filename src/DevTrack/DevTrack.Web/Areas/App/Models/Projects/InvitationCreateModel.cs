using Autofac;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Services;
using DevTrack.Infrastructure.Validations;

namespace DevTrack.Web.Areas.App.Models.Projects
{
    public class InvitationCreateModel
    {
        [ListItemIsEmail]
        public List<string> Emails { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }

        private IInvitationService _invitationService;
        private ILifetimeScope _scope;

        public InvitationCreateModel()
        {

        }

        public InvitationCreateModel(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _invitationService = _scope.Resolve<IInvitationService>();
        }

        public async Task CreateInvitationsAsync(Guid ProjectId)
        {
            foreach(var email in Emails)
            {
                if(email != null)
                {
                    var invitation = new Invitation();
                    invitation.ProjectId = ProjectId;
                    invitation.Email = email;

                    await _invitationService.CreateInvitationsAsync(invitation);
                }
            }
        }
    }
}