namespace WCB.Web.Domain.DataObjects
{
    public class CurrentSettings
    {
        public CurrentSettings()
        {
            Delay = 2;
            SensorMinimumLimit = 20;
            WorkPeriod = 2;
            AirFlow = 50;
        }

        public CurrentSettings(CurrentSettings settings)
        {
            Delay = settings.Delay;
            SensorMinimumLimit = settings.SensorMinimumLimit;
            WorkPeriod = settings.WorkPeriod;
            AirFlow = settings.AirFlow;
        }

        public uint Delay { get; set; }
        public uint SensorMinimumLimit { get; set; }
        public uint SensorLimitTimeTreshold { get; set; }
        public uint WorkPeriod { get; set; }
        public uint AirFlow { get; set; }
    }
}