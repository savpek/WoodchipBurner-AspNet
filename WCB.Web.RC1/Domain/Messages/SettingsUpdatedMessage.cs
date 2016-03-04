using WCB.Web.RC1.Domain.DataObjects;

namespace WCB.Web.RC1.Domain.Messages
{
    public class SettingsUpdatedMessage
    {
        public CurrentSettings Settings { get; private set; }

        public SettingsUpdatedMessage(CurrentSettings settings)
        {
            Settings = settings;
        }
    }
}