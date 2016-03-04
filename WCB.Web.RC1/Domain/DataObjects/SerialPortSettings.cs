﻿namespace WCB.Web.RC1.Domain.DataObjects
{
    public class SerialPortSettings
    {
        public SerialPortSettings()
        {
            Port = "COM4";
            Baud = 9600;
        }

        public int Baud { get; private set; }

        public string Port { get; private set; }
    }
}