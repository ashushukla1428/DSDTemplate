using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class MailBoxModel
    {
        public Int64  MessageNumber { get; set; }
        public string MessageId { get; set; }
        public string FromAddress { get; set; }
        public string BCC { get; set; }
        public string CC { get; set; }
        public string TO { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public string DateSent { get; set; }
    }
}