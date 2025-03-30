using System.Threading.Tasks;
using ApprovalFunctionApp.Interfaces;
using ApprovalFunctionApp.Services;

namespace ApprovalFunctionApp.Helpers
{
    public interface IEmailHelper
    {
        Task SendEmailAsync(string to, string subject, string message);
    }

    public class EmailHelper : IEmailHelper
    {
        private readonly IEmailService _emailService;

        public EmailHelper(EmailServiceFactory emailServiceFactory)
        {
            _emailService = emailServiceFactory.CreateEmailService();
        }
        public Task SendEmailAsync(string to, string subject, string message)
        {
            return _emailService.SendEmailAsync(to, subject, message);
        }
    }
}
