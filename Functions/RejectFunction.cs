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
    public class RejectFunction
    {
        private readonly IApprovalService _approvalService;

        public RejectFunction(IApprovalService approvalService)
        {
            _approvalService = approvalService;
        }

        [Function("RejectFunction")]
        public async Task<HttpResponseData> Reject(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "reject/{instanceId}")] HttpRequestData req,
            string instanceId)
        {
            await _approvalService.HandleApprovalResponse(instanceId, "Rejected");

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"Rejection for {instanceId} completed successfully.");
            return response;
        }
    }
}
