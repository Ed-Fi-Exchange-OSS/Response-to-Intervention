using EdFi.RtI.Core.Errors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EdFi.RtI.Api.Utils
{
    public class ErrorUtils
    {
        public static StatusCodeResult InternalServerError()
        {
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }

        public static ObjectResult InternalServerError(Exception exception)
        {
            var details = new ExceptionDetails(exception);
            return InternalServerError(details);
        }

        public static ObjectResult InternalServerError(object value)
        {
            var result = new ObjectResult(value);
            result.StatusCode = (int)HttpStatusCode.InternalServerError;
            return result;
        }
    }
}
