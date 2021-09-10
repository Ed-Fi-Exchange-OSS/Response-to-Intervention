using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class ObjectiveAssessmentReferencev3
    {
        public string AssessmentIdentifier { get; set; }
        public string IdentificationCode { get; set; }
        public string Namespace { get; set; }
        public LinkReferencev3 Link { get; set; }
    }
}
