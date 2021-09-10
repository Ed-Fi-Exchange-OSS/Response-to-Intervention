using EdFi.RtI.Core.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Services.Assessments
{
    public class AssessmentNotFoundException : DomainException
    {
        public AssessmentNotFoundException(string identifier)
            : base($"Could not find Assessment with identifier {identifier}")
        {
            StatusCode = System.Net.HttpStatusCode.NotFound;
        }
    }
}
