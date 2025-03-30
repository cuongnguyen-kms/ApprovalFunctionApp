using System.Threading.Tasks;

namespace ApprovalFunctionApp.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
