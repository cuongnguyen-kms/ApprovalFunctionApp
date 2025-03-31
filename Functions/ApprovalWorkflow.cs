using ApprovalFunctionApp.Constants;
using ApprovalFunctionApp.DTOs;
using ApprovalFunctionApp.Interfaces;
using ApprovalFunctionApp.Mapping;
using ApprovalFunctionApp.Models;
using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApprovalFunctionApp.Functions
{
    public class ApprovalWorkflow
    {
        private readonly ILogger<ApprovalWorkflow> _logger;
        private readonly IApprovalService _approvalService;
        private readonly IMapper _mapper;

        public ApprovalWorkflow(ILogger<ApprovalWorkflow> logger, IApprovalService approvalService)
        {
            _logger = logger;
            _approvalService = approvalService;

            // Manually configure AutoMapper instance
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ApprovalMappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Function(FunctionRoutes.StartApproval)]
        public async Task<HttpResponseData> StartApproval(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = FunctionRoutes.StartApprovalUrl)] HttpRequestData req,
            [DurableClient] DurableTaskClient client)
        {
            try
            {
                var dto = await JsonSerializer.DeserializeAsync<ApprovalRequestDto>(req.Body);
                if (dto == null || string.IsNullOrEmpty(dto.RequestId) || string.IsNullOrEmpty(dto.RequesterEmail))
                {
                    return req.CreateResponse(HttpStatusCode.BadRequest);
                }

                // Use AutoMapper instead of manual mapping
                var approvalRequest = _mapper.Map<ApprovalRequest>(dto);
                string instanceId = await _approvalService.StartApprovalAsync(client, approvalRequest);

                _logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(new ApprovalResponse(AppConstants.ApprovalStarted, instanceId));
                return response;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error in StartApproval");
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [Function(FunctionRoutes.ApprovalOrchestration)]
        public async Task RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context)
        {
            await _approvalService.RunApprovalOrchestrationAsync(context);
        }

        [Function(FunctionRoutes.Approve)]
        public async Task<HttpResponseData> Approve(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = FunctionRoutes.ApproveUrl)] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            string instanceId)
        {
            try
            {
                if (string.IsNullOrEmpty(instanceId))
                    return req.CreateResponse(HttpStatusCode.BadRequest);

                var approvalEvent = new ApprovalEventDto
                {
                    Action = AppConstants.ApprovalApproved,
                    ActionData = new { Reason = "Approved by user", Timestamp = DateTime.UtcNow }
                };

                await _approvalService.HandleApprovalActionAsync(client, instanceId, approvalEvent);

                _logger.LogInformation($"Approval granted for instance {instanceId}");
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(new ApprovalResponse(AppConstants.InstanceApprovedMessage, instanceId));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving instance.");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteAsJsonAsync(new { error = ex.Message });
                return response;
            }
        }

        [Function(FunctionRoutes.Reject)]
        public async Task<HttpResponseData> Reject(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = FunctionRoutes.RejectUrl)] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            string instanceId)
        {
            try
            {
                if (string.IsNullOrEmpty(instanceId))
                    return req.CreateResponse(HttpStatusCode.BadRequest);

                var approvalEvent = new ApprovalEventDto
                {
                    Action = AppConstants.ApprovalRejected,
                    ActionData = new { Reason = "Rejected by user", Timestamp = DateTime.UtcNow }
                };

                await _approvalService.HandleApprovalActionAsync(client, instanceId, approvalEvent);

                _logger.LogInformation($"Approval rejected for instance {instanceId}");
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(new ApprovalResponse(AppConstants.InstanceRejectedMessage, instanceId));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting instance.");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteAsJsonAsync(new { error = ex.Message });
                return response;
            }
        }
    }
}
