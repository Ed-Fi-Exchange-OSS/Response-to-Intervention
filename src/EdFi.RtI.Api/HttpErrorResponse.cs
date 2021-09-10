using EdFi.RtI.Core.Errors;
using Newtonsoft.Json;
using System;
using System.Net;

namespace EdFi.RtI.Api
{
    public class HttpErrorResponse
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public int StatusCode { get; set; }
        public Exception InnerException { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static HttpErrorResponse FromException(Exception ex)
        {
            var statusCode = HttpStatusCode.InternalServerError;

            if (ex is DomainException)
                statusCode = (ex as DomainException).StatusCode;

            return new HttpErrorResponse
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                StatusCode = (int)statusCode,
                InnerException = ex.InnerException,
            };
        }
    }
}
