using EdFi.RtI.Core.Errors;
using EdFi.RtI.Core.Infrastructure;
using EdFi.RtI.Core.Providers.Cache;
using EdFi.RtI.Core.Services.Settings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.UserRoleMappings
{
    public interface IUserRoleMappingsFacade
    {
        Task<IList<string>> GetUserRoleAdminMappings();
        Task<IList<string>> GetUserRoleTeacherMappings();
        Task SetUserRoleAdminMappings(IEnumerable<string> staffClassificationDescriptors);
        Task SetUserRoleTeacherMappings(IEnumerable<string> staffClassificationDescriptors);
    }

    public class UserRoleMappingsFacade : IUserRoleMappingsFacade
    {
        private readonly AppSettings _appSettings;
        private readonly ICacheStore _cacheProvider;
        private readonly UserRoleMappingsAppSettings _userRoleMappingsAppSettings;

        public UserRoleMappingsFacade(AppSettings appSettings, ICacheStore cacheProvider, UserRoleMappingsAppSettings userRoleMappingsAppSettings)
        {
            _appSettings = appSettings;
            _cacheProvider = cacheProvider;
            _userRoleMappingsAppSettings = userRoleMappingsAppSettings;
        }

        public async Task<IList<string>> GetUserRoleAdminMappings()
        {
            if (_appSettings.IsStartupModeHosted)
                return await GetUserRoleAdminMappingsFromCache();

            if (_appSettings.IsStartupModeStandalone)
                return GetUserRoleAdminMappingsFromAppSettings();

            throw new StartupModeNotSupportedException(_appSettings.StartupMode);
        }

        public async Task<IList<string>> GetUserRoleTeacherMappings()
        {
            if (_appSettings.IsStartupModeHosted)
                return await GetUserRoleTeacherMappingsFromCache();

            if (_appSettings.IsStartupModeStandalone)
                return GetUserRoleTeacherMappingsFromAppSettings();

            throw new StartupModeNotSupportedException(_appSettings.StartupMode);
        }

        public async Task SetUserRoleAdminMappings(IEnumerable<string> staffClassificationDescriptors)
        {
            if (_appSettings.IsStartupModeStandalone)
                throw new UnsupportedDomainOperationException(nameof(SetUserRoleAdminMappings), _appSettings.StartupMode);

            var edFiVersion = await GetEdFiVersion();
            var key = CacheKeys.Composed(CacheKeys.UserRoleAdminMappings, edFiVersion);
            await _cacheProvider.Set(key, staffClassificationDescriptors);
        }

        public async Task SetUserRoleTeacherMappings(IEnumerable<string> staffClassificationDescriptors)
        {
            if (_appSettings.IsStartupModeStandalone)
                throw new UnsupportedDomainOperationException(nameof(SetUserRoleTeacherMappings), _appSettings.StartupMode);

            var edFiVersion = await GetEdFiVersion(); 
            var key = CacheKeys.Composed(CacheKeys.UserRoleTeacherMappings, edFiVersion);
            await _cacheProvider.Set(key, staffClassificationDescriptors);
        }

        private async Task<string> GetEdFiVersion()
        {
            var edFiVersion = await _cacheProvider.GetOrDefault<string>(CacheKeys.EdFiVersion);

            if (edFiVersion == null)
                throw new EdFiVersionNotSetException();

            return edFiVersion;
        }

        private async Task<IList<string>> GetUserRoleAdminMappingsFromCache()
        {
            var edFiVersion = await GetEdFiVersion();
            var key = CacheKeys.Composed(CacheKeys.UserRoleAdminMappings, edFiVersion);
            var staffClassificationDescriptors = await _cacheProvider.GetOrDefault<IList<string>>(key);
            return staffClassificationDescriptors ?? new List<string>();
        }

        private async Task<IList<string>> GetUserRoleTeacherMappingsFromCache()
        {
            var edFiVersion = await GetEdFiVersion();
            var key = CacheKeys.Composed(CacheKeys.UserRoleTeacherMappings, edFiVersion);
            var staffClassificationDescriptors = await _cacheProvider.GetOrDefault<IList<string>>(key);
            return staffClassificationDescriptors ?? new List<string>();
        }

        private IList<string> GetUserRoleAdminMappingsFromAppSettings()
        {
            return _userRoleMappingsAppSettings.Admins;
        }

        private IList<string> GetUserRoleTeacherMappingsFromAppSettings()
        {
            return _userRoleMappingsAppSettings.Teachers;
        }
    }
}
