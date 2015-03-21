using Microsoft.AspNet.SignalR;
using NSubstitute;
using NUnit.Framework;

namespace WCB.Web.Lib.Domain
{
    public class BurnerControllerApi
    {
        private readonly IHubContext<ScrewHub> _context;

        public BurnerControllerApi(IHubContext<ScrewHub> context)
        {
            _context = context;
        }
    }

    [TestFixture]
    public class BurnerControllerApiTests
    {
        private ScrewHub _hub;
        private IHubContext _hubContext;
        private IHubMessage _client;

        [SetUp]
        public void Init()
        {
            _hubContext = Substitute.For<IHubContext>();
            _client = Substitute.For<IHubMessage>();
            SubstituteExtensions.Returns(_hubContext.Clients.All, _client);
            _hub = new ScrewHub(_hubContext);
        }

        [Test]
        public void ScrewEnabledExecuteRoutines()
        {
            _hub.ScrewState(State.Disabled);
            _client.message("setScrewState", "setScrewState");

            _client.Received(1).message("screwState", "disabled");
        }
    }

}
