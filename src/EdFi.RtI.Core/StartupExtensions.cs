using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.KeyVault;
using EdFi.RtI.Core.OdsApi;
using EdFi.RtI.Core.Providers.Cache;
using EdFi.RtI.Core.Providers.Environment;
using EdFi.RtI.Core.Providers.Staffs;
using EdFi.RtI.Core.Services;
using EdFi.RtI.Core.Services.Cache;
using EdFi.RtI.Core.Services.CacheRefresh;
using EdFi.RtI.Core.Services.Features;
using EdFi.RtI.Core.Services.Settings;
using EdFi.RtI.Core.UserRoleMappings;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace EdFi.RtI.Core
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCoreLayer(this IServiceCollection services)
        {
            return services
                .AddDomainServices()
                .AddTransient<IEnvironmentProvider, EnvironmentProvider>()
                .AddTransient<IKeyVaultFacade, KeyVaultFacade>()
                .AddTransient<IKeyVaultStore, KeyVaultStore>()
                .AddTransient<IOdsApiClientFactory, OdsApiClientFactory>()
                .AddTransient<IOdsApiClientProvider, OdsApiClientProvider>()
                .AddTransient<IOdsApiSettingsProvider, OdsApiSettingsProvider>()
                .AddTransient<IOdsTokenProvider, OdsTokenProvider>()
                .AddTransient<ISessionContext, SessionContext>()
                .AddTransient<IUserRoleMappingsFacade, UserRoleMappingsFacade>()
                .AddScoped<ICacheRefreshService, CacheRefreshService>()
                .AddScoped<ICacheService, CacheService>()
                .AddScoped<ICacheStore, CacheStore>()
                .AddScoped<IFeaturesService, FeaturesService>()
                .AddScoped<ISettingsService, SettingsService>()
                ;
        }

        private static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<IDomainServiceProvider, DomainServiceProvider.DomainServiceProvider>();

            var exportedCoreLayerTypes = typeof(ICoreLayerMarker).Assembly.ExportedTypes;
            var domainServiceTypes = exportedCoreLayerTypes.Where(type => type.Name.EndsWith("V2") || type.Name.EndsWith("V3"));

            foreach (var service in domainServiceTypes)
                services.AddTransient(service);

            return services;
        }
    }
}
