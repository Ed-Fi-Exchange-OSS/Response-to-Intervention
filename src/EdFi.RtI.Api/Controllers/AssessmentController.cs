using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Api.Filters;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.Services.Assessments;
using Microsoft.AspNetCore.Mvc;

namespace EdFi.RtI.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AssessmentController : ControllerBase
    {
        private readonly IDomainServiceProvider _domainServiceProvider;

        public AssessmentController(IDomainServiceProvider domainServiceProvider)
        {
            _domainServiceProvider = domainServiceProvider;
        }

        private IAssessmentService AssessmentService => _domainServiceProvider.GetService<IAssessmentService>();

        [HttpPost("search")]
        public async Task<IEnumerable<Assessment>> Search([FromBody] AssessmentSearchParams searchParams)
        {
            return await AssessmentService.Search(searchParams);
        }

        [HttpPost, UserRoleAuthorizationFilter(UserRole.Admin)]
        public async Task<Assessment> Create([FromBody] Assessment assessment)
        {
            return await AssessmentService.Create(assessment);
        }

        [HttpGet("{assessmentId}")]
        public async Task<ActionResult<object>> GetById(string assessmentId)
        {
            var result = await AssessmentService.GetById(assessmentId);
            return Ok(result);
        }

        [HttpPost("count")]
        public async Task<int> GetTotalCount([FromBody] AssessmentSearchParams searchParams)
        {
            searchParams.StoreInCache = false;
            searchParams.PageIndex = null;
            searchParams.PageSize = null;
            var result = await AssessmentService.Search(searchParams);
            return result.Count();
        }

        [HttpPut("{assessmentId}"), UserRoleAuthorizationFilter(UserRole.Admin)]
        public async Task<Assessment> Update(string assessmentId, Assessment assessment)
        {
            return await AssessmentService.Update(assessmentId, assessment);
        }

        [HttpGet("assessmentFamily")]
        public async Task<object> GetAsessmentFamilies()
        {
            return await AssessmentService.GetAssessmentFamilies();
        }

        [HttpDelete("deleteAssessmentById/{assessmentId}"), UserRoleAuthorizationFilter(UserRole.Admin)]
        public async Task<IEnumerable<Assessment>> Delete(string assessmentId)
        {
            return await AssessmentService.Delete(assessmentId);
        }
    }
}
