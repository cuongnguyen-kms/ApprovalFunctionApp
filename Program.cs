using ApprovalFunctionApp.Configurations;
using ApprovalFunctionApp.Helpers;
using ApprovalFunctionApp.Interfaces;
using ApprovalFunctionApp.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApprovalFunctionApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            FunctionsDebugger.Enable();

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((context, services) =>
                {
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();
                    services.AddSingleton<IApprovalService, ApprovalService>();
                    services.AddSingleton<IEmailHelper, EmailHelper>();
                    services.AddSingleton<EmailServiceFactory>();
                    services.Configure<EmailSettings>(context.Configuration.GetSection("EmailSettings"));
                })
                .Build();

            host.Run();
        }
    }
}
