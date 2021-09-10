using System.Threading.Tasks;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.Services.Types;
using Microsoft.AspNetCore.Mvc;

namespace EdFi.RtI.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        private readonly IDomainServiceProvider _domainServiceProvider;

        public TypesController(IDomainServiceProvider domainServiceProvider)
        {
            _domainServiceProvider = domainServiceProvider;
        }

        private ITypesService TypesService => _domainServiceProvider.GetService<ITypesService>();

        [HttpGet("assessmentResult")]
        public async Task<object> GetAssessmentItemResultTypes()
        {
            return await TypesService.GetAssessmentItemResults();
        }

        [HttpGet("assessmentReportingMethod")]
        public async Task<object> GetAssessmentReportingMethodTypes()
        {
            return await TypesService.GetAssessmentReportingMethods();
         }

        [HttpGet("deliveryMethod")]
        public async Task<object> GetDeliveryMethodTypes()
        {
            return await TypesService.GetDeliveryMethods();
         }

        [HttpGet("interventionClass")]
        public async Task<object> GetInterventionClassTypes()
        {
            return await TypesService.GetInterventionClasses();
        }
    }
}
