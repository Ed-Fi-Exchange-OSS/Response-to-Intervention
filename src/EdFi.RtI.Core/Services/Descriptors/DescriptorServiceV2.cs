using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.OdsApi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Descriptors
{
    public class DescriptorServiceV2 : IDescriptorService
    {
        private readonly IOdsApiClientProvider _odsApiClientProvider;

        public DescriptorServiceV2(IOdsApiClientProvider odsApiClientProvider)
        {
            _odsApiClientProvider = odsApiClientProvider;
        }

        public async Task<IEnumerable<AssessmentCategoryDescriptor>> GetAssessmentCategoryDescriptors()
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            var descriptors = await ods.Get<IEnumerable<AssessmentCategoryDescriptor>>("assessmentCategoryDescriptors");
            return descriptors
                .OrderBy(category => category.Description)
                .ToList();
        }

        public async Task<IList<AssessmentPeriodDescriptor>> GetAssessmentPeriodDescriptors()
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            var descriptors = await ods.Get<IList<AssessmentPeriodDescriptor>>("assessmentPeriodDescriptors");
            return descriptors
                .OrderBy(a => a.Description)
                .ToList();
        }

        public async Task<IList<object>> GetStaffClassificationDescriptors()
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();

            var descriptors = await ods.Get<IList<StaffClassificationDescriptor>>("staffClassificationDescriptors");

            return descriptors
                .OrderBy(x => x.Description)
                .Select(x => x as object)
                .ToList();
        }
    }
}
