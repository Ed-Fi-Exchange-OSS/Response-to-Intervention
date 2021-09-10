using EdFi.RtI.Core.Errors;
using EdFi.RtI.Core.Infrastructure;
using EdFi.RtI.Core.KeyVault;
using EdFi.RtI.Core.Providers.Cache;
using EdFi.RtI.Core.Providers.Environment;
using EdFi.RtI.Core.Services.Settings;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.OdsApi
{
    public interface IOdsApiSettingsProvider
    {
        Task<string> GetAssessmentNamespace();
        Task<OdsApiSettings> GetOdsApiSettings();
        Task<string> GetEdFiVersion();
    }

    public class OdsApiSettingsProvider : IOdsApiSettingsProvider
    {
        private readonly AppSettings _appSettings;
        private readonly ICacheStore _cacheProvider;
        private readonly IConfiguration _configuration;
        private readonly IEnvironmentProvider _environmentProvider;
        private readonly IKeyVaultFacade _keyVaultProvider;

        public OdsApiSettingsProvider(AppSettings appSettings, ICacheStore cacheProvider, IConfiguration configuration, IEnvironmentProvider environmentProvider, IKeyVaultFacade keyVaultProvider)
        {
            _appSettings = appSettings;
            _cacheProvider = cacheProvider;
            _configuration = configuration;
            _environmentProvider = environmentProvider;
            _keyVaultProvider = keyVaultProvider;
        }

        public async Task<string> GetAssessmentNamespace()
        {
            var odsApiSettings = await GetOdsApiSettings();
            return odsApiSettings.AssessmentNamespace;
        }

        public async Task<string> GetEdFiVersion()
        {
            var odsApiSettings = await GetOdsApiSettings();
            return odsApiSettings.Version;
        }

        public async Task<OdsApiSettings> GetOdsApiSettings()
        {
            if (_appSettings.IsStartupModeHosted)
                return await GetOdsApiSettingsForHostedMode();

            if (_appSettings.IsStartupModeStandalone)
                return GetOdsApiSettingsForStandaloneMode();

            throw new StartupModeNotSupportedException(_appSettings.StartupMode);
        }

        public async Task<OdsApiSettings> GetOdsApiSettingsForHostedMode()
        {
            var edFiVersion = await _cacheProvider.GetOrDefault<string>(CacheKeys.EdFiVersion);

            if (edFiVersion == null)
                throw new EdFiVersionNotSetException();

            var odsApiSettingsKey = CacheKeys.Composed(CacheKeys.OdsApiSettings, edFiVersion);
            var odsApiSettings = await _cacheProvider.Get<OdsApiSettings>(odsApiSettingsKey);

            if (!_environmentProvider.IsEnvironmentLocal)
                odsApiSettings.ClientSecret = await _keyVaultProvider.GetClientSecret(odsApiSettings.Version);

            return odsApiSettings;
        }

        public OdsApiSettings GetOdsApiSettingsForStandaloneMode()
        {
            return new OdsApiSettings
            {
                AssessmentNamespace = _configuration["OdsApiSettings:AssessmentNamespace"],
                AuthUrl = _configuration["OdsApiSettings:AuthUrl"],
                ClientId = _configuration["OdsApiSettings:ClientId"],
                ClientSecret = _configuration["OdsApiSettings:ClientSecret"],
                CompositesUrl = _configuration["OdsApiSettings:CompositesUrl"],
                ResourcesUrl = _configuration["OdsApiSettings:ResourcesUrl"],
                TokenUrl = _configuration["OdsApiSettings:TokenUrl"],
                Version = _configuration["OdsApiSettings:Version"],
            };
        }
    }
}
