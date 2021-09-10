using System.Collections.Generic;
using System.Threading.Tasks;
using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.Services.Descriptors;
using Microsoft.AspNetCore.Mvc;

namespace EdFi.RtI.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DescriptorController : ControllerBase
    {
        private readonly IDomainServiceProvider _domainServiceProvider;

        public DescriptorController(IDomainServiceProvider domainServiceProvider)
        {
            _domainServiceProvider = domainServiceProvider;
        }

        private IDescriptorService DescriptorService
        {
            get {
                return _domainServiceProvider.GetService<IDescriptorService>();
            }
        }

        [HttpGet("assessmentCategory")]
        public async Task<IEnumerable<AssessmentCategoryDescriptor>> GetAssessmentCategoryDescriptors()
        {
            return await DescriptorService.GetAssessmentCategoryDescriptors();
        }

        [HttpGet("assessmentPeriod")]
        public async Task<IList<AssessmentPeriodDescriptor>> GetAssessmentPeriodDescriptors()
        {
            return await DescriptorService.GetAssessmentPeriodDescriptors();
        }

        [HttpGet("staffClassificationDescriptors")]
        public async Task<IList<object>> GetStaffDescriptors()
        {
            return await DescriptorService.GetStaffClassificationDescriptors();
        }
    }
}
