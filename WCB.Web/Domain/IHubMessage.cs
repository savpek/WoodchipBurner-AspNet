using System.Diagnostics.CodeAnalysis;

namespace WCB.Web.Domain
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface IHubMessage
    {
        void message(string message, object value);
    }
}