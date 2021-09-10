using System.Collections.Generic;
using System.Threading.Tasks;
using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Api.Filters;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.RequestBodies;
using EdFi.RtI.Core.Services.ScoringAssessments;
using Microsoft.AspNetCore.Mvc;

namespace EdFi.RtI.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ScoringAssessmentsController : ControllerBase
    {
        private readonly IDomainServiceProvider _domainServiceProvider;

        public ScoringAssessmentsController(IDomainServiceProvider domainServiceProvider)
        {
            _domainServiceProvider = domainServiceProvider;
        }

        private IScoringAssessmentsService ScoringAssessmentsService => _domainServiceProvider.GetService<IScoringAssessmentsService>();

        [HttpPost, UserRoleAuthorizationFilter(UserRole.Admin)]
        public async Task<ScoringAssessmentDTO> CreateScoring([FromBody] ScoringAssessmentDTO scoring)
        {
            return await ScoringAssessmentsService.CreateScoring(scoring);
        }

        [HttpPost("association"), UserRoleAuthorizationFilter(UserRole.Admin)]
        public async Task<StudentAssessment> CreateAssociation([FromBody] ScoringAssessmentPostBody body)
        {
            return await ScoringAssessmentsService.CreateAssociation(body);
        }

        [HttpDelete, UserRoleAuthorizationFilter(UserRole.Admin)]
        public async Task DeleteStudentAssessment([FromQuery] string studentAssessmentId, [FromQuery] string uniqueSectionCode, [FromQuery] string identifier)
        {
            await ScoringAssessmentsService.DeleteStudentAssessment(studentAssessmentId, uniqueSectionCode, identifier);
        }

        [HttpDelete("deleteAll"), UserRoleAuthorizationFilter(UserRole.Admin)]
        public async Task DeleteStudentAssessmentAll([FromQuery] string uniqueSectionCode, [FromQuery] string assessmentId, [FromQuery] string date)
        {
            await ScoringAssessmentsService.DeleteStudentAssessmentAll(uniqueSectionCode, assessmentId, date);
        }

        [HttpPost("search")]
        public async Task<IEnumerable<ScoringAssessmentDTO>> SearchScoringAssessments([FromBody] ScoringAssessmentSearchParams searchParams)
        {
            return await ScoringAssessmentsService.Search(searchParams);
        }

        [HttpPut, UserRoleAuthorizationFilter(UserRole.Admin)]
        public async Task UpdateScoring(IEnumerable<ScoringAssessmentDTO> scorings)
        {
            await ScoringAssessmentsService.UpdateScorings(scorings);
        }
    }
}
