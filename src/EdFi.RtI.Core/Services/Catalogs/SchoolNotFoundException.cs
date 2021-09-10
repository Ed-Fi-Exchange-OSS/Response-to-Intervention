using EdFi.RtI.Core.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Services.Catalogs
{
    public class SchoolNotFoundException : DomainException
    {
        public SchoolNotFoundException(string schoolId)
            : base($"Could not find School with SchoolId \"{schoolId}\"")
        {
            StatusCode = System.Net.HttpStatusCode.NotFound;
        }
    }
}
