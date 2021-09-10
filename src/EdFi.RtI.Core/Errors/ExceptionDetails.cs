using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Errors
{
    public class ExceptionDetails
    {
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string StringRepresentation { get; set; }
        public string Type { get; set; }

        public ExceptionDetails(Exception exception)
        {
            Message = exception.Message;
            Source = exception.Source;
            StackTrace = exception.StackTrace;
            StringRepresentation = exception.ToString();
            Type = exception.GetType().Name;
        }
    }
}
