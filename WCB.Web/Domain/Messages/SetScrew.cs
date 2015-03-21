using System;

namespace WCB.Web.Domain.Messages
{
    public class SetScrew : IObservable<SetScrew>
    {
        public State State { get; set; }

        public IDisposable Subscribe(IObserver<SetScrew> observer)
        {
            throw new NotImplementedException();
        }
    }
}