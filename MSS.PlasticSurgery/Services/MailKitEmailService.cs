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

        public void SendAsync(EmailMessage msg)
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
                    client.Connect(_eConfig.SmtpServer, 465, true);

                    client.Authenticate(_eConfig.SmtpUsername, _eConfig.SmtpPassword);

                    client.Send(message);
                    client.Disconnect(true);
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
