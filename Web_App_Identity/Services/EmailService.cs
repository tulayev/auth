using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Web_App_Identity.Settings;

namespace Web_App_Identity.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<SMTPSettings> _smptpSettings;

        public EmailService(IOptions<SMTPSettings> smptpSettings)
        {
            _smptpSettings = smptpSettings;
        }

        public async Task SendAsync(string from, string to, string subject, string body)
        {
            var message = new MailMessage(_smptpSettings.Value.Username, to, subject, body);

            using (var emailClient = new SmtpClient(_smptpSettings.Value.Host, _smptpSettings.Value.Port))
            {
                emailClient.Credentials = new NetworkCredential(_smptpSettings.Value.Username, _smptpSettings.Value.Password);
                emailClient.EnableSsl = true;
                await emailClient.SendMailAsync(message);
            }
        }
    }
}
