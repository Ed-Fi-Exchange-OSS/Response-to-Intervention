using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Errors
{
    public class CacheProviderConnectionException : DomainException
    {
        public CacheProviderConnectionException(string connectionString)
            : base($"Could not establish Redis connection with connection string \"{connectionString}\"")
        { }

        public CacheProviderConnectionException(string connectionString, Exception ex)
            : base($"Could not establish Redis connection with connection string \"{connectionString}\"", ex)
        { }
    }
}
