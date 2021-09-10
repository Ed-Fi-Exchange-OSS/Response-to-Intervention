using EdFi.RtI.Core.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Providers.Cache
{
    public class KeyNotFoundException : DomainException
    {
        public KeyNotFoundException(string key)
            : base($"Could not find cached value with key \"{key}\"")
        {
            StatusCode = System.Net.HttpStatusCode.BadRequest;
        }
    }
}
