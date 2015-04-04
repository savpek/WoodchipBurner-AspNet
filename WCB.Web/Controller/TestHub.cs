using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using WCB.Web.Lib.Domain;

namespace WCB.Web.Controller
{
    public class TestHub : Hub
    {
        private readonly IHubContext _context;

        public TestHub(IConnectionManager connectionManager)
        {
            _context = connectionManager.GetHubContext<TestHub>();
        }

        public string Get()
        {
            return "";
        }

        public void Test()
        {
            _context.Clients.All.sendMessage("3");
        }
    }
}   