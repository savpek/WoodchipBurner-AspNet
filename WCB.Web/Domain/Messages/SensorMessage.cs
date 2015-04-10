namespace WCB.Web.Domain.Messages
{
    public class SensorMessage
    {
        public int RawValue { get; }
        public int LowerLimit => 0;
        public int UpperLimit => 250;

        public SensorMessage(int rawValue)
        {
            RawValue = rawValue;
        }

        public Percent AsPercent => CalculateAsPercent();

        private Percent CalculateAsPercent()
        {
            if (RawValue == 0)
                return new Percent(0);

            if(RawValue > UpperLimit)
                return new Percent(100);

            if (RawValue < LowerLimit)
                return new Percent(0);

            return new Percent(RawValue/UpperLimit);
        }
    }
}
