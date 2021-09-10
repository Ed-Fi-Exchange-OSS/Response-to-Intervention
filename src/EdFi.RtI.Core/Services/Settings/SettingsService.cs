using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Errors;
using EdFi.RtI.Core.Infrastructure;
using EdFi.RtI.Core.KeyVault;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.Models;
using EdFi.RtI.Core.OdsApi;
using EdFi.RtI.Core.Providers.Cache;
using EdFi.RtI.Core.Providers.Environment;
using EdFi.RtI.Core.UserRoleMappings;
using EdFi.RtI.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Settings
{
    public interface ISettingsService
    {
        Task<string> GetDefaultEdFiVersion();
        Task<OdsApiSettings> GetOdsApiSettings(string version);
        Task<IList<string>> GetUserRoleAdminMappings();
        Task<IList<string>> GetUserRoleTeacherMappings();
        Task RemoveOdsApiSettings();
        Task SaveOdsApiSettings(OdsApiSettings settings);
        Task SetDefaultEdFiVersion(string edFiVersion);
        Task SetUserRoleAdminMappings(IEnumerable<string> adminStaffClassificationDescriptors);
        Task SetUserRoleTeacherMappings(IEnumerable<string> teacherStaffClassificationDescriptors);
        Task TestOdsApiSettings(OdsApiSettings settings);
    }

    public class SettingsService : ISettingsService
    {
        private readonly AppSettings _appSettings;
        private readonly ICacheStore _cacheProvider;
        private readonly IEnvironmentProvider _environmentProvider;
        private readonly IKeyVaultFacade _keyVaultProvider;
        private readonly IOdsApiClientFactory _odsApiClientFactory;
        private readonly IOdsApiSettingsProvider _odsApiSettingsProvider;
        private readonly IUserRoleMappingsFacade _userRoleMappingsFacade;

        public SettingsService(AppSettings appSettings, ICacheStore cacheProvider, IEnvironmentProvider environmentProvider, IKeyVaultFacade keyVaultProvider, IOdsApiClientFactory odsApiClientFactory, IOdsApiSettingsProvider odsApiSettingsProvider, IUserRoleMappingsFacade userRoleMappingsFacade)
        {
            _appSettings = appSettings;
            _cacheProvider = cacheProvider;
            _environmentProvider = environmentProvider;
            _keyVaultProvider = keyVaultProvider;
            _odsApiClientFactory = odsApiClientFactory;
            _odsApiSettingsProvider = odsApiSettingsProvider;
            _userRoleMappingsFacade = userRoleMappingsFacade;
        }

        public async Task<string> GetDefaultEdFiVersion()
        {
            if (_appSettings.IsStartupModeHosted)
                return await _cacheProvider.GetOrDefault<string>(CacheKeys.EdFiVersion);

            if (_appSettings.IsStartupModeStandalone)
                return await _odsApiSettingsProvider.GetEdFiVersion();

            throw new StartupModeNotSupportedException(_appSettings.StartupMode);
        }

        public async Task<OdsApiSettings> GetOdsApiSettings(string version)
        {
            ThrowIfStartupModeIsNotHosted(nameof(GetOdsApiSettings));
            var key = CacheKeys.Composed(CacheKeys.OdsApiSettings, version);
            var odsApiSettings = await _cacheProvider.GetOrDefault<OdsApiSettings>(key);
            odsApiSettings.ClientSecret = null;
            return odsApiSettings;
        }

        public async Task<IList<string>> GetUserRoleAdminMappings()
        {
            return await _userRoleMappingsFacade.GetUserRoleAdminMappings();
        }

        public async Task<IList<string>> GetUserRoleTeacherMappings()
        {
            return await _userRoleMappingsFacade.GetUserRoleTeacherMappings();
        }

        public async Task RemoveOdsApiSettings()
        {
            ThrowIfStartupModeIsNotHosted(nameof(RemoveOdsApiSettings));
            await _cacheProvider.Remove(CacheKeys.OdsApiSettings);
        }

        public async Task SaveOdsApiSettings(OdsApiSettings settings)
        {
            ThrowIfStartupModeIsNotHosted(nameof(SaveOdsApiSettings));

            if (!_environmentProvider.IsEnvironmentLocal)
            {
                var clientSecret = settings.ClientSecret;
                await _keyVaultProvider.SaveClientSecret(settings.Version, clientSecret);
                settings.ClientSecret = null;
            }

            var odsApiSettingsKey = CacheKeys.Composed(CacheKeys.OdsApiSettings, settings.Version);
            await _cacheProvider.Set(odsApiSettingsKey, settings);
        }

        public async Task SetDefaultEdFiVersion(string edFiVersion)
        {
            ThrowIfStartupModeIsNotHosted(nameof(SetDefaultEdFiVersion));

            if (!EdFiVersion.IsValid(edFiVersion))
                throw new UnsupportedEdFiVersionException(edFiVersion);

            await _cacheProvider.Set(CacheKeys.EdFiVersion, edFiVersion);
        }

        public async Task SetUserRoleAdminMappings(IEnumerable<string> staffClassificationDescriptors)
        {
            ThrowIfStartupModeIsNotHosted(nameof(SetUserRoleAdminMappings));
            await _userRoleMappingsFacade.SetUserRoleAdminMappings(staffClassificationDescriptors);
        }

        public async Task SetUserRoleTeacherMappings(IEnumerable<string> staffClassificationDescriptors)
        {
            ThrowIfStartupModeIsNotHosted(nameof(SetUserRoleTeacherMappings));
            await _userRoleMappingsFacade.SetUserRoleTeacherMappings(staffClassificationDescriptors);
        }

        public async Task TestOdsApiSettings(OdsApiSettings settings)
        {
            ThrowIfStartupModeIsNotHosted(nameof(TestOdsApiSettings));

            if (settings.Version == EdFiVersion.v2)
            {
                await TestResourcesV2(settings);
                return;
            }

            if (settings.Version == EdFiVersion.v3)
            {
                await TestResourcesV3(settings);
                await TestCompositesV3(settings);
                return;
            }

            throw new UnsupportedEdFiVersionException(settings.Version);
        }

        private async Task<Intervention> CreateSampleInterventionV2(OdsApiClient client)
        {
            return new Intervention
            {
                IdentificationCode = KeyGenerator.New(30),
                EducationOrganizationReference = await GetSampleEducationOrganizationReferenceV2(client),
                BeginDate = DateTime.Now,
                DeliveryMethodType = await GetSampleDeliveryMethodDescriptorV2(client),
                ClassType = await GetSampleInterventionClassDescriptorV2(client),
            };
        }

        private async Task<InterventionModelv3> CreateSampleInterventionV3(OdsApiClient client)
        {
            return new InterventionModelv3
            {
                InterventionIdentificationCode = KeyGenerator.New(30),
                EducationOrganizationReference = await GetSampleEducationOrganizationReferenceV3(client),
                BeginDate = DateTime.Now.ToString("yyyy-MM-dd"),
                DeliveryMethodDescriptor = await GetSampleDeliveryMethodDescriptorV3(client),
                InterventionClassDescriptor = await GetSampleInterventionClassDescriptorV3(client),
            };
        }

        private async Task<string> GetSampleDeliveryMethodDescriptorV2(OdsApiClient client)
        {
            var descriptors = await client.Get<IList<Dictionary<string, object>>>("deliveryMethodTypes");
            return descriptors.First()["codeValue"].ToString();
        }

        private async Task<string> GetSampleDeliveryMethodDescriptorV3(OdsApiClient client)
        {
            var descriptors = await client.Get<IList<DeliveryMethodDescriptorv3>>("deliveryMethodDescriptors");
            return descriptors.First().CodeValue.MapToDeliveryMethodDescriptorV3();
        }

        private async Task<EducationOrganizationReferencev3> GetSampleEducationOrganizationReferenceV3(OdsApiClient client)
        {
            try
            {
                var localEducationAgencies = await client.Get<IList<LocalEducationAgencyv3>>("localEducationAgencies");
                return new EducationOrganizationReferencev3
                {
                    EducationOrganizationId = localEducationAgencies.First().LocalEducationAgencyId,
                };
            }
            catch
            {
                var schools = await client.Get<IList<Schoolv3>>("schools");
                return new EducationOrganizationReferencev3
                {
                    EducationOrganizationId = schools.First().SchoolId,
                };
            }
        }

        private async Task<InterventionEducationOrganizationReference> GetSampleEducationOrganizationReferenceV2(OdsApiClient client)
        {
            try
            {
                var localEducationAgencies = await client.Get<IList<LocalEducationAgency>>("localEducationAgencies");
                return new InterventionEducationOrganizationReference
                {
                    EducationOrganizationId = localEducationAgencies.First().LocalEducationAgencyId,
                };
            }
            catch
            {
                var schools = await client.Get<IList<School>>("schools");
                return new InterventionEducationOrganizationReference
                {
                    EducationOrganizationId = schools.First().SchoolId,
                };
            }
        }

        private async Task<string> GetSampleInterventionClassDescriptorV2(OdsApiClient client)
        {
            var descriptor = await client.Get<IList<Dictionary<string, object>>>("interventionClassTypes");
            return descriptor.First()["codeValue"].ToString();
        }

        private async Task<string> GetSampleInterventionClassDescriptorV3(OdsApiClient client)
        {
            var descriptor = await client.Get<IList<InterventionClassDescriptorv3>>("interventionClassDescriptors");
            return descriptor.First().CodeValue.MapToInterventionClassDescriptorTypeV3();
        }

        private async Task TestResourcesV2(OdsApiSettings settings)
        {
            var client = _odsApiClientFactory.NewResourcesClient(settings);

            var sampleIntervention = await CreateSampleInterventionV2(client);
            var postResponse = await client.Post("interventions", sampleIntervention);
            var createdInterventionId = await client.HandleHttpResponseGetCreatedResourceId(postResponse);

            var deleteResponse = await client.Delete($"interventions/{createdInterventionId}");
            await client.HandleHttpResponse(deleteResponse);
        }

        private async Task TestResourcesV3(OdsApiSettings settings)
        {
            var client = _odsApiClientFactory.NewResourcesClient(settings);

            var sampleIntervention = await CreateSampleInterventionV3(client);
            var postResponse = await client.Post("interventions", sampleIntervention);
            var createdInterventionId = await client.HandleHttpResponseGetCreatedResourceId(postResponse);

            var deleteResponse = await client.Delete($"interventions/{createdInterventionId}");
            await client.HandleHttpResponse(deleteResponse);
        }

        private async Task TestCompositesV3(OdsApiSettings settings)
        {
            var client = _odsApiClientFactory.NewCompositesClient(settings);
            var response = await client.Get("enrollment/Staffs");
            await client.HandleHttpResponse(response);
        }

        private void ThrowIfStartupModeIsNotHosted(string methodName)
        {
            if (!_appSettings.IsStartupModeHosted)
                throw new UnsupportedDomainOperationException(methodName, _appSettings.StartupMode);
        }
    }
}
