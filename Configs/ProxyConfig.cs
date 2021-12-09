using Microsoft.Extensions.DependencyInjection;
using System;


namespace Cross.Proxy.Configs
{
    public static class ProxyConfig
    {
        public static IServiceCollection AddProxy(this IServiceCollection services, Action<ProxySettings> options = null)
        {
            services.AddHttpClient<IProxy, Proxy>();
            services.Configure(options);
            return services;
        }
    }
}
