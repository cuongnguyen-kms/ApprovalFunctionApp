using ApprovalFunctionApp.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using ApprovalFunctionApp.Configurations;
using Microsoft.Extensions.Options;

namespace ApprovalFunctionApp.Services
{
    public class GmailEmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public GmailEmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpClient = new SmtpClient(_emailSettings.SmtpServer)
            {
                Port = _emailSettings.SmtpPort,
                Credentials = new NetworkCredential(
                    _emailSettings.Username,
                    _emailSettings.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
