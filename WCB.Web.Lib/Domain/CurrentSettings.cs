namespace WCB.Web.Lib.Domain
{
    public class CurrentSettings
    {
        public CurrentSettings()
        {
            Delay = 2;
            SensorMinimumLimit = 20;
            WorkPeriod = 2;
        }

        public CurrentSettings(CurrentSettings settings)
        {
            Delay = settings.Delay;
            SensorMinimumLimit = settings.SensorMinimumLimit;
            WorkPeriod = settings.WorkPeriod;
            settings.ScrewEnabled = settings.ScrewEnabled;
        }

        public uint Delay { get; set; }
        public uint SensorMinimumLimit { get; set; }
        public uint WorkPeriod { get; set; }
        public State ScrewEnabled { get; set; }
    }
}