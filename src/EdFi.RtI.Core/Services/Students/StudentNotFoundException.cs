using EdFi.RtI.Core.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Services.Students
{
    public class StudentNotFoundException : DomainException
    {
        public StudentNotFoundException(string studentUniqueId)
            : base($"Could not find Student with StudentUniqueId \"{studentUniqueId}\"")
        {
            StatusCode = System.Net.HttpStatusCode.NotFound;
        }
    }
}
