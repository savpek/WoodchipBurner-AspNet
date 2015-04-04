using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using WCB.Web.Lib.Domain;

namespace WCB.Web.Controller
{
    public class BurnerController : Microsoft.AspNet.Mvc.Controller
    {
        private readonly IHubContext _hub;
        private uint _currentDelay = 3;

        public BurnerController(IConnectionManager connectionManager)
        {
            _hub = connectionManager.GetHubContext<DelayHub>();
        }

        [HttpGet("burner/delay")]
        public uint GetScrewDelay() => _currentDelay;

        [HttpPut("burner/delay/{valueSeconds}")]
        public void SetScrewDelay(uint valueSeconds)
        {
            _currentDelay = valueSeconds;
            _hub.Clients.All.message("delay", valueSeconds);
        }

        [HttpGet("burner/delay/{valueSeconds}")]
        public void SetScrewDelay2(uint valueSeconds)
        {
            _currentDelay = valueSeconds;
            _hub.Clients.All.message("delay", valueSeconds);
        }
    }
}