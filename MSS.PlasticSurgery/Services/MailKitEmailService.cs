using System;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MSS.PlasticSurgery.Services.Models;

namespace MSS.PlasticSurgery.Services
{
    public class MailKitEmailService : IEmailService
    {
        private readonly EmailServerConfiguration _eConfig;

        public MailKitEmailService(EmailServerConfiguration config)
        {
            _eConfig = config;
        }

        public async Task SendAsync(EmailMessage msg)
        {
            var message = new MimeMessage();
            message.To.AddRange(msg.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.From.AddRange(msg.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.Subject = msg.Subject;
            message.Body = new TextPart("plain")
            {
                Text = msg.Content
            };

            try
            { 
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync(_eConfig.SmtpServer, 465, true);

                    await client.AuthenticateAsync(_eConfig.SmtpUsername, _eConfig.SmtpPassword);

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
