using System;
using System.Reactive.Linq;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WCB.Web.RC1.Domain;
using WCB.Web.RC1.Domain.Messages;
using WCB.Web.RC1.Messaging;

namespace WCB.Web.RC1
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var publisher = new MessagePublisher();
            var logger = new Log(publisher);
            var io = new VirtualIOCard(logger);
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

            services.AddSignalR(o =>
            {
                o.Hubs.EnableDetailedErrors = true;
                o.Hubs.EnableJavaScriptProxies = true;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            loggerfactory.AddConsole();

            if (string.Equals(env.EnvironmentName, "Development", StringComparison.OrdinalIgnoreCase))
            {
                app.UseBrowserLink();
            }

            app.UseSignalR();
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }

        public static void Main(string[] args) =>
            WebApplication.Run<Startup>(args);
    }
}
