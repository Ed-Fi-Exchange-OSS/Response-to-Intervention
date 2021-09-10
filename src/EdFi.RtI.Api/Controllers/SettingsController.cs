using EdFi.RtI.Api.Filters;
using EdFi.RtI.Core.OdsApi;
using EdFi.RtI.Core.Services.Settings;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdFi.RtI.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet("edFiVersion")]
        public async Task<string> GetDefaultEdFiVersion() =>
            await _settingsService.GetDefaultEdFiVersion();

        [HttpGet("odsApiSettings/{version}")]
        public async Task<OdsApiSettings> GetOdsApiSettings(string version) =>
            await _settingsService.GetOdsApiSettings(version);

        [HttpGet("userRoleAdminMappings")]
        public async Task<IList<string>> GetUserRoleAdminMappings() =>
            await _settingsService.GetUserRoleAdminMappings();

        [HttpGet("userRoleTeacherMappings")]
        public async Task<IList<string>> GetUserRoleTeacherMappings() =>
            await _settingsService.GetUserRoleTeacherMappings();

        [HttpDelete("odsApiSettings"), UserRoleAuthorizationFilter(UserRole.Admin)]
        public async Task RemoveOdsApiSettings() =>
            await _settingsService.RemoveOdsApiSettings();

        [HttpPost("odsApiSettings")]
        public async Task SaveOdsApiSettings([FromBody] OdsApiSettings odsApiSettings) =>
            await _settingsService.SaveOdsApiSettings(odsApiSettings);

        [HttpPost("edFiVersion")]
        public async Task SetDefaultEdFiVersion([FromBody] SetDefaultEdFiVersionRequest request) =>
            await _settingsService.SetDefaultEdFiVersion(request.EdFiVersion);

        [HttpPost("userRoleAdminMappings")]
        public async Task SetUserRoleAdminMappings([FromBody] IEnumerable<string> staffClassificationDescriptors) =>
            await _settingsService.SetUserRoleAdminMappings(staffClassificationDescriptors);

        [HttpPost("userRoleTeacherMappings")]
        public async Task SetUserRoleTeacherMappings([FromBody] IEnumerable<string> staffClassificationDescriptors) =>
            await _settingsService.SetUserRoleTeacherMappings(staffClassificationDescriptors);

        [HttpPost("odsApiSettings/test")]
        public async Task TestOdsApiSettings([FromBody] OdsApiSettings odsApiSettings) =>
            await _settingsService.TestOdsApiSettings(odsApiSettings);
    }

    public class SetDefaultEdFiVersionRequest
    {
        public string EdFiVersion { get; set; }
    }

    public class SetUserEmailMappingsRequest
    {
        public string Email { get; set; }
        public IList<string> MappedEmails { get; set; }
    }
}
