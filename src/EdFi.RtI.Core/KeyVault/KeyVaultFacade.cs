using EdFi.RtI.Core.Infrastructure;
using EdFi.RtI.Core.Services;
using EdFi.RtI.Core.Services.Settings;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.KeyVault
{
    public interface IKeyVaultFacade
    {
        Task<string> GetClientSecret(string edFiVersion);
        string GetClientSecretKey(string edFiVersion);
        Task SaveClientSecret(string edFiVersion, string clientSecret);
    }

    public class KeyVaultFacade : IKeyVaultFacade
    {
        private readonly IKeyVaultStore _keyVaultStore;
        private readonly ISessionContext _sessionContext;
        private readonly KeyVaultAppSettings _keyVaultAppSettings;

        public KeyVaultFacade(IKeyVaultStore keyVaultStore, ISessionContext sessionContext, KeyVaultAppSettings keyVaultAppSettings)
        {
            _keyVaultStore = keyVaultStore;
            _sessionContext = sessionContext;
            _keyVaultAppSettings = keyVaultAppSettings;
        }

        public async Task<string> GetClientSecret(string edFiVersion)
        {
            if (!EdFiVersion.IsValid(edFiVersion))
                throw new UnsupportedEdFiVersionException(edFiVersion);

            var key = GetClientSecretKeyUncheked(edFiVersion);
            return await _keyVaultStore.Get(key);
        }

        public string GetClientSecretKey(string edFiVersion)
        {
            if (!EdFiVersion.IsValid(edFiVersion))
                throw new UnsupportedEdFiVersionException(edFiVersion);

            return GetClientSecretKeyUncheked(edFiVersion);
        }

        public async Task SaveClientSecret(string edFiVersion, string clientSecret)
        {
            if (!EdFiVersion.IsValid(edFiVersion))
                throw new UnsupportedEdFiVersionException(edFiVersion);

            var key = GetClientSecretKeyUncheked(edFiVersion);
            await _keyVaultStore.Set(key, clientSecret);
        }

        private string GetClientSecretKeyUncheked(string edFiVersion)
        {
            return _keyVaultAppSettings
                .SecretTemplate
                .Replace("{TenantId}", _sessionContext.TenantId)
                .Replace("{EdFiVersion}", edFiVersion)
                ;
        }
    }
}
