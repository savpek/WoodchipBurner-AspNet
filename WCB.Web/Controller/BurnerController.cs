using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using WCB.Web.Lib.Domain;

namespace WCB.Web.Controller
{
    public class BurnerController : Microsoft.AspNet.Mvc.Controller
    {
        private IHubContext _hub;

        public BurnerController(IConnectionManager connectionManager)
        {
            _hub = connectionManager.GetHubContext<ScrewHub>();
        }

        [HttpGet("burner/screw/delay")]
        public void GetScrewDelay()
        {
        }

        [HttpPut("burner/screw/{valueSeconds}")]
        public void SetScrewDelay(uint valueSeconds)
        {
        }
    }
}