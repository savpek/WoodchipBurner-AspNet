using System;

namespace WCB.Web.Domain
{
    internal class IOCardException : Exception
    {
        public IOCardException(string message) : base(message)
        {
        }
    }
}