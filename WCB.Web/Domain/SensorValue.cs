﻿namespace WCB.Web.Domain
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