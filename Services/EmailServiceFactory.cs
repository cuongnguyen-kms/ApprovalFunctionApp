using ApprovalFunctionApp.Intefaces;
using Microsoft.Extensions.Configuration;
using System;

namespace ApprovalFunctionApp.Services
{
    public class EmailServiceFactory
    {
        private readonly IConfiguration _configuration;

        public EmailServiceFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEmailService CreateEmailService()
        {
            var provider = _configuration["EmailSettings:Provider"];

            switch (provider)
            {
                case "Gmail":
                    return new GmailEmailService(_configuration);
                default:
                    throw new NotImplementedException("Email provider not implemented");
            }
        }
    }
}
