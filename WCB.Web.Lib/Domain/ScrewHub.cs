using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace WCB.Web.Lib.Domain
{
    public class ScrewHub : Hub, IScrewHub
    {
        private readonly IHubContext _hubContext;
        private uint _delay = 3;
        private uint _workPeriod;

        public ScrewHub(IConnectionManager hubContext)
        {
            _hubContext = hubContext.GetHubContext<ScrewHub>();
        }

        public void SetDelay(uint newDelaySec)
        {
            _delay = newDelaySec;
            _hubContext.Clients.All.message("delay", _delay);
        }

        public void SetWorkPeriod(uint valueSec)
        {
            _workPeriod = valueSec;
            _hubContext.Clients.All.message("workPeriod", _workPeriod);
        }

        public void ScrewState(State state)
        {
            _hubContext.Clients.All.message("screwState", state.ToString());
        }
    }
}