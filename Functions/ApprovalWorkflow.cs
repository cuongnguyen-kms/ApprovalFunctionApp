using ApprovalFunctionApp.Interfaces;
using ApprovalFunctionApp.Models;
using Azure;
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
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "approval/start")] HttpRequestData req,
            [DurableClient] DurableTaskClient client)
        {
            var reader = new StreamReader(req.Body);
            var body = await reader.ReadToEndAsync();
            var approvalData = JsonSerializer.Deserialize<ApprovalRequest>(body);

            if (approvalData == null)
                return req.CreateResponse(HttpStatusCode.BadRequest);
            string instanceId = await _approvalService.StartApprovalAsync(client, approvalData);

            _logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new ApprovalResponse("Approval started", instanceId));
            return response;
        }

        [Function("ApprovalOrchestration")]
        public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
        {
            await _approvalService.RunApprovalOrchestrationAsync(context);
        }

        [Function("Approve")]
        public async Task<HttpResponseData> Approve(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "approval/approve/{instanceId}")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            string instanceId)
        {
            try
            {
                if (instanceId == null)
                    return req.CreateResponse(HttpStatusCode.BadRequest);

                await _approvalService.ApproveAsync(client, instanceId);

                _logger.LogInformation($"Approval granted for instance {instanceId}");
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(new ApprovalResponse("Instance Approved", instanceId));
                return response;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error approving instance.");
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteAsJsonAsync(new { error = ex.Message });
                return response;
            }
        }

        [Function("Reject")]
        public async Task<HttpResponseData> Reject(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "approval/reject/{instanceId}")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            string instanceId)
        {
            try
            {
                if (instanceId == null)
                    return req.CreateResponse(HttpStatusCode.BadRequest);

                await _approvalService.RejectAsync(client, instanceId);

                _logger.LogInformation($"Approval rejected for instance {instanceId}");
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(new ApprovalResponse("Instance rejected", instanceId));
                return response;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error rejecting instance.");
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteAsJsonAsync(new { error = ex.Message });
                return response;
            }
        }
    }
}
