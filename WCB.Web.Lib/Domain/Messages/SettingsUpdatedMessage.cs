namespace WCB.Web.Lib.Domain.Messages
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