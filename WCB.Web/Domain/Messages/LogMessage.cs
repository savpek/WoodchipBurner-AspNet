using System;

namespace WCB.Web.Domain.Messages
{
    public class LogMessage
    {
        public LogMessage(string message)
        {
            Message = message;
            Stamp = Time.Now;
        }
        public string Message { get; }
        public DateTime Stamp { get; }
    }
}