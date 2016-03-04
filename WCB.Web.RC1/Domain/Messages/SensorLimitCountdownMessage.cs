namespace WCB.Web.RC1.Domain.Messages
{
    public class SensorLimitCountdownMessage
    {
        public double CurrentTotal { get; set; }
        public double LimitSeconds { get; set; }

        public SensorLimitCountdownMessage(double currentTotal, double limitSeconds)
        {
            CurrentTotal = currentTotal;
            LimitSeconds = limitSeconds;
        }
    }
}