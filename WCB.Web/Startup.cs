using System;
using System.Reactive.Linq;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using WCB.Web.Domain;
using WCB.Web.Domain.Messages;
using WCB.Web.Messaging;
using Microsoft.Framework.Logging.Console;

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
                app.UseErrorPage(ErrorPageOptions.ShowAll);
            }

            app.UseSignalR();
            app.UseStaticFiles();
        }
    }
}
