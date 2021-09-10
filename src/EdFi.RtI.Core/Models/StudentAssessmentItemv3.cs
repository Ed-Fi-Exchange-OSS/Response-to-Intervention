using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class StudentAssessmentItemv3
    {
        public string AssessmentItemResultDescriptor { get; set; }
        public string ResponseIndicatorDescriptor { get; set; }
        public string AssessmentResponse { get; set; }
        public string DescriptiveFeedback { get; set; }
        public double RawScoreResult { get; set; }
        public string TimeAssessed { get; set; }
        public AssessmentItemReferencev3 AssessmentItemReference { get; set; }
    }
}
