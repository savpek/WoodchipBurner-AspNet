using System;
using System.Runtime.CompilerServices;

namespace WCB.Web.Domain
{
    public interface ILog
    {
        void Write(string message, [CallerMemberName] string caller = "", [CallerFilePath] string path = "");
    }
}