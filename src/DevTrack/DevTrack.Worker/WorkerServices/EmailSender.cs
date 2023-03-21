using Autofac;
using DevTrack.Infrastructure.Services.Email;
using DevTrack.Infrastructure.Services.Encryption;
using DevTrack.Worker.Models;

namespace DevTrack.Worker.WorkerServices
{
    public class EmailSender
    {
        protected ILifetimeScope? _scope;
        protected ILogger? _logger;
        protected IEmailService? _emailService;
        protected ISymmetricEncryptionService? _symmetricEncryptionService;

        public void ResolveDependency(
            ILifetimeScope scope,
            ILogger logger)
        {
            _scope = scope;
            _logger = logger;
            _emailService = _scope.Resolve<IEmailService>();
            _symmetricEncryptionService = _scope.Resolve<ISymmetricEncryptionService>();
        }

        internal async Task sendEmails()
        {
            var invitationModel = _scope.Resolve<InvitationModel>();
            invitationModel.ResolveDependency(_scope);
            var invitations = await invitationModel.GetNewInvitations();

            if(invitations.Count > 0)
            {   
                foreach (var invitation in invitations)
                {
                    var projectModel = _scope.Resolve<ProjectModel>();
                    projectModel.ResolveDependency(_scope);
                    var project = await projectModel.GetProjectByIDAsync(invitation.ProjectId);

                    var memberName = invitation.Email.Split('@')[0];
                    var memberEmail = invitation.Email;
                    var emailSubject = $"You've been invited to join {project.Title} on DevTrack";
                    var encryptedProjectId = await _symmetricEncryptionService.Encrypt(project.Id.ToString());
                    var encryptedInvitationId = await _symmetricEncryptionService.Encrypt(invitation.Id.ToString());
                    var emailBody = await _emailService.GetInvitationEmailBody(encryptedInvitationId, encryptedProjectId, project.Title);
                    await _emailService.SendSingleEmail(memberName, memberEmail, emailSubject, emailBody);
                    await invitationModel.UpdateStatusToPending(invitation.Id);
                }
            }
        }
    }
}
