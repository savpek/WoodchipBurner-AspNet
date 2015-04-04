using System;
using System.Reactive;
using Microsoft.AspNet.SignalR;

namespace WCB.Web.Lib.Domain
{
    public class DelayHub : Hub
    {
    }

    public class ScrewHub : Hub, IObserver<Screw>, IScrewHub, IObserver<Timestamped<long>>
    {
        private readonly IHubContext _hubContext;

        public ScrewHub(IHubContext hubContext)
        {
            _hubContext = hubContext;
        }

        public ScrewHub()
        {
        }

        public void ScrewState(State state)
        {
            _hubContext.Clients.All.message("screwState", state.ToString());
        }

        public void OnNext(Screw value)
        {
        }

        public void OnNext(Timestamped<long> value)
        {
            _hubContext.Clients.All.message("currentDelay", DateTime.Now.Second);
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}