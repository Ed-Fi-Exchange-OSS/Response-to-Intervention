using System.Collections.Generic;
using System.Threading.Tasks;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.Providers.Staffs;
using EdFi.RtI.Core.Services;
using EdFi.RtI.Core.Services.Me;
using Microsoft.AspNetCore.Mvc;

namespace EdFi.RtI.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MeController : ControllerBase
    {
        private readonly IDomainServiceProvider _domainServiceProvider;
        private readonly ISessionContext _sessionContext;

        public MeController(IDomainServiceProvider domainServiceProvider, ISessionContext sessionContext)
        {
            _domainServiceProvider = domainServiceProvider;
            _sessionContext = sessionContext;
        }

        private IMeService MeService => _domainServiceProvider.GetService<IMeService>();
        private IStaffsProvider StaffsProvider => _domainServiceProvider.GetService<IStaffsProvider>();

        [HttpGet("session-profile")]
        public async Task<IEnumerable<UserSessionProfile>> GetUserAll()
        {
            return await MeService.SearchUsers(new UserSessionProfileSearchParams
            {
                GetFromCache = true,
                StoreInCache = true,
            });
        }

        [HttpGet("session-admins")]
        public async Task<IEnumerable<UserSessionProfile>> GetUserAllAdmins()
        {
            return await MeService.GetUserAllAdmins();
        }

        [HttpGet("session-teachers")]
        public async Task<IEnumerable<UserSessionProfile>> GetUserAllTeachers()
        {
            return await MeService.GetUserAllTeachers();
        }

        [HttpGet("profile")]
        public async Task<UserSessionProfile> GetMyUserProfile() =>
            await MeService.GetMyUserProfile();
        
        [HttpGet("session-profile/email/{email}")]
        public async Task<UserSessionProfile> GetUserByEmail(string email)
        {
            return await MeService.GetUserByEmail(email);
        }

        [HttpGet("session-profile/{staffId}")]
        public async Task<UserSessionProfile> GetUserByStaffId(string staffId)
        {
            return await MeService.GetUserByStaffId(staffId);
        }

        [HttpPost("search")]
        public async Task<IEnumerable<UserSessionProfile>> SearchUsers([FromBody] UserSessionProfileSearchParams searchParams)
        {
            return await MeService.SearchUsers(searchParams);
        }
    }
}
