using System;
using System.Reactive.Linq;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using WCB.Web.Domain;
using WCB.Web.Domain.Messages;
using WCB.Web.Messaging;

namespace WCB.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            var serialPortConfig = new Configuration();
            serialPortConfig.AddJsonFile("config.json");

            var publisher = new MessagePublisher();
            var logger = new Log(publisher);
            var io = new IOCard(logger, serialPortConfig);
            var screw = new ScrewAndAir(io, publisher);
            
            Observable
                .Interval(TimeSpan.FromMilliseconds(250))
                .Timestamp()
                .Subscribe(_ => publisher.Publish(new TickMessage()));

            Observable
                .Interval(TimeSpan.FromMilliseconds(1000))
                .Subscribe(_ => publisher.Publish(new SensorMessage(io.GetSensor())));

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
