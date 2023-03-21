using DevTrack.Infrastructure.BusinessObjects;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DevTrack.Infrastructure.Services.Email
{
    public class HtmlEmailService : IEmailService
    {
        private readonly Smtp _emailSettings;

        public HtmlEmailService(IOptions<Smtp> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendSingleEmail(string receiverName, string receiverEmail, string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_emailSettings.SenderName,
                _emailSettings.SenderEmail));

            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                client.Timeout = 30000;
                await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        public async Task<string> GetInvitationEmailBody(string encryptedInvitationId,
            string encryptedProjectId, string projectName)
        {
            var link = _emailSettings.InvitationURL + "?invitationId=" + encryptedInvitationId + "&projectId=" + encryptedProjectId;
            return $"Please join {projectName} by <a href='{link}'>clicking here</a>.";
        }
    }
}
