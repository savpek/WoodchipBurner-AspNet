using System;
using System.Reactive.Linq;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using WCB.Web.Lib;

namespace WCB.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSignalR();
            app.UseMvc();
            var screw = new Screw();
            var observable = Observable.Interval(TimeSpan.FromSeconds(3)).Timestamp();
            observable.Subscribe(screw);
        }
    }
}
