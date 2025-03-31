using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApprovalFunctionApp.DTOs
{
    public class ApprovalEventDto
    {
        public string Action { get; set; } // "Approve", "Reject", "Cancel"
        public object ActionData { get; set; } // Additional metadata (JSON or a complex object)
    }
}
