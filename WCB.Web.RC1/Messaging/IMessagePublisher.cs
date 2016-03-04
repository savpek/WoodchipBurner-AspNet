using System;

namespace WCB.Web.RC1.Messaging
{
    public interface IMessagePublisher
    {
        IObservable<TEvent> GetEvent<TEvent>();
        void Publish<TEvent>(TEvent sampleEvent);
    }
}