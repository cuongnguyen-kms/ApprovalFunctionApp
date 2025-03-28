using ApprovalFunctionApp.Helpers;
using ApprovalFunctionApp.Models;
using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;

namespace ApprovalFunctionApp.Functions
{
    public class SendMailFunction
    {
        private readonly IEmailHelper _emailHelper;

        public SendMailFunction(IEmailHelper emailHelper)
        {
            _emailHelper = emailHelper;
        }

        [Function("SendMailFunction")]
        public async Task SendMail([ActivityTrigger] EmailData emailData)
        {
            // Email bypass logic (no actual sending)
            await _emailHelper.SendEmailAsync(emailData.To, emailData.Subject, emailData.Body);
        }
    }
}
