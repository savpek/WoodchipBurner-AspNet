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
        private uint _workPeriod = 5;

        public BurnerController(IConnectionManager connectionManager)
        {
            _hub = connectionManager.GetHubContext<ScrewHub>();
        }

        [HttpGet("burner/delay")]
        public uint GetScrewDelay() => _currentDelay;

        [HttpPut("burner/delay/{valueSeconds}")]
        public void SetScrewDelay(uint valueSeconds)
        {
            _currentDelay = valueSeconds;
            _hub.Clients.All.message("delay", valueSeconds);
        }

        [HttpGet("burner/workperiod")]
        public uint GetScrewWorkPeriod() => _workPeriod;

        [HttpPut("burner/workperiod/{valueSeconds}")]
        public void SetScrewWorkPeriod(uint valueSeconds)
        {
            _workPeriod = valueSeconds;
            _hub.Clients.All.message("workPeriod", valueSeconds);
        }

        public uint GetBrightnessSensor() => 50;
    }
}