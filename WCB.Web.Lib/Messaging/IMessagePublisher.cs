using System;

namespace WCB.Web.Lib.Messaging
{
    public interface IMessagePublisher
    {
        IObservable<TEvent> GetEvent<TEvent>();
        void Publish<TEvent>(TEvent sampleEvent);
    }
}