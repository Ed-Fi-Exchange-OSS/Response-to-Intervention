using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.Models;
using EdFi.RtI.Core.OdsApi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Descriptors
{
    public class DescriptorServiceV3 : IDescriptorService
    {
        private readonly IOdsApiClientProvider _odsApiClientProvider;

        public DescriptorServiceV3(IOdsApiClientProvider odsApiClientProvider)
        {
            _odsApiClientProvider = odsApiClientProvider;
        }

        public async Task<IEnumerable<AssessmentCategoryDescriptor>> GetAssessmentCategoryDescriptors()
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();
            var descriptors = await odsApi.Get<IList<AssessmentCategoryDescriptorv3>>("assessmentCategoryDescriptors");
            return descriptors
                .Select(a => a.MapToAssessmentCategoryDescriptor())
                .OrderBy(a => a.AssessmentCategoryType)
                .ToList();
        }

        public async Task<IList<AssessmentPeriodDescriptor>> GetAssessmentPeriodDescriptors()
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();
            var descriptors = await odsApi.Get<IList<AssessmentPeriodDescriptorv3>>("assessmentPeriodDescriptors");
            return descriptors
                .Select(a => a.MapToAssessmentPeriodDescriptor())
                .OrderBy(a => a.Description)
                .ToList();
        }

        public async Task<IList<object>> GetStaffClassificationDescriptors()
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();
            var descriptors = await odsApi.Get<IList<StaffClassificationDescriptorv3>>("staffClassificationDescriptors");
            return descriptors
                .OrderBy(x => x.Description)
                .Select(x => x as object)
                .ToList();
        }
    }
}
