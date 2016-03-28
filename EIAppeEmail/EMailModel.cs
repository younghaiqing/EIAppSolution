using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIAppeEmail
{
    public class EMailModel
    {
        public string UID { set; get; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public List<string> Bcc { get; set; }
        public List<string> CC { get; set; }
        public List<string> Attachments { get; set; }
    }
}