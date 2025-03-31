using System.Threading.Tasks;
using Microsoft.DurableTask.Client;
using ApprovalFunctionApp.Interfaces;
using ApprovalFunctionApp.Models;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using ApprovalFunctionApp.Constants;
using ApprovalFunctionApp.DTOs;

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
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(FunctionRoutes.ApprovalOrchestration, approvalData);
            _logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");
            return instanceId;
        }

        public async Task RunApprovalOrchestrationAsync(TaskOrchestrationContext context)
        {
            var approvalData = context.GetInput<ApprovalRequest>();
            await context.CallActivityAsync(FunctionRoutes.SendMailFunction, new EmailData(approvalData.RequesterEmail, AppConstants.ApprovalStarted, AppConstants.ApprovalStartedMessage));

            var approvalEvent = await context.WaitForExternalEvent<ApprovalEventDto>(AppConstants.ApprovalEvent);

            string subject, body;

            switch (approvalEvent.Action)
            {
                case AppConstants.ApprovalApproved:
                    subject = AppConstants.ApprovalApproved;
                    body = AppConstants.InstanceApprovedMessage;
                    break;

                case AppConstants.ApprovalRejected:
                    subject = AppConstants.ApprovalRejected;
                    body = AppConstants.InstanceRejectedMessage;
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported approval action: {approvalEvent.Action}");
            }
            await context.CallActivityAsync(FunctionRoutes.SendMailFunction, new EmailData(approvalData.RequesterEmail, subject, body));
        }
        public async Task HandleApprovalActionAsync(DurableTaskClient client, string instanceId, ApprovalEventDto approvalEvent)
        {
            var status = await client.GetInstancesAsync(instanceId);
            if (status == null || status.RuntimeStatus == OrchestrationRuntimeStatus.Completed)
                throw new InvalidOperationException($"Instance {instanceId} is already completed.");

            await client.RaiseEventAsync(instanceId, AppConstants.ApprovalEvent, approvalEvent);
            _logger.LogInformation($"Approval action '{approvalEvent.Action}' sent for instance {instanceId}");
        }
    }
}
