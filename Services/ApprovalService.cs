using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using ApprovalFunctionApp.Models;
using ApprovalFunctionApp.Helpers;
using Microsoft.DurableTask.Client;

namespace ApprovalFunctionApp.Services
{
    public class ApprovalService : IApprovalService
    {
        private readonly DurableTaskClient _client;

        public ApprovalService(DurableTaskClient client)
        {
            _client = client;
        }

        public async Task HandleApprovalResponse(string instanceId, string response)
        {
            await _client.RaiseEventAsync(instanceId, "ApprovalResponse", response);
        }
    }
}
