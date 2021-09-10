using EdFi.RtI.Core.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Services.Settings
{
    public class UnsupportedEdFiVersionException : DomainException
    {
        public UnsupportedEdFiVersionException(string edFiVersion)
            : base($"Ed-Fi version \"{edFiVersion}\" is currently unsupported")
        {
            StatusCode = System.Net.HttpStatusCode.BadRequest;
        }
    }
}
