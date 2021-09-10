using EdFi.RtI.Api.Filters;
using EdFi.RtI.Core.DTOs.Features;
using EdFi.RtI.Core.Services.Features;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EdFi.RtI.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeaturesController : ControllerBase
    {
        private readonly IFeaturesService _service;

        public FeaturesController(IFeaturesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<FeaturesSettings>> GetFeaturesSettings()
        {
            var features = await _service.GetFeaturesSettings();
            return Ok(features);
        }

        [HttpPut, UserRoleAuthorizationFilter(UserRole.Admin)]
        public async Task UpdateFeaturesSettings([FromBody] FeaturesSettings features)
        {
            await _service.UpdateFeaturesSettings(features);
        }
    }
}
