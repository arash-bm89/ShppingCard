using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Athena.CacheHelper
{
    public static class ConfigureCacheServicesExtension
    {
        public static void AddCacheServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDatabase>(cfg =>
            {
                var connection = ConnectionMultiplexer.Connect(configuration.GetConnectionString("redis"));
                return connection.GetDatabase();
            });

            services.AddScoped<ICacheHelper, CacheHelper>();
        }
    }
}
