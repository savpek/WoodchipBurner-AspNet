using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using WCB.Web.Domain.Messages;
using WCB.Web.Messaging;

namespace WCB.Web.Domain
{
    [HubName("screwHub")]
    public class ScrewHub : Hub
    {
        private readonly IMessagePublisher _publisher;
        private readonly IHubContext _hubContext;
        private CurrentSettings _settings = new CurrentSettings();
        private Percent _lastSensor = new Percent(0);

        public ScrewHub(IConnectionManager hubContext, IMessagePublisher publisher)
        {
            _publisher = publisher;
            _hubContext = hubContext.GetHubContext<ScrewHub>();

            _publisher.GetEvent<SettingsUpdatedMessage>()
                .Subscribe(x =>
                {
                    _hubContext.Clients.All.message("settingsUpdated", x);
                    _settings = new CurrentSettings(x.Settings);
                });
                
            _publisher.GetEvent<SensorMessage>()
                .Subscribe(x =>
                {
                    _lastSensor = x.AsPercent;
                    _hubContext.Clients.All.message("sensorValue", x.AsPercent);
                });

            _publisher.GetEvent<ScrewStateUpdatedMessage>()
                .Subscribe(x => _hubContext.Clients.All.messsage("screwState", x));
        }

        public void UpdateSettings(CurrentSettings settings)
        {
            _publisher.Publish(new SettingsUpdatedMessage(settings));
        }

        public CurrentSettings GetCurrentSettings()
        {
            return _settings;
        }

        public int GetCurrentSensor()
        {
            return _lastSensor;
        }
    }
}