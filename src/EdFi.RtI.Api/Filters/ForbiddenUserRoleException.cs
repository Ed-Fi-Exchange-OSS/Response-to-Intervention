using EdFi.RtI.Core.Errors;
using System.Collections;
using System.Collections.Generic;

namespace EdFi.RtI.Api.Filters
{
    public class ForbiddenUserRoleException : DomainException
    {
        public ForbiddenUserRoleException(UserRole role, IEnumerable<string> staffClassificationDescriptors, string endpoint)
            : base($"User does not have required role of {role} ({string.Join(", ", staffClassificationDescriptors)}) to access the endpoint {endpoint}")
        {
            StatusCode = System.Net.HttpStatusCode.Forbidden;
        }
    }
}
