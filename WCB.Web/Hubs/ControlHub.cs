using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using WCB.Web.Domain;
using WCB.Web.Domain.DataObjects;
using WCB.Web.Domain.Messages;
using WCB.Web.Messaging;

namespace WCB.Web.Hubs
{
    [HubName("screwHub")]
    public class ControlHub : Hub
    {
        private readonly IMessagePublisher _publisher;
        private readonly IHubContext _hubContext;
        private static CurrentSettings _settings = new CurrentSettings();
        private static Percent _lastSensor = new Percent(0);
        private static bool _broadcastInitialized;

        public ControlHub(IConnectionManager hubContext, IMessagePublisher publisher)
        {
            _publisher = publisher;
            _hubContext = hubContext.GetHubContext<ControlHub>();

            if (!_broadcastInitialized)
            {
                InitializeBroadcasts();
                _broadcastInitialized = true;
            }
        }

        private void InitializeBroadcasts()
        {
            _publisher.GetEvent<SettingsUpdatedMessage>()
                .Subscribe(x =>
                {
                    _hubContext.Clients.All.message("settingsUpdated", x.Settings);
                    _settings = new CurrentSettings(x.Settings);
                });

            _publisher.GetEvent<SensorMessage>()
                .Subscribe(x =>
                {
                    _lastSensor = x.AsPercent;
                    _hubContext.Clients.All.message("sensorValue", x.AsPercent);
                });

            _publisher.GetEvent<ScrewStateUpdatedMessage>()
                .Subscribe(x => _hubContext.Clients.All.message("screwState", x.State));

            _publisher.GetEvent<AirStateUpdatedMessage>()
                .Subscribe(x => _hubContext.Clients.All.message("airState", x.State));

            _publisher.GetEvent<SensorLimitCountdownMessage>()
                .Subscribe(x => _hubContext.Clients.All.message("sensorLimit", x));
        }

        public void UpdateSettings(CurrentSettings settings)
        {
            _publisher.Publish(new SettingsUpdatedMessage(settings));
        }

        public CurrentSettings GetCurrentSettings()
        {
            return _settings;
        }

        public void Enable()
        {
            _publisher.Publish(new EnableOrDisableDeviceMessage(State.Enabled));
        }

        public void Disable()
        {
            _publisher.Publish(new EnableOrDisableDeviceMessage(State.Disabled));
        }

        public int GetCurrentSensor()
        {
            return _lastSensor;
        }
    }
} 