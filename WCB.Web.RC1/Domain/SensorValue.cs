namespace WCB.Web.RC1.Domain
{
    public class SensorValue
    {
        public SensorValue(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public static implicit operator int (SensorValue value)
        {
            return value.Value;
        }
    }
}