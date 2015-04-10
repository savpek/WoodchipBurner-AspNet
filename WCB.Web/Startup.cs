using System;
using System.Reactive.Linq;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.SignalR;
using Microsoft.Framework.DependencyInjection;
using WCB.Web.Lib.Domain;
using WCB.Web.Lib.Domain.Messages;
using WCB.Web.Lib.Messaging;

namespace WCB.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var publisher = new MessagePublisher();
            var screw = new ScrewAndAir(new ScrewIO(), publisher);

            Observable
                .Interval(TimeSpan.FromMilliseconds(250))
                .Timestamp()
                .Subscribe(_ => publisher.Publish(new TickMessage()));

            var random = new Random();

            Observable
                .Interval(TimeSpan.FromMilliseconds(1000))
                .Subscribe(_ => publisher.Publish(new SensorMessage(random.Next(0, 250))));

            services.AddInstance<IMessagePublisher>(publisher);
            services.AddInstance(screw);

            services.AddMvc();
            services.AddSignalR(o =>
            {
                o.Hubs.EnableDetailedErrors = true;
                o.Hubs.EnableJavaScriptProxies = true;
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSignalR();
            app.UseMvc();
        }
    }
}
