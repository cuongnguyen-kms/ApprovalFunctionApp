// /Helpers/EmailHelper.cs
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ApprovalFunctionApp.Helpers
{
    public interface IEmailHelper
    {
        Task SendEmailAsync(string to, string subject, string message);
    }

    public class EmailHelper : IEmailHelper
    {
        public async Task SendEmailAsync(string to, string subject, string message)
        {
            // Mailgun logic here
            await Task.CompletedTask;
        }
    }
}
