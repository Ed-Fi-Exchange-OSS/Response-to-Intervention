using EdFi.RtI.Core.Infrastructure;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.OdsApi;
using EdFi.RtI.Core.Providers.Cache;
using EdFi.RtI.Core.Providers.Staffs;
using EdFi.RtI.Core.Services.Assessments;
using EdFi.RtI.Core.Services.CacheRefresh;
using EdFi.RtI.Core.Services.Catalogs;
using EdFi.RtI.Core.Services.Descriptors;
using EdFi.RtI.Core.Services.Interventions;
using EdFi.RtI.Core.Services.Me;
using EdFi.RtI.Core.Services.ScoringAssessments;
using EdFi.RtI.Core.Services.ScoringInterventions;
using EdFi.RtI.Core.Services.Students;
using EdFi.RtI.Core.Services.Types;
using System;

namespace EdFi.RtI.Core.DomainServiceProvider
{
    public interface IDomainServiceProvider
    {
        T GetService<T>() where T : IDomainService;
    }

    public class DomainServiceProvider : IDomainServiceProvider
    {
        private readonly IOdsApiSettingsProvider _odsApiSettingsProvider;
        private readonly IServiceProvider _serviceProvider;

        public DomainServiceProvider(IOdsApiSettingsProvider odsApiSettingsProvider, IServiceProvider serviceProvider)
        {
            _odsApiSettingsProvider = odsApiSettingsProvider;
            _serviceProvider = serviceProvider;
        }

        public T GetService<T>() where T : IDomainService
        {
            var edFiVersion = _odsApiSettingsProvider.GetEdFiVersion().Result;

            if (typeof(T) == typeof(IAssessmentService))
            {
                return (T)TryGetService(
                    edFiVersion: edFiVersion,
                    serviceName: nameof(IAssessmentService),
                    serviceType: typeof(IAssessmentService),
                    implemenationTypeV2: typeof(AssessmentServiceV2),
                    implemenationTypeV3: typeof(AssessmentServiceV3)
                );
            }

            if (typeof(T) == typeof(ICacheRefreshService))
            {
                var service = _serviceProvider.GetService(typeof(CacheRefreshService));

                if (service == null)
                    throw new DomainServiceNotImplementedException(typeof(ICacheRefreshService));

                return (T)service;
            }

            if (typeof(T) == typeof(ICatalogService))
            {
                return (T)TryGetService(
                    edFiVersion: edFiVersion,
                    serviceName: nameof(ICatalogService),
                    serviceType: typeof(ICatalogService),
                    implemenationTypeV2: typeof(CatalogServiceV2),
                    implemenationTypeV3: typeof(CatalogServiceV3)
                );
            }

            if (typeof(T) == typeof(IDescriptorService))
            {
                return (T)TryGetService(
                    edFiVersion: edFiVersion,
                    serviceName: nameof(IDescriptorService),
                    serviceType: typeof(IDescriptorService),
                    implemenationTypeV2: typeof(DescriptorServiceV2),
                    implemenationTypeV3: typeof(DescriptorServiceV3)
                );
            }

            if (typeof(T) == typeof(IEdFiMapper))
            {
                return (T)TryGetService(
                    edFiVersion: edFiVersion,
                    serviceName: nameof(IEdFiMapper),
                    serviceType: typeof(IEdFiMapper),
                    implemenationTypeV2: typeof(EdFiMapperV2),
                    implemenationTypeV3: typeof(EdFiMapperV3)
                );
            }

            if (typeof(T) == typeof(IInterventionService))
            {
                return (T)TryGetService(
                    edFiVersion: edFiVersion,
                    serviceName: nameof(IInterventionService),
                    serviceType: typeof(IInterventionService),
                    implemenationTypeV2: typeof(InterventionServiceV2),
                    implemenationTypeV3: typeof(InterventionServiceV3)
                );
            }

            if (typeof(T) == typeof(IMeService))
            {
                return (T)TryGetService(
                    edFiVersion: edFiVersion,
                    serviceName: nameof(IMeService),
                    serviceType: typeof(IMeService),
                    implemenationTypeV2: typeof(MeServiceV2),
                    implemenationTypeV3: typeof(MeServiceV3)
                );
            }

            if (typeof(T) == typeof(IScoringAssessmentsService))
            {
                return (T)TryGetService(
                    edFiVersion: edFiVersion,
                    serviceName: nameof(IScoringAssessmentsService),
                    serviceType: typeof(IScoringAssessmentsService),
                    implemenationTypeV2: typeof(ScoringAssessmentsServiceV2),
                    implemenationTypeV3: typeof(ScoringAssessmentsServiceV3)
                );
            }

            if (typeof(T) == typeof(IScoringInterventionsService))
            {
                return (T)TryGetService(
                    edFiVersion: edFiVersion,
                    serviceName: nameof(IScoringInterventionsService),
                    serviceType: typeof(IScoringInterventionsService),
                    implemenationTypeV2: typeof(ScoringInterventionsServiceV2),
                    implemenationTypeV3: typeof(ScoringInterventionsServiceV3)
                );
            }

            if (typeof(T) == typeof(IStaffsProvider))
            {
                return (T)TryGetService(
                    edFiVersion: edFiVersion,
                    serviceName: nameof(IStaffsProvider),
                    serviceType: typeof(IStaffsProvider),
                    implemenationTypeV2: typeof(StaffsProviderV2),
                    implemenationTypeV3: typeof(StaffsProviderV3)
                );
            }

            if (typeof(T) == typeof(IStudentService))
            {
                return (T)TryGetService(
                    edFiVersion: edFiVersion,
                    serviceName: nameof(IStudentService),
                    serviceType: typeof(IStudentService),
                    implemenationTypeV2: typeof(StudentServiceV2),
                    implemenationTypeV3: typeof(StudentServiceV3)
                );
            }
            
            if (typeof(T) == typeof(ITypesService))
            {
                return (T)TryGetService(
                    edFiVersion: edFiVersion,
                    serviceName: nameof(ITypesService),
                    serviceType: typeof(ITypesService),
                    implemenationTypeV2: typeof(TypesServiceV2),
                    implemenationTypeV3: typeof(TypesServiceV3)
                );
            }

            throw new DomainServiceNotImplementedException(typeof(T));
        }

        private object TryGetService(string edFiVersion, string serviceName, Type serviceType, Type implemenationTypeV2, Type implemenationTypeV3)
        {
            object service;

            if (edFiVersion == EdFiVersion.v2)
                service = _serviceProvider.GetService(implemenationTypeV2);

            else if (edFiVersion == EdFiVersion.v3)
                service = _serviceProvider.GetService(implemenationTypeV3);
            else
                throw new DomainServiceVersionNotImplementedException(serviceName, edFiVersion);

            if (service == null)
                throw new DomainServiceNotImplementedException(serviceType);

            return service;
        }
    }
}
