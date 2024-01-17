using System.Collections.Generic;
using System.Net.Mail;

namespace Ftp.CrossCutting.Email
{
    public class Email
    {
        public string DisplayName { get; set; }
        public string FromAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> ToAddress { get; set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public List<string> ToHiddenAddress { get; set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Attachment Attachment { get; set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public AlternateView AlternateView { get; set; }
        public MailPriority Priority { get; set; }
    }
}