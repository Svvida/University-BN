using Domain.Interfaces;
using Infrastructure.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.Services
{
    public class MailKitEmailService : IEmailService
    {
        private readonly SmtpSettings _smptSettings;

        public MailKitEmailService(IOptions<SmtpSettings> smptSettings)
        {
            _smptSettings = smptSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_smptSettings.SenderName, _smptSettings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            email.Body = new TextPart(TextFormat.Html) { Text = message };

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(_smptSettings.Server, _smptSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_smptSettings.Username, _smptSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to send email to {toEmail}.", ex);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
