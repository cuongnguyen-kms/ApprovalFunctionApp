using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApprovalFunctionApp.Configurations
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string Username { get; set; }
        public string SenderEmail { get; set; }
        public string Password { get; set; }
        public string Provider { get; set; }
    }
}
