using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrangeCloud.Core
{
    public static class StaticServiceProviderExtensions
    {
        public static IServiceCollection AddServiceProviderAccessor(this IServiceCollection services)
        {
            return services;
        }

        public static IApplicationBuilder UseServiceProvider(this IApplicationBuilder app)
        {
            DI.ServiceProvider = app.ApplicationServices;
            return app;
        }
    }
}
