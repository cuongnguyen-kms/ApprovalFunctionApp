using ApprovalFunctionApp.Helpers;
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
                .ConfigureServices(services =>
                {
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();
                    services.AddScoped<IApprovalService, ApprovalService>();
                    services.AddSingleton<IEmailHelper, EmailHelper>();
                })
                .Build();

            host.Run();
        }
    }
}
