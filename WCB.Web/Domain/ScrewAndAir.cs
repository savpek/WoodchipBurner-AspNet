using System;
using WCB.Web.Domain.Messages;
using WCB.Web.Messaging;

namespace WCB.Web.Domain
{
    public class ScrewAndAir
    {
        private readonly IScrewIO _ioCard;
        private readonly IMessagePublisher _publisher;
        private CurrentSettings _settings = new CurrentSettings();

        private DateTime _sequenceBegin;
        private bool _workStarted;

        public ScrewAndAir(IScrewIO ioCard, IMessagePublisher publisher)
        {
            _ioCard = ioCard;
            _publisher = publisher;
            publisher.GetEvent<TickMessage>().Subscribe(x => OnTick(x.Occurred));
            publisher.GetEvent<SettingsUpdatedMessage>().Subscribe(x => OnSettingsUpdated(x.Settings));
        }

        private void OnTick(DateTime time)
        {
            if (time > _sequenceBegin.AddSeconds(_settings.Delay).AddSeconds(_settings.WorkPeriod))
                ResetSequence(time);

            if (_workStarted && time > _sequenceBegin.AddSeconds(_settings.WorkPeriod))
                UpdateScrewState(State.Disabled);
        }

        private void ResetSequence(DateTime value)
        {
            _sequenceBegin = value;
            _workStarted = true;
            UpdateScrewState(_settings.ScrewEnabled);
        }

        private void UpdateScrewState(State state)
        {
            _ioCard.SetScrew(state);
            _publisher.Publish(new ScrewStateUpdatedMessage(state));
        }

        private void OnSettingsUpdated(CurrentSettings value)
        {
            _settings = value;
            ResetSequence(DateTime.MinValue);
        }
    }
}