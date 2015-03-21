using System;
using System.Reactive.Linq;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;

namespace WCB.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            var screw = new Screw();

            var observable = Observable.Interval(TimeSpan.FromSeconds(3)).Timestamp();
            observable.Subscribe(screw);
        }
    }
}
