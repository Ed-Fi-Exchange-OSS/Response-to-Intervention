using EdFi.RtI.Core.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DomainServiceProvider
{
    public class DomainServiceVersionNotImplementedException : DomainException
    {
        public DomainServiceVersionNotImplementedException(string serviceName, string edFiVersion)
            : base($"{serviceName} not implemented for Ed-Fi version \"${edFiVersion}\"")
        {

        }
    }
}
