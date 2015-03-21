using System;
using Microsoft.AspNet.SignalR;

namespace WCB.Web.Lib.Domain
{
    public class ScrewHub : Hub, IObserver<Screw>, IScrewHub
    {
        private readonly IHubContext _hubContext;

        public ScrewHub(IHubContext hubContext)
        {
            _hubContext = hubContext;
        }

        public void ScrewState(State state)
        {
            _hubContext.Clients.All.message("screwState", state.ToString());
        }

        public void OnNext(Screw value)
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}