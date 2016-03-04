using System.IO;
using System.Runtime.CompilerServices;
using WCB.Web.RC1.Domain.Messages;
using WCB.Web.RC1.Messaging;

namespace WCB.Web.RC1.Domain
{
    public class Log : ILog
    {
        private readonly IMessagePublisher _publisher;

        public Log(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        public void Write(string message, [CallerMemberName] string caller = "", [CallerFilePath] string path = "")
        {
            message = $"[{caller}:{Path.GetFileName(path)}] {message}";
            _publisher.Publish(new LogMessage(message));
        }
    }
}