using EdFi.RtI.Core.Errors;
using EdFi.RtI.Core.Services;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Providers.Cache
{
    public interface ICacheStore
    {
        Task Clear();
        Task<T> Get<T>(string key);
        Task<T> GetOrDefault<T>(string key);
        IList<string> GetKeys();
        Task<bool> HasKey(string key);
        Task Set(string key, object value);
        Task<bool> TryHasKey(string key);
        Task<bool> TrySet(string key, object value);
        Task Remove(string key);
    }

    public class CacheStore : ICacheStore
    {
        private readonly CacheAppSettings _cacheAppSettings;
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
        private readonly ISessionContext _sessionContext;

        public CacheStore(CacheAppSettings cacheAppSettings, ISessionContext sessionContext)
        {
            _cacheAppSettings = cacheAppSettings;
            _sessionContext = sessionContext;

            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                var connectionString = _cacheAppSettings.CacheConn;

                try
                {
                    return ConnectionMultiplexer.Connect(connectionString);
                }
                catch (Exception ex)
                {
                    throw new CacheProviderConnectionException(connectionString, ex);
                }
            });
        }

        private IDatabase Cache
        {
            get {
                return Connection.GetDatabase();
            }
        }

        private ConnectionMultiplexer Connection
        {
            get {
                return _lazyConnection.Value;
            }
        }

        public IList<string> GetKeys()
        {
            var host = _cacheAppSettings.CacheHost;
            var redis = Connection.GetServer(host);
            var keys = redis
                .Keys()
                .Select(key => key.ToString())
                .ToList();

            keys.Sort();

            return keys;
        }

        public async Task Set(string key, object value)
        {
            string cacheKey = Key(key);
            string json = JsonConvert.SerializeObject(value);
            await Cache.StringSetAsync(cacheKey, json);
        }

        public async Task<bool> TrySet(string key, object value)
        {
            try
            {
                await Set(key, value);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task Remove(string key)
        {
            var cacheKey = Key(key);
            await Cache.KeyDeleteAsync(cacheKey);
        }

        public async Task<bool> HasKey(string key)
        {
            var cacheKey = Key(key);
            return await Cache.KeyExistsAsync(cacheKey);
        }

        public async Task<bool> TryHasKey(string key)
        {
            try
            {
                var cacheKey = Key(key);
                var result = await HasKey(cacheKey);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task Clear()
        {
            var endpoints = Connection.GetEndPoints(true);

            foreach (var endpoint in endpoints)
            {
                var server = Connection.GetServer(endpoint);
                await server.FlushAllDatabasesAsync();
            }
        }

        public async Task<T> Get<T>(string key)
        {
            var cacheKey = Key(key);
            var value = await Cache.StringGetAsync(cacheKey);

            if (value == RedisValue.Null)
                throw new KeyNotFoundException(key);

            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<T> GetOrDefault<T>(string key)
        {
            try
            {
                var cacheKey = Key(key);
                return await Get<T>(cacheKey);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        private string Key(string key)
        {
            if (KeyHasPrefix(key))
                return key;

            var rtiPrefix = _cacheAppSettings.CachePrefix;
            var tenantId = _sessionContext.TenantId;
            return $"{rtiPrefix}-{tenantId}-{key}";
        }

        private bool KeyHasPrefix(string key)
        {
            var rtiPrefix = _cacheAppSettings.CachePrefix;
            return key.Contains(rtiPrefix);
        }
    }
}
