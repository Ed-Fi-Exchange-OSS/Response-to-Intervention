using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.Providers.Staffs;
using EdFi.RtI.Core.Services;
using EdFi.RtI.Core.UserRoleMappings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Api.Filters
{
    public class UserRoleAuthorizationFilter : IActionFilter
    {
        private readonly IDomainServiceProvider _domainServiceProvider;
        private readonly ISessionContext _sessionContext;
        private readonly IUserRoleMappingsFacade _userRoleMappingsFacade;
        private readonly UserRole[] _userRoles;

        public UserRoleAuthorizationFilter(IDomainServiceProvider domainServiceProvider, ISessionContext sessionContext, IUserRoleMappingsFacade userRoleMappingsFacade, UserRole[] userRoles)
        {
            _domainServiceProvider = domainServiceProvider;
            _sessionContext = sessionContext;
            _userRoleMappingsFacade = userRoleMappingsFacade;
            _userRoles = userRoles;
        }

        private IStaffsProvider _staffsProvider => _domainServiceProvider.GetService<IStaffsProvider>();

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var userStaffClassificationDescriptor = GetCurrentUserStaffClassificationDescriptor().Result;
            var adminStaffClassificationDescriptors = GetAdminMappings().Result;
            var teacherStaffClassificationDescriptors = GetTeacherMappings().Result;

            foreach (var role in _userRoles)
            {
                if (role == UserRole.Admin)
                {
                    var isUserAdmin = adminStaffClassificationDescriptors.Any(descriptor => userStaffClassificationDescriptor.Contains(descriptor));

                    if (!isUserAdmin)
                        throw new ForbiddenUserRoleException(role, adminStaffClassificationDescriptors, context.HttpContext.Request.Path);
                }
                else if (role == UserRole.Teacher)
                {
                    var isUserTeacher = teacherStaffClassificationDescriptors.Any(descriptor => userStaffClassificationDescriptor.Contains(descriptor));

                    if (!isUserTeacher)
                        throw new ForbiddenUserRoleException(role, teacherStaffClassificationDescriptors, context.HttpContext.Request.Path);
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        private async Task<HashSet<string>> GetAdminMappings()
        {
            var mappings = await _userRoleMappingsFacade.GetUserRoleAdminMappings();
            return mappings.ToHashSet();
        }

        private async Task<HashSet<string>> GetTeacherMappings()
        {
            var mappings = await _userRoleMappingsFacade.GetUserRoleTeacherMappings();
            return mappings.ToHashSet();
        }

        private async Task<HashSet<string>> GetCurrentUserStaffClassificationDescriptor()
        {
            var descriptors = await _staffsProvider.GetStaffClassificationDescriptorsByEmail(_sessionContext.Email);
            return descriptors.ToHashSet();
        }
    }

    public class UserRoleAuthorizationFilterAttribute : TypeFilterAttribute
    {
        public UserRoleAuthorizationFilterAttribute(params UserRole[] roles) : base(typeof(UserRoleAuthorizationFilter))
        {
            Arguments = new object[] { roles };
        }
    }

    public enum UserRole
    {
        Admin,
        Teacher,
    }
}
