using ApprovalFunctionApp.Models;
using Microsoft.DurableTask.Client;
using Microsoft.DurableTask;
using System.Threading.Tasks;

namespace ApprovalFunctionApp.Interfaces
{
    public interface IApprovalService
    {
        Task<string> StartApprovalAsync(DurableTaskClient client, ApprovalRequest approvalData);
        Task RunApprovalOrchestrationAsync(TaskOrchestrationContext context);
        Task ApproveAsync(DurableTaskClient client, string instanceId);
        Task RejectAsync(DurableTaskClient client, string instanceId);
    }
}
