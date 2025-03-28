using ApprovalFunctionApp.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApprovalFunctionApp.Functions
{
    public class StartApprovalFunction
    {
        [Function("StartApprovalFunction")]
        public async Task<HttpResponseData> StartApproval(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "start")] HttpRequestData req,
            [DurableClient] DurableTaskClient client)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var approvalRequest = JsonSerializer.Deserialize<ApprovalRequest>(requestBody);

            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync("OrchestratorFunction", approvalRequest);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new ApprovalResponse("Approval process started successfully.", instanceId));
            return response;
        }
    }
}
