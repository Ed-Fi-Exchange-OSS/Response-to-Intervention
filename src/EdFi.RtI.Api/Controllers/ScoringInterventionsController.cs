using System.Collections.Generic;
using System.Threading.Tasks;
using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Api.Filters;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.RequestBodies;
using EdFi.RtI.Core.Services.ScoringInterventions;
using Microsoft.AspNetCore.Mvc;

namespace EdFi.RtI.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ScoringInterventionsController : ControllerBase
    {
        private readonly IDomainServiceProvider _domainServiceProvider;

        public ScoringInterventionsController(IDomainServiceProvider domainServiceProvider)
        {
            _domainServiceProvider = domainServiceProvider;
        }

        private IScoringInterventionsService ScoringInterventionsService => _domainServiceProvider.GetService<IScoringInterventionsService>();

        [HttpPost, UserRoleAuthorizationFilter(UserRole.Admin)]
        public async Task<StudentInterventionAssociation> CreateAssociation([FromBody] ScoringInterventionPostBody body)
        {
            return await ScoringInterventionsService.CreateAssociation(body);
        }

        [HttpPut("delete"), UserRoleAuthorizationFilter(UserRole.Admin)]
        public async Task DeleteAssociation([FromBody] ScoringInterventionDeleteBody body)
        {
            await ScoringInterventionsService.DeleteAssociation(body);
        }

        [HttpPost("scorings")]
        public async Task<IEnumerable<StudentInterventionsDTO>> GetScorings([FromBody] ScoringInterventionSearchFilters filters, [FromQuery] bool getFromCache = true, [FromQuery] bool storeInCache = true)
        {
            return await ScoringInterventionsService.GetStudentInterventionsDTO(filters, getFromCache, storeInCache);
        }

        [HttpGet("scorings/{studentUniqueId}")]
        public async Task<StudentInterventionsDTO> GetScoringsByStudent(string studentUniqueId)
        {
            return await ScoringInterventionsService.GetStudentInterventionsDTOByStudent(studentUniqueId);
        }
    }
}
