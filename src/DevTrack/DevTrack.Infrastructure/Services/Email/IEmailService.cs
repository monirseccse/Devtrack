namespace DevTrack.Infrastructure.Services.Email
{
    public interface IEmailService
    {
        Task SendSingleEmail(string receiverName, string receiverEmail,
            string subject, string body);
        Task<string> GetInvitationEmailBody(string encryptedInvitationId, string encryptedProjectId, string projectName);
    }
}
