using Hangfire;
using Hangfire.PostgreSql;

namespace ShoppingCard.Api.ExtensionMethods
{
    public static class HangfireServiceCollectionExtension
    {
        public static void AddHangfireServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddHangfire(configuration =>
                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UsePostgreSqlStorage(config.GetConnectionString("HangfireConnection"), new PostgreSqlStorageOptions()));
        }
    }
}
