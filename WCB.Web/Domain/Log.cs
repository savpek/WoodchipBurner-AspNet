using System;
using System.IO;
using System.Runtime.CompilerServices;
using WCB.Web.Domain.Messages;
using WCB.Web.Messaging;

namespace WCB.Web.Domain
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