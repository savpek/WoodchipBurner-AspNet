using System;
using FluentAssertions;
using Xunit;

namespace WCB.Web.Messaging
{
    public class MessagePublisherTests
    {
        public MessagePublisherTests() { }

        private class SampleEvent
        {
            public string SomeString => "Bar";
        }

        [Fact]
        public void OnSubscribeEverythingWorksAsExpected()
        {
            bool eventWasRaised = false;
            var eventPublisher = new MessagePublisher();

            eventPublisher.GetEvent<SampleEvent>()
                .Subscribe(se => eventWasRaised = true);

            eventPublisher.Publish(new SampleEvent());
            eventWasRaised.Should().BeTrue();
        }

        [Fact]
        public void OnUnsubscribeEverythingWorksAsExpected()
        {
            bool eventWasRaised = false;
            var eventPublisher = new MessagePublisher();

            var subscription = eventPublisher.GetEvent<SampleEvent>()
                .Subscribe(se => eventWasRaised = true);

            subscription.Dispose();
            eventPublisher.Publish(new SampleEvent());
            eventWasRaised.Should().BeFalse();
        }
    }
}
