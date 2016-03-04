using System.Diagnostics.CodeAnalysis;

namespace WCB.Web.RC1.Domain
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface IHubMessage
    {
        void message(string message, object value);
    }
}