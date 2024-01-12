using Microsoft.AspNetCore.Builder;

namespace BlazorApp2.Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOqtane();
        }
        public void Configure(IApplicationBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
            
        }

    }
}
