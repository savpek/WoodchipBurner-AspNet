using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using WCB.Web.Domain.DataObjects;
using WCB.Web.Domain.Messages;
using WCB.Web.Messaging;

namespace WCB.Web.Hubs
{
    [HubName("logHub")]
    public class LogHub : Hub
    {
        private readonly IHubContext _hubContext;
        private static bool _broadCastInitilized;

        public LogHub(IConnectionManager hubContext, IMessagePublisher publisher)
        {
            _hubContext = hubContext.GetHubContext<LogHub>();

            if (!_broadCastInitilized)
            {
                publisher.GetEvent<LogMessage>()
                    .Subscribe(x =>
                    {
                        _hubContext.Clients.All.message(x);
                    });
                _broadCastInitilized = true;
            }
        }
    }
} 