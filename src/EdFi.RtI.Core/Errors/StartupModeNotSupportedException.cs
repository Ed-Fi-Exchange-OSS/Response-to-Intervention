using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Errors
{
    public class StartupModeNotSupportedException : DomainException
    {
        public StartupModeNotSupportedException(string attemptedStartupMode)
            : base($"Startup mode \"{attemptedStartupMode}\" is not supported")
        {
            StatusCode = System.Net.HttpStatusCode.InternalServerError;
        }
    }
}
