namespace WCB.Web.Domain.Messages
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