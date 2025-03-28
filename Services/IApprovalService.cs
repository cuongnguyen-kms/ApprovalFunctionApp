using ApprovalFunctionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApprovalFunctionApp.Services
{
    public interface IApprovalService
    {
        Task HandleApprovalResponse(string instanceId, string response);
    }
}
