using System.Threading.Tasks;
using Microsoft.DurableTask.Client;
using ApprovalFunctionApp.Intefaces;
using ApprovalFunctionApp.Models;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace ApprovalFunctionApp.Services
{
    public class ApprovalService : IApprovalService
    {
        private readonly ILogger<ApprovalService> _logger;

        public ApprovalService(ILogger<ApprovalService> logger)
        {
            _logger = logger;
        }

        public async Task<string> StartApprovalAsync(DurableTaskClient client, ApprovalRequest approvalData)
        {
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync("ApprovalOrchestration", approvalData);
            _logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");
            return instanceId;
        }

        public async Task RunApprovalOrchestrationAsync(TaskOrchestrationContext context)
        {
            var approvalData = context.GetInput<ApprovalRequest>();
            await context.CallActivityAsync("SendMailFunction", new EmailData(approvalData.RequesterEmail, "Approval Started", "Your approval has started."));

            string result = await context.WaitForExternalEvent<string>("ApprovalEvent");
            string body = result == "Approved" ? "Your approval has been approved." : "Your approval has been rejected.";
            string subject = result == "Approved" ? "Approval Approved" : "Approval Rejected";
            await context.CallActivityAsync("SendMailFunction", new EmailData(approvalData.RequesterEmail, subject, body));
        }

        public async Task ApproveAsync(DurableTaskClient client, string instanceId)
        {
            await client.RaiseEventAsync(instanceId, "ApprovalEvent", "Approved");
            _logger.LogInformation($"Approval granted for instance {instanceId}");
        }

        public async Task RejectAsync(DurableTaskClient client, string instanceId)
        {
            await client.RaiseEventAsync(instanceId, "ApprovalEvent", "Rejected");
            _logger.LogInformation($"Approval rejected for instance {instanceId}");
        }
    }
}
