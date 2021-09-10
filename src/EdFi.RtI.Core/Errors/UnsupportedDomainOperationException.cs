using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Errors
{
    public class UnsupportedDomainOperationException : DomainException
    {
        public UnsupportedDomainOperationException(string methodName, string startupMode)
            : base($"{methodName} method is not supported for StartupMode \"{startupMode}\"")
        {
            StatusCode = System.Net.HttpStatusCode.BadRequest;
        }
    }
}
