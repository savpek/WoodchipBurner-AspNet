using System;
using System.Reactive;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using WCB.Web.Lib.Domain.Messages;
using WCB.Web.Lib.Messaging;

namespace WCB.Web.Lib.Domain
{
    [TestFixture]
    public class ScrewTests
    {
        private IScrewIO _io;
        private ScrewAndAir _screwAndAir;
        private State _currentState = State.Disabled;
        private MessagePublisher _publisher;

        [SetUp]
        public void Init()
        {
            _io = Substitute.For<IScrewIO>();
            _io.When(x => x.SetScrew(Arg.Any<State>())).Do(x => _currentState = x.Arg<State>());

            _publisher = new MessagePublisher();
            _screwAndAir = new ScrewAndAir(_io, _publisher);
        }

        [Test]
        public void SettingsUpdated_ResetSequence()
        {
            var settings = new CurrentSettings
            {
                Delay = 3,
                WorkPeriod = 3
            };

            _publisher.Publish(new SettingsUpdatedMessage(settings));
            _currentState.Should().Be(State.Enabled);
        }

        [Test]
        public void CommonEnabledSequence()
        {
            var settings = new CurrentSettings
            {
                ScrewEnabled = State.Enabled,
                Delay = 3,
                WorkPeriod = 2
            };
            _publisher.Publish(new SettingsUpdatedMessage(settings));

            _publisher.Publish(new TickMessage(GetTimestamp(0)));

            _publisher.Publish(new TickMessage(GetTimestamp(1500)));
            _currentState.Should().Be(State.Enabled);
            _publisher.Publish(new TickMessage(GetTimestamp(2500)));
            _currentState.Should().Be(State.Disabled);
            _publisher.Publish(new TickMessage(GetTimestamp(5500)));
            _currentState.Should().Be(State.Enabled);
        }

        private static DateTime GetTimestamp(int offSetMs)
        {
            return new DateTime(2014, 1, 1).AddMilliseconds(offSetMs);
        }
    }
}