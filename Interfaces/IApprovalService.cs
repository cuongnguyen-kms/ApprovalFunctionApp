using ApprovalFunctionApp.Models;
using Microsoft.DurableTask.Client;
using Microsoft.DurableTask;
using System.Threading.Tasks;
using ApprovalFunctionApp.DTOs;

namespace ApprovalFunctionApp.Interfaces
{
    public interface IApprovalService
    {
        Task<string> StartApprovalAsync(DurableTaskClient client, ApprovalRequest approvalData);
        Task RunApprovalOrchestrationAsync(TaskOrchestrationContext context);
        Task HandleApprovalActionAsync(DurableTaskClient client, string instanceId, ApprovalEventDto approvalEvent);
    }
}
