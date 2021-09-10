using EdFi.RtI.Core.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DomainServiceProvider
{
    public class DomainServiceNotImplementedException : DomainException
    {
        public DomainServiceNotImplementedException(Type serviceType)
            : base($"Domain Service not implemented for type \"{serviceType}\"")
        {
        }
    }
}
