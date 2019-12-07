using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Email
{
    public class SendEmailRequest
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ToEmail { get; set; }
        public string Template { get; set; }
    }
}
