using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Types;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.OdsApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Types
{
    public class TypesServiceV2 : ITypesService
    {
        private readonly IDomainServiceProvider _domainServiceProvider;
        private readonly IOdsApiClientProvider _odsApiClientProvider;

        public TypesServiceV2(IDomainServiceProvider domainServiceProvider, IOdsApiClientProvider odsApiClientProvider)
        {
            _domainServiceProvider = domainServiceProvider;
            _odsApiClientProvider = odsApiClientProvider;
        }

        private IEdFiMapper _mapper => _domainServiceProvider.GetService<IEdFiMapper>();

        public async Task<object> GetAssessmentItemResults()
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();

            var rawAssociations = await ods.Get<object>("assessmentItemResultTypes", new Dictionary<string, string>
            {
                ["limit"] = "100",
            });

            return _mapper.Map<IEnumerable<AssessmentResultDTO>>(rawAssociations);
        }

        public async Task<object> GetAssessmentReportingMethods()
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();

            object rawAssociations = await ods.Get<object>("assessmentReportingMethodTypes", new Dictionary<string, string>
            {
                ["limit"] = "100",
            });

            return _mapper.Map<IEnumerable<AssessmentReportingMethodDTO>>(rawAssociations);
        }

        public async Task<object> GetInterventionClasses()
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();

            var rawAssociations = await ods.Get<object>("interventionClassTypes", new Dictionary<string, string>
            {
                ["limit"] = "100",
            });

            return _mapper.Map<IEnumerable<InterventionClassDTO>>(rawAssociations);
        }

        public async Task<object> GetDeliveryMethods()
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();

            var rawAssociations = await ods.Get<object>("deliveryMethodTypes", new Dictionary<string, string>
            {
                ["limit"] = "100",
            });

            return _mapper.Map<IEnumerable<DeliveryMethodDTO>>(rawAssociations);
        }
    }
}
