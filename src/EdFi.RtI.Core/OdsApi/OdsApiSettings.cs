using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.OdsApi
{
    public class OdsApiSettings
    {
        public string Version { get; set; }
        public string AuthUrl { get; set; }
        public string TokenUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AssessmentNamespace { get; set; }
        public string ResourcesUrl { get; set; }
        public string CompositesUrl { get; set; }
    }
}
