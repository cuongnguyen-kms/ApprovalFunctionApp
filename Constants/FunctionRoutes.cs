using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApprovalFunctionApp.Constants
{
    public static class FunctionRoutes
    {
        public const string StartApproval = "StartApproval";
        public const string ApprovalOrchestration = "ApprovalOrchestration";
        public const string Approve = "Approve";
        public const string Reject = "Reject";
        public const string SendMailFunction = "SendMailFunction";
        public const string StartApprovalUrl = "approval/start";
        public const string ApproveUrl = "approval/approve/{instanceId}";
        public const string RejectUrl = "approval/reject/{instanceId}";

    }
}
