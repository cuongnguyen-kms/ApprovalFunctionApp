using ApprovalFunctionApp.Helpers;
using ApprovalFunctionApp.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace ApprovalFunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IApprovalService, ApprovalService>();
            builder.Services.AddSingleton<EmailHelper>();
        }
    }
}
