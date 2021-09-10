using EdFi.RtI.Core.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Services.Features
{
    public class FeaturesNotFoundException : DomainException
    {
        public FeaturesNotFoundException(string tenantId)
            : base($"Features for Tenant {tenantId} have not been stored")
        {
            StatusCode = System.Net.HttpStatusCode.NotFound;
        }
    }
}
