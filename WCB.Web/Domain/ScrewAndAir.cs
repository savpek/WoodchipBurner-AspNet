using System;
using WCB.Web.Domain.DataObjects;
using WCB.Web.Domain.Messages;
using WCB.Web.Messaging;

namespace WCB.Web.Domain
{
    public class ScrewAndAir
    {
        private readonly IScrewAndAirIO _ioCard;
        private readonly IMessagePublisher _publisher;
        private CurrentSettings _settings = new CurrentSettings();
        private State _deviceEnabled = State.Disabled;

        private DateTime _sequenceBegin;
        private bool _workStarted;
        private DateTime _lastGoodSensorTiming;
        private Percent _lastSensorValue = new Percent(0);

        public ScrewAndAir(IScrewAndAirIO ioCard, IMessagePublisher publisher)
        {
            _ioCard = ioCard;
            _publisher = publisher;
            publisher.GetEvent<TickMessage>().Subscribe(x => OnTick(x.Occurred));
            publisher.GetEvent<SettingsUpdatedMessage>().Subscribe(x => OnSettingsUpdated(x.Settings));
            publisher.GetEvent<EnableOrDisableDeviceMessage>().Subscribe(x => _deviceEnabled = x.DesiredState);
            publisher.GetEvent<SensorMessage>().Subscribe(x => _lastSensorValue = x.AsPercent);
        }

        private void OnTick(DateTime time)
        {
            if (_lastSensorValue > _settings.SensorMinimumLimit )
                _lastGoodSensorTiming = time;

            var timeout = (time - (_lastGoodSensorTiming)).TotalSeconds;
            _publisher.Publish(new SensorLimitCountdownMessage(timeout, _settings.SensorLimitTimeTreshold));

            if (timeout > _settings.SensorLimitTimeTreshold)
                _deviceEnabled = State.Disabled;

            if (SequenceShouldReset(time))
                ResetSequence(time);

            if (ScrewWorkIsDone(time))
                UpdateScrewState(State.Disabled);

            if (NewMethod(time))
                UpdateAirState(State.Disabled);
        }

        private bool NewMethod(DateTime time)
        {
            return _workStarted && AirFlowTimeout(time);
        }

        private bool ScrewWorkIsDone(DateTime time)
        {
            return _workStarted && time > _sequenceBegin.AddSeconds(_settings.WorkPeriod);
        }

        private bool SequenceShouldReset(DateTime time)
        {
            return time > _sequenceBegin.AddSeconds(_settings.Delay).AddSeconds(_settings.WorkPeriod);
        }

        private bool AirFlowTimeout(DateTime currentTime)
        {
            var secondsElapsed = (currentTime - _sequenceBegin).TotalSeconds;

            if (_settings.AirFlow == 0)
                return true;

            return secondsElapsed > (_settings.WorkPeriod + _settings.Delay)*(((float)_settings.AirFlow)/100);
        }

        private void ResetSequence(DateTime value)
        {
            _sequenceBegin = value;
            _workStarted = true;
            UpdateScrewState(_deviceEnabled);
            UpdateAirState(_deviceEnabled);
        }

        private void UpdateScrewState(State state)
        {
            _ioCard.SetScrew(state);
            _publisher.Publish(new ScrewStateUpdatedMessage(state));
        }

        private void UpdateAirState(State state)
        {
            _ioCard.SetAir(state);
            _publisher.Publish(new AirStateUpdatedMessage(state));
        }

        private void OnSettingsUpdated(CurrentSettings value)
        {
            _settings = value;
            ResetSequence(DateTime.MinValue);
        }
    }
}