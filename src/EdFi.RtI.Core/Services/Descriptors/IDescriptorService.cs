using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Descriptors
{
    public interface IDescriptorService : IDomainService
    {
        Task<IEnumerable<AssessmentCategoryDescriptor>> GetAssessmentCategoryDescriptors();
        Task<IList<AssessmentPeriodDescriptor>> GetAssessmentPeriodDescriptors();
        Task<IList<object>> GetStaffClassificationDescriptors();
    }
}
