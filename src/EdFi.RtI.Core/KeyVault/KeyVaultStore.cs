using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.KeyVault
{
    public interface IKeyVaultStore
    {
        Task<string> Get(string key);
        Task Remove(string key);
        Task Set(string key, string value);
    }

    public class KeyVaultStore : IKeyVaultStore
    {
        private readonly KeyVaultAppSettings _keyVaultAppSettings;

        public KeyVaultStore(KeyVaultAppSettings keyVaultAppSettings)
        {
            _keyVaultAppSettings = keyVaultAppSettings;
        }

        public async Task<string> Get(string key)
        {
            try
            {
                var client = CreateSecretClient();
                var response = await client.GetSecretAsync(key);
                return response.Value.Value;
            }
            catch (Exception ex)
            {
                throw new KeyVaultGetException(key, ex);
            }
        }

        public async Task Remove(string key)
        {
            try
            {
                var client = CreateSecretClient();
                await client.StartDeleteSecretAsync(key);
            }
            catch (Exception ex)
            {
                throw new KeyVaultRemoveException(key, ex);
            }
        }

        public async Task Set(string key, string value)
        {
            var client = CreateSecretClient();
            var secret = new KeyVaultSecret(key, value);
            secret.Properties.ExpiresOn = DateTimeOffset.Now.AddYears(1);

            try
            {
                await client.SetSecretAsync(secret);
            }
            catch (Exception ex)
            {
                throw new KeyVaultSetException(key, ex);
            }
        }

        private SecretClient CreateSecretClient()
        {
            var uri = new Uri(_keyVaultAppSettings.Url);
            return new SecretClient(uri, new DefaultAzureCredential());
        }
    }
}
