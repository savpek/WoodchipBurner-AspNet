﻿using System;

namespace WCB.Web.Domain.Messages
{
    public class TickMessage
    {
        public TickMessage()
        {
            Occurred = Time.Now;
        }

        public TickMessage(DateTime time)
        {
            Occurred = time;
        }

        public DateTime Occurred { get; private set; }
    }
}
