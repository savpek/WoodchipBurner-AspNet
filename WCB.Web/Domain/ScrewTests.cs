using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using WCB.Web.Domain.DataObjects;
using WCB.Web.Domain.Messages;
using WCB.Web.Messaging;
using Xunit;

namespace WCB.Web.Domain
{
    public class ScrewTests
    {
        private ScrewAndAir _screwAndAir;
        private State _currentScrewState = State.Disabled;
        private readonly MessagePublisher _publisher;
        private State _currentAirState;

        public ScrewTests()
        {
            var io = Substitute.For<IScrewAndAirIO>();
            io.When(x => x.SetScrew(Arg.Any<State>())).Do(x => _currentScrewState = x.Arg<State>());
            io.When(x => x.SetAir(Arg.Any<State>())).Do(x => _currentAirState = x.Arg<State>());

            _publisher = new MessagePublisher();
            _screwAndAir = new ScrewAndAir(io, _publisher);
        }

        [Fact]
        public void SettingsUpdated_ResetSequence()
        {
            var settings = new CurrentSettings
            {
                Delay = 3,
                WorkPeriod = 3,
                SensorMinimumLimit = 50,
                SensorLimitTimeTreshold = 10
            };
            Setup(settings);

            _currentScrewState.Should().Be(State.Enabled);
        }

        [Fact]
        public void AirFlowControl_EnableAndDisableTimingWorksCorrectly()
        {
            var settings = new CurrentSettings
            {
                Delay = 3,
                WorkPeriod = 2,
                SensorMinimumLimit = 50,
                SensorLimitTimeTreshold = 10,
                AirFlow = 50 /* This is percent of hole sequence. */
            };
            Setup(settings);

            _publisher.Publish(new TickMessage(GetTimestamp(2400)));
            _currentAirState.Should().Be(State.Enabled);
            _publisher.Publish(new TickMessage(GetTimestamp(2600)));
            _currentAirState.Should().Be(State.Disabled);
        }

        [Fact]
        public void CommonEnabledSequence_EnableAndDisableScrewTimingWorksCorrectly()
        {
            var settings = new CurrentSettings
            {
                Delay = 3,
                WorkPeriod = 2,
                SensorMinimumLimit = 50,
                SensorLimitTimeTreshold = 10
            };
            Setup(settings);

            _publisher.Publish(new TickMessage(GetTimestamp(200)));
            _currentScrewState.Should().Be(State.Enabled);
            _publisher.Publish(new TickMessage(GetTimestamp(2500)));
            _currentScrewState.Should().Be(State.Disabled);
            _publisher.Publish(new TickMessage(GetTimestamp(5500)));
            _currentScrewState.Should().Be(State.Enabled);

        }

        [Fact]
        public void IfSensorValueUnderLimit_DisableAfterCountDown()
        {
            var settings = new CurrentSettings
            {
                Delay = 3,
                WorkPeriod = 2,
                SensorMinimumLimit = 50,
                SensorLimitTimeTreshold = 10
            };
            Setup(settings);

            var receivedLimitMessages = new List<SensorLimitCountdownMessage>();
            _publisher.GetEvent<SensorLimitCountdownMessage>().Subscribe(x => receivedLimitMessages.Add(x));

            _publisher.Publish(new SensorMessage(0));
            _publisher.Publish(new TickMessage(GetTimestamp(8000)));
            _currentScrewState.Should().Be(State.Enabled);
            _publisher.Publish(new TickMessage(GetTimestamp(11000)));
            _currentScrewState.Should().Be(State.Disabled);

            receivedLimitMessages.ShouldBeEquivalentTo(new List<SensorLimitCountdownMessage>
            {
                new SensorLimitCountdownMessage(currentTotal: 8, limitSeconds: 10),
                new SensorLimitCountdownMessage(currentTotal: 11, limitSeconds: 10)
            });
        }

        private void Setup(CurrentSettings settings)
        {
            _publisher.Publish(new SettingsUpdatedMessage(settings));
            _publisher.Publish(new EnableOrDisableDeviceMessage(State.Enabled));
            _publisher.Publish(new SensorMessage(250));
            _publisher.Publish(new TickMessage(GetTimestamp(0)));
        }

        private static DateTime GetTimestamp(int offSetMs)
        {
            return new DateTime(2014, 1, 1).AddMilliseconds(offSetMs);
        }
    }
}