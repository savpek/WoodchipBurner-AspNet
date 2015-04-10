using System;
using FluentAssertions;
using NSubstitute;
using WCB.Web.Domain.Messages;
using WCB.Web.Messaging;
using Xunit;

namespace WCB.Web.Domain
{
    public class ScrewTests
    {
        private ScrewAndAir _screwAndAir;
        private State _currentState = State.Disabled;
        private MessagePublisher _publisher;

        public ScrewTests()
        {
            var io = Substitute.For<IScrewIO>();
            io.When(x => x.SetScrew(Arg.Any<State>())).Do(x => _currentState = x.Arg<State>());

            _publisher = new MessagePublisher();
            _screwAndAir = new ScrewAndAir(io, _publisher);
        }

        [Fact]
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

        [Fact]
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