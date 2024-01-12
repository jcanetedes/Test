﻿using Microsoft.Extensions.DependencyInjection;

namespace Test.Core.Interfaces;
public interface IClientStartup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    void ConfigureServices(IServiceCollection services);
}
