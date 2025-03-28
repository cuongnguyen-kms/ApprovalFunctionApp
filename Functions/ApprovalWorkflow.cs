using ApprovalFunctionApp.Intefaces;
using ApprovalFunctionApp.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApprovalFunctionApp.Functions
{
    public class ApprovalWorkflow
    {
        private readonly ILogger<ApprovalWorkflow> _logger;
        private readonly IApprovalService _approvalService;

        public ApprovalWorkflow(ILogger<ApprovalWorkflow> logger, IApprovalService approvalService)
        {
            _logger = logger;
            _approvalService = approvalService;
        }

        [Function("StartApproval")]
        public async Task<HttpResponseData> StartApproval(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "approval-start")] HttpRequestData req,
            [DurableClient] DurableTaskClient client)
        {
            var approvalData = await JsonSerializer.DeserializeAsync<ApprovalRequest>(req.Body);
            string instanceId = await _approvalService.StartApprovalAsync(client, approvalData);

            _logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new { InstanceId = instanceId });
            return response;
        }

        [Function("ApprovalOrchestration")]
        public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
        {
            await _approvalService.RunApprovalOrchestrationAsync(context);
        }

        [Function("Approve")]
        public async Task<HttpResponseData> Approve(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "approve/{instanceId}")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            string instanceId)
        {
            await _approvalService.ApproveAsync(client, instanceId);

            _logger.LogInformation($"Approval granted for instance {instanceId}");
            return req.CreateResponse(HttpStatusCode.OK);
        }

        [Function("Reject")]
        public async Task<HttpResponseData> Reject(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "reject/{instanceId}")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            string instanceId)
        {
            await _approvalService.RejectAsync(client, instanceId);

            _logger.LogInformation($"Approval rejected for instance {instanceId}");
            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
