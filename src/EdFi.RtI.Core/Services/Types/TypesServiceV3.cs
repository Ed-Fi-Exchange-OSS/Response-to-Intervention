using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Types;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.OdsApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Types
{
    public class TypesServiceV3 : ITypesService
    {
        private readonly IDomainServiceProvider _domainServiceProvider;
        private readonly IOdsApiClientProvider _odsApiClientProvider;

        public TypesServiceV3(IDomainServiceProvider domainServiceProvider, IOdsApiClientProvider odsApiClientProvider)
        {
            _domainServiceProvider = domainServiceProvider;
            _odsApiClientProvider = odsApiClientProvider;
        }

        private IEdFiMapper _mapper => _domainServiceProvider.GetService<IEdFiMapper>();

        public async Task<object> GetAssessmentItemResults()
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            var descriptors = await ods.Get<IList<object>>("assessmentItemResultDescriptors");
            return _mapper.Map<IList<AssessmentResultDTO>>(descriptors);
        }

        public async Task<object> GetAssessmentReportingMethods()
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            var descriptors = await ods.Get<IList<object>>("assessmentReportingMethodDescriptors");
            return _mapper.Map<IList<AssessmentReportingMethodDTO>>(descriptors);
        }

        public async Task<object> GetInterventionClasses()
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();
            var descriptors = await odsApi.Get<IList<object>>("interventionClassDescriptors");
            return _mapper.Map<IList<InterventionClassDTO>>(descriptors);
        }

        public async Task<object> GetDeliveryMethods()
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();
            var descriptors = await odsApi.Get<IList<object>>("deliveryMethodDescriptors");
            return _mapper.Map<IList<DeliveryMethodDTO>>(descriptors);
        }
    }
}
