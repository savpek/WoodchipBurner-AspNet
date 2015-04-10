namespace WCB.Web.Domain
{
    public class Percent
    {
        public readonly int Value;

        public Percent(int value)
        {
            Value = value;
        }

        public static implicit operator Percent(int value)
        {
            return new Percent(value);
        }

        public static implicit operator int(Percent percent)
        {
            return percent.Value;
        }
    }
}