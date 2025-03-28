using ApprovalFunctionApp.Helpers;
using ApprovalFunctionApp.Intefaces;
using ApprovalFunctionApp.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
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
                .ConfigureServices(services =>
                {
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();
                    services.AddSingleton<IApprovalService, ApprovalService>();
                    services.AddSingleton<IEmailHelper, EmailHelper>();
                    services.AddSingleton<EmailServiceFactory>();
                })
                .Build();

            host.Run();
        }
    }
}
