namespace ApprovalFunctionApp.Models
{
    public class ApprovalResponse
    {
        public string Message { get; set; } = string.Empty;
        public string InstanceId { get; set; } = string.Empty;

        public ApprovalResponse(string message, string instanceId)
        {
            Message = message;
            InstanceId = instanceId;
        }
    }
}
