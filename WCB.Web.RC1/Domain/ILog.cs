using System.Runtime.CompilerServices;

namespace WCB.Web.RC1.Domain
{
    public interface ILog
    {
        void Write(string message, [CallerMemberName] string caller = "", [CallerFilePath] string path = "");
    }
}