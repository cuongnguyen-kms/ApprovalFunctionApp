using ApprovalFunctionApp.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace ApprovalFunctionApp.Functions
{
    public class ApproveFunction
    {
        private readonly IApprovalService _approvalService;

        public ApproveFunction(IApprovalService approvalService)
        {
            _approvalService = approvalService;
        }

        [Function("ApproveFunction")]
        public async Task<HttpResponseData> Approve(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "approve/{instanceId}")] HttpRequestData req,
            string instanceId)
        {
            await _approvalService.HandleApprovalResponse(instanceId, "Approved");

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"Approval for {instanceId} completed successfully.");
            return response;
        }
    }
}
