using EdFi.RtI.Core.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Services.Interventions
{
    public class InterventionNotFoundException : DomainException
    {
        public InterventionNotFoundException(string identificationCode)
            : base($"Could not find Intervention with Identification Code \"{identificationCode}\"")
        {
            StatusCode = System.Net.HttpStatusCode.NotFound;
        }
    }
}
