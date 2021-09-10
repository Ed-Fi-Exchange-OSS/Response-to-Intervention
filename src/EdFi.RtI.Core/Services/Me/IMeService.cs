using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.DTOs.StaffEducationOrganizationAssignmentAssociations;
using EdFi.RtI.Core.DTOs.Staffs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Me
{
    public interface IMeService : IDomainService
    {
        Task<UserSessionProfile> GetMyUserProfile();
        Task<IEnumerable<UserSessionProfile>> GetUserAllAdmins(bool getFromCache = true, bool storeInCache = true);
        Task<IEnumerable<UserSessionProfile>> GetUserAllTeachers(bool getFromCache = true, bool storeInCache = true);
        Task<UserSessionProfile> GetUserByEmail(string email);
        Task<UserSessionProfile> GetUserByStaffId(string staffId);
        Task<IEnumerable<UserSessionProfile>> SearchUsers(UserSessionProfileSearchParams searchParams);
    }
}
