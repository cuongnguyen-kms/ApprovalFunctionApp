using System;

namespace ApprovalFunctionApp.Models
{
    public class ApprovalRequest
    {
        public string RequestId { get; set; }
        public string RequesterEmail { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
