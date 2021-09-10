using System.Collections.Generic;
using System.Threading.Tasks;
using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.Services.Students;
using Microsoft.AspNetCore.Mvc;

namespace EdFi.RtI.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IDomainServiceProvider _domainServiceProvider;

        public StudentController(IDomainServiceProvider domainServiceProvider)
        {
            _domainServiceProvider = domainServiceProvider;
        }

        private IStudentService StudentService => _domainServiceProvider.GetService<IStudentService>();

        [HttpGet("section/{sectionId}")]
        public async Task<IEnumerable<Student>> GetStudentsBySection(string sectionId)
        {
            return await StudentService.GetStudentsBySectionId(sectionId);
        }
    }
}
