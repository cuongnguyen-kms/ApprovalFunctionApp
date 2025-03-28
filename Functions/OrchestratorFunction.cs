using ApprovalFunctionApp.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ApprovalFunctionApp.Functions
{
    public static class OrchestratorFunction
    {
        [Function("OrchestratorFunction")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            var approvalRequest = context.GetInput<ApprovalRequest>();

            await context.CallActivityAsync("SendMailFunction", new EmailData(approvalRequest.RequesterEmail, "Approval Started", "Your approval process has started."));

            var result = await context.WaitForExternalEvent<string>("ApprovalResponse");

            var subject = result == "Approved" ? "Approval Approved" : "Approval Rejected";
            var message = result == "Approved" ? "Your approval has been approved." : "Your approval has been rejected.";

            await context.CallActivityAsync("SendMailFunction", new EmailData(approvalRequest.RequesterEmail, subject, message));
        }
    }
}
