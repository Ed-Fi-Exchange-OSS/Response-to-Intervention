using System.Collections.Generic;
using System.Threading.Tasks;
using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.Services.Catalogs;
using Microsoft.AspNetCore.Mvc;

namespace EdFi.RtI.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IDomainServiceProvider _domainServiceProvider;

        public CatalogController(IDomainServiceProvider domainServiceProvider)
        {
            _domainServiceProvider = domainServiceProvider;
        }

        private ICatalogService CatalogService
        {
            get {
                return _domainServiceProvider.GetService<ICatalogService>();
            }
        }

        [HttpGet("sections")]
        public async Task<IEnumerable<Section>> GetSectionsAll()
        {
            return await CatalogService.GetSectionsAll();
        }

        [HttpGet("sections/{staffUniqueId}")]
        public async Task<IEnumerable<Section>> GetSectionsByStaff(string staffUniqueId)
        {
            return await CatalogService.GetSectionsByStaff(staffUniqueId);
        }

        [HttpGet("schools")]
        public async Task<IEnumerable<School>> GetSchoolsAll()
        {
            return await CatalogService.GetSchoolsAll();
        }

        [HttpGet("schools/staff/{staffUniqueId}")]
        public async Task<IEnumerable<School>> GetSchoolsByStaff(int staffUniqueId)
        {
            return await CatalogService.GetSchoolsByStaff(staffUniqueId);
        }

        [HttpGet("staffs/school/{schoolId}")]
        public async Task<IEnumerable<Staff>> GetStaffsBySchool(string schoolId)
        {
            return await CatalogService.GetStaffsBySchool(schoolId);
        }
    }
}
