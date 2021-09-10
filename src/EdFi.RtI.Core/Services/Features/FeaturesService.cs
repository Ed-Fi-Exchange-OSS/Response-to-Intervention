using EdFi.RtI.Core.DTOs.Features;
using EdFi.RtI.Core.Providers.Cache;
using System;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Features
{
    public class FeaturesService : IFeaturesService
    {
        private readonly ICacheStore _cache;
        private readonly ISessionContext _sessionContext;

        public FeaturesService(ICacheStore cache, ISessionContext sessionContext)
        {
            _cache = cache;
            _sessionContext = sessionContext;
        }

        public async Task<FeaturesSettings> GetFeaturesSettings()
        {
            var features = await _cache.GetOrDefault<FeaturesSettings>(CacheKeys.FeaturesSettings);

            if (features == null)
                throw new FeaturesNotFoundException(_sessionContext.TenantId);

            return features;
        }

        public async Task UpdateFeaturesSettings(FeaturesSettings featureSettings)
        {
            await _cache.Set(CacheKeys.FeaturesSettings, featureSettings);
        }
    }
}
