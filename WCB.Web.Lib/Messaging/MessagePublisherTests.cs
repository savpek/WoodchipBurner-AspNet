using System;
using FluentAssertions;
using NUnit.Framework;

namespace WCB.Web.Lib.Messaging
{
    [TestFixture]
    public class MessagePublisherTests
    {
        [SetUp]
        public void Init() { }

        private class SampleEvent
        {
            public string SomeString => "Bar";
        }

        [Test]
        public void OnSubscribeEverythingWorksAsExpected()
        {
            bool eventWasRaised = false;
            var eventPublisher = new MessagePublisher();

            eventPublisher.GetEvent<SampleEvent>()
                .Subscribe(se => eventWasRaised = true);

            eventPublisher.Publish(new SampleEvent());
            eventWasRaised.Should().BeTrue();
        }

        [Test]
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
