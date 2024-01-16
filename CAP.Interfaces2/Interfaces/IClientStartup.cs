using Microsoft.Extensions.DependencyInjection;

namespace CAP.Interfaces2;
public interface IClientStartup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    void ConfigureServices(IServiceCollection services);
}
