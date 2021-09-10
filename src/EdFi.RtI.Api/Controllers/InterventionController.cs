using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Api.Filters;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Interventions;
using EdFi.RtI.Core.Services.Interventions;
using Microsoft.AspNetCore.Mvc;

namespace EdFi.RtI.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InterventionController : ControllerBase
    {
        private readonly IDomainServiceProvider _domainServiceProvider;

        public InterventionController(IDomainServiceProvider domainServiceProvider)
        {
            _domainServiceProvider = domainServiceProvider;
        }

        private IInterventionService InterventionService => _domainServiceProvider.GetService<IInterventionService>();

        [HttpPost("search")]
        public async Task<IEnumerable<Intervention>> Search([FromBody] InterventionSearchParams searchParams)
        {
            return await InterventionService.Search(searchParams);
        }

        [HttpGet("{interventionId}")]
        public async Task<InterventionDTO> GetById(string interventionId)
        {
            return await InterventionService.GetById(interventionId);
        }

        [HttpPut("{interventionId}"), UserRoleAuthorizationFilter(UserRole.Admin)]
        public async Task<Intervention> Update(string interventionId, Intervention intervention)
        {
            return await InterventionService.Update(interventionId, intervention);
        }

        [HttpGet("count")]
        public async Task<int> GetInterventionsTotalCount()
        {
            var results = await InterventionService.Search(new InterventionSearchParams());
            return results.Count();
        }

        [HttpPost, UserRoleAuthorizationFilter(UserRole.Admin)]
        public async Task<object> Create(Intervention intervention)
        {
            return await InterventionService.Create(intervention);
        }

        [HttpDelete("deleteInterventionById/{interventionId}"), UserRoleAuthorizationFilter(UserRole.Admin)]
        public async Task<IEnumerable<InterventionDTO>> DeleteInterventionById(string interventionId)
        {
            return await InterventionService.Delete(interventionId);
        }
    }
}
