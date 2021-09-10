using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace EdFi.RtI.Core.Errors
{
    public class DomainException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public DomainException()
        {
            StatusCode = System.Net.HttpStatusCode.InternalServerError;
        }

        public DomainException(string message)
            : base(message)
        {
            StatusCode = System.Net.HttpStatusCode.InternalServerError;
        }

        public DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = System.Net.HttpStatusCode.InternalServerError;
        }
    }
}
