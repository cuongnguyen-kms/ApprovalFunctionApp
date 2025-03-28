using System.Threading.Tasks;

namespace ApprovalFunctionApp.Intefaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
