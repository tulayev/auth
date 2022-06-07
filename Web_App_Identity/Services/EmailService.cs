using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Web_App_Identity.Settings;

namespace Web_App_Identity.Services
{
    public class EmailService : IEmailService
    {
        private readonly SMTPSettings _smptpSettings;

        public EmailService(IOptions<SMTPSettings> smptpSettings)
        {
            _smptpSettings = smptpSettings.Value;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            /*var message = new MailMessage(_smptpSettings.Username, to, subject, body);

            using (var emailClient = new SmtpClient(_smptpSettings.Host, _smptpSettings.Port))
            {
                emailClient.Credentials = new NetworkCredential(_smptpSettings.Username, _smptpSettings.Password);
                emailClient.EnableSsl = true;
                await emailClient.SendMailAsync(message);
            }*/

            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Web App Identity", _smptpSettings.Username));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smptpSettings.Host, _smptpSettings.Port, true);
                await client.AuthenticateAsync(_smptpSettings.Username, _smptpSettings.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
