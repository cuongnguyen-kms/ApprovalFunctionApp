using ApprovalFunctionApp.Configurations;
using ApprovalFunctionApp.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace ApprovalFunctionApp.Services
{
    public class EmailServiceFactory
    {
        private readonly EmailSettings _emailSettings;

        public EmailServiceFactory(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public IEmailService CreateEmailService()
        {

            switch (_emailSettings.Provider)
            {
                case "Gmail":
                    return new GmailEmailService(_emailSettings);
                default:
                    throw new NotImplementedException("Email provider not implemented");
            }
        }
    }
}
