using EdFi.RtI.Core.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Providers.Staffs
{
    public class StaffNotFoundException : DomainException
    {
        public StaffNotFoundException(string email)
            : base($"Could not find Staff with email \"{email}\"")
        {
            StatusCode = System.Net.HttpStatusCode.NotFound;
        }
    }
}
